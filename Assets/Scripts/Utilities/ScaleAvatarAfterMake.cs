using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAvatarAfterMake : MonoBehaviour {

	public float _scaleFactor;
	private Vector3 _scale;
	private bool _isScaled;


	void Start () {
		_scale = new Vector3(_scaleFactor, _scaleFactor, _scaleFactor);
	}
	
	// Update is called once per frame
	void Update () {
		if(_isScaled)
			return;
		else if(GetComponent<CapsuleCollider>() != null)
		{
			
			transform.Find("Root").transform.localScale = _scale;
			_isScaled = true;
		}


	}
}
