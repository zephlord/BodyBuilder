using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class ServerCommsUtility : Singleton<ServerCommsUtility>
{
	public string serializeDataToString(string[] labels, string[] data)
	{
		string jsonString = "";
		for(int i = 0; i < labels.Length; i ++)
			jsonString += "{\"" + labels[i] + "\": \"" + data[i] + "\"}";
		return jsonString;
	}
	public JSONObject serializeData(string[] labels, string[] data)
		{
			Dictionary<string, string> pairedData = new Dictionary<string, string>();
			for(int i = 0; i < labels.Length; i ++)
				pairedData.Add(labels[i], data[i]);
			return new JSONObject(pairedData);
		}

	public string ToLiteral(string input)
	{
		return Regex.Escape(input);
	}
}
