using System;
using UnityEngine;

public static class FK3_SimpleUtil
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
