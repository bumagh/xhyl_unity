using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSE_MaryGameController : MSE_MB_Singleton<MSE_MaryGameController>
{
	private GameObject _goUIContainer;

	private MSE_TileManager tileManager;

	private Image[] imgTileTimes = new Image[8];

	private Text txtCredit;

	private Text txtTotalWin;

	private Text txtTotalBet;

	private Text txtTimes;

	private GameObject goBigWinTip;

	private Text txtBigWinTip;

	private GameObject goCellWin;

	private Transform timesPares;

	private Vector3 vecLeft3 = Vector3.left * 285f + Vector3.down * 17f;

	private Vector3 vecRight3 = Vector3.right * 285f + Vector3.down * 17f;

	private Vector3 vecAll = Vector3.down * 17f;

	private int[] _totalWinArray;

	private int[][] photosArray;

	private int[] photoNumberArray;

	private int credit;

	private int totalBet;

	private int totalWin;

	private int times;

	public bool bEnterMary;

	public Action<int> onMaryGameEnd;

	private bool bReset;

	private bool bAwake;

	private bool bChasingFinish;

	public static MSE_CellType[] TileMap = new MSE_CellType[24]
	{
		MSE_CellType.None,
		MSE_CellType.Jian,
		MSE_CellType.Shi,
		MSE_CellType.HaiMa,
		MSE_CellType.Gou,
		MSE_CellType.Jiu,
		MSE_CellType.None,
		MSE_CellType.King,
		MSE_CellType.Jian,
		MSE_CellType.Shi,
		MSE_CellType.Gou,
		MSE_CellType.HaiXing,
		MSE_CellType.None,
		MSE_CellType.Quan,
		MSE_CellType.Jiu,
		MSE_CellType.Gou,
		MSE_CellType.Jian,
		MSE_CellType.King,
		MSE_CellType.None,
		MSE_CellType.Quan,
		MSE_CellType.Shi,
		MSE_CellType.Gou,
		MSE_CellType.Jiu,
		MSE_CellType.HaiXing
	};

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		InitFinGame();
		bAwake = true;
		if (MSE_MB_Singleton<MSE_MaryGameController>._instance == null)
		{
			MSE_MB_Singleton<MSE_MaryGameController>.SetInstance(this);
			PreInit();
		}
	}

	private void InitFinGame()
	{
		_goUIContainer = base.gameObject;
		tileManager = base.transform.Find("LightUp").GetComponent<MSE_TileManager>();
		timesPares = base.transform.Find("Times");
		for (int i = 0; i < timesPares.childCount; i++)
		{
			imgTileTimes[i] = timesPares.GetChild(i).GetComponent<Image>();
		}
		txtCredit = base.transform.Find("TxtCredit").GetComponent<Text>();
		txtTotalWin = base.transform.Find("TxtTotalWin").GetComponent<Text>();
		txtTotalBet = base.transform.Find("TxtTotalBet").GetComponent<Text>();
		txtTimes = base.transform.Find("TxtTime").GetComponent<Text>();
		goBigWinTip = base.transform.parent.Find("Win").gameObject;
		txtBigWinTip = goBigWinTip.transform.Find("Text").GetComponent<Text>();
		goCellWin = base.transform.Find("ImgCellWin").gameObject;
	}

	private void Start()
	{
		Init();
	}

	public void PreInit()
	{
		if (_goUIContainer == null)
		{
			_goUIContainer = base.gameObject;
		}
	}

	public void Init()
	{
		for (int i = 0; i < 8; i++)
		{
			imgTileTimes[i].enabled = false;
		}
		HideBigWinTip();
		HideCellWin();
		txtCredit.text = string.Empty;
		txtTimes.text = string.Empty;
		txtTotalBet.text = string.Empty;
		txtTotalWin.text = string.Empty;
		bReset = true;
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("userService/maryStart", delegate
		{
		});
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("userService/maryStop", delegate
		{
		});
	}

	public void Show()
	{
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().ChangeView("MaryGame");
		MSE_Utils.TrySetActive(_goUIContainer, active: true);
		if (!bReset)
		{
			Init();
		}
	}

	public void Hide()
	{
		bEnterMary = false;
		MSE_Utils.TrySetActive(_goUIContainer, active: false);
	}

	public void Send_MaryStart()
	{
		object[] args = new object[0];
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().Send("userService/maryStart", args);
	}

	public void Send_MaryStop()
	{
		object[] args = new object[0];
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().Send("userService/maryStop", args);
	}

	public void PrepareGame(int times, int credit, int totalBet, int[] photoNumberArray, int[][] photosArray, int[] totalWinArray)
	{
		SetCredit(credit);
		SetTotalBet(totalBet);
		SetTimes(times);
		this.photoNumberArray = photoNumberArray;
		this.photosArray = photosArray;
		_totalWinArray = totalWinArray;
		if (photoNumberArray.Length != photosArray.Length || photoNumberArray.Length != totalWinArray.Length)
		{
			UnityEngine.Debug.LogError("mary game data is not correct");
			UnityEngine.Debug.Log($"times: {times},photoNumberArray.length: {photoNumberArray.Length},:photosArray.Length {photosArray.Length},:totalWinArray.Length {totalWinArray.Length}");
		}
		int num = photoNumberArray.Length;
		int num2 = 0;
		int num3 = 0;
		List<int> list = new List<int>();
		int num4 = 0;
		for (int i = 0; i < num; i++)
		{
			MSE_CellType tile = TileMap[photoNumberArray[i]];
			MSE_MaryComputer mSE_MaryComputer = new MSE_MaryComputer((int)tile, photosArray[i]);
			mSE_MaryComputer.Calculate();
			int num5 = mSE_MaryComputer.totalRate * totalBet;
			num2 += num5;
			num4 += ((mSE_MaryComputer.tileType == MSE_CellType.None) ? 1 : 0);
			if (num5 != totalWinArray[i])
			{
				num3++;
			}
			list.Add(num5);
		}
		if (num3 > 0)
		{
			string message = $"times: {times}, credit: {credit}, totalBet: {totalBet}, tiles: {MSE_Utils.PrintIntArray(photoNumberArray)}, types: {MSE_Utils.PrintIntArray(ConvertTilesToTypes(photoNumberArray))}, cells: {MSE_Utils.PrintInt2DJaggedArray(this.photosArray)}\n";
			string message2 = $"[times]:server:{times}, client: {num4}, [wins]:server: {MSE_Utils.PrintIntArray(totalWinArray)}, client: {MSE_Utils.PrintIntArray(list.ToArray())}";
			UnityEngine.Debug.Log(message);
			UnityEngine.Debug.LogError(message2);
		}
	}

	public void StartGame(int win)
	{
		MSE_SoundManager.Instance.PlayMaryBGM();
		bReset = false;
		DisplayText();
		SetTotalWin(win);
		StartCoroutine(RunningControl(photoNumberArray, photosArray));
		Send_MaryStart();
	}

	public void DisplayText()
	{
		SetCredit(credit);
		SetTotalBet(totalBet);
		SetTotalWin(totalWin);
		SetTimes(times);
	}

	public void ResetGame()
	{
		if (bAwake)
		{
		}
	}

	private IEnumerator RunningControl(int[] endTiles, int[][] cellsArray)
	{
		int chaseTime = endTiles.Length;
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < chaseTime; i++)
		{
			yield return StartCoroutine(Running(endTiles[i], cellsArray[i]));
			yield return StartCoroutine(PlayHitAni(endTiles[i], cellsArray[i]));
			yield return new WaitForSeconds(0.5f);
			if (endTiles[i] % 6 == 0)
			{
				SetTimes(times - 1);
			}
		}
		ShowBigWinTip(totalWin);
		yield return new WaitForSeconds(3f);
		yield return null;
		Send_MaryStop();
		if (onMaryGameEnd != null)
		{
			onMaryGameEnd(totalWin);
		}
	}

	private IEnumerator Running(int endTile, int[] cells)
	{
		StartCoroutine(tileManager.StartChasing(endTile, delegate
		{
			bChasingFinish = true;
		}));
		yield break;
	}

	private IEnumerator PlayHitAni(int tile, int[] cells)
	{
		MSE_CellType xtileType = TileMap[tile];
		MSE_MaryComputer computer = new MSE_MaryComputer((int)xtileType, cells);
		computer.Calculate();
		Coroutine co = null;
		if (computer.cellHitType != 0)
		{
			MSE_SoundManager.Instance.PlayDrawLineAudio(0);
			co = StartCoroutine(CellWinBlink(computer.cellHitType));
		}
		yield return StartCoroutine(WaitForChasingFinish());
		bChasingFinish = false;
		if (computer.totalRate > 0)
		{
			StartCoroutine(MSE_Utils.DelayCall(0.5f, delegate
			{
			}));
			SetTotalWin(totalWin + totalBet * computer.totalRate);
			int tileType = 0;
			Coroutine r3 = null;
			Coroutine r2 = null;
			if (computer.isTileWin)
			{
				r3 = StartCoroutine(tileManager.TileBlink(tile));
				tileType = (int)TileMap[tile];
				r2 = StartCoroutine(BlinkTileTimes(tileType));
			}
			yield return new WaitForSeconds(0.2f);
			if (computer.isTileWin)
			{
				HideTileTimes(tileType);
				StopCoroutine(r3);
				StopCoroutine(r2);
			}
			if (computer.cellHitType != 0)
			{
				StopCoroutine(co);
				HideCellWin();
			}
		}
		else
		{
			yield return new WaitForSeconds(0.5f);
		}
		yield return null;
	}

	private IEnumerator WaitForChasingFinish()
	{
		while (!bChasingFinish)
		{
			yield return null;
		}
	}

	private IEnumerator ImageBlink(Image image, float interval)
	{
		while (true)
		{
			image.enabled = !image.enabled;
			yield return new WaitForSeconds(interval);
		}
	}

	private IEnumerator GameObjectBlink(GameObject go, float interval)
	{
		bool state = false;
		while (true)
		{
			go.SetActive(state);
			state = !state;
			yield return new WaitForSeconds(interval);
		}
	}

	private IEnumerator BlinkTileTimes(int type)
	{
		if (type != 0)
		{
			bool enable = true;
			for (int i = 0; i < 1000; i++)
			{
				imgTileTimes[type - 1].enabled = enable;
				enable = !enable;
				yield return new WaitForSeconds(0.1f);
			}
			UnityEngine.Debug.Log("_blinkTileTimes timeout");
			yield return null;
		}
	}

	private IEnumerator CellWinBlink(MSE_MaryCellHitType type)
	{
		goCellWin.SetActive(value: false);
		Transform transform = goCellWin.transform;
		Vector3 localPosition;
		switch (type)
		{
		default:
			localPosition = vecRight3;
			break;
		case MSE_MaryCellHitType.Left3:
			localPosition = vecLeft3;
			break;
		case MSE_MaryCellHitType.All:
			localPosition = vecAll;
			break;
		}
		transform.localPosition = localPosition;
		bool state = false;
		while (true)
		{
			goCellWin.SetActive(state);
			state = !state;
			yield return new WaitForSeconds(0.2f);
		}
	}

	private int[] ConvertTilesToTypes(int[] tiles)
	{
		int num = tiles.Length;
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (int)TileMap[tiles[i]];
		}
		return array;
	}

	private void SetTileTimesEnable(int type, bool isEnable)
	{
		imgTileTimes[type - 1].enabled = isEnable;
	}

	private void HideTileTimes(int type)
	{
		if (type != 0)
		{
			imgTileTimes[type - 1].enabled = false;
		}
	}

	private void SetCredit(int credit)
	{
		this.credit = credit;
		txtCredit.text = credit.ToString();
	}

	private void SetTotalWin(int totalWin)
	{
		this.totalWin = totalWin;
		txtTotalWin.text = totalWin.ToString();
	}

	private void SetTotalBet(int totalBet)
	{
		this.totalBet = totalBet;
		txtTotalBet.text = totalBet.ToString();
	}

	private void SetTimes(int times)
	{
		this.times = times;
		txtTimes.text = times.ToString();
	}

	private void ShowBigWinTip(int win)
	{
		goBigWinTip.SetActive(value: true);
		MSE_SoundManager.Instance.StopMaryBGM();
		if (!MSE_MB_Singleton<MSE_MajorGameController>.GetInstance().JungeAuto())
		{
		}
		txtBigWinTip.text = win.ToString();
	}

	private void HideBigWinTip()
	{
		goBigWinTip.SetActive(value: false);
	}

	private void HideCellWin()
	{
		goCellWin.SetActive(value: false);
	}
}
