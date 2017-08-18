using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

///<summary>
/// allows for a button to be held in order to repeatedly call a function
///</summary>
public class DelayedButtonHold : MonoBehaviour {

	[SerializeField]
	private UnityEvent _action;
	protected bool _isDown;
	protected float _timer;
	[SerializeField]
	private float _activationTime;
	private bool _hasStarted;
	void Update()
	{
		
		if(_isDown)
		{
			_timer += Time.deltaTime;
			if(_timer >= _activationTime)
			{
				_timer %= _activationTime;
				activate();
			}

		}

		if(!_hasStarted)
		{
			_hasStarted = true;
			if(GetComponent<UIPolygon>() != null)
				GetComponent<UIPolygon>().color = Color.white;
		}
	}
	
	public void OnPointerDown()
	{
		_isDown = true;
	}

	public void OnPointerUp()
	{
		_isDown = false;
		_timer = 0;
	}

	protected virtual void activate()
	{
		_action.Invoke();
	}
}
