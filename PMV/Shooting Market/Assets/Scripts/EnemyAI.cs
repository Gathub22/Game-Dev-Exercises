using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
	public int Mood;
	public Vector3 LastPosition;
	public GameObject Target;
	public Weapon Weapon;
	public GameObject Hand;
	public List<GameObject> Limbs;

	[SerializeField] private NavMeshAgent nma;
	[SerializeField] private Animator animator;
	[SerializeField] private bool isResting;
	[SerializeField] private EnemyVision enemyVision;

	void Start()
	{
		nma = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		enemyVision = transform.GetChild(0).GetComponent<EnemyVision>();
		Weapon.transform.parent = Hand.transform;
		Weapon.IgnoredEntity = gameObject;
	}

	void Update()
	{
		UpdateWeaponPosition();

		if (isResting)
			return;

		switch (Mood) {
			case 0:
				Roam();
				break;

			case 1:
				Attack();
				break;
		}

		if (Vector3.Distance(nma.velocity, Vector3.zero) > 0.5f) {
			animator.Play("Walking");
		} else {
			if (!enemyVision.InSight)
				animator.Play("Idling");
		}
	}

	private void UpdateWeaponPosition()
	{

		// Reset the Weapon's local position and rotation to match the bone
		Weapon.transform.localPosition = Vector3.zero;

		// // Optionally, adjust the Weapon's position and rotation relative to the bone
		// Weapon.transform.localPosition = new Vector3(0.1f, 0.1f, 0.1f); // Example adjustment
		// Weapon.transform.localRotation = Quaternion.Euler(0, 90, 0); // Example adjustment
	}

	private void Attack()
	{
		nma.speed = 5;
		if (enemyVision.InSight) {

			Vector3 direction = Target.transform.position - transform.position;
			Quaternion desiredRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 50.0f);

			nma.destination = transform.position;

			animator.Play("Point");

			//TODO: Randomize
			if (!Weapon.IsBusy && Random.Range(0,100) > 75){
				if (Weapon.Ammo > 0)
					Weapon.Shoot();
				else
					Weapon.Reload();
			}

		} else {
			nma.destination = LastPosition;
		}
	}

	private void Roam()
	{
		if (isResting == false && Vector3.Distance(nma.destination, transform.position) < 1f) {
			isResting = true;
			Invoke("ResumeWalking", Random.Range(3, 8));
		}
	}

	private void ResumeWalking()
	{
		isResting = false;
		nma.destination = GetRandomPointOnNavMesh();
	}

	private Vector3 GetRandomPointOnNavMesh()
	{
		Vector3 randomDirection = Random.insideUnitSphere * 10;
		randomDirection += transform.position;

		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomDirection, out hit, 10, NavMesh.AllAreas))
		{
				return hit.position;
		}

		return Vector3.zero;
	}

	public void AddAttackTarget(GameObject entity, bool notify)
	{
		Target = entity;
		Mood = 1;
		LastPosition = entity.transform.position;
		NotifyNearbyEnemies(entity, notify);
	}

	private void NotifyNearbyEnemies(GameObject entity, bool notify)
	{
		Collider[] entities = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);

		if (notify){
			foreach (Collider e in entities) {
				EnemyAI enemy = e.GetComponent<EnemyAI>();
				if (enemy && enemy != this) {
					enemy.AddAttackTarget(entity, false);
				}
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		Player p = collider.GetComponent<Player>();
		if (p != null) {

			if (!p.IsSneaking) {
				AddAttackTarget(p.gameObject, true);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		Player p = collision.collider.GetComponent<Player>();
		if (p != null) {
			AddAttackTarget(p.gameObject, true);
		}
	}
}
