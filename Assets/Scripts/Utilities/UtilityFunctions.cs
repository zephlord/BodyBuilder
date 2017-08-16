using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions {

	public static void transferData<T>(int newSize, ref T[] oldVals)
	{
		int oldSize = oldVals.Length;
		int minSize = Mathf.Min(newSize, oldSize);
		T[] newVals = new T[newSize];

		for(int i = 0; i < minSize; i++)
			newVals[i] = oldVals[i];

		oldVals = newVals;
	}

	public static bool containsBlank<T>(T[] data)
	{
		for(int i = 0; i < data.Length; i++)
		{
			if(data[i].Equals(null) || data[i].Equals(default(T)))
				return true;
		}

		return false;
	}
}
