using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using SimpleJSON;
using SocketIO;

	/// <summary>
	/// Storage Container for Library of Animations.
	/// Downloads Animation Manifest from the server.
	/// The Animation Manifest has all animations on server sorted under their tags.
	/// </summary>
public class AnimationLibrary : MonoBehaviour {
	// the socket for communication with the server
	[SerializeField]
	private SocketIOComponent _socket; 
	private int _animDownloadedNum;
	private int _animNum;
	private bool _isLibraryDownloaded;
	private Dictionary<string, List<string>> _animationManifest;
	
	void Start()
	{
		_animDownloadedNum = 0;
		_animNum = -1;
		_animationManifest = new Dictionary<string, List<string>>();
		_socket.On(Constants.CAREGIVER_RECEIVE_ANIMATION_LIBRARY_MESSAGE, downloadAnimationLibrary);
	}

	/// <summary>
	/// The function that begins the download.
	/// Sends message to server to download all animations.
	/// Server should check the platform
	/// </summary>
	public void getAnimationLibrary()
	{
		string[] dataStrings = new string[1]{Application.platform.ToString()};
		_socket.Emit(Constants.CAREGIVER_GET_ANIMATION_LIBRARY_MESSAGE);
	}


	/// <summary>
	/// Processing server response of animation Library
	/// </summary>
	///<param name="ev">the returned event containing the JSON of the Animation Manifest </param>
	///
	void downloadAnimationLibrary(SocketIOEvent ev)
	{

		JSONNode data = JSON.Parse(ev.data.ToString());
		JSONNode allTags = data[Constants.MANIFEST_TAG_LIST_FIELD];
		JSONNode tagLists = data[Constants.MANIFEST_TAGS_FIELD];
		// goes through all tags and adds each animation under each tag to the dict
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
