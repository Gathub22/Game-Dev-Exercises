using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Room : MonoBehaviour
{

	public bool IsConnected;
	public GameObject UpWall;
	public GameObject LeftWall;
	public GameObject RightWall;
	public GameObject DownWall;
	public int RoomsLeftToGenerate;
	public int GenerationAttemptLimit;


	void Start()
	{
		if (RoomsLeftToGenerate <= 0){
			Destroy(this);
			return;
		}

		float x, z;
		int wall = -1;
		int attempts = 0;
		int res;

		for (int i = 0; i < 2; i++) {
			do {

				attempts++;
				x = transform.position.x;
				z = transform.position.z;
				res = Random.Range(0,3);

				switch (res) {
					case 0:
						z += 10;
						wall = 0;
						break;

					case 1:
						x += 10;
						wall = 1;
						break;

					case 2:
						z -= 10;
						wall = 2;
						break;

					case 3:
						x -= 10;
						wall = 3;
						break;
				}

			} while (isOccupied(new Vector3(x, 0, z)) && attempts < GenerationAttemptLimit);

			if (attempts < GenerationAttemptLimit){
				switch (res) {
					case 0:
						Destroy(UpWall);
						break;

					case 1:
						Destroy(RightWall);
						break;

					case 2:
						Destroy(DownWall);
						break;

					case 3:
						Destroy(LeftWall);
						break;
				}
				GenerateRoom(x,z,wall);
				attempts = 0;
			}
		}
	}

	public void GenerateRoom(float x, float z, int wall)
	{
		if (isOccupied(new Vector3(x, 0, z)) && wall != -1){
			//yield return new WaitForEndOfFrame();
			return;
		}

		Object r = Instantiate(
			Resources.Load("Room"),
			new Vector3(x, 0, z),
			Quaternion.identity
		);

		Room room = r.GetComponent<Room>();
		room.RoomsLeftToGenerate = RoomsLeftToGenerate - 1 ;
		room.GenerationAttemptLimit = GenerationAttemptLimit;

		switch (wall) {
			case 0:
				Destroy(room.DownWall);
				break;

			case 1:
				Destroy(room.LeftWall);
				break;

			case 2:
				Destroy(room.UpWall);
				break;

			case 3:
				Destroy(room.RightWall);
				break;
		}
		// yield return new WaitForEndOfFrame();
		return;
	}

	private bool isOccupied(Vector3 pos)
	{
		return Physics.Raycast(pos, transform.forward, 6);
	}
}
