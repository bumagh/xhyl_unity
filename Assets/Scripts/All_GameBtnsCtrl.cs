using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class All_GameBtnsCtrl : MonoBehaviour
{
	[HideInInspector]
	public Button btnArrow;

	[HideInInspector]
	public Button btnBack;

	[HideInInspector]
	public Button btnSet;

	[HideInInspector]
	public Button btnExcharge;

	[HideInInspector]
	public Button btnSafe;

	[HideInInspector]
	public Button btnTopUp;

	private Transform imgArrow;

	private Transform btns;

	[HideInInspector]
	public bool bShow;

	private bool isMove;

	private int language;

	[HideInInspector]
	public Image imgExcharge;

	private Transform tagPos;

	private Transform oldPos;

	private Coroutine waitHidBtn;

	private void Awake()
	{
		language = (int)ZH2_GVars.language_enum;
		btns = base.transform.Find("Btns");
		btnArrow = base.transform.Find("BtnArrow").GetComponent<Button>();
		imgArrow = base.transform.Find("ImgArrow");
		btnBack = btns.Find("BtnBack").GetComponent<Button>();
		btnSet = btns.Find("BtnSet").GetComponent<Button>();
		btnSafe = btns.Find("BtnSafe").GetComponent<Button>();
		btnExcharge = btns.Find("BtnExcharge").GetComponent<Button>();
		btnTopUp = btns.Find("BtnTopUp").GetComponent<Button>();
		bShow = false;
		isMove = false;
		tagPos = base.transform.parent.Find("TagGameBtns");
		oldPos = base.transform.parent.Find("OldGameBtns");
		imgExcharge = base.transform.parent.Find("ExchargeDialog").GetComponent<Image>();
	}

	public void ShowBtns()
	{
		if (isMove)
		{
			UnityEngine.Debug.LogError("还没移动到位");
			return;
		}
		isMove = true;
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
		transform.DOLocalMoveX(x, 0.35f).OnComplete(delegate
		{
			isMove = false;
		});
		bShow = !bShow;
	}

	private IEnumerator WaitHidBtn(Transform tagTransform, float distance, float time)
	{
		yield return new WaitForSeconds(time);
		Vector3 localPosition = tagTransform.localPosition;
		tagTransform.localPosition = new Vector3(distance, localPosition.y);
	}
}
