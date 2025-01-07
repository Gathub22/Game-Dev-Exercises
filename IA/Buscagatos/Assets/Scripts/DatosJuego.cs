using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosJuego : MonoBehaviour
{
	public float dificultad;
	public int estado; // 0 = empezado 1 = ganado 2 = perdido

	void Awake()
	{
		if (FindObjectsOfType<DatosJuego>().Length > 1) {
			Destroy(gameObject);
		} else {
			gameObject.name = "Datos";
			DontDestroyOnLoad(gameObject);
		}
	}
}
