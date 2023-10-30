using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STWM_MaryGameController : STWM_MB_Singleton<STWM_MaryGameController>
{
	[SerializeField]
	private GameObject _goUIContainer;

	[SerializeField]
	private STWM_TileManager tileManager;

	[SerializeField]
	private STWM_MaryCellManager cellManager;

	[SerializeField]
	private Image[] imgTileTimes = new Image[8];

	[SerializeField]
	private Text txtCredit;

	[SerializeField]
	private Text txtTotalWin;

	[SerializeField]
	private Text txtTotalBet;

	[SerializeField]
	private Text txtTimes;

	[SerializeField]
	private GameObject goBigWinTip;

	[SerializeField]
	private Text txtBigWinTip;

	[SerializeField]
	private GameObject goCellWin;

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

	public static STWM_CellType[] TileMap = new STWM_CellType[24]
	{
		STWM_CellType.None,
		STWM_CellType.Lu,
		STWM_CellType.Spear,
		STWM_CellType.Hall,
		STWM_CellType.Axe,
		STWM_CellType.Blade,
		STWM_CellType.None,
		STWM_CellType.Song,
		STWM_CellType.Lu,
		STWM_CellType.Spear,
		STWM_CellType.Axe,
		STWM_CellType.Sky,
		STWM_CellType.None,
		STWM_CellType.Lin,
		STWM_CellType.Blade,
		STWM_CellType.Axe,
		STWM_CellType.Lu,
		STWM_CellType.Song,
		STWM_CellType.None,
		STWM_CellType.Lin,
		STWM_CellType.Spear,
		STWM_CellType.Axe,
		STWM_CellType.Blade,
		STWM_CellType.Sky
	};

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		bAwake = true;
		if (STWM_MB_Singleton<STWM_MaryGameController>._instance == null)
		{
			STWM_MB_Singleton<STWM_MaryGameController>.SetInstance(this);
			PreInit();
		}
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
		cellManager.SetCells(new int[4]
		{
			1,
			1,
			1,
			1
		});
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
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("userService/maryStart", delegate
		{
		});
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("userService/maryStop", delegate
		{
		});
	}

	public void Show()
	{
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().ChangeView("MaryGame");
		STWM_Utils.TrySetActive(_goUIContainer, active: true);
		if (!bReset)
		{
			Init();
		}
	}

	public void Hide()
	{
		bEnterMary = false;
		STWM_Utils.TrySetActive(_goUIContainer, active: false);
	}

	public void Send_MaryStart()
	{
		object[] args = new object[0];
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/maryStart", args);
	}

	public void Send_MaryStop()
	{
		object[] args = new object[0];
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/maryStop", args);
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
			STWM_CellType tile = TileMap[photoNumberArray[i]];
			STWM_MaryComputer sTWM_MaryComputer = new STWM_MaryComputer((int)tile, photosArray[i]);
			sTWM_MaryComputer.Calculate();
			int num5 = sTWM_MaryComputer.totalRate * totalBet;
			num2 += num5;
			num4 += ((sTWM_MaryComputer.tileType == STWM_CellType.None) ? 1 : 0);
			if (num5 != totalWinArray[i])
			{
				num3++;
			}
			list.Add(num5);
		}
		if (num3 > 0)
		{
			string message = $"times: {times}, credit: {credit}, totalBet: {totalBet}, tiles: {STWM_Utils.PrintIntArray(photoNumberArray)}, types: {STWM_Utils.PrintIntArray(ConvertTilesToTypes(photoNumberArray))}, cells: {STWM_Utils.PrintInt2DJaggedArray(this.photosArray)}\n";
			string message2 = $"[times]:server:{times}, client: {num4}, [wins]:server: {STWM_Utils.PrintIntArray(totalWinArray)}, client: {STWM_Utils.PrintIntArray(list.ToArray())}";
			UnityEngine.Debug.Log(message);
			UnityEngine.Debug.LogError(message2);
		}
	}

	public void StartGame(int win)
	{
		STWM_SoundManager.Instance.PlayMaryBGM();
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
			cellManager.ResetCells();
			cellManager.HideAllBorders();
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
		yield return StartCoroutine(cellManager.Rolling(cells));
	}

	private IEnumerator PlayHitAni(int tile, int[] cells)
	{
		STWM_CellType xtileType = TileMap[tile];
		STWM_MaryComputer computer = new STWM_MaryComputer((int)xtileType, cells);
		computer.Calculate();
		Coroutine co = null;
		if (computer.cellHitType != 0)
		{
			STWM_SoundManager.Instance.PlayDrawLineAudio(0);
			co = StartCoroutine(CellWinBlink(computer.cellHitType));
		}
		yield return StartCoroutine(WaitForChasingFinish());
		bChasingFinish = false;
		if (computer.totalRate > 0)
		{
			StartCoroutine(STWM_Utils.DelayCall(0.5f, delegate
			{
				STWM_SoundManager.Instance.PlayMaryWinAudio();
			}));
			SetTotalWin(totalWin + totalBet * computer.totalRate);
			cellManager.SetCellsHitState(computer.aniDic);
			int tileType = 0;
			Coroutine r3 = null;
			Coroutine r2 = null;
			if (computer.isTileWin)
			{
				r3 = StartCoroutine(tileManager.TileBlink(tile));
				tileType = (int)TileMap[tile];
				r2 = StartCoroutine(BlinkTileTimes(tileType));
			}
			yield return new WaitForSeconds(STWM_Cell.CelebrateDuration);
			cellManager.SetAllCellsState();
			cellManager.HideAllBorders();
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

	private IEnumerator CellWinBlink(STWM_MaryCellHitType type)
	{
		goCellWin.SetActive(value: false);
		Transform transform = goCellWin.transform;
		Vector3 localPosition;
		switch (type)
		{
		default:
			localPosition = vecRight3;
			break;
		case STWM_MaryCellHitType.Left3:
			localPosition = vecLeft3;
			break;
		case STWM_MaryCellHitType.All:
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
		STWM_SoundManager.Instance.StopMaryBGM();
		if (!STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().JungeAuto())
		{
			STWM_SoundManager.Instance.PlayMaryBigWinAudio();
		}
		else
		{
			STWM_SoundManager.Instance.PlayMaryBigWinWithoutLaterAudio();
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
