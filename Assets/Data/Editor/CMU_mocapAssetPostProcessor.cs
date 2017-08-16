using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class CMU_mocapAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Huge Mocap Library/mocap animations/cmu-mocap-index-spreadsheet.xls";
    private static readonly string assetFilePath = "Assets/Huge Mocap Library/mocap animations/CMU_mocap.asset";
    private static readonly string sheetName = "CMU_mocap";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            CMU_mocap data = (CMU_mocap)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(CMU_mocap));
            if (data == null) {
                data = ScriptableObject.CreateInstance<CMU_mocap> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<CMU_mocapData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<CMU_mocapData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
