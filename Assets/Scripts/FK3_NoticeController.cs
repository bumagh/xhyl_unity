using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FK3_NoticeController : FK3_SimpleSingletonBehaviour<FK3_NoticeController>
{
	[SerializeField]
	private Text _txtMsg;

	private Queue<string> _msgQueue;

	private bool isScrolling;

	private void Awake()
	{
		InitNotice();
	}

	public void SetInstance()
	{
		FK3_SimpleSingletonBehaviour<FK3_NoticeController>.s_instance = this;
		CloseNotice();
	}

	public void InitNotice()
	{
		_msgQueue = new Queue<string>();
	}

	public void AddMessage(string msg)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
			InitNotice();
		}
		UnityEngine.Debug.LogError("跑马灯信息" + msg);
		_msgQueue.Enqueue(msg);
		if (!isScrolling)
		{
			StartCoroutine(Scrolling());
		}
	}

	public IEnumerator Scrolling()
	{
		float beginX = 450f;
		float leftX = -400f;
		while (_msgQueue.Count > 0)
		{
			float speed = 200f;
			int loop = 1;
			string msg = _msgQueue.Dequeue();
			_txtMsg.text = msg;
			float txtWidth = _txtMsg.preferredWidth;
			Vector3 pos = _txtMsg.rectTransform.localPosition;
			float distance = beginX - leftX + txtWidth;
			float duration = distance / speed;
			isScrolling = true;
			while (true)
			{
				int num2;
				int num = num2 = loop;
				loop = num2 - 1;
				if (num <= 0)
				{
					break;
				}
				_txtMsg.rectTransform.localPosition = new Vector3(beginX, pos.y, pos.z);
				_txtMsg.rectTransform.DOLocalMoveX(0f - distance, duration).SetEase(Ease.Linear);
				yield return new WaitForSeconds(duration);
			}
			yield return null;
		}
		CloseNotice();
	}

	public void CloseNotice()
	{
		isScrolling = false;
		base.gameObject.SetActive(value: false);
	}
}
