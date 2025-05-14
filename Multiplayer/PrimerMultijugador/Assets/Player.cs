using UnityEngine;

public class Player : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		GetComponent<SpriteRenderer>().color = Random.ColorHSV();
	}
}
