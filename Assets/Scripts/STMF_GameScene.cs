using DG.Tweening;
using GameConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STMF_GameScene : MonoBehaviour
{
	private STMF_GameInfo gameInfo;

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

	private bool bAutoFire;

	private bool bManuFire;

	private float autoDeltaTime;

	private float lastFiredTime;

	private bool bPressBg;

	private bool bSubDown;

	private bool bPlusDown;

	private bool bStartSubPlus;

	private float downTime;

	private STMF_Area area;

	public STMF_Area[] areas = new STMF_Area[4];

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
	private All_GameBtnsCtrl sptGameBtns;

	[SerializeField]
	private STMF_DlgSet sptDlgSet;

	private STMF_GunCtrl sptGunCtrl;

	[SerializeField]
	private STMF_DlgUserInfo sptDlgUserInfo;

	private STMF_ErrorTipAnim sptGunPosTip;

	private bool isHideTip;

	private int hour = -1;

	private int minute = -1;

	private int tempMinute = -100;

	private void Start()
	{
		gameInfo = STMF_GameInfo.getInstance();
		language = gameInfo.Language;
		gameInfo.GameScene = this;
		gameInfo.currentState = STMF_GameState.On_Game;
		STMF_LocalData.getInstance().applySetting();
		STMF_TipManager.getInstance().InitTip();
		sptDlgSet.gameObject.SetActive(value: false);
		sptDlgUserInfo.gameObject.SetActive(value: false);
		Init();
		AddListener();
		sptGunPosTip.transform.localPosition = ((gameInfo.User.SeatIndex % 2 == 1) ? (Vector3.right * -380f + Vector3.up * -250f) : (Vector3.right * 320f + Vector3.up * -250f));
		sptGunPosTip.img.sprite = spiTip[language];
		sptGunPosTip.img.SetNativeSize();
		sptGunPosTip.img.DOKill();
		sptGunPosTip.img.DOFade(1f, 0f);
		UpdateTableConfig();
		UpdateTableUser();
		UpdateUserInfo();
		sptGunPosTip.PlayTipAnim();
		ZH2_GVars.closeSafeBox = (Action)Delegate.Combine(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Combine(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	private void Update()
	{
		CheckChangeGunValue();
		CheckPressBG();
		CheckAutoFire();
		UpdateTime();
	}

	private void CloseSafeBox()
	{
		STMF_NetMngr.GetSingleton().MyCreateSocket.SendCoinIn();
	}

	private void SaveScore()
	{
		STMF_NetMngr.GetSingleton().MyCreateSocket.SendCoinOut();
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
		sptGunPosTip = base.transform.Find("ImgTip").GetComponent<STMF_ErrorTipAnim>();
		btnPlus = base.transform.Find("ImgTip/BtnPlus").GetComponent<Button>();
		btnSub = base.transform.Find("ImgTip/BtnSub").GetComponent<Button>();
		for (int i = 0; i < 4; i++)
		{
			strLastUserIds[i] = string.Empty;
			STMF_Area sTMF_Area = new STMF_Area();
			string n = "Area" + i.ToString();
			sTMF_Area.area = base.transform.Find(n).gameObject;
			Transform transform = base.transform.Find(n);
			(sTMF_Area.overFlow = transform.Find("OverFlow").gameObject).SetActive(value: false);
			sTMF_Area.sptTipOverFlow = transform.Find("OverFlow").GetComponent<STMF_ErrorTipAnim>();
			sTMF_Area.sptGC = transform.Find("Gun").GetComponent<STMF_GunCtrl>();
			sTMF_Area.objChangeGun = transform.Find("Gun/ChangeGun").gameObject;
			sTMF_Area.objEnergeGun = transform.Find("Gun/EnergeGun").gameObject;
			sTMF_Area.sptCC = transform.Find("Chat").GetComponent<STMF_ChatCtrl>();
			sTMF_Area.sptCC.gameObject.SetActive(value: false);
			sTMF_Area.name = transform.Find("Info/TxtName").GetComponent<Text>();
			sTMF_Area.gameScore = transform.Find("Info/TxtScore").GetComponent<Text>();
			sTMF_Area.gunNum = transform.Find("Gun/TxtGunValue").GetComponent<Text>();
			sTMF_Area.board = transform.Find("Info/ImgBg").GetComponent<Image>();
			sTMF_Area.btnUser = transform.Find("Gun/BtnUser").GetComponent<Button>();
			sTMF_Area.coins = transform.Find("Coins").transform;
			sTMF_Area.coinList = new List<GameObject>();
			sTMF_Area.extraCoinList = new List<STMF_CoinItem>();
			areas[i] = sTMF_Area;
		}
		if (gameInfo.User.SeatIndex > 2)
		{
			STMF_Area sTMF_Area2 = areas[0];
			areas[0] = areas[2];
			areas[2] = sTMF_Area2;
			sTMF_Area2 = areas[1];
			areas[1] = areas[3];
			areas[3] = sTMF_Area2;
		}
		MonoBehaviour.print(gameInfo.User.SeatIndex);
		area = areas[gameInfo.User.SeatIndex - 1];
		sptGunCtrl = area.sptGC;
		STMF_TipManager.getInstance().mTipType = STMF_TipType.NoneTip;
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
			STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common2);
			ClickBG();
			sptGameBtns.ShowBtns();
		});
		sptGameBtns.btnBack.onClick.AddListener(ClickBtnBack);
		sptGameBtns.btnSet.onClick.AddListener(ShowSet);
		sptGameBtns.btnSafe.onClick.AddListener(ShowChat);
		sptGameBtns.btnExcharge.onClick.AddListener(ShowExcharge);
		sptGameBtns.btnTopUp.onClick.AddListener(ShowInOut2);
		for (int i = 0; i < 4; i++)
		{
			int index = i;
			areas[i].btnUser.onClick.AddListener(delegate
			{
				ClickBtnUser(index);
			});
		}
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
			STMF_CoinItem item = new STMF_CoinItem(gunScore, gunValue);
			areas[seatIndex - 1].extraCoinList.Add(item);
		}
	}

	private void UpdateCoinPillars(int seatIndex, int gunScore, int gunValue)
	{
		GameObject go = UnityEngine.Object.Instantiate(objCoinItem);
		STMF_CoinItemCoinfig cic = go.GetComponent<STMF_CoinItemCoinfig>();
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
		go.transform.SetParent(areas[seatIndex - 1].coins);
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

	private void OnEnable()
	{
		isHideTip = false;
	}

	private void OnDisable()
	{
		isHideTip = false;
		ZH2_GVars.closeSafeBox = (Action)Delegate.Remove(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Remove(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	public void HideTip()
	{
		if (!isHideTip)
		{
			isHideTip = true;
			UnityEngine.Debug.LogError("==========HideTip==========");
			txtSyncFish.gameObject.SetActive(value: true);
			txtSyncFish.DOColor(Color.black, 3f);
			txtSyncFish.DOFade(0f, 2f).OnComplete(delegate
			{
				txtSyncFish.gameObject.SetActive(value: false);
			});
			StartCoroutine(WaitSetHideTip());
		}
	}

	private IEnumerator WaitSetHideTip()
	{
		yield return new WaitForSeconds(2f);
		if (txtSyncFish.gameObject.activeInHierarchy)
		{
			txtSyncFish.gameObject.SetActive(value: false);
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
				sptGunCtrl.indexGun += 2;
			}
		}
		else if (!areas[seatIndex - 1].isEnergyOver)
		{
			areas[seatIndex - 1].isEnergyOver = true;
			areas[seatIndex - 1].objEnergeGun.SetActive(value: false);
			sptGunCtrl.indexGun -= 2;
		}
		if (seatIndex > 2)
		{
			areas[seatIndex - 1].objEnergeGun.transform.localScale = new Vector3(-1f, -1f, 1f);
		}
		else
		{
			areas[seatIndex - 1].objEnergeGun.transform.localScale = Vector3.one;
		}
		try
		{
			areas[seatIndex - 1].sptGC.imgGun.sprite = spiGun[sptGunCtrl.indexGun];
		}
		catch (Exception ex)
		{
			int num = sptGunCtrl.indexGun;
			UnityEngine.Debug.LogError("数组越界啦 " + num + "  " + ex);
			if (num < 0 || num >= spiGun.Length)
			{
				num = 0;
				try
				{
					int num2 = int.Parse(areas[seatIndex - 1].gunNum.text);
					UnityEngine.Debug.LogError("炮值: " + num2);
					num = ((num2 > 50) ? 1 : 0);
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("尝试获取炮值失败 " + arg);
				}
			}
			areas[seatIndex - 1].sptGC.imgGun.sprite = spiGun[num];
		}
		areas[seatIndex - 1].sptGC.imgGun.SetNativeSize();
	}

	private void CheckAutoFire()
	{
		if (bAutoFire)
		{
			autoDeltaTime += Time.deltaTime;
			if (autoDeltaTime > ZH2_GVars.firingInterval)
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
			gameInfo.User.ScoreCount = newScore;
		}
		areas[seatIndex - 1].gameScore.text = newScore.ToString();
		if (areas[seatIndex - 1].overFlow.activeSelf && newScore <= 900000)
		{
			areas[seatIndex - 1].overFlow.SetActive(value: false);
		}
		UpdateWhenChangeGun(seatIndex, gunValue);
		areas[seatIndex - 1].sptGC.tfGun.transform.localEulerAngles = Vector3.forward * direction;
		areas[seatIndex - 1].sptGC.GunShot(bFire: false);
	}

	public void UpdateTableUser()
	{
		for (int i = 0; i < 4; i++)
		{
			STMF_UserInfo sTMF_UserInfo = gameInfo.UserList[i];
			if (sTMF_UserInfo == null || areas[i] == area)
			{
				continue;
			}
			if (sTMF_UserInfo.IsExist)
			{
				areas[i].gameScore.text = sTMF_UserInfo.ScoreCount.ToString();
				areas[i].gunNum.text = sTMF_UserInfo.GunValue.ToString();
				UnityEngine.Debug.LogError($"更新玩家{sTMF_UserInfo.SeatIndex}炮值{sTMF_UserInfo.GunValue}");
				UpdateWhenChangeGun(sTMF_UserInfo.SeatIndex, sTMF_UserInfo.GunValue);
				areas[i].name.text = ZH2_GVars.GetBreviaryName(sTMF_UserInfo.Name) + " " + sTMF_UserInfo.SeatIndex + "#";
				areas[i].board.sprite = spiUserBoard[sTMF_UserInfo.SeatIndex - 1];
				if (strLastUserIds[i] != sTMF_UserInfo.UserAccount)
				{
					strLastUserIds[i] = sTMF_UserInfo.UserAccount;
				}
				areas[i].area.SetActive(value: true);
			}
			else
			{
				areas[i].area.SetActive(value: false);
				if (!areas[i].isEnergyOver)
				{
					PlayEnergyGun(i + 1, isPlay: false);
				}
				STMF_EffectMngr.GetSingleton().ResetLiziTime(i + 1);
				strLastUserIds[i] = string.Empty;
			}
		}
	}

	public void UpdateUserInfo()
	{
		area.name.text = ZH2_GVars.GetBreviaryName(gameInfo.User.Name) + " " + gameInfo.User.SeatIndex + "#";
		area.gameScore.text = gameInfo.User.ScoreCount + string.Empty;
		if (area.overFlow.activeSelf && gameInfo.User.ScoreCount <= 900000)
		{
			area.overFlow.SetActive(value: false);
		}
		area.board.sprite = spiUserBoard[gameInfo.User.SeatIndex - 1];
		UpdateWhenChangeGun(gameInfo.User.SeatIndex, gameInfo.User.GunValue, bInit: true);
	}

	public void UpdateWhenChangeGun(int seatIndex, int gunValue, bool bInit = false)
	{
		areas[seatIndex - 1].gunNum.text = gunValue.ToString();
		if (!bInit)
		{
			areas[seatIndex - 1].objChangeGun.SetActive(value: true);
		}
		sptGunCtrl.indexGun = ((gunValue > 50) ? 1 : 0);
		if (!areas[seatIndex - 1].isEnergyOver)
		{
			sptGunCtrl.indexGun += 2;
		}
		areas[seatIndex - 1].sptGC.imgGun.sprite = spiGun[sptGunCtrl.indexGun];
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
			float duration = (num + 600f) / 200f * 2f;
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
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common2);
		STMF_TipManager.getInstance().ShowTip(STMF_TipType.IsExitGame, 0, string.Empty);
	}

	private void HandlePress()
	{
		if (bPressBg)
		{
			ReleaseBg();
		}
	}

	public void ShowUserInfo(STMF_UserInfo user, int honor)
	{
		if (STMF_TipManager.getInstance().mTipType == STMF_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG();
			sptDlgUserInfo.ShowUserInfo(user, honor);
		}
	}

	public void ShowExcharge()
	{
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common2);
		if (STMF_TipManager.getInstance().mTipType == STMF_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG();
			sptGameBtns.imgExcharge.gameObject.SetActive(value: true);
		}
	}

	public void ShowInOut()
	{
		if (!All_TipCanvas.GetInstance().IsPayPanelActive())
		{
			STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common);
			if (STMF_TipManager.getInstance().mTipType == STMF_TipType.NoneTip)
			{
				HandlePress();
				HideButtonBar();
				ClickBG();
				STMF_TipManager.getInstance().ShowTip(STMF_TipType.OpenToUp, 0, "金币不足,是否前往商城充值");
			}
		}
	}

	public void ShowInOut2()
	{
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common2);
		HandlePress();
		HideButtonBar();
		ClickBG();
		if (ZH2_GVars.OpenPlyBoxPanel != null)
		{
			ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.lion_desk);
		}
	}

	public void ShowSet()
	{
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common2);
		if (STMF_TipManager.getInstance().mTipType == STMF_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG();
			sptDlgSet.ShowSet();
		}
	}

	public void ShowChat()
	{
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common2);
		if (STMF_TipManager.getInstance().mTipType != STMF_TipType.NoneTip)
		{
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
		HandlePress();
		HideButtonBar();
		ClickBG();
		if (ZH2_GVars.OpenCheckSafeBoxPwdPanel != null)
		{
			ZH2_GVars.OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ.lion_desk);
		}
	}

	public void ClickBG()
	{
		if (STMF_TipManager.getInstance().mTipType == STMF_TipType.NoneTip)
		{
			HideButtonBar();
			if (sptGameBtns.imgExcharge.gameObject.activeSelf)
			{
				sptGameBtns.imgExcharge.gameObject.SetActive(value: false);
			}
			else if (sptDlgUserInfo.gameObject.activeSelf)
			{
				sptDlgUserInfo.gameObject.SetActive(value: false);
			}
			else if (All_TipCanvas.GetInstance().IsPayPanelActive())
			{
				sptDlgSet.SaveUserSetting();
				sptDlgSet.gameObject.SetActive(value: false);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("========" + STMF_TipManager.getInstance().mTipType);
		}
	}

	private void AutoFired()
	{
		if (STMF_TipManager.getInstance().isSomethingError || !STMF_BulletPoolMngr.GetSingleton().IsFireEnable())
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
		if (!All_TipCanvas.GetInstance().IsPayPanelActive())
		{
			ShowInOut();
		}
	}

	private void ManuFired()
	{
		if (STMF_TipManager.getInstance().isSomethingError || !STMF_BulletPoolMngr.GetSingleton().IsFireEnable())
		{
			return;
		}
		if (gameInfo.User.ScoreCount > 0)
		{
			if (lastFiredTime + ZH2_GVars.firingInterval < Time.time || lastFiredTime == 0f)
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
		if (!All_TipCanvas.GetInstance().IsPayPanelActive())
		{
			ShowInOut();
		}
	}

	public void ClickBtnUser(int index)
	{
		UnityEngine.Debug.LogError("点击头像");
	}

	public void ClickBtnAuto()
	{
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common);
		ClickBG();
		if (bPressBg)
		{
			return;
		}
		if (!bAutoFire)
		{
			if (gameInfo.User.ScoreCount <= 0)
			{
				ShowInOut();
				return;
			}
			btnAuto.image.sprite = spiAutoPlay[language * 2 + 1];
			bAutoFire = true;
		}
		else
		{
			btnAuto.image.sprite = spiAutoPlay[language * 2];
			bAutoFire = false;
			autoDeltaTime = 0f;
		}
	}

	public void PressBg()
	{
		sptGunCtrl.CaculateGunDir();
		ClickBG();
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
		}
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
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.addGun);
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
	}

	public void DownPlus()
	{
		bPlusDown = true;
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
}
