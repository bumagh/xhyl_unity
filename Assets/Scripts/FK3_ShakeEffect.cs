using UnityEngine;

public class FK3_ShakeEffect
{
	private Transform m_shakeTs;

	private Vector3 m_originalPos = Vector3.zero;

	private Vector3 m_startshakePos = Vector3.zero;

	private float m_shakevalue = 1f;

	private float m_shakefactor = 1.1f;

	public bool IsShaking
	{
		get;
		private set;
	}

	public FK3_ShakeEffect(Transform shakets)
	{
		m_shakeTs = shakets;
		m_startshakePos = shakets.position;
	}

	public void SetShakeInfo(float factor = -1f, float value = -1f)
	{
		if (!IsShaking)
		{
			IsShaking = true;
			if (factor != -1f)
			{
				m_shakefactor = factor;
			}
			else
			{
				m_shakefactor = 1.5f;
			}
			if (value != -1f)
			{
				m_shakevalue = value;
			}
			else
			{
				m_shakevalue = 0.12f;
			}
		}
	}

	public void OnUpdate()
	{
		if (IsShaking)
		{
			CameraShake();
		}
	}

	private void CameraShake()
	{
		m_originalPos.x = Random.Range(0f, m_shakevalue * 2f) - m_shakevalue;
		m_originalPos.y = Random.Range(0f, m_shakevalue * 2f) - m_shakevalue;
		m_originalPos.z = Random.Range(0f, m_shakevalue * 2f) - m_shakevalue;
		m_shakevalue /= m_shakefactor;
		if (m_shakevalue < 0.01f)
		{
			IsShaking = false;
			m_shakevalue = 0f;
			m_originalPos.x = 0f;
			m_originalPos.y = 0f;
			m_shakeTs.position = m_startshakePos;
			m_shakeTs.localScale = Vector3.one;
		}
		else
		{
			m_shakeTs.position = m_startshakePos + m_originalPos;
			m_shakeTs.localScale = Vector3.one * 1.05f;
		}
	}
}
