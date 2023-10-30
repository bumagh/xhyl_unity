using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DK_SceneBgMngr : MonoBehaviour
{
	public static DK_SceneBgMngr G_SceneBgMngr;

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

	public static DK_SceneBgMngr GetSingleton()
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
		UnityEngine.Debug.LogError("vecIniRollBg: " + vecIniRollBg);
		vecIniRollBg = Vector3.right * DK_SetCanvas.Width;
		UnityEngine.Debug.LogError("vecIniRollBg2: " + vecIniRollBg);
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
		if ((bool)DK_NetMngr.GetSingleton() && DK_NetMngr.GetSingleton().mSceneBg != -1)
		{
			SetScene(DK_NetMngr.GetSingleton().mSceneBg);
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
			if (!DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
			{
				DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
			}
		}
	}

	public void ChangeScene(int nIndex)
	{
		if (nIndex <= 2 && nIndex >= 0)
		{
			DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_CLEAR_SCENE);
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
		OnChangSceneEnd();
		DK_FishPoolMngr.GetSingleton().RemoveAllFish();
		DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
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

	private void ShowGrass()
	{
		for (int i = 0; i < 3; i++)
		{
			objGrass[i].SetActive(i == indexScene);
		}
	}
}
