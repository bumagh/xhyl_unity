using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class iTweenAlphaTo : iTweenEditor
{
	[Serializable]
	public class OnStart : UnityEvent
	{
	}

	[Serializable]
	public class OnComplete : UnityEvent
	{
	}

	public float valueFrom;

	public float valueTo = 1f;

	public OnStart onStart;

	public OnComplete onComplete;

	private SpriteRenderer _spriteRenderer;

	private Image _uiImage;

	private RawImage _uiRawImage;

	private CanvasGroup _uiCanvasGroup;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_uiImage = GetComponent<Image>();
		_uiRawImage = GetComponent<RawImage>();
		_uiCanvasGroup = GetComponent<CanvasGroup>();
		if (autoPlay)
		{
			iTweenPlay();
		}
	}

	public override void iTweenPlay()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", valueFrom);
		hashtable.Add("to", valueTo);
		hashtable.Add("time", tweenTime);
		hashtable.Add("delay", waitTime);
		hashtable.Add("looptype", loopType);
		hashtable.Add("easetype", easeType);
		hashtable.Add("onstart", (Action<object>)delegate
		{
			_onUpdate(valueFrom);
			if (onStart != null)
			{
				onStart.Invoke();
			}
		});
		hashtable.Add("onupdate", (Action<object>)delegate(object newVal)
		{
			_onUpdate((float)newVal);
		});
		hashtable.Add("oncomplete", (Action<object>)delegate
		{
			if (onComplete != null)
			{
				onComplete.Invoke();
			}
		});
		hashtable.Add("ignoretimescale", ignoreTimescale);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void _onUpdate(float value)
	{
		if (_spriteRenderer != null)
		{
			SpriteRenderer spriteRenderer = _spriteRenderer;
			Color color = _spriteRenderer.color;
			float r = color.r;
			Color color2 = _spriteRenderer.color;
			float g = color2.g;
			Color color3 = _spriteRenderer.color;
			spriteRenderer.color = new Color(r, g, color3.b, value);
		}
		if (_uiImage != null)
		{
			Image uiImage = _uiImage;
			Color color4 = _uiImage.color;
			float r2 = color4.r;
			Color color5 = _uiImage.color;
			float g2 = color5.g;
			Color color6 = _uiImage.color;
			uiImage.color = new Color(r2, g2, color6.b, value);
		}
		if (_uiRawImage != null)
		{
			RawImage uiRawImage = _uiRawImage;
			Color color7 = _uiRawImage.color;
			float r3 = color7.r;
			Color color8 = _uiRawImage.color;
			float g3 = color8.g;
			Color color9 = _uiRawImage.color;
			uiRawImage.color = new Color(r3, g3, color9.b, value);
		}
		if (_uiCanvasGroup != null)
		{
			_uiCanvasGroup.alpha = value;
		}
	}
}
