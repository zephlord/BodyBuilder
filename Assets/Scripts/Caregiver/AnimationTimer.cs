using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTimer : MonoBehaviour {
	private float _timer;
	[SerializeField]
	private Text _text;

	void Start () {
		_timer = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_timer > 0)
		{
			_timer = Mathf.Max(0, _timer - Time.deltaTime);
		}
		setText();	
	}

	public void setTimer(float time)
	{
		_timer = time;
	}

	private void setText()
	{
		int mins = (int)(_timer / 60);
		int secs = (int)(_timer % 60);
		_text.text = mins + "mins " + secs + "secs";
	}
}
