using UnityEngine;

public class Optimization : MonoBehaviour
{
	[SerializeField]
	private bool enableLog = true;

	[SerializeField]
	private bool enableLogger = true;

	[SerializeField]
	private GameObject reporter;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		HW2_GVars.dontDestroyOnLoadList.Add(base.gameObject);
	}

	private void OnValidate()
	{
		UnityEngine.Debug.unityLogger.logEnabled = enableLogger;
	}
}
