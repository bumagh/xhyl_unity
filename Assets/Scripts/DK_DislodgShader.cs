using UnityEngine;
using UnityEngine.UI;

public class DK_DislodgShader : MonoBehaviour
{
	private Material material;

	private int tempChildCount = -1;

	private void Awake()
	{
		material = null;
		material = Resources.Load<Material>("tableShader");
	}

	private void OnEnable()
	{
		if (material == null)
		{
			material = Resources.Load<Material>("tableShader");
		}
		tempChildCount = -1;
	}

	private void SetMaterial()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform transform = base.transform.GetChild(i).transform.Find("tableLogo");
			if (transform != null)
			{
				Image component = transform.GetComponent<Image>();
				if (component != null)
				{
					component.material = null;
					component.material = material;
				}
			}
		}
	}

	public void SetImaAndText(float time)
	{
		tempChildCount = -1;
	}

	public void SetOver()
	{
		tempChildCount = -1;
	}

	private void LateUpdate()
	{
		if (tempChildCount != base.transform.childCount)
		{
			tempChildCount = base.transform.childCount;
			SetMaterial();
		}
	}
}
