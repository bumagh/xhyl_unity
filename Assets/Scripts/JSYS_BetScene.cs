using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_BetScene : MonoBehaviour
{
	public static JSYS_BetScene publicBetScene;

	[Header("下注显示Text组件链表[0:得分][1:总分][2:下注]")]
	public List<Text> displayText = new List<Text>();

	public double[] displaylaue = new double[3];

	[Header("计数显示组件")]
	public Text CounterText;

	private int SwitchVakue = 10;

	private int laue;

	[Header("个人下注显示组件[0金鲨][1银鲨][2狮子][3老鹰][4熊猫][5孔雀][6猴子][7鸽子][8兔子][9燕子][10飞禽][11走兽]")]
	public List<Text> BetGerenText = new List<Text>();

	public int[] BetGerenValue = new int[12];

	[Header("总下注显示组件[0金鲨][1银鲨][2狮子][3老鹰][4熊猫][5孔雀][6猴子][7鸽子][8兔子][9燕子][10飞禽][11走兽]")]
	public List<Text> BetZongText = new List<Text>();

	public List<Text> BetText_List = new List<Text>();

	public int[] BetZongValue = new int[12];

	[HideInInspector]
	public int[] BetValue = new int[12];

	[Header("遮挡界面")]
	public GameObject OcclusionScene;

	[Header("按钮动画链表[0金鲨][1银鲨][2狮子][3老鹰][4熊猫][5孔雀][6猴子][7鸽子][8兔子][9燕子][10飞禽][11走兽]")]
	public List<Animator> Buttonanimators = new List<Animator>();

	public Text SwitchText;

	[HideInInspector]
	public int[] BetChips = new int[5];

	public Button SwitchBtn;

	public AudioClip[] audio_clip;

	private void Awake()
	{
		publicBetScene = this;
		SwitchBtn = base.transform.Find("Button/切换按钮").GetComponent<Button>();
		SwitchBtn.onClick.AddListener(SwitchButton2);
		SwitchText = SwitchBtn.transform.Find("Text").GetComponent<Text>();
		SwitchText.resizeTextForBestFit = true;
	}

	private void OnEnable()
	{
		SetBetChip(JSYS_LL_GameInfo.getInstance().BetChip);
	}

	private void Start()
	{
		StartCoroutine(displayMethon());
	}

	public void SetBetChip(int[] betChip)
	{
		BetChips = betChip;
		SwitchVakue = BetChips[0];
		SwitchText.text = SwitchVakue.ToString();
		laue++;
	}

	public void SwitchButton()
	{
	}

	public void SwitchButton2()
	{
		if (laue >= BetChips.Length || laue < 0)
		{
			laue = 0;
		}
		SwitchVakue = BetChips[laue];
		laue++;
		SwitchText.text = SwitchVakue.ToString();
	}

	public void BetButton(GameObject Obj)
	{
		string label = string.Empty;
		int iD = 0;
		int inix = 0;
		switch (Obj.name)
		{
		case "飞禽按钮":
			label = "10";
			iD = 56;
			inix = 10;
			break;
		case "金鲨按钮":
			label = "8";
			iD = 46;
			inix = 0;
			break;
		case "银鲨按钮":
			label = "9";
			iD = 47;
			inix = 1;
			break;
		case "走兽按钮":
			label = "11";
			iD = 57;
			inix = 11;
			break;
		case "兔子按钮":
			label = "0";
			iD = 54;
			inix = 8;
			break;
		case "猴子按钮":
			label = "1";
			iD = 52;
			inix = 6;
			break;
		case "熊猫按钮":
			label = "2";
			iD = 50;
			inix = 4;
			break;
		case "狮子按钮":
			label = "3";
			iD = 48;
			inix = 2;
			break;
		case "老鹰按钮":
			label = "7";
			iD = 49;
			inix = 3;
			break;
		case "孔雀按钮":
			label = "6";
			iD = 51;
			inix = 5;
			break;
		case "鸽子按钮":
			label = "5";
			iD = 53;
			inix = 7;
			break;
		case "燕子按钮":
			label = "4";
			iD = 55;
			inix = 9;
			break;
		}
		JSYS_link.publiclink.BetMethon(SwitchVakue, label, iD, inix);
	}

	public void ContinueBet()
	{
		UnityEngine.Debug.LogError("UserId: " + JSYS_LL_GameInfo.getInstance().UserInfo.UserId);
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SenContinueBet(JSYS_LL_GameInfo.getInstance().UserInfo.UserId, JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
	}

	public void CancelBet()
	{
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendCancelBet(JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
	}

	private IEnumerator displayMethon()
	{
		int[] TempBetLv = new int[12]
		{
			4,
			8,
			12,
			24,
			4,
			8,
			12,
			24,
			99,
			48,
			2,
			2
		};
		while (true)
		{
			for (int i = 0; i < displayText.Count; i++)
			{
				displayText[i].text = displaylaue[i].ToString();
				displayText[i].resizeTextForBestFit = true;
			}
			for (int j = 0; j < BetGerenText.Count; j++)
			{
				BetGerenText[j].text = BetGerenValue[j].ToString();
				BetGerenText[j].resizeTextForBestFit = true;
			}
			for (int k = 0; k < BetZongText.Count; k++)
			{
				BetZongText[k].text = BetZongValue[k].ToString();
				BetZongText[k].resizeTextForBestFit = true;
			}
			for (int l = 0; l < BetText_List.Count; l++)
			{
				if (JSYS_LL_GameInfo.getInstance() == null)
				{
					BetText_List[l].text = $"X{TempBetLv[l].ToString(string.Empty)}";
				}
				else
				{
					BetText_List[l].text = $"X{JSYS_LL_GameInfo.getInstance().BeiLv[l].ToString(string.Empty)}";
				}
				BetText_List[l].resizeTextForBestFit = true;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	public IEnumerator ButtonAnimatorMethon(string Name)
	{
		switch (Name)
		{
		case "金鲨":
			Buttonanimators[0].GetComponent<Animator>().enabled = true;
			break;
		case "银鲨":
			Buttonanimators[1].GetComponent<Animator>().enabled = true;
			break;
		case "兔子":
			Buttonanimators[8].GetComponent<Animator>().enabled = true;
			Buttonanimators[11].GetComponent<Animator>().enabled = true;
			break;
		case "猴子":
			Buttonanimators[6].GetComponent<Animator>().enabled = true;
			Buttonanimators[11].GetComponent<Animator>().enabled = true;
			break;
		case "熊猫":
			Buttonanimators[4].GetComponent<Animator>().enabled = true;
			Buttonanimators[11].GetComponent<Animator>().enabled = true;
			break;
		case "狮子":
			Buttonanimators[2].GetComponent<Animator>().enabled = true;
			Buttonanimators[11].GetComponent<Animator>().enabled = true;
			break;
		case "老鹰":
			Buttonanimators[3].GetComponent<Animator>().enabled = true;
			Buttonanimators[10].GetComponent<Animator>().enabled = true;
			break;
		case "孔雀":
			Buttonanimators[5].GetComponent<Animator>().enabled = true;
			Buttonanimators[10].GetComponent<Animator>().enabled = true;
			break;
		case "鸽子":
			Buttonanimators[7].GetComponent<Animator>().enabled = true;
			Buttonanimators[10].GetComponent<Animator>().enabled = true;
			break;
		case "燕子":
			Buttonanimators[9].GetComponent<Animator>().enabled = true;
			Buttonanimators[10].GetComponent<Animator>().enabled = true;
			break;
		}
		yield return new WaitForSeconds(4f);
	}

	public void Init_ani()
	{
		foreach (Animator buttonanimator in Buttonanimators)
		{
			buttonanimator.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			buttonanimator.GetComponent<Animator>().enabled = false;
		}
	}

	public void TimeMethon(int Timelaue)
	{
		switch (Timelaue)
		{
		case -1:
			CounterText.text = "0";
			JSYS_Audio.publicAudio.ButtonAudio.PlayOneShot(audio_clip[0]);
			return;
		case 2:
			CounterText.text = "0";
			JSYS_Control.publicControl.BetSceneMethon("向上");
			JSYS_Control.publicControl.waitScene.SetActive(value: true);
			return;
		case 47:
			JSYS_Control.publicControl.NumList.Clear();
			Init_ani();
			CounterText.text = "30";
			displaylaue[0] = 0.0;
			JSYS_Control.publicControl.BetSceneMethon("向上");
			OcclusionScene.SetActive(value: false);
			JSYS_Control.publicControl.waitScene.SetActive(value: false);
			JSYS_Audio.publicAudio.ButtonAudio.PlayOneShot(audio_clip[1]);
			return;
		case 5:
			for (int i = 0; i < BetGerenValue.Length; i++)
			{
				BetGerenValue[i] = 0;
			}
			displaylaue[2] = 0.0;
			OcclusionScene.SetActive(value: true);
			return;
		case 20:
			JSYS_Audio.publicAudio.ButtonAudio.PlayOneShot(audio_clip[2]);
			OcclusionScene.SetActive(value: true);
			break;
		case 19:
			JSYS_Audio.publicAudio.ButtonAudio.PlayOneShot(audio_clip[0]);
			break;
		case 18:
			JSYS_Audio.publicAudio.ButtonAudio.PlayOneShot(audio_clip[0]);
			break;
		case 17:
			JSYS_Control.publicControl.BetSceneMethon("向下");
			break;
		}
		if (Timelaue - 17 >= 0)
		{
			CounterText.text = string.Empty + (Timelaue - 17);
		}
	}

	public void DeFenhgMethon(string Name)
	{
		switch (Name)
		{
		case "金鲨":
			displaylaue[0] = 91 * BetGerenValue[0];
			break;
		case "银鲨":
			displaylaue[0] = 24 * BetGerenValue[1];
			break;
		case "狮子":
			displaylaue[0] = 12 * BetGerenValue[2] + BetGerenValue[11] * 2;
			break;
		case "老鹰":
			displaylaue[0] = 12 * BetGerenValue[3] + BetGerenValue[10] * 2;
			break;
		case "熊猫":
			displaylaue[0] = 8 * BetGerenValue[4] + BetGerenValue[11] * 2;
			break;
		case "孔雀":
			displaylaue[0] = 8 * BetGerenValue[5] + BetGerenValue[10] * 2;
			break;
		case "猴子":
			displaylaue[0] = 8 * BetGerenValue[6] + BetGerenValue[11] * 2;
			break;
		case "鸽子":
			displaylaue[0] = 8 * BetGerenValue[7] + BetGerenValue[10] * 2;
			break;
		case "兔子":
			displaylaue[0] = 6 * BetGerenValue[8] + BetGerenValue[11] * 2;
			break;
		case "燕子":
			displaylaue[0] = 6 * BetGerenValue[9] + BetGerenValue[10] * 2;
			break;
		}
	}
}
