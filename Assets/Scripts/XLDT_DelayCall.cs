using STDT_GameConfig;
using System;
using System.Collections;
using UnityEngine;

public class XLDT_DelayCall : MonoBehaviour
{
	public static XLDT_DelayCall G_DelayCall;

	public static XLDT_DelayCall GetSingleton()
	{
		return G_DelayCall;
	}

	private void Awake()
	{
		if (G_DelayCall == null)
		{
			G_DelayCall = this;
		}
	}

	public void StartDelay(float t, Action act)
	{
		UnityEngine.Debug.Log("gameInfo.getInstance().currentState1: " + XLDT_GameInfo.getInstance().currentState);
		StartCoroutine(DelayCallTipManger(t, act));
	}

	private IEnumerator DelayCallTipManger(float t, Action act)
	{
		while (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Loading)
		{
			yield return XLDT_GameInfo.getInstance().currentState > XLDT_GameState.On_Loading;
		}
		UnityEngine.Debug.Log("gameInfo.getInstance().currentState2: " + XLDT_GameInfo.getInstance().currentState);
		act();
	}
}
