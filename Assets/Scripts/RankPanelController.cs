using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPanelController : MonoBehaviour
{
	[SerializeField]
	private GameObject m_rankButton;

	public static int rankButtonCount;

	[SerializeField]
	private Transform grid;

	public GameObject[] rankContent;

	public GameObject[] gameRankContent;

	[SerializeField]
	private GameObject m_rankGrid;

	[SerializeField]
	private GameObject m_gameRankGrid;

	[SerializeField]
	private int m_gameIndex;

	[SerializeField]
	private GameObject m_DayWeekMonthButton;

	[SerializeField]
	private UserIconDataConfig m_userIconDataConfig;

	[SerializeField]
	private InnerGameConfig m_InnerGameConfig;

	private List<GameObject> rankButtonGroup = new List<GameObject>();

	private Tween_SlowAction tween_SlowAction;

	private void Awake()
	{
		grid = base.transform.Find("Rank_bg/GameTypeButton/GameTypeButton_bg/Grid");
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("getRankList", HandleNetMsg_GameRankType);
		tween_SlowAction = GetComponent<Tween_SlowAction>();
	}

	private void Init()
	{
		rankButtonCount = m_InnerGameConfig.list.Count + 2;
		for (int i = 0; i < rankButtonCount; i++)
		{
			GameObject go = UnityEngine.Object.Instantiate(m_rankButton);
			rankButtonGroup.Add(go);
			go.name = i.ToString();
			switch (i)
			{
			case 0:
				go.transform.Find("Image0/Text").GetComponent<Text>().text = ZH2_GVars.ShowTip("财富榜", "Wealth list", "รายชื่อความมั่งคั่ง", "Tài nguyên");
				go.transform.Find("Image1/Text").GetComponent<Text>().text = ZH2_GVars.ShowTip("财富榜", "Wealth list", "รายชื่อความมั่งคั่ง", "Tài nguyên");
				break;
			case 1:
                    go.transform.Find("Image0/Text").GetComponent<Text>().text = ZH2_GVars.ShowTip("等级榜", "Level list", "รายการรายการ", "Bảng xếp hạng");
                    go.transform.Find("Image1/Text").GetComponent<Text>().text = ZH2_GVars.ShowTip("等级榜", "Level list", "รายการรายการ", "Bảng xếp hạng");
				break;
			default:
                    go.transform.Find("Image0/Text").GetComponent<Text>().text = ZH2_GVars.ShowTip(m_InnerGameConfig[i - 2].name_cn, m_InnerGameConfig[i - 2].name_en, m_InnerGameConfig[i - 2].name_cn, m_InnerGameConfig[i - 2].name_vn);
                    go.transform.Find("Image1/Text").GetComponent<Text>().text = ZH2_GVars.ShowTip(m_InnerGameConfig[i - 2].name_cn, m_InnerGameConfig[i - 2].name_en, m_InnerGameConfig[i - 2].name_cn, m_InnerGameConfig[i - 2].name_vn);
                    break;
			}
			go.transform.SetParent(grid);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			go.GetComponent<Button>().onClick.AddListener(delegate
			{
				int num = int.Parse(go.name);
				m_gameIndex = ((num < 2) ? (num + 1) : m_InnerGameConfig[num - 2].rankType);
				Debug.LogError("点击了: " + go.name + " 发送序号: " + m_gameIndex + "  游戏名字: " + go.transform.Find("Image0/Text").GetComponent<Text>().text);
				rankButtonToggle(num);
				InitRankItem(m_gameIndex);
			});
		}
		rankButtonToggle(0);
	}

	private void OnEnable()
	{
		Debug.Log("OnEnable");
		foreach (GameObject item in rankButtonGroup)
		{
            Destroy(item);
		}
		rankButtonGroup.Clear();
		Init();
	}

	public void OnBtnClick_ChangeType(int index)
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getRankList", new object[2]
		{
			m_gameIndex,
			index + 1
		});
	}

	public void InitRankItem(int index)
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		m_DayWeekMonthButton.transform.Find("DayButton").GetComponent<Toggle>().isOn = true;
		m_DayWeekMonthButton.transform.Find("WeekButton").GetComponent<Toggle>().isOn = false;
		m_DayWeekMonthButton.transform.Find("MonthButton").GetComponent<Toggle>().isOn = false;
		Debug.Log("fasonglqingqiubaowen: " + index);
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getRankList", new object[2]
		{
			index,
			1
		});
	}

	private void HandleNetMsg_GameRankType(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		int num = (int)dictionary["type"];
		Debug.Log("type: " + num);
		if (num < 3)
		{
			object[] array = (object[])dictionary["rankList"];
			int num2 = array.Length;
			if (num2 != 0)
			{
				WealthLevel[] array2 = new WealthLevel[num2];
				for (int i = 0; i < num2; i++)
				{
					array2[i] = WealthLevel.CreateWithDic((Dictionary<string, object>)array[i]);
				}
				ShowWealthLevel(array2, num);
			}
		}
		else if (num > 2)
		{
			object[] array3 = (object[])dictionary["rankList"];
			int num3 = array3.Length;
			TopRank[] array4 = new TopRank[num3];
			for (int j = 0; j < num3; j++)
			{
				array4[j] = TopRank.CreateWithDic((Dictionary<string, object>)array3[j]);
			}
			ShowGameRank(array4);
		}
	}

	private void ShowWealthLevel(WealthLevel[] wl, int type)
	{
		int num = 0;
		int num2 = wl.Length - num;
		m_rankGrid.SetActive(value: true);
		m_gameRankGrid.SetActive(value: false);
		m_DayWeekMonthButton.SetActive(value: false);
		GameObject[] array = rankContent;
		foreach (GameObject gameObject in array)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(value: true);
			}
			else
			{
				Debug.LogError("====Go为空====");
			}
		}
		for (int j = 0; j < rankContent.Length; j++)
		{
			for (int k = 0; k < num2; k++)
			{
				try
				{
					rankContent[k].GetComponent<RankItem>().Init(m_userIconDataConfig, wl[k], type);
				}
				catch (Exception arg)
				{
					Debug.LogWarning("错误: " + arg);
				}
			}
			for (int l = num2; l < rankContent.Length; l++)
			{
				rankContent[l].SetActive(value: false);
			}
		}
	}

	private void ShowGameRank(TopRank[] tr)
	{
		Debug.Log("aaaaaaaaaaaaaaaaaaa");
		int num = tr.Length;
		m_rankGrid.SetActive(value: false);
		m_gameRankGrid.SetActive(value: true);
		m_DayWeekMonthButton.SetActive(value: true);
		GameObject[] array = gameRankContent;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(value: true);
		}
		for (int j = 0; j < gameRankContent.Length; j++)
		{
			for (int k = 0; k < num; k++)
			{
				gameRankContent[k].GetComponent<GameRankItem>().Init(tr[k]);
			}
			for (int l = num; l < gameRankContent.Length; l++)
			{
				gameRankContent[l].SetActive(value: false);
			}
		}
	}

	public void rankButtonToggle(int i)
	{
		foreach (GameObject item in rankButtonGroup)
		{
			item.transform.Find("Image1").gameObject.SetActive(value: false);
		}
		rankButtonGroup[i].transform.Find("Image1").gameObject.SetActive(value: true);
	}

	public void OnBtnClick_Close()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		Debug.Log(base.gameObject.name + ">>OnBtnClick_Close:");
		rankButtonToggle(0);
		GameObject[] array = rankContent;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(value: true);
		}
		GameObject[] array2 = gameRankContent;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(value: true);
		}
		if ((object)tween_SlowAction != null)
		{
			tween_SlowAction.Hide(base.gameObject);
		}
	}

	public void OnBtnClick_Return()
	{
		Debug.Log(base.gameObject.name + ">>OnBtnClick_Return:");
		base.gameObject.SetActive(value: false);
	}
}
