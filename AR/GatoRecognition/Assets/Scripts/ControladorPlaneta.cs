using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class ControladorPlaneta : MonoBehaviour
{
	public int id;
	private static DatosJuego dj;

	void Start()
	{
		dj = GameObject.Find("DatosJuego").GetComponent<DatosJuego>();
	}

	public void ComprobarPlaneta()
	{
		if (!dj.terminado) {
			TMP_Text tmp = GameObject.Find("Texto").GetComponent<TMP_Text>();

			if (dj.id == id) {
				dj.puntos++;
				dj.ReproducirCorrecto();
				tmp.text = "HAS GANADO";
			} else {
				dj.ReproducirError();
				tmp.text = "HAS PERDIDO";
			}

			dj.rondas--;
			if (dj.rondas > 0) {
				dj.pausa = true;
				Invoke("RequestRound",3f);
			}
			else
				dj.EndGame();
		}
	}

	void RequestRound()
	{
		dj.LoadRound();
	}
}
