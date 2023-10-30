using UnityEngine;

public class BaiJiaLe_DealerAnime : MonoBehaviour
{
	public static BaiJiaLe_DealerAnime instance;

	private Animator DealerAnimator;

	public Texture2D[] CardTextures;

	public Material[] CardMaterials;

	public GameObject[] CardObjects;

	public GameObject[] CardPosition;

	public GameObject DealCard1;

	public GameObject DealCard2;

	public GameObject[] RecycleCards;

	private void Awake()
	{
		instance = this;
		DealerAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.A))
		{
			DealerAnimator.Play("Card_Start");
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.S))
		{
			DealerAnimator.Play("Card_Reverse");
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.D))
		{
			DealerAnimator.Play("Card_Collect_1-6");
		}
	}

	public void PlayDealCards()
	{
		DealerAnimator.SetBool("XianZeng", value: false);
		DealerAnimator.SetBool("ZhuangZeng", value: false);
		DealerAnimator.SetInteger("Result", -1);
		DealerAnimator.Play("Card_Start");
	}

	public void ShowCards()
	{
		CardObjects[0].gameObject.SetActive(value: true);
		CardObjects[0].transform.position = CardPosition[0].transform.position;
		CardObjects[0].transform.rotation = CardPosition[0].transform.rotation;
		CardObjects[1].gameObject.SetActive(value: true);
		CardObjects[1].transform.position = CardPosition[1].transform.position;
		CardObjects[1].transform.rotation = CardPosition[1].transform.rotation;
		CardObjects[2].gameObject.SetActive(value: true);
		CardObjects[2].transform.position = CardPosition[2].transform.position;
		CardObjects[2].transform.rotation = CardPosition[2].transform.rotation;
		CardObjects[3].gameObject.SetActive(value: true);
		CardObjects[3].transform.position = CardPosition[3].transform.position;
		CardObjects[3].transform.rotation = CardPosition[3].transform.rotation;
	}

	public void PlayOpenCards(bool zz, bool xz, int rNum)
	{
		DealerAnimator.SetBool("XianZeng", xz);
		DealerAnimator.SetBool("ZhuangZeng", zz);
		DealerAnimator.SetInteger("Result", rNum);
		DealerAnimator.Play("Card_Reverse");
		UpdateCards();
	}

	public void HideCards()
	{
		for (int i = 0; i < CardObjects.Length; i++)
		{
			CardObjects[i].SetActive(value: false);
		}
	}

	public void ShowDealCard1()
	{
		DealCard1.SetActive(value: true);
	}

	public void ShowCard1()
	{
		DealCard1.SetActive(value: false);
		CardObjects[0].gameObject.SetActive(value: true);
		CardObjects[0].transform.position = CardPosition[0].transform.position;
		CardObjects[0].transform.rotation = CardPosition[0].transform.rotation;
	}

	public void ShowCard2()
	{
		DealCard1.SetActive(value: false);
		CardObjects[1].gameObject.SetActive(value: true);
		CardObjects[1].transform.position = CardPosition[1].transform.position;
		CardObjects[1].transform.rotation = CardPosition[1].transform.rotation;
	}

	public void ShowCard3()
	{
		DealCard1.SetActive(value: false);
		CardObjects[2].gameObject.SetActive(value: true);
		CardObjects[2].transform.position = CardPosition[2].transform.position;
		CardObjects[2].transform.rotation = CardPosition[2].transform.rotation;
	}

	public void ShowCard4()
	{
		DealCard1.SetActive(value: false);
		CardObjects[3].gameObject.SetActive(value: true);
		CardObjects[3].transform.position = CardPosition[3].transform.position;
		CardObjects[3].transform.rotation = CardPosition[3].transform.rotation;
	}

	public void ShowCard5()
	{
		CardObjects[4].gameObject.SetActive(value: true);
		CardObjects[4].transform.position = CardPosition[4].transform.position;
		CardObjects[4].transform.rotation = CardPosition[4].transform.rotation;
	}

	public void ShowCard6()
	{
		CardObjects[5].gameObject.SetActive(value: true);
		CardObjects[5].transform.position = CardPosition[5].transform.position;
		CardObjects[5].transform.rotation = CardPosition[5].transform.rotation;
	}

	public void HideXianCard()
	{
		CardObjects[0].SetActive(value: false);
		CardObjects[2].SetActive(value: false);
		DealCard1.SetActive(value: true);
		DealCard2.SetActive(value: true);
		DealCard1.transform.Find("card_00").GetComponent<MeshRenderer>().material.mainTexture = CardMaterials[0].mainTexture;
		DealCard2.transform.Find("card_00").GetComponent<MeshRenderer>().material.mainTexture = CardMaterials[2].mainTexture;
	}

	public void OpenXianCard()
	{
		RecycleCards[0].SetActive(value: true);
		RecycleCards[2].SetActive(value: true);
		DealCard1.SetActive(value: false);
		DealCard2.SetActive(value: false);
		BaiJiaLe_GameInfo.getInstance().XianValue = (BaiJiaLe_GameInfo.getInstance().GetCardNum(BaiJiaLe_GameInfo.getInstance().XianCards[0]) + BaiJiaLe_GameInfo.getInstance().GetCardNum(BaiJiaLe_GameInfo.getInstance().XianCards[1])) % 10;
		BaiJiaLe_Game.instance.ShowXianCount();
	}

	public void HideZhuangCard()
	{
		CardObjects[1].SetActive(value: false);
		CardObjects[3].SetActive(value: false);
		DealCard1.SetActive(value: true);
		DealCard2.SetActive(value: true);
		DealCard1.transform.Find("card_00").GetComponent<MeshRenderer>().material.mainTexture = CardMaterials[1].mainTexture;
		DealCard2.transform.Find("card_00").GetComponent<MeshRenderer>().material.mainTexture = CardMaterials[3].mainTexture;
	}

	public void OpenZhuangCard()
	{
		RecycleCards[1].SetActive(value: true);
		RecycleCards[3].SetActive(value: true);
		DealCard1.SetActive(value: false);
		DealCard2.SetActive(value: false);
		BaiJiaLe_GameInfo.getInstance().ZhuangValue = (BaiJiaLe_GameInfo.getInstance().GetCardNum(BaiJiaLe_GameInfo.getInstance().ZhuangCards[0]) + BaiJiaLe_GameInfo.getInstance().GetCardNum(BaiJiaLe_GameInfo.getInstance().ZhuangCards[1])) % 10;
		BaiJiaLe_Game.instance.ShowZhuangCount();
		if (DealerAnimator.GetBool("XianZeng"))
		{
			DealCard1.transform.Find("card_00").GetComponent<MeshRenderer>().material.mainTexture = CardMaterials[4].mainTexture;
		}
		else if (DealerAnimator.GetBool("ZhuangZeng"))
		{
			DealCard1.transform.Find("card_00").GetComponent<MeshRenderer>().material.mainTexture = CardMaterials[5].mainTexture;
		}
	}

	public void XianFillCard()
	{
		RecycleCards[4].SetActive(value: true);
		DealCard1.SetActive(value: false);
		BaiJiaLe_GameInfo.getInstance().XianValue = BaiJiaLe_GameInfo.getInstance().XianNum;
		BaiJiaLe_Game.instance.ShowXianCount();
		if (DealerAnimator.GetBool("ZhuangZeng"))
		{
			DealCard1.transform.Find("card_00").GetComponent<MeshRenderer>().material.mainTexture = CardMaterials[5].mainTexture;
		}
	}

	public void ZhuangFillCard()
	{
		RecycleCards[5].SetActive(value: true);
		DealCard1.SetActive(value: false);
		BaiJiaLe_GameInfo.getInstance().ZhuangValue = BaiJiaLe_GameInfo.getInstance().ZhuangNum;
		BaiJiaLe_Game.instance.ShowZhuangCount();
	}

	public void ShowWin()
	{
		BaiJiaLe_Game.instance.ShowWin();
		BaiJiaLe_Game.instance.RecyclingChip();
	}

	public void RecycleCard()
	{
		for (int i = 0; i < RecycleCards.Length; i++)
		{
			RecycleCards[i].SetActive(value: false);
		}
		for (int j = 0; j < CardObjects.Length; j++)
		{
			CardObjects[j].SetActive(value: false);
		}
	}

	public void UpdateCards()
	{
		if (BaiJiaLe_GameInfo.getInstance().XianCards[0] != 0)
		{
			CardMaterials[0].mainTexture = CardTextures[BaiJiaLe_GameInfo.getInstance().XianCards[0]];
		}
		if (BaiJiaLe_GameInfo.getInstance().XianCards[1] != 0)
		{
			CardMaterials[2].mainTexture = CardTextures[BaiJiaLe_GameInfo.getInstance().XianCards[1]];
		}
		if (BaiJiaLe_GameInfo.getInstance().XianCards[2] != 0)
		{
			CardMaterials[4].mainTexture = CardTextures[BaiJiaLe_GameInfo.getInstance().XianCards[2]];
		}
		if (BaiJiaLe_GameInfo.getInstance().ZhuangCards[0] != 0)
		{
			CardMaterials[1].mainTexture = CardTextures[BaiJiaLe_GameInfo.getInstance().ZhuangCards[0]];
		}
		if (BaiJiaLe_GameInfo.getInstance().ZhuangCards[1] != 0)
		{
			CardMaterials[3].mainTexture = CardTextures[BaiJiaLe_GameInfo.getInstance().ZhuangCards[1]];
		}
		if (BaiJiaLe_GameInfo.getInstance().ZhuangCards[2] != 0)
		{
			CardMaterials[5].mainTexture = CardTextures[BaiJiaLe_GameInfo.getInstance().ZhuangCards[2]];
		}
	}
}
