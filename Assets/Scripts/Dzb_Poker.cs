using UnityEngine;
using UnityEngine.UI;

public class Dzb_Poker : MonoBehaviour
{
	public enum ImageType
	{
		Cards,
		Fruit
	}

	public Image Poker;

	public GameObject Held;

	public Button ClickBtn;

	public bool IsHeld;

	public bool IsPlayAudio;

	public int PokerNum;

	public static bool IsOneKing;

	public static bool IsTwoKing;

	public ImageType _cardType;

	private void Awake()
	{
		Poker = base.transform.Find("Poker").GetComponent<Image>();
		Held = base.transform.Find("HELD").gameObject;
		ClickBtn = base.transform.Find("Click").GetComponent<Button>();
		ClickBtn.onClick.AddListener(delegate
		{
			IsHeld = !IsHeld;
			Held.SetActive(IsHeld);
		});
		Reset();
		HeldDisable();
	}

	public void Clear()
	{
		IsHeld = false;
		Held.SetActive(IsHeld);
		IsOneKing = false;
		IsTwoKing = false;
		IsPlayAudio = false;
	}

	public void Reset()
	{
		IsPlayAudio = false;
		PokerNum = 0;
		Poker.transform.localScale = Vector3.one;
		if (_cardType == ImageType.Cards)
		{
			Poker.sprite = Dzb_Singleton<Dzb_GameManager>.GetInstance().CardBack;
		}
		else if (_cardType == ImageType.Fruit)
		{
			Poker.sprite = Dzb_Singleton<Dzb_GameManager>.GetInstance().PCardBack;
		}
	}

	public void SetHeld()
	{
		IsHeld = true;
		Held.SetActive(IsHeld);
	}

	public void Stop()
	{
		ClickBtn.interactable = false;
	}

	public void HeldEnabled()
	{
		ClickBtn.interactable = true;
	}

	public void HeldDisable()
	{
		ClickBtn.interactable = false;
	}

	public void UpdateCard()
	{
		if (_cardType == ImageType.Cards)
		{
			if (!IsOneKing && !IsTwoKing && PokerNum == 52)
			{
				UnityEngine.Debug.Log("一个王");
				IsOneKing = true;
			}
			else if (IsOneKing && !IsTwoKing && PokerNum == 52)
			{
				UnityEngine.Debug.Log("俩个王");
				PokerNum = 53;
				IsTwoKing = true;
			}
			else if (IsOneKing && IsTwoKing && PokerNum == 52)
			{
				UnityEngine.Debug.Log("三个王");
				PokerNum = 54;
				IsOneKing = false;
				IsTwoKing = false;
			}
			Poker.sprite = Dzb_Singleton<Dzb_GameManager>.GetInstance().CardSprites[PokerNum];
		}
		else if (_cardType == ImageType.Fruit)
		{
			Poker.sprite = Dzb_Singleton<Dzb_GameManager>.GetInstance().PCardSprites[PokerNum];
		}
	}
}
