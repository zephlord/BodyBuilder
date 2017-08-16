using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class TestData
{
  [SerializeField]
  string filename;
  public string FILENAME { get {return filename; } set { filename = value;} }
  
  [SerializeField]
  string displayname;
  public string DISPLAYNAME { get {return displayname; } set { displayname = value;} }
  
  [SerializeField]
  string[] tags = new string[0];
  public string[] TAGS { get {return tags; } set { tags = value;} }
  
}