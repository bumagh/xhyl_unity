using UnityEngine;

public class JSYS_LL_SetActive : MonoBehaviour
{
	public Transform maskLoad;

	public Transform mask;

	public GameObject NetMngr;

	public GameObject gameMngr;

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
			gameMngr.SetActive(value: true);
		}
		if (ZH2_GVars.isFirstToDaTing)
		{
			if ((bool)NetMngr)
			{
				NetMngr.SetActive(value: true);
			}
		}
		else if ((bool)NetMngr)
		{
			UnityEngine.Object.DestroyImmediate(NetMngr);
		}
	}
}
