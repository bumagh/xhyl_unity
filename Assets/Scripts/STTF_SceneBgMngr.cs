using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class STTF_SceneBgMngr : MonoBehaviour
{
	public static STTF_SceneBgMngr G_SceneBgMngr;

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

	public static STTF_SceneBgMngr GetSingleton()
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
		if ((bool)STTF_NetMngr.GetSingleton() && STTF_NetMngr.GetSingleton().mSceneBg != -1)
		{
			indexScene = STTF_NetMngr.GetSingleton().mSceneBg;
			SetScene();
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
		objTide.transform.localPosition = vecIniTide;
		imgRollBg.transform.DOKill();
		imgRollBg.transform.localPosition = vecIniRollBg;
	}

	public void BigFishFormatReset()
	{
		OnRollEnd();
		Reset();
	}

	public void SetScene()
	{
		Reset();
		imgSeaBg.sprite = spiRollBg[indexScene];
		if (!STTF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
		{
			STTF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
		}
	}

	public void ChangeScene()
	{
		indexScene++;
		indexScene %= 3;
		STTF_MusicMngr.GetSingleton().PlayGameSound(STTF_MusicMngr.GAME_SOUND.SOUND_CLEAR_SCENE);
		objTide.SetActive(value: true);
		imgRollBg.sprite = spiRollBg[indexScene];
		imgRollBg.enabled = true;
		objTide.transform.DOKill();
		base.transform.DOKill();
		objTide.transform.localPosition = vecIniTide;
		float num = 3.8f;
		objTide.transform.DOLocalMove(vecEndTide, num).SetEase(Ease.Linear).SetDelay(0.1f)
			.OnComplete(OnChangSceneEnd);
		imgRollBg.transform.DOKill();
		imgRollBg.transform.localPosition = vecIniRollBg;
		float num2 = (vecIniRollBg.x - vecEndRollBg.x) / (vecIniTide.x - vecEndTide.x);
		imgRollBg.transform.DOLocalMove(vecEndRollBg, num * num2).SetEase(Ease.Linear).SetDelay(0.1f)
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
		objTide.SetActive(value: false);
	}

	private void OnRollEnd()
	{
		STTF_FishPoolMngr.GetSingleton().RemoveAllFish();
		STTF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
		StopCoroutine("IE_RollEnd");
		StartCoroutine("IE_RollEnd");
	}

	private IEnumerator IE_RollEnd()
	{
		yield return new WaitForEndOfFrame();
		imgRollBg.enabled = false;
		imgSeaBg.sprite = spiRollBg[indexScene];
	}

	private void ShowGrass()
	{
		for (int i = 0; i < 3; i++)
		{
			objGrass[i].SetActive(i == indexScene);
		}
	}
}
