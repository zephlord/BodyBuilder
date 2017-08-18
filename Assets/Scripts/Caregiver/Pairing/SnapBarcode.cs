using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing.QrCode;
using ZXing;

///<sumary>
/// Uses the webcam to take a picture of the barcode for pairing
///</summary>
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

	void Update()
	{
		// if we are not scanning and should be, start the webcam
		if(_isScannerDisplayed && !_camTexture.isPlaying && !_pair.IsPaired)
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
		while(!_pair.IsPaired)
		{
			if(_isScannerDisplayed)
			{

			
			yield return new WaitForSeconds(Constants.SCAN_RATE);
			try 
			{
				IBarcodeReader barcodeReader = new BarcodeReader ();
				if(!_camTexture.isPlaying)
					_camTexture.Play();
				// take a picture with the scanner and try to decipher it 
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

	// stop scanning
	public void disableCamera()
	{
		_isScannerDisplayed = false;
		_camTexture.Stop();
		_camTexture = null;
	}
}
