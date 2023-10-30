using M__M.HaiWang.Player.Gun;
using UnityEngine;

public class FK3_GunDebugLine2 : MonoBehaviour
{
	[SerializeField]
	[FK3_SortingLayer]
	private string _layerName = "Default";

	[Range(-20000f, 20000f)]
	[SerializeField]
	private int _orderInLayer;

	private FK3_GunBehaviour m_gun;

	private FK3_DebugLine m_line;

	private void Start()
	{
		m_gun = GetComponent<FK3_GunBehaviour>();
		m_line = base.gameObject.AddComponent<FK3_DebugLine>();
		m_line.SetLayer(_layerName, _orderInLayer);
		m_gun.Event_RotateByInput_Handler += Handle_RotateByInput;
	}

	private void Handle_RotateByInput(FK3_GunBehaviour gun, Transform begin, Vector3 end, float angle)
	{
		m_line.SetLine(begin, end);
	}
}
