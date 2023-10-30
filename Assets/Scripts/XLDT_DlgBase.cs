using STDT_GameConfig;
using UnityEngine;

public class XLDT_DlgBase : MonoBehaviour
{
	private GameObject[] objs;

	private void Awake()
	{
		base.transform.localScale = Vector3.zero;
		objs = new GameObject[4];
		objs[0] = base.transform.Find("SettingPanel").gameObject;
		objs[1] = base.transform.Find("ChatPanel").gameObject;
		objs[2] = base.transform.Find("InOutPanel").gameObject;
		objs[3] = base.transform.Find("PersonalPanel").gameObject;
	}

	public void PopOut()
	{
	}

	public void PopBack()
	{
		if (XLDT_GameUIMngr.GetSingleton().mCurDlgType != XLDT_POP_DLG_TYPE.DLG_NONE)
		{
			XLDT_GameUIMngr.GetSingleton().mCurDlgType = XLDT_POP_DLG_TYPE.DLG_NONE;
		}
	}

	private void HideObj()
	{
		for (int i = 0; i < 4; i++)
		{
			if (objs[i].activeSelf)
			{
				objs[i].SetActive(value: false);
			}
		}
	}
}
