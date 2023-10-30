using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DK_ChatCtrl : MonoBehaviour
{
	private GameObject objChat;

	private Text txtChat;

	private bool bBubleEnd;

	public Queue<string> queChat;

	private void Awake()
	{
		bBubleEnd = true;
		queChat = new Queue<string>();
		objChat = base.transform.Find("Chat").gameObject;
		objChat.SetActive(value: false);
		txtChat = objChat.transform.Find("ImgChat/Mask/TxtChat").GetComponent<Text>();
	}

	private void Update()
	{
		UpdateChatWords();
	}

	public void ShowChat(string words)
	{
		queChat.Enqueue(words);
	}

	private void UpdateChatWords()
	{
		if (bBubleEnd && queChat.Count > 0)
		{
			bBubleEnd = false;
			objChat.SetActive(value: true);
			txtChat.text = queChat.Dequeue();
			Vector2 sizeDelta = txtChat.rectTransform.sizeDelta;
			float y = sizeDelta.y;
			float num = 80f;
			float num2 = (y + num) / 2f;
			txtChat.transform.localPosition = Vector3.down * num2;
			float num3 = 30f;
			txtChat.transform.DOLocalMoveY(num2, num2 * 2f / num3).OnComplete(delegate
			{
				bBubleEnd = true;
				objChat.SetActive(value: false);
			});
		}
	}
}
