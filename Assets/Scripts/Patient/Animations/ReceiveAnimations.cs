using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

///<summary>
/// get the animations sent by the caregiver
///</summary>
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
	
	void Update () 
	{
		if(_animator == null && GetComponent<Animator>() != null)
		{
			_animator = GetComponent<Animator>();
		}
	}

	// get all the animations that were sent
	// and play them
	void downloadAnimations(SocketIOEvent ev)
	{
		string animCt = "";
		int animCount = -1;
		ev.data.GetField(ref animCt, "animNum");
		animCount = Convert.ToInt32(animCt);
		
		// for each animation in the list perscribed by the caregiver...
		for(int i = 0; i < animCount; i ++)
		{
			string durationStr = "";
			float duration = -1;
			string url = "";
			// get the duration and download link
			ev.data.GetField(ref durationStr, "anim" + i + "duration");
			duration = (float) Convert.ToDouble(durationStr);
			ev.data.GetField(ref url, "anim" + i + "url");

			// begin downloading and add the clip to the queue
			AnimationSelection animSel = new AnimationSelection(url, duration);
			StartCoroutine(animSel.downloadClip());
			_anims.Enqueue(animSel);
		}

		StartCoroutine(playAnimations());
	}

	IEnumerator playAnimations()
	{
		// wait until all the animations have finished downloading
		while(!finishedDownloadingAllAnims())
		{
			yield return null;
		}

		// tell the avatar to enter the animation section of the animator
		_animator.SetBool(Constants.ENTER_ANIMATIONS_BOOL, true);

		string anim1ToChange = Constants.DEFAULT_ANIM_1;
		string anim2ToChange = Constants.DEFAULT_ANIM_2;
		bool isAnim1 = true;
		AnimationClip previousAnim = null;

		// while we still have animations to process
		while(_anims.Count > 0)
		{
			// get the next animation
			AnimationSelection anim = _anims.Dequeue();

			// get the animator controller
			RuntimeAnimatorController runtimeCtrl = _animator.runtimeAnimatorController;
			// if we have already created an override controller we use it
			// This is to avoid override controllers created from override controllers, which unity disallows
			AnimatorOverrideController overideRuntime = runtimeCtrl as AnimatorOverrideController;
			if(overideRuntime != null)
				runtimeCtrl = overideRuntime.runtimeAnimatorController;
	
			AnimatorOverrideController animCtrl = new AnimatorOverrideController();
			animCtrl.runtimeAnimatorController = runtimeCtrl;

			// and override the correct animation
			// set the previous animation to allow for smooth transition between animations
			if(isAnim1)
			{
				animCtrl[anim1ToChange] = anim.getClip();
				if(previousAnim != null)
					animCtrl[anim2ToChange] = previousAnim;
			}

			else
			{
				animCtrl[anim2ToChange] = anim.getClip();
				if(previousAnim != null)
					animCtrl[anim1ToChange] = previousAnim;
			}

			// set the new previous animation
			previousAnim = anim.getClip();
			// set the override controller
			_animator.runtimeAnimatorController = animCtrl;
			// set the bool to enter animation 1 or 2 
			_animator.SetBool(Constants.TOGGLE_MODIFIED_ANIM_BOOL, !isAnim1);
			// allow for animation to run for perscribed time
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
