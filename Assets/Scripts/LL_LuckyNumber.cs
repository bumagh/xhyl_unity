using System.Collections;
using UnityEngine;

public class LL_LuckyNumber : MonoBehaviour
{
	public Transform LuckyNum;

	public Transform LuckyPrize;

	public Transform Spark;

	public Transform leftBG;

	public Transform rightBG;

	public Texture[] mLuckyNumberTx;

	public Texture[] mLuckyPrizeTx;

	public GameObject mPrize;

	public GameObject mLeftBg;

	public GameObject mRightBg;

	public GameObject mFireworks;

	public GameObject mLightBack;

	public GameObject mLightRibbon;

	public GameObject mLuckyPrizeShine;

	private Vector3 _LightRibbonCenter;

	private Vector3 _RibbonScale;

	private float _fRibbonlen;

	private int _nLuckyNumber;

	private Vector3 Ini_Position_left;

	private Vector3 Ini_Position_right;

	private void Start()
	{
		LuckyNum = mPrize.transform.Find("LuckyNum");
		LuckyPrize = mPrize.transform.Find("LuckyPrize");
		Spark = mLightBack.transform.Find("Spark");
		leftBG = base.transform.Find("BGLeft");
		rightBG = base.transform.Find("BGRight");
		Ini_Position_left = leftBG.localPosition;
		Ini_Position_right = rightBG.localPosition;
		Hide();
		_RibbonScale = mLightRibbon.transform.localScale;
		_fRibbonlen = 4f;
		Vector3 position = mLightRibbon.transform.position;
		Vector3 a = Vector3.right * _fRibbonlen;
		Vector3 localScale = mLightRibbon.transform.localScale;
		_LightRibbonCenter = position + a * localScale.x;
	}

	private void Update()
	{
	}

	public void OnLR_Together()
	{
		mFireworks.SetActive(value: true);
		mFireworks.GetComponent<ParticleSystem>().Play();
	}

	public void OnLuckyPrizeShow()
	{
		if (LuckyNum != null)
		{
			LuckyNum.GetComponent<Renderer>().enabled = true;
		}
		if (LuckyPrize != null)
		{
			LuckyPrize.GetComponent<Renderer>().enabled = true;
		}
		iTween.PunchScale(mPrize, new Vector2(1.3f, 0f), 0.8f);
	}

	public void OnFireWorksDisappear()
	{
		mFireworks.GetComponent<ParticleSystem>().Stop();
	}

	public void OnScaleToZero()
	{
		iTween.ScaleTo(mPrize, new Vector3(0f, 1f, 1f), 0.5f);
	}

	public void OnWordsScaleZeroEnd()
	{
		Spark.GetComponent<ParticleSystem>().Play();
	}

	public void OnSparkEnd()
	{
		Spark.GetComponent<ParticleSystem>().Stop();
		OnLuckyRibbonGo();
	}

	public void OnLuckyRibbonGo()
	{
		float num = 0f;
		if (_nLuckyNumber >= 0 && _nLuckyNumber <= 3)
		{
			num = 57f / 320f * (float)Screen.width;
		}
		else if (_nLuckyNumber >= 4 && _nLuckyNumber <= 7)
		{
			num = 521f / 640f * (float)Screen.width;
		}
		int num2 = _nLuckyNumber % 4;
		float num3 = (0.8222222f - (float)num2 * 120f / 720f) * (float)Screen.height;
		Camera camera = Camera.allCameras[0];
		Camera camera2 = camera;
		float x = num;
		float y = num3;
		Vector3 vector = camera.WorldToScreenPoint(mLightRibbon.transform.position);
		Vector3 vector2 = camera2.ScreenToWorldPoint(new Vector3(x, y, vector.z));
		Vector3 toDirection = vector2 - _LightRibbonCenter;
		Quaternion rotation = default(Quaternion);
		rotation.SetFromToRotation(Vector3.left, toDirection);
		float magnitude = toDirection.magnitude;
		mLightRibbon.transform.position = vector2;
		mLightRibbon.transform.rotation = rotation;
		mLightRibbon.transform.localScale = new Vector3(magnitude / 0.4f * 0.1f, 0.1f, 0.1f);
		Camera camera3 = camera;
		float x2 = num;
		float y2 = num3;
		Vector3 vector3 = camera.WorldToScreenPoint(mLuckyPrizeShine.transform.position);
		vector2 = camera3.ScreenToWorldPoint(new Vector3(x2, y2, vector3.z));
		mLuckyPrizeShine.transform.position = vector2;
		mLightRibbon.GetComponent<LL_LuckyRibbonAnimScript>().OnAnimationEnd();
	}

	public void ShowAnimation(int nLuckyNumber, int nLuckyTyp)
	{
		Hide();
		if (nLuckyNumber <= 8 && nLuckyNumber >= 1 && nLuckyTyp <= 2 && nLuckyTyp >= 0)
		{
			_nLuckyNumber = nLuckyNumber - 1;
			LuckyNum.GetComponent<Renderer>().sharedMaterial.mainTexture = mLuckyNumberTx[nLuckyNumber - 1];
			LuckyPrize.GetComponent<Renderer>().sharedMaterial.mainTexture = mLuckyPrizeTx[nLuckyTyp];
			StartCoroutine("enumerator");
		}
		else
		{
			UnityEngine.Debug.Log("ShowAnimation type error");
		}
	}

	public void Hide()
	{
		iTween.Stop(base.gameObject);
		mFireworks.SetActive(value: false);
		mFireworks.GetComponent<ParticleSystem>().Stop();
		Spark.GetComponent<ParticleSystem>().Stop();
		LuckyNum.GetComponent<Renderer>().enabled = false;
		LuckyPrize.GetComponent<Renderer>().enabled = false;
		mLightRibbon.SetActive(value: false);
		mLuckyPrizeShine.SetActive(value: false);
		mPrize.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void Reset()
	{
		_nLuckyNumber = 0;
		iTween.Stop(mPrize);
		Hide();
	}

	private IEnumerator enumerator()
	{
		while (true)
		{
			Vector3 localPosition = leftBG.localPosition;
			if (localPosition.x < -2.13f)
			{
				BGMove();
				yield return 1;
				continue;
			}
			break;
		}
		OnLR_Together();
		OnLuckyPrizeShow();
		yield return new WaitForSeconds(1f);
		OnFireWorksDisappear();
		OnScaleToZero();
		OnWordsScaleZeroEnd();
		yield return new WaitForSeconds(0.5f);
		OnSparkEnd();
		leftBG.localPosition = Ini_Position_left;
		rightBG.localPosition = Ini_Position_right;
	}

	private void BGMove()
	{
		leftBG.transform.Translate(leftBG.right * Time.deltaTime * 20f, Space.World);
		rightBG.transform.Translate(-rightBG.right * Time.deltaTime * 20f, Space.World);
	}
}
