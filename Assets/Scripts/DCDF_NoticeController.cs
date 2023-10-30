using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DCDF_NoticeController : DCDF_MB_Singleton<DCDF_NoticeController>
{
	[SerializeField]
	private Text m_textMsg;

	private Queue<string> m_msgQueue;

	private bool m_isScrolling;

	public void PreInit()
	{
		m_msgQueue = new Queue<string>();
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
		while (m_msgQueue.Count > 0)
		{
			float speed = 200f;
			int loop = 2;
			string msg = m_msgQueue.Dequeue();
			m_textMsg.text = msg;
			float textWdith = GetTextWidth();
			UnityEngine.Debug.Log($"textWdith: {textWdith}");
			Vector3 pos = m_textMsg.rectTransform.localPosition;
			float beginX = 360f + textWdith / 2f;
			float distance = 720f + textWdith;
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
		m_msgQueue.Enqueue(msg);
		if (!base.gameObject.activeSelf && !DCDF_MySqlConnection.bNoticeOff)
		{
			Show();
		}
		if (!m_isScrolling)
		{
			DCDF_MB_Singleton<DCDF_GameManager>.GetInstance().StartCoroutine(Scrolling());
		}
	}

	public void ChangeScene()
	{
		float y;
		switch (DCDF_MySqlConnection.curView)
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
