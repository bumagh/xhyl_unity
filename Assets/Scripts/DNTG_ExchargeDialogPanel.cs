using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_ExchargeDialogPanel : MonoBehaviour
{
	public List<Button> buttons;

	public List<GameObject> panels;

	private List<GameObject> pitchs;

	private void OnEnable()
	{
		pitchs = new List<GameObject>();
		for (int i = 0; i < buttons.Count; i++)
		{
			pitchs.Add(buttons[i].transform.Find("Pitch").gameObject);
		}
		OnClickBtn(0);
	}

	private void Start()
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			int index = i;
			buttons[i].onClick.AddListener(delegate
			{
				OnClickBtn(index);
			});
		}
	}

	private void OnClickBtn(int index)
	{
		for (int i = 0; i < pitchs.Count; i++)
		{
			pitchs[i].SetActive(value: false);
		}
		pitchs[index].SetActive(value: true);
		for (int j = 0; j < panels.Count; j++)
		{
			panels[j].SetActive(value: false);
		}
		panels[index].SetActive(value: true);
	}
}
