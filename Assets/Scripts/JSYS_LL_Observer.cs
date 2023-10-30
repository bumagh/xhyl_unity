using UnityEngine;

public abstract class JSYS_LL_Observer : MonoBehaviour, JSYS_LL_IObserver
{
	public abstract void OnRcvMsg(int type, object obj = null);
}
