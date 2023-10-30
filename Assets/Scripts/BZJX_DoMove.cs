using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class BZJX_DoMove : MonoBehaviour
{
	public Vector3[] points;

	private TweenerCore<Vector3, Path, PathOptions> _TC_Path;

	public bool bPlaying;

	public void DoPlay()
	{
		bPlaying = true;
		if (points.Length != 0)
		{
			base.transform.position = points[0];
			_TC_Path = base.transform.DOPath(points, 10f, PathType.CatmullRom).SetLookAt(0.01f).SetEase(Ease.Linear)
				.OnComplete(delegate
				{
					base.gameObject.SendMessage("OnPathEnd");
				});
		}
	}

	private void Update()
	{
		if (_TC_Path != null && _TC_Path.IsPlaying())
		{
			_TC_Path.timeScale = 0.5f;
		}
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.z = 0f;
		base.transform.localEulerAngles = localEulerAngles;
	}

	public void Stop()
	{
		bPlaying = false;
		_TC_Path.Pause();
	}

	public void Over()
	{
		bPlaying = false;
		_TC_Path.Pause();
		_TC_Path.Kill();
	}

	public void Play()
	{
		bPlaying = true;
		_TC_Path.Play();
	}

	public void Reset()
	{
		Over();
		if (points.Length != 0)
		{
			base.transform.position = points[0];
		}
	}
}
