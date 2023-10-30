using BCBM_UICommon;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_RoomList : MonoBehaviour
{
	public Text coinCount;

	public Text coinTestCount;

	public Text gameName;

	public GameObject tableInfo;

	private BCBM_GameInfo mGameInfo;

	private void Start()
	{
		BCBM_NetMngr.isInLoading = false;
		mGameInfo = BCBM_GameInfo.getInstance();
	}

	public void OnClickRoom(int iRoomId)
	{
		if (BCBM_MyTest.TEST)
		{
			string[] array = new string[8];
			int[] array2 = new int[8];
			int[] array3 = new int[8];
			for (int i = 0; i < 6; i++)
			{
				array[i] = "person" + i;
				array2[i] = i + 1;
				array3[i] = i + 1;
			}
		}
	}

	public void clickBack()
	{
		BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.IsExitApplication, string.Empty);
	}
}
