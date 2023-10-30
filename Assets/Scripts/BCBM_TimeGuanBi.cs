using UnityEngine;

public class BCBM_TimeGuanBi : MonoBehaviour
{
	private float Time = 0.2f;

	public void GuanBi1()
	{
		Invoke("GuanBi2", Time);
	}

	public void GuanBi2()
	{
		base.gameObject.SetActive(value: false);
	}

	public void GuanBi()
	{
	}

	public void DestroyMoeb()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
