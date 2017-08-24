using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

///<summary>
///the controller and display for previewing the environments in the server
///</summary>
public class ExperiencePreviewController : MonoBehaviour {

	[SerializeField]
	private UIManager _ui;
	[SerializeField]
	private Text _title;
	[SerializeField]
	private ThumbnailDisplay _thumb;
	[SerializeField]
	private Text _info;
	[SerializeField]
	private SocketIOComponent _socket;
	[SerializeField]
	private GameObject _experienceInfoUI;
	private ExperienceInfoBundle _experienceInfo;
	private string[] _thumbURLs;
	private int _thumbURLCount = 0;

	void Awake()
	{
		_socket.On(Constants.CAREGIVER_THUMBNAIL_URL_RESPONSE_MESSAGE, thumbnailURLReceived);
	}

	///<summary>
	/// update the preview with new content
	///</summary>
	public void selectNewExperience(ExperienceInfoBundle selected)
	{
		_thumbURLCount = 0;
		_experienceInfo = selected;
		_experienceInfoUI.SetActive(true);
		updateContentDisplay();
		
	}

	///<summary>
	/// update the preview display
	///</summary>
	private void updateContentDisplay(){
		_title.text = _experienceInfo.getName();
		_info.text = _experienceInfo.getInfo();
		_thumbURLs = new string[_experienceInfo.getThumbIDs().Length];
		
		// send messages to the server to get the thumbnail urls to download
		for(int i = 0; i < _thumbURLs.Length; i++)
		{
			string[] data = new string[1]{_experienceInfo.getThumbIDs()[i]};
			JSONObject requestData = ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_THUMBNAIL_URL_REQUEST_FIELDS,data);
			_socket.Emit(Constants.CAREGIVER_THUMBNAIL_URL_REQUEST_MESSAGE, requestData);
		}
	}


	///<summary>
	/// receive the thumbnail url from the server
	///</summary>
	private void thumbnailURLReceived(SocketIOEvent ev)
	{
		//set the thumbnail url
		ev.data.GetField(ref _thumbURLs[_thumbURLCount], Constants.THUMBNAIL_URL_FIELD);
		_thumbURLCount++;

		// if we have all the urls, send them to the thumbnail manager
		if(_thumbURLCount == _thumbURLs.Length)
		{
			_thumb.switchExperience(_thumbURLs);
		}
	}

	///<summary>
	/// send message to the server to send the selected environment/experience to the patient
	///</summary>
	public void requestExperience()
	{
		string[] data = new string[3]{_experienceInfo.getID(), _experienceInfo.getSceneName(), _experienceInfo.getName()};
		JSONObject requestData = ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_EXPERIENCE_REQUEST_FIELDS, data);
		_socket.Emit(Constants.CAREGIVER_EXPERIENCE_REQUEST_MESSAGE, requestData);
		_ui.nextUI();
		
	}
}
