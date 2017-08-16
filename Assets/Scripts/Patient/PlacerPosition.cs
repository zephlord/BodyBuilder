using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacerPosition : MonoBehaviour {

	private Vector3 _pos;
	void Start () {
		_pos = new Vector3(0,0,Constants.PLACEMENT_DISTANCE);
	}
	
	void LateUpdate () {
		transform.localPosition = _pos;
	}
}
