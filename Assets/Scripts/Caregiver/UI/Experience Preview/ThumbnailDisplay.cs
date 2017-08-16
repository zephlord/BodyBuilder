using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ThumbnailDisplay : MonoBehaviour {

	private int _index;
	private Texture2D[] _thumbs;
	void Start () {
		_thumbs = new Texture2D[1];
		_thumbs[0] = null;
		StartCoroutine(changeThumbnail());
	}
	
	public void switchExperience(string[] thumbURLS)
	{
		_index = 0;
		_thumbs = new Texture2D[thumbURLS.Length];
		for(int i = 0; i < thumbURLS.Length; i++)
		{
			_thumbs[i] = null;
			StartCoroutine(downloadTexture(i,thumbURLS[i]));
		}
	}

	IEnumerator downloadTexture(int index, string url)
	{
		 // Start a download of the given URL
		 UnityWebRequest www = UnityWebRequest.GetTexture(url);

        // Wait for download to complete
        yield return www.Send();
		while(!www.isDone)
		{
			yield return www;
		}
		if(www.error != null)
			Debug.Log(www.error);
        // assign texture
		Texture2D newTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
		newTex.Apply();
        _thumbs[index] = newTex;
	}

	IEnumerator changeThumbnail()
	{
		while(true)
		{
			GetComponent<RawImage>().texture = _thumbs[_index];
			yield return new WaitForSeconds(Constants.THUMBNAIL_DISPLAY_TIME);
			_index = (_index + 1) % _thumbs.Length;
			yield return null;
		}
	}
}
