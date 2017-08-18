using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// manages ui elements in a sequence
///</summary>
public class UIManager : MonoBehaviour {

	
	[System.Serializable]
	///<summary>
	/// a UI Set is a ui element
	/// and whether or not (and if so where) it needs the avatar to be shown while its UI is displayed
	///</summary>
	public struct uiSet
	{
		public GameObject obj;
		public Vector3 avatarPos;
		public bool needsAvatar;
	}

	// the UI elements
	[SerializeField]
	private uiSet[] _UIs;
	// The avatar
	[SerializeField]
	private GameObject _avatar;
	private int _index;
	void Start () {
		foreach(uiSet ui in _UIs)
			ui.obj.SetActive(false);
		_index = 0;
		_UIs[_index].obj.SetActive(true);
	}
	
	///<summary>
	///Goes to the next ui in the sequence
	///</summary>
	public void nextUI()
	{
		_UIs[_index].obj.SetActive(false);
		_index = Mathf.Min( _index +1, _UIs.Length - 1);
		_UIs[_index].obj.SetActive(true);
		setAvatar (_UIs [_index]);
	}

	///<summary>
	///Goes to the previous ui in the sequence
	///</summary>
	public void previousUI()
	{
		_UIs[_index].obj.SetActive(false);
		_index = Mathf.Max( _index - 1, 0);
		_UIs[_index].obj.SetActive(true);
		setAvatar (_UIs [_index]);
	}

	private void setAvatar(uiSet ui)
	{
		if (ui.needsAvatar) {
			_avatar.SetActive (true);
			_avatar.transform.position = ui.avatarPos;
		} else
			_avatar.SetActive (false);
	}
}
