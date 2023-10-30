using System.Diagnostics;
using UnityEngine;

public class FK3_GOReplacer : MonoBehaviour
{
	public GameObject gmObj;

	private Stopwatch _stopwatch = new Stopwatch();

	private void Awake()
	{
		Stopwatch stopwatch = new Stopwatch();
		_stopwatch.Start();
		stopwatch.Start();
		if (gmObj != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(gmObj);
			if (base.transform.parent != null)
			{
				gameObject.transform.SetParent(base.transform.parent);
				gameObject.transform.SetSiblingIndex(base.transform.GetSiblingIndex());
				base.transform.SetSiblingIndex(base.transform.parent.childCount - 1);
			}
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log(FK3_LogHelper.Orange($"加载耗时 in Awake：{stopwatch.Elapsed.TotalSeconds}"));
	}

	private void Start()
	{
		_stopwatch.Stop();
		UnityEngine.Debug.Log(FK3_LogHelper.Orange($"加载耗时 Awake to Start：{_stopwatch.Elapsed.TotalSeconds}"));
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
