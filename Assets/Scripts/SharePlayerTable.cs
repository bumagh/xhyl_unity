using UnityEngine;
using UnityEngine.UI;

public class SharePlayerTable : MonoBehaviour
{
	[HideInInspector]
	private Text txtPlayerID;

	private Text txtPlayerName;

	private Text txtTeamCont;

	private Text txtPlayerCont;

	private Text txtTeamNum;

	private Text txtDirPlayer;

	private Button btnType;

	private Image imgType;

	[SerializeField]
	private Sprite[] sprites;

	[SerializeField]
	private ShareSetRakebake shareSetRakebake;

	private void Awake()
	{
		txtPlayerID = base.transform.Find("TxtID").GetComponent<Text>();
		txtPlayerName = base.transform.Find("TxtNickname").GetComponent<Text>();
		txtTeamCont = base.transform.Find("TxtTeamCont").GetComponent<Text>();
		txtPlayerCont = base.transform.Find("TxtPlayerCont").GetComponent<Text>();
		txtTeamNum = base.transform.Find("TxtTeamNum").GetComponent<Text>();
		txtDirPlayer = base.transform.Find("TxtDirPlayer").GetComponent<Text>();
		btnType = base.transform.Find("BtnType").GetComponent<Button>();
		imgType = btnType.GetComponent<Image>();
		btnType.onClick.AddListener(ClickBtnType);
	}

	public void Init(string[] strs)
	{
		txtPlayerID.text = strs[0];
		txtPlayerName.text = strs[1];
		txtTeamCont.text = strs[2];
		txtPlayerCont.text = strs[3];
		txtTeamNum.text = strs[4];
		txtDirPlayer.text = strs[5];
		int num = int.Parse(strs[6]);
		imgType.sprite = sprites[num];
	}

	private void ClickBtnType()
	{
		shareSetRakebake.gameObject.SetActive(value: true);
		shareSetRakebake.Init(txtPlayerID.text, txtPlayerName.text);
	}
}
