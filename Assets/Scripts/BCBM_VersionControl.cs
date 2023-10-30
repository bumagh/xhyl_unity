using UnityEngine;

public class BCBM_VersionControl : MonoBehaviour
{
	private void Start()
	{
		GetComponent<UILabel>().text = "V" + BCBM_Constants.VERSION_CODE;
	}
}
