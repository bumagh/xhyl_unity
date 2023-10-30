using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_Control : MonoBehaviour
{
	public enum BetScenestatus
	{
		向上,
		向下
	}

	public static JSYS_Control publicControl;

	[Header("下注")]
	public BetScenestatus betScenestatus = BetScenestatus.向下;

	[Header("下注界面动画组件")]
	public Animator Betanimator;

	[Header("下注界面动画资源[0向上][1向下]")]
	public List<AnimationClip> BetanimationClips = new List<AnimationClip>();

	[Header("记录链表图片[0金鲨][1银鲨][2狮子][3老鹰][4熊猫][5孔雀][6猴子][7鸽子][8兔子][9燕子][10飞禽][11走兽]")]
	public List<Sprite> Recording = new List<Sprite>();

	[Header("记录父物体")]
	public GameObject RecordingObj;

	[Header("等待提示")]
	public GameObject waitScene;

	[Header("奖池链表")]
	public List<GameObject> Box = new List<GameObject>();

	[Header("鱼链表")]
	public List<GameObject> FishObj = new List<GameObject>();

	private int Counter;

	private string Num;

	public int ExceedTime;

	public GameObject ExceedTimeScene;

	private string allScore = "0";

	private float waitTime;

	private bool jiesuan;

	private bool show1;

	public static bool isMoreInfo;

	private int Yanzinum;

	private int roundnum;

	public List<string> NumList = new List<string>();

	private void Awake()
	{
		publicControl = this;
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendResultList(JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
	}

	private void Start()
	{
		StartCoroutine(FishMove());
		ZH2_GVars.isCanSenEnterGame = true;
	}

	private IEnumerator WaitStart()
	{
		while (ZH2_GVars.isCanSenEnterGame)
		{
			yield return new WaitForSeconds(2.5f);
			try
			{
				WebSocket2.GetInstance().SenEnterGame(true, ZH2_GVars.GameType.jsys_desk, JSYS_LL_GameInfo.getInstance().UserInfo.TableId.ToString(), allScore);
			}
			catch (Exception ex)
			{
				Debug.LogError("错误: " + ex);
			}
		}
	}

	private void Update()
	{
		if (!jiesuan)
		{
			return;
		}
		waitTime += Time.deltaTime;
		if (!(waitTime > 3f))
		{
			return;
		}
		if (!show1)
		{
			StartCoroutine(JSYS_AnimationScene.publicAnimationScene.AnimatonMethon(Box[Counter].name));
			show1 = true;
		}
		else if (waitTime > 6f)
		{
			if (JSYS_link.moreInfo != string.Empty)
			{
				Box[Counter].transform.GetChild(0).gameObject.SetActive(false);
				waitTime = 0f;
				isMoreInfo = true;
				jiesuan = false;
				show1 = false;
				JSYS_link.Time_Dji = 17;
				JSYS_link.publiclink.parameter(JSYS_link.Time_Dji, JSYS_link.moreInfo);
				JSYS_link.name_ani = JSYS_link.moreInfo;
				StartCoroutine(JSYS_AnimationScene.publicAnimationScene.AnimatonMethon("tingzhi"));
				JSYS_link.moreInfo = string.Empty;
			}
			else
			{
				show1 = false;
				Box[Counter].transform.GetChild(0).gameObject.SetActive(false);
				JSYS_BetScene.publicBetScene.TimeMethon(2);
				jiesuan = false;
				waitTime = 0f;
				StartCoroutine(JSYS_AnimationScene.publicAnimationScene.AnimatonMethon("tingzhi"));
			}
		}
	}

	public void Init()
	{
		jiesuan = false;
		waitTime = 0f;
		roundnum = 0;
	}

	public IEnumerator Open(string Name)
	{
		Num = Name;
		NumList.Add(Num);
		float Sdu = 0.3f;
		Yanzinum = 0;
		while (true)
		{
			if (Counter == 0)
			{
				roundnum++;
			}
			if (Counter > 0)
			{
				Box[Counter - 1].transform.GetChild(0).gameObject.GetComponent<JSYS_TimeGuanBi>().GuanBi1();
			}
			else
			{
				Box[27].transform.GetChild(0).gameObject.GetComponent<JSYS_TimeGuanBi>().GuanBi1();
			}
			Box[Counter].transform.GetChild(0).gameObject.SetActive(true);
			if (roundnum <= 3)
			{
				if (Sdu > 0.05f)
				{
					Sdu -= 0.05f;
				}
				if (isMoreInfo && roundnum > 1)
				{
					if (Counter < Box.Count - 4)
					{
						if (Box[Counter + 4].name == Name)
						{
							Sdu = 0.6f;
							Yanzinum = Counter + 4;
						}
					}
					else if (Box[Counter + 4 - Box.Count].name == Name)
					{
						Sdu = 0.6f;
						Yanzinum = Counter + 4 - Box.Count;
					}
				}
				else if (Sdu > 0.05f)
				{
					Sdu -= 0.05f;
				}
			}
			else
			{
				if (Counter < Box.Count - 4)
				{
					if (Box[Counter + 4].name == Name)
					{
						Sdu = 0.6f;
						Yanzinum = Counter + 4;
					}
				}
				else if (Box[Counter + 4 - Box.Count].name == Name)
				{
					Sdu = 0.6f;
					Yanzinum = Counter + 4 - Box.Count;
				}
				if (Sdu < 0.2f)
				{
					Sdu += 0.01f;
				}
			}
			if (Box[Counter].name == Name && Counter == Yanzinum && (roundnum > 3 || (isMoreInfo && roundnum > 1)))
			{
				break;
			}
			Counter = (Counter + 1) % Box.Count;
			JSYS_Audio.publicAudio.BgAudioMethon("开始旋转");
			yield return new WaitForSeconds(Sdu);
		}
		isMoreInfo = false;
		StartCoroutine(JSYS_Audio.publicAudio.AudiioMethon(Box[Counter].name));
		JSYS_Audio.publicAudio.BgAudioMethon("停止旋转");
		Recordingmethon(Name);
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendResultList(JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
		jiesuan = true;
		Box[Counter].transform.GetChild(1).gameObject.SetActive(false);
		Box[Counter].transform.GetChild(2).gameObject.SetActive(true);
		yield return new WaitForSeconds(5f);
		Box[Counter].transform.GetChild(1).gameObject.SetActive(true);
		Box[Counter].transform.GetChild(2).gameObject.SetActive(false);
	}

	public void BetSceneMethon(string name)
	{
		switch (name)
		{
		case "向上":
			if (betScenestatus == BetScenestatus.向下)
			{
				Betanimator.Play(BetanimationClips[0].name);
				for (int i = 0; i < NumList.Count; i++)
				{
					MonoBehaviour.print("按钮动画" + NumList[i]);
					StartCoroutine(JSYS_BetScene.publicBetScene.ButtonAnimatorMethon(NumList[i]));
				}
				betScenestatus = BetScenestatus.向上;
			}
			break;
		case "向下":
			if (betScenestatus == BetScenestatus.向上)
			{
				Betanimator.Play(BetanimationClips[1].name);
				betScenestatus = BetScenestatus.向下;
			}
			break;
		}
	}

	public void Recordingmethon(string name)
	{
		int index = 0;
		switch (name)
		{
		case "金鲨":
			index = 0;
			break;
		case "银鲨":
			index = 1;
			break;
		case "狮子":
			index = 2;
			break;
		case "老鹰":
			index = 3;
			break;
		case "熊猫":
			index = 4;
			break;
		case "孔雀":
			index = 5;
			break;
		case "猴子":
			index = 6;
			break;
		case "鸽子":
			index = 7;
			break;
		case "兔子":
			index = 8;
			break;
		case "燕子":
			index = 9;
			break;
		}
		RecordingObj.transform.GetChild(19).GetComponent<Image>().sprite = Recording[index];
		RecordingObj.transform.GetChild(19).SetAsFirstSibling();
	}

	private IEnumerator FishMove()
	{
		while (true)
		{
			for (int i = 0; i < FishObj.Count; i++)
			{
				FishObj[i].transform.Translate(Vector3.right * FishObj[i].GetComponent<JSYS_Fish>().speed * Time.deltaTime);
			}
			yield return new WaitForSeconds(0.02f);
		}
	}

	public IEnumerator ExceedTimeMethon()
	{
		while (true)
		{
			ExceedTime++;
			if (ExceedTime == 0)
			{
				if (ExceedTimeScene.activeInHierarchy)
				{
					ExceedTimeScene.SetActive(false);
				}
			}
			else if (ExceedTime >= 2)
			{
				JSYS_link.publiclink.OpenNewTcpNet();
			}
			else if (ExceedTime >= 5)
			{
				ExceedTimeScene.SetActive(true);
			}
			yield return new WaitForSeconds(1f);
		}
	}
}
