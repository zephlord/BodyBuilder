using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UMA.AssetBundles;

public class AnimationSelection {

	private AnimationClip _clip;
	private string _clipBundleURL;
	private float _duration;
	private bool _isDownloadComplete = false;

	public AnimationSelection(string URL, float duration)
	{
		_duration = duration;
		_clipBundleURL = URL;
	}


	public IEnumerator downloadClip()
	{
		UnityWebRequest www = UnityWebRequest.GetAssetBundle(_clipBundleURL);
		yield return www.Send();


	    if(www.isError) {
            Debug.Log(www.error);
        }

		else 
		{
			AssetBundle bundle = ((DownloadHandlerAssetBundle)www.downloadHandler).assetBundle;
			AnimationClip[] clips = bundle.LoadAllAssets<AnimationClip>();
			_clip = clips[0];
			bundle.Unload(false);
			_isDownloadComplete = true;
			
		}
	}

	public bool isFinishedDownloading()
	{
		return _isDownloadComplete;
	}

	public float getDuration()
	{
		return _duration;
	}

	public AnimationClip getClip()
	{
		return _clip;
	}
}
