using STDT_GameConfig;
using UnityEngine;

public class XLDT_UserListCtrl : MonoBehaviour
{
	private XLDT_UserCtrl[] sptUserCtrls;

	private void Start()
	{
		sptUserCtrls = new XLDT_UserCtrl[8];
		for (int i = 0; i < 8; i++)
		{
			sptUserCtrls[i] = base.transform.Find($"Person{i}").GetComponent<XLDT_UserCtrl>();
			sptUserCtrls[i].Index = i;
			HideUser(i);
		}
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().UpdateUserList(XLDT_GameInfo.getInstance().UserList);
		}
		if (XLDT_DanTiaoCommon.G_TEST)
		{
			ShowUser(0, 1, "test1000");
			ShowUser(1, 3, "test1000", isPlayer: true);
			ShowUser(2, 2, "test1000");
			ShowUser(3, 4, "test1000");
			ShowUser(7, 4, "test1000");
			ShowChat(2, "123456780----34344你你你你我我我我我");
			ShowUserWinScore(0, 2000);
			ShowUserWinScore(1, -3000);
		}
	}

	public void HideUser(int index)
	{
		XLDT_UserCtrl xLDT_UserCtrl = sptUserCtrls[index];
		xLDT_UserCtrl.Hide();
	}

	public void ShowUser(int index, int icon, string name, bool isPlayer = false)
	{
		XLDT_UserCtrl xLDT_UserCtrl = sptUserCtrls[index];
		xLDT_UserCtrl.Show(icon, name, isPlayer);
	}

	public void Restart()
	{
		for (int i = 0; i < sptUserCtrls.Length; i++)
		{
			XLDT_UserCtrl xLDT_UserCtrl = sptUserCtrls[i];
			xLDT_UserCtrl.ShowWinScore(isShow: false);
		}
	}

	public void ShowUserWinScore(int index, int score, bool isShow = true)
	{
		XLDT_UserCtrl xLDT_UserCtrl = sptUserCtrls[index];
		xLDT_UserCtrl.ShowWinScore(isShow, score);
	}

	public void ShowChat(int index, string words)
	{
		XLDT_UserCtrl xLDT_UserCtrl = sptUserCtrls[index];
		xLDT_UserCtrl.ShowChat(words);
	}
}
