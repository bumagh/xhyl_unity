using UnityEngine;

public abstract class LL_Observer : MonoBehaviour, LL_IObserver
{
	public abstract void OnRcvMsg(int type, object obj = null);
}
