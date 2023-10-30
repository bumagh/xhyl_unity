using UnityEngine;
using UnityEngine.UI;

public class DP_Table : MonoBehaviour
{
	public Transform tfTable0;

	public Transform tfTable1;

	public Transform tfTable2;

	public GameObject objTable3;

	public GameObject objTableInfo;

	public Button btnCurTable;

	public Button btnLeftArrow;

	public Button btnRightArrow;

	public Text txtTableName;

	public Text txtCoin;

	public Text txtMinBet;

	public Text txtMaxBet;

	public Text txtExchange;

	public Text txtPersonCount;

	public void InitTableUI()
	{
		tfTable0.localPosition = Vector3.left * 500f;
		tfTable1.localPosition = Vector3.up * 15f;
		tfTable2.localPosition = Vector3.right * 500f;
		objTable3.SetActive(value: false);
		objTableInfo.SetActive(value: true);
		btnCurTable.transform.localPosition = Vector3.left * 10f;
		btnLeftArrow.gameObject.SetActive(value: true);
		btnRightArrow.gameObject.SetActive(value: true);
	}
}
