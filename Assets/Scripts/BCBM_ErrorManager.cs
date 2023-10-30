using BCBM_UICommon;
using UnityEngine;

public class BCBM_ErrorManager : MonoBehaviour
{
	protected bool mIsVisable;

	protected float mTotalTime;

	public bool mIsAutoDisable = true;

	public float mApartTime = 5f;

	protected float gTotalTime;

	protected UIPanel mErrorPanel;

	protected UITextList mErrorText;

	public static BCBM_ErrorManager gErrorMgr;

	public static BCBM_ErrorManager GetSingleton()
	{
		return gErrorMgr;
	}

	private void Awake()
	{
		if (!gErrorMgr)
		{
			gErrorMgr = this;
		}
	}

	private void Start()
	{
		mErrorPanel = GetComponent<UIPanel>();
		mErrorText = GetComponent<UITextList>();
		HideError();
	}

	private void Update()
	{
		if (mIsVisable && mIsAutoDisable)
		{
			mTotalTime += Time.deltaTime;
			if (mTotalTime >= mApartTime)
			{
				HideError();
			}
		}
	}

	public void SetError(EErrorType EType)
	{
		switch (EType)
		{
		case EErrorType.SeatIndexCrossBorder:
			mErrorText.Add("座位序号越界");
			break;
		case EErrorType.CreditBelowZero:
			mErrorText.Add("总分低于0");
			break;
		}
	}

	public void ShowError()
	{
		mErrorPanel.enabled = true;
		mIsVisable = true;
		if (mIsAutoDisable)
		{
			mTotalTime = 0f;
		}
	}

	public void HideError()
	{
		mErrorPanel.enabled = false;
		mIsVisable = false;
	}

	public void AddError(string strMessage)
	{
		mErrorText.Add(strMessage);
		if (mIsAutoDisable)
		{
			mTotalTime = 0f;
		}
		if (!mIsVisable)
		{
			ShowError();
		}
	}
}
