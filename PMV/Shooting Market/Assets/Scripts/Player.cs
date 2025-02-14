using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

	public bool IsSneaking;
	public GameObject Hand;
	public Weapon Gun;
	public Weapon NearbyGun;

	public int Health {
		set {
			_health = value;
			GameObject.Find("Health").GetComponent<TMP_Text>().text = _health.ToString();
		}

		get {
			return _health;
		}
	}
	public float walkingSpeed = 7.5f;
	public float runningSpeed = 11.5f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	public Camera playerCamera;
	public float lookSpeed = 2.0f;
	public float lookXLimit = 45.0f;

	CharacterController characterController;
	Vector3 moveDirection = Vector3.zero;
	float rotationX = 0;

	[HideInInspector]
	public bool canMove = true;

	private int _health;

	void Start() {
		Health = 100;
		characterController = GetComponent<CharacterController>();

		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {
		// We are grounded, so recalculate move direction based on axes
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);
		// Press Left Shift to run
		bool isRunning = Input.GetKey(KeyCode.LeftShift);
		float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
		float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
		float movementDirectionY = moveDirection.y;
		moveDirection = (forward * curSpeedX) + (right * curSpeedY);

		moveDirection.y = movementDirectionY;

		// Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
		// when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
		// as an acceleration (ms^-2)
		if (!characterController.isGrounded) {
			moveDirection.y -= gravity * Time.deltaTime;
		}

		// Move the controller
		characterController.Move(moveDirection * Time.deltaTime);

		// Player and Camera rotation
		if (canMove) {
			rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
			rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
			playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
			transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			if (NearbyGun != null && Gun == null) {
				Gun = NearbyGun;

				Gun.Rb.isKinematic = true;
				Gun.transform.rotation = Hand.transform.rotation;
				Gun.transform.position = Hand.transform.position;
				Gun.transform.parent = Hand.transform;
				Gun.transform.forward = Hand.transform.forward;
				Gun.IgnoredEntity = gameObject;

				NearbyGun = null;
			} else if (Gun != null) {
				Gun.transform.parent = null;
				Gun.Rb.isKinematic = false;
				Gun.Rb.AddForce((Hand.transform.forward + Hand.transform.up) * 2, ForceMode.Impulse);

				Gun = null;
			}
		}

		try {
			if (Input.GetMouseButtonDown(0) && !Gun.IsBusy) {
				Gun.Shoot();
			}
		} catch {}

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Weapon") {
			NearbyGun = collision.collider.GetComponent<Weapon>();
		}
	}

	void OnCollisionExit(Collision collision)
	{
		try {
			if (collision.collider.gameObject == NearbyGun.gameObject) {
				NearbyGun = null;
			}
		} catch {}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Weapon") {
			NearbyGun = collider.GetComponent<Weapon>();
		}
	}

	void OnTriggerExit(Collider collider)
	{
		try {
			if (collider.gameObject == NearbyGun.gameObject) {
				NearbyGun = null;
			}
		} catch {}
	}
}
