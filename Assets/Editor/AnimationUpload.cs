using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using SimpleJSON;
using System.IO;
using UMA.AssetBundles;
using UMA;


public class AnimationUpload : EditorWindow {

	protected Object _animDir;
	protected BuildTarget[] _buildTarg;
	protected Dictionary<string, CMU_mocapData> _tagLib;
	protected Dictionary<string, CMU_mocapData> _renamedTagLib;
	protected Dictionary<string, string> _renameDict;
	protected AnimationRenamer _renamer;
	protected bool _doRename;
	protected List<AnimationClip> _clips;
	protected string _uploadFailedDir;
	protected CMU_mocap _inputSheet;

	[MenuItem("Assets/Animation/Upload Animation Library")]
    static void Init()
    {
        AnimationUpload window = ScriptableObject.CreateInstance<AnimationUpload>();
		
        //window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
		window.title = "Animation Upload Tool";
		window.ShowUtility();
    }

	[ExecuteInEditMode]
	void OnGUI()
	{
		if(_animDir == null)
			_animDir = new Object();
		if(_tagLib == null)
			_tagLib = new Dictionary<string, CMU_mocapData>();
		if(_renamedTagLib == null)
			_renamedTagLib = new Dictionary<string, CMU_mocapData>();
		if(_renamer == null)
			_renamer = new AnimationRenamer();
		if(_buildTarg == null)
			_buildTarg = new BuildTarget[0];

		_animDir = EditorGUILayout.ObjectField("Animation Directory", _animDir, typeof(Object), true);
		if(GUILayout.Button("Select unuploaded Animation directory."))
			_uploadFailedDir = EditorUtility.OpenFolderPanel("Unuploaded Anims", "", "");
        
		EditorGUILayout.LabelField("chosen unuploaded directory: " + _uploadFailedDir);
		
		
		_inputSheet = EditorGUILayout.ObjectField("Information Sheet", _inputSheet, typeof(CMU_mocap), true) as CMU_mocap;
		int numTargets = EditorGUILayout.DelayedIntField("How many Build Targets?", _buildTarg.Length);
		if(numTargets != _buildTarg.Length)
			UtilityFunctions.transferData<BuildTarget>(numTargets, ref _buildTarg);
		EditorGUI.indentLevel++;
		for(int i = 0; i < _buildTarg.Length; i++)
			_buildTarg[i] = (BuildTarget) EditorGUILayout.EnumPopup("Platform", _buildTarg[i]);
		EditorGUI.indentLevel--;
		_doRename = EditorGUILayout.Toggle("Rename animation files before upload?", _doRename);


		if(GUILayout.Button("Upload"))
		{
			if(_animDir != null && !UtilityFunctions.containsBlank<BuildTarget>(_buildTarg))
				buildAnimationAssets();
			else
				EditorUtility.DisplayDialog("Problem with upload config",
					"Please fill out all fields before uploading", "OK");
		}
	}
	
