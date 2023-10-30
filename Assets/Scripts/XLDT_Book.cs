using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class XLDT_Book : MonoBehaviour
{
	public Canvas canvas;

	[SerializeField]
	private RectTransform BookPanel;

	public Sprite background;

	public Sprite[] bookPages;

	public bool interactable = true;

	public bool enableShadowEffect = true;

	public int currentPage;

	public Image ClippingPlane;

	public Image NextPageClip;

	public Image Shadow;

	public Image ShadowLTR;

	public Image Left;

	public Image LeftNext;

	public Image Right;

	public Image RightNext;

	public UnityEvent OnFlip;

	private float radius1;

	private float radius2;

	private Vector3 sb;

	private Vector3 st;

	private Vector3 c;

	private Vector3 ebr;

	private Vector3 ebl;

	private Vector3 f;

	private bool pageDragging;

	private FlipMode mode;

	private Vector3 initPos;

	private Coroutine currentCoroutine;

	public int TotalPageCount => bookPages.Length;

	public Vector3 EndBottomLeft => ebl;

	public Vector3 EndBottomRight => ebr;

	public float Height => BookPanel.rect.height;

	private void Start()
	{
		float num = 1f;
		if ((bool)canvas)
		{
			num = canvas.scaleFactor;
		}
		float num2 = (BookPanel.rect.width * num - 1f) / 2f;
		float num3 = BookPanel.rect.height * num;
		Left.gameObject.SetActive(value: false);
		Right.gameObject.SetActive(value: false);
		UpdateSprites();
		Vector3 global = BookPanel.transform.position + new Vector3(0f, (0f - num3) / 2f);
		sb = transformPoint(global);
		Vector3 global2 = BookPanel.transform.position + new Vector3(num2, (0f - num3) / 2f);
		ebr = transformPoint(global2);
		Vector3 global3 = BookPanel.transform.position + new Vector3(0f - num2, (0f - num3) / 2f);
		ebl = transformPoint(global3);
		Vector3 global4 = BookPanel.transform.position + new Vector3(0f, num3 / 2f);
		st = transformPoint(global4);
		radius1 = Vector2.Distance(sb, ebr);
		float num4 = num2 / num;
		float num5 = num3 / num;
		radius2 = Mathf.Sqrt(num4 * num4 + num5 * num5);
		ClippingPlane.rectTransform.sizeDelta = new Vector2(num4 * 2f, num5 + num4 * 2f);
		Shadow.rectTransform.sizeDelta = new Vector2(num4, num5 + num4 * 0.6f);
		ShadowLTR.rectTransform.sizeDelta = new Vector2(num4, num5 + num4 * 0.6f);
		NextPageClip.rectTransform.sizeDelta = new Vector2(num4, num5 + num4 * 0.6f);
		initPos = base.transform.localPosition;
	}

	public Vector3 transformPoint(Vector3 global)
	{
		Vector2 v = BookPanel.InverseTransformPoint(global);
		return v;
	}

	public void UpdateBook()
	{
		f = Vector3.Lerp(f, transformPoint(UnityEngine.Input.mousePosition), Time.deltaTime * 10f);
		if (mode == FlipMode.RightToLeft)
		{
			UpdateBookRTLToPoint(f);
		}
		else
		{
			UpdateBookLTRToPoint(f);
		}
	}

	public void UpdateBookLTRToPoint(Vector3 followLocation)
	{
		mode = FlipMode.LeftToRight;
		f = followLocation;
		ShadowLTR.transform.SetParent(ClippingPlane.transform, worldPositionStays: true);
		ShadowLTR.transform.localPosition = Vector3.zero;
		ShadowLTR.transform.localEulerAngles = Vector3.zero;
		Left.transform.SetParent(ClippingPlane.transform, worldPositionStays: true);
		Right.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		LeftNext.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		c = Calc_C_Position(followLocation);
		Vector3 t;
		float num = Calc_T0_T1_Angle(c, ebl, out t);
		if (num < 0f)
		{
			num += 180f;
		}
		ClippingPlane.transform.eulerAngles = Vector3.forward * (num - 90f);
		ClippingPlane.transform.position = BookPanel.TransformPoint(t);
		Left.transform.position = BookPanel.TransformPoint(c);
		float y = t.y - c.y;
		float x = t.x - c.x;
		float num2 = Mathf.Atan2(y, x) * 57.29578f;
		Left.transform.eulerAngles = Vector3.forward * (num2 - 180f);
		NextPageClip.transform.eulerAngles = Vector3.forward * (num - 90f);
		NextPageClip.transform.position = BookPanel.TransformPoint(t);
		LeftNext.transform.SetParent(NextPageClip.transform, worldPositionStays: true);
		Right.transform.SetParent(ClippingPlane.transform, worldPositionStays: true);
		Right.transform.SetAsFirstSibling();
		ShadowLTR.rectTransform.SetParent(Left.rectTransform, worldPositionStays: true);
	}

	public void UpdateBookRTLToPoint(Vector3 followLocation)
	{
		mode = FlipMode.RightToLeft;
		f = followLocation;
		Shadow.transform.SetParent(ClippingPlane.transform, worldPositionStays: true);
		Shadow.transform.localPosition = new Vector3(0f, 0f, 0f);
		Shadow.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		Right.transform.SetParent(ClippingPlane.transform, worldPositionStays: true);
		Left.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		RightNext.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		c = Calc_C_Position(followLocation);
		Vector3 t;
		float num = Calc_T0_T1_Angle(c, ebr, out t);
		if (num >= -90f)
		{
			num -= 180f;
		}
		ClippingPlane.rectTransform.pivot = new Vector2(1f, 0.35f);
		ClippingPlane.transform.eulerAngles = new Vector3(0f, 0f, num + 90f);
		ClippingPlane.transform.position = BookPanel.TransformPoint(t);
		Right.transform.position = BookPanel.TransformPoint(c);
		float y = t.y - c.y;
		float x = t.x - c.x;
		float z = Mathf.Atan2(y, x) * 57.29578f;
		Right.transform.eulerAngles = new Vector3(0f, 0f, z);
		NextPageClip.transform.eulerAngles = new Vector3(0f, 0f, num + 90f);
		NextPageClip.transform.position = BookPanel.TransformPoint(t);
		RightNext.transform.SetParent(NextPageClip.transform, worldPositionStays: true);
		Left.transform.SetParent(ClippingPlane.transform, worldPositionStays: true);
		Left.transform.SetAsFirstSibling();
		Shadow.rectTransform.SetParent(Right.rectTransform, worldPositionStays: true);
	}

	private float Calc_T0_T1_Angle(Vector3 c, Vector3 bookCorner, out Vector3 t1)
	{
		Vector3 vector = (c + bookCorner) / 2f;
		float num = bookCorner.y - vector.y;
		float x = bookCorner.x - vector.x;
		float num2 = Mathf.Atan2(num, x);
		float num3 = 90f - num2;
		float t2 = vector.x - num * Mathf.Tan(num2);
		t2 = normalizeT1X(t2, bookCorner, sb);
		t1 = new Vector3(t2, sb.y, 0f);
		float y = t1.y - vector.y;
		float x2 = t1.x - vector.x;
		return Mathf.Atan2(y, x2) * 57.29578f;
	}

	private float normalizeT1X(float t1, Vector3 corner, Vector3 sb)
	{
		if (t1 > sb.x && sb.x > corner.x)
		{
			return sb.x;
		}
		if (t1 < sb.x && sb.x < corner.x)
		{
			return sb.x;
		}
		return t1;
	}

	private Vector3 Calc_C_Position(Vector3 followLocation)
	{
		f = followLocation;
		float y = f.y - sb.y;
		float x = f.x - sb.x;
		float num = Mathf.Atan2(y, x);
		Vector3 vector = new Vector3(radius1 * Mathf.Cos(num), radius1 * Mathf.Sin(num), 0f) + sb;
		float num2 = Vector2.Distance(f, sb);
		Vector3 vector2 = (!(num2 < radius1)) ? vector : f;
		float y2 = vector2.y - st.y;
		float x2 = vector2.x - st.x;
		float num3 = Mathf.Atan2(y2, x2);
		Vector3 vector3 = new Vector3(radius2 * Mathf.Cos(num3), radius2 * Mathf.Sin(num3), 0f) + st;
		float num4 = Vector2.Distance(vector2, st);
		if (num4 > radius2)
		{
			vector2 = vector3;
		}
		return vector2;
	}

	public void DragRightPageToPoint(Vector3 point)
	{
		if (currentPage < bookPages.Length)
		{
			pageDragging = true;
			mode = FlipMode.RightToLeft;
			f = point;
			NextPageClip.rectTransform.pivot = new Vector2(0f, 0.12f);
			ClippingPlane.rectTransform.pivot = new Vector2(1f, 0.35f);
			Left.gameObject.SetActive(value: true);
			Left.rectTransform.pivot = new Vector2(0f, 0f);
			Left.transform.position = RightNext.transform.position;
			Left.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			Left.sprite = ((currentPage >= bookPages.Length) ? background : bookPages[currentPage]);
			Left.transform.SetAsFirstSibling();
			Right.gameObject.SetActive(value: true);
			Right.transform.position = RightNext.transform.position;
			Right.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			Right.sprite = ((currentPage >= bookPages.Length - 1) ? background : bookPages[currentPage + 1]);
			RightNext.sprite = ((currentPage >= bookPages.Length - 2) ? background : bookPages[currentPage + 2]);
			LeftNext.transform.SetAsFirstSibling();
			if (enableShadowEffect)
			{
				Shadow.gameObject.SetActive(value: true);
			}
			UpdateBookRTLToPoint(f);
		}
	}

	public void OnMouseDragRightPage()
	{
		if (interactable)
		{
			DragRightPageToPoint(transformPoint(UnityEngine.Input.mousePosition));
		}
	}

	public void DragLeftPageToPoint(Vector3 point)
	{
		if (currentPage > 0)
		{
			pageDragging = true;
			mode = FlipMode.LeftToRight;
			f = point;
			NextPageClip.rectTransform.pivot = new Vector2(1f, 0.12f);
			ClippingPlane.rectTransform.pivot = new Vector2(0f, 0.35f);
			Right.gameObject.SetActive(value: true);
			Right.transform.position = LeftNext.transform.position;
			Right.sprite = bookPages[currentPage - 1];
			Right.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			Right.transform.SetAsFirstSibling();
			Left.gameObject.SetActive(value: true);
			Left.rectTransform.pivot = new Vector2(1f, 0f);
			Left.transform.position = LeftNext.transform.position;
			Left.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			Left.sprite = ((currentPage < 2) ? background : bookPages[currentPage - 2]);
			LeftNext.sprite = ((currentPage < 3) ? background : bookPages[currentPage - 3]);
			RightNext.transform.SetAsFirstSibling();
			if (enableShadowEffect)
			{
				ShadowLTR.gameObject.SetActive(value: true);
			}
			UpdateBookLTRToPoint(f);
		}
	}

	public void OnMouseDragLeftPage()
	{
		if (interactable)
		{
			DragLeftPageToPoint(transformPoint(UnityEngine.Input.mousePosition));
		}
	}

	public void OnMouseRelease()
	{
		if (interactable)
		{
			ReleasePage();
		}
	}

	public void ReleasePage()
	{
		if (pageDragging)
		{
			pageDragging = false;
			float num = Vector2.Distance(c, ebl);
			float num2 = Vector2.Distance(c, ebr);
			if (num2 < num && mode == FlipMode.RightToLeft)
			{
				TweenBack();
			}
			else if (num2 > num && mode == FlipMode.LeftToRight)
			{
				TweenBack();
			}
			else
			{
				TweenForward();
			}
		}
	}

	public void TestReleasePage()
	{
		if (pageDragging)
		{
			pageDragging = false;
			float num = Vector2.Distance(c, ebl);
			float num2 = Vector2.Distance(c, ebr);
			TweenBack();
		}
	}

	private void UpdateSprites()
	{
		LeftNext.sprite = ((currentPage <= 0 || currentPage > bookPages.Length) ? background : bookPages[currentPage - 1]);
		RightNext.sprite = ((currentPage < 0 || currentPage >= bookPages.Length) ? background : bookPages[currentPage]);
	}

	public void TweenForward()
	{
		if (mode == FlipMode.RightToLeft)
		{
			currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f, delegate
			{
				Flip();
			}));
		}
		else
		{
			currentCoroutine = StartCoroutine(TweenTo(ebr, 0.15f, delegate
			{
				Flip();
			}));
		}
	}

	private void Flip()
	{
		if (mode == FlipMode.RightToLeft)
		{
			currentPage += 2;
		}
		else
		{
			currentPage -= 2;
		}
		LeftNext.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		Left.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		LeftNext.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		Left.gameObject.SetActive(value: false);
		Right.gameObject.SetActive(value: false);
		Right.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		RightNext.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		UpdateSprites();
		Shadow.gameObject.SetActive(value: false);
		ShadowLTR.gameObject.SetActive(value: false);
		if (OnFlip != null)
		{
			OnFlip.Invoke();
		}
	}

	public void ResetFlip()
	{
		XLDT_AutoFlip.DrawLock = true;
		LeftNext.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		Left.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		LeftNext.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		Left.gameObject.SetActive(value: false);
		Right.gameObject.SetActive(value: false);
		Right.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		RightNext.transform.SetParent(BookPanel.transform, worldPositionStays: true);
		UpdateSprites();
		Shadow.gameObject.SetActive(value: false);
		ShadowLTR.gameObject.SetActive(value: false);
		if (OnFlip != null)
		{
			OnFlip.Invoke();
		}
	}

	public void TweenBack()
	{
		if (mode == FlipMode.RightToLeft)
		{
			currentCoroutine = StartCoroutine(TweenTo(ebr, 0.5f, delegate
			{
				UpdateSprites();
				RightNext.transform.SetParent(BookPanel.transform);
				Right.transform.SetParent(BookPanel.transform);
				Left.gameObject.SetActive(value: false);
				Right.gameObject.SetActive(value: false);
				pageDragging = false;
				ResetFlip();
			}));
		}
		else
		{
			currentCoroutine = StartCoroutine(TweenTo(ebl, 0.5f, delegate
			{
				UpdateSprites();
				LeftNext.transform.SetParent(BookPanel.transform);
				Left.transform.SetParent(BookPanel.transform);
				Left.gameObject.SetActive(value: false);
				Right.gameObject.SetActive(value: false);
				pageDragging = false;
				ResetFlip();
			}));
		}
	}

	public void ResetZero()
	{
		currentPage = 2;
		BookPanel.anchoredPosition = Vector2.zero;
		LeftNext.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		RightNext.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
	}

	public IEnumerator TweenTo(Vector3 to, float duration, Action onFinish)
	{
		int steps = (int)(duration / 0.025f);
		Vector3 displacement = (to - f) / steps;
		for (int i = 0; i < steps - 1; i++)
		{
			if (XLDT_AutoFlip.DrawLock)
			{
				break;
			}
			if (mode == FlipMode.RightToLeft)
			{
				UpdateBookRTLToPoint(f + displacement);
			}
			else
			{
				UpdateBookLTRToPoint(f + displacement);
			}
			yield return new WaitForSeconds(0.025f);
		}
		onFinish?.Invoke();
	}

	public void ResetPage()
	{
		base.transform.localPosition = initPos;
		currentPage = 2;
		RightNext.sprite = background;
		LeftNext.sprite = bookPages[1];
	}
}
