using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

///<summary>
/// the script that downloads and cycles through thumbnails for the experience preview
///</summary>
public class ThumbnailDisplay : MonoBehaviour {

	private int _index;
	private Texture2D[] _thumbs;
	void Start () 
	{
		_thumbs = new Texture2D[1];
		_thumbs[0] = null;
		StartCoroutine(changeThumbnail());
	}
	
	///<summary>
	///set a new group of thumbnails
	///</summary>
	///<param name="thumbURLS">the list of urls where the thumbnails can be found</param>
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

	///<summary>
	/// coroutine used to download the thumbnails
	///</summary>
	///<param name="index">the index of the thumbnail being downloaded</param>
	///<param name="url">the url where the thumbnail is located</param>
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

	// cycles through the thumbnails
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
