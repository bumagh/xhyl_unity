using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_Area
{
	public GameObject area;

	public Text name;

	public Text gameScore;

	public Text txtFixTime;

	public DNTG_Lock sptLock;

	public DNTG_GunCtrl sptGC;

	public GameObject objChangeGun;

	public GameObject objEnergeGun;

	public GameObject overFlow;

	public DNTG_ErrorTipAnim sptTipOverFlow;

	public Text gunNum;

	public Image board;

	public DNTG_ChatCtrl sptCC;

	public Transform coins;

	public List<GameObject> coinList;

	public List<DNTG_CoinItem> extraCoinList;

	public int lastDepth = 3;

	public int nextCoinColor;

	public bool isEnergyOver = true;

	public bool bUp;
}
