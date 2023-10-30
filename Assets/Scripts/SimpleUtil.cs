using System;
using UnityEngine;

public static class SimpleUtil
{
	public static void SafeInvoke(Action action)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.Message);
		}
	}
}
