using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour {


	[SerializeField]
	private Text _text;
	private float _counter;
	[SerializeField]
	private float _stepSize;
	[SerializeField]
	private float _min;
	[SerializeField]
	private float _max;

	void setText()
	{
		_text.text = _counter.ToString();
	}
	public void increment()
	{
		_counter = Mathf.Min(_counter + _stepSize, _max);
		setText();
	}

	public void decrement()
	{
		_counter = Mathf.Max(_counter - _stepSize, _min);
		setText();
	}

	public float getCounter()
	{
		return _counter;
	}

	public void reset()
	{
		_counter = 0;
		setText();
	}

	public void setVal(float val)
	{
		_counter = val;
		setText();
	}

}
