using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Suelo : MonoBehaviour
{
	public bool bomba = false;
	public int fila;
	public int columna;

	void OnMouseDown()
	{
		DatosJuego dj = GameObject.Find("Datos").GetComponent<DatosJuego>();
		if (dj.estado == 0) {
			if (bomba) {
				SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

				Texture2D t = Resources.Load<Texture2D>("Sprites/Gato");
				sr.sprite = Sprite.Create(t, new Rect(0.0f, 0.0f, t.width, t.height), new Vector2(0.5f, 0.5f), 60.0f);

				BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
				bc.enabled = false;

				dj.estado = 2;
				TMP_Text tmp = GameObject.Find("Resultado").GetComponent<TMP_Text>();
				tmp.text = "¡Has perdido!";
				tmp.color = Color.red;
			} else {
				// Imprimir números
				BuscarBombas();
			}
		}
	}

	public void BuscarBombas()
	{
		int n = GameObject.Find("Generador").GetComponent<Generador>().BuscarMinas(fila, columna);

		if ( n > 0 ) {
			GameObject t = new GameObject();
			TextMeshPro tmp = t.AddComponent<TextMeshPro>();
			tmp.text = n.ToString();
			tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
			tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
			tmp.fontSize = 7f;
			t.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
			Destroy(gameObject);
		} else {
			Destroy(gameObject);
			GameObject.Find("Generador").GetComponent<Generador>().LimpiarVacios(fila, columna);
		}
	}
}
