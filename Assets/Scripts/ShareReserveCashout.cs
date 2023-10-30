using UnityEngine;
using UnityEngine.UI;

public class ShareReserveCashout : MonoBehaviour
{
	private Button btnCash;

	private Text[] txtTotal = new Text[5];

	[SerializeField]
	private ReserveCashOutPanel reserveCashoutPanel;

	[SerializeField]
	private CashoutRecordTable cashoutRecordTable;

	private void Awake()
	{
		btnCash = base.transform.Find("BtnReserveCashout").GetComponent<Button>();
		for (int i = 0; i < 5; i++)
		{
			txtTotal[i] = base.transform.Find("ImgReserveCashRecord/Table0").GetChild(i).GetComponent<Text>();
		}
		btnCash.onClick.AddListener(ClickBtnCash);
	}

	public void InitTotal()
	{
		for (int i = 0; i < 5; i++)
		{
			txtTotal[i].text = string.Empty;
		}
	}

	public void InitRecord(int count)
	{
		Transform parent = base.transform.Find("ImgCashRecord");
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = cashoutRecordTable.gameObject;
			CashoutRecordTable component = gameObject.GetComponent<CashoutRecordTable>();
			if (i > 0)
			{
				Object.Instantiate(gameObject);
			}
			gameObject.transform.SetParent(parent);
			gameObject.transform.localPosition = Vector3.up * (65 - i % 4 * 47);
		}
	}

	private void ClickBtnCash()
	{
		reserveCashoutPanel.gameObject.SetActive(value: true);
		reserveCashoutPanel.Init();
	}
}
