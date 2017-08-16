using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using UMA;

public class PreviewAvatarAnimation : MonoBehaviour {


	private bool _isAvatarSet;
	[SerializeField]
	private DynamicCharacterAvatar _avatar;
	private UMATextRecipe _recipe;
	private AnimationClip _clip;
	private bool _isAnimating;
	private Animator _animator;
	private RuntimeAnimatorController _oldCtrl;

	
	// Update is called once per frame
	void Update () 
	{
		if(GetComponent<CapsuleCollider>() != null)
		{
			if(!_isAvatarSet && _recipe != null)
			{
				_avatar.LoadFromRecipe(_recipe);
				_isAvatarSet = true;
			}

			if(_clip != null && !_isAnimating)
			{
				_animator = GetComponent<Animator>();
				
				AnimatorOverrideController animCtrl = new AnimatorOverrideController();
				animCtrl.runtimeAnimatorController = _animator.runtimeAnimatorController;
				animCtrl[Constants.DEFAULT_ANIM_1] = _clip;
				_oldCtrl = _animator.runtimeAnimatorController;
				_animator.runtimeAnimatorController = animCtrl;
				_animator.SetBool(Constants.ENTER_ANIMATIONS_BOOL, true);
				// GetComponent<MecanimControl>().AddClip(_clip, _clip.name);
				// GetComponent<MecanimControl>().Play(_clip.name);
				_isAnimating = true;
			}
		}

	}


	public void setRecipe(UMATextRecipe rec)
	{
		_recipe = rec;
	}

	public void setClip(AnimationClip clip)
	{
		_clip = clip;
	}

	public void resetAvatar()
	{
		_animator.runtimeAnimatorController = _oldCtrl;
		_animator.SetBool("animPreview", false);
		_clip = null;
		_isAnimating = false;
	}


}
