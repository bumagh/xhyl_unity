using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class STOF_GameBtnsCtrl : MonoBehaviour
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

	private Transform tagPos;

	private Transform oldPos;

	private Coroutine waitHidBtn;

	private void Awake()
	{
		language = STOF_GameInfo.getInstance().Language;
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
		tagPos = base.transform.parent.Find("TagGameBtns");
		oldPos = base.transform.parent.Find("OldGameBtns");
	}

	public void ShowBtns()
	{
		imgArrow.transform.localEulerAngles = ((!bShow) ? Vector3.zero : (Vector3.back * 180f));
		Transform transform = base.transform;
		float x;
		if (bShow)
		{
			Vector3 localPosition = oldPos.localPosition;
			x = localPosition.x;
		}
		else
		{
			Vector3 localPosition2 = tagPos.localPosition;
			x = localPosition2.x;
		}
		transform.DOLocalMoveX(x, 0.5f);
		if (bShow)
		{
			if (waitHidBtn != null)
			{
				StopCoroutine(waitHidBtn);
			}
			Transform transform2 = base.transform;
			Vector3 localPosition3 = oldPos.localPosition;
			waitHidBtn = StartCoroutine(WaitHidBtn(transform2, localPosition3.x, 0.5f));
		}
		bShow = !bShow;
	}

	private IEnumerator WaitHidBtn(Transform tagTransform, float distance, float time)
	{
		yield return new WaitForSeconds(time);
		Vector3 localPosition = tagTransform.localPosition;
		tagTransform.localPosition = new Vector3(distance, localPosition.y);
	}
}
