using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityItem_signInCor : MonoBehaviour
{
	private Text ActTime;

	private Text AlreadySignInDay;

	private List<Transform> signDayList;

	private Transform dayList;

	private bool isNeed;

	private JsonData Jd;

	private Coroutine waitShowInfo;

	private void Awake()
	{
		ActTime = base.transform.Find("ActTime").GetComponent<Text>();
		AlreadySignInDay = base.transform.Find("signDay/Text").GetComponent<Text>();
		dayList = base.transform.Find("signDayList");
		signDayList = new List<Transform>();
		for (int i = 0; i < dayList.childCount; i++)
		{
			signDayList.Add(dayList.GetChild(i));
		}
		for (int j = 0; j < signDayList.Count; j++)
		{
			int index = j;
			Button button = signDayList[j].GetComponent<Button>();
			button.onClick.AddListener(delegate
			{
				SignClick(button.transform, index);
			});
		}
	}

	private void OnEnable()
	{
		Jd = new JsonData();
		ShowSignInfo();
		if (waitShowInfo != null)
		{
			StopCoroutine(waitShowInfo);
		}
		waitShowInfo = StartCoroutine(WaitShowInfo());
	}

	private void ShowSignInfo()
	{
		Jd = ZH2_GVars.activity_signIn;
		UnityEngine.Debug.LogError(Jd.ToJson());
		isNeed = (bool)Jd["need"];
		ActTime.text = Jd["timeout"]["startDate"].ToString() + "--" + Jd["timeout"]["endDate"].ToString();
		ZH2_GVars.alreadySignInDay = (int)Jd["alreadySignInDay"];
		AlreadySignInDay.text = $"已累计签到{ZH2_GVars.alreadySignInDay}天";
		int num = (int)Jd["signInDays"];
		JsonData jsonData = new JsonData();
		jsonData = Jd["signInAward"];
		for (int i = 0; i < jsonData.Count; i++)
		{
			for (int j = 0; j < signDayList.Count; j++)
			{
				if ((int)jsonData[i]["id"] == j + 1)
				{
					signDayList[j].Find("CoinNum").GetComponent<Text>().text = jsonData[i]["award"].ToString();
					break;
				}
			}
		}
		for (int k = 0; k < signDayList.Count; k++)
		{
			if (k < ZH2_GVars.alreadySignInDay)
			{
				signDayList[k].Find("signed").gameObject.SetActive(value: true);
			}
			else
			{
				signDayList[k].Find("signed").gameObject.SetActive(value: false);
			}
		}
	}

	private void SignClick(Transform @object, int index)
	{
		if (index > ZH2_GVars.alreadySignInDay || !isNeed || @object.Find("signed").gameObject.activeInHierarchy)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("您不满足签到条件或已经签到", "You do not meet the check-in criteria or have already checked in", string.Empty));
		}
		else
		{
			MB_Singleton<NetManager>.Get().Send("gcuserService/signIn", new object[1]
			{
				ZH2_GVars.user.id
			});
		}
	}

	private IEnumerator WaitShowInfo()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			if (Jd != ZH2_GVars.activity_signIn)
			{
				ShowSignInfo();
			}
		}
	}
}
