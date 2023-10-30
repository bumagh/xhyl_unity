using M__M.HaiWang;
using UnityEngine;

public class BannedClick : MonoBehaviour
{
	private void OnEnable()
	{
		SimpleSingletonBehaviour<UserInput>.Get().allowInput = false;
	}

	private void OnDisable()
	{
		SimpleSingletonBehaviour<UserInput>.Get().allowInput = true;
	}
}
