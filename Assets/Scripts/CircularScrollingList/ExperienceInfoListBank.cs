using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceInfoListBank : MonoBehaviour
{
	public static ExperienceInfoListBank Instance;

	protected ExperienceInfoBundle[] contents;

	void Awake()
	{
		Instance = this;
	}

	public ExperienceInfoBundle getListContent(int index)
	{
		return contents[index];
	}

	public int getListLength()
	{
		return contents.Length;
	}

	public void setContents(ExperienceInfoBundle[] newContents)
	{
		contents = newContents;
	}
}
