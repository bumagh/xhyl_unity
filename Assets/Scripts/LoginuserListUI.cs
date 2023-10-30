using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginuserListUI : MonoBehaviour
{
	public Button hideButton;

	public LoginuserItemUI itemUI;

	public RectTransform root;

	private List<string> options = new List<string>();

	private string curSelectUser;

	private List<LoginuserItemUI> items = new List<LoginuserItemUI>();

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
	}

	private void Start()
	{
		items.Add(itemUI);
		itemUI.gameObject.SetActive(value: false);
		Hide();
		hideButton.onClick.AddListener(Hide);
	}

	private LoginuserItemUI GetItemUI(int index)
	{
		while (index >= items.Count)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(itemUI.gameObject, root);
			gameObject.gameObject.SetActive(value: false);
			items.Add(gameObject.GetComponent<LoginuserItemUI>());
		}
		return items[index];
	}

	public void Show(List<string> setOptions, string setCurSelectUser)
	{
		options = setOptions;
		curSelectUser = setCurSelectUser;
		UpdateList();
	}

	public void UpdateList()
	{
		foreach (LoginuserItemUI item in items)
		{
			item.gameObject.SetActive(value: false);
		}
		for (int i = 0; i < options.Count; i++)
		{
			LoginuserItemUI loginuserItemUI = GetItemUI(i);
			loginuserItemUI.gameObject.SetActive(value: true);
			loginuserItemUI.SetItemData(i, options[i], curSelectUser == options[i], showDeleteBtn: true, this);
		}
		base.gameObject.SetActive(value: true);
		hideButton.SetActive();
	}

	public void ReamoveAt(int index)
	{
		if (index >= 0 && index < options.Count)
		{
			options.RemoveAt(index);
		}
		UpdateList();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		hideButton.SetActive(active: false);
	}
}
