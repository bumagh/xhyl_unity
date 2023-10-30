using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Play Idle Animations")]
public class PlayIdleAnimations : MonoBehaviour
{
	private Animation mAnim;

	private AnimationClip mIdle;

	private List<AnimationClip> mBreaks = new List<AnimationClip>();

	private float mNextBreak;

	private int mLastIndex;

	private void Start()
	{
		mAnim = GetComponentInChildren<Animation>();
		if (mAnim == null)
		{
			UnityEngine.Debug.LogWarning(NGUITools.GetHierarchy(base.gameObject) + " has no Animation component");
			UnityEngine.Object.Destroy(this);
			return;
		}
		IEnumerator enumerator = mAnim.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				AnimationState animationState = (AnimationState)enumerator.Current;
				if (animationState.clip.name == "idle")
				{
					animationState.layer = 0;
					mIdle = animationState.clip;
					mAnim.Play(mIdle.name);
				}
				else if (animationState.clip.name.StartsWith("idle"))
				{
					animationState.layer = 1;
					mBreaks.Add(animationState.clip);
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
		if (mBreaks.Count == 0)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void Update()
	{
		if (!(mNextBreak < Time.time))
		{
			return;
		}
		if (mBreaks.Count == 1)
		{
			AnimationClip animationClip = mBreaks[0];
			mNextBreak = Time.time + animationClip.length + UnityEngine.Random.Range(5f, 15f);
			mAnim.CrossFade(animationClip.name);
			return;
		}
		int num = UnityEngine.Random.Range(0, mBreaks.Count - 1);
		if (mLastIndex == num)
		{
			num++;
			if (num >= mBreaks.Count)
			{
				num = 0;
			}
		}
		mLastIndex = num;
		AnimationClip animationClip2 = mBreaks[num];
		mNextBreak = Time.time + animationClip2.length + UnityEngine.Random.Range(2f, 8f);
		mAnim.CrossFade(animationClip2.name);
	}
}
