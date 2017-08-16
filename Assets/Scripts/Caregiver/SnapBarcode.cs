using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing.QrCode;
using ZXing;

public class SnapBarcode : MonoBehaviour {


	[SerializeField]
	private GameObject _barcodeScanner;
	[SerializeField]
	private InputField _IDField;
	private WebCamTexture _camTexture;
	[SerializeField]
	private PairAttempt _pair;
	private bool _isScannerDisplayed;
	void Start () {
		_camTexture = new WebCamTexture();
		_camTexture.requestedHeight = Constants.BARCODE_HEIGHT; 
		_camTexture.requestedWidth = Constants.BARCODE_WIDTH;
		StartCoroutine(scan());
    }

	void Update(){
		if(_isScannerDisplayed && !_camTexture.isPlaying && !PairingTracker.Instance.isPaired)
		{
			_camTexture.Play();
			_barcodeScanner.GetComponent<RawImage>().texture = _camTexture;
		}
	}
	public void displayScanner()
	{
		_isScannerDisplayed = true;
	}

	IEnumerator scan()
	{
		while(!PairingTracker.Instance.isPaired)
		{
			if(_isScannerDisplayed){

			
			yield return new WaitForSeconds(Constants.SCAN_RATE);
			try 
			{
				IBarcodeReader barcodeReader = new BarcodeReader ();
				if(!_camTexture.isPlaying)
					_camTexture.Play();
				var result = barcodeReader.Decode(_camTexture.GetPixels32(), _camTexture.width , _camTexture.height);
				if (result != null) 
				{
					_IDField.text = result.Text;
					_pair.pair();
				}
			} 
				catch(Exception ex) { Debug.LogWarning (ex.Message); }
			}
			yield return null;
		}
		
	}

	public void disableCamera()
	{
		_isScannerDisplayed = false;
		_camTexture.Stop();
		_camTexture = null;
	}
}
