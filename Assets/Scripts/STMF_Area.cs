using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STMF_Area
{
	public GameObject area;

	public Text name;

	public Text gameScore;

	public STMF_GunCtrl sptGC;

	public GameObject objChangeGun;

	public GameObject objEnergeGun;

	public GameObject overFlow;

	public STMF_ErrorTipAnim sptTipOverFlow;

	public Text gunNum;

	public Image board;

	public Button btnUser;

	public STMF_ChatCtrl sptCC;

	public Transform coins;

	public List<GameObject> coinList;

	public List<STMF_CoinItem> extraCoinList;

	public int lastDepth = 3;

	public int nextCoinColor;

	public bool isEnergyOver = true;
}
