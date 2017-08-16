using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosKeeper : MonoBehaviour {

	public Vector3 _pos;
	private Vector3 _posPos;
	private Vector3 _hipsPos;
	private bool _isCollider;

	void LateUpdate () 
	{
		//transform.position = _pos;
		if(!_isCollider && GetComponent<CapsuleCollider>() != null)
		{
			_isCollider = true;
			_hipsPos = GameObject.Find("Hips").transform.localPosition;
			_posPos = GameObject.Find("Position").transform.localPosition;
		}
		if(_isCollider)
		{
			//transform.position = _pos;
			GameObject.Find("Hips").transform.localPosition = _hipsPos;
			GameObject.Find("Position").transform.localPosition = _posPos;
		}
	}
}
