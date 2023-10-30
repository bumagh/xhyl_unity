using DG.Tweening;
using JsonFx.Json;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeTipController : MonoBehaviour
{
	[SerializeField]
	private Text m_textMsg;

	private Queue<string> m_msgQueue;

	private Font m_font;

	private bool isInited;

	private bool m_isScrolling;

	private void Awake()
	{
		PreInit();
	}

	public void PreInit()
	{
		if (!isInited)
		{
			m_msgQueue = new Queue<string>();
			m_font = m_textMsg.font;
			isInited = true;
		}
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public IEnumerator Scrolling()
	{
		float beginX = 1600f;
		float leftX = -250f;
		while (m_msgQueue.Count > 0)
		{
			float speed = 200f;
			int loop = 3;
			string msg3 = m_msgQueue.Dequeue();
			msg3 = msg3.Replace("\n", "-");
			msg3 = msg3.Replace(" ", "-");
			m_textMsg.text = msg3;
			float textWdith = GetTextWidth();
			UnityEngine.Debug.Log($"textWdith: {textWdith}");
			Vector3 pos = m_textMsg.rectTransform.localPosition;
			UnityEngine.Debug.Log($"localPosition: {pos}, positon: {m_textMsg.rectTransform.position}, pivot: {m_textMsg.rectTransform.pivot}");
			float distance = beginX - leftX + textWdith + 10f;
			float duration = distance / speed;
			m_isScrolling = true;
			while (true)
			{
				int num2;
				int num = num2 = loop;
				loop = num2 - 1;
				if (num <= 0)
				{
					break;
				}
				m_textMsg.rectTransform.localPosition = new Vector3(beginX, pos.y, pos.z);
				m_textMsg.rectTransform.DOLocalMoveX(beginX - distance, duration).SetEase(Ease.Linear);
				yield return new WaitForSeconds(duration);
			}
			yield return null;
		}
		m_isScrolling = false;
		Hide();
	}

	public IEnumerator ScrollingGame()
	{
		float beginX = 2600f;
		float leftX = -250f;
		while (m_msgQueue.Count > 0)
		{
			float speed = 200f;
			int loop = 3;
			string msg3 = m_msgQueue.Dequeue();
			msg3 = msg3.Replace("\n", "-");
			msg3 = msg3.Replace(" ", "-");
			m_textMsg.text = msg3;
			float textWdith = GetTextWidth();
			UnityEngine.Debug.Log($"textWdith: {textWdith}");
			Vector3 pos = m_textMsg.rectTransform.localPosition;
			UnityEngine.Debug.Log($"localPosition: {pos}, positon: {m_textMsg.rectTransform.position}, pivot: {m_textMsg.rectTransform.pivot}");
			float distance = beginX - leftX + textWdith + 10f;
			float duration = distance / speed;
			m_isScrolling = true;
			while (true)
			{
				int num2;
				int num = num2 = loop;
				loop = num2 - 1;
				if (num <= 0)
				{
					break;
				}
				m_textMsg.rectTransform.localPosition = new Vector3(beginX, pos.y, pos.z);
				m_textMsg.rectTransform.DOLocalMoveX(beginX - distance, duration).SetEase(Ease.Linear);
				yield return new WaitForSeconds(duration);
			}
			yield return null;
		}
		m_isScrolling = false;
		Hide();
	}

	public float GetTextWidth()
	{
		return m_textMsg.preferredWidth;
	}

	public void AddMessage(string msg)
	{
		if (All_NoticePanel.GetInstance() != null)
		{
			string text = "{\"msg\":\"{0}\",\"0\":\"" + msg + "\"}";
			UnityEngine.Debug.LogError(text);
			JsonData jsonData = JsonMapper.ToObject(text);
			All_NoticePanel.GetInstance().AddTip(jsonData);
		}
	}

	public void ChangeScene()
	{
		float y = 250f;
		Vector3 localPosition = base.transform.localPosition;
		base.transform.localPosition = new Vector3(localPosition.x, y, localPosition.z);
	}

	public void HandleNetMsg_Notice(object[] args)
	{
		UnityEngine.Debug.Log(LogHelper.NetHandle("HandleNetMsg_Notice"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string text = (string)dictionary["content"];
		UnityEngine.Debug.Log(LogHelper.NetHandle($"msg: {text}"));
		AddMessage(text);
	}

	public void HandleNetMsg_Notice(object[] args, bool isF)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		string text = (string)jsonData["content"];
		UnityEngine.Debug.Log(LogHelper.NetHandle($"msg: {text}"));
		AddMessage(text);
	}
}
