using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	[SerializeField]
	private float _rotateStep;
	[SerializeField]
	private GameObject _toRotate;

	public void rotateLeft()
	{
		_toRotate.transform.eulerAngles += new Vector3(0,_rotateStep,0);
	}

	public void rotateRight()
	{
		_toRotate.transform.eulerAngles -= new Vector3(0,_rotateStep,0);
	}
}
