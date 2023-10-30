using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MSE_TileManager : MonoBehaviour
{
	private Image[] imgs = new Image[24];

	private int curIndex;

	private void Awake()
	{
		for (int i = 0; i < 24; i++)
		{
			imgs[i] = base.transform.GetChild(i).GetComponent<Image>();
		}
		curIndex = 0;
	}

	private void Start()
	{
		_hideAllTiles();
	}

	public IEnumerator StartChasing(int targetIndex, Action endAction)
	{
		UnityEngine.Debug.Log("StartChasing> targetIndex: " + targetIndex);
		MSE_XWait xwait = new MSE_XWait(this).Begin();
		int count = 0;
		float timeCount = 0f;
		float timePoint3 = Time.time;
		int total = targetIndex - curIndex + 96 + 1;
		int speedUp = 4;
		int slowDown = 6;
		float lightTime3 = 0.5f;
		StartCoroutine(WaitSecondsAction(0.2f, delegate
		{
		}));
		for (int k = 0; k < speedUp; k++)
		{
			count++;
			float duration = 0.3f - (float)k * 0.07f;
			imgs[curIndex].enabled = true;
			StartCoroutine(WaitSecondsAction(param1: curIndex, seconds: Mathf.Max(duration, lightTime3), action: delegate(int __index)
			{
				imgs[__index].enabled = false;
			}));
			curIndex = _getNextIndex();
			yield return xwait.WaitForSeconds(duration);
			timeCount += duration;
		}
		UnityEngine.Debug.Log("chasing> speed up time: " + timeCount + "s");
		UnityEngine.Debug.Log("chasing> speed up real time: " + (Time.time - timePoint3) + "s");
		timePoint3 = Time.time;
		lightTime3 = 0.07f;
		float frameTime = 1.4f / (float)(total - slowDown - count);
		for (int j = count; j < total - slowDown; j++)
		{
			count++;
			float duration2 = 0.02f;
			imgs[curIndex].enabled = true;
			StartCoroutine(WaitSecondsAction(param1: curIndex, seconds: Mathf.Max(duration2, lightTime3), action: delegate(int __index)
			{
				imgs[__index].enabled = false;
			}));
			curIndex = _getNextIndex();
			if (xwait.TestShouldYield(frameTime))
			{
				yield return xwait.WaitForSeconds(frameTime, hasAdd: true);
			}
			timeCount += duration2;
		}
		UnityEngine.Debug.Log("chasing> high speed real time: " + (Time.time - timePoint3) + "s");
		timePoint3 = Time.time;
		lightTime3 = 0.07f;
		for (int i = 0; i < slowDown; i++)
		{
			count++;
			float duration3 = 0.13f + (float)i * 0.12f;
			imgs[curIndex].enabled = true;
			int index3 = curIndex;
			if (curIndex == targetIndex)
			{
				break;
			}
			StartCoroutine(WaitSecondsAction(Mathf.Max(duration3, lightTime3), delegate(int __index)
			{
				imgs[__index].enabled = false;
			}, index3));
			curIndex = _getNextIndex();
			yield return xwait.WaitForSeconds(duration3);
			timeCount += duration3;
		}
		UnityEngine.Debug.Log("chasing> speed down real time: " + (Time.time - timePoint3) + "s");
		xwait.Stop();
		endAction?.Invoke();
	}

	public IEnumerator TileBlink(int tileIndex)
	{
		bool enable = true;
		for (int i = 0; i < 1000; i++)
		{
			imgs[tileIndex].enabled = enable;
			enable = !enable;
			yield return new WaitForSeconds(0.1f);
		}
		UnityEngine.Debug.Log("TileBlink timeout");
		yield return null;
	}

	private void _hideAllTiles()
	{
		for (int i = 0; i < 24; i++)
		{
			imgs[i].enabled = false;
		}
	}

	private int _getNextIndex()
	{
		return (curIndex + 1) % 24;
	}

	private int _getValidIndex(int index)
	{
		return index % 24;
	}

	public IEnumerator WaitFrameAction(int frame, Action action)
	{
		yield return frame;
		action();
	}

	public IEnumerator WaitSecondsAction(float seconds, Action action)
	{
		yield return new WaitForSeconds(seconds);
		action();
	}

	public IEnumerator WaitSecondsAction(float seconds, Action<int> action, int param1, MSE_XWait xwait = null)
	{
		if (xwait == null)
		{
			yield return new WaitForSeconds(seconds);
		}
		else
		{
			yield return xwait.WaitForSeconds(seconds);
		}
		action(param1);
	}
}
