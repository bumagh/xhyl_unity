using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STTF_Area
{
	public GameObject area;

	public Text name;

	public Text gameScore;

	public Text txtFixTime;

	public STTF_Lock sptLock;

	public STTF_GunCtrl sptGC;

	public GameObject objChangeGun;

	public GameObject objEnergeGun;

	public GameObject overFlow;

	public STTF_ErrorTipAnim sptTipOverFlow;

	public Text gunNum;

	public Image board;

	public Button btnUser;

	public STTF_ChatCtrl sptCC;

	public Transform coins;

	public List<GameObject> coinList;

	public List<STTF_CoinItem> extraCoinList;

	public int lastDepth = 3;

	public int nextCoinColor;

	public bool isEnergyOver = true;

	public bool bUp;
}
