using UnityEngine;

public class DK_SetActive : MonoBehaviour
{
	public Transform maskLoad;

	public Transform mask;

	public GameObject NetMngr;

	public GameObject soundManager;

	private void Awake()
	{
		UnityEngine.Debug.LogError("是否第一次进大厅: " + ZH2_GVars.isFirstToDaTing + " 是否从游戏返回大厅: " + ZH2_GVars.isGameToDaTing);
		if (!ZH2_GVars.isGameToDaTing)
		{
			maskLoad.gameObject.SetActive(value: true);
			mask.gameObject.SetActive(value: false);
		}
		else
		{
			maskLoad.gameObject.SetActive(value: false);
			mask.gameObject.SetActive(value: true);
		}
		if (ZH2_GVars.isFirstToDaTing)
		{
			soundManager.SetActive(value: true);
			NetMngr.SetActive(value: true);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(soundManager);
			UnityEngine.Object.DestroyImmediate(NetMngr);
		}
	}
}
