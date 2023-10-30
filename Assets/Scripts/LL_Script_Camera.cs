using LL_GameCommon;
using UnityEngine;

public class LL_Script_Camera : MonoBehaviour
{
	private enum CameraState
	{
		CameraState_Normal,
		CameraState_Shake,
		CameraState_PlayPrize
	}

	private Vector3 _doPrizeDestPos = new Vector3(0f, 1.4f, 2.6f);

	private Vector3 _iniPos;

	private Quaternion _iniRotation;

	private Vector3 _targetPoint = new Vector3(0f, 0.6f, 0f);

	private Vector3 _iniLookAtPoint = new Vector3(0f, 0f, 0f);

	public Transform _curLookAtTransform;

	private CameraState _cameraState;

	private void Start()
	{
		_iniPos = base.transform.localPosition;
		_iniRotation = base.transform.localRotation;
		_iniLookAtPoint = base.transform.rotation * Vector3.forward * 3f + base.transform.position;
		_curLookAtTransform.rotation = Quaternion.identity;
		_curLookAtTransform.position = _iniLookAtPoint;
		_targetPoint = new Vector3(0f, -0.2f, -5f);
		_doPrizeDestPos = new Vector3(0f, 2.5f, 3f);
		_setViewport();
	}

	private void Awake()
	{
	}

	private void _setViewport()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float num3 = Screen.width;
		float num4 = Screen.height;
		float x = 0f;
		float y = 0f;
		float num5 = 1f;
		float num6 = 1f;
		if (num3 / num4 > num / num2)
		{
			num5 = 1f;
			num6 = num4 * num / num3 / num2;
			y = (1f - num6) / 2f;
		}
		else if (num3 / num4 < num / num2)
		{
			num6 = 1f;
			num5 = num3 * num2 / num4 / num;
			x = (1f - num5) / 2f;
		}
		for (int i = 0; i < Camera.allCameras.Length; i++)
		{
			Camera camera = Camera.allCameras[i];
			camera.rect = new Rect(x, y, num5, num6);
		}
	}

	private void Update()
	{
		_lookAt();
	}

	private void _logicCtrl()
	{
	}

	public void PlayAnimation(bool isPlay)
	{
		if (isPlay)
		{
			iTween.MoveTo(_curLookAtTransform.gameObject, iTween.Hash("position", _targetPoint, "time", 2f, "delay", LL_Parameter.G_fAnimalBounceTime + 0.8f, "easetype", iTween.EaseType.easeInOutSine));
			iTween.MoveTo(base.gameObject, iTween.Hash("position", _doPrizeDestPos, "time", 2f, "delay", LL_Parameter.G_fAnimalBounceTime + 0.8f, "easetype", iTween.EaseType.easeInOutSine));
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
}
