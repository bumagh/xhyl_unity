using System;
using System.Collections;
using UnityEngine;

public class STTF_FishNetpoolMngr : MonoBehaviour
{
	public static STTF_FishNetpoolMngr G_FishNetpoolMngr;

	public Transform mNet;

	private ArrayList _allNetArr = new ArrayList();

	public static STTF_FishNetpoolMngr GetSingleton()
	{
		return G_FishNetpoolMngr;
	}

	private void Awake()
	{
		if (G_FishNetpoolMngr == null)
		{
			G_FishNetpoolMngr = this;
		}
	}

	private void OnEnable()
	{
		ZH2_GVars.DestroyFishNet = (Action<GameObject>)Delegate.Combine(ZH2_GVars.DestroyFishNet, new Action<GameObject>(DestroyFishNet));
	}

	private void OnDisable()
	{
		ZH2_GVars.DestroyFishNet = (Action<GameObject>)Delegate.Remove(ZH2_GVars.DestroyFishNet, new Action<GameObject>(DestroyFishNet));
	}

	public void DestroyFishNet(GameObject fishNet)
	{
		_allNetArr.Remove(fishNet);
		TF_PoolManager.Pools["TFNetPool"].Despawn(fishNet.transform);
	}

	public void RemoveAllFishNet()
	{
		UnityEngine.Debug.Log("RemoveAllFishNet");
		TF_PoolManager.Pools["TFNetPool"].DespawnAll();
	}

	public void CreateFishNet(Vector3 pos, bool isLizi, int nPower, int nPlayerIndex)
	{
		if (_allNetArr.Count >= 10)
		{
			DestroyFishNet(_allNetArr[0] as GameObject);
		}
		Transform transform = TF_PoolManager.Pools["TFNetPool"].Spawn(mNet);
		if (nPlayerIndex == 3 || nPlayerIndex == 4)
		{
			transform.transform.eulerAngles = Vector3.forward * 180f;
		}
		else
		{
			transform.transform.eulerAngles = Vector3.zero;
		}
		_allNetArr.Insert(_allNetArr.Count, transform.gameObject);
		transform.position = pos;
		All_FishNet component = transform.GetComponent<All_FishNet>();
		component.SetPower(nPower, ZH2_GVars.GameType.golden_cicada_fish_desk);
		component.SetLizi(isLizi);
		component.SetPlayerColor(nPlayerIndex);
	}
}
