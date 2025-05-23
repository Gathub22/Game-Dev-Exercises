﻿using UnityEngine;
using Fusion;
using Fusion.Addons.SimpleKCC;

namespace Starter.Platformer
{
	/// <summary>
	/// Main player scrip - controls player movement and animations.
	/// </summary>
	public sealed class Player : NetworkBehaviour
	{
		[Header("References")]
		public SimpleKCC KCC;
		public PlayerInput Input;
		public Animator Animator;
		public Transform CameraPivot;
		public Transform CameraHandle;
		public Transform ScalingRoot;
		public UINameplate Nameplate;

		[Header("Movement Setup")]
		public float WalkSpeed = 2f;
		public float SprintSpeed = 5f;
		public float JumpImpulse = 10f;
		public float UpGravity = 25f;
		public float DownGravity = 40f;
		public float RotationSpeed = 8f;

		[Header("Movement Accelerations")]
		public float GroundAcceleration = 55f;
		public float GroundDeceleration = 25f;
		public float AirAcceleration = 25f;
		public float AirDeceleration = 1.3f;

		[Header("Sounds")]
		public AudioSource FootstepSound;
		public AudioClip JumpAudioClip;
		public AudioClip LandAudioClip;
		public AudioClip CoinCollectedAudioClip;

		[Header("VFX")]
		public ParticleSystem DustParticles;

		[Networked, HideInInspector, Capacity(24), OnChangedRender(nameof(OnNicknameChanged))]
		public string Nickname { get; private set; }
		[Networked, HideInInspector]
		public NetworkBool IsFinished { get; set; }
		[Networked, HideInInspector, OnChangedRender(nameof(OnCollectedCoinsChanged))]
		public int CollectedCoins { get; set; }

		[Networked]
		private Vector3 _moveVelocity { get; set; }
		[Networked, OnChangedRender(nameof(OnJumpingChanged))]
		private NetworkBool _isJumping { get; set; }
		[Networked]
		private NetworkButtons _previousButtons { get; set; }

		// Animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;

		public void Respawn(Vector3 position, bool resetCoins)
		{
			IsFinished = false;

			KCC.SetPosition(position);
			KCC.SetLookRotation(0f, 0f);

			_moveVelocity = Vector3.zero;

			if (resetCoins)
			{
				CollectedCoins = 0;
			}
		}

		public override void Spawned()
		{
			if (HasInputAuthority)
			{
				// Sending player nickname that is saved in UIGameMenu
				RPC_SetNickname(PlayerPrefs.GetString("PlayerName"));
			}

			// In case the nickname is already changed,
			// we need to trigger the change manually
			OnNicknameChanged();
		}

		public override void FixedUpdateNetwork()
		{
			var input = IsFinished == false ? GetInput<GameplayInput>().GetValueOrDefault() : default;
			ProcessInput(input, _previousButtons);

			if (KCC.IsGrounded)
			{
				// Stop jumping
				_isJumping = false;
			}

			// Save current button input as previous.
			// Previous buttons need to be networked to detect correctly pressed/released events.
			_previousButtons = input.Buttons;
		}

		public override void Render()
		{
			Animator.SetFloat(_animIDSpeed, KCC.RealSpeed);
			Animator.SetBool(_animIDGrounded, KCC.IsGrounded);

			FootstepSound.enabled = KCC.IsGrounded && KCC.RealSpeed > 1f;
			FootstepSound.pitch = KCC.RealSpeed > SprintSpeed - 1 ? 1.5f : 1f;

			ScalingRoot.localScale = Vector3.Lerp(ScalingRoot.localScale, Vector3.one, Time.deltaTime * 8f);

			var emission = DustParticles.emission;
			emission.enabled = KCC.IsGrounded && KCC.RealSpeed > 1f;
		}

		private void Awake()
		{
			AssignAnimationIDs();
		}

		private void LateUpdate()
		{
			if (IsFinished)
				return;

			// Only InputAuthority needs to update camera
			if (HasInputAuthority == false)
				return;

			// Update camera pivot and transfer properties from camera handle to Main Camera.
			CameraPivot.rotation = Quaternion.Euler(Input.LookRotation);
			Camera.main.transform.SetPositionAndRotation(CameraHandle.position, CameraHandle.rotation);
		}

		private void ProcessInput(GameplayInput input, NetworkButtons previousButtons)
		{
			float jumpImpulse = 0f;

			// Comparing current input buttons to previous input buttons - this prevents glitches when input is lost
			if (KCC.IsGrounded && input.Buttons.WasPressed(previousButtons, EInputButton.Jump))
			{
				// Set world space jump vector
				jumpImpulse = JumpImpulse;
				_isJumping = true;
			}

			// It feels better when the player falls quicker
			KCC.SetGravity(KCC.RealVelocity.y >= 0f ? UpGravity : DownGravity);

			float speed = input.Buttons.IsSet(EInputButton.Sprint) ? SprintSpeed : WalkSpeed;

			var lookRotation = Quaternion.Euler(0f, input.LookRotation.y, 0f);
			// Calculate correct move direction from input (rotated based on camera look)
			var moveDirection = lookRotation * new Vector3(input.MoveDirection.x, 0f, input.MoveDirection.y);
			var desiredMoveVelocity = moveDirection * speed;

			float acceleration;
			if (desiredMoveVelocity == Vector3.zero)
			{
				// No desired move velocity - we are stopping
				acceleration = KCC.IsGrounded ? GroundDeceleration : AirDeceleration;
			}
			else
			{
				// Rotate the character towards move direction over time
				var currentRotation = KCC.TransformRotation;
				var targetRotation = Quaternion.LookRotation(moveDirection);
				var nextRotation = Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Runner.DeltaTime);

				KCC.SetLookRotation(nextRotation.eulerAngles);

				acceleration = KCC.IsGrounded ? GroundAcceleration : AirAcceleration;
			}

			_moveVelocity = Vector3.Lerp(_moveVelocity, desiredMoveVelocity, acceleration * Runner.DeltaTime);

			KCC.Move(_moveVelocity, jumpImpulse);
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
		}

		private void OnTriggerEnter(Collider other)
		{
			// Coins are collected only on state authority
			// - on clients the OnTriggerEnter/Exit calls
			// are not reliable due to resimulations
			if (HasStateAuthority == false)
				return;

			var coin = other.GetComponent<Coin>();
			if (coin != null)
			{
				coin.Collect();
				CollectedCoins++;
			}
		}

		private void OnJumpingChanged()
		{
			if (_isJumping)
			{
				AudioSource.PlayClipAtPoint(JumpAudioClip, KCC.Position, 1f);
				ScalingRoot.localScale = new Vector3(0.5f, 1.5f, 0.5f);
			}
			else
			{
				AudioSource.PlayClipAtPoint(LandAudioClip, KCC.Position, 1f);
				ScalingRoot.localScale = new Vector3(1.25f, 0.75f, 1.25f);
			}
		}

		private void OnCollectedCoinsChanged()
		{
			if (CollectedCoins <= 0)
				return; // Just coins reset

			AudioSource.PlayClipAtPoint(CoinCollectedAudioClip, KCC.Position, 1f);
		}

		private void OnNicknameChanged()
		{
			if (HasInputAuthority)
				return; // Do not show nickname for local player

			Nameplate.SetNickname(Nickname);
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		private void RPC_SetNickname(string nickname)
		{
			Nickname = nickname;
		}
	}
}
