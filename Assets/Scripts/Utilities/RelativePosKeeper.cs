using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativePosKeeper : MonoBehaviour {

	public Vector3 _pos;
	
	// Update is called once per frame
	void LateUpdate () {
		transform.localPosition = _pos;
	}
}
