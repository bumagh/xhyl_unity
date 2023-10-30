using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHT_NoticeController : SHT_MB_Singleton<SHT_NoticeController>
{
	private Text m_textMsg;

	private Queue<string> m_msgQueue;

	private Font m_font;

	private bool m_isScrolling;

	public void PreInit()
	{
		FinText();
		m_msgQueue = new Queue<string>();
		m_font = m_textMsg.font;
	}

	public void Show()
	{
		FinText();
		base.gameObject.SetActive(value: true);
	}

	private void FinText()
	{
		if (m_textMsg == null)
		{
			m_textMsg = base.transform.Find("Image/Text").GetComponent<Text>();
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public IEnumerator Scrolling()
	{
		float beginX = 450f;
		while (m_msgQueue.Count > 0)
		{
			float speed = 200f;
			int loop = 2;
			string msg = m_msgQueue.Dequeue();
			m_textMsg.text = msg;
			float textWdith = GetTextWidth();
			UnityEngine.Debug.Log($"textWdith: {textWdith}");
			Vector3 pos = m_textMsg.rectTransform.localPosition;
			UnityEngine.Debug.Log($"localPosition: {pos}, positon: {m_textMsg.rectTransform.position}, pivot: {m_textMsg.rectTransform.pivot}");
			float distance = beginX + 410f + textWdith;
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
				UnityEngine.Debug.Log(loop);
				pos.x = beginX;
				m_textMsg.rectTransform.localPosition = pos;
				m_textMsg.rectTransform.DOLocalMoveX(beginX - distance, duration).SetEase(Ease.Unset);
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
		m_msgQueue.Enqueue(msg);
		if (!base.gameObject.activeSelf)
		{
			Show();
		}
		if (!m_isScrolling)
		{
			SHT_MB_Singleton<SHT_GameManager>.GetInstance().StartCoroutine(Scrolling());
		}
	}

	public void ChangeScene()
	{
		float y;
		switch (SHT_GVars.curView)
		{
		case "MajorGame":
			y = 314f;
			break;
		case "MaryGame":
			y = 227f;
			break;
		case "DiceGame":
			y = 250f;
			break;
		case "RoomSelectionView":
			y = 250f;
			break;
		case "DeskSelectionView":
			y = 250f;
			break;
		default:
			y = 250f;
			break;
		}
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = y;
		base.transform.GetChild(0).localPosition = localPosition;
	}
}
