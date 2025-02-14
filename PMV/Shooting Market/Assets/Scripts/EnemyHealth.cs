using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

	public int Health;

	[SerializeField]
	private EnemyAI enemyAI;

	void Start()
	{
		enemyAI = GetComponent<EnemyAI>();
	}

	public void AddDamage(int damage)
	{
		Health -= damage;

		if (Health < 1){
			enemyAI.Weapon.transform.parent = null;
			enemyAI.Weapon.GetComponent<Rigidbody>().isKinematic = false;
			Destroy(gameObject);
		}
	}
}
