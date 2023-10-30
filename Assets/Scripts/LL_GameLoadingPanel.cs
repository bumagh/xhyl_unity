using LL_GameCommon;
using System.Collections;
using UnityEngine;

public class LL_GameLoadingPanel : MonoBehaviour
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
		_setViewport();
		mGameTitle.alpha = 0f;
		StartCoroutine("_doLoadingProcess");
	}

	private void Awake()
	{
		_setViewport();
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

	protected void _setViewport()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float g_fWidth = LL_ScreenParameter.G_fWidth;
		float g_fHeight = LL_ScreenParameter.G_fHeight;
		float x = 0f;
		float y = 0f;
		float num3 = 1f;
		float num4 = 1f;
		if (g_fWidth / g_fHeight > num / num2)
		{
			num3 = 1f;
			num4 = g_fHeight * num / g_fWidth / num2;
			y = (1f - num4) / 2f;
		}
		else if (g_fWidth / g_fHeight < num / num2)
		{
			num4 = 1f;
			num3 = g_fWidth * num2 / g_fHeight / num;
			x = (1f - num3) / 2f;
		}
		Camera camera = Camera.allCameras[0];
		camera.rect = new Rect(x, y, num3, num4);
	}

	private IEnumerator loadScene()
	{
		async = Application.LoadLevelAsync("Game");
		yield return async;
	}
}
