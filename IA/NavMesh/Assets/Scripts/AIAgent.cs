using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
	public GameObject Objeto;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		GetComponent<NavMeshAgent>().destination = Objeto.transform.position;
	}
}
