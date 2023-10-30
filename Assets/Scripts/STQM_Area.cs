using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STQM_Area
{
	public GameObject area;

	public Text name;

	public Text gameScore;

	public Text txtFixTime;

	public STQM_Lock sptLock;

	public STQM_GunCtrl sptGC;

	public GameObject objChangeGun;

	public GameObject objEnergeGun;

	public GameObject overFlow;

	public STQM_ErrorTipAnim sptTipOverFlow;

	public Text gunNum;

	public Image board;

	public Button btnUser;

	public STQM_ChatCtrl sptCC;

	public Transform coins;

	public List<GameObject> coinList;

	public List<STQM_CoinItem> extraCoinList;

	public int lastDepth = 3;

	public int nextCoinColor;

	public bool isEnergyOver = true;

	public bool bUp;
}
