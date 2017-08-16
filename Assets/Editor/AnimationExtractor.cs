using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationExtractor : EditorWindow {

	protected Object _animInputDir;
	protected Object _animOutputDir;

	[MenuItem("Assets/Animation/Extract Animation from FBX")]
    static void Init()
    {
        AnimationExtractor window = ScriptableObject.CreateInstance<AnimationExtractor>();
        //window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
		window.title = "Extract Animation Clip From FBX Tool";
		window.ShowUtility();
    }

	[ExecuteInEditMode]
	void OnGUI()
	{
		if(_animInputDir == null)
			_animInputDir = new Object();

		_animInputDir = EditorGUILayout.ObjectField("FBX Directory", _animInputDir, typeof(Object), true);
		_animOutputDir = EditorGUILayout.ObjectField("Output Animation Directory", _animOutputDir, typeof(Object), true);

		if(GUILayout.Button("Extract"))
		{
			if(_animInputDir != null && _animOutputDir != null)
				extractAnimations();
			else
				EditorUtility.DisplayDialog("Problem with upload config",
					"Please fill out all fields before uploading", "OK");
		}
	}
	
	void extractAnimations()
	{

		string dataPath = Application.dataPath;
		dataPath = dataPath.Substring(0, dataPath.Length - 6);
		string[] animFiles = Directory.GetFiles(dataPath + AssetDatabase.GetAssetPath(_animInputDir), "*.fbx", SearchOption.AllDirectories);

		foreach(string path in animFiles)
		{
			saveAnimFile(path);
		}
		
		AssetDatabase.SaveAssets();
		this.Close();
	}

	void saveAnimFile(string path)
	{
		string assetPath = "Assets" + path.Replace(Application.dataPath, "").Replace('\\', '/');
			
		UnityEngine.Object[] data;
		data = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
		for (int i = 0; i < data.Length; ++i) 
		{
			if (data[i].GetType() == typeof(AnimationClip))
			{
				AnimationClip originalClip = data [i] as AnimationClip;
				string newAnimPath = AssetDatabase.GetAssetPath(_animOutputDir) + "/" + originalClip.name + ".anim";
				SerializedObject serializedClip = new SerializedObject(originalClip);
				AnimationClip newClip = new AnimationClip();
				if( !Resources.Load(newAnimPath) )
				{
					EditorUtility.CopySerialized(originalClip,newClip);
					AssetDatabase.CreateAsset(newClip,newAnimPath);
					AssetDatabase.Refresh();
				}
			}
		}
	}
}
