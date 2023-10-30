using System;
using System.Collections;
using UnityEngine;

public class BCBM_PtcBase : MonoBehaviour
{
	protected float _fTime;

	protected bool _isPlay;

	private void Start()
	{
	}

	private void Update()
	{
		OnUpdate();
	}

	protected void OnUpdate()
	{
		if (_fTime > 0f)
		{
			_fTime -= Time.deltaTime;
		}
		else if (_isPlay)
		{
			_isPlay = false;
			StopPtc();
		}
	}

	public virtual void PlayPtc(float time)
	{
		_fTime = time;
		_isPlay = true;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				ParticleSystem component = transform.GetComponent<ParticleSystem>();
				component.Stop();
				component.loop = true;
				component.enableEmission = true;
				component.Play();
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void StopPtc()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				ParticleSystem component = transform.GetComponent<ParticleSystem>();
				component.enableEmission = false;
				component.loop = false;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}
}
