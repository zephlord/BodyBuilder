using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using SimpleJSON;
using SocketIO;

public class AnimationLibrary : MonoBehaviour {
	
	//private Dictionary<string, AnimationClip> _library;
	private Dictionary<string, List<string>> _animationManifest;
	[SerializeField]
	private SocketIOComponent _socket; 
	private int _animDownloadedNum;
	private int _animNum;
	private bool _isLibraryDownloaded;
	public bool test;
	void Start()
	{
		_animDownloadedNum = 0;
		_animNum = -1;
		//_library = new Dictionary<string, AnimationClip>();
		_animationManifest = new Dictionary<string, List<string>>();
		_socket.On(Constants.CAREGIVER_RECEIVE_ANIMATION_LIBRARY_MESSAGE, downloadAnimationLibrary);
	}


	public void getAnimationLibrary()
	{
		_socket.Emit(Constants.CAREGIVER_GET_ANIMATION_LIBRARY_MESSAGE);
	}

	void Update()
	{
		if(test)
		{
			test = false;
			getAnimationLibrary();
		}
	}
	void downloadAnimationLibrary(SocketIOEvent ev)
	{

		JSONNode data = JSON.Parse(ev.data.ToString());
		JSONNode allTags = data[Constants.MANIFEST_TAG_LIST_FIELD];
		JSONNode tagLists = data[Constants.MANIFEST_TAGS_FIELD];
		for(int i = 0; i < allTags.Count; i++)
		{
			string currentTag = allTags[i];
			_animationManifest.Add(currentTag, new List<string>());
			JSONNode anims = tagLists[currentTag];
			for(int j = 0; j < anims.Count; j++)
			{
				_animationManifest[currentTag].Add(anims[j]);
			}
		}
			_isLibraryDownloaded = true;
	}


	// IEnumerator downloadAnimation(string url)
	// {
	// 	UnityWebRequest www = UnityWebRequest.GetAssetBundle(url);
    //     yield return www.Send();
    //     if(www.isError) 
	// 	{
    //         Debug.Log(www.error);
    //     }
    //     else 
	// 	{
    //         AssetBundle bundle = ((DownloadHandlerAssetBundle)www.downloadHandler).assetBundle;
	// 		AnimationClip[] animsInBundle = bundle.LoadAllAssets<AnimationClip>();
	// 		if(_library.ContainsKey(animsInBundle[0].name))
	// 			_library[animsInBundle[0].name] = animsInBundle[0];
	// 		else
	// 			_library.Add(animsInBundle[0].name, animsInBundle[0]);
	// 		_animDownloadedNum++;
	// 		bundle.Unload(false);
    //     }
	// 	if( _animDownloadedNum >= _animNum)
	// 		_isLibraryDownloaded = true;

	// }

	// public Dictionary<string, AnimationClip> getLibrary()
	// {
	// 	return _library;
	// }

	public Dictionary<string, List<string>> getLibraryManifest()
	{
		return _animationManifest;
	}

	public bool isLibraryDownloaded()
	{
		return _isLibraryDownloaded;
	}

	public int getAnimNum()
	{
		return _animNum;
	}

}
