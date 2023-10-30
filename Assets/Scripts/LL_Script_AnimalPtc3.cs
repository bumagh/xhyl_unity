using System;
using System.Collections;
using UnityEngine;

public class LL_Script_AnimalPtc3 : LL_PtcBase
{
	private Vector3 _PosStart = Vector3.zero;

	private float _fLen = 8f;

	private void Start()
	{
	}

	private void Update()
	{
		if (_isPlay)
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					Transform transform = (Transform)current;
					ParticleSystem component = transform.GetComponent<ParticleSystem>();
					Vector3 forward = Vector3.forward;
					component.transform.Translate(forward * Time.deltaTime * 3f, Space.Self);
					if (Vector3.Distance(component.transform.localPosition, _PosStart) > _fLen)
					{
						StopPtc();
						PlayPtc(3f);
					}
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

	public override void PlayPtc(float time)
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
				component.transform.localPosition = _PosStart;
				if (component.isPlaying)
				{
					component.Stop();
				}
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
}
