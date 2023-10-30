using UnityEngine;

public abstract class BCBM_Observer : MonoBehaviour, BCBM_IObserver
{
	public abstract void OnRcvMsg(int type, object obj = null);
}
