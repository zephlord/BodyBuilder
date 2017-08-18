using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Call functions in this to rotate game object
/// </summary>
public class Rotate : MonoBehaviour {

	[SerializeField]
	private float _rotateStep;
	[SerializeField]
	private GameObject _toRotate;

	// If there is no assigned gameObject,
	// set it to the object with this script attached
	void Start()
	{
		if(_toRotate == null)
			_toRotate = gameObject;
	}

	/// <summary>
	/// Rotate left the step sized set in the editor
	/// </summary>
	public void rotateLeft()
	{
		_toRotate.transform.eulerAngles += new Vector3(0,_rotateStep,0);
	}

	/// <summary>
	/// Rotate left a specified step size
	/// </summary>
	/// <param name="step"> how much to rotate</param>
	public void rotateLeft(float step)
	{
		_toRotate.transform.eulerAngles += new Vector3(0,step,0);
	}

	/// <summary>
	/// Rotate right the step sized set in the editor
	/// </summary>
	public void rotateRight()
	{
		_toRotate.transform.eulerAngles -= new Vector3(0,_rotateStep,0);
	}

	/// <summary>
	/// Rotate right a specified step size
	/// </summary>
	/// <param name="step"> how much to rotate</param>
	public void rotateRight(float step)
	{
		_toRotate.transform.eulerAngles -= new Vector3(0,step,0);
	}
}
