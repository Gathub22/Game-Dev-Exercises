using UnityEngine;

public class Bala : MonoBehaviour
{
	public GameObject Explosion;
	private float RemainingTime = 2;

	void Update()
	{
		RemainingTime -= Time.deltaTime;
		if (RemainingTime <= 0f) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		Instantiate(Explosion, transform.position, Quaternion.identity);
		Destroy(collision.gameObject);
		Destroy(gameObject);
	}
}
