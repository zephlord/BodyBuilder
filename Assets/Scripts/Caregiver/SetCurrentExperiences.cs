using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCurrentExperiences : MonoBehaviour {

	[SerializeField]
	private GameObject _experienceSelectUI;
	public void setExperiencesInTag(string tag)
	{
		GetComponent<ExperienceInfoListBank>().setContents(ExperiencesManifest.Instance.getExperiencesUnderTag(tag));
	}
	
	public void tagSelected(ListBox selectedTagBox)
	{
		setExperiencesInTag(selectedTagBox.content.text);
		_experienceSelectUI.SetActive(true);		
	}
}
