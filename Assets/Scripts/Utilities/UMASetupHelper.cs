using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Helps keep UMA from not fucking up everything graphically
/// enables scaling and gravit/collider disable so it doesn't fly away
///</summary>
public class UMASetupHelper : MonoBehaviour {

	public bool _disableGravity;
	public bool _disableCollider;
	public bool _doScale;
	[SerializeField]
	private float _scale;

	private bool _isGravityDisabled;
	private bool _isColliderDisabled;
	private bool _isScaled;

	
	void Update () 
	{
		// Disable the collider
		if(_disableCollider && GetComponent<Collider>() != null && !_isColliderDisabled)
		{
			GetComponent<Collider>().enabled = false;
			_isColliderDisabled = true;
		}

		// Disable gravity
		if(_disableGravity && GetComponent<Rigidbody>() != null && !_isGravityDisabled)
		{
			GetComponent<Rigidbody>().useGravity = false;
			_isGravityDisabled = true;
		}	

		// Safely Scale UMA
		if(_doScale && GetComponent<Collider>() != null && !_isScaled)
		{
			transform.Find("Root").transform.localScale *= _scale;
			_isScaled = true; 
		}
	}
}
