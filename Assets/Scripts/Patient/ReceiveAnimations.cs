using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class ReceiveAnimations : MonoBehaviour {

	[SerializeField]
	private SocketIOComponent _socket;
	private Queue<AnimationSelection> _anims;

	private Animator _animator;
	
	void Start () 
	{
		_anims = new Queue<AnimationSelection>();
		_socket.On(Constants.PATIENT_RECEIVE_ANIMATIONS_MESSAGE, downloadAnimations);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_animator == null && GetComponent<Animator>() != null)
		{
			_animator = GetComponent<Animator>();
		}
	}

	void downloadAnimations(SocketIOEvent ev)
	{
		string animCt = "";
		int animCount = -1;
		ev.data.GetField(ref animCt, "animNum");
		animCount = Convert.ToInt32(animCt);
		
		for(int i = 0; i < animCount; i ++)
		{
			string durationStr = "";
			float duration = -1;
			string url = "";
			ev.data.GetField(ref durationStr, "anim" + i + "duration");
			duration = (float) Convert.ToDouble(durationStr);
			ev.data.GetField(ref url, "anim" + i + "url");

			AnimationSelection animSel = new AnimationSelection(url, duration);
			StartCoroutine(animSel.downloadClip());
			_anims.Enqueue(animSel);
		}

		StartCoroutine(playAnimations());
	}

	IEnumerator playAnimations()
	{
		_animator.SetBool(Constants.ENTER_ANIMATIONS_BOOL, true);
		while(!finishedDownloadingAllAnims())
		{
			yield return null;
		}

		string anim1ToChange = Constants.DEFAULT_ANIM_1;
		string anim2ToChange = Constants.DEFAULT_ANIM_2;
		bool isAnim1 = true;
		AnimationClip previousAnim = null;

		while(_anims.Count > 0)
		{
			AnimationSelection anim = _anims.Dequeue();
			RuntimeAnimatorController runtimeCtrl = _animator.runtimeAnimatorController;
			AnimatorOverrideController overideRuntime = runtimeCtrl as AnimatorOverrideController;
			if(overideRuntime != null)
				runtimeCtrl = overideRuntime.runtimeAnimatorController;
			AnimatorOverrideController animCtrl = new AnimatorOverrideController();
			animCtrl.runtimeAnimatorController = runtimeCtrl;

			if(isAnim1)
			{
				animCtrl[anim1ToChange] = anim.getClip();
				//anim1ToChange = anim.getClip().name;
				if(previousAnim != null)
					animCtrl[anim2ToChange] = previousAnim;
			}

			else
			{
				animCtrl[anim2ToChange] = anim.getClip();
				//anim2ToChange = anim.getClip().name;
				if(previousAnim != null)
					animCtrl[anim1ToChange] = previousAnim;
			}
			previousAnim = anim.getClip();
			_animator.runtimeAnimatorController = animCtrl;
			_animator.SetBool(Constants.TOGGLE_MODIFIED_ANIM_BOOL, !isAnim1);
			yield return new WaitForSeconds(anim.getDuration());
			isAnim1 = !isAnim1;
		}

		_animator.SetBool(Constants.ENTER_ANIMATIONS_BOOL, false);
	}

	bool finishedDownloadingAllAnims()
	{
		foreach(AnimationSelection anim in _anims)
		{
			if(!anim.isFinishedDownloading())
				return false;
		}
		return true;
	}
}
