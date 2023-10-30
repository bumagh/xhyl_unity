using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_SceneBgMngr : MonoBehaviour
{
	public static DNTG_SceneBgMngr G_SceneBgMngr;

	public Sprite[] spiRollBg;

	[SerializeField]
	private GameObject objTide;

	[SerializeField]
	private Transform objTips;

	[SerializeField]
	private Transform objTipsHLPos;

	[SerializeField]
	private Transform objTipsShowPos;

	[SerializeField]
	private Transform objTipsHRPos;

	private Vector3 vecIniTide = Vector3.right * 820f;

	private Vector3 vecEndTide = Vector3.left * 820f;

	private Vector3 vecIniRollBg = Vector3.right * 1280f;

	private Vector3 vecEndRollBg = Vector3.zero;

	private readonly GameObject[] objGrass = new GameObject[3];

	[SerializeField]
	private Image imgRollBg;

	private Canvas cvRollBg;

	private int indexScene;

	[SerializeField]
	private Image imgSeaBg;

	private Canvas cvSeaBg;

	private Material materialLine;

	public static DNTG_SceneBgMngr GetSingleton()
	{
		if (G_SceneBgMngr == null)
		{
			G_SceneBgMngr = new DNTG_SceneBgMngr();
			G_SceneBgMngr.Awake();
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
		vecIniRollBg = Vector3.right * DNTG_SetCanvas.Width;
		if (materialLine == null)
		{
			materialLine = Resources.Load<Material>("FlowLightShader_ImgLine");
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
		if ((bool)DNTG_NetMngr.GetSingleton() && DNTG_NetMngr.GetSingleton().mSceneBg != -1)
		{
			SetScene(DNTG_NetMngr.GetSingleton().mSceneBg);
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
			vecIniRollBg = Vector3.right * DNTG_SetCanvas.Width;
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
			if (!DNTG_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
			{
				DNTG_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
			}
		}
	}

	public void ShowTip()
	{
		objTips.GetComponent<Image>().material = materialLine;
		objTips.DOKill();
		objTips.localScale = new Vector3(1f, 0f, 1f);
		objTips.localPosition = objTipsHRPos.localPosition;
		objTips.DOLocalMove(objTipsShowPos.localPosition, 0.15f);
		objTips.DOScaleY(1f, 0.15f).OnComplete(delegate
		{
			objTips.DOScale(Vector3.one, 0.8f).OnComplete(delegate
			{
				objTips.DOLocalMove(objTipsHLPos.localPosition, 1f);
				objTips.DOScaleY(0f, 0.15f);
			});
		});
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.P))
		{
			ChangeScene();
		}
	}

	public void ChangeScene()
	{
		UnityEngine.Debug.LogError("====测试====");
		ShowTip();
		DNTG_MusicMngr.GetSingleton().PlayGameSound(DNTG_MusicMngr.GAME_SOUND.SOUND_CLEAR_SCENE);
		StartCoroutine(IE_ChangeScene());
	}

	private IEnumerator IE_ChangeScene()
	{
		yield return new WaitForSeconds(0.8f);
		indexScene++;
		indexScene %= 3;
		objTide.SetActive(value: true);
		imgRollBg.sprite = spiRollBg[indexScene];
		imgRollBg.enabled = true;
		objTide.transform.DOKill();
		base.transform.DOKill();
		float num3 = 3.8f;
		imgRollBg.transform.DOKill();
		imgRollBg.transform.localPosition = vecIniRollBg;
		float num2 = (vecIniRollBg.x - vecEndRollBg.x) / (vecIniTide.x - vecEndTide.x);
		float changeTime = num3 * num2;
		if (changeTime < 3f)
		{
			changeTime = 3f;
		}
		imgRollBg.transform.DOLocalMove(vecEndRollBg, changeTime).SetEase(Ease.Linear).SetDelay(0.1f)
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
		if (objTide != null)
		{
			objTide.SetActive(value: false);
		}
		else
		{
			UnityEngine.Debug.LogError("objTide为空");
		}
	}

	private void OnRollEnd()
	{
		OnChangSceneEnd();
		DNTG_FishPoolMngr.GetSingleton().RemoveAllFish();
		DNTG_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = true;
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
