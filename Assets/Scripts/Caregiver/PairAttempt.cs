using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using ZXing.QrCode;
using ZXing;
public class PairAttempt : MonoBehaviour {

	[SerializeField]
	private UIManager _ui;
	[SerializeField]
	private InputField _patientID;
	[SerializeField]
	private SnapBarcode _scanningUI;
	
	[SerializeField]
	private SocketIOComponent _socket;
	private  WebCamTexture _camTex;

	[SerializeField]
	private AnimationLibrary _animLib;
	


	void Start()
	{
		Debug.Log(Application.platform.ToString());
		_socket.On(Constants.CAREGIVER_EXPERIENCE_MANIFEST_RESPONSE_MESSAGE, receiveManifest);
		_socket.On(Constants.CAREGIVER_PAIRED_MESSAGE, connectionPaired);
		
	}

	void connectionPaired(SocketIOEvent ev)
	{
		_animLib.getAnimationLibrary();
		_socket.Emit(Constants.CAREGIVER_GET_CONTENT_MESSAGE);
	}

	void receiveManifest(SocketIOEvent ev)
	{
		if(!PairingTracker.Instance.isPaired)
		{
			PairingTracker.Instance.isPaired = true;		
			ExperiencesManifest.Instance.Manifest = ev.data.ToString();
			_scanningUI.disableCamera();
			_ui.nextUI();
			Debug.Log("next ui in pair");
		}
	}

	
	public void  pair()
	{
		if(_patientID.text == null || _patientID.text == "")
			return;
			
		string[] data = new string[]{_patientID.text, Application.platform.ToString()};
		_socket.Emit(Constants.CAREGIVER_CONNECTED_MESSAGE, ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_PAIRING_FIELDS, data));
	}
}
