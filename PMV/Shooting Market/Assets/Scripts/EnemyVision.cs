using System;
using System.Linq;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
	public bool InSight;
	[SerializeField] private EnemyAI enemyAI;

	void Start()
	{
		enemyAI = transform.parent.GetComponent<EnemyAI>();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.GetComponent<Player>()) {
			CheckForPlayer(collider);
		}
	}

	void OnTriggerStay(Collider collider)
	{
		if (collider.GetComponent<Player>()) {
			CheckForPlayer(collider);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.GetComponent<Player>()) {
			InSight = false;
		}
	}

	void CheckForPlayer(Collider collider)
	{
		Player p = collider.GetComponent<Player>();

		if (p != null) {
			Vector3 normalized = Vector3.Normalize(p.transform.position - transform.parent.position);
			Vector3 target = new Vector3(normalized.x * 20, 0, normalized.z * 20);
			Ray r = new Ray(transform.parent.position, target);
			RaycastHit[] hits = Physics.RaycastAll(r);
			Array.Reverse(hits);
			Debug.DrawRay(transform.position, target, Color.red);

			foreach (RaycastHit hit in hits) {
				if (!hit.collider.GetComponent<Player>() &&
				 !hit.collider.GetComponent<EnemyAI>() &&
				 !hit.collider.GetComponent<EnemyVision>() &&
				 !hit.collider.tag.Equals("Player")
				){
					InSight = false;
					return;
				} else if(hit.collider.tag.Equals("Player")) {
					enemyAI.AddAttackTarget(p.gameObject, true);
					InSight = true;
					return;
				}
			}
		}
	}
}
