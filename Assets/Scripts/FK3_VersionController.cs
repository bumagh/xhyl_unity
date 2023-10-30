using UnityEngine;
using UnityEngine.UI;

public class FK3_VersionController : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Text>().text = "V" + Application.version;
	}
}
