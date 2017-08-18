using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

///<summary>
/// the item that holds a selected animation and its selected duration
///</summary>
public class AnimDurationSelectionItem : MonoBehaviour {

	[SerializeField]
	private Button _animName;
	[SerializeField]
	private Text _duration;
	[SerializeField]
	private Button _delete;
	private AnimationClip _anim;
	private float _mins;
	private float _secs;
	

	///<summary>
	/// set the info to be displayed
	///</summary>
	///<param name="mins">the minutes the animation is to play for</param>
	///<param name="secs">the seconds the animation is to play for</param>
	///<param name="anim">the selected animation</param>
	public void setInfo(float mins, float secs, AnimationClip anim)
	{
		_mins = mins;
		_secs = secs;
		_duration.text = mins + "mins " + secs + "secs";
		_animName.gameObject.transform.GetChild(0).GetComponent<Text>().text = anim.name;
		_anim = anim;
	}

	public float getMins()
	{
		return _mins;
	}

	public float getSecs()
	{
		return _secs;
	}

	public AnimationClip getAnim()
	{
		return _anim;
	}

	///<summary>
	///the action to do when the animation name is clicked
	///</summary>
	///<param name="action">the action to perform</param>
	public void setOnClickAnim(UnityAction action)
	{
		_animName.onClick.AddListener(action);
	}

	///<summary>
	///the action to do when the X is clicked
	///</summary>
	///<param name="action">the action to perform</param>
		public void setOnClickDelete(UnityAction action)
	{
		_delete.onClick.AddListener(action);
	}
}
