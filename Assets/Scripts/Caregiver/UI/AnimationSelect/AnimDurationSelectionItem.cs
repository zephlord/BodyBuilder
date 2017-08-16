using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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

	public void setOnClickAnim(UnityAction action)
	{
		_animName.onClick.AddListener(action);
	}

		public void setOnClickDelete(UnityAction action)
	{
		_delete.onClick.AddListener(action);
	}
}
