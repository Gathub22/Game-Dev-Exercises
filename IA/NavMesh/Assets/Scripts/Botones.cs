using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
  public void CargarEscena(string nombre)
	{
		SceneManager.LoadScene(nombre);
	}
}
