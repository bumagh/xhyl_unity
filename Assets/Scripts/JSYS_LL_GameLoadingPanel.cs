using System.Collections;
using UnityEngine;

public class JSYS_LL_GameLoadingPanel : MonoBehaviour
{
	protected enum EGameLoad
	{
		logoFadeIn,
		logoFadeOut,
		loadingFadeIn,
		titleDropDown,
		loadingFadeOut,
		loadingComplete
	}

	protected bool mIsLoading = true;

	protected UIPanel mLogo;

	protected UIPanel mLoading;

	protected UISprite mGameTitle;

	protected float mTotalTime;

	protected EGameLoad mLoadingProcess;

	private AsyncOperation async;

	private void Start()
	{
		mLogo = base.transform.Find("LogoPanel").GetComponent<UIPanel>();
		mLoading = base.transform.Find("LodingPanel").GetComponent<UIPanel>();
		mGameTitle = base.transform.Find("LodingPanel").Find("GameTitle").GetComponent<UISprite>();
		mLoading.alpha = 0f;
		mGameTitle.alpha = 0f;
		StartCoroutine("_doLoadingProcess");
	}

	protected IEnumerator _doLoadingProcess()
	{
		yield return new WaitForSeconds(1f);
		float fLogoShowTime = 0.5f;
		TweenAlpha.Begin(mLogo.transform.gameObject, fLogoShowTime, 0f);
		float fLogoKeepTime = 0.5f;
		yield return new WaitForSeconds(fLogoShowTime + fLogoKeepTime);
		float fLoadingShowTime = 0.5f;
		TweenAlpha.Begin(mLoading.transform.gameObject, fLoadingShowTime, 1f);
		yield return new WaitForSeconds(fLoadingShowTime);
		float fGameTitleScaleTime = 0.9f;
		iTween.ScaleTo(mGameTitle.gameObject, iTween.Hash("scale", new Vector3(571f, 292f, 1f), "time", fGameTitleScaleTime, "easetype", iTween.EaseType.elastic));
		float fGameTitleAlphaTime = 0.28f;
		TweenAlpha.Begin(mGameTitle.gameObject, fGameTitleAlphaTime, 1f);
		float fLoadingKeepTime = 0.5f;
		yield return new WaitForSeconds(fLoadingKeepTime);
		StartCoroutine(loadScene());
	}

	private void Update()
	{
	}

	private IEnumerator loadScene()
	{
		async = Application.LoadLevelAsync("Game");
		yield return async;
	}
}
