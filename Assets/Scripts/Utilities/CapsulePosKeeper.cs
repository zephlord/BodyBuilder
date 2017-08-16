using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsulePosKeeper : MonoBehaviour {

	private Vector3 _colliderCenter;
	private float _colliderRadius;
	private float _colliderHeight;
	private bool _isCapsulePosSet;

	
	void Update () 
	{
		if(!_isCapsulePosSet && GetComponent<CapsuleCollider>() != null)
		{
			// CapsuleCollider cap = GetComponent<CapsuleCollider>();
			// _colliderCenter = cap.center;
			// _colliderRadius = cap.radius;
			// _colliderHeight = cap.height;
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<CapsuleCollider>().enabled = false;
			_isCapsulePosSet = true;
		}	

		if(_isCapsulePosSet)
		{
			// CapsuleCollider cap = GetComponent<CapsuleCollider>();
			// cap.center = _colliderCenter;
			// cap.radius = _colliderRadius;
			// cap.height = _colliderHeight;
		}
	}
}
