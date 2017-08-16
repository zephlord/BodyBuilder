using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using ZXing.QrCode;
using ZXing;
public class PairAttemptTest : MonoBehaviour {

	[SerializeField]
	private UIManager _ui;
	[SerializeField]
	private InputField _patientID;
	[SerializeField]
	private RawImage _scanTex;
	[SerializeField]
	private SocketIOComponent _socket;
	public bool _test;
	public string _id;
	[SerializeField]
	private InitializePatientID _pID;
	[SerializeField]
	private AnimationLibrary _animLib;

	void Update()
	{

		if(_test)
		{
			_test = false;
			if(_pID == null || _pID.ID == null || _pID.ID == "" )
				_patientID.text = _id;
			else
				_patientID.text = _pID.ID;
			pair();
		}
	}
	


	void Start()
	{
		_socket.On(Constants.CAREGIVER_EXPERIENCE_MANIFEST_RESPONSE_MESSAGE, receiveManifest);
		_socket.On(Constants.CAREGIVER_PAIRED_MESSAGE, connectionPaired);
		
	}

	void connectionPaired(SocketIOEvent ev)
	{
		PairingTracker.Instance.isPaired = true;
		_animLib.getAnimationLibrary();
		_socket.Emit(Constants.CAREGIVER_GET_CONTENT_MESSAGE);
	}

	void receiveManifest(SocketIOEvent ev)
	{
		ExperiencesManifest.Instance.Manifest = ev.data.ToString();
		_ui.nextUI();
	}

	
	public void  pair()
	{
		if(_patientID.text == null || _patientID.text == "")
			return;
			
		string[] data = new string[]{_patientID.text, Application.platform.ToString()};
		_socket.Emit(Constants.CAREGIVER_CONNECTED_MESSAGE, ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_PAIRING_FIELDS, data));
	}
}
