using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BZJX_SceneBgMngr : MonoBehaviour
{
	public static BZJX_SceneBgMngr G_SceneBgMngr;

	public Sprite[] spiRollBg;

	[SerializeField]
	private GameObject objTide;

	private Vector3 vecIniTide = Vector3.right * 820f;

	private Vector3 vecEndTide = Vector3.left * 820f;

	private Vector3 vecIniRollBg = Vector3.right * 1280f;

	private Vector3 vecEndRollBg = Vector3.zero;

	private GameObject[] objGrass = new GameObject[3];

	[SerializeField]
	private Image imgRollBg;

	private Canvas cvRollBg;

	private int indexScene;

	[SerializeField]
	private Image imgSeaBg;

	private Canvas cvSeaBg;

	public static BZJX_SceneBgMngr GetSingleton()
	{
		if (G_SceneBgMngr == null)
		{
			G_SceneBgMngr = new BZJX_SceneBgMngr();
		}
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
		vecIniRollBg = Vector3.right * BZJX_SetCanvas.Width;
	}

	private void Start()
	{
		objGrass[0] = base.transform.Find("Grass").gameObject;
		objGrass[1] = base.transform.Find("Grass1").gameObject;
		objGrass[2] = base.transform.Find("Grass2").gameObject;
		cvRollBg = imgRollBg.transform.GetComponent<Canvas>();
		cvSeaBg = imgSeaBg.transform.GetComponent<Canvas>();
		cvSeaBg.sortingOrder = -1;
		cvRollBg.sortingOrder = 943;
		Reset();
		if ((bool)BZJX_NetMngr.GetSingleton() && BZJX_NetMngr.GetSingleton().mSceneBg != -1)
		{
			SetScene(BZJX_NetMngr.GetSingleton().mSceneBg);
		}
	}

	public void Reset()
	{
		imgSeaBg.sprite = spiRollBg[indexScene];
		ShowGrass();
		objTide.SetActive(value: false);
		imgRollBg.enabled = false;
		objTide.transform.DOKill();
		base.transform.DOKill();
		imgRollBg.transform.DOKill();
		if (vecIniRollBg.x <= 0f)
		{
			vecIniRollBg = Vector3.right * BZJX_SetCanvas.Width;
		}
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
			if (!BZJX_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
			{
				BZJX_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
			}
		}
	}

	public void ChangeScene()
	{
		BZJX_MusicMngr.GetSingleton().PlayGameSound(BZJX_MusicMngr.GAME_SOUND.SOUND_CLEAR_SCENE);
		indexScene++;
		indexScene %= 3;
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

	public void ShakeBg()
	{
		StartCoroutine(IE_ShakeBg());
	}

	private IEnumerator IE_ShakeBg()
	{
		UnityEngine.Debug.Log("ShakeBg start");
		base.transform.DOKill();
		yield return 1;
		base.transform.DOShakePosition(1.2f, Vector3.right * 10f + Vector3.up * 10f).SetEase(Ease.Linear).OnComplete(ShakeEnd);
		UnityEngine.Debug.Log("ShakeBg end");
	}

	private void ShakeEnd()
	{
		UnityEngine.Debug.Log("_shakeEnd");
		base.transform.localPosition = Vector3.zero;
	}

	private void OnChangSceneEnd()
	{
		if (objTide == null)
		{
			objTide = base.transform.Find("BGRoll/Tide").gameObject;
		}
		objTide.SetActive(value: false);
	}

	private void OnRollEnd()
	{
		OnChangSceneEnd();
		BZJX_FishPoolMngr.GetSingleton().RemoveAllFish();
		BZJX_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
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
		else
		{
			imgSeaBg.sprite = spiRollBg[0];
		}
	}

	private void ShowGrass()
	{
		for (int i = 0; i < 3; i++)
		{
			objGrass[i].SetActive(i == indexScene);
		}
	}
}
