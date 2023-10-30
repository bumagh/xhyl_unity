using FullInspector;
using System;
using UnityEngine;

[fiInspectorOnly]
public class FK3_LockChainController : MonoBehaviour
{
	[SerializeField]
	private Transform m_transPoint;

	[SerializeField]
	private Transform m_transChain;

	private float m_length = 8f;

	[SerializeField]
	private float m_depth;

	[ShowInInspector]
	private Transform m_transFrom;

	[ShowInInspector]
	private Transform m_transTo;

	[ShowInInspector]
	private Vector3 m_fromOffset;

	[ShowInInspector]
	private Vector3 m_toOffset;

	public Action Event_OnChangeTarget;

	public Action Event_OnStopLocking;

	public int fishId;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (m_transFrom != null && m_transTo != null && m_length != 0f)
		{
			Draw();
		}
	}

	private void Draw()
	{
		Vector3 position = m_transFrom.position;
		Vector3 position2 = m_transTo.position;
		position.z = m_depth;
		position2.z = m_depth;
		Vector3 toDirection = position2 - position;
		Vector3 position3 = (position + position2) / 2f;
		float magnitude = toDirection.magnitude;
		float y = magnitude / m_length;
		m_transChain.localScale = new Vector3(1f, y, 1f);
		m_transChain.position = position3;
		m_transChain.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);
		if (m_transPoint != null)
		{
			m_transPoint.transform.position = position2;
		}
	}

	public void SetTarget(Transform from, Vector3 fromOffset, Transform to, Vector3 toOffset)
	{
		m_transFrom = from;
		m_fromOffset = fromOffset;
		m_transTo = to;
		m_toOffset = toOffset;
	}

	public void SetTarget(Transform from, Transform to, int sortingOrder = 0)
	{
		m_transFrom = from;
		m_transTo = to;
	}

	public void DoChangeTarget(int fishId)
	{
		this.fishId = fishId;
		if (Event_OnChangeTarget != null)
		{
			Event_OnChangeTarget();
		}
	}

	public void UnlockTarget()
	{
		if (Event_OnChangeTarget != null)
		{
			Event_OnChangeTarget();
		}
	}

	public bool IsTarget(Transform to)
	{
		return m_transTo == to;
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("锁定炮连接2"));
		base.gameObject.SetActive(value: false);
		m_transFrom = null;
		m_transTo = null;
	}

	public void Reset_EventHandler()
	{
		Event_OnChangeTarget = null;
		Event_OnStopLocking = null;
	}

	public void Reset_Chain()
	{
		Hide();
	}
}
