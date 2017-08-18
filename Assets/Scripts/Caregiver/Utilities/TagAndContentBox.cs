using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// the container that displays tags and items under said tags
///</summary>
public abstract class TagAndContentBox<T> : MonoBehaviour {

	protected delegate void ProcessClick<T>(T content);

	protected List<T> _content;
	protected List<string> _tags;
	protected Dictionary<string, List<T>> _library;
	[SerializeField]
	protected InputField _input;
	[SerializeField]
	protected ScrollRect _contentView;
	[SerializeField]
	protected GameObject _buttonPrefab;
	[SerializeField]
	protected GameObject _backToTagsButton;
	protected string _currentTag;
	protected bool _hasValChanged = false;
	protected ProcessClick<T> _click;

	void Start()
	{
		_library = new Dictionary<string, List<T>>();
		StartCoroutine(InitializeLibrary());
	}

	void OnGUI()
	{
		// if the content has changed or been filtered,
		if(_hasValChanged)
		{
			// remove all old items from the content
			foreach(Transform child in _contentView.content.transform) 
    			Destroy(child.gameObject);

			if(_currentTag != null)
			{
				// set the new items
				foreach(T val in _content)
				{
					GameObject button = Instantiate(_buttonPrefab,_contentView.content.transform, false);
					button.transform.GetChild(0).GetComponent<Text>().text = val.ToString();
					button.GetComponent<Button>().onClick.AddListener(() => 
					{
						_click(val);
					});
				}
			}

			else
			{
				// set the new items
				foreach(string val in _tags)
				{
					GameObject button = Instantiate(_buttonPrefab,_contentView.content.transform, false);
					button.transform.GetChild(0).GetComponent<Text>().text = val;
					button.GetComponent<Button>().onClick.AddListener(() => 
					{
						selectTag(val);
					});
				}
			}


			// the back button is only visable if we have set a tag
			_backToTagsButton.SetActive(_currentTag != null);
			_hasValChanged = false;
		}
	}

	private void setTags(List<string> tags)
	{
		_tags = tags;
	}

		private void setContent(List<T> content)
	{
		_content = content;
	}

	/// <summary>
	/// setup the library
	/// </summary>
	protected abstract IEnumerator InitializeLibrary();


	/// <summary>
	/// display only the things that contain the input
	/// </summary>
	public void filterContent(string input)
	{
		// if we are viewing the tags, then filter on tags
		if(_currentTag == null)
		{
			List<string> content = new List<string>();
			List<string> contentStrings = new List<string>(_library.Keys);
			foreach(string val in contentStrings)
			{
				if(val.ToLower().Contains(input.ToLower()))
					content.Add(val);
			}
			setTags(content);
		}

		// if we are viewing animations under a tag, filter on animations
		else
		{
			List<T> content = new List<T>();
			List<T> tempList = new List<T>(_library[_currentTag]);
			foreach(T val in tempList)
			{
				if(val.ToString().ToLower().Contains(input.ToLower()))
					content.Add(val);
			}
			setContent(content);
		}

		_hasValChanged = true;
	}

	/// <summary>
	/// select a tag to view animations under
	/// </summary>
	public void selectTag(string tag)
	{
		_currentTag = tag;
		_input.text = "";
		filterContent(_input.text);
		
	}

	/// <summary>
	/// deselect a tag
	/// </summary>
	public void resetTag()
	{
		_currentTag = null;
		_input.text = "";
		filterContent(_input.text);
	}

}

