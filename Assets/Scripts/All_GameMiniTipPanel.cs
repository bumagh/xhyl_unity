using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class All_GameMiniTipPanel : MonoBehaviour
{
	public static All_GameMiniTipPanel publicMiniTip;

	public GameObject tipPre;

	public Transform tipStartPos;

	public Transform tipTagPos;

	private UIDepth depth;

	private FK3_UIDepth fkDepth;

	private void Awake()
	{
		publicMiniTip = this;
		if (fkDepth == null)
		{
			fkDepth = base.gameObject.GetComponent<FK3_UIDepth>();
			if (fkDepth != null)
			{
				fkDepth.order = 11;
			}
		}
		if (fkDepth != null)
		{
			return;
		}
		if (depth == null)
		{
			depth = base.gameObject.GetComponent<UIDepth>();
			if (depth == null)
			{
				depth = base.gameObject.AddComponent<UIDepth>();
			}
		}
		if (depth != null)
		{
			depth.order = 10;
			depth.isHaveButton = false;
		}
	}

	public void ShowTip(string tips)
	{
		GameObject objectTip = UnityEngine.Object.Instantiate(tipPre, base.transform);
		objectTip.transform.Find("Text").GetComponent<Text>().text = tips;
		objectTip.transform.localPosition = tipStartPos.localPosition;
		objectTip.transform.DOLocalMove(tipTagPos.localPosition, 1f).OnComplete(delegate
		{
			objectTip.transform.DOScale(Vector3.one, 1f).OnComplete(delegate
			{
				UnityEngine.Object.Destroy(objectTip);
			});
		});
	}
}
