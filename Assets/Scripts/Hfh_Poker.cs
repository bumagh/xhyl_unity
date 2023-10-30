using UnityEngine;
using UnityEngine.UI;

public class Hfh_Poker : MonoBehaviour
{
	public Image Poker;

	public GameObject Hold;

	public Button ClickBtn;

	public bool IsHold;

	public bool IsPlayAudio;

	public int PokerNum;

	public static bool IsOneKing;

	public static bool IsTwoKing;

	public static bool IsThreeKing;

	public static bool IsFourKing;

	private void Awake()
	{
		Poker = base.transform.Find("Poker").GetComponent<Image>();
		Hold = base.transform.Find("Hold").gameObject;
		ClickBtn = base.transform.Find("Click").GetComponent<Button>();
		ClickBtn.onClick.AddListener(delegate
		{
			IsHold = !IsHold;
			Hold.SetActive(IsHold);
		});
		Reset();
		HeldDisable();
	}

	public void Clear()
	{
		IsHold = false;
		Hold.SetActive(IsHold);
		IsOneKing = false;
		IsTwoKing = false;
		IsThreeKing = false;
		IsFourKing = false;
		IsPlayAudio = false;
	}

	public void Reset()
	{
		IsPlayAudio = false;
		PokerNum = 0;
		Poker.transform.localScale = Vector3.one;
		Poker.sprite = Hfh_Singleton<Hfh_GameManager>.GetInstance().CardBack;
	}

	public void SetHeld()
	{
		IsHold = true;
		Hold.SetActive(IsHold);
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
		if (!IsOneKing && !IsTwoKing && !IsThreeKing && !IsFourKing && PokerNum == 52)
		{
			UnityEngine.Debug.Log("一个王");
			IsOneKing = true;
		}
		else if (IsOneKing && !IsTwoKing && !IsThreeKing && !IsFourKing && PokerNum == 52)
		{
			UnityEngine.Debug.Log("俩个王");
			PokerNum = 53;
			IsTwoKing = true;
		}
		else if (IsOneKing && IsTwoKing && !IsThreeKing && !IsFourKing && PokerNum == 52)
		{
			UnityEngine.Debug.Log("三个王");
			PokerNum = 54;
			IsThreeKing = true;
		}
		else if (IsOneKing && IsTwoKing && IsThreeKing && !IsFourKing && PokerNum == 52)
		{
			UnityEngine.Debug.Log("四个王");
			PokerNum = 55;
			IsFourKing = true;
		}
		else if (IsOneKing && IsTwoKing && IsThreeKing && IsFourKing && PokerNum == 52)
		{
			UnityEngine.Debug.Log("五个王");
			PokerNum = 56;
			IsOneKing = false;
			IsTwoKing = false;
			IsThreeKing = false;
			IsFourKing = false;
		}
		Poker.sprite = Hfh_Singleton<Hfh_GameManager>.GetInstance().CardSprites[PokerNum];
	}
}
