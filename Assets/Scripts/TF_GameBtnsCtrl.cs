using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TF_GameBtnsCtrl : MonoBehaviour
{
	[HideInInspector]
	public Button btnArrow;

	[HideInInspector]
	public Button btnBack;

	[HideInInspector]
	public Button btnSet;

	[HideInInspector]
	public Button btnChat;

	[HideInInspector]
	public Button btnExcharge;

	[HideInInspector]
	public Button btnInOut;

	private Transform imgArrow;

	[HideInInspector]
	public bool bShow;

	[SerializeField]
	private Sprite[] spiExcharge;

	[HideInInspector]
	public Image imgExcharge;

	private int language;

	private void Awake()
	{
		language = TF_GameInfo.getInstance().Language;
		btnArrow = base.transform.Find("BtnArrow").GetComponent<Button>();
		btnBack = base.transform.Find("BtnBack").GetComponent<Button>();
		btnSet = base.transform.Find("BtnSet").GetComponent<Button>();
		btnChat = base.transform.Find("BtnChat").GetComponent<Button>();
		btnExcharge = base.transform.Find("BtnExcharge").GetComponent<Button>();
		btnInOut = base.transform.Find("BtnInOut").GetComponent<Button>();
		imgArrow = base.transform.Find("ImgArrow");
		imgExcharge = base.transform.parent.Find("ExchargeDialog").GetComponent<Image>();
		imgExcharge.sprite = spiExcharge[language];
		bShow = false;
	}

	public void ShowBtns()
	{
		bShow = !bShow;
		imgArrow.transform.localEulerAngles = (bShow ? Vector3.zero : (Vector3.back * 180f));
		base.transform.DOLocalMoveX(bShow ? (-585) : (-695), 0.5f);
	}
}
