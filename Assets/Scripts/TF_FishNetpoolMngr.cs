using System.Collections;
using UnityEngine;

public class TF_FishNetpoolMngr : MonoBehaviour
{
	public static TF_FishNetpoolMngr G_FishNetpoolMngr;

	public Transform mNet;

	private ArrayList _allNetArr = new ArrayList();

	public static TF_FishNetpoolMngr GetSingleton()
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
		TF_PoolManager.Pools["TFNetPool"].Despawn(fishNet.transform);
	}

	public void RemoveAllFishNet()
	{
		UnityEngine.Debug.Log("RemoveAllFishNet");
		TF_PoolManager.Pools["TFNetPool"].DespawnAll();
	}

	public void CreateFishNet(Vector3 pos, bool isLizi, int nPower, int nPlayerIndex)
	{
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
		TF_FishNet component = transform.GetComponent<TF_FishNet>();
		component.SetPower(nPower);
		component.SetLizi(isLizi);
		component.SetPlayerColor(nPlayerIndex);
	}
}
