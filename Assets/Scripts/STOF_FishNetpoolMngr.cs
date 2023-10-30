using System;
using System.Collections;
using UnityEngine;

public class STOF_FishNetpoolMngr : MonoBehaviour
{
	public static STOF_FishNetpoolMngr G_FishNetpoolMngr;

	public Transform mNet;

	private ArrayList _allNetArr = new ArrayList();

	public static STOF_FishNetpoolMngr GetSingleton()
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
		STOF_PoolManager.Pools["OFNetPool"].Despawn(fishNet.transform);
	}

	public void RemoveAllFishNet()
	{
		UnityEngine.Debug.Log("RemoveAllFishNet");
		STOF_PoolManager.Pools["OFNetPool"].DespawnAll();
	}

	public void CreateFishNet(Vector3 pos, bool isLizi, int nPower, int nPlayerIndex)
	{
		if (_allNetArr.Count >= 10)
		{
			DestroyFishNet(_allNetArr[0] as GameObject);
		}
		Transform transform = STOF_PoolManager.Pools["OFNetPool"].Spawn(mNet);
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
		component.SetPower(nPower, ZH2_GVars.GameType.li_kui_fish_desk);
		component.SetLizi(isLizi);
		component.SetPlayerColor(nPlayerIndex);
	}
}
