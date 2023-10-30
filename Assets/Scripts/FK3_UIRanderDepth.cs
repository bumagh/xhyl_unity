using UnityEngine;

public class FK3_UIRanderDepth : MonoBehaviour
{
	private MeshRenderer m_renderer;

	public int Order;

	private void Start()
	{
		m_renderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		if ((bool)m_renderer && m_renderer.sortingOrder != Order)
		{
			m_renderer.sortingOrder = Order;
		}
	}
}
