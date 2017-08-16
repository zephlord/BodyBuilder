using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour {

	[SerializeField]
	private GameObject _VRCamera;
	[SerializeField]
	private GameObject _UICamera;
	[SerializeField]
	private GameObject _UI;


	void Start () {
		
	}
	
	void Update () {
		
	}

	public void resetCams(float x, float y, float z)
	{
		_UI.SetActive(false);
		_UICamera.SetActive(false);
		Camera.main.gameObject.SetActive(false);
		_VRCamera.SetActive(true);
		_VRCamera.transform.position = new Vector3(x,y,z);
	}
}
