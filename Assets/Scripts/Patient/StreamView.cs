using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SocketIO;



public class StreamView : MonoBehaviour {

	[SerializeField]
	private bool _isPositionCam;
	[SerializeField]
	private SocketIOComponent _socket;
	
	[SerializeField]
	private RenderTexture _view;
	private float _frameRate = Constants.FRAME_RATE;
	private float _refreshRate;
	private float _timer;
	private Texture2D _transferImage;
	private string _sendMessage;
	private bool _hasStarted;
	
	void Start () 
	{
		if(_isPositionCam)
			_sendMessage = Constants.PATIENT_SEND_POSITIONING_VIEW_MESSAGE;
		else
			_sendMessage = Constants.PATIENT_SEND_VIEW_MESSAGE;
		_refreshRate = 1f / _frameRate;
		_transferImage = new Texture2D(_view.width, _view.height);
		_timer = 0;
		_hasStarted = true;
		StartCoroutine(transferPix());
	}

	void OnEnable()
	{
		if(_hasStarted)
		{
			StopCoroutine("transferPix");
			StartCoroutine(transferPix());
		}
	}
	
	IEnumerator transferPix() 
	{
		
		while(true)
		{
			_timer += Time.deltaTime;
			if(_timer >= _refreshRate)
			{
				RenderTexture.active = _view;
				_transferImage.ReadPixels(new Rect(0, 0, _view.width, _view.height), 0, 0);
				_transferImage.Apply();
				byte[] rawData = _transferImage.EncodeToPNG();
				string[] data = new string[1]{Convert.ToBase64String(rawData)};
				
				JSONObject requestData = ServerCommsUtility.Instance.serializeData(Constants.PATIENT_SEND_VIEW_REQUEST_FIELDS, data);
				_socket.Emit(_sendMessage, requestData);
				_timer = 0;
			}
			yield return null;
		}
	}
}
