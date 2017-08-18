using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A few helpful all-purpose functions
///</summary>
public static class UtilityFunctions {


	///<summary>
	/// Transfers data from an array to a new, larger array.
	/// Use in editor scripts when dealing with user-input in arrays
	///</summary>
	///<param name="newSize"> the new size of the array</param>
	///<param name="oldVals"> a reference to the current array.
	/// WILL BE MODIFIED TO BE OF NEW SIZE!!</param>
	public static void transferData<T>(int newSize, ref T[] oldVals)
	{
		int oldSize = oldVals.Length;
		int minSize = Mathf.Min(newSize, oldSize);
		T[] newVals = new T[newSize];

		for(int i = 0; i < minSize; i++)
			newVals[i] = oldVals[i];

		oldVals = newVals;
	}

	///<summary>
	/// Checks if there is a blank or unset data in an array
	///</summary>
	///<param name="data"> the array to check</param>
	///<returns>True if there is null or unset data in the array. False otherwise.</returns>
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
