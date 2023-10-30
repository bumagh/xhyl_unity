using UnityEngine;
using UnityEngine.UI;

public class DK_LoadMaterial : MonoBehaviour
{
	public string loadName;

	private Material material;

	private void OnEnable()
	{
		material = Resources.Load<Material>(loadName);
		GetComponent<Image>().material = material;
	}
}
