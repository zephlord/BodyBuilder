using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

///<summary>
/// The Experiences Manifest holds all the info for the environments/experiences available on the server.
/// The manifest is sorted like so
///   {
///   allTags:[tags under which experiences are sorted],
///	  tag_0:[experienceRefID_0, experienceRefID_2, ..., experienceRefID_M],
///   tag_1: [experienceRefID_3, experienceRefID_5, ..., experienceRefID_Q],
///   ...
///   tag_N: [experienceRefID_0, experienceRefID_4, ..., experienceRefID_P],
///   experienceRefID_0:{name, sceneName, info, thumbs:[thumbnailID_0, thumbnailID_1,..., thumbnailID_Y], IDinServer},
///   experienceRefID_1:{name, sceneName, info, thumbs:[thumbnailID_0, thumbnailID_1,..., thumbnailID_Z], IDinServer},
///   ...
///   experienceRefID_A:{name, sceneName, info, thumbs:[thumbnailID_0, thumbnailID_1,..., thumbnailID_C], IDinServer},
///   }
///</summary>
public class ExperiencesManifest : Singleton<ExperiencesManifest> {

	private JSONNode _manifest;
	private string _activeTag;
	private string[] _experiencesUnderTag;
	public JSONNode Manifest
	{
		get{ return _manifest;}
		set{ _manifest = JSON.Parse(value);}
	}

	///<summary>
	/// get all the tags under which experiences are sorted
	///</summary>
	///<returns> a list of strings representing all the tags</returns>
	public string[] getTags()
	{
		JSONNode tags = _manifest[Constants.MANIFEST_TAG_LIST_FIELD];
		string[] tagArray = new string[tags.Count];
		for(int i = 0; i < tags.Count; i++)
			tagArray[i] = tags[i];

		return tagArray;
	}

	///<summary>
	///get the environments/experiences sotred under a selected tag
	///</summary>
	///<param name="tag">the selected tag</param>
	///<returns>a list of ExperienceInfoBundle 
	///containing the info around the experiences under the selected tag</returns>
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

	///
	public ExperienceInfoBundle getExperience(string contentID)
	{
		return new ExperienceInfoBundle(_manifest[contentID]);
	}

	public string getFiledInExperience(string contentID, string field)
	{
		return _manifest[contentID][field];
	}
}
