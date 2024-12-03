using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

	public int Width;
	public int Height;
	private GameObject[,] rooms;
	public int RoomQuantity;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rooms = new GameObject[Width, Height];

		int x = Random.Range(0, Width);
		int y = Random.Range(0, Height);

		StartCoroutine(GenerateRoom(x, y, -1));
	}

	public IEnumerator GenerateRoom(int x, int y, int wall)
	{
		RoomQuantity--;

		Object r = Instantiate(
			Resources.Load("Room"),
			new Vector3(x, 0, y),
			Quaternion.identity
		);

		Room room = r.GetComponent<Room>();

		switch(wall) {
			case 0:
				Destroy(room.UpWall);
				break;
			case 1:
				Destroy(room.RightWall);
				break;
			case 2:
				Destroy(room.DownWall);
				break;
			case 3:
				Destroy(room.LeftWall);
				break;
		}

		yield return new WaitForEndOfFrame();

		if (RoomQuantity > 0) {

			int res = Random.Range(0,3);
			switch (res) {
				case 0:
					Destroy(room.UpWall);
					StartCoroutine(GenerateRoom(x, y+10, 0));
					break;

				case 1:
					Destroy(room.RightWall);
					StartCoroutine(GenerateRoom(x+10, y, 1));
					break;

				case 2:
					Destroy(room.DownWall);
					StartCoroutine(GenerateRoom(x, y-10, 2));
					break;

				case 3:
					Destroy(room.LeftWall);
					StartCoroutine(GenerateRoom(x-10, y, 3));
					break;
			}
		}

	}
}
