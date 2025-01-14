using UnityEngine;

public class Enemy : MonoBehaviour
{

	public Sprite[] Sprites;
	public float Speed;
	private float RemainingTime = 5f;
	private bool SelfDestructing = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		GetComponent<SpriteRenderer>().sprite = Sprites[Random.Range(0, Sprites.Length)];
	}

	void Update()
	{
		RemainingTime -= Time.deltaTime;

		if (RemainingTime <= 0f) {
			SelfDestructing = true;
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		transform.position -= new Vector3(0,Speed);
	}

	void OnDestroy()
	{
		Juego j = GameObject.Find("GestionJuego").GetComponent<Juego>();

		if(SelfDestructing) {
			if (tag == "Enemy") {
				j.puntos -= 5;
				j.Puntos_texto.text = j.puntos.ToString();
			} else {
				j.puntos += 10;
				j.Puntos_texto.text = j.puntos.ToString();
			}
		} else {
			if (tag == "Enemy") {
				j.puntos += 10;
				j.Puntos_texto.text = j.puntos.ToString();
			} else {
				j.puntos -= 5;
				j.Puntos_texto.text = j.puntos.ToString();
			}
		}

		if (j.EnemigosRestantes == 0) {
			j.AcabarPartida();
		}
	}
}
