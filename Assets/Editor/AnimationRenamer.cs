using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class AnimationRenamer
{

	private Dictionary<string, int> _nameCounts;

	public AnimationRenamer()
	{
		_nameCounts = new Dictionary<string, int>();
	}



	void incrementNameCounts(string fileName)
	{
		if(!_nameCounts.ContainsKey(fileName))
			_nameCounts.Add(fileName, 0);
		else
			_nameCounts[fileName]++;
	}

	public string rename(string fileName, string newName)
	{
		string newFileName = newName;
		Regex replaceText = new Regex("[^A-Za-z0-9]");
		newFileName = replaceText.Replace(newFileName, "_");
		
		incrementNameCounts(newFileName);
		newFileName += _nameCounts[newFileName].ToString();
		return newFileName;
	}

	public string quickRename(string file)
	{
		Regex replaceText = new Regex("[^A-Za-z0-9]");
		return replaceText.Replace(file, "_");
	}
}