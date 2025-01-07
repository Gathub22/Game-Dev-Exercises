using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LamparaJuego : MonoBehaviour
{

	public float escala;

	void OnMouseEnter()
	{
		Vector3 s = gameObject.transform.localScale;
		s.x += escala;
		s.y += escala;
		gameObject.transform.localScale = s;
	}
	void OnMouseExit()
	{
		Vector3 s = gameObject.transform.localScale;
		s.x -= escala;
		s.y -= escala;
		gameObject.transform.localScale = s;
	}

	void OnMouseDown()
	{
		int e = GameObject.Find("Datos").GetComponent<DatosJuego>().estado;

		if (e > 1) {
			SceneManager.LoadScene("Juego");
		} else {
			SceneManager.LoadScene("Menu");
		}
	}
}
