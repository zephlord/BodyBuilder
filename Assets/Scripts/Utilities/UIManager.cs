using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	[System.Serializable]
	public struct uiSet
	{
		public GameObject obj;
		public Vector3 avatarPos;
		public bool needsAvatar;
	}

	[SerializeField]
	private uiSet[] _UIs;
	[SerializeField]
	private GameObject _avatar;
	private int _index;
	void Start () {
		foreach(uiSet ui in _UIs)
			ui.obj.SetActive(false);
		_index = 0;
		_UIs[_index].obj.SetActive(true);
	}
	
	public void nextUI()
	{
		_UIs[_index].obj.SetActive(false);
		_index = Mathf.Min( _index +1, _UIs.Length - 1);
		_UIs[_index].obj.SetActive(true);
		setAvatar (_UIs [_index]);
	}

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
