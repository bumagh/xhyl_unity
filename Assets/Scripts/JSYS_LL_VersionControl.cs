using UnityEngine;

public class JSYS_LL_VersionControl : MonoBehaviour
{
	private void Start()
	{
		GetComponent<UILabel>().text = "V" + JSYS_LL_Constants.VERSION_CODE;
	}
}
