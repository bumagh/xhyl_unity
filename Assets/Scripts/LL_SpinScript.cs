using LL_GameCommon;
using UnityEngine;

public class LL_SpinScript : MonoBehaviour
{
	public enum SPIN_STATE
	{
		ST_ACCELERATION,
		ST_CONSTANT,
		ST_DEACCELERATION,
		ST_STOP
	}

	private float _Acceleration = 60f;

	public int m_DecelerationRound = 2;

	public float m_MaxSpinSpeed = 260f;

	public float m_MinSpinSpeed = 3f;

	private SPIN_STATE m_SpinState = SPIN_STATE.ST_STOP;

	private int m_No = -1;

	private float m_CurrentRotation;

	private float m_CurSpinSpeed;

	private float m_RemainRotation;

	private bool m_bClockwise = true;

	private float _fAglPerNo;

	private bool _isSpin;

	private bool _isGoToStop;

	private float _fTotalSpinTime;

	private float _fTotalGoToStopTime;

	private float m_AglDistance;

	private float _fCurSpinTime;

	private float _fBeginStopTime;

	public void Reset()
	{
		m_CurSpinSpeed = 0f;
		_isSpin = false;
		_isGoToStop = false;
		_fTotalSpinTime = 0f;
		_fTotalGoToStopTime = 0f;
		m_AglDistance = 0f;
		_fCurSpinTime = 0f;
		_fBeginStopTime = 0f;
	}

	public void SpinTo(int nNo, float time, bool isClockwise = true)
	{
		Reset();
		m_SpinState = SPIN_STATE.ST_ACCELERATION;
		m_No = nNo;
		m_bClockwise = isClockwise;
		m_CurSpinSpeed = 0f;
		_fTotalSpinTime = time;
		_fCurSpinTime = 0f;
		_isSpin = true;
	}

	public void GoToStop(float timeLen, float speed = 100f)
	{
		_fTotalGoToStopTime = timeLen;
		Vector3 eulerAngles = base.transform.eulerAngles;
		float y = eulerAngles.y;
		float num = 2f;
		if (m_bClockwise)
		{
			float num2 = ((float)m_No * 360f / (float)LL_Parameter.G_nAnimalNumber - y + 360f) % 360f;
			num = (int)(timeLen * speed) / 360;
			m_AglDistance = num2 + num * 360f;
		}
		else
		{
			float num3 = (y - (float)m_No * 360f / (float)LL_Parameter.G_nAnimalNumber + 360f) % 360f;
			m_AglDistance = num3 + num * 360f;
		}
		_isGoToStop = true;
		_isSpin = false;
	}

	private void spinUpdate()
	{
		_fCurSpinTime += Time.deltaTime;
		if (_fCurSpinTime > _fTotalSpinTime / 3f)
		{
			GoToStop(_fTotalSpinTime - _fCurSpinTime);
			return;
		}
		if (m_CurSpinSpeed < m_MaxSpinSpeed)
		{
			m_CurSpinSpeed += _Acceleration * Time.deltaTime;
		}
		float rotation = getRotation(m_CurSpinSpeed, m_bClockwise);
		base.transform.Rotate(Vector3.up * rotation, Space.World);
	}

	private void gotoStopUpdate()
	{
		if (_fTotalGoToStopTime > 0f)
		{
			float num = 2f * m_AglDistance / (_fTotalGoToStopTime * _fTotalGoToStopTime);
			m_CurSpinSpeed = num * _fTotalGoToStopTime;
			float rotation = getRotation(m_CurSpinSpeed, m_bClockwise);
			base.transform.Rotate(Vector3.up * rotation, Space.World);
			m_AglDistance -= Mathf.Abs(rotation);
			_fTotalGoToStopTime -= Time.deltaTime;
		}
		else
		{
			_isGoToStop = false;
			m_SpinState = SPIN_STATE.ST_STOP;
			SetAglToNo(m_No);
		}
	}

	private float getRotation(float speed, bool isClockwize)
	{
		if (isClockwize)
		{
			return speed * Time.deltaTime;
		}
		return (0f - speed) * Time.deltaTime;
	}

	public void StartSpinTo(int nNo, bool isClockwise = true)
	{
	}

	public bool IsStop()
	{
		return m_SpinState == SPIN_STATE.ST_STOP;
	}

	public void SetAglToNo(int nNo)
	{
		float y = (float)nNo * 15f;
		base.transform.eulerAngles = new Vector3(0f, y, 0f);
	}

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		if (_isSpin)
		{
			spinUpdate();
		}
		if (_isGoToStop)
		{
			gotoStopUpdate();
		}
	}
}
