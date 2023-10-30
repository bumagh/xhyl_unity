using UnityEngine;
using UnityEngine.UI;

public class DP_UserItem : MonoBehaviour
{
	[HideInInspector]
	public Transform tfUserItem;

	[HideInInspector]
	public Image imgPersonIcon;

	[HideInInspector]
	public Text txtNickname;

	[HideInInspector]
	public bool bFree;

	private void Awake()
	{
		tfUserItem = base.transform.GetChild(0);
		imgPersonIcon = tfUserItem.Find("UserIcon").GetComponent<Image>();
		txtNickname = tfUserItem.Find("Nickname").GetComponent<Text>();
	}
}
