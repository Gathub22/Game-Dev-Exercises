using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccionMenu : MonoBehaviour
{

	public float escala;
	public string texto;
	public string razon;
	public GameObject titulo;
	private TMP_Text tmp;

	void Start()
	{
		tmp = titulo.GetComponent<TMP_Text>();
		// print(JsonUtility.ToJson(tmp));
	}

	void OnMouseEnter()
	{
		Vector3 s = gameObject.transform.localScale;
		s.x += escala;
		s.y += escala;
		gameObject.transform.localScale = s;

		tmp.text = texto;
		tmp.fontStyle = 0;
	}
	void OnMouseExit()
	{
		Vector3 s = gameObject.transform.localScale;
		s.x -= escala;
		s.y -= escala;
		gameObject.transform.localScale = s;

		tmp.text = "Buscagatos";
		tmp.fontStyle = (FontStyles)3;
	}

	void OnMouseDown()
	{
		switch (razon) {

			case "Cerrar":
				Cerrar();
				break;

			case "Facil":
				Facil();
				break;

			case "Dificil":
				Dificil();
				break;
		}
	}

	void Cerrar()
	{
		Application.Quit();
		print("Cerrado");
	}

	void Facil()
	{
		DatosJuego j = GameObject.Find("Datos").GetComponent<DatosJuego>();
		j.dificultad = 0.2f;

		SceneManager.LoadScene("Juego");
	}

	void Dificil()
	{
		DatosJuego j = GameObject.Find("Datos").GetComponent<DatosJuego>();
		j.dificultad = 0.5f;

		SceneManager.LoadScene("Juego");
	}
}
