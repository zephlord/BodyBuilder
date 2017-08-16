using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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


	public void confirm()
	{
		if(_mins.getCounter() == 0 && _secs.getCounter() == 0)
			return;
		_animSelect.selectAnimation(_mins.getCounter(), _secs.getCounter(), _anim);
		_avatar.resetAvatar();
		_anim = null;
		_mins.reset();
		_secs.reset();
	}

	public void setAnimation(AnimationClip anim)
	{
		_anim = anim;
		_avatar.setClip(anim);
	}

	public void setAnimation(string url, string animName)
	{
		StartCoroutine(downloadClip(url, animName));
	}

	public void setAnimation(AnimationClip anim, float min, float sec)
	{
		_anim = anim;
		_avatar.setClip(anim);
		_mins.setVal(min);
		_secs.setVal(sec);
	}

	public void cancel()
	{
		_mins.reset();
		_secs.reset();
		_avatar.resetAvatar();
		_animSelect.cancelAnimDurationSelection();
	}

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
