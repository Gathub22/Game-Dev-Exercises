
using UnityEngine;

public class GiroPlaneta : MonoBehaviour
{
	public float giro = -1f;
	private Vector3 vGiro;

	void Start()
	{
		vGiro = new Vector3(0, giro, 0);
	}

	// Update is called once per frame
	void Update()
	{
		gameObject.transform.Rotate(vGiro);
	}
}
