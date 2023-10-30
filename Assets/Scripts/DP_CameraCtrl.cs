using DG.Tweening;
using DP_GameCommon;
using UnityEngine;

public class DP_CameraCtrl : MonoBehaviour
{
	private Vector3 _doPrizeDestPos = new Vector3(0f, 1.4f, 2.6f);

	private Vector3 posPrizeDest = new Vector3(0f, 2.2f, 2.5f);

	private Vector3 _iniPos;

	private Quaternion _iniRotation;

	private Vector3 _targetPoint = new Vector3(0f, 0.6f, 0f);

	private Vector3 _iniLookAtPoint = new Vector3(0f, 0f, 0f);

	public Transform _curLookAtTransform;

	private void Start()
	{
		_iniPos = base.transform.localPosition;
		_iniRotation = base.transform.localRotation;
		_iniLookAtPoint = base.transform.rotation * Vector3.forward * 3f + base.transform.position;
		_curLookAtTransform.rotation = Quaternion.identity;
		_curLookAtTransform.position = _iniLookAtPoint;
		_targetPoint = new Vector3(0f, -0.2f, -5f);
		_doPrizeDestPos = new Vector3(0f, 2.5f, 3f);
	}

	private void Awake()
	{
	}

	private void _setViewport()
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
		for (int i = 0; i < Camera.allCameras.Length; i++)
		{
			Camera camera = Camera.allCameras[i];
			camera.rect = new Rect(x, y, num3, num4);
		}
	}

	private void Update()
	{
		_lookAt();
	}

	public void PlayAnimation(bool isPlay)
	{
		if (isPlay)
		{
			_curLookAtTransform.DOMove(_targetPoint, 2f).SetDelay(LL_Parameter.G_fAnimalBounceTime + 0.8f).SetEase(Ease.InOutSine);
			base.transform.DOMove(_doPrizeDestPos, 2f).SetDelay(LL_Parameter.G_fAnimalBounceTime + 0.8f).SetEase(Ease.InOutSine);
		}
		else
		{
			_Reset();
		}
	}

	private void _lookAt()
	{
		base.transform.LookAt(_curLookAtTransform);
	}

	private void _Reset()
	{
		iTween.Stop(base.gameObject);
		iTween.Stop(_curLookAtTransform.gameObject);
		base.transform.localPosition = _iniPos;
		base.transform.localRotation = _iniRotation;
		_curLookAtTransform.rotation = Quaternion.identity;
		_curLookAtTransform.position = _iniLookAtPoint;
		base.transform.LookAt(_curLookAtTransform);
	}

	public void PlayAnim()
	{
		_curLookAtTransform.DOLocalMove(_targetPoint, 1.5f).SetDelay(0.5f).SetEase(Ease.Linear);
		base.transform.DOLocalMove(posPrizeDest, 1.5f).SetDelay(0.5f).SetEase(Ease.Linear);
		_curLookAtTransform.DOLocalMove(_iniLookAtPoint, 1.5f).SetDelay(10f).SetEase(Ease.Linear);
		base.transform.DOLocalMove(_iniPos, 1.5f).SetDelay(10f).SetEase(Ease.Linear);
	}
}
