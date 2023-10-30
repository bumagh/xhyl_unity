using M__M.HaiWang.Player.Gun;
using UnityEngine;

public class GunDebugLine2 : MonoBehaviour
{
	[SortingLayer]
	[SerializeField]
	private string _layerName = "Default";

	[SerializeField]
	[Range(-20000f, 20000f)]
	private int _orderInLayer;

	private GunBehaviour m_gun;

	private DebugLine m_line;

	private void Start()
	{
		m_gun = GetComponent<GunBehaviour>();
		m_line = base.gameObject.AddComponent<DebugLine>();
		m_line.SetLayer(_layerName, _orderInLayer);
		m_gun.Event_RotateByInput_Handler += Handle_RotateByInput;
	}

	private void Handle_RotateByInput(GunBehaviour gun, Transform begin, Vector3 end, float angle)
	{
		m_line.SetLine(begin, end);
	}
}
