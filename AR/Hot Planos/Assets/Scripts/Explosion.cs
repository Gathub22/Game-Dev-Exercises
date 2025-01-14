using UnityEngine;

public class Explosion : MonoBehaviour
{

	public float Duracion;
	public float Aumento;
	private float tiempoTranscurrido = 0f;

	void FixedUpdate()
	{
		tiempoTranscurrido += Time.fixedDeltaTime;

		if (tiempoTranscurrido < Duracion) {
			transform.localScale += new Vector3(Aumento, Aumento, Aumento);
		} else {
			Destroy(gameObject);
		}
	}
}
