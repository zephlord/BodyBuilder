using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

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
	public void selectNewExperience(ExperienceInfoListBox selected)
	{
		_thumbURLCount = 0;
		_experienceInfo = selected.content;
		_experienceInfoUI.SetActive(true);
		updateContentDisplay();
		
	}

	private void updateContentDisplay(){
		_title.text = _experienceInfo.getName();
		_info.text = _experienceInfo.getInfo();
		_thumbURLs = new string[_experienceInfo.getThumbIDs().Length];
		for(int i = 0; i < _thumbURLs.Length; i++)
		{
			string[] data = new string[1]{_experienceInfo.getThumbIDs()[i]};
			JSONObject requestData = ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_THUMBNAIL_URL_REQUEST_FIELDS,data);
			_socket.Emit(Constants.CAREGIVER_THUMBNAIL_URL_REQUEST_MESSAGE, requestData);
		}
	}


	private void thumbnailURLReceived(SocketIOEvent ev)
	{
		ev.data.GetField(ref _thumbURLs[_thumbURLCount], Constants.THUMBNAIL_URL_FIELD);
		_thumbURLCount++;
		if(_thumbURLCount == _thumbURLs.Length)
		{
			_thumb.switchExperience(_thumbURLs);
		}
	}

	public void requestExperience()
	{
		string[] data = new string[3]{_experienceInfo.getID(), _experienceInfo.getSceneName(), _experienceInfo.getName()};
		JSONObject requestData = ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_EXPERIENCE_REQUEST_FIELDS, data);
		_socket.Emit(Constants.CAREGIVER_EXPERIENCE_REQUEST_MESSAGE, requestData);
		_ui.nextUI();
		
	}
}
