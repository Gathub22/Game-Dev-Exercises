
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class DatosJuego : MonoBehaviour
{
	public int id = -1;
	public bool terminado;
	private int _puntos;
	public int puntos
	{
		get
		{
			return _puntos;
		}

		set
		{
			t_puntos.text = "Puntos: " + value.ToString();
			_puntos = value;
		}
	}

	public List<PlanetData> planetas;
	public List<GameObject> modelosPlanetas;

	public TMP_Text t_puntos;
	public TMP_Text titulo;
	public TMP_Text temporizador;

	public int rondas;
	public float segundos;


	private AudioSource correcto;
	private AudioSource error;
	public bool pausa;

	// Start is called before the first frame update
	void Start()
	{
		puntos = 0;
		terminado = false;
		AudioSource[] lista = GameObject.Find("ARCamera").GetComponents<AudioSource>();
		correcto = lista[0];
		error = lista[1];
		pausa = true;
	}

	void Update()
	{
		if (segundos > 0 && !pausa)
		{
			segundos -= Time.deltaTime;
			temporizador.text = ((int)segundos).ToString();
		}

		if (segundos < 1)
		{
			if (!terminado)
			{
				LoadRound();
				ReproducirError();
			}
			else
				EndGame();
		}
	}
	public void LoadRound()
	{
		pausa = false;
		try {
			Destroy(GameObject.Find("ImageTarget").transform.GetChild(0).gameObject);
		} catch {}
		id = Random.Range(1, planetas.Count);
		titulo.text = "Encuentra " + GetPlanetNameById(id);
		puntos = puntos;
		if (--rondas < 1)
		{
			terminado = true;
		}
		segundos = 10;
	}

	public void EndGame()
	{
		terminado = true;
		titulo.text = "FIN DEL JUEGO";
		t_puntos.text = "Puntos: " + puntos;
	}

	private GameObject GetPlanetModelById(int id)
	{
		foreach (GameObject g in modelosPlanetas)
		{
			if (g.GetComponent<PlanetData>().id == id)
				return g;
		}
		return null;
	}

	private string GetPlanetNameById(int id)
	{
		foreach (PlanetData p in planetas)
		{
			if (p.id == id)
				return p.nombre;
		}
		return null;
	}

	public void CheckPlanet(int id)
	{
		GameObject obj = null;

		if (id == this.id) {
			obj = GetPlanetModelById(id);
			puntos++;
			ReproducirCorrecto();
			titulo.text = "HAS GANADO";

			obj = Instantiate(obj);
			obj.transform.parent = GameObject.Find("ImageTarget").transform;
			obj.transform.localScale = new Vector3(1.5f,1.5f,1.5f);

		} else {
			ReproducirError();
			titulo.text = "HAS PERDIDO";
		}

		rondas--;
		if (rondas > 0)
		{
			pausa = true;
			Invoke("LoadRound", 3f);
		}
		else
			EndGame();
	}

	public void ReproducirError()
	{
		error.Play();
	}

	public void ReproducirCorrecto()
	{
		correcto.Play();
	}
}
