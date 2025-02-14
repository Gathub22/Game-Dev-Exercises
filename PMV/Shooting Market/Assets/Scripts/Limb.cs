using UnityEngine;

public class Limb : MonoBehaviour
{
	public int Health;
	public float DamageMultiplier;
	public EnemyHealth EntityHealth;

	void Start()
	{
		EntityHealth = transform.parent.GetComponent<EnemyHealth>();
	}
	public void ApplyDamage(int damage)
	{
		Health -= damage;
		EntityHealth.AddDamage((int) (damage*DamageMultiplier));
	}

}
