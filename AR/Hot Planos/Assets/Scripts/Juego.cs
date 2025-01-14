

using TMPro;
using UnityEngine;

public class Juego : MonoBehaviour
{

	public GameObject Enemigo;
	public GameObject Aliado;
	public GameObject Bala;
	public GameObject CamaraAR;
	public TMP_Text Puntos_texto;

	public float GenerarEnemigo = 5;
	public int EnemigosRestantes;
	public int puntos;
	private float tiempoTranscurrido;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		puntos = 0;
		Puntos_texto.text = "0";
		EnemigosRestantes = Random.Range(10, 15);
	}

	// Update is called once per frame
	void Update()
	{
		tiempoTranscurrido += Time.deltaTime;
		if(tiempoTranscurrido > GenerarEnemigo && EnemigosRestantes > 0) {
			tiempoTranscurrido = 0f;
			generarEnemigoAleatorio();
			EnemigosRestantes--;
		}
	}

	private void generarEnemigoAleatorio()
	{
		Vector3 posicionEnemigo = new Vector3(Random.Range(-2f,2f), Random.Range(3f,4f), Random.Range(7f,8f));
		if (Random.Range(0f,1f) > 0.5){
			Instantiate(Enemigo, posicionEnemigo, Quaternion.identity);
		}else {
			Instantiate(Aliado, posicionEnemigo, Quaternion.identity);
		}
	}

	public void DispararBala()
	{
		GameObject bala = Instantiate(Bala, CamaraAR.transform.position, CamaraAR.transform.rotation);
		bala.GetComponent<Rigidbody>().AddForce(CamaraAR.transform.forward * 2000);
	}

	public void AcabarPartida()
	{
		Puntos_texto.text = "Puntos: " + puntos;
		GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
		Destroy(this);
	}
}
