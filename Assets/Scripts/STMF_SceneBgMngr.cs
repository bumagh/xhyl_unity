using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class STMF_SceneBgMngr : MonoBehaviour
{
	public static STMF_SceneBgMngr G_SceneBgMngr;

	public Sprite[] spiRollBg;

	[SerializeField]
	private GameObject objTide;

	private Vector3 vecIniTide = Vector3.right * 820f;

	private Vector3 vecEndTide = Vector3.left * 820f;

	private Vector3 vecIniRollBg = Vector3.right * 1280f;

	private Vector3 vecEndRollBg = Vector3.zero;

	[SerializeField]
	private Image imgRollBg;

	private Canvas cvRollBg;

	private int indexScene;

	[SerializeField]
	private Image imgSeaBg;

	private Canvas cvSeaBg;

	public static STMF_SceneBgMngr GetSingleton()
	{
		return G_SceneBgMngr;
	}

	private void Awake()
	{
		if (G_SceneBgMngr == null)
		{
			G_SceneBgMngr = this;
		}
	}

	private void OnEnable()
	{
		vecIniRollBg = Vector3.right * STMF_SetCanvas.Width;
	}

	private void Start()
	{
		cvRollBg = imgRollBg.transform.GetComponent<Canvas>();
		cvSeaBg = imgSeaBg.transform.GetComponent<Canvas>();
		cvSeaBg.sortingOrder = -1;
		cvRollBg.sortingOrder = 642;
		Reset();
		if ((bool)STMF_NetMngr.GetSingleton() && STMF_NetMngr.GetSingleton().mSceneBg != -1)
		{
			SetScene(STMF_NetMngr.GetSingleton().mSceneBg);
		}
	}

	public void Reset()
	{
		imgSeaBg.sprite = spiRollBg[indexScene];
		objTide.SetActive(value: false);
		imgRollBg.enabled = false;
		base.transform.DOKill();
		imgRollBg.transform.DOKill();
		imgRollBg.transform.localPosition = vecIniRollBg;
	}

	public void BigFishFormatReset()
	{
		OnRollEnd();
		Reset();
	}

	public void SetScene(int nIndex)
	{
		Reset();
		if (nIndex <= 2 && nIndex >= 0)
		{
			indexScene = nIndex;
			imgSeaBg.sprite = spiRollBg[indexScene];
			if (!STMF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
			{
				STMF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
			}
		}
	}

	public void ChangeScene(int nIndex)
	{
		if (nIndex <= 2 && nIndex >= 0)
		{
			STMF_MusicMngr.GetSingleton().PlayGameSound(STMF_MusicMngr.GAME_SOUND.SOUND_CLEAR_SCENE);
			indexScene = nIndex;
			objTide.SetActive(value: true);
			imgRollBg.sprite = spiRollBg[indexScene];
			imgRollBg.enabled = true;
			objTide.transform.DOKill();
			base.transform.DOKill();
			float num = 3.8f;
			imgRollBg.transform.DOKill();
			imgRollBg.transform.localPosition = vecIniRollBg;
			float num2 = (vecIniRollBg.x - vecEndRollBg.x) / (vecIniTide.x - vecEndTide.x);
			float num3 = num * num2;
			UnityEngine.Debug.LogError("====切换场景时间1====" + num3);
			if (num3 < 3f)
			{
				UnityEngine.Debug.LogError("切换场景时间过短!!!");
				num3 = 3f;
			}
			imgRollBg.transform.DOLocalMove(vecEndRollBg, num3).SetEase(Ease.Linear).SetDelay(0.1f)
				.OnComplete(OnRollEnd);
		}
	}

	public void ShakeBg()
	{
		StartCoroutine(IE_ShakeBg());
	}

	private IEnumerator IE_ShakeBg()
	{
		base.transform.DOKill();
		yield return 1;
		base.transform.DOShakePosition(1.2f, Vector3.right * 10f + Vector3.up * 10f).SetEase(Ease.Linear).OnComplete(ShakeEnd);
	}

	private void ShakeEnd()
	{
		base.transform.localPosition = Vector3.zero;
	}

	private void OnChangSceneEnd()
	{
		objTide.SetActive(value: false);
	}

	private void OnRollEnd()
	{
		OnChangSceneEnd();
		STMF_FishPoolMngr.GetSingleton().RemoveAllFish();
		STMF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
		StopCoroutine("IE_RollEnd");
		StartCoroutine("IE_RollEnd");
	}

	private IEnumerator IE_RollEnd()
	{
		yield return new WaitForEndOfFrame();
		imgRollBg.enabled = false;
		if (indexScene <= 2 && indexScene >= 0)
		{
			imgSeaBg.sprite = spiRollBg[indexScene];
		}
	}
}
