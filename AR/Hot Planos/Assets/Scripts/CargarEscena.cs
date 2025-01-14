using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{
		public static void Cargar()
		{
			SceneManager.LoadScene("Juego");
		}
}
