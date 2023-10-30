using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CampaignPanelController : MonoBehaviour
{
	private int curIndex;

	private List<GameObject> objTitles;

	private RectTransform rtfContentContent;

	public Transform contents_Left;

	public Transform contents_Right;

	private Color colDef = new Color(0.039f, 0.039f, 0.039f, 0.7843f);

	public GameObject activityTitleItemPre;

	public GameObject activityTextItemPre;

	public GameObject activityImgItemPre;

	public GameObject LoadAnim;

	private string getVIPUrl = string.Empty;

	private List<GameObject> activityObj_List = new List<GameObject>();

	private List<GameObject> activityObjTitel_List = new List<GameObject>();

	private Tween_SlowAction tween_SlowAction;

	private void Awake()
	{
		Init();
		MB_Singleton<NetManager>.Get().RegisterHandler("getAllActivity", HandleNetMsg_CheckGameVersion);
		tween_SlowAction = GetComponent<Tween_SlowAction>();
	}

	private void OnEnable()
	{
		curIndex = 0;
		MB_Singleton<NetManager>.Get().Send("gcuserService/getAllActivity", new object[0]);
	}

	public void OnClosePanel()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		if ((object)tween_SlowAction != null)
		{
			tween_SlowAction.Hide(base.gameObject);
		}
	}

	private void HandleNetMsg_CheckGameVersion(object[] args)
	{
		LoadAnim.SetActive(value: true);
		ResList();
		activityObj_List = new List<GameObject>();
		activityObjTitel_List = new List<GameObject>();
		JsonData jsonData = JsonMapper.ToObject(JsonMapper.ToJson(args[0]));
		UnityEngine.Debug.LogError("JD: " + jsonData.ToJson());
		int count = jsonData["activity"].Count;
		try
		{
			for (int i = 0; i < count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(activityTitleItemPre, contents_Left);
				gameObject.transform.localScale = Vector3.one;
				string text = jsonData[0][i]["name"].ToString();
				gameObject.transform.Find("Select0/TxtTitle").GetComponent<Text>().text = text;
				gameObject.transform.Find("Select1/TxtTitle").GetComponent<Text>().text = text;
				activityObjTitel_List.Add(gameObject);
				int num = (int)jsonData[0][i]["type"];
				if (num == 2)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(activityImgItemPre, contents_Right);
					gameObject2.transform.localScale = Vector3.one;
					activityObj_List.Add(gameObject2);
				}
				else
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(activityTextItemPre, contents_Right);
					gameObject3.transform.localScale = Vector3.one;
					gameObject3.transform.Find("TitelTxt").GetComponent<Text>().text = jsonData[0][i]["contentTitle"].ToString();
					gameObject3.transform.Find("Txt").GetComponent<Text>().text = jsonData[0][i]["content"].ToString();
					activityObj_List.Add(gameObject3);
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
		All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip( "系统响应错误", "System response error", "การตอบสนองของระบบผิดพลาด", "Lỗi phản hồi hệ thống"));
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

	private IEnumerator GetVipList(string url)
	{
		LoadAnim.SetActive(value: true);
		activityObj_List = new List<GameObject>();
		activityObjTitel_List = new List<GameObject>();
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			UnityEngine.Debug.LogError("JD: " + jsonData.ToJson());
			int num = int.Parse(jsonData["number"].ToString());
			try
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(activityTitleItemPre, contents_Left);
					gameObject.transform.localScale = Vector3.one;
					string text = jsonData["data"][i]["name"].ToString();
					gameObject.transform.Find("Select0/TxtTitle").GetComponent<Text>().text = text;
					gameObject.transform.Find("Select1/TxtTitle").GetComponent<Text>().text = text;
					activityObjTitel_List.Add(gameObject);
					GameObject gameObject2 = UnityEngine.Object.Instantiate(activityTextItemPre, contents_Right);
					gameObject2.transform.localScale = Vector3.one;
					gameObject2.transform.Find("TitelTxt").GetComponent<Text>().text = jsonData["data"][i]["content_title"].ToString();
					gameObject2.transform.Find("Txt").GetComponent<Text>().text = jsonData["data"][i]["content"].ToString();
					activityObj_List.Add(gameObject2);
					LoadAnim.SetActive(value: false);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
			InitShowActivity();
		}
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
		ResList();
	}

	private void ResList()
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
