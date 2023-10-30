using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using GameCommon;
using UnityEngine;

public class DNTG_DoMove : MonoBehaviour
{
	[HideInInspector]
	public Vector3[] points = new Vector3[0];

	private TweenerCore<Vector3, Path, PathOptions> _TC_Path;

	public bool bPlaying;

	private DNTG_ISwimObj _ISwimObj;

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
		_ISwimObj = GetComponent<DNTG_ISwimObj>();
	}

	private void Update()
	{
		if (_TC_Path != null && _TC_Path.IsPlaying())
		{
			_TC_Path.timeScale = 0.5f;
		}
		if (_ISwimObj != null)
		{
			if (_ISwimObj.specialFishType == SpecialFishType.MonkeyKing)
			{
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				localEulerAngles.z = localEulerAngles.x;
				localEulerAngles.x = 0f;
				localEulerAngles.y = 0f;
				base.transform.localEulerAngles = localEulerAngles;
			}
			else
			{
				SetOtherFish();
			}
		}
		else
		{
			SetOtherFish();
		}
	}

	private void SetOtherFish()
	{
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
