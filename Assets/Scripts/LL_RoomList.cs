using LL_UICommon;
using UnityEngine;
using UnityEngine.UI;

public class LL_RoomList : MonoBehaviour
{
	protected bool mIsButtonActive = true;

	protected bool mIsAnimated;

	protected float mAnimationTotalTime;

	public Text coinCount;

	public Text coinTestCount;

	public Text gameName;

	public GameObject tableInfo;

	private LL_GameInfo mGameInfo;

	public Button btnBlack;

	private void Start()
	{
		LL_NetMngr.isInLoading = false;
		mGameInfo = LL_GameInfo.getInstance();
		updateUserInfo();
		btnBlack = base.transform.Find("BtnBack").GetComponent<Button>();
		if (btnBlack != null)
		{
			btnBlack.onClick.AddListener(clickBack);
		}

		transform.Find("TxtName").GetComponent<Text>().text = ZH2_GVars.ShowTip("幸运六狮", "LuckyLion", "LuckyLion", "Lục Sư May Măn");
	}

	private void Update()
	{
		if (mIsAnimated)
		{
			mAnimationTotalTime += Time.deltaTime;
			if (mAnimationTotalTime >= 0.5f)
			{
				mAnimationTotalTime = 0f;
				mIsButtonActive = true;
				mIsAnimated = false;
			}
		}
	}

	public void updateUserInfo()
	{
		coinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.CoinCount % 10000).ToString() : mGameInfo.UserInfo.CoinCount.ToString());
		coinTestCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.ExpCoinCount % 10000).ToString() : mGameInfo.UserInfo.ExpCoinCount.ToString());
		if (LL_GameInfo.getInstance().UserInfo != null)
		{
			updateUserInfo(LL_GameInfo.getInstance().UserInfo.TableId);
		}
		if (LL_AppUIMngr.GetSingleton() != null)
		{
			if (ZH2_GVars.hallInfo2 != null && LL_AppUIMngr.GetSingleton().hallInfo != ZH2_GVars.hallInfo2)
			{
				LL_AppUIMngr.GetSingleton().ShowHall();
				LL_AppUIMngr.GetSingleton().hallInfo = ZH2_GVars.hallInfo2;
			}
			int num = mGameInfo.UserInfo.IconIndex;
			if (num <= 0 || num >= LL_AppUIMngr.GetSingleton().icoSprite.Count)
			{
				num = 1;
				mGameInfo.UserInfo.IconIndex = 1;
			}
			LL_AppUIMngr.GetSingleton().imaIco.sprite = LL_AppUIMngr.GetSingleton().icoSprite[num];
		}
		else
		{
			UnityEngine.Debug.LogError("====LL_AppUIMngr为空====");
		}
	}

	public void updateUserInfo(int room)
	{
		coinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.CoinCount % 10000).ToString() : mGameInfo.UserInfo.CoinCount.ToString());
	}

	public void ShowRoomList(int iMode)
	{
		LL_GameTipManager.GetSingleton().EndNetTiming();
		if (iMode == 1)
		{
			mIsAnimated = true;
			mAnimationTotalTime = 0f;
			mIsButtonActive = false;
		}
		else
		{
			mIsAnimated = false;
			mAnimationTotalTime = 0f;
			mIsButtonActive = true;
		}
	}

	public void HideRoomList(int iMode = 0)
	{
		tableInfo.SetActive(iMode == 0);
	}

	public void clickBack()
	{
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.IsExitApplication, string.Empty);
	}
}
