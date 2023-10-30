using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_GetPlayerList : MonoBehaviour
{
	public Transform content;

	private List<Transform> content_List;

	public List<Sprite> sprites;

	private void Awake()
	{
	}

	private void OnEnable()
	{
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendPlayerList(BCBM_GameInfo.getInstance().UserInfo.TableId);
		content_List = new List<Transform>();
		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			content_List.Add(child);
		}
		for (int j = 0; j < content_List.Count; j++)
		{
			Text component = content_List[j].transform.Find("CiText").GetComponent<Text>();
			component.text = (j + 1).ToString();
			if (j >= 3)
			{
				component.transform.GetChild(0).gameObject.SetActive(value: false);
			}
			else
			{
				component.transform.GetChild(0).gameObject.SetActive(value: true);
			}
			content_List[j].gameObject.SetActive(value: false);
		}
	}

	public void ShowPlayerList(JsonData jsonData)
	{
		jsonData = BubbleSort(jsonData);
		for (int i = 0; i < jsonData.Count && i < content_List.Count; i++)
		{
			content_List[i].gameObject.SetActive(value: true);
			content_List[i].Find("Ico/Name").GetComponent<Text>().text = ZH2_GVars.GetBreviaryName(jsonData[i]["nickname"].ToString());
			int num = (int)jsonData[i]["headId"] - 1;
			if (num < 0 || num >= sprites.Count)
			{
				num = 0;
			}
			content_List[i].Find("Ico/Image").GetComponent<Image>().sprite = sprites[num];
			content_List[i].Find("God/Text").GetComponent<Text>().text = jsonData[i]["score"].ToString();
			content_List[i].Find("bet/Text").GetComponent<Text>().text = jsonData[i]["betScore"].ToString();
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
}
