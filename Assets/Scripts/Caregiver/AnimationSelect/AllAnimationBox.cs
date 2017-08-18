using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// the container that displays the animation library
///</summary>
public class AllAnimationBox : TagAndContentBox<string> {


	[SerializeField]
	private AnimationSelectManager _handler;
	[SerializeField]
	private AnimationLibrary _lib;

	private void handleClick(string val)
	{
		_handler.animPreview(val);
	}

	/// <summary>
	/// wait for animation library to finish downloading and then set it
	/// </summary>
	protected override IEnumerator InitializeLibrary()
	{
		_click = new ProcessClick<string>(handleClick);
		while(!_lib.isLibraryDownloaded())
			yield return null;
		
		_library = _lib.getLibraryManifest();
		filterContent(_input.text);
	}

}

