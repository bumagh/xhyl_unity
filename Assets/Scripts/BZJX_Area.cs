using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BZJX_Area
{
	public GameObject area;

	public Text name;

	public Text gameScore;

	public Text txtFixTime;

	public BZJX_Lock sptLock;

	public BZJX_GunCtrl sptGC;

	public GameObject objChangeGun;

	public GameObject objEnergeGun;

	public GameObject overFlow;

	public BZJX_ErrorTipAnim sptTipOverFlow;

	public Text gunNum;

	public Image board;

	public Button btnUser;

	public BZJX_ChatCtrl sptCC;

	public Transform coins;

	public List<GameObject> coinList;

	public List<BZJX_CoinItem> extraCoinList;

	public int lastDepth = 3;

	public int nextCoinColor;

	public bool isEnergyOver = true;

	public bool bUp;
}
