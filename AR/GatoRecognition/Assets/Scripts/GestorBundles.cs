using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GestorBundles : MonoBehaviour
{

	void Start()
	{
		string url = "https://drive.google.com/uc?export=download&id=1S41mCK75HdeAysR_mqM-65EgGu95mCqe";
		string manifest = "planetabundle";
		StartCoroutine(FetchGameObjectFromServer(url, manifest, 0, new Hash128()));
	}
	IEnumerator FetchGameObjectFromServer(string url, string manifestFileName, uint crcR, Hash128 hashR)
	{

		//Get from generated manifest file of assetbundle.
		uint crcNumber = crcR;
		//Get from generated manifest file of assetbundle.
		Hash128 hashCode = hashR;
		UnityWebRequest webrequest =
			UnityWebRequestAssetBundle.GetAssetBundle(url);

		webrequest.SendWebRequest();

		while (!webrequest.isDone)
		{
			Debug.Log(webrequest.downloadProgress);
		}

		AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(webrequest);
		yield return assetBundle;
		if (assetBundle == null)
			yield break;

		//Gets name of all the assets in that assetBundle.
		string[] allAssetNames = assetBundle.GetAllAssetNames();
		Debug.Log(allAssetNames.Length + "objects inside prefab bundle");

		DatosJuego dj = GameObject.Find("DatosJuego").GetComponent<DatosJuego>();
		dj.planetas = new List<PlanetData>();
		dj.modelosPlanetas = new List<GameObject>();

		foreach (string gameObjectsName in allAssetNames)
		{
			string gameObjectName = Path.GetFileNameWithoutExtension(gameObjectsName).ToString();
			GameObject objectFound = assetBundle.LoadAsset(gameObjectName) as GameObject;
			dj.planetas.Add(objectFound.GetComponent<PlanetData>()); // TODO: Por que no importa la calse del script al exportar el bundle???
			dj.modelosPlanetas.Add(objectFound);
		}

		dj.pausa = false;
		dj.LoadRound();

		assetBundle.Unload(false);
		yield return null;
	}
}
