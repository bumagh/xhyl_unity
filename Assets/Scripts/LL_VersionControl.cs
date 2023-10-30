using UnityEngine;

public class LL_VersionControl : MonoBehaviour
{
	private void Start()
	{
		GetComponent<UILabel>().text = "V" + LL_Constants.VERSION_CODE;
	}
}
