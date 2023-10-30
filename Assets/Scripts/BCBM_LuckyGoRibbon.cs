using UnityEngine;

public class BCBM_LuckyGoRibbon : MonoBehaviour
{
	private AnimationState mState;

	private Animator mAnimator;

	private Animation mAnim;

	private void Start()
	{
		mAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
	}

	private void OnAnimStart()
	{
	}

	private void OnAnimEnd()
	{
		mAnimator.StartPlayback();
	}
}
