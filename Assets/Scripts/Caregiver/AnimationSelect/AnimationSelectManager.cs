using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using SocketIO;

///<summary>
/// The manager for selecting animations to send to the patient
///</summary>
public class AnimationSelectManager : MonoBehaviour {

	[SerializeField]
	private SocketIOComponent _socket;
	[SerializeField]
	private AnimationDurationSelectionManager _durationSelection;
	[SerializeField]
	private GameObject _animSelectionPrefab;
	private List<GameObject> _animSelections;	
	[SerializeField]
	private ReorderableList _selectionListView;
	[SerializeField]
	private GameObject _selectAnimUI;
	[SerializeField]
	private GameObject _setAnimDurationUI;
	private GameObject _isEditing;
	[SerializeField]
	private UIManager _ui;
	[SerializeField]
	private AnimationTimer _timer;
	void Start () {
		_animSelections = new List<GameObject>();
		_socket.On(Constants.CAREGIVER_DOWNLOAD_ANIMATION_RECEIVE_URL_MESSAGE, setAnimPreview);
	}
	

	///<summary>
	/// toggle the ui on and off
	///</summary>
	public void toggleUI()
	{
		toggleSelectAnimation(!_selectAnimUI.activeSelf);
	}

	///<summary>
	/// set the animation preview and go to the duration selection
	///</summary>
	///<param name="anim">the animation to be previewed</param>
	public void animPreview(AnimationClip anim)
	{
		toggleSelectAnimation(false);
		_durationSelection.setAnimation(anim);
	}

	///<summary>
	/// Send a message to the server to get the url of the selected animation to download
	///</summary>
	///<param name="animName">the name of the animation to be downloaded</param>
	public void animPreview(string animName)
	{
		string[] animNamePackage = new string[1] {animName};
		JSONObject data = ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_DOWNLOAD_ANIMATION_REQUEST_FIELDS, animNamePackage);
		_socket.Emit(Constants.CAREGIVER_DOWNLOAD_ANIMATION_MESSAGE, data);
	}

	///<summary>
	/// get the url and name of the animation to download after server responds
	///</summary>
	void setAnimPreview(SocketIOEvent ev)
	{
		string url = "";
		string animName = "";
		ev.data.GetField( ref url, "url");
		ev.data.GetField( ref animName, "animName");
		toggleSelectAnimation(false);
		_durationSelection.setAnimation(url, animName);
	}

	///<summary>
	///edit the duration of an animation
	///</summary>
	///<param name="item">the animation duration item that contains the info to edit</param>
	public void editAnimDuration(AnimDurationSelectionItem item)
	{
		_durationSelection.setAnimation(item.getAnim(), item.getMins(), item.getSecs());
		toggleSelectAnimation(false);
		_isEditing = item.gameObject;
	}

	///<summary>
	/// cancel selecting the duration. Turn back on the all anim box
	///</summary>
	public void cancelAnimDurationSelection()
	{
		toggleSelectAnimation(true);
		_isEditing = null;
	}

	///<summary>
	/// an animation was selected, so set the animation and its duration
	///</summary>
	///<param name="mins">the selected minutes for the animation to play</param>
	///<param name="secs">the selected seconds fot the animation to play</param>
	///<param name="anim">the animation to play</param>
	public void selectAnimation(float mins, float seconds, AnimationClip anim)
	{
		toggleSelectAnimation(true);
		GameObject animSelection;

		// if we were not editing a selected animation, create a new selection
		if(_isEditing == null)
			animSelection = Instantiate(_animSelectionPrefab, _selectionListView.Content.transform, false);
		// else we set the selection to the container of the info we were editing
		else
			animSelection = _isEditing;
		AnimDurationSelectionItem item = animSelection.GetComponent<AnimDurationSelectionItem>();
		item.setInfo(mins, seconds, anim);

		// if we weren't editing, then we set the new click items
		if(_isEditing == null)
		{
			item.setOnClickAnim(()=> editAnimDuration(item));
			item.setOnClickDelete(()=> removeSelection(animSelection));
			_animSelections.Add(animSelection);
		}
		
		_isEditing = null;
	}

	void toggleSelectAnimation(bool selectOn)
	{
		_selectAnimUI.SetActive(selectOn);
		_setAnimDurationUI.SetActive(!selectOn);
	}

	void removeSelection(GameObject item)
	{
		_animSelections.Remove(item);
		Destroy(item);
	}


	// allows for the animation selection items to be reordered
	public void reorderAnimation(ReorderableList.ReorderableListEventStruct ev)
	{
		Transform contentTransform = ev.ToList.Content.gameObject.transform;
		List<GameObject> newList = new List<GameObject>();
		for(int i = 0; i < contentTransform.GetChildCount(); i++)
		{
			GameObject obj = contentTransform.GetChild(i).gameObject;
			if(obj.GetComponent<AnimDurationSelectionItem>() != null)
				newList.Add(obj);
		}
			
		_animSelections = newList;
	}

	///<summary>
	/// sequence and send the animations and their duration to the patient
	///</summary>
	public void sendAnimations()
	{
		// setup our labels and data
		List<string> keys = new List<string>();
		List<string> vals = new List<string>();

		// add how many animations we are sending
		keys.Add("animNum");
		vals.Add(_animSelections.Count.ToString());

		// set the duration and name of each animation being sent in the sequence
		float totalAnimTime = 0;
		for(int i = 0; i < _animSelections.Count; i++)
		{
			AnimDurationSelectionItem item = _animSelections[i].GetComponent<AnimDurationSelectionItem>();
			keys.Add(Constants.ANIMATION_FIELD_PREFIX + i);
			vals.Add(item.getAnim().name.ToLower());
			keys.Add(Constants.ANIMATION_FIELD_PREFIX + i + Constants.ANIMATION_DURATION_FIELD_SUFFIX);
			float animDuration = item.getMins() * 60 + item.getSecs();
			totalAnimTime += animDuration;
			vals.Add(animDuration.ToString());
		}

		JSONObject data = ServerCommsUtility.Instance.serializeData(keys.ToArray(), vals.ToArray());
		_socket.Emit(Constants.CAREGIVER_SEND_ANIMATIONS_MESSAGE, data);
		_timer.setTimer(totalAnimTime);
		_ui.previousUI();
	}

}
