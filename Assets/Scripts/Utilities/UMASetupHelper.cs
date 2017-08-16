using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMASetupHelper : MonoBehaviour {

	public bool _disableGravity;
	public bool _disableCollider;
	public bool _doScale;
	[SerializeField]
	private float _scale;

	private bool _isGravityDisabled;
	private bool _isColliderDisabled;
	private bool _isScaled;

	
	// Update is called once per frame
	void Update () 
	{
		if(_disableCollider && GetComponent<Collider>() != null && !_isColliderDisabled)
		{
			GetComponent<Collider>().enabled = false;
			_isColliderDisabled = true;
		}

		if(_disableGravity && GetComponent<Rigidbody>() != null && !_isGravityDisabled)
		{
			GetComponent<Rigidbody>().useGravity = false;
			_isGravityDisabled = true;
		}	

		if(_doScale && GetComponent<Collider>() != null && !_isScaled)
		{
			transform.Find("Root").transform.localScale *= _scale;
			_isScaled = true; 
		}
	}
}
