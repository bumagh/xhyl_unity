using System;
using System.Collections;
using UnityEngine;

public class BCBM_RoleAnimationScript : MonoBehaviour
{
	public enum AnimationType
	{
		AT_STOP,
		AT_WAIT,
		AT_WIN
	}

	public AnimationClip mWaitAnimation;

	public AnimationClip mWinAnimation;

	private Animation _animation;

	public AnimationType _AnimState;

	private void Awake()
	{
		_animation = GetComponent<Animation>();
		IEnumerator enumerator = _animation.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				AnimationState animationState = (AnimationState)current;
				animationState.wrapMode = WrapMode.Loop;
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

	public void SetAnimation(AnimationType type)
	{
		_AnimState = type;
		if (!(mWaitAnimation == null) && !(mWinAnimation == null))
		{
			switch (type)
			{
			case AnimationType.AT_STOP:
				_animation[mWaitAnimation.name].time = 0f;
				_animation[mWinAnimation.name].time = 0f;
				_animation.Sample();
				_animation.enabled = false;
				break;
			case AnimationType.AT_WAIT:
				_animation.enabled = true;
				_animation.CrossFade(mWaitAnimation.name);
				break;
			case AnimationType.AT_WIN:
				_animation.enabled = true;
				_animation.Play(mWinAnimation.name);
				break;
			}
		}
	}
}
