using System;
using System.Collections;
using UnityEngine;

public class JSYS_LL_BackWallAnim : MonoBehaviour
{
	public enum WALL_COLOR_TYPE
	{
		WALL_RED_SHINE,
		WALL_GREEN_SHINE,
		WALL_YELLOW_SHINE,
		WALL_ALL_SHINE,
		WALL_BLUE
	}

	private GameObject _group1;

	private GameObject _group2;

	private ArrayList mColorWallList = new ArrayList();

	public Texture[] mWallColorTx;

	public Texture mBaseColorTx;

	private bool _isShowBaseColor = true;

	private float fAnimTime = 0.333333343f;

	private float fCurTime;

	private int _ncolorIndex;

	private Animation _animation;

	public AnimationClip mAnimClip;

	private bool _isWallAnimShow;

	private bool _isLastWallAnimShow;

	private WALL_COLOR_TYPE m_CurColorType = WALL_COLOR_TYPE.WALL_BLUE;

	private void Start()
	{
		_group1 = base.transform.Find("CJ_ShuiJin").gameObject;
		IEnumerator enumerator = _group1.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform value = (Transform)current;
				mColorWallList.Add(value);
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
		_animation = GetComponent<Animation>();
		IEnumerator enumerator2 = _animation.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object current2 = enumerator2.Current;
				AnimationState animationState = (AnimationState)current2;
				animationState.wrapMode = WrapMode.Once;
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		Reset();
	}

	private void Update()
	{
		_Shine();
	}

	public void ShowWallAnim(bool isShow)
	{
		if (_isWallAnimShow && !isShow)
		{
			GetComponent<Animation>()["WallDown"].normalizedTime = 1f;
			GetComponent<Animation>()["WallDown"].speed = -1f;
			GetComponent<Animation>().Play();
		}
		if (isShow)
		{
			GetComponent<Animation>()["WallDown"].speed = 0.8f;
			_animation.enabled = true;
			_animation.Play("WallDown");
		}
		_isWallAnimShow = isShow;
	}

	private void _Shine()
	{
		if (m_CurColorType == WALL_COLOR_TYPE.WALL_BLUE)
		{
			return;
		}
		fCurTime += Time.deltaTime;
		if (!(fCurTime > fAnimTime))
		{
			return;
		}
		fCurTime = 0f;
		switch (m_CurColorType)
		{
		case WALL_COLOR_TYPE.WALL_RED_SHINE:
		case WALL_COLOR_TYPE.WALL_GREEN_SHINE:
		case WALL_COLOR_TYPE.WALL_YELLOW_SHINE:
			if (_isShowBaseColor)
			{
				_SetAllColorWallTex(mBaseColorTx);
			}
			else
			{
				_SetAllColorWallTex(mWallColorTx[(int)m_CurColorType]);
			}
			_isShowBaseColor = !_isShowBaseColor;
			break;
		case WALL_COLOR_TYPE.WALL_ALL_SHINE:
			_SetAllColorWallTex(mWallColorTx[_ncolorIndex]);
			_ncolorIndex = (_ncolorIndex + 1) % 3;
			break;
		}
	}

	private void _SetAllColorWallTex(Texture colorTexture)
	{
		for (int i = 0; i < mColorWallList.Count; i++)
		{
			((Transform)mColorWallList[i]).GetComponent<Renderer>().sharedMaterial.mainTexture = colorTexture;
		}
	}

	public void ShowColorAnim(WALL_COLOR_TYPE Type)
	{
		m_CurColorType = Type;
		if (Type == WALL_COLOR_TYPE.WALL_BLUE)
		{
			_SetAllColorWallTex(mBaseColorTx);
		}
	}

	public void Reset()
	{
		ShowColorAnim(WALL_COLOR_TYPE.WALL_BLUE);
		ShowWallAnim(isShow: false);
		_ncolorIndex = 0;
	}
}
