using UnityEngine;
using UnityEngine.AI;

public class ExitTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player") {
			#if (UNITY_EDITOR)
				UnityEditor.EditorApplication.isPlaying = false;
			#elif (UNITY_STANDALONE)
				Application.Quit();
			#elif (UNITY_WEBGL)
				Application.OpenURL("data:text/html,<html><body><h1 style=\"color:blue;\">You have escaped!</h1><button onclick=\"window.location.href='https://rgatob.itch.io/shooting-market';\">Play again</button></body></html>");
			#endif
		}
	}
}
