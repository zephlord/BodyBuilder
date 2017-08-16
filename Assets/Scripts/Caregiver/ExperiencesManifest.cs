using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ExperiencesManifest : Singleton<ExperiencesManifest> {

	private JSONNode _manifest;
	private string _activeTag;
	private string[] _experiencesUnderTag;
	public JSONNode Manifest
	{
		get{ return _manifest;}
		set{ _manifest = JSON.Parse(value);}
	}

	public string[] getTags()
	{
		JSONNode tags = _manifest[Constants.MANIFEST_TAG_LIST_FIELD];
		string[] tagArray = new string[tags.Count];
		for(int i = 0; i < tags.Count; i++)
			tagArray[i] = tags[i];

		return tagArray;
	}

	public ExperienceInfoBundle[] getExperiencesUnderTag(string tag)
	{
		
		JSONNode experiences = _manifest[Constants.MANIFEST_TAGS_FIELD][tag];
		ExperienceInfoBundle[] experiencesArray = new ExperienceInfoBundle[experiences.Count];
		for(int i = 0; i < experiences.Count; i++)
		{
			string infoID = experiences[i];
			experiencesArray[i] = new ExperienceInfoBundle(_manifest[infoID]);
		}

		return experiencesArray;
	}

	public ExperienceInfoBundle getExperience(string contentID)
	{
		return new ExperienceInfoBundle(_manifest[contentID]);
	}

	public string getFiledInExperience(string contentID, string field)
	{
		return _manifest[contentID][field];
	}
}
