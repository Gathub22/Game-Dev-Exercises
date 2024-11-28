using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorBoton : MonoBehaviour
{
	GameObject cubo;
	public Vector3 pos;
	public Vector3 escala;
	public float Intensidad = 0.5f;

	void Start()
	{
		cubo = GameObject.Find("Cube");
		pos = cubo.transform.position;
		escala = cubo.transform.localScale;
	}

	void Update()
	{
		pos = cubo.transform.position;
		escala = cubo.transform.localScale;
	}

	public void BotonArriba()
	{
		cubo.transform.position = new Vector3( pos.x, pos.y, pos.z  + Intensidad);
	}
	public void BotonAbajo()
	{
		cubo.transform.position = new Vector3( pos.x, pos.y, pos.z - Intensidad);
	}
	public void BotonIzquierda()
	{
		cubo.transform.position = new Vector3( pos.x - Intensidad, pos.y, pos.z);
	}
	public void BotonDerecha()
	{
		cubo.transform.position = new Vector3( pos.x + Intensidad, pos.y, pos.z);
	}
	public void BotonAumentar()
	{
		cubo.transform.localScale = new Vector3(escala.x + Intensidad, escala.y + Intensidad, escala.z + Intensidad);
	}
	public void BotonDisminuir()
	{
		cubo.transform.localScale = new Vector3(escala.x - Intensidad, escala.y - Intensidad, escala.z - Intensidad);
	}
}
