using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

public class SendAssetBundleToServerWindow : EditorWindow {
	protected string assetBundleName;
	protected string sceneName;
	protected string[] tags;
	protected Texture2D[] thumbnails;
	protected string info;
	protected BuildTarget[] buildTarg;
	protected bool showTags;
	protected bool showThumbs;
	protected GUIStyle infoFieldStyle;
	private bool isUploading = false;

	[MenuItem("Assets/AssetBundles/Upload Asset Bundle")]
    static void Init()
    {
        SendAssetBundleToServerWindow window = ScriptableObject.CreateInstance<SendAssetBundleToServerWindow>();
        //window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
		window.title = "Asset Bundle Upload Tool";
		window.ShowUtility();
    }

	[ExecuteInEditMode]
    void OnGUI()
    {
		if(infoFieldStyle == null)
		{
			infoFieldStyle = new GUIStyle();
			infoFieldStyle.stretchHeight = true;
			infoFieldStyle.fixedWidth = 75;
			infoFieldStyle.wordWrap = true;
		}

		if(buildTarg == null)
			buildTarg = new BuildTarget[0];
		

        EditorGUILayout.LabelField("Set the AssetBundle to be uploaded", EditorStyles.wordWrappedLabel);
        GUILayout.Space(30);
		assetBundleName = EditorGUILayout.DelayedTextField("AssetBundle Name", assetBundleName);
		sceneName = EditorGUILayout.DelayedTextField("Scene Name", sceneName);

		if(tags == null)
			tags = new string[0];
		int tagLen = tags.Length;
		tagLen = EditorGUILayout.DelayedIntField("How Many Tags", tagLen);
		if(tagLen != tags.Length)
			UtilityFunctions.transferData<string>(tagLen, ref tags);


		showTags = EditorGUILayout.Foldout(showTags, "Tags");
		if(showTags)
		{
			EditorGUI.indentLevel++;
			for(int i = 0; i < tags.Length; i++)
			{
				tags[i] = EditorGUILayout.DelayedTextField("Tag"+i, tags[i]);
			}
			EditorGUI.indentLevel--;			
		}

		if(thumbnails == null)
			thumbnails = new Texture2D[0];
		int thumbLen = thumbnails.Length;
		thumbLen = EditorGUILayout.DelayedIntField("How Many Thumbnails", thumbLen);
		if(thumbLen != thumbnails.Length)
			UtilityFunctions.transferData<Texture2D>(thumbLen, ref thumbnails);
		
		showThumbs = EditorGUILayout.Foldout(showThumbs, "Thumbnails");
		if(showThumbs)
		{
			EditorGUI.indentLevel++;
			for(int i = 0; i < thumbnails.Length; i++)
			{
				thumbnails[i] = EditorGUILayout.ObjectField("Thumbnail"+i, thumbnails[i], typeof(Texture2D), true ) as Texture2D;
			}
			EditorGUI.indentLevel--;
		}

		int buildTargNum = EditorGUILayout.DelayedIntField("How many Build Targets?", buildTarg.Length);
		if(buildTargNum != buildTarg.Length)
			UtilityFunctions.transferData<BuildTarget>(buildTargNum, ref buildTarg);
		for(int i = 0; i < buildTarg.Length; i++)
			buildTarg[i] = (BuildTarget) EditorGUILayout.EnumPopup("Platform", buildTarg[i]);
		
		info = EditorGUILayout.TextField("Info", info, infoFieldStyle);

		if(GUILayout.Button("Upload!"))
		{
			if(thumbnails.Length == 0 ||
					sceneName == "" ||
					assetBundleName == "" ||
					tags.Length == 0 ||
					info == "" ||
					UtilityFunctions.containsBlank<BuildTarget>(buildTarg) ||
					UtilityFunctions.containsBlank<string>(tags) ||
					UtilityFunctions.containsBlank<Texture2D>(thumbnails))
				{
					EditorUtility.DisplayDialog("Problem with upload config",
					"Please fill out all fields before uploading", "OK");
				}
				else
				{
					foreach(BuildTarget targ in buildTarg)
					{
						string outputPath = Path.Combine(UMA.AssetBundles.Utility.AssetBundlesOutputPath,  UMA.AssetBundles.Utility.GetPlatformName(targ));
						BuildAssetBundles(outputPath, targ);
						beginUpload(targ);
					}
				}
			}
		}



	void BuildAssetBundles(string outputPath, BuildTarget buildPlatform)
	{
		// Choose the output path according to the build target.
		if (!Directory.Exists(outputPath) )
			Directory.CreateDirectory (outputPath);

		//@TODO: use append hash... (Make sure pipeline works correctly with it.)
		BuildPipeline.BuildAssetBundles (outputPath, BuildAssetBundleOptions.None, buildPlatform);
	}

	void beginUpload(BuildTarget targ) 
	{

		string outPath = Path.Combine(UMA.AssetBundles.Utility.AssetBundlesOutputPath,  UMA.AssetBundles.Utility.GetPlatformName(targ));

		WWWForm uploadForm = new WWWForm();
		string tagsString = "{\"tags\":[";
		for(int i = 0; i < tags.Length; i++)
			tagsString += "\"" + tags[i] + "\",";
		tagsString = tagsString.Substring(0, tagsString.Length - 1) + "]}";

		uploadForm.AddField(Constants.UPLOAD_ASSET_BUNDLE_TAGS_FIELD, tagsString);
		uploadForm.AddField(Constants.UPLOAD_ASSET_BUNDLE_NAME_FIELD, assetBundleName);
		uploadForm.AddField(Constants.UPLOAD_ASSET_BUNDLE_SCENE_NAME_FIELD, sceneName);
		uploadForm.AddField(Constants.UPLOAD_ASSET_BUNDLE_INFO_FIELD, info);
		uploadForm.AddField(Constants.UPLOAD_ASSET_BUNDLE_PLATFORM_FIELD, targ.ToString());
		uploadForm.AddField(Constants.UPLOAD_ASSET_BUNDLE_THUMBNAIL_NUMBER_FIELD, thumbnails.Length);
		uploadForm.AddBinaryData(Constants.UPLOAD_ASSET_BUNDLE_BUNDLE_FIELD, File.ReadAllBytes(outPath + "/" + assetBundleName));
		for(int i = 0; i < thumbnails.Length; i++)
		{
			byte[] png = thumbnails[i].EncodeToPNG();
			uploadForm.AddBinaryData("thumbnail" + i, png);
		}

		string uri = Constants.SERVER_URL + Constants.SERVER_API_UPLOAD_BUNDLE;
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
				Debug.Log("assetBundle upload for " + targ.ToString() + " complete!");
			}
		});
		
	}
}
