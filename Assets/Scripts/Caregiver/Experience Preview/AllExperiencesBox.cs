using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// the container that displays the Experiences on the server
///</summary>
public class AllExperiencesBox : TagAndContentBox<ExperienceInfoBundle> {

	[SerializeField]
	private ExperiencePreviewController _preview; 


	private void handleClick(ExperienceInfoBundle val)
	{
		_preview.selectNewExperience(val);
	}

	/// <summary>
	/// wait for animation library to finish downloading and then set it
	/// </summary>
	protected override IEnumerator InitializeLibrary()
	{
		_click = new ProcessClick<ExperienceInfoBundle>(handleClick);
		string[] tags = ExperiencesManifest.Instance.getTags();
		Dictionary<string, List<ExperienceInfoBundle>> library = new Dictionary<string, List<ExperienceInfoBundle>>();

		foreach (string tag in tags)
			library.Add(tag, new List<ExperienceInfoBundle>(ExperiencesManifest.Instance.getExperiencesUnderTag(tag)));

		_library = library;
		filterContent(_input.text);
		yield return null;
	}

}

