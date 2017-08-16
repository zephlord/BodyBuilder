using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AvatarCameraControl : MonoBehaviour {

	[SerializeField]
	private Camera _camera;
	[SerializeField]
	private Text _text;
	[SerializeField]
	private Vector3 _faceCamPos;
	[SerializeField]
	private Vector3 _bodyCamPos;
	private bool _isViewingFace;
	private float _faceFOV = 20;
	void Start () {
		_camera.transform.position = _bodyCamPos;
		_isViewingFace = false;
	}
	
	public void toggleCam()
	{
		if(_isViewingFace)
		{
			_text.text = "View Face";
			_camera.transform.position = _bodyCamPos;
		}

		else
		{
			_text.text = "View Body";
			_camera.transform.position = _faceCamPos;
		}

		_isViewingFace = !_isViewingFace;
	}
}
