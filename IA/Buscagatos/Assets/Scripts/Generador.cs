using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using TMPro;
using UnityEngine;

public class Generador : MonoBehaviour
{
	public int filas;
	public int columnas;
	public float margen = 0f;
	public float probabilidadBombas = 0f;
	private GameObject[][] mapa;

	private Vector3 origen;

	// Start is called before the first frame update
	void Start()
	{
		DatosJuego dj = GameObject.Find("Datos").GetComponent<DatosJuego>();
		dj.estado = 0;
		probabilidadBombas = dj.dificultad;

		origen = gameObject.transform.position;
		mapa = new GameObject[filas][];

		for (int i = 0; i < filas; i++) {

			mapa[i] = new GameObject[columnas];

			for (int j = 0; j < columnas; j++) {

				GameObject c = new GameObject();
				Texture2D t = Resources.Load<Texture2D>("Sprites/Almohada");
				SpriteRenderer sr = c.AddComponent<SpriteRenderer>();
				sr.sprite = Sprite.Create(t, new Rect(0.0f, 0.0f, t.width, t.height), new Vector2(0.5f, 0.5f), 60.0f);

				c.AddComponent<BoxCollider2D>();

				Suelo s = c.AddComponent<Suelo>();
				s.fila = i;
				s.columna = j;
				if (Random.Range(0.0f, 1.0f) <= probabilidadBombas)
					s.bomba = true;

				c.transform.position = new Vector3( origen.x + j * (sr.size.x + margen), origen.y + i * (sr.size.y + margen), origen.z);
				mapa[i][j] = c;
			}
		}

	}

	public int BuscarMinas(int fila, int columna)
	{
		int b = 0;

		mapa[fila][columna] = null;
		for (int i = -1; i < 2; i++) {
			for (int j = -1; j < 2; j++) {
				try {
					Suelo s = mapa[fila+i][columna+j].GetComponent<Suelo>();
					if (s.bomba) {
						b++;
					}
				} catch {}
			}
		}

		ComprobarVictoria();

		return b;
	}

	public void LimpiarVacios(int fila, int columna)
	{
		for (int i = -1; i < 2; i++) {
			for (int j = -1; j < 2; j++) {
				try {
					mapa[fila+i][columna+j].GetComponent<Suelo>().BuscarBombas();
				} catch {}
			}
		}
	}

	private void ComprobarVictoria()
	{
		bool sueloLimpio = false;
		int i = 0;
		int j = 0;

		while ( i < filas && !sueloLimpio ) {
			while ( j < columnas && !sueloLimpio ) {
				try {
					if ( !mapa[i][j].GetComponent<Suelo>().bomba )
						sueloLimpio = true;
				}catch{}
				j++;
			}
			i++;
		}

		if ( !sueloLimpio ) {
			TMP_Text tmp = GameObject.Find("Resultado").GetComponent<TMP_Text>();
			tmp.text = "¡Has ganado!";
			tmp.color = Color.green;
			GameObject.Find("Datos").GetComponent<DatosJuego>().estado = 1;
		}
	}
}
