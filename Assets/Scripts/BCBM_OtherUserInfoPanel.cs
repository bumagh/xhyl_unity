using UnityEngine;

public class BCBM_OtherUserInfoPanel : MonoBehaviour
{
	protected BCBM_ChatPanel mChatPanel;

	protected UIPanel mInfoPanel;

	protected Collider mPanelCol;

	protected Collider mBackgroundCol;

	protected Collider mPrivateChatBtnCol;

	protected GameObject mChatBtnObj;

	protected Transform mInfoTran;

	protected UILabel mNickNameText;

	protected UILabel mLevelText;

	protected UILabel mGameScoreText;

	private void Start()
	{
		mInfoPanel = base.transform.GetComponent<UIPanel>();
		mPanelCol = base.transform.Find("OtherUserCollider").GetComponent<Collider>();
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		mPrivateChatBtnCol = base.transform.Find("PrivateChatBtn").GetComponent<Collider>();
		mChatBtnObj = base.transform.Find("PrivateChatBtn").gameObject;
		mInfoTran = base.transform.Find("Info");
		mNickNameText = mInfoTran.Find("Nickname").GetComponent<UILabel>();
		mLevelText = mInfoTran.Find("Level").GetComponent<UILabel>();
		mGameScoreText = mInfoTran.Find("GameScore").GetComponent<UILabel>();
		mChatPanel = GameObject.Find("ChatPanel").GetComponent<BCBM_ChatPanel>();
		HideOtherInfo();
	}

	private void Update()
	{
	}

	public void ShowOtherInfo(string strNickname, int nGameScore, int nUserLever, bool bIsNeedPrivateChat)
	{
		if (nUserLever > 16 || nUserLever < 1)
		{
			BCBM_ErrorManager.GetSingleton().AddError("玩家" + strNickname + "等级错误：" + nUserLever);
			nUserLever = 1;
		}
		SetColliderActive(bIsActive: true);
		if (bIsNeedPrivateChat)
		{
			mChatBtnObj.SetActiveRecursively(state: true);
			mInfoTran.localPosition = new Vector3(0f, 50f, 0f);
		}
		else
		{
			mChatBtnObj.SetActiveRecursively(state: false);
			mChatBtnObj.SetActiveRecursively(state: false);
			mInfoTran.localPosition = new Vector3(0f, 0f, 0f);
		}
		if (BCBM_GameInfo.getInstance().Language == 1)
		{
			mNickNameText.text = "[0bbffa]NickName: [ffffff]" + strNickname;
			mLevelText.text = "[0bbffa]Level: [ffffff]lv." + nUserLever + "(" + BCBM_MyUICommon.gLeverName[nUserLever - 1] + ")";
			mGameScoreText.text = "[0bbffa]Credits: [ffffff]" + nGameScore;
		}
		else
		{
			mNickNameText.text = "[0bbffa]昵    称: [ffffff]" + strNickname;
			mLevelText.text = "[0bbffa]等    级: [ffffff]lv." + nUserLever + "(" + BCBM_MyUICommon.gLeverName[nUserLever - 1] + ")";
			mGameScoreText.text = "[0bbffa]游戏分值: [ffffff]" + nGameScore;
		}
		if ((bool)mInfoPanel)
		{
			mInfoPanel.enabled = true;
		}
	}

	public void HideOtherInfo()
	{
		SetColliderActive(bIsActive: false);
		mInfoPanel.enabled = false;
	}

	public void SetColliderActive(bool bIsActive)
	{
		if ((bool)mPanelCol)
		{
			mPanelCol.enabled = bIsActive;
		}
		if ((bool)mBackgroundCol)
		{
			mBackgroundCol.enabled = bIsActive;
		}
		if ((bool)mPrivateChatBtnCol)
		{
			mPrivateChatBtnCol.enabled = bIsActive;
		}
	}

	public void _onClickPrivateChatBtn()
	{
		if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
		{
			BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
		mChatPanel.ShowPrivateChatWindow();
		HideOtherInfo();
	}
}
