using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DNTG_FishNetpoolMngr : MonoBehaviour
{
	public static DNTG_FishNetpoolMngr G_FishNetpoolMngr;

	public Transform mNet;

	private ArrayList _allNetArr = new ArrayList();

	public static DNTG_FishNetpoolMngr GetSingleton()
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

	public void DestroyFishNet(GameObject fishNet)
	{
		_allNetArr.Remove(fishNet);
		DNTG_PoolManager.Pools["DNTGNetPool"].Despawn(fishNet.transform);
	}

	public void RemoveAllFishNet()
	{
		UnityEngine.Debug.Log("RemoveAllFishNet");
		DNTG_PoolManager.Pools["DNTGNetPool"].DespawnAll();
	}

	public void CreateFishNet(Vector3 pos, int nPower, int nPlayerIndex, DNTG_Bullet bullet)
	{
		Transform transform = DNTG_PoolManager.Pools["DNTGNetPool"].Spawn(mNet);
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
		DNTG_FishNet component = transform.GetComponent<DNTG_FishNet>();
		component.SetPower(nPlayerIndex, nPower, bullet);
	}

	public void CreateFishNet2(Vector3 pos, DNTG_FishNet fishNet)
	{
		UnityEngine.Debug.LogError("=======切换位置======");
		fishNet.transform.DOMove(pos, 0.1f);
	}
}
