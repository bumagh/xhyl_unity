using UnityEngine;

public class ParticleCor : MonoBehaviour
{
	private Material material;

	private Renderer yuanPs;

	private void Awake()
	{
		material = null;
		GetMaterial();
		yuanPs = base.transform.GetComponent<Renderer>();
	}

	private void GetMaterial()
	{
		material = Resources.Load<Material>("FX Rain Drop_Gold");
	}

	private void OnEnable()
	{
		if (material == null)
		{
			GetMaterial();
		}
		if (yuanPs != null && material != null)
		{
			yuanPs.material = material;
		}
	}
}
