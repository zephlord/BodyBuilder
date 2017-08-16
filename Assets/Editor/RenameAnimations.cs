using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UniRx;
public class RenameAnimations : EditorWindow {

	protected CMU_mocap _nameData;
	protected UnityEngine.Object _animFolderDir;
	protected Dictionary<string, Tuple<string, string[]>> _renameDict;
	protected AnimationRenamer _renamer;
	[MenuItem("Assets/Animation/Rename Animations")]
    static void Init()
    {
        RenameAnimations window = ScriptableObject.CreateInstance<RenameAnimations>();
        //window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
		window.title = "Rename Animations Based on Excel Sheet";
		window.ShowUtility();
    }

	[ExecuteInEditMode]
	void OnGUI()
	{
		if(_animFolderDir == null)
			_animFolderDir = new UnityEngine.Object();
		if(_renamer == null)
			_renamer = new AnimationRenamer();

		_animFolderDir = EditorGUILayout.ObjectField(".anim Directory", _animFolderDir, typeof(UnityEngine.Object), true);
		_nameData = EditorGUILayout.ObjectField("Renaming Sheet", _nameData, typeof(CMU_mocap), true) as CMU_mocap;

		if(GUILayout.Button("Rename"))
		{
			if(_nameData != null && _animFolderDir != null)
				batchRename();
			else
				EditorUtility.DisplayDialog("Problem with upload config",
					"Please fill out all fields before uploading", "OK");
		}
	}

	void rename(string path)
	{
		string dataPath = Application.dataPath;
		dataPath = dataPath.Substring(0, dataPath.Length - 6);
		string assetPath = path.Replace(dataPath, "");
		string fileName = Path.GetFileNameWithoutExtension(path);
		string newFileName = "";
		if(!_renameDict.ContainsKey(fileName))
			newFileName = _renamer.rename(fileName, fileName);		
		else
			newFileName = _renamer.rename(fileName, _renameDict[fileName].Item1);
		AnimationClip clip = (AnimationClip) AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimationClip));
		clip.name = newFileName;
		//AssetDatabase.RenameAsset(assetPath, newFileName);
	}

	void batchRename()
	{
		setupRenameDict();
		string dataPath = Application.dataPath;
		dataPath = dataPath.Substring(0, dataPath.Length - 6);
		string[] animFiles = Directory.GetFiles(dataPath + AssetDatabase.GetAssetPath(_animFolderDir), "*.anim", SearchOption.AllDirectories);
		foreach(string path in animFiles)
			rename(path);

	}

	void setupRenameDict()
	{
		_renameDict = new Dictionary<string, Tuple<string, string[]>>();
		for(int i = 0; i < _nameData.dataArray.Length; i++)
		{
			Tuple<string, string[]> data = new Tuple<string, string[]>(
							_nameData.dataArray[i].DISPLAYNAME,
							_nameData.dataArray[i].TAGS);
			
			if(!_renameDict.ContainsKey(_nameData.dataArray[i].FILENAME))
				_renameDict.Add(_nameData.dataArray[i].FILENAME, data);
		}
	}
}
