using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
	public float RemainingTime = 10f;
	public GameObject IgnoredEntity;
	private Vector3 origin;

	void Start()
	{
		origin = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		Debug.DrawLine(origin, transform.position, Color.red);
		RemainingTime -= Time.deltaTime;
			if(RemainingTime < 0)
				Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision)
	{
		CalculateCollision(collision);
	}

	void OnCollisionStay(Collision collision)
	{
		CalculateCollision(collision);
	}

	void OnCollisionExit(Collision collision)
	{
		CalculateCollision(collision);
	}

	private void CalculateCollision(Collision collision)
	{
		if (collision.collider.tag == "Bullet" || IgnoredEntity == collision.collider.gameObject || IgnoredEntity.transform.IsChildOf(collision.collider.gameObject.transform))
				return;

		Player p = collision.gameObject.GetComponent<Player>();
		if (p != null) {
			p.Health -= Random.Range(5,12);

			if (p.Health < 1)
				SceneManager.LoadScene("Level");

			return;
		}

		GameObject h;
		if (collision.collider.tag != "Human") {
			h = (GameObject) Instantiate(Resources.Load("Prefabs/Hole1"));
		} else {

			// If it was shot by an enemy...
			EnemyAI e = IgnoredEntity.GetComponent<EnemyAI>();
			if (e != null){
				if (!e.Limbs.Contains(collision.gameObject)){
					h = (GameObject) Instantiate(Resources.Load("Prefabs/Hole2"));
					collision.gameObject.GetComponent<Limb>().ApplyDamage(Random.Range(5,12));
				} else {
					return;
				}
			} else {

				// If it was shot by the player
				Limb l = collision.gameObject.GetComponent<Limb>();
				if (l != null) {
					h = (GameObject) Instantiate(Resources.Load("Prefabs/Hole2"));
					collision.gameObject.GetComponent<Limb>().ApplyDamage(Random.Range(5,12));
				} else {
					return;
				}
			}
		}

		PlaceHoleSprite(h, collision);
	}

	private void PlaceHoleSprite(GameObject h, Collision collision)
	{
		Vector3 originalScale = h.transform.localScale;
		Transform parentTransform = collision.gameObject.transform;
		h.transform.position = transform.position;
		h.transform.parent = parentTransform;
		h.transform.localScale = Vector3.Scale(originalScale, new Vector3(1 / parentTransform.localScale.x, 1 / parentTransform.localScale.y, 1 / parentTransform.localScale.z));

		h.transform.rotation = Quaternion.LookRotation(-collision.contacts[0].normal);

		Destroy(gameObject);
	}
}
