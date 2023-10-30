using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHD_PlayerPanel : MonoBehaviour
{
	private Button closeBtn;

	public Transform content;

	private List<Transform> content_List;

	public List<Sprite> sprites;

	private void Awake()
	{
		closeBtn = base.transform.Find("CloseBtn").GetComponent<Button>();
		closeBtn.onClick.AddListener(Close);
		content = base.transform.Find("ScrollView/Viewport/Content");
		content_List = new List<Transform>();
		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			content_List.Add(child);
		}
	}

	private void OnEnable()
	{
		LHD_GameInfo instance = LHD_GameInfo.getInstance();
		instance.upDatePlayerList = (Action<JsonData>)Delegate.Combine(instance.upDatePlayerList, new Action<JsonData>(UpDatePlayerList));
		LHD_GameInfo instance2 = LHD_GameInfo.getInstance();
		instance2.getPlayerList = (Action)Delegate.Combine(instance2.getPlayerList, new Action(GetPlayerList));
		LHD_GameInfo.getInstance().getPlayerList?.Invoke();
	}

	private void GetPlayerList()
	{
		LHD_NetMngr.GetSingleton().MyCreateSocket.SendPlayerList(LHD_GameInfo.getInstance().roominfo.id);
	}

	private void OnDisable()
	{
		LHD_GameInfo instance = LHD_GameInfo.getInstance();
		instance.upDatePlayerList = (Action<JsonData>)Delegate.Remove(instance.upDatePlayerList, new Action<JsonData>(UpDatePlayerList));
		LHD_GameInfo instance2 = LHD_GameInfo.getInstance();
		instance2.getPlayerList = (Action)Delegate.Remove(instance2.getPlayerList, new Action(GetPlayerList));
	}

	private void UpDatePlayerList(JsonData jsonData)
	{
		UnityEngine.Debug.LogError("在线列表: " + jsonData.ToJson() + "  " + jsonData.Count);
		for (int i = 0; i < content_List.Count; i++)
		{
			content_List[i].gameObject.SetActive(value: false);
		}
		jsonData = BubbleSort(jsonData);
		for (int j = 0; j < jsonData.Count && j < content_List.Count; j++)
		{
			content_List[j].gameObject.SetActive(value: true);
			content_List[j].Find("NoText").GetComponent<Text>().text = (j + 1).ToString("00");
			content_List[j].Find("NickName").GetComponent<Text>().text = ZH2_GVars.GetBreviaryName(jsonData[j]["nickname"].ToString());
			int num = (int)jsonData[j]["headId"] - 1;
			if (num < 0 || num >= sprites.Count)
			{
				num = 0;
			}
			content_List[j].Find("Head").GetComponent<Image>().sprite = sprites[num];
			content_List[j].Find("GoldText").GetComponent<Text>().text = jsonData[j]["score"].ToString();
			content_List[j].Find("BetText").GetComponent<Text>().text = jsonData[j]["betScore"].ToString();
		}
	}

	private JsonData BubbleSort(JsonData jsonData)
	{
		JsonData jsonData2 = null;
		for (int i = 0; i < jsonData.Count; i++)
		{
			for (int j = 0; j < jsonData.Count - 1 - i; j++)
			{
				if ((int)jsonData[j]["score"] < (int)jsonData[j + 1]["score"])
				{
					jsonData2 = jsonData[j];
					jsonData[j] = jsonData[j + 1];
					jsonData[j + 1] = jsonData2;
				}
			}
		}
		return jsonData;
	}

	private void Close()
	{
		base.gameObject.SetActive(value: false);
	}
}
