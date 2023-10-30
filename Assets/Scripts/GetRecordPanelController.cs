using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRecordPanelController : MonoBehaviour
{
	public GameObject NoExist;

	public GameObject Content;

	private void Awake()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("getIncomeLog", HandleNetMsg_getIncomeLog);
	}

	private void OnEnable()
	{
		MB_Singleton<NetManager>.GetInstance().Send("shareService/getIncomeLog", new object[1]
		{
			ZH2_GVars.user.id
		});
	}

	private void HandleNetMsg_getIncomeLog(object[] objs)
	{
		UnityEngine.Debug.Log("HandleNetMsg_getIncomeLog");
		object[] array = objs[0] as object[];
		if (array.Length > 0)
		{
			Content.SetActive(value: true);
			NoExist.SetActive(value: false);
			if (Content.transform.childCount >= array.Length)
			{
				for (int i = 0; i < Content.transform.childCount; i++)
				{
					if (i < array.Length)
					{
						Dictionary<string, object> dictionary = array[i] as Dictionary<string, object>;
						Content.transform.GetChild(i).gameObject.SetActive(value: true);
						Content.transform.GetChild(i).GetChild(0).GetComponent<Text>()
							.text = dictionary["dateTime"].ToString();
							Content.transform.GetChild(i).GetChild(1).GetComponent<Text>()
								.text = ((double)dictionary["income"]).ToString("f2");
							}
							else
							{
								Content.transform.GetChild(i).gameObject.SetActive(value: false);
							}
						}
						return;
					}
					for (int j = 0; j < array.Length; j++)
					{
						Dictionary<string, object> dictionary2 = array[j] as Dictionary<string, object>;
						if (j < Content.transform.childCount)
						{
							Content.transform.GetChild(j).gameObject.SetActive(value: true);
							Content.transform.GetChild(j).GetChild(0).GetComponent<Text>()
								.text = dictionary2["dateTime"].ToString();
								Content.transform.GetChild(j).GetChild(1).GetComponent<Text>()
									.text = ((double)dictionary2["income"]).ToString("f2");
								}
								else
								{
									GameObject gameObject = UnityEngine.Object.Instantiate(Content.transform.GetChild(0).gameObject, Content.transform);
									gameObject.SetActive(value: true);
									gameObject.transform.GetChild(0).GetComponent<Text>().text = dictionary2["dateTime"].ToString();
									gameObject.transform.GetChild(1).GetComponent<Text>().text = ((double)dictionary2["income"]).ToString("f2");
								}
							}
						}
						else
						{
							Content.SetActive(value: false);
							NoExist.SetActive(value: true);
						}
					}
				}
