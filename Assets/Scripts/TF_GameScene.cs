using DG.Tweening;
using GameCommon;
using GameConfig;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TF_GameScene : MonoBehaviour
{
	private TF_GameInfo gameInfo;

	private int language;

	private GameObject objNotice;

	private string[] strLastUserIds = new string[4];

	private List<string> listNotice = new List<string>();

	private bool bAllNoticeHasEnd = true;

	private Text txtNotice;

	[SerializeField]
	private Text txtTime;

	private Text tableName;

	private Text txtSyncFish;

	private Button btnAuto;

	private Button btnSub;

	private Button btnPlus;

	public bool bAutoFire;

	public bool bManuFire;

	private float autoDeltaTime;

	private float lastFiredTime;

	private bool bPressBg;

	private bool bSubDown;

	private bool bPlusDown;

	private bool bStartSubPlus;

	private float downTime;

	private bool bHasShow;

	private bool bCountTime;

	private int mCountNum = 10;

	private float fTime;

	[HideInInspector]
	public bool bAuto;

	private TF_Area area;

	[HideInInspector]
	public TF_Area[] areas = new TF_Area[4];

	[SerializeField]
	private Sprite[] spiTip;

	[SerializeField]
	public Sprite[] spiGun;

	[SerializeField]
	private Sprite[] spiNextCoin;

	[SerializeField]
	private Sprite[] spiUserBoard;

	[SerializeField]
	private Sprite[] spiAutoPlay;

	[SerializeField]
	private GameObject objCoinItem;

	[SerializeField]
	private TF_GameBtnsCtrl sptGameBtns;

	[SerializeField]
	private TF_DlgSet sptDlgSet;

	public TF_DlgChat sptDlgChat;

	public TF_DlgInOut sptDlgInOut;

	private TF_GunCtrl sptGunCtrl;

	[SerializeField]
	private TF_DlgUserInfo sptDlgUserInfo;

	private TF_ErrorTipAnim sptGunPosTip;

	[SerializeField]
	private TF_LockFuncTip sptLockFuncTip;

	private bool bClickGameBtn;

	[SerializeField]
	private GameObject objFishRecord;

	private int hour = -1;

	private int minute = -1;

	private int tempMinute = -100;

	private void Start()
	{
		gameInfo = TF_GameInfo.getInstance();
		language = gameInfo.Language;
		gameInfo.GameScene = this;
		gameInfo.currentState = TF_GameState.On_Game;
		TF_LocalData.getInstance().applySetting();
		TF_TipManager.getInstance().InitTip();
		sptDlgSet.gameObject.SetActive(value: false);
		sptDlgChat.gameObject.SetActive(value: false);
		sptDlgInOut.gameObject.SetActive(value: false);
		sptDlgUserInfo.gameObject.SetActive(value: false);
		Init();
		AddListener();
		sptGunPosTip.transform.localPosition = ((gameInfo.User.SeatIndex % 2 == 1) ? (Vector3.right * -380f + Vector3.up * -250f) : (Vector3.right * 320f + Vector3.up * -250f));
		sptGunPosTip.img.sprite = spiTip[language];
		sptGunPosTip.img.SetNativeSize();
		UpdateTableConfig();
		UpdateTableUser();
		UpdateUserInfo();
		if (PlayerPrefs.GetInt("Profile") == 0)
		{
			sptLockFuncTip.gameObject.SetActive(value: true);
		}
		else
		{
			sptGunPosTip.PlayTipAnim();
		}
	}

	private void Update()
	{
		UpdateFixTime();
		CheckChangeGunValue();
		sptDlgInOut.CheckUpDownCoin();
		CheckPressBG();
		CheckAutoFire();
		UpdateTime();
	}

	private void Init()
	{
		objNotice = base.transform.Find("Notice").gameObject;
		objNotice.SetActive(value: false);
		txtNotice = objNotice.transform.Find("ImgNotice/Text").GetComponent<Text>();
		tableName = base.transform.Find("RoomInfo/TxtName").GetComponent<Text>();
		txtSyncFish = base.transform.Find("TxtSyncFish").GetComponent<Text>();
		btnAuto = base.transform.Find("BtnAuto").GetComponent<Button>();
		btnAuto.image.sprite = spiAutoPlay[language * 2];
		sptGunPosTip = base.transform.Find("ImgTip").GetComponent<TF_ErrorTipAnim>();
		btnPlus = base.transform.Find("ImgTip/BtnPlus").GetComponent<Button>();
		btnSub = base.transform.Find("ImgTip/BtnSub").GetComponent<Button>();
		for (int i = 0; i < 4; i++)
		{
			strLastUserIds[i] = string.Empty;
			TF_Area tF_Area = new TF_Area();
			string n = "Area" + i.ToString();
			tF_Area.area = base.transform.Find(n).gameObject;
			Transform transform = base.transform.Find(n);
			(tF_Area.overFlow = transform.Find("OverFlow").gameObject).SetActive(value: false);
			tF_Area.sptTipOverFlow = transform.Find("OverFlow").GetComponent<TF_ErrorTipAnim>();
			tF_Area.sptGC = transform.Find("Gun").GetComponent<TF_GunCtrl>();
			tF_Area.objChangeGun = transform.Find("Gun/ChangeGun").gameObject;
			tF_Area.objEnergeGun = transform.Find("Gun/EnergeGun").gameObject;
			tF_Area.sptCC = transform.GetComponent<TF_ChatCtrl>();
			tF_Area.name = transform.Find("Info/TxtName").GetComponent<Text>();
			tF_Area.gameScore = transform.Find("Info/TxtScore").GetComponent<Text>();
			tF_Area.gunNum = transform.Find("Gun/TxtGunValue").GetComponent<Text>();
			tF_Area.txtFixTime = transform.Find("Gun/TxtFixTime").GetComponent<Text>();
			tF_Area.sptLock = transform.Find("Gun/TfGun/ImgLine").GetComponent<TF_Lock>();
			tF_Area.board = transform.Find("Info/ImgBg").GetComponent<Image>();
			tF_Area.btnUser = transform.Find("Gun/BtnUser").GetComponent<Button>();
			tF_Area.coins = transform.Find("Coins").transform;
			tF_Area.coinList = new List<GameObject>();
			tF_Area.extraCoinList = new List<TF_CoinItem>();
			areas[i] = tF_Area;
			if (i < 2)
			{
				tF_Area.bUp = false;
			}
			else
			{
				tF_Area.bUp = true;
			}
		}
		if (gameInfo.User.SeatIndex > 2)
		{
			TF_Area tF_Area2 = areas[0];
			areas[0] = areas[2];
			areas[2] = tF_Area2;
			tF_Area2 = areas[1];
			areas[1] = areas[3];
			areas[3] = tF_Area2;
		}
		MonoBehaviour.print(gameInfo.User.SeatIndex);
		area = areas[gameInfo.User.SeatIndex - 1];
		sptDlgChat.sptChatCtrl = area.sptCC;
		sptGunCtrl = area.sptGC;
	}

	private void AddListener()
	{
		btnSub.onClick.AddListener(delegate
		{
			ChangeGunValue(-1);
		});
		btnPlus.onClick.AddListener(delegate
		{
			ChangeGunValue(1);
		});
		btnAuto.onClick.AddListener(ClickBtnAuto);
		sptGameBtns.btnArrow.onClick.AddListener(delegate
		{
			bClickGameBtn = true;
			ClickBG();
			bClickGameBtn = false;
			sptGameBtns.ShowBtns();
		});
		sptGameBtns.btnBack.onClick.AddListener(ClickBtnBack);
		sptGameBtns.btnSet.onClick.AddListener(ShowSet);
		sptGameBtns.btnChat.onClick.AddListener(ShowChat);
		sptGameBtns.btnExcharge.onClick.AddListener(ShowExcharge);
		sptGameBtns.btnInOut.onClick.AddListener(ShowInOut);
		for (int i = 0; i < 4; i++)
		{
			int index = i;
			areas[i].btnUser.onClick.AddListener(delegate
			{
				ClickBtnUser(index);
			});
		}
	}

	public void ClickLockFuncTipBtnClose()
	{
		sptLockFuncTip.CloseLockTip();
		sptGunPosTip.PlayTipAnim();
	}

	private void AddCoinItem(int seatIndex, int gunScore, int gunValue)
	{
		if (seatIndex <= 4 && seatIndex >= 1)
		{
			if (areas[seatIndex - 1].coinList.Count < 5 && areas[seatIndex - 1].extraCoinList.Count == 0)
			{
				UpdateCoinPillars(seatIndex, gunScore, gunValue);
				return;
			}
			TF_CoinItem item = new TF_CoinItem(gunScore, gunValue);
			areas[seatIndex - 1].extraCoinList.Add(item);
		}
	}

	private void UpdateCoinPillars(int seatIndex, int gunScore, int gunValue)
	{
		GameObject go = UnityEngine.Object.Instantiate(objCoinItem);
		TF_CoinItemCoinfig cic = go.GetComponent<TF_CoinItemCoinfig>();
		float num = (float)gunScore * 50f / (float)gunValue / 5000f * 372f + 30f;
		num = ((!(num <= 372f)) ? 372f : num);
		float endValue = num / 372f;
		float duration = 0.5f;
		cic.imgCoinPillar.DOFillAmount(endValue, duration);
		float num2 = -176f + num;
		cic.tfCoinGet.gameObject.SetActive(value: true);
		cic.imgScoreBg.transform.localPosition = Vector3.up * num2;
		cic.imgScoreBg.sprite = spiNextCoin[areas[seatIndex - 1].nextCoinColor];
		areas[seatIndex - 1].nextCoinColor = ((areas[seatIndex - 1].nextCoinColor == 0) ? 1 : 0);
		cic.tfCoinGet.DOLocalMoveY(num2 + 5f, duration).OnComplete(delegate
		{
			cic.imgScoreBg.gameObject.SetActive(value: true);
			cic.tfCoinGet.gameObject.SetActive(value: false);
		});
		cic.txtScore.text = gunScore.ToString();
		areas[seatIndex - 1].lastDepth += 2;
		go.transform.parent = areas[seatIndex - 1].coins;
		go.name = "coin" + seatIndex;
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.right * (areas[seatIndex - 1].coinList.Count * -30);
		go.transform.localEulerAngles = Vector3.zero;
		areas[seatIndex - 1].coinList.Add(go);
		float delayTime = (float)areas[seatIndex - 1].coinList.Count + 1f;
		cic.delayTime = delayTime;
		cic.imgCoinPillar.DOFade(0f, 0.5f).SetDelay(cic.delayTime);
		cic.imgScoreBg.DOFade(0f, 0.5f).SetDelay(cic.delayTime);
		cic.txtScore.DOFade(0f, 0.5f).SetDelay(cic.delayTime).OnComplete(delegate
		{
			CoinAlphaEnd(go);
		});
	}

	private void CoinAlphaEnd(GameObject go)
	{
		int num = Convert.ToInt32(go.name.Substring(4)) - 1;
		areas[num].coinList.RemoveAt(0);
		UnityEngine.Object.DestroyImmediate(go);
		if (areas[num].coinList.Count > 0)
		{
			int childCount = areas[num].coins.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = areas[num].coins.GetChild(i);
				Vector3 endValue = child.localPosition + Vector3.right * 30f;
				if (i == 0)
				{
					child.DOLocalMove(endValue, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
					{
						CoinPositionEnd(num);
					});
				}
				else
				{
					child.DOLocalMove(endValue, 0.5f).SetEase(Ease.Linear);
				}
			}
		}
		else
		{
			areas[num].lastDepth = 50;
		}
	}

	private void CoinPositionEnd(int num)
	{
		if (areas[num].coinList.Count < 5 && areas[num].extraCoinList.Count > 0)
		{
			UpdateCoinPillars(num + 1, areas[num].extraCoinList[0].gunScore, areas[num].extraCoinList[0].gunValue);
			areas[num].extraCoinList.RemoveAt(0);
		}
	}

	public void HideTip()
	{
		if (!bHasShow)
		{
			bHasShow = true;
			txtSyncFish.DOColor(Color.black, 3f);
			txtSyncFish.DOFade(0f, 3f);
		}
	}

	private void UpdateTime()
	{
		hour = DateTime.Now.Hour;
		minute = DateTime.Now.Minute;
		if (txtTime != null && tempMinute != minute)
		{
			tempMinute = minute;
			txtTime.text = string.Format("{0}:{1}", hour.ToString("00"), minute.ToString("00"));
		}
	}

	public void PlayEnergyGun(int seatIndex, bool isPlay)
	{
		if (isPlay)
		{
			if (areas[seatIndex - 1].isEnergyOver)
			{
				areas[seatIndex - 1].isEnergyOver = false;
				areas[seatIndex - 1].objEnergeGun.SetActive(value: true);
			}
		}
		else if (!areas[seatIndex - 1].isEnergyOver)
		{
			areas[seatIndex - 1].isEnergyOver = true;
			areas[seatIndex - 1].objEnergeGun.SetActive(value: false);
		}
		areas[seatIndex - 1].sptGC.imgGun.sprite = spiGun[areas[seatIndex - 1].sptGC.indexGun];
		areas[seatIndex - 1].sptGC.imgGun.SetNativeSize();
	}

	private void CheckAutoFire()
	{
		if (bAutoFire)
		{
			autoDeltaTime += Time.deltaTime;
			if (autoDeltaTime > 0.333333343f)
			{
				AutoFired();
				autoDeltaTime = 0f;
			}
		}
		if (bManuFire)
		{
			bManuFire = false;
			ManuFired();
		}
	}

	private void CheckPressBG()
	{
		if (bPressBg && !bAutoFire)
		{
			bManuFire = true;
		}
	}

	public void UpdateTableConfig()
	{
		tableName.text = gameInfo.Table.Name;
	}

	public void UpdateWhenDead(int seatIndex, int newScore, int gunScore, int gunValue)
	{
		if (gameInfo.User.SeatIndex == seatIndex)
		{
			sptDlgInOut.txtCreditValue.text = newScore.ToString();
			gameInfo.User.ScoreCount = newScore;
		}
		areas[seatIndex - 1].gameScore.text = newScore.ToString();
		if (newScore >= 900000 && !areas[seatIndex - 1].overFlow.activeSelf)
		{
			areas[seatIndex - 1].overFlow.SetActive(value: true);
			areas[seatIndex - 1].sptTipOverFlow.PlayTipAnim();
		}
		if (gunScore != 0)
		{
			AddCoinItem(seatIndex, gunScore, gunValue);
		}
	}

	public void UpdateWhenFired(int seatIndex, int newScore, int gunValue, float direction)
	{
		if (gameInfo.User.SeatIndex == seatIndex)
		{
			sptDlgInOut.txtCreditValue.text = newScore.ToString();
			gameInfo.User.ScoreCount = newScore;
		}
		areas[seatIndex - 1].gameScore.text = newScore.ToString();
		if (areas[seatIndex - 1].overFlow.activeSelf && newScore <= 900000)
		{
			areas[seatIndex - 1].overFlow.SetActive(value: false);
		}
		UpdateWhenChangeGun(seatIndex, gunValue);
		bool @lock = gameInfo.UserList[seatIndex - 1].Lock;
		if (gameInfo.UserList[seatIndex - 1].LockFish == null)
		{
			areas[seatIndex - 1].sptGC.tfGun.transform.localEulerAngles = Vector3.forward * direction;
		}
		areas[seatIndex - 1].sptGC.GunShot(bFire: false);
		areas[seatIndex - 1].sptLock.gameObject.SetActive(@lock);
		if (gameInfo.UserList[seatIndex - 1].LockFish != null)
		{
			gameInfo.UserList[seatIndex - 1].LockFish.transform.Find("LockFlag").gameObject.SetActive(@lock);
		}
		areas[seatIndex - 1].sptLock.sptLockCard.gameObject.SetActive(@lock);
	}

	public void UpdateTableUser()
	{
		for (int i = 0; i < 4; i++)
		{
			TF_UserInfo tF_UserInfo = gameInfo.UserList[i];
			if (areas[i] == area)
			{
				continue;
			}
			if (tF_UserInfo.IsExist)
			{
				areas[i].gameScore.text = tF_UserInfo.ScoreCount.ToString();
				areas[i].gunNum.text = tF_UserInfo.GunValue.ToString();
				areas[i].name.text = tF_UserInfo.Name + " " + tF_UserInfo.SeatIndex + "#";
				areas[i].board.sprite = spiUserBoard[tF_UserInfo.SeatIndex - 1];
				if (strLastUserIds[i] != tF_UserInfo.UserAccount)
				{
					strLastUserIds[i] = tF_UserInfo.UserAccount;
				}
				areas[i].area.SetActive(value: true);
			}
			else
			{
				areas[i].sptLock.gameObject.SetActive(value: false);
				areas[i].sptLock.imgLockFlag.gameObject.SetActive(value: false);
				areas[i].area.SetActive(value: false);
				if (!areas[i].isEnergyOver)
				{
					PlayEnergyGun(i + 1, isPlay: false);
				}
				TF_EffectMngr.GetSingleton().ResetLiziTime(i + 1);
				strLastUserIds[i] = string.Empty;
			}
		}
	}

	public void UpdateUserInfo()
	{
		area.name.text = gameInfo.User.Name + " " + gameInfo.User.SeatIndex + "#";
		area.gameScore.text = gameInfo.User.ScoreCount + string.Empty;
		if (area.overFlow.activeSelf && gameInfo.User.ScoreCount <= 900000)
		{
			area.overFlow.SetActive(value: false);
		}
		area.board.sprite = spiUserBoard[gameInfo.User.SeatIndex - 1];
		UpdateWhenChangeGun(gameInfo.User.SeatIndex, gameInfo.User.GunValue, bInit: true);
		sptDlgInOut.UpdateCoinScore();
	}

	public void UpdateWhenChangeGun(int seatIndex, int gunValue, bool bInit = false)
	{
		areas[seatIndex - 1].gunNum.text = gunValue.ToString();
		if (!bInit)
		{
			areas[seatIndex - 1].objChangeGun.SetActive(value: true);
		}
		if (gunValue > 0 && gunValue < 100)
		{
			areas[seatIndex - 1].sptGC.indexGun = ((!areas[seatIndex - 1].isEnergyOver) ? 3 : 0);
		}
		else if (gunValue >= 100 && gunValue < 1000)
		{
			areas[seatIndex - 1].sptGC.indexGun = (areas[seatIndex - 1].isEnergyOver ? 1 : 4);
		}
		else if (gunValue >= 1000)
		{
			areas[seatIndex - 1].sptGC.indexGun = ((!areas[seatIndex - 1].isEnergyOver) ? 5 : 2);
		}
		areas[seatIndex - 1].sptGC.imgGun.sprite = spiGun[areas[seatIndex - 1].sptGC.indexGun];
		areas[seatIndex - 1].sptGC.imgGun.SetNativeSize();
	}

	public void AddNotice(string noticeMessage)
	{
		if (bAllNoticeHasEnd)
		{
			UpdateNotice(noticeMessage);
			objNotice.SetActive(value: true);
			bAllNoticeHasEnd = false;
		}
		else
		{
			listNotice.Add(noticeMessage);
		}
	}

	private void UpdateNotice(string noticeMessage)
	{
		txtNotice.text = noticeMessage;
		txtNotice.transform.DOScale(1f, 0.02f).OnComplete(delegate
		{
			Vector2 sizeDelta = txtNotice.rectTransform.sizeDelta;
			float num = sizeDelta.x / 2f;
			txtNotice.transform.localPosition = Vector3.right * (num + 300f);
			float duration = (num + 600f) / 100f;
			txtNotice.transform.DOLocalMoveX(0f - num - 300f, duration).OnComplete(NoticeEnd);
		});
	}

	public void NoticeEnd()
	{
		if (listNotice.Count == 0)
		{
			bAllNoticeHasEnd = true;
			objNotice.SetActive(value: false);
		}
		else
		{
			UpdateNotice(listNotice[0]);
			listNotice.RemoveAt(0);
		}
	}

	public void ClickBtnBack()
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		if (objFishRecord.activeSelf)
		{
			objFishRecord.SetActive(value: false);
		}
		TF_TipManager.getInstance().ShowTip(TF_TipType.IsExitGame);
	}

	private void HandlePress()
	{
		if (bPressBg)
		{
			ReleaseBg();
		}
	}

	public void ShowUserInfo(TF_UserInfo user, int honor)
	{
		if (TF_TipManager.getInstance().mTipType == TF_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG();
			sptDlgUserInfo.ShowUserInfo(user, honor);
		}
	}

	public void ShowExcharge()
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		if (TF_TipManager.getInstance().mTipType == TF_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG();
			sptGameBtns.imgExcharge.gameObject.SetActive(value: true);
		}
	}

	public void ShowInOut()
	{
		if (!sptDlgInOut.gameObject.activeSelf)
		{
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
			if (TF_TipManager.getInstance().mTipType == TF_TipType.NoneTip)
			{
				HandlePress();
				HideButtonBar();
				ClickBG();
				sptDlgInOut.ShowInOut();
			}
		}
	}

	public void ShowSet()
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		if (TF_TipManager.getInstance().mTipType == TF_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG();
			sptDlgSet.ShowSet();
		}
	}

	public void ShowChat()
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		if (TF_TipManager.getInstance().mTipType == TF_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG();
			sptDlgChat.SetPrivateChat(isPrivate: false, string.Empty);
			sptDlgChat.ShowChat();
		}
	}

	public void ClickBG()
	{
		if (TF_TipManager.getInstance().mTipType == TF_TipType.NoneTip)
		{
			if (!bClickGameBtn)
			{
				HideButtonBar();
			}
			if (sptGameBtns.imgExcharge.gameObject.activeSelf)
			{
				sptGameBtns.imgExcharge.gameObject.SetActive(value: false);
			}
			else if (sptDlgUserInfo.gameObject.activeSelf)
			{
				sptDlgUserInfo.gameObject.SetActive(value: false);
			}
			else if (sptDlgInOut.gameObject.activeSelf)
			{
				sptDlgInOut.gameObject.SetActive(value: false);
			}
			else if (sptDlgSet.gameObject.activeSelf)
			{
				sptDlgSet.SaveUserSetting();
				sptDlgSet.gameObject.SetActive(value: false);
			}
			else if (sptDlgChat.gameObject.activeSelf)
			{
				sptDlgChat.SetPrivateChat(isPrivate: false, string.Empty);
				sptDlgChat.gameObject.SetActive(value: false);
			}
			if (objFishRecord.activeSelf)
			{
				objFishRecord.SetActive(value: false);
			}
		}
	}

	private void AutoFired()
	{
		if (TF_TipManager.getInstance().isSomethingError || !TF_BulletPoolMngr.GetSingleton().IsFireEnable())
		{
			return;
		}
		if (gameInfo.User.ScoreCount > 0)
		{
			sptGunCtrl.GunShot(bFire: true);
			lastFiredTime = Time.time;
			return;
		}
		if (bAutoFire)
		{
			if (!bPressBg)
			{
				btnAuto.image.sprite = spiAutoPlay[language * 2];
			}
			else
			{
				bPressBg = false;
			}
			bAutoFire = false;
			autoDeltaTime = 0f;
		}
		if (!sptDlgInOut.gameObject.activeSelf)
		{
			ShowInOut();
		}
	}

	private void ManuFired()
	{
		if (TF_TipManager.getInstance().isSomethingError || !TF_BulletPoolMngr.GetSingleton().IsFireEnable())
		{
			return;
		}
		if (gameInfo.User.ScoreCount > 0)
		{
			if (lastFiredTime + 0.333333343f < Time.time || lastFiredTime == 0f)
			{
				lastFiredTime = Time.time;
				sptGunCtrl.GunShot(bFire: true);
			}
			return;
		}
		if (bManuFire)
		{
			if (bPressBg)
			{
				bPressBg = false;
			}
			bManuFire = false;
			autoDeltaTime = 0f;
		}
		if (!sptDlgInOut.gameObject.activeSelf)
		{
			ShowInOut();
		}
	}

	public void ClickBtnUser(int index)
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		HideButtonBar();
		if (sptGameBtns.imgExcharge.gameObject.activeSelf)
		{
			sptGameBtns.imgExcharge.gameObject.SetActive(value: false);
		}
		else if (sptDlgInOut.gameObject.activeSelf)
		{
			sptDlgInOut.gameObject.SetActive(value: false);
		}
		else if (sptDlgSet.gameObject.activeSelf)
		{
			sptDlgSet.SaveUserSetting();
			sptDlgSet.gameObject.SetActive(value: false);
		}
		else if (sptDlgChat.gameObject.activeSelf)
		{
			sptDlgChat.SetPrivateChat(isPrivate: false, string.Empty);
			sptDlgChat.gameObject.SetActive(value: false);
		}
		if (area != areas[index])
		{
			TF_NetMngr.GetSingleton().MyCreateSocket.SendRequestPlayerInfo(gameInfo.UserList[index].Id);
		}
		else if (sptDlgUserInfo.gameObject.activeSelf)
		{
			sptDlgUserInfo.gameObject.SetActive(value: false);
		}
		if (objFishRecord.activeSelf)
		{
			objFishRecord.SetActive(value: false);
		}
	}

	public void ClickBtnAuto()
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		ClickBG();
		if (bPressBg)
		{
			return;
		}
		if (!bAuto)
		{
			if (gameInfo.User.ScoreCount <= 0)
			{
				ShowInOut();
				return;
			}
			btnAuto.image.sprite = spiAutoPlay[language * 2 + 1];
			bAutoFire = true;
			bAuto = true;
			return;
		}
		btnAuto.image.sprite = spiAutoPlay[language * 2];
		for (int i = 0; i < gameInfo.UserList.Count; i++)
		{
			if (gameInfo.UserList[i].SeatIndex == gameInfo.User.SeatIndex && !gameInfo.UserList[i].Lock)
			{
				bAutoFire = false;
			}
		}
		autoDeltaTime = 0f;
		bAuto = false;
	}

	public void PressBg()
	{
		if (TF_TipManager.getInstance().mTipType == TF_TipType.NoneTip)
		{
			if (!bClickGameBtn)
			{
				HideButtonBar();
			}
			if (sptGameBtns.imgExcharge.gameObject.activeSelf)
			{
				sptGameBtns.imgExcharge.gameObject.SetActive(value: false);
				return;
			}
			if (sptDlgUserInfo.gameObject.activeSelf)
			{
				sptDlgUserInfo.gameObject.SetActive(value: false);
				return;
			}
			if (sptDlgInOut.gameObject.activeSelf)
			{
				sptDlgInOut.gameObject.SetActive(value: false);
				return;
			}
			if (sptDlgSet.gameObject.activeSelf)
			{
				sptDlgSet.SaveUserSetting();
				sptDlgSet.gameObject.SetActive(value: false);
				return;
			}
			if (sptDlgChat.gameObject.activeSelf)
			{
				sptDlgChat.SetPrivateChat(isPrivate: false, string.Empty);
				sptDlgChat.gameObject.SetActive(value: false);
				return;
			}
			if (objFishRecord.activeSelf)
			{
				objFishRecord.SetActive(value: false);
			}
		}
		sptGunCtrl.CaculateGunDir();
		if (area.sptLock.lockProcess == TF_ELOCK.Locked)
		{
			CancelLockFishAutoFire();
			area.sptLock.imgLockFlag.gameObject.SetActive(value: true);
			area.sptLock.sptLockFlag.HideLockingFlag();
			area.sptLock.lockProcess = TF_ELOCK.EndMoving;
		}
		if (!bAutoFire)
		{
			if (gameInfo.User.ScoreCount <= 0)
			{
				ShowInOut();
			}
			else
			{
				bPressBg = true;
			}
		}
	}

	private void HideButtonBar()
	{
		if (sptGameBtns.bShow)
		{
			sptGameBtns.ShowBtns();
		}
	}

	public void DragBg()
	{
		sptGunCtrl.CaculateGunDir();
	}

	public void ReleaseBg()
	{
		if (bPressBg)
		{
			bPressBg = false;
			bManuFire = false;
			lastFiredTime = 0f;
		}
	}

	public void ClickFish()
	{
		if (TF_TipManager.getInstance().mTipType != TF_TipType.NoneTip)
		{
			return;
		}
		if (!bClickGameBtn)
		{
			HideButtonBar();
		}
		if (!sptGameBtns.imgExcharge.gameObject.activeSelf && !sptDlgUserInfo.gameObject.activeSelf && !sptDlgInOut.gameObject.activeSelf && !sptDlgSet.gameObject.activeSelf && !sptDlgChat.gameObject.activeSelf)
		{
			if (objFishRecord.activeSelf)
			{
				objFishRecord.SetActive(value: false);
			}
			sptGunCtrl.CaculateGunDir();
			ManuFired();
		}
	}

	public void ReleaseFish()
	{
		bPressBg = false;
		bManuFire = false;
	}

	public void CheckChangeGunValue()
	{
		if (bSubDown)
		{
			downTime += Time.deltaTime;
			if (bStartSubPlus || downTime > 0.5f)
			{
				bStartSubPlus = true;
				if (downTime > 0.1f)
				{
					downTime = 0f;
					ChangeGunValue(-1);
				}
			}
		}
		else
		{
			if (!bPlusDown)
			{
				return;
			}
			downTime += Time.deltaTime;
			if (bStartSubPlus || downTime > 0.5f)
			{
				bStartSubPlus = true;
				if (downTime > 0.1f)
				{
					downTime = 0f;
					ChangeGunValue(1);
				}
			}
		}
	}

	private void ChangeGunValue(int step)
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.addGun);
		int gunValue = gameInfo.User.GunValue;
		if (gunValue < gameInfo.Table.MaxGun && gunValue + gameInfo.Table.DeltaGun * step > gameInfo.Table.MaxGun)
		{
			gameInfo.User.GunValue = gameInfo.Table.MaxGun;
		}
		else if (gunValue > gameInfo.Table.MinGun && gunValue + gameInfo.Table.DeltaGun * step < gameInfo.Table.MinGun)
		{
			gameInfo.User.GunValue = gameInfo.Table.MinGun;
		}
		else
		{
			gameInfo.User.GunValue += gameInfo.Table.DeltaGun * step;
		}
		if (gameInfo.User.GunValue > gameInfo.Table.MaxGun)
		{
			gameInfo.User.GunValue = gameInfo.Table.MinGun;
		}
		else if (gameInfo.User.GunValue < gameInfo.Table.MinGun)
		{
			gameInfo.User.GunValue = gameInfo.Table.MaxGun;
		}
		UpdateWhenChangeGun(gameInfo.User.SeatIndex, gameInfo.User.GunValue);
	}

	public void DownSub()
	{
		bSubDown = true;
		if (objFishRecord.activeSelf)
		{
			objFishRecord.SetActive(value: false);
		}
	}

	public void DownPlus()
	{
		bPlusDown = true;
		if (objFishRecord.activeSelf)
		{
			objFishRecord.SetActive(value: false);
		}
	}

	public void UpSub()
	{
		downTime = 0f;
		bSubDown = false;
		bStartSubPlus = false;
	}

	public void UpPlus()
	{
		downTime = 0f;
		bPlusDown = false;
		bStartSubPlus = false;
	}

	public void StartFixScreenCountTime(bool show)
	{
		bCountTime = show;
		mCountNum = 10;
		for (int i = 0; i < gameInfo.UserList.Count; i++)
		{
			if (gameInfo.UserList[i].IsExist)
			{
				if (gameInfo.UserList[i].SeatIndex == gameInfo.User.SeatIndex)
				{
					area.txtFixTime.gameObject.SetActive(show);
					continue;
				}
				areas[i].txtFixTime.gameObject.SetActive(show);
				areas[i].txtFixTime.transform.eulerAngles = Vector3.zero;
			}
		}
	}

	private void UpdateFixTime()
	{
		if (!bCountTime)
		{
			return;
		}
		fTime += Time.deltaTime;
		if (fTime >= 1f)
		{
			fTime = 0f;
			mCountNum--;
		}
		else if (mCountNum == -1)
		{
			bCountTime = false;
		}
		if (mCountNum >= 0)
		{
			for (int i = 0; i < areas.Length; i++)
			{
				areas[i].txtFixTime.text = mCountNum.ToString();
			}
		}
	}

	public void LockFishAutoFire()
	{
		if (bAutoFire)
		{
			return;
		}
		if (gameInfo.User.ScoreCount <= 0)
		{
			ShowInOut();
			for (int i = 0; i < gameInfo.UserList.Count; i++)
			{
				if (gameInfo.UserList[i].SeatIndex == gameInfo.User.SeatIndex)
				{
					gameInfo.UserList[i].Lock = false;
					gameInfo.UserList[i].LockFish = null;
					TF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				}
			}
		}
		else
		{
			bAutoFire = true;
		}
	}

	public void CancelLockFishAutoFire()
	{
		TF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
		if (!(btnAuto.image.sprite == spiAutoPlay[language * 2 + 1]))
		{
			bAutoFire = false;
			autoDeltaTime = 0f;
		}
	}

	public bool GetIsUp(GameObject go)
	{
		for (int i = 0; i < areas.Length; i++)
		{
			if (areas[i].area == go)
			{
				return areas[i].bUp;
			}
		}
		return false;
	}

	private void SelfCancelLock()
	{
		CancelLockFishAutoFire();
		GetCurrentGun(gameInfo.User.SeatIndex).EndLock();
	}

	public TF_Lock GetCurrentGun(int seatid)
	{
		TF_Lock result = null;
		if (seatid == gameInfo.User.SeatIndex)
		{
			return area.sptLock;
		}
		for (int i = 0; i < 4; i++)
		{
			if (areas[i].name.text == gameInfo.UserList[i].Name + " " + seatid + "#")
			{
				return areas[i].sptLock;
			}
		}
		return result;
	}

	public void ClickBtnFind()
	{
		if (!objFishRecord.activeSelf)
		{
			objFishRecord.SetActive(value: true);
		}
		else
		{
			objFishRecord.SetActive(value: false);
		}
	}
}
