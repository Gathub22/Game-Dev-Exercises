
using TMPro;
using UnityEngine;

public class DatosJuego : MonoBehaviour
{
	public int id;
	public bool terminado;
	private int _puntos;
	public int puntos
	{
		get {
			return _puntos;
		}

		set {
			t_puntos.text = "Puntos: " + value.ToString();
			_puntos = value;
		}
	}

	public string[] planetas = {"Tierra", "Jupiter", "Marte", "Mercurio", "Neptuno", "Saturno", "Sol", "Urano", "Venus"};

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
		LoadRound();
		AudioSource[] lista = Camera.main.gameObject.GetComponents<AudioSource>();
		correcto = lista[0];
		error = lista[1];
		pausa = false;
	}

	void Update()
	{
		if (segundos > 0 && !pausa) {
			segundos -= Time.deltaTime;
			temporizador.text = ((int)segundos).ToString();
		}

		if (segundos < 1) {
			if (!terminado) {
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
		id = Random.Range(0, 8);
		titulo.text = "Encuentra "+planetas[id];
		puntos = puntos;
		if (--rondas < 1) {
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

	public void ReproducirError()
	{
		error.Play();
	}

	public void ReproducirCorrecto()
	{
		correcto.Play();
	}
}
