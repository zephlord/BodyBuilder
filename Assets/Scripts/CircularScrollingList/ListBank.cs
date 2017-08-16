/* Store the contents for ListBoxes to display.
 */
using UnityEngine;



public class ListBank : MonoBehaviour
{
	public static ListBank Instance;

	private string[] contents;

	void Awake()
	{
		Instance = this;
		setContents(ExperiencesManifest.Instance.getTags());
	}

	public string getListContent(int index)
	{
		return contents[index].ToString();
	}

	public int getListLength()
	{
		return contents.Length;
	}

	public void setContents(string[] newContents)
	{
		contents = newContents;
	}
}
