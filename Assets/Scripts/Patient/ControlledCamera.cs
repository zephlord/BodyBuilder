using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;


public class ControlledCamera : MonoBehaviour {

	[SerializeField]
	private Transform _cameraTransform;
	[SerializeField]
	private GameObject _patientViewRig;
	[SerializeField]
	private GameObject _patientPositionMarker;

	
	void Start () {
	}
	
	public void moveCam(int cmd)
	{
		switch(cmd)
		{
			case (int)Constants.CameraControls.FORWARD:
			transform.Translate(new Vector3(0,0,Constants.CAMERA_MOVE_AMOUNT));
			break;

			case (int)Constants.CameraControls.BACKWARD:
			transform.Translate(new Vector3(0,0,-Constants.CAMERA_MOVE_AMOUNT));
			break;
			
			case (int)Constants.CameraControls.LEFT:
			transform.Translate(new Vector3(Constants.CAMERA_MOVE_AMOUNT,0,0));
			break;

			case (int)Constants.CameraControls.RIGHT:
			transform.Translate(new Vector3(-Constants.CAMERA_MOVE_AMOUNT,0,0));
			break;

			case (int)Constants.CameraControls.UP:
			transform.Translate(new Vector3(0,Constants.CAMERA_MOVE_AMOUNT,0));
			break;
			
			case (int)Constants.CameraControls.DOWN:
			transform.Translate(new Vector3(0,-Constants.CAMERA_MOVE_AMOUNT,0));
			break;

			case (int)Constants.CameraControls.ZOOM_IN:
			_cameraTransform.Translate(new Vector3(0,0,Constants.CAMERA_MOVE_AMOUNT));
			break;

			case (int)Constants.CameraControls.ZOOM_OUT:
			_cameraTransform.Translate(new Vector3(0,0,-Constants.CAMERA_MOVE_AMOUNT));
			break;
			
			case (int)Constants.CameraControls.ROTATE_PLUS_X:
			transform.Rotate(new Vector3(Constants.CAMERA_MOVE_AMOUNT,0,0));
			break;

			case (int)Constants.CameraControls.ROTATE_MINUS_X:
			transform.Rotate(new Vector3(-Constants.CAMERA_MOVE_AMOUNT,0,0));
			break;			
			
			case (int)Constants.CameraControls.ROTATE_PLUS_Y:
			transform.Rotate(new Vector3(0,Constants.CAMERA_MOVE_AMOUNT,0));
			break;			
			
			case (int)Constants.CameraControls.ROTATE_MINUS_Y:
			transform.Rotate(new Vector3(0,-Constants.CAMERA_MOVE_AMOUNT,0));
			break;

			case (int)Constants.CameraControls.ROTATE_PLUS_Z:
			transform.Rotate(new Vector3(0,0,Constants.CAMERA_MOVE_AMOUNT));
			break;

			case (int)Constants.CameraControls.ROTATE_MINUS_Z:
			transform.Rotate(new Vector3(0,0,-Constants.CAMERA_MOVE_AMOUNT));
			break;
			
			default:
			Debug.Log("Command not recognized");
			break;
		}
	}

	public void setNewPatientPosition()
	{
		_patientViewRig.transform.position = _patientPositionMarker.transform.position;
	}
}
