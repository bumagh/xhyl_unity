using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TF_Area
{
	public GameObject area;

	public Text name;

	public Text gameScore;

	public Text txtFixTime;

	public TF_Lock sptLock;

	public TF_GunCtrl sptGC;

	public GameObject objChangeGun;

	public GameObject objEnergeGun;

	public GameObject overFlow;

	public TF_ErrorTipAnim sptTipOverFlow;

	public Text gunNum;

	public Image board;

	public Button btnUser;

	public TF_ChatCtrl sptCC;

	public Transform coins;

	public List<GameObject> coinList;

	public List<TF_CoinItem> extraCoinList;

	public int lastDepth = 3;

	public int nextCoinColor;

	public bool isEnergyOver = true;

	public bool bUp;
}
