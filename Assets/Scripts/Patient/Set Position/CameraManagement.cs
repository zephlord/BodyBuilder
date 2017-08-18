using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class CameraManagement : MonoBehaviour {

	[SerializeField]
	private SocketIOComponent _socket;
	[SerializeField]
	private StreamView _positionView;
	[SerializeField]
	private GameObject _positionViewCam;
	[SerializeField]
	private ControlledCamera _positionCam;
	void Start () {
		_socket.On(Constants.PATIENT_RECEIVE_POSITION_VIEW_MOVE_MESSAGE, moveCam);
		_socket.On(Constants.PATIENT_TOGGLE_ON_POSITION_VIEW_MESSAGE, turnOnPositionView);
		_socket.On(Constants.PATIENT_TOGGLE_OFF_POSITION_VIEW_MESSAGE, turnOffPositionView);
		_socket.On(Constants.PATIENT_SET_NEW_POSITION_VIEW_MESSAGE, selectNewPosition);
	}
	
	void turnOnPositionView(SocketIOEvent ev)
	{
		toggleOnPatientView(false);
	}

	void turnOffPositionView(SocketIOEvent ev)
	{
		toggleOnPatientView(true);
	}

	void moveCam(SocketIOEvent ev)
	{
		string dir = "";
		ev.data.GetField(ref dir, Constants.MOVE_POSITION_CAMERA_FIELD);
		_positionCam.moveCam(Convert.ToInt32(dir));
	}

	void selectNewPosition(SocketIOEvent ev)
	{
		_positionCam.setNewPatientPosition();
		toggleOnPatientView(true);
	}
	void toggleOnPatientView(bool toggleOn)
	{
		//_patientViewCam.SetActive(toggleOn);
		_positionView.enabled = !toggleOn;
		_positionViewCam.SetActive(!toggleOn);
	}
}
