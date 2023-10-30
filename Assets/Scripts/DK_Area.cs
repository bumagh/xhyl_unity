using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DK_Area
{
	public GameObject area;

	public Text name;

	public Text gameScore;

	public Text txtFixTime;

	public DK_Lock sptLock;

	public DK_GunCtrl sptGC;

	public GameObject objChangeGun;

	public GameObject objEnergeGun;

	public GameObject overFlow;

	public DK_ErrorTipAnim sptTipOverFlow;

	public Text gunNum;

	public Image board;

	public Button btnUser;

	public DK_ChatCtrl sptCC;

	public Transform coins;

	public List<GameObject> coinList;

	public List<DK_CoinItem> extraCoinList;

	public int lastDepth = 3;

	public int nextCoinColor;

	public bool isEnergyOver = true;

	public bool bUp;
}
