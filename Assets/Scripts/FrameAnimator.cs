using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class FrameAnimator : MonoBehaviour
{
	[SerializeField]
	private Sprite[] frames;

	[SerializeField]
	private float framerate = 20f;

	[SerializeField]
	private bool ignoreTimeScale = true;

	[SerializeField]
	private bool loop = true;

	[SerializeField]
	private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 1f, 0f, 0f), new Keyframe(1f, 1f, 0f, 0f));

	public Action FinishEvent;

	private Image image;

	private SpriteRenderer spriteRenderer;

	private int currentFrameIndex;

	private float timer;

	private float currentFramerate = 20f;

	public Sprite[] Frames
	{
		get
		{
			return frames;
		}
		set
		{
			frames = value;
		}
	}

	public float Framerate
	{
		get
		{
			return framerate;
		}
		set
		{
			framerate = value;
		}
	}

	public bool IgnoreTimeScale
	{
		get
		{
			return ignoreTimeScale;
		}
		set
		{
			ignoreTimeScale = value;
		}
	}

	public bool Loop
	{
		get
		{
			return loop;
		}
		set
		{
			loop = value;
		}
	}

	public int NextIndex
	{
		get;
		set;
	}

	public void Reset()
	{
		currentFrameIndex = ((framerate < 0f) ? (frames.Length - 1) : 0);
	}

	public void Play()
	{
		Reset();
		base.enabled = true;
	}

	public void Pause()
	{
		base.enabled = false;
	}

	public void Stop()
	{
		Pause();
		Reset();
	}

	private void Awake()
	{
		image = GetComponent<Image>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (frames == null || frames.Length == 0)
		{
			base.enabled = false;
			return;
		}
		float num = curve.Evaluate((float)currentFrameIndex / (float)frames.Length);
		float num2 = num * framerate;
		if (num2 != 0f)
		{
			float num3 = (!ignoreTimeScale) ? Time.time : Time.unscaledTime;
			float num4 = Mathf.Abs(1f / num2);
			if (num3 - timer > num4)
			{
				DoUpdate();
			}
		}
	}

	private void DoUpdate()
	{
		NextIndex = currentFrameIndex + (int)Mathf.Sign(currentFramerate);
		if (NextIndex < 0 || NextIndex >= frames.Length)
		{
			if (FinishEvent != null)
			{
				FinishEvent();
			}
			if (!loop)
			{
				currentFrameIndex = Mathf.Clamp(currentFrameIndex, 0, frames.Length - 1);
				base.enabled = false;
				return;
			}
		}
		currentFrameIndex = NextIndex % frames.Length;
		if (image != null)
		{
			image.sprite = frames[currentFrameIndex];
		}
		else if (spriteRenderer != null)
		{
			spriteRenderer.sprite = frames[currentFrameIndex];
		}
		timer = ((!ignoreTimeScale) ? Time.time : Time.unscaledTime);
	}
}
