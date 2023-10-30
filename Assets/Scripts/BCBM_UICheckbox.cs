using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox")]
public class BCBM_UICheckbox : MonoBehaviour
{
	public delegate void OnStateChange(bool state);

	public static BCBM_UICheckbox current;

	public UISprite checkSprite;

	public Animation checkAnimation;

	public bool instantTween;

	public bool startsChecked = true;

	public Transform radioButtonRoot;

	public bool optionCanBeNone;

	public GameObject eventReceiver;

	public string functionName = "OnActivate";

	public OnStateChange onStateChange;

	[HideInInspector]
	[SerializeField]
	private bool option;

	private bool mChecked = true;

	private bool mStarted;

	private Transform mTrans;

	public bool isChecked
	{
		get
		{
			return mChecked;
		}
		set
		{
			if (radioButtonRoot == null || value || optionCanBeNone || !mStarted)
			{
				Set(value);
			}
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		if (checkSprite != null)
		{
			checkSprite.alpha = (startsChecked ? 1f : 0f);
		}
		if (option)
		{
			option = false;
			if (radioButtonRoot == null)
			{
				radioButtonRoot = mTrans.parent;
			}
		}
	}

	private void Start()
	{
		if (eventReceiver == null)
		{
			eventReceiver = base.gameObject;
		}
		mChecked = !startsChecked;
		mStarted = true;
		Set(startsChecked);
	}

	private void OnClick()
	{
		if (base.enabled)
		{
			isChecked = !isChecked;
		}
	}

	private void Set(bool state)
	{
		if (!mStarted)
		{
			mChecked = state;
			startsChecked = state;
			if (checkSprite != null)
			{
				checkSprite.alpha = (state ? 1f : 0f);
			}
		}
		else
		{
			if (mChecked == state)
			{
				return;
			}
			if (radioButtonRoot != null && state)
			{
				BCBM_UICheckbox[] componentsInChildren = radioButtonRoot.GetComponentsInChildren<BCBM_UICheckbox>(includeInactive: true);
				int i = 0;
				for (int num = componentsInChildren.Length; i < num; i++)
				{
					BCBM_UICheckbox bCBM_UICheckbox = componentsInChildren[i];
					if (bCBM_UICheckbox != this && bCBM_UICheckbox.radioButtonRoot == radioButtonRoot)
					{
						bCBM_UICheckbox.Set(state: false);
					}
				}
			}
			mChecked = state;
			if (checkSprite != null)
			{
				if (instantTween)
				{
					checkSprite.alpha = (mChecked ? 1f : 0f);
				}
				else
				{
					TweenAlpha.Begin(checkSprite.gameObject, 0.15f, mChecked ? 1f : 0f);
				}
			}
			current = this;
			if (onStateChange != null)
			{
				onStateChange(mChecked);
			}
			if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
			{
				eventReceiver.SendMessage(functionName, mChecked, SendMessageOptions.DontRequireReceiver);
			}
			current = null;
			if (checkAnimation != null)
			{
				ActiveAnimation.Play(checkAnimation, state ? Direction.Forward : Direction.Reverse);
			}
		}
	}
}
