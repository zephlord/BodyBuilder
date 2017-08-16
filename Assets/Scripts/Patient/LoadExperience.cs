using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UMA.AssetBundles;

public class LoadExperience : MonoBehaviour {



	[SerializeField]
	private SocketIOComponent _socket;
	[SerializeField]
	private StreamView _streamView;
	private AssetBundle _bundle;
	

	void Start()
	{
		_socket.On(Constants.PATIENT_RECEIVE_CONTENT_URL_MESSAGE, beginExperience);
	}
	
	private void beginExperience(SocketIOEvent ev)
	{
		string sceneName = "";
	    string sceneAssetBundle = "";
		string url = "";
		ev.data.GetField(ref sceneName, Constants.SCENE_NAME_FIELD);		
		ev.data.GetField(ref sceneAssetBundle, Constants.ASSET_BUNDLE_NAME_FIELD);		
		ev.data.GetField(ref url, Constants.CONTENT_URL_FIELD);
		StartCoroutine(Init(url, sceneName, sceneAssetBundle));
	}

	IEnumerator Init (string url, string sceneName, string sceneAssetBundle)
	{	
		yield return StartCoroutine(LoadLevel(url) );
		
	}

	protected IEnumerator LoadLevel(string url)
	{
		UnityWebRequest www = UnityWebRequest.GetAssetBundle(url);
        yield return www.Send();
        if(www.isError) {
            Debug.Log(www.error);
        }
        else {
            _bundle = ((DownloadHandlerAssetBundle)www.downloadHandler).assetBundle;
			string[] scenePaths = _bundle.GetAllScenePaths();
			float startTime = Time.realtimeSinceStartup;
			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.LoadSceneAsync(scenePaths[0], LoadSceneMode.Additive);
			_streamView.enabled = true;
			float elapsedTime = Time.realtimeSinceStartup - startTime;
			Debug.Log("Finished loading scene " + scenePaths[0] + " in " + elapsedTime + " seconds" );
	    }
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] objs = scene.GetRootGameObjects();
		_bundle.Unload(false);
		_bundle = null;
		foreach(GameObject obj in objs)
		{
			GameObject mainCamera = findTag(obj, "MainCamera");
			if(mainCamera != null)
				Destroy(mainCamera);
		}
    }

	GameObject findTag(GameObject parent, string tag)
	{
		if(parent.CompareTag(tag))
			return parent;
		foreach(Transform child in parent.transform)
			return findTag(child.gameObject, tag);
		return null;
	}

}
