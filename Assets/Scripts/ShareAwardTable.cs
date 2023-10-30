using UnityEngine;
using UnityEngine.UI;

public class ShareAwardTable : MonoBehaviour
{
	private Text[] txtTable;

	private void Awake()
	{
		txtTable = new Text[6];
		for (int i = 0; i < 6; i++)
		{
			txtTable[i] = base.transform.GetChild(i).GetComponent<Text>();
		}
	}

	public void Init(string[] strs)
	{
		for (int i = 0; i < 6; i++)
		{
			txtTable[i].text = strs[i];
		}
	}
}
