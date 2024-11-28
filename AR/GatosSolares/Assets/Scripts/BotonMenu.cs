using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonMenu : MonoBehaviour
{

	public void BotonJugar()
	{
		SceneManager.LoadScene("Escena");
	}

	public void BotonSalir()
	{
		Application.Quit();
	}
}
