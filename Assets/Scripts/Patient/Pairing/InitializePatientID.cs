using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using ZXing.QrCode;
using ZXing;

///<summary>
/// connect the patient and give them an ID
///</summary>
public class InitializePatientID : MonoBehaviour {

	[SerializeField]
	private GameObject _barcode;
	[SerializeField]
	private SocketIOComponent _socket;
	[SerializeField]
	private GameObject _avatar;
	public string ID;
	private bool _isReadyToSendPairing;
	void Start () {
		_socket.On(Constants.PATIENT_CONNECTED_RESPONSE_MESSAGE, displayQR);
		_socket.On(Constants.PATIENT_RECEIVE_CONTENT_URL_MESSAGE, hidePairingTool);
    }


	void Update()
	{
		if(_isReadyToSendPairing)
		{
			string[] platformString = new string[1]{/*"Android"};*/Application.platform.ToString()};
			JSONObject data = ServerCommsUtility.Instance.serializeData(Constants.PATIENT_SEND_PLATFORM_REQUEST_FIELDS, platformString);
			_socket.Emit(Constants.PATIENT_CONNECTED_MESSAGE, data);
			_isReadyToSendPairing = false;
		}
	}

	// create the QR code
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
		_avatar.SetActive(true);
	}

	public void connectionStarted()
	{
		_isReadyToSendPairing = true;
	}

	// display the qr once the connection is established
	void displayQR(SocketIOEvent ev)
	{
		var encoded = new Texture2D (Constants.BARCODE_WIDTH, Constants.BARCODE_HEIGHT);
		var color32 = QREncode(_socket.sid, encoded.width, encoded.height);
		ID = _socket.sid;
		encoded.SetPixels32(color32);
		encoded.Apply();
		_barcode.GetComponent<RawImage>().texture= encoded;
	}


	
}
