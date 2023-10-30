using UnityEngine;
using UnityEngine.UI;

public class VersionController : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Text>().text = "V" + Application.version;
	}
}
