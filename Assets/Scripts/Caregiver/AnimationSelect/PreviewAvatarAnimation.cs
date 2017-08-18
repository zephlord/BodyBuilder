using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using UMA;

///<summary>
///the way to preview an animation on the avatar
///</summary>
public class PreviewAvatarAnimation : MonoBehaviour {


	private bool _isAvatarSet;
	[SerializeField]
	private DynamicCharacterAvatar _avatar;
	private UMATextRecipe _recipe;
	private AnimationClip _clip;
	private bool _isAnimating;
	private Animator _animator;
	private RuntimeAnimatorController _oldCtrl;

	
	void Update () 
	{
		if(GetComponent<CapsuleCollider>() != null)
		{
			// set the recipe if not already set
			// this changes the avatar's appearance
			if(!_isAvatarSet && _recipe != null)
			{
				_avatar.LoadFromRecipe(_recipe);
				_isAvatarSet = true;
			}

			// if we have a clip to preview and are not currently animating
			// start the animation process
			if(_clip != null && !_isAnimating)
			{
				_animator = GetComponent<Animator>();
				
				// get the current controller and make an override controller from it
				AnimatorOverrideController animCtrl = new AnimatorOverrideController();
				animCtrl.runtimeAnimatorController = _animator.runtimeAnimatorController;

				// override the first preview clip
				animCtrl[Constants.DEFAULT_ANIM_1] = _clip;

				// save the old controller and set the override
				_oldCtrl = _animator.runtimeAnimatorController;
				_animator.runtimeAnimatorController = animCtrl;

				// begin the preview
				_animator.SetBool(Constants.ENTER_ANIMATIONS_BOOL, true);
				_isAnimating = true;
			}
		}

	}


	public void setRecipe(UMATextRecipe rec)
	{
		_recipe = rec;
	}

	///<summary>
	/// set the clip to preview
	///</summar>
	///<param name="clip">the clip to preview</param>
	public void setClip(AnimationClip clip)
	{
		_clip = clip;
	}

	///<summary>
	/// reset the avatar preview. 
	/// Resets the animator and removes the clip
	///</summary>
	public void resetAvatar()
	{
		_animator.runtimeAnimatorController = _oldCtrl;
		_animator.SetBool(Constants.ENTER_ANIMATIONS_BOOL, false);
		_clip = null;
		_isAnimating = false;
	}


}
