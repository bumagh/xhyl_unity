using UnityEngine;
using UnityEngine.UI;

public class LoginuserItemUI : MonoBehaviour
{
	public Text nameText;

	public Image bg;

	public Button deleteButton;

	private Button SelectBtn;

	private int m_id;

	private LoginuserListUI m_listUI;

	private bool isClearButton;

	public int id => m_id;

	private void Start()
	{
		deleteButton.onClick.AddListener(DeleteItem);
		SelectBtn = GetComponent<Button>();
		SelectBtn.onClick.AddListener(SelectItem);
	}

	public void SetItemData(int index, string name, bool isSelect, bool showDeleteBtn, LoginuserListUI listUI)
	{
		m_id = index;
		m_listUI = listUI;
		nameText.text = name;
		deleteButton.SetActive(showDeleteBtn);
		isClearButton = !showDeleteBtn;
	}

	public void SelectItem()
	{
		if (!isClearButton)
		{
			if (MB_Singleton<AppManager>.Get() != null)
			{
				MB_Singleton<AppManager>.Get().m_loginPanel.ChangeUser(nameText.text);
			}
		}
		else
		{
			ZH2_GVars.ClearLoginUser();
			m_listUI.Hide();
		}
	}

	public void DeleteItem()
	{
		ZH2_GVars.RemoveLoginUser(id);
		m_listUI.ReamoveAt(id);
	}
}
