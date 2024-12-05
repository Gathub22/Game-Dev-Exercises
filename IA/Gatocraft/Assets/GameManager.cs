using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Jobs;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public GameObject Cube;
	public int Width, Large, Seed;
	public float Detail;

	// Start is called before the first frame update
	void Start()
	{
		int height;

		for (int x = 0; x < Width; x++) {
			for (int z = 0; z < Large; z++) {
				height = (int) (Mathf.PerlinNoise((x / 2 + Seed) / Detail, (z / 2 + Seed) / Detail) * Detail);
				for (int y = height; y > 0; y--) {
					GameObject c = Instantiate(Cube, new Vector3(x,y,z), Quaternion.identity);
					if (Physics.Raycast(c.transform.position, transform.up, 2)) {
						c.GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}
		}
	}
}
