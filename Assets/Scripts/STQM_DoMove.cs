using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class STQM_DoMove : MonoBehaviour
{
	public Vector3[] points;

	private TweenerCore<Vector3, Path, PathOptions> _TC_Path;

	public void DoPlay()
	{
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
	}

	public void Stop()
	{
		_TC_Path.Pause();
	}

	public void Play()
	{
		_TC_Path.Play();
	}

	public void Reset()
	{
		Stop();
		if (points.Length != 0)
		{
			base.transform.position = points[0];
		}
	}
}
