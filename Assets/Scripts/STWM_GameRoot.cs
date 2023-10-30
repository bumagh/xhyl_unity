using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STWM_GameRoot : STWM_MB_Singleton<STWM_GameRoot>
{
	private static GameObject _rootObj;

	private static List<Action> _singletonReleaseList = new List<Action>();

	public STWM_NetManager sTWM_NetManager;

	public void Awake()
	{
		_rootObj = base.gameObject;
		if (!(STWM_MB_Singleton<STWM_GameRoot>._instance != null))
		{
			StartCoroutine(InitSingletons());
			STWM_MB_Singleton<STWM_GameRoot>.SetInstance(this);
			ZH2_GVars.sendCheckSafeBoxPwd = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendCheckSafeBoxPwd, new Action<object[]>(SendCheckSafeBoxPwd));
			ZH2_GVars.sendChangeSafeBoxPwd = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendChangeSafeBoxPwd, new Action<object[]>(SendChangeSafeBoxPwd));
			ZH2_GVars.sendDeposit = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendDeposit, new Action<object[]>(SendDeposit));
			ZH2_GVars.sendExtract = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendExtract, new Action<object[]>(SendExtract));
			ZH2_GVars.sendTransactionRecord = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendTransactionRecord, new Action<object[]>(SendGetTransactionRecord));
			ZH2_GVars.sendGamePay = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendGamePay, new Action<object[]>(SendGamePay));
			ZH2_GVars.closeSafeBox = (Action)Delegate.Combine(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
			ZH2_GVars.saveScore = (Action)Delegate.Combine(ZH2_GVars.saveScore, new Action(SaveScore));
			StartCoroutine(WaitNetManager());
		}
	}

	private IEnumerator WaitNetManager()
	{
		while (!sTWM_NetManager.isConnected)
		{
			Debug.LogError("=======等待=======");
			yield return null;
		}
		Debug.LogError("=======链接成功!=======");
		yield return new WaitForSeconds(0.5f);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("getTransactionRecord", TransactionRecord);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("checkSafeBoxPwd", CheckSafeBoxPwd);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("changeSafeBoxPwd", ChangeSafeBoxPwd);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("deposit", Deposit);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("extract", Extract);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("pay", Pay);
	}

	private void TransactionRecord(object[] msgs)
	{
		if (ZH2_GVars.getTransactionRecord != null)
		{
			ZH2_GVars.getTransactionRecord(msgs);
		}
		else
		{
			Debug.LogError("getTransactionRecord为空");
		}
	}

	private void CheckSafeBoxPwd(object[] msgs)
	{
		if (ZH2_GVars.getCheckSafeBoxPwd != null)
		{
			ZH2_GVars.getCheckSafeBoxPwd(msgs);
		}
		else
		{
			Debug.LogError("getCheckSafeBoxPwd为空");
		}
	}

	private void ChangeSafeBoxPwd(object[] msgs)
	{
		if (ZH2_GVars.getChangeSafeBoxPwd != null)
		{
			ZH2_GVars.getChangeSafeBoxPwd(msgs);
		}
		else
		{
			Debug.LogError("getChangeSafeBoxPwd为空");
		}
	}

	private void Deposit(object[] msgs)
	{
		if (ZH2_GVars.getDeposit != null)
		{
			ZH2_GVars.getDeposit(msgs);
		}
		else
		{
			Debug.LogError("getDeposit为空");
		}
	}

	private void Extract(object[] msgs)
	{
		if (ZH2_GVars.getExtract != null)
		{
			ZH2_GVars.getExtract(msgs);
		}
		else
		{
			Debug.LogError("getExtract为空");
		}
	}

	private void Pay(object[] msgs)
	{
		if (ZH2_GVars.getGamePay != null)
		{
			ZH2_GVars.getGamePay(msgs);
		}
		else
		{
			Debug.LogError("getPay为空");
		}
	}

	private void SendCheckSafeBoxPwd(object[] msgs)
	{
		string strMethod = "mainGameService/checkSafeBoxPwd";
		_sendMsg(strMethod, msgs);
	}

	private void SendChangeSafeBoxPwd(object[] msgs)
	{
		string strMethod = "mainGameService/changeSafeBoxPwd";
		_sendMsg(strMethod, msgs);
	}

	private void SendDeposit(object[] msgs)
	{
		string strMethod = "mainGameService/deposit";
		_sendMsg(strMethod, msgs);
	}

	private void SendExtract(object[] msgs)
	{
		string strMethod = "mainGameService/extract";
		_sendMsg(strMethod, msgs);
	}

	private void SendGetTransactionRecord(object[] msgs)
	{
		string strMethod = "mainGameService/getTransactionRecord";
		_sendMsg(strMethod, msgs);
	}

	private void SendGamePay(object[] msgs)
	{
		string strMethod = "mainGameService/pay";
		_sendMsg(strMethod, msgs);
	}

	private void CloseSafeBox()
	{
		string strMethod = "userService/userCoinIn";
		object[] args = new object[1] { ZH2_GVars.gameGold };
		_sendMsg(strMethod, args);
	}

	private void SaveScore()
	{
		string strMethod = "userService/userCoinOut";
		object[] args = new object[1] { STWM_GVars.credit };
		_sendMsg(strMethod, args);
	}

	private void OnDisable()
	{
		ZH2_GVars.sendCheckSafeBoxPwd = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendCheckSafeBoxPwd, new Action<object[]>(SendCheckSafeBoxPwd));
		ZH2_GVars.sendChangeSafeBoxPwd = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendChangeSafeBoxPwd, new Action<object[]>(SendChangeSafeBoxPwd));
		ZH2_GVars.sendDeposit = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendDeposit, new Action<object[]>(SendDeposit));
		ZH2_GVars.sendExtract = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendExtract, new Action<object[]>(SendExtract));
		ZH2_GVars.sendTransactionRecord = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendTransactionRecord, new Action<object[]>(SendGetTransactionRecord));
		ZH2_GVars.sendGamePay = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendGamePay, new Action<object[]>(SendGamePay));
		ZH2_GVars.closeSafeBox = (Action)Delegate.Remove(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Remove(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	public void _sendMsg(string strMethod, object[] args)
	{
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send(strMethod, args);
	}

	public void OnApplicationQuit()
	{
		for (int num = _singletonReleaseList.Count - 1; num >= 0; num--)
		{
			_singletonReleaseList[num]();
		}
	}

	private IEnumerator InitSingletons()
	{
		yield return null;
	}

	private static void AddSingleton<T>() where T : STWM_MB_Singleton<T>
	{
		if ((UnityEngine.Object)_rootObj.GetComponent<T>() == (UnityEngine.Object)null)
		{
			T t = _rootObj.AddComponent<T>();
			STWM_MB_Singleton<T>.SetInstance(t);
			t.InitSingleton();
			_singletonReleaseList.Add(delegate
			{
				t.Release();
			});
		}
	}

	public static T GetSingleton<T>() where T : STWM_MB_Singleton<T>
	{
		T component = _rootObj.GetComponent<T>();
		if ((UnityEngine.Object)component == (UnityEngine.Object)null)
		{
			AddSingleton<T>();
		}
		return component;
	}

	public void ClearCanvas()
	{
		Transform transform = GameObject.Find("Canvas").transform;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform2 = (Transform)current;
				UnityEngine.Object.Destroy(transform2.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static void PureAddReleaseAction<T>(T t) where T : STWM_MB_Singleton<T>
	{
		_singletonReleaseList.Add(delegate
		{
			t.Release();
		});
	}
}
