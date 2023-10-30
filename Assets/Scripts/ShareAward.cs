using System.Collections.Generic;
using UnityEngine;

public class ShareAward : MonoBehaviour
{
	[SerializeField]
	private ShareAwardTable shareAwardTable;

	private List<ShareAwardTable> listTable = new List<ShareAwardTable>();

	public void Init(int count)
	{
		Transform parent = base.transform.Find("ImgTable");
		listTable.Clear();
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = shareAwardTable.gameObject;
			ShareAwardTable component = gameObject.GetComponent<ShareAwardTable>();
			listTable.Add(component);
			if (i > 0)
			{
				Object.Instantiate(gameObject);
			}
			gameObject.transform.SetParent(parent);
			gameObject.transform.localPosition = Vector3.down * (65 + i * 65);
		}
	}
}
