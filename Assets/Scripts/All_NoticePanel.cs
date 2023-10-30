using DG.Tweening;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class All_NoticePanel : MonoBehaviour
{
	private static All_NoticePanel publicNoticePanel;

	private Queue<JsonData> m_msgQueue;

	public Transform showTipObj;

	public Transform tipTextStartPos;

	public Transform tipTextTagPos;

	private Text tipText;

	private bool isShow;

	private string[] colors;

	public static All_NoticePanel GetInstance()
	{
		if (publicNoticePanel == null)
		{
			publicNoticePanel = All_TipCanvas.GetInstance().transform.Find("NoticePanel").GetComponent<All_NoticePanel>();
			publicNoticePanel.Awake2();
		}
		return publicNoticePanel;
	}

	private void Awake2()
	{
		publicNoticePanel = this;
		m_msgQueue = new Queue<JsonData>();
		showTipObj.gameObject.SetActive(value: false);
		tipText = showTipObj.Find("Text").GetComponent<Text>();
	}

	public void AddTip(JsonData jsonData)
	{
		m_msgQueue.Enqueue(jsonData);
		if (!isShow)
		{
			StartCoroutine(WaitShouTips());
		}
	}

	private IEnumerator WaitShouTips()
	{
		colors = new string[3]
		{
			"<color=#02FF47>",
			"<color=#FE9600>",
			"<color=#FFFB1E>"
		};
		while (m_msgQueue.Count > 0)
		{
			isShow = true;
			yield return new WaitForSeconds(1f);
			JsonData jsonData = m_msgQueue.Dequeue();
			string msg = jsonData["msg"].ToString();
			for (int i = 0; i < jsonData.Count - 1; i++)
			{
				string str = (i < colors.Length) ? colors[i] : colors[colors.Length - 1];
				string oldValue = "{" + i + "}";
				msg = msg.Replace(oldValue, str + jsonData[i.ToString()].ToString() + "</color>");
			}
			ShowTip(msg);
			yield return new WaitForSeconds(5f);
			HideTip();
		}
		isShow = false;
		UnityEngine.Debug.LogError("=====公告全部播报完毕====");
	}

	private void ShowTip(string tips)
	{
		showTipObj.gameObject.SetActive(value: true);
		showTipObj.localScale = Vector3.one;
		tipText.transform.localPosition = tipTextStartPos.localPosition;
		tipText.text = tips;
		tipText.transform.DOLocalMove(tipTextTagPos.localPosition, 0.35f);
	}

	private void HideTip()
	{
		showTipObj.DOScale(new Vector3(1f, 0f, 1f), 0.1f).OnComplete(delegate
		{
			showTipObj.gameObject.SetActive(value: false);
		});
	}
}
