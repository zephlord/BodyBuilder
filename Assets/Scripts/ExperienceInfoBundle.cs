using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

///<summary>
/// a container for information on an environment stored in the server
///</summary>
public class ExperienceInfoBundle {

	// the ID on the server
	private string assetID;
	// the environment name
	private string name;
	// the thumbnail urls
	private string[] thumbs;
	// the description
	private string info;
	// the name of the scene to be loaded
	private string sceneName;

	public ExperienceInfoBundle(JSONNode infoNode)
	{
		assetID = infoNode[Constants.MANIFEST_EXPERIENCE_ID].Value;
		name = infoNode[Constants.MANIFEST_EXPERIENCE_NAME].Value;
		info = infoNode[Constants.MANIFEST_EXPERIENCE_SUMMARY].Value;
		sceneName = infoNode[Constants.MANIFEST_EXPERIENCE_SCENE_NAME].Value;
		JSONArray thumbArray = infoNode[Constants.MANIFEST_EXPERIENCE_THUMBNAILS].AsArray;
		thumbs = new string[thumbArray.Count];
		for(int i = 0; i < thumbArray.Count; i++)
			thumbs[i] = thumbArray[i].Value;
	}

	public override string ToString()
	{
		return name;
	}

	public string getName(){
		return name;
	}

	public string getInfo(){
		return info;
	}

	public string getID(){
		return assetID;
	}

	public string[] getThumbIDs(){
		return thumbs;
	}
	public string getSceneName(){
		return sceneName;
	}
}
