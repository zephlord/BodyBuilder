using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class ReceiveView : MonoBehaviour {

	[SerializeField]
	private bool _isPositionCam;
	[SerializeField]
	private SocketIOComponent _socket;
	[SerializeField]
	private RawImage _display;
	private Texture2D _tex;
	private string _receiveMessage;
	void Start () {
		if(_isPositionCam)
			_receiveMessage = Constants.CAREGIVER_RECEIVE_POSITION_VIEW_MESSAGE;
		else
			_receiveMessage = Constants.CAREGIVER_RECEIVE_PATIENT_VIEW_MESSAGE;
		_socket.On(_receiveMessage, displayData);
		_tex = new Texture2D(Constants.BARCODE_WIDTH, Constants.BARCODE_HEIGHT);
	}

	void displayData(SocketIOEvent ev)
	{
		string byteString = "";
		ev.data.GetField(ref byteString, "tex");
		_tex.LoadImage(Convert.FromBase64String(byteString));
		_tex.Apply();
		_display.texture = _tex;
	}
	
}
