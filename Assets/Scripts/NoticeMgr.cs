using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class NoticeMgr : BaseUIForm
{
	[SerializeField]
	private GameObject noticeObj;

	[SerializeField]
	private Text _txtMsg;

	private Queue<string> _msgQueue;

	private bool isScrolling;

	private float duration;

	private float lastTime;

	private float curTime;

	private static NoticeMgr instance;

	private int i;

	public static NoticeMgr Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		InitNotice();
		uiType.uiFormType = UIFormTypes.Front;
	}

	private void OnEnable()
	{
		instance = this;
	}

	public void InitNotice()
	{
		_msgQueue = new Queue<string>();
	}

	public void AddMessage(string msg)
	{
		if (!noticeObj.gameObject.activeSelf)
		{
			noticeObj.gameObject.SetActive(value: true);
			InitNotice();
		}
		_msgQueue.Enqueue(msg);
		if (!isScrolling)
		{
			Scroll();
		}
	}

	public IEnumerator Scrolling()
	{
		float beginX = 450f;
		float leftX = -295f;
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

	private void Update()
	{
	}

	private void Scroll()
	{
		float num = 630f;
		float num2 = 80f;
		string text = _msgQueue.Dequeue();
		if (_msgQueue.Count > 0)
		{
			num2 = 160f;
		}
		_txtMsg.text = text;
		float preferredWidth = _txtMsg.preferredWidth;
		Vector3 localPosition = _txtMsg.transform.localPosition;
		float num3 = num + preferredWidth - 400f;
		duration = num3 / num2;
		isScrolling = true;
		_txtMsg.rectTransform.localPosition = new Vector3(num, localPosition.y, localPosition.z);
		_txtMsg.rectTransform.DOLocalMoveX(0f - num3, duration).SetEase(Ease.Linear).OnComplete(delegate
		{
			if (_msgQueue.Count > 0)
			{
				Scroll();
			}
			else
			{
				CloseNotice();
			}
		});
	}

	public void CloseNotice()
	{
		isScrolling = false;
		noticeObj.gameObject.SetActive(value: false);
	}

	private void OnDisable()
	{
		CloseNotice();
	}
}
