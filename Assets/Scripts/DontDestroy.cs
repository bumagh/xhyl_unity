using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		HW2_GVars.dontDestroyOnLoadList.Add(base.gameObject);
	}

	private void Update()
	{
	}
}
