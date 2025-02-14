using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public int Ammo;
	public float CadencyInSeconds;
	public float ShotInaccuracy;
	public int BulletsPerShot;
	public bool IsBusy;
	public GameObject Tip;
	public Rigidbody Rb;
	public GameObject IgnoredEntity;

	private int originalAmmo;

	[SerializeField]
	private AudioSource shotSource;

	void Start()
	{
		originalAmmo = Ammo;
		Rb = GetComponent<Rigidbody>();
		shotSource = GetComponent<AudioSource>();
	}

	public void Shoot()
	{
		if (Ammo > 0) {

			Ammo--;
			shotSource.Play();

			for (int i = 0;  i < BulletsPerShot; i++) {
				GameObject b = (GameObject) Instantiate(Resources.Load("Prefabs/Bullet"), Tip.transform.position, Quaternion.identity);
				b.transform.position = Tip.transform.position;
				b.GetComponent<Bullet>().IgnoredEntity = IgnoredEntity;

				Rigidbody rb = b.GetComponent<Rigidbody>();
				var v = Tip.transform.forward * 10 + new Vector3(Random.Range(0,ShotInaccuracy), Random.Range(0,ShotInaccuracy), Random.Range(0,ShotInaccuracy));
				rb.AddForce(v, ForceMode.Impulse);
			}

			IsBusy = true;
			Invoke("Recover", CadencyInSeconds);
		}
	}

	public void Recover()
	{
		IsBusy = false;
	}

	public void Reload()
	{
		IsBusy = true;
		Ammo = originalAmmo;
		Invoke("Recover", 5);
	}
}
