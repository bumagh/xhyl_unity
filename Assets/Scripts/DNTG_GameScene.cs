using DG.Tweening;
using GameCommon;
using GameConfig;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_GameScene : MonoBehaviour
{
	private DNTG_GameInfo gameInfo;

	private int language;

	private string[] strLastUserIds = new string[4];

	private List<string> listNotice = new List<string>();

	private bool bAllNoticeHasEnd = true;

	[SerializeField]
	private Text txtTime;

	private Text tableName;

	private Image txtSyncFish;

	private Button btnAuto;

	private Button btnShootUp;

	private Button btnSub;

	private Button btnPlus;

	private Button btnLock;

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

	[HideInInspector]
	public bool bShootUp;

	[HideInInspector]
	public bool bLock;

	private DNTG_Area area;

	[HideInInspector]
	public DNTG_Area[] areas = new DNTG_Area[4];

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
	private Sprite[] spiShootUp;

	[SerializeField]
	private Sprite[] spiLock;

	[SerializeField]
	private GameObject objCoinItem;

	[SerializeField]
	private All_GameBtnsCtrl sptGameBtns;

	[SerializeField]
	private DNTG_DlgSet sptDlgSet;

	private DNTG_GunCtrl sptGunCtrl;

	private DNTG_ErrorTipAnim sptGunPosTip;

	[SerializeField]
	private DNTG_LockFuncTip sptLockFuncTip;

	private bool bClickGameBtn;

	private int hour = -1;

	private int minute = -1;

	private int tempMinute = -100;

	private GameObject btnAutoEff;

	private void Start()
	{
		gameInfo = DNTG_GameInfo.getInstance();
		language = gameInfo.Language;
		gameInfo.GameScene = this;
		gameInfo.currentState = DNTG_GameState.On_Game;
		DNTG_LocalData.getInstance().applySetting();
		DNTG_TipManager.getInstance().InitTip();
		sptDlgSet.gameObject.SetActive(value: false);
		Init();
		UpdateTableConfig();
		UpdateTableUser();
		UpdateUserInfo();
		AddListener();
		sptGunPosTip.transform.localPosition = ((gameInfo.User.SeatIndex % 2 == 1) ? (Vector3.right * -380f + Vector3.up * -250f) : (Vector3.right * 320f + Vector3.up * -250f));
		sptGunPosTip.img.sprite = spiTip[language];
		sptGunPosTip.img.SetNativeSize();
		if (PlayerPrefs.GetInt("Profile") == 0)
		{
			sptLockFuncTip.gameObject.SetActive(value: true);
		}
		else
		{
			sptGunPosTip.PlayTipAnim();
		}
		ZH2_GVars.closeSafeBox = (Action)Delegate.Combine(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
	}

	private IEnumerator WaitStart()
	{
		while (ZH2_GVars.isCanSenEnterGame)
		{
			yield return new WaitForSeconds(2.5f);
			try
			{
				WebSocket2.GetInstance().SenEnterGame(isEnterGame: true, ZH2_GVars.GameType.li_kui_fish_desk, gameInfo.Table.Id.ToString(), gameInfo.User.ScoreCount.ToString());
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}
	}

	private void Update()
	{
		UpdateFixTime();
		CheckChangeGunValue();
		CheckPressBG();
		CheckAutoFire();
		UpdateTime();
	}

	private void CheckUpDownCoin()
	{
		ZH2_GVars.gameGold = ((gameInfo.User.RoomId == 1) ? (gameInfo.IsSpecial ? (gameInfo.User.CoinCount % 10000) : gameInfo.User.CoinCount) : (gameInfo.IsSpecial ? (gameInfo.User.TestCoinCount % 10000) : gameInfo.User.TestCoinCount));
	}

	private void Init()
	{
		tableName = base.transform.Find("RoomInfo/TxtName").GetComponent<Text>();
		txtSyncFish = base.transform.Find("TxtSyncFish").GetComponent<Image>();
		btnAuto = base.transform.Find("BtnAuto").GetComponent<Button>();
		btnAuto.image.sprite = spiAutoPlay[language * 2];
		SetBtnAutoEff(isShow: false);
		btnShootUp = base.transform.Find("BtnShootUp").GetComponent<Button>();
		btnShootUp.image.sprite = spiShootUp[0];
		btnLock = base.transform.Find("BtnLock").GetComponent<Button>();
		btnLock.image.sprite = spiLock[0];
		sptGunPosTip = base.transform.Find("ImgTip").GetComponent<DNTG_ErrorTipAnim>();
		btnPlus = base.transform.Find("ImgTip/BtnPlus").GetComponent<Button>();
		btnSub = base.transform.Find("ImgTip/BtnSub").GetComponent<Button>();
		for (int i = 0; i < 4; i++)
		{
			strLastUserIds[i] = string.Empty;
			DNTG_Area dNTG_Area = new DNTG_Area();
			string n = "Area" + i.ToString();
			dNTG_Area.area = base.transform.Find(n).gameObject;
			Transform transform = base.transform.Find(n);
			(dNTG_Area.overFlow = transform.Find("OverFlow").gameObject).SetActive(value: false);
			dNTG_Area.sptTipOverFlow = transform.Find("OverFlow").GetComponent<DNTG_ErrorTipAnim>();
			dNTG_Area.sptGC = transform.Find("Gun").GetComponent<DNTG_GunCtrl>();
			dNTG_Area.objChangeGun = transform.Find("Gun/ChangeGun").gameObject;
			dNTG_Area.objEnergeGun = transform.Find("Gun/EnergeGun").gameObject;
			dNTG_Area.sptCC = transform.GetComponent<DNTG_ChatCtrl>();
			dNTG_Area.name = transform.Find("Info/TxtName").GetComponent<Text>();
			dNTG_Area.gameScore = transform.Find("Info/TxtScore").GetComponent<Text>();
			dNTG_Area.gunNum = transform.Find("Gun/TxtGunValue").GetComponent<Text>();
			dNTG_Area.txtFixTime = transform.Find("Gun/TxtFixTime").GetComponent<Text>();
			dNTG_Area.sptLock = transform.Find("Gun/TfGun/ImgLine").GetComponent<DNTG_Lock>();
			dNTG_Area.board = transform.Find("Info/ImgBg").GetComponent<Image>();
			dNTG_Area.coins = transform.Find("Coins").transform;
			dNTG_Area.coinList = new List<GameObject>();
			dNTG_Area.extraCoinList = new List<DNTG_CoinItem>();
			areas[i] = dNTG_Area;
			if (i < 2)
			{
				dNTG_Area.bUp = false;
			}
			else
			{
				dNTG_Area.bUp = true;
			}
		}
		if (gameInfo.User.SeatIndex > 2)
		{
			DNTG_Area dNTG_Area2 = areas[0];
			areas[0] = areas[2];
			areas[2] = dNTG_Area2;
			dNTG_Area2 = areas[1];
			areas[1] = areas[3];
			areas[3] = dNTG_Area2;
		}
		MonoBehaviour.print(gameInfo.User.SeatIndex);
		area = areas[gameInfo.User.SeatIndex - 1];
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
		btnShootUp.onClick.AddListener(ClickBtnShootUp);
		btnLock.onClick.AddListener(ClickBtnLock);
		sptGameBtns.btnArrow.onClick.AddListener(delegate
		{
			bClickGameBtn = true;
			ClickBG(bShow: true);
			bClickGameBtn = false;
			sptGameBtns.ShowBtns();
		});
		sptGameBtns.btnBack.onClick.AddListener(ClickBtnBack);
		sptGameBtns.btnSet.onClick.AddListener(ShowSet);
		sptGameBtns.btnSafe.onClick.AddListener(ShowSafe);
		sptGameBtns.btnExcharge.onClick.AddListener(ShowExcharge);
		sptGameBtns.btnTopUp.onClick.AddListener(ShowTopUp2);
	}

	private void CloseSafeBox()
	{
		DNTG_NetMngr.GetSingleton().MyCreateSocket.SendCoinIn();
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
			DNTG_CoinItem item = new DNTG_CoinItem(gunScore, gunValue);
			areas[seatIndex - 1].extraCoinList.Add(item);
		}
	}

	private void UpdateCoinPillars(int seatIndex, int gunScore, int gunValue)
	{
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
			iTween component = txtSyncFish.GetComponent<iTween>();
			if (component != null)
			{
				component.enabled = false;
			}
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
	}

	private void CheckAutoFire()
	{
		if (bAutoFire)
		{
			autoDeltaTime += Time.deltaTime;
			if (autoDeltaTime > ZH2_GVars.firingInterval * ((!IsSuperShoot()) ? 1f : 0.5f))
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

	private bool IsSuperShoot()
	{
		return gameInfo.IsSuperShoot[gameInfo.User.SeatIndex - 1];
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

	public void UpdateWhenFired(int seatIndex, int newScore, int gunValue, float direction, bool isSpeed)
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
		UpdateWhenChangeGun(seatIndex, gunValue, bInit: true, isSpeed);
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
			DNTG_UserInfo dNTG_UserInfo = gameInfo.UserList[i];
			if (areas[i] == area)
			{
				continue;
			}
			if (dNTG_UserInfo.IsExist)
			{
				areas[i].gameScore.text = dNTG_UserInfo.ScoreCount.ToString();
				areas[i].gunNum.text = dNTG_UserInfo.GunValue.ToString();
				areas[i].name.text = ZH2_GVars.GetBreviaryName(dNTG_UserInfo.Name) + " " + dNTG_UserInfo.SeatIndex + "#";
				areas[i].board.sprite = spiUserBoard[dNTG_UserInfo.SeatIndex - 1];
				if (strLastUserIds[i] != dNTG_UserInfo.UserAccount)
				{
					strLastUserIds[i] = dNTG_UserInfo.UserAccount;
				}
				areas[i].area.SetActive(value: true);
			}
			else
			{
				areas[i].sptLock.gameObject.SetActive(value: false);
				areas[i].area.SetActive(value: false);
				if (!areas[i].isEnergyOver)
				{
					UnityEngine.Debug.LogError("====粒子炮====");
				}
				DNTG_EffectMngr.GetSingleton().ResetLiziTime(i + 1);
				strLastUserIds[i] = string.Empty;
			}
		}
	}

	public void UpdateUserInfo()
	{
		if (area == null)
		{
			UnityEngine.Debug.LogError("=====area为空=======");
			return;
		}
		area.name.text = ZH2_GVars.GetBreviaryName(gameInfo.User.Name) + " " + gameInfo.User.SeatIndex + "#";
		area.gameScore.text = gameInfo.User.ScoreCount + string.Empty;
		if (area.overFlow.activeSelf && gameInfo.User.ScoreCount <= 900000)
		{
			area.overFlow.SetActive(value: false);
		}
		int num = gameInfo.User.SeatIndex - 1;
		if (num < 0 || num >= spiUserBoard.Length)
		{
			num = 0;
		}
		area.board.sprite = spiUserBoard[num];
		UpdateWhenChangeGun(gameInfo.User.SeatIndex, gameInfo.User.GunValue, bInit: true, DNTG_GameInfo.getInstance().IsSuperShoot[gameInfo.User.SeatIndex - 1]);
	}

	public void UpdateWhenChangeGun(int seatIndex, int gunValue, bool bInit = false, bool isSpeed = false)
	{
		int num = seatIndex - 1;
		DNTG_GameInfo.getInstance().IsSuperShoot[num] = isSpeed;
		int num2 = 0;
		areas[num].gunNum.text = gunValue.ToString();
		if (!bInit)
		{
			areas[num].objChangeGun.SetActive(value: true);
		}
		num2 = ((gunValue > 0 && gunValue <= 50) ? (isSpeed ? 3 : 0) : ((gunValue > 50 && gunValue < 200) ? ((!isSpeed) ? 1 : 4) : (isSpeed ? 5 : 2)));
		areas[num].sptGC.SetShot(num2);
		areas[num].sptGC.imgGun.sprite = spiGun[num2];
		areas[num].sptGC.imgGun.SetNativeSize();
	}

	public void AddNotice(string noticeMessage)
	{
		if (bAllNoticeHasEnd)
		{
			UpdateNotice(noticeMessage);
			bAllNoticeHasEnd = false;
		}
		else
		{
			listNotice.Add(noticeMessage);
		}
	}

	private void UpdateNotice(string noticeMessage)
	{
	}

	public void NoticeEnd()
	{
		if (listNotice.Count == 0)
		{
			bAllNoticeHasEnd = true;
			return;
		}
		UpdateNotice(listNotice[0]);
		listNotice.RemoveAt(0);
	}

	public void ClickBtnBack()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.IsExitGame, 0, string.Empty);
	}

	private void HandlePress()
	{
		if (bPressBg)
		{
			ReleaseBg();
		}
	}

	public void ShowUserInfo(DNTG_UserInfo user, int honor)
	{
		if (DNTG_TipManager.getInstance().mTipType == DNTG_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG(bShow: false);
		}
	}

	public void ShowExcharge()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		if (DNTG_TipManager.getInstance().mTipType == DNTG_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG(bShow: false);
			sptGameBtns.imgExcharge.gameObject.SetActive(value: true);
		}
	}

	public void ShowTopUp()
	{
		if (!All_TipCanvas.GetInstance().IsPayPanelActive())
		{
			DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
			if (DNTG_TipManager.getInstance().mTipType == DNTG_TipType.NoneTip)
			{
				HandlePress();
				HideButtonBar();
				ClickBG(bShow: false);
				All_GameMiniTipPanel.publicMiniTip.ShowTip("分数不足,请充值");
				UnityEngine.Debug.LogError("=====分数不足=====");
			}
		}
	}

	public void ShowTopUp2()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		if (DNTG_TipManager.getInstance().mTipType == DNTG_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG(bShow: false);
			if (ZH2_GVars.OpenPlyBoxPanel != null)
			{
				ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.heaven_fish_desk);
			}
		}
	}

	public void ShowSet()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		if (DNTG_TipManager.getInstance().mTipType == DNTG_TipType.NoneTip)
		{
			HandlePress();
			HideButtonBar();
			ClickBG(bShow: false);
			sptDlgSet.ShowSet();
		}
	}

	public void ShowSafe()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		if (DNTG_TipManager.getInstance().mTipType != DNTG_TipType.NoneTip)
		{
			return;
		}
		if (bAutoFire)
		{
			if (!bPressBg)
			{
				btnAuto.image.sprite = spiAutoPlay[language * 2];
				SetBtnAutoEff(isShow: false);
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
		ClickBG(bShow: false);
		if (ZH2_GVars.OpenCheckSafeBoxPwdPanel != null)
		{
			ZH2_GVars.OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ.heaven_fish_desk);
		}
	}

	public void ClickBG(bool bShow)
	{
		if (DNTG_TipManager.getInstance().mTipType == DNTG_TipType.NoneTip)
		{
			if (!bClickGameBtn)
			{
				HideButtonBar();
			}
			if (sptGameBtns.imgExcharge.gameObject.activeSelf)
			{
				sptGameBtns.imgExcharge.gameObject.SetActive(value: false);
			}
			else if (sptDlgSet.gameObject.activeSelf)
			{
				sptDlgSet.SaveUserSetting();
				sptDlgSet.gameObject.SetActive(value: false);
			}
		}
	}

	private void AutoFired()
	{
		if (DNTG_TipManager.getInstance().isSomethingError || !DNTG_BulletPoolMngr.GetSingleton().IsFireEnable())
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
				SetBtnAutoEff(isShow: false);
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
			ShowTopUp();
		}
	}

	private void ManuFired()
	{
		if (DNTG_TipManager.getInstance().isSomethingError || !DNTG_BulletPoolMngr.GetSingleton().IsFireEnable())
		{
			return;
		}
		if (gameInfo.User.ScoreCount > 0)
		{
			if (lastFiredTime + ZH2_GVars.firingInterval * ((!IsSuperShoot()) ? 1f : 0.5f) < Time.time || lastFiredTime == 0f)
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
			ShowTopUp();
		}
	}

	public void ClickBtnUser(int index)
	{
	}

	public void ClickBtnAuto()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		ClickBG(bShow: false);
		UnityEngine.Debug.LogError("点击自动按钮1");
		if (bPressBg)
		{
			return;
		}
		if (!bAuto)
		{
			UnityEngine.Debug.LogError("开始自动");
			if (gameInfo.User.ScoreCount <= 0)
			{
				ShowTopUp();
				return;
			}
			try
			{
				btnAuto.image.sprite = spiAutoPlay[language * 2 + 1];
				SetBtnAutoEff(isShow: true);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			bAutoFire = true;
			bAuto = true;
			return;
		}
		UnityEngine.Debug.LogError("结束自动");
		try
		{
			btnAuto.image.sprite = spiAutoPlay[language * 2];
			SetBtnAutoEff(isShow: false);
		}
		catch (Exception message2)
		{
			UnityEngine.Debug.LogError(message2);
		}
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

	private void ClickBtnShootUp()
	{
		if (!bShootUp)
		{
			btnShootUp.image.sprite = spiShootUp[1];
			bShootUp = true;
		}
		else
		{
			btnShootUp.image.sprite = spiShootUp[0];
			bShootUp = false;
		}
		int num = gameInfo.User.SeatIndex - 1;
		gameInfo.IsSuperShoot[num] = bShootUp;
		int num2 = 0;
		num2 = int.Parse(area.gunNum.text);
		UnityEngine.Debug.LogError(num + " ===炮加速: " + JsonMapper.ToJson(gameInfo.IsSuperShoot) + "  " + num2);
		UpdateWhenChangeGun(num + 1, num2, bInit: false, bShootUp);
	}

	private void ClickBtnLock()
	{
		if (!bLock)
		{
			if (gameInfo.User.ScoreCount <= 0)
			{
				UnityEngine.Debug.LogError("======余额不足======");
				return;
			}
			btnLock.image.sprite = spiLock[1];
			bLock = true;
			DNTG_Lock currentGun = GetCurrentGun(gameInfo.User.SeatIndex);
			currentGun.SetLockID(gameInfo.User.SeatIndex);
			currentGun.lockProcess = DNTG_ELOCK.LookForALockFish;
			currentGun.gameObject.SetActive(value: true);
			return;
		}
		bLock = false;
		btnLock.image.sprite = spiLock[0];
		DNTG_Lock currentGun2 = GetCurrentGun(gameInfo.User.SeatIndex);
		currentGun2.EndLock();
		CancelLockFishAutoFire();
		DNTG_NetMngr.GetSingleton().MyCreateSocket.SendUnLockFish();
		if (!bAutoFire)
		{
			bAuto = false;
		}
	}

	public void PressBg()
	{
		if (DNTG_TipManager.getInstance().mTipType == DNTG_TipType.NoneTip)
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
			if (All_TipCanvas.GetInstance().IsPayPanelActive())
			{
				return;
			}
			if (sptDlgSet.gameObject.activeSelf)
			{
				sptDlgSet.SaveUserSetting();
				sptDlgSet.gameObject.SetActive(value: false);
				return;
			}
			if (All_TipCanvas.GetInstance().IsCheckPwdActive())
			{
				return;
			}
		}
		sptGunCtrl.CaculateGunDir();
		if (area.sptLock.lockProcess == DNTG_ELOCK.Locked)
		{
			CancelLockFishAutoFire();
			area.sptLock.lockProcess = DNTG_ELOCK.EndMoving;
		}
		if (!bAutoFire)
		{
			if (gameInfo.User.ScoreCount <= 0)
			{
				ShowTopUp();
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

	public void ClickFish()
	{
		if (DNTG_TipManager.getInstance().mTipType == DNTG_TipType.NoneTip)
		{
			if (!bClickGameBtn)
			{
				HideButtonBar();
			}
			if (!sptGameBtns.imgExcharge.gameObject.activeSelf && !All_TipCanvas.GetInstance().IsPayPanelActive() && !sptDlgSet.gameObject.activeSelf && !All_TipCanvas.GetInstance().IsCheckPwdActive())
			{
				sptGunCtrl.CaculateGunDir();
				ManuFired();
			}
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
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.addGun);
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
		UpdateWhenChangeGun(gameInfo.User.SeatIndex, gameInfo.User.GunValue, bInit: false, DNTG_GameInfo.getInstance().IsSuperShoot[gameInfo.User.SeatIndex - 1]);
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
			ShowTopUp();
			for (int i = 0; i < gameInfo.UserList.Count; i++)
			{
				if (gameInfo.UserList[i].SeatIndex == gameInfo.User.SeatIndex)
				{
					gameInfo.UserList[i].Lock = false;
					gameInfo.UserList[i].LockFish = null;
					DNTG_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				}
			}
		}
		else
		{
			bAutoFire = true;
		}
	}

	public bool IsLock()
	{
		for (int i = 0; i < gameInfo.UserList.Count; i++)
		{
			if (gameInfo.UserList[i].SeatIndex == gameInfo.User.SeatIndex)
			{
				return gameInfo.UserList[i].Lock;
			}
		}
		return false;
	}

	public void CancelLockFishAutoFire()
	{
		DNTG_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
		if (!(btnAuto.image.sprite == spiAutoPlay[language * 2 + 1]))
		{
			bAutoFire = false;
			autoDeltaTime = 0f;
		}
	}

	private void SetBtnAutoEff(bool isShow)
	{
		if (btnAuto != null)
		{
			if (btnAutoEff == null)
			{
				btnAutoEff = btnAuto.transform.GetChild(0).gameObject;
			}
			if (btnAutoEff != null)
			{
				btnAutoEff.SetActive(isShow);
			}
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

	private void SelfCancelLock(DNTG_UserInfo self)
	{
		CancelLockFishAutoFire();
		GetCurrentGun(gameInfo.User.SeatIndex).EndLock();
	}

	public DNTG_Lock GetCurrentGun(int seatid)
	{
		DNTG_Lock result = null;
		if (seatid == gameInfo.User.SeatIndex)
		{
			return area.sptLock;
		}
		for (int i = 0; i < 4; i++)
		{
			if (areas[i].name.text == ZH2_GVars.GetBreviaryName(gameInfo.UserList[i].Name) + " " + seatid + "#")
			{
				return areas[i].sptLock;
			}
		}
		return result;
	}

	private void OnDisable()
	{
		ZH2_GVars.closeSafeBox = (Action)Delegate.Remove(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
	}
}
