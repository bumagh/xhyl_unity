using UnityEngine;

public class FK3_DontDestroy : MonoBehaviour
{
	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		FK3_GVars.dontDestroyOnLoadList.Add(base.gameObject);
	}

	private void Update()
	{
	}
}
