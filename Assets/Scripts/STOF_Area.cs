using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STOF_Area
{
	public GameObject area;

	public Text name;

	public Text gameScore;

	public Text txtFixTime;

	public STOF_Lock sptLock;

	public STOF_GunCtrl sptGC;

	public GameObject objChangeGun;

	public GameObject objEnergeGun;

	public GameObject overFlow;

	public STOF_ErrorTipAnim sptTipOverFlow;

	public Text gunNum;

	public Image board;

	public Button btnUser;

	public STOF_ChatCtrl sptCC;

	public Transform coins;

	public List<GameObject> coinList;

	public List<STOF_CoinItem> extraCoinList;

	public int lastDepth = 3;

	public int nextCoinColor;

	public bool isEnergyOver = true;

	public bool bUp;
}
