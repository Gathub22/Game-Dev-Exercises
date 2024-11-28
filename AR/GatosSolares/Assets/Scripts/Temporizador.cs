
using System.Collections;
using TMPro;
using UnityEngine;

public class Temporizador : MonoBehaviour
{

	public float segundos;

	// Start is called before the first frame update
	void Start()
	{
		segundos = 10f;
	}

	// Update is called once per frame
	void Update()
	{
		segundos -= Time.deltaTime;
		GetComponent<TMP_Text>().text = ((int)segundos).ToString();
	}

}
