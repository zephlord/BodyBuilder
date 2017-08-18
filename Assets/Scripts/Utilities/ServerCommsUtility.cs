using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

///<summary>
/// The object to help setup data to be sent to the server
/// </summary>
public class ServerCommsUtility : Singleton<ServerCommsUtility>
{
	///<summary>
	/// setup data into JSON-formatted string to be sent to server
	///</summary>
	///<param name="labels"> a list of strings that are to be the indexes of the data</param>
	///<param name="data"> a list of strings that are the data under correspondingly indexed label.
	/// i.e. data[0] is the data under index label[0]</param>
	///<returns>json-formatted string</returns>
	public string serializeDataToString(string[] labels, string[] data)
	{
		string jsonString = "";
		for(int i = 0; i < labels.Length; i ++)
			jsonString += "{\"" + labels[i] + "\": \"" + data[i] + "\"}";
		return jsonString;
	}

	///<summary>
	/// setup data into JSON object to be sent to server
	///</summary>
	///<param name="labels"> a list of strings that are to be the indexes of the data</param>
	///<param name="data"> a list of strings that are the data under correspondingly indexed label.
	/// i.e. data[0] is the data under index label[0]</param>
	///<returns>JSONObject</returns>
	public JSONObject serializeData(string[] labels, string[] data)
		{
			Dictionary<string, string> pairedData = new Dictionary<string, string>();
			for(int i = 0; i < labels.Length; i ++)
				pairedData.Add(labels[i], data[i]);
			return new JSONObject(pairedData);
		}

	///<summary>
	/// escapes string
	///</summary>
	///<param name="input"> the string to be escaped </param>
	///<returns>escaped string</returns>
	public string ToLiteral(string input)
	{
		return Regex.Escape(input);
	}
}
