using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTM_XWait
{
	private List<float> _list;

	private MonoBehaviour _mb;

	private float _timeCount;

	private float _totalTime;

	private float _elapsedTime;

	public PTM_XWait(MonoBehaviour mb)
	{
		_list = new List<float>();
		_mb = mb;
		_timeCount = 0f;
		_totalTime = 0f;
	}

	public PTM_XWait Begin()
	{
		_elapsedTime = Time.time;
		return this;
	}

	public float Stop()
	{
		float num = Time.time - _elapsedTime;
		UnityEngine.Debug.Log($"elapsed: {num}s, totalWait: {_totalTime}s");
		return num;
	}

	public Coroutine WaitForSeconds(float sec, bool hasAdd = false)
	{
		return _mb.StartCoroutine(_wait(sec, hasAdd));
	}

	public bool TestShouldYield(float sec)
	{
		_timeCount += sec;
		return _timeCount > _totalTime;
	}

	private IEnumerator _wait(float sec, bool hasAdd = false)
	{
		if (!hasAdd)
		{
			_timeCount += sec;
		}
		while (_totalTime < _timeCount)
		{
			_totalTime += Time.deltaTime;
			yield return null;
		}
	}

	private float _getTotalTime()
	{
		return 0f;
	}
}
