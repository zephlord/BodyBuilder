using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages selection the duration for an animation
/// </summary>
public class AnimationDurationSelectionManager : MonoBehaviour {

	[SerializeField]
	private AnimationSelectManager _animSelect;
	[SerializeField]
	private Counter _mins;
	[SerializeField]
	private Counter _secs;
	[SerializeField]
	private PreviewAvatarAnimation _avatar;
	private AnimationClip _anim;
	[SerializeField]
	private GameObject _avatarObj;


	/// <summary>
	/// confirms choice of animation and duration
	/// </summary>
	public void confirm()
	{
		// make sure duration is nonzero
		if(_mins.getCounter() == 0 && _secs.getCounter() == 0)
			return;
		// set the animation into the selected list
		_animSelect.selectAnimation(_mins.getCounter(), _secs.getCounter(), _anim);

		// setup to be used again
		_avatar.resetAvatar();
		_anim = null;
		_mins.reset();
		_secs.reset();
	}

	/// <summary>
	/// set the animation clip to be displayed in the duration selection
	/// </summary>
	///<param name="anim"> the animation clip whose duration is being adjusted</param>
	public void setAnimation(AnimationClip anim)
	{
		_anim = anim;
		_avatar.setClip(anim);
	}

	/// <summary>
	/// set the animation clip to be displayed in the duration selection
	/// </summary>
	///<param name="url"> the url the clip will be downloaded from</param>
	///<param name="animName"> the name of the animation in case the downloaded item is incorrectly named</param>
	public void setAnimation(string url, string animName)
	{
		StartCoroutine(downloadClip(url, animName));
	}

	/// <summary>
	/// set the animation clip to be displayed in the duration selection.
	/// This version is used when editing a animation already selected.
	/// </summary>
	///<param name="anim"> the animation clip to edit the duration of</param>
	///<param name="min"> the minutes the animation is to play for</param>
	///<param name="sec"> the seconds the animation is to play for</param>
	public void setAnimation(AnimationClip anim, float min, float sec)
	{
		_anim = anim;
		_avatar.setClip(anim);
		_mins.setVal(min);
		_secs.setVal(sec);
	}

	/// <summary>
	/// cancel selecting duration for this animation
	/// </summary>
	public void cancel()
	{
		_mins.reset();
		_secs.reset();
		_avatar.resetAvatar();
		_animSelect.cancelAnimDurationSelection();
	}

	// Download the clip from the server and set its name
	IEnumerator downloadClip(string url, string animName)
	{
		UnityWebRequest www = UnityWebRequest.GetAssetBundle(url);
        yield return www.Send();
        if(www.isError) 
		{
            Debug.Log(www.error);
        }
        else 
		{
            AssetBundle bundle = ((DownloadHandlerAssetBundle)www.downloadHandler).assetBundle;
			AnimationClip[] animsInBundle = bundle.LoadAllAssets<AnimationClip>();
			_anim = animsInBundle[0];
			_anim.name = animName;
			_avatar.setClip(_anim);
			bundle.Unload(false);
        }
	}

	void OnEnable()
	{
		_avatarObj.SetActive (true);
	}
	void OnDisable()
	{
		_avatarObj.SetActive (false);
	}
}
