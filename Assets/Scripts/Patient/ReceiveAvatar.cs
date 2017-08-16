using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using SimpleJSON;
using SocketIO;
using System;
using System.IO;

public class ReceiveAvatar : MonoBehaviour {



	[SerializeField]
	private DynamicCharacterAvatar _avatar;
	[SerializeField]
	private Camera _patientView;
	public Vector3 _patientViewOffset;
	[SerializeField]
	private SocketIOComponent _socket;
	private bool _inputRecipe;
	private string _recipeString;
	private bool _inputDNARecipe;
	private string _DNArecipeString;
	private bool _inputColorsRecipe;
	private string _ColorsRecipeString;
	private bool _inputFileRecipe;
	private string _FileRecipeString;
	void Start () {
		_socket.On(Constants.PATIENT_RECEIVE_AVATAR_MESSAGE, getAvatar);
		_socket.On(Constants.PATIENT_RECEIVE_AVATAR_DNA_MESSAGE, getAvatarDNA);
		_socket.On(Constants.PATIENT_RECEIVE_AVATAR_COLORS_MESSAGE, getAvatarColors);
		_socket.On(Constants.PATIENT_RECEIVE_AVATAR_FILE_MESSAGE, getAvatarFile);
	}
	
	void Update()
	{
		if(_inputRecipe && _inputDNARecipe && _inputColorsRecipe && _avatar.gameObject.GetComponent<Collider>() != null)
		{
			_inputRecipe = false;
			//_avatar.LoadFromRecipeString(_recipeString);
			_inputDNARecipe = false;
			_avatar.LoadDNAFromRecipeString(_DNArecipeString);
			_inputColorsRecipe = false;
			_avatar.LoadColorsFromRecipeString(_ColorsRecipeString);
			_patientView.transform.position = _avatar.gameObject.transform.Find("HeadAdjust").position + _patientViewOffset;
			Debug.Log(_avatar.GetDNA()["height"].Value + " = height");
		}	
	}

	private void getAvatar(SocketIOEvent ev)
	{
		_recipeString = ev.data.ToString();
		_avatar.SetLoadString(_recipeString);
		_avatar.enabled = true;
		_inputRecipe = true;
	}

	private void getAvatarDNA(SocketIOEvent ev)
	{
		_DNArecipeString = ev.data.ToString();
		_avatar.enabled = true;
		_inputDNARecipe = true;
	}

		private void getAvatarColors(SocketIOEvent ev)
	{
		_ColorsRecipeString = ev.data.ToString();
		_avatar.enabled = true;
		_inputColorsRecipe = true;
	}

	private void getAvatarFile(SocketIOEvent ev)
	{
		string byteString = "";
		ev.data.GetField(ref byteString, Constants.AVATAR_FILE_FIELD);
		byte[] recBytes = Convert.FromBase64String(byteString);
		_avatar.loadPathType = DynamicCharacterAvatar.loadPathTypes.Resources;
		_avatar.LoadFromTextFile(Constants.UMA_OUT_FILE);
		// _avatar.LoadFromTextFile()
		_FileRecipeString = ev.data.ToString();
		_avatar.enabled = true;
		_inputFileRecipe = true;
	}
}
