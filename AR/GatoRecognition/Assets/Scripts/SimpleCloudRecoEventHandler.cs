using System.Runtime.CompilerServices;
using UnityEngine;
using Vuforia;

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
	CloudRecoBehaviour mCloudRecoBehaviour;
	bool mIsScanning = false;
	string mTargetMetadata = "";

	public ImageTargetBehaviour ImageTargetTemplate;

	// Register cloud reco callbacks
	void Awake()
	{
		mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
		mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
		mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
		mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
		mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
		mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
	}
	//Unregister cloud reco callbacks when the handler is destroyed
	void OnDestroy()
	{
		mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
		mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
		mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
		mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
		mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
	}

	public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
	{
		Debug.Log("Cloud Reco initialized");
	}

	public void OnInitError(CloudRecoBehaviour.InitError initError)
	{
		Debug.Log("Cloud Reco init error " + initError.ToString());
	}

	public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
	{
		Debug.Log("Cloud Reco update error " + updateError.ToString());

	}
	public void OnStateChanged(bool scanning)
	{
		mIsScanning = scanning;

		if (scanning)
		{
			// Clear all known targets
		}
	}

	// Here we handle a cloud target recognition event
	public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
	{
		// Store the target metadata
		mTargetMetadata = cloudRecoSearchResult.MetaData;
		print(mTargetMetadata);
		PlanetDataTemp m = JsonUtility.FromJson<PlanetDataTemp>(mTargetMetadata);
		PlanetData pd = new PlanetData();
		pd.id = m.id;
		pd.nombre = m.nombre;
		GameObject.Find("DatosJuego").GetComponent<DatosJuego>().CheckPlanet(pd.id);

		// Stop the scanning by disabling the behaviour
		mCloudRecoBehaviour.enabled = false;

		// Build augmentation based on target
		if (ImageTargetTemplate)
		{
			/* Enable the new result with the same ImageTargetBehaviour: */
			mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
		}
		mCloudRecoBehaviour.enabled = true;
		mTargetMetadata = "";
	}

	/*
	void OnGUI()
	{
		// Display current 'scanning' status
		GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
		// Display metadata of latest detected cloud-target
		GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
		// If not scanning, show button
		// so that user can restart cloud scanning
		if (!mIsScanning)
		{
			if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
			{
				// Reset Behaviour
				mCloudRecoBehaviour.enabled = true;
				mTargetMetadata = "";
			}
		}
	}
	**/
}

[System.Serializable]
class PlanetDataTemp
{
	public int id;
	public string nombre;
}
