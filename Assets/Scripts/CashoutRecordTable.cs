using UnityEngine;
using UnityEngine.UI;

public class CashoutRecordTable : MonoBehaviour
{
	private Text[] txtTable = new Text[6];

	private void Awake()
	{
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
