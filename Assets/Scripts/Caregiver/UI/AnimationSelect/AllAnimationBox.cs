using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AllAnimationBox : MonoBehaviour {


	[SerializeField]
	private AnimationSelectManager _handler;
	[SerializeField]
	private AnimationLibrary _lib;
	private List<string> _content;
	private Dictionary<string, List<string>> _library;
	[SerializeField]
	private InputField _input;
	[SerializeField]
	private ScrollRect _contentView;
	[SerializeField]
	private GameObject _buttonPrefab;
	[SerializeField]
	private GameObject _backToTagsButton;
	private string _currentTag;
	
	private bool _hasValChanged = false;

	void Start()
	{
		_library = new Dictionary<string, List<string>>();
		StartCoroutine(InitializeLibrary());
	}

	void OnGUI()
	{
		if(_hasValChanged)
		{
			foreach(Transform child in _contentView.content.transform) 
    			Destroy(child.gameObject);


			foreach(string val in _content)
			{
				GameObject button = Instantiate(_buttonPrefab,_contentView.content.transform, false);
				button.transform.GetChild(0).GetComponent<Text>().text = val;
				if(_currentTag != null)
				{
					button.GetComponent<Button>().onClick.AddListener(() => 
					{
							_handler.animPreview(val);
					});
				}
				else
				{
					button.GetComponent<Button>().onClick.AddListener(() => 
					{
							selectTag(val);
					});
				}
			}

			_backToTagsButton.SetActive(_currentTag != null);
			_hasValChanged = false;
		}
	}

	private void setContent(List<string> content)
	{
		_content = content;
	}

	IEnumerator InitializeLibrary()
	{
		while(!_lib.isLibraryDownloaded())
			yield return null;
		
		_library = _lib.getLibraryManifest();
		filterContent(_input.text);
	}

	public void filterContent(string input)
	{
		List<string> content = new List<string>();
		List<string> contentStrings;
		if(_currentTag == null)
			contentStrings = new List<string>(_library.Keys);

		else
			contentStrings = new List<string>(_library[_currentTag]);

		foreach(string val in contentStrings)
		{
			if(val.ToLower().Contains(input.ToLower()))
				content.Add(val);
		}
		_hasValChanged = true;
		setContent(content);
	}

	public void selectTag(string tag)
	{
		_currentTag = tag;
		_input.text = "";
		filterContent(_input.text);
		
	}

	public void resetTag()
	{
		_currentTag = null;
		_input.text = "";
		filterContent(_input.text);
	}

}

