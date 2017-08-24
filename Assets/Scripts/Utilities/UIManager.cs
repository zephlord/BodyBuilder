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
	

	private void swapUI(int newIndex)
	{
		_UIs[_index].obj.SetActive(false);
		_index = newIndex;
		_UIs[_index].obj.SetActive(true);
		setAvatar(_UIs[_index]);
	}

	public void nextUI()
	{
		swapUI(Mathf.Min( _index +1, _UIs.Length - 1));
	}

	public void previousUI()
	{
		swapUI(Mathf.Max( _index - 1, 0));
	}

	public void goToUI(int uiIndex)
	{
		swapUI(uiIndex);
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
