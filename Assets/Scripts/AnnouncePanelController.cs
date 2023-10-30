using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncePanelController : MonoBehaviour
{
	private int curIndex;

	private List<GameObject> objTitles;

	private RectTransform rtfContentContent;

	public Transform contents_Left;

	public Transform contents_Right;

	private Color colDef = new Color(0.039f, 0.039f, 0.039f, 0.7843f);

	public GameObject activityTitleItemPre;

	public GameObject activityTextItemPre;

	public GameObject LoadAnim;

	private List<GameObject> activityObj_List = new List<GameObject>();

	private List<GameObject> activityObjTitel_List = new List<GameObject>();

	private Tween_SlowAction tween_SlowAction;

	private void Awake()
	{
		Init();
		MB_Singleton<NetManager>.Get().RegisterHandler("getNotice", HandleNetMsg_CheckGameVersion);
		tween_SlowAction = GetComponent<Tween_SlowAction>();
	}

	private void OnEnable()
	{
		curIndex = 0;
		tween_SlowAction.Show();
	}

	public void OnClosePanel()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		if ((object)tween_SlowAction != null)
		{
			tween_SlowAction.Hide(delegate
			{
				if (!ZH2_GVars.isStartedFromGame)
				{
					MB_Singleton<AppManager>.Get().m_mainPanel.OppenPanel("CampaignPanel");
					ZH2_GVars.isStartedFromGame = true;
				}
			}, base.gameObject);
		}
	}

	private void HandleNetMsg_CheckGameVersion(object[] args)
	{
		LoadAnim.SetActive(value: true);
		Rest();
		activityObj_List = new List<GameObject>();
		activityObjTitel_List = new List<GameObject>();
		JsonData jsonData = JsonMapper.ToObject(JsonMapper.ToJson(args[0]));
		UnityEngine.Debug.LogError("JD: " + jsonData.ToJson());
		int count = jsonData["notice"].Count;
		try
		{
			for (int i = 0; i < count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(activityTitleItemPre, contents_Left);
				gameObject.transform.localScale = Vector3.one;
				string text = jsonData[0][i]["title"].ToString();
				gameObject.transform.Find("Select0/TxtTitle").GetComponent<Text>().text = text;
				gameObject.transform.Find("Select1/TxtTitle").GetComponent<Text>().text = text;
				activityObjTitel_List.Add(gameObject);
				int num = (int)jsonData[0][i]["type"];
				if (num != 1)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(activityTextItemPre, contents_Right);
					gameObject2.transform.localScale = Vector3.one;
					gameObject2.transform.Find("TitelTxt").GetComponent<Text>().text = jsonData[0][i]["name"].ToString();
					gameObject2.transform.Find("Txt").GetComponent<Text>().text = jsonData[0][i]["content"].ToString();
					activityObj_List.Add(gameObject2);
				}
			}
			LoadAnim.SetActive(value: false);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("     " + arg);
			Err();
			return;
		}
		InitShowActivity();
	}

	private void Err()
    {
        All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("系统响应错误", "System response error", "การตอบสนองของระบบผิดพลาด", "Lỗi phản hồi hệ thống"));
        LoadAnim.SetActive(value: false);
		for (int i = 0; i < activityObj_List.Count; i++)
		{
			if (activityObj_List[i] != null)
			{
				UnityEngine.Object.Destroy(activityObj_List[i].gameObject);
			}
		}
		for (int j = 0; j < activityObjTitel_List.Count; j++)
		{
			if (activityObjTitel_List[j] != null)
			{
				UnityEngine.Object.Destroy(activityObjTitel_List[j].gameObject);
			}
		}
		activityObj_List = new List<GameObject>();
		activityObjTitel_List = new List<GameObject>();
	}

	private void InitShowActivity()
	{
		objTitles = new List<GameObject>();
		for (int i = 0; i < activityObjTitel_List.Count; i++)
		{
			objTitles.Add(activityObjTitel_List[i].transform.Find("Select1").gameObject);
			int temp = i;
			activityObjTitel_List[i].transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate
			{
				RechargeButtonToggle(temp);
			});
			objTitles[i].SetActive(i == 0);
		}
		for (int j = 0; j < activityObj_List.Count; j++)
		{
			activityObj_List[j].SetActive(j == 0);
		}
	}

	private void Init()
	{
		rtfContentContent = contents_Right.GetComponent<RectTransform>();
	}

	private void RechargeButtonToggle(int i)
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		if (curIndex != i)
		{
			rtfContentContent.localPosition = Vector2.zero;
			objTitles[curIndex].SetActive(value: false);
			activityObj_List[curIndex].SetActive(value: false);
			curIndex = i;
			objTitles[curIndex].SetActive(value: true);
			activityObj_List[curIndex].SetActive(value: true);
			SetSizeDelta();
		}
	}

	private void SetSizeDelta()
	{
		for (int i = 0; i < contents_Right.childCount; i++)
		{
			Transform transform = contents_Right.GetChild(i).Find("Txt");
			if (contents_Right.GetChild(i).gameObject.activeInHierarchy && (bool)transform)
			{
				rtfContentContent.sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
			}
			else if (contents_Right.GetChild(i).gameObject.activeInHierarchy)
			{
				rtfContentContent.sizeDelta = contents_Right.parent.GetComponent<RectTransform>().sizeDelta;
			}
		}
	}

	private void OnDisable()
	{
		Rest();
	}

	private void Rest()
	{
		for (int i = 0; i < activityObj_List.Count; i++)
		{
            Destroy(activityObj_List[i].gameObject);
		}
		for (int j = 0; j < activityObjTitel_List.Count; j++)
		{
            Destroy(activityObjTitel_List[j].gameObject);
		}
	}
}
