using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using ZXing.QrCode;
using ZXing;

public class InitializePatientTest : MonoBehaviour {

	public bool _testScene;
	[SerializeField]
	private GameObject _barcode;
	[SerializeField]
	private SocketIOComponent _socket;
	public string ID;
	private bool _isReadyToSendPairing;
	void Start () {
		_socket.On(Constants.CONNECTED_MESSAGE, connectionStarted);
		_socket.On(Constants.PATIENT_CONNECTED_RESPONSE_MESSAGE, test);
		_socket.On(Constants.PATIENT_RECEIVE_CONTENT_URL_MESSAGE, hidePairingTool);
    }


	void Update()
	{
		if(_isReadyToSendPairing)
		{
			string[] platformString = new string[1]{Application.platform.ToString()};
			JSONObject data = ServerCommsUtility.Instance.serializeData(Constants.PATIENT_SEND_PLATFORM_REQUEST_FIELDS, platformString);
			_socket.Emit(Constants.PATIENT_CONNECTED_MESSAGE, data);
			_isReadyToSendPairing = false;
		}

		if (_testScene) {
			_testScene = false;
			StartCoroutine (testServer ());
		}
	}

    private Color32[] QREncode(string textForEncoding, int width, int height) 
	{
		var writer = new BarcodeWriter 
		{
			Format = BarcodeFormat.QR_CODE,
			Options = new QrCodeEncodingOptions 
			{
				Height = height,
				Width = width
			}
		};
	return writer.Write(textForEncoding);
	}



	void hidePairingTool(SocketIOEvent ev)
	{
		_barcode.SetActive(false);
	}

	void connectionStarted(SocketIOEvent ev)
	{
		_isReadyToSendPairing = true;
	}
	public void test(SocketIOEvent ev)
	{
		_testScene = true;
	}

	public void test()
	{
		_testScene = true;
	}
		
	IEnumerator testServer()
	{
		yield return new WaitForSeconds (10);
		_socket.Emit ("stageTest");
	}


	
}