	void buildAnimationAssets()
	{
		buildTagLibrary();
		string dataPath = Application.dataPath;
		dataPath = dataPath.Substring(0, dataPath.Length - 6);
		string[] animFiles = Directory.GetFiles(dataPath + AssetDatabase.GetAssetPath(_animDir), "*.anim", SearchOption.AllDirectories);

		_renameDict = new Dictionary<string,string>();
		_clips = new List<AnimationClip>();
		foreach(string path in animFiles)
		{
			string assetPath = "Assets" + path.Replace(Application.dataPath, "").Replace('\\', '/');
			string fileName = Path.GetFileNameWithoutExtension(path);
			AnimationClip clip = (AnimationClip) AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimationClip));
			if(clip != null)
			{
				if(_doRename)
				{
					clip.name = renameClip(fileName);
					_renameDict.Add(fileName, clip.name);
				}
				else
				{
					CMU_mocapData data = _tagLib[clip.name];
					clip.name = _renamer.quickRename(clip.name);
					_renamedTagLib.Add(clip.name, data);
				}
				if(clip.name.Contains("/"))
				{
					failedToUpload(assetPath, true);
					continue;
				}
				AssetImporter.GetAtPath(assetPath).assetBundleName = clip.name.ToLower();
				_clips.Add(clip);
			}
		}

		if(_doRename)
			_tagLib = _renamedTagLib;
		
		// foreach(AnimationClip clip in clips)
		// {
		// 	if(string.IsNullOrEmpty(AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip)).assetBundleName))
		// 		AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip)).assetBundleName = clip.name.ToLower();
		// }
		
		buildBundles();

		List<string> animationBundlesToUpload = new List<string>();
		foreach(AnimationClip clip in _clips)
		{
			if(_doRename)
			{
				if(_renameDict.ContainsKey(clip.name))
				{
					animationBundlesToUpload.Add(_renameDict[clip.name]);
				}
				else
					animationBundlesToUpload.Add(clip.name.ToLower());
				
			}
			else			
				animationBundlesToUpload.Add(clip.name.ToLower());
			
		}
		beginUpload(animationBundlesToUpload);
		this.Close();
	}

	void buildBundles()
	{
		foreach(BuildTarget targ in _buildTarg)
		{
			string outputPath = Path.Combine(UMA.AssetBundles.Utility.AssetBundlesOutputPath,  UMA.AssetBundles.Utility.GetPlatformName(targ));
			if (!Directory.Exists(outputPath) )
				Directory.CreateDirectory (outputPath);
			
			BuildPipeline.BuildAssetBundles (outputPath, BuildAssetBundleOptions.None, targ);
		}
	}


	void beginUpload(List<string> bundles) 
	{

		

		WWWForm uploadForm = new WWWForm();
		
		uploadForm.AddField(Constants.UPLOAD_ANIMATION_NUMBER_OF_BUNDLES_FIELD, bundles.Count);
		string platformsString = "{\"platforms\":[";
		foreach(BuildTarget targ in _buildTarg)
			platformsString += "\"" + targ.ToString() + "\",";
		platformsString = platformsString.Substring(0, platformsString.Length - 1) + "]}";

		uploadForm.AddField(Constants.UPLOAD_ANIMATION_BUNDLE_PLATFORM_FIELD, platformsString);

		for(int i = 0; i < bundles.Count; i++)
		{
			string tagsString = "";
			string currentBundle = bundles[i];
			List<string> dictKeys = new List<string>(_tagLib.Keys);
			bool containsCurrentBundle = _tagLib.ContainsKey(currentBundle);
			if(containsCurrentBundle)
			{
				tagsString = "{\"tags\":[";
				for(int j = 0; j < _tagLib[bundles[i]].TAGS.Length; j++)
					tagsString += "\"" + _tagLib[bundles[i]].TAGS[j] + "\",";

				tagsString = tagsString.Substring(0, tagsString.Length - 1) + "]}";

				
			}

			else
			{
				failedToUpload(bundles[i], false);
				continue;
			}

			uploadForm.AddField(Constants.UPLOAD_ANIMATION_BUNDLE_NAME_FIELD + i, bundles[i]);
			uploadForm.AddField(Constants.UPLOAD_ANIMATION_BUNDLE_TAGS_FIELD + i, tagsString);
			foreach(BuildTarget targ in _buildTarg)
			{
				string outPath = Path.Combine(UMA.AssetBundles.Utility.AssetBundlesOutputPath,  UMA.AssetBundles.Utility.GetPlatformName(targ));
				uploadForm.AddBinaryData(targ.ToString() + Constants.UPLOAD_ANIMATION_BUNDLE_FIELD + i, File.ReadAllBytes(outPath + "/" + bundles[i]));
			}
			
			
		}
		

		string uri = Constants.SERVER_URL + Constants.SERVER_API_UPLOAD_ANIMATION;
		UnityWebRequest www = UnityWebRequest.Post(uri, uploadForm);
		www.Send();
		ContinuationManager.Run(() => www.isDone, () =>
		{
			if(www.isError) 
			{
				Debug.Log(www.error);
			}
			else 
			{
				JSONNode data = JSONNode.Parse(www.downloadHandler.text);
				JSONNode errors = data["error"];
				Debug.Log("animations for " + _buildTarg.ToString() + " uploaded!");
			}
		});
		
	}

	void buildTagLibrary()
	{
		foreach(CMU_mocapData row in _inputSheet.dataArray)
		{
			if(!_tagLib.ContainsKey(row.FILENAME))
			{
				if(_doRename)
					_tagLib.Add(row.FILENAME, row);
				else
					_tagLib.Add(row.DISPLAYNAME, row);
			}
				
			else
				Debug.Log("duplicate entry for file " + row.FILENAME);
		}
	}

	string renameClip(string oldName)
	{
		if(_tagLib.ContainsKey(oldName))
		{
			CMU_mocapData data = _tagLib[oldName];
			string newName = _renamer.rename(oldName, data.DISPLAYNAME);
			_renamedTagLib.Add(newName, data);
			return newName;
		}
		else
		{
			Debug.Log(oldName + " not listed in sheet");
			string newName = _renamer.rename(oldName, oldName);
			return newName;
		}
	}

	void failedToUpload(string file, bool isPath)
	{
		AnimationClip oldClip = null;
		if(!isPath)
		{
			foreach(AnimationClip clip in _clips)
			{
				if(clip.name == file ||
					(_renameDict.ContainsKey(clip.name) && _renameDict[clip.name] == file))
				{
					oldClip = clip;
					break;
				}
			}
		}
		else
		{
			oldClip = (AnimationClip) AssetDatabase.LoadAssetAtPath(file, typeof(AnimationClip));
		}

		if(oldClip == null)
			return;
		string assetPath = AssetDatabase.GetAssetPath(oldClip);
		string filePath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
		if(Application.platform.ToString().Contains("Windows"))
			filePath = filePath.Replace("/", "\\");

		string fileName = Path.GetFileName(assetPath);

		string newAnimPath = _uploadFailedDir + "/" + fileName;
		if(Application.platform.ToString().Contains("Windows"))
			newAnimPath = newAnimPath.Replace("/", "\\");
		
		File.Copy(filePath, newAnimPath);
	}

}
