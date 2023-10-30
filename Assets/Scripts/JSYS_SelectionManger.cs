using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_SelectionManger : MonoBehaviour
{
	public List<Button> Selection_List;

	private void Awake()
	{
		for (int i = 0; i < Selection_List.Count; i++)
		{
			int index = i;
			Selection_List[i].onClick.AddListener(delegate
			{
				ChangeHaed(index);
			});
		}
	}

	private void OnEnable()
	{
		ChangeHaed(0);
	}

	public void ChangeHaed(int index)
	{
		ShowSelectionNum(index);
	}

	private void ShowSelectionNum(int index)
	{
		ZH2_GVars.selectRoomId = index;
		for (int i = 0; i < Selection_List.Count; i++)
		{
			Selection_List[i].transform.Find("PitchOn").gameObject.SetActive(value: false);
		}
		Selection_List[index].transform.Find("PitchOn").gameObject.SetActive(value: true);
	}
}
