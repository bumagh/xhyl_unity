using UnityEngine;

public class JSYS_TimeGuanBi : MonoBehaviour
{
	[Header("延时时间")]
	private float Time = 0.4f;

	[Header("是否开启延时关闭")]
	public bool PanDuan;

	private void GuanBi()
	{
	}

	public void GuanBi1()
	{
		Invoke("GuanBi2", Time);
	}

	public void GuanBi2()
	{
		base.gameObject.SetActive(value: false);
	}

	public void DestroyMoeb()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
