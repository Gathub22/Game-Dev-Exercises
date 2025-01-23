using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JugadorController : MonoBehaviour
{
	public float Velocidad;
	void FixedUpdate()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		transform.position += new Vector3(x, 0, y) * Velocidad;
	}

	void OnCollisionEnter(Collision collision)
	{
		Cargador c = collision.collider.GetComponent<Cargador>();
		if (c != null)
			SceneManager.LoadScene(c.Nivel);
		else if (collision.gameObject.name != "Suelo")
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
