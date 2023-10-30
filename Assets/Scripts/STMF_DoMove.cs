using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class STMF_DoMove : MonoBehaviour
{
	public STMF_PathManager pathContainer;

	private Vector3[] waypoints;

	private DOTweenPath tweenPath;

	[HideInInspector]
	public Animation anim;

	public AnimationClip walkAnim;

	public AnimationClip idleAnim;

	public bool crossfade;

	internal void Start()
	{
		if (!anim)
		{
			anim = GetComponentInChildren<Animation>();
		}
	}

	private void StartMove()
	{
		if (pathContainer == null)
		{
			UnityEngine.Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
			return;
		}
		InitTweenPath();
		waypoints = pathContainer.vecs;
		for (int i = 0; i < waypoints.Length; i++)
		{
			tweenPath.wps.Add(waypoints[i]);
		}
		tweenPath.DOPlay();
		PlayWalk();
	}

	private void InitTweenPath()
	{
		tweenPath = new DOTweenPath
		{
			easeType = Ease.Linear,
			loopType = LoopType.Yoyo,
			duration = 5f,
			pathType = PathType.CatmullRom,
			pathResolution = 10,
			orientType = OrientType.ToPath,
			lookAhead = 0f
		};
		tweenPath.wps.Clear();
	}

	public void Stop()
	{
		tweenPath.DOKill();
		tweenPath = null;
		PlayIdle();
	}

	public void Reset()
	{
		Stop();
		if ((bool)pathContainer)
		{
			base.transform.position = waypoints[0];
		}
	}

	internal void PlayIdle()
	{
		if ((bool)idleAnim)
		{
			if (crossfade)
			{
				anim.CrossFade(idleAnim.name, 0.2f);
			}
			else
			{
				anim.Play(idleAnim.name);
			}
		}
	}

	internal void PlayWalk()
	{
		if ((bool)walkAnim)
		{
			if (crossfade)
			{
				anim.CrossFade(walkAnim.name, 0.2f);
			}
			else
			{
				anim.Play(walkAnim.name);
			}
		}
	}
}
