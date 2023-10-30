using System.Collections;
using UnityEngine;

[RequireComponent(typeof(XLDT_Book))]
public class XLDT_AutoFlip : MonoBehaviour
{
	public FlipMode Mode;

	public float PageFlipTime = 1f;

	public float TimeBetweenPages = 1f;

	public float DelayBeforeStarting;

	public bool AutoStartFlip = true;

	public XLDT_Book ControledBook;

	public int AnimationFramesCount = 40;

	private bool isFlipping;

	public static bool DrawLock;

	private void Start()
	{
		if (!ControledBook)
		{
			ControledBook = GetComponent<XLDT_Book>();
		}
		ControledBook.OnFlip.AddListener(PageFlipped);
	}

	private void PageFlipped()
	{
		isFlipping = false;
	}

	public void FlipRightPage()
	{
		if (!isFlipping && ControledBook.currentPage < ControledBook.TotalPageCount)
		{
			isFlipping = true;
			float frameTime = PageFlipTime / (float)AnimationFramesCount;
			Vector3 endBottomRight = ControledBook.EndBottomRight;
			float x = endBottomRight.x;
			Vector3 endBottomLeft = ControledBook.EndBottomLeft;
			float xc = (x + endBottomLeft.x) / 2f;
			Vector3 endBottomRight2 = ControledBook.EndBottomRight;
			float x2 = endBottomRight2.x;
			Vector3 endBottomLeft2 = ControledBook.EndBottomLeft;
			float num = (x2 - endBottomLeft2.x) / 2f * 0.9f;
			Vector3 endBottomRight3 = ControledBook.EndBottomRight;
			float h = Mathf.Abs(endBottomRight3.y) * 0.9f;
			float dx = num * 2f / (float)AnimationFramesCount;
			DrawLock = false;
			StartCoroutine(FlipRTL(xc, num, h, frameTime, dx));
		}
	}

	public void FlipLeftPage()
	{
		if (!isFlipping && ControledBook.currentPage > 0)
		{
			isFlipping = true;
			float frameTime = PageFlipTime / (float)AnimationFramesCount;
			Vector3 endBottomRight = ControledBook.EndBottomRight;
			float x = endBottomRight.x;
			Vector3 endBottomLeft = ControledBook.EndBottomLeft;
			float xc = (x + endBottomLeft.x) / 2f;
			Vector3 endBottomRight2 = ControledBook.EndBottomRight;
			float x2 = endBottomRight2.x;
			Vector3 endBottomLeft2 = ControledBook.EndBottomLeft;
			float num = (x2 - endBottomLeft2.x) / 2f * 0.9f;
			Vector3 endBottomRight3 = ControledBook.EndBottomRight;
			float h = Mathf.Abs(endBottomRight3.y) * 0.9f;
			float dx = num * 2f / (float)AnimationFramesCount;
			DrawLock = false;
			StartCoroutine(FlipLTR(xc, num, h, frameTime, dx));
		}
	}

	public void FlipRightPage_T()
	{
		if (!isFlipping && ControledBook.currentPage < ControledBook.TotalPageCount)
		{
			isFlipping = true;
			float frameTime = PageFlipTime / (float)AnimationFramesCount;
			Vector3 endBottomRight = ControledBook.EndBottomRight;
			float x = endBottomRight.x;
			Vector3 endBottomLeft = ControledBook.EndBottomLeft;
			float xc = (x + endBottomLeft.x) / 2f;
			Vector3 endBottomRight2 = ControledBook.EndBottomRight;
			float x2 = endBottomRight2.x;
			Vector3 endBottomLeft2 = ControledBook.EndBottomLeft;
			float num = (x2 - endBottomLeft2.x) / 2f * 0.9f;
			Vector3 endBottomRight3 = ControledBook.EndBottomRight;
			float h = Mathf.Abs(endBottomRight3.y) * 0.9f;
			float dx = num * 2f / (float)AnimationFramesCount;
			DrawLock = false;
			UnityEngine.Debug.LogError("激活翻页2");
			StartCoroutine(FlipRTL_T(xc, num, h, frameTime, dx));
		}
	}

	public void FlipLeftPage_T()
	{
		if (!isFlipping && ControledBook.currentPage > 0)
		{
			isFlipping = true;
			float frameTime = PageFlipTime / (float)AnimationFramesCount;
			Vector3 endBottomRight = ControledBook.EndBottomRight;
			float x = endBottomRight.x;
			Vector3 endBottomLeft = ControledBook.EndBottomLeft;
			float xc = (x + endBottomLeft.x) / 2f;
			Vector3 endBottomRight2 = ControledBook.EndBottomRight;
			float x2 = endBottomRight2.x;
			Vector3 endBottomLeft2 = ControledBook.EndBottomLeft;
			float num = (x2 - endBottomLeft2.x) / 2f * 0.9f;
			Vector3 endBottomRight3 = ControledBook.EndBottomRight;
			float h = Mathf.Abs(endBottomRight3.y) * 0.9f;
			float dx = num * 2f / (float)AnimationFramesCount;
			DrawLock = false;
			StartCoroutine(FlipLTR_T(xc, num, h, frameTime, dx));
		}
	}

	private IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
	{
		float x = xc + xl;
		float y2 = (0f - h) / (xl * xl) * (x - xc) * (x - xc);
		ControledBook.DragRightPageToPoint(new Vector3(x, y2, 0f));
		for (int i = 0; i < AnimationFramesCount; i++)
		{
			if (DrawLock)
			{
				break;
			}
			y2 = (0f - h) / (xl * xl) * (x - xc) * (x - xc);
			ControledBook.UpdateBookRTLToPoint(new Vector3(x, y2, 0f));
			if (i > AnimationFramesCount / 2)
			{
				ControledBook.transform.localPosition = Vector3.Lerp(ControledBook.transform.localPosition, Vector3.right * ControledBook.GetComponent<RectTransform>().rect.width / 2f, frameTime);
			}
			yield return new WaitForSeconds(frameTime);
			x -= dx;
		}
		if (!DrawLock)
		{
			ControledBook.ReleasePage();
		}
	}

	private IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
	{
		float x = xc - xl;
		float y2 = (0f - h) / (xl * xl) * (x - xc) * (x - xc);
		float time = 0f;
		ControledBook.DragLeftPageToPoint(Vector3.right * x + Vector3.up * y2);
		Vector3 initPos = ControledBook.transform.localPosition;
		Vector3 targetPos = initPos + Vector3.left * 511f / 2f;
		WaitForSeconds waitFor = new WaitForSeconds(frameTime);
		for (int i = 0; i < AnimationFramesCount; i++)
		{
			if (DrawLock)
			{
				break;
			}
			y2 = (0f - h) / (xl * xl) * (x - xc) * (x - xc);
			ControledBook.UpdateBookLTRToPoint(Vector3.right * x + Vector3.up * y2);
			if (i > AnimationFramesCount / 2)
			{
				time += frameTime * 2.4f;
				ControledBook.transform.localPosition = Vector3.Lerp(initPos, targetPos, time);
			}
			yield return waitFor;
			x += dx;
		}
		if (!DrawLock)
		{
			ControledBook.transform.localPosition = targetPos;
			ControledBook.ReleasePage();
		}
	}

	private IEnumerator FlipRTL_T(float xc, float xl, float h, float frameTime, float dx)
	{
		float x = xc + xl;
		float y2 = (0f - h) / (xl * xl) * (x - xc) * (x - xc);
		ControledBook.DragRightPageToPoint(new Vector3(x, y2, 0f));
		for (int i = 0; i < AnimationFramesCount; i++)
		{
			y2 = (0f - h) / (xl * xl) * (x - xc) * (x - xc);
			UnityEngine.Debug.Log("坐标：" + x + ":" + y2);
			ControledBook.UpdateBookRTLToPoint(new Vector3(x, y2, 0f));
			if (i > AnimationFramesCount / 2)
			{
				break;
			}
			yield return new WaitForSeconds(frameTime);
			x -= dx;
		}
		UnityEngine.Debug.Log("翻角完成：右到左");
	}

	private IEnumerator FlipLTR_T(float xc, float xl, float h, float frameTime, float dx)
	{
		float x = xc - xl;
		float y2 = (0f - h) / (xl * xl) * (x - xc) * (x - xc);
		ControledBook.DragLeftPageToPoint(new Vector3(x, y2, 0f));
		for (int i = 0; i < AnimationFramesCount; i++)
		{
			y2 = (0f - h) / (xl * xl) * (x - xc) * (x - xc);
			ControledBook.UpdateBookLTRToPoint(new Vector3(x, y2, 0f));
			yield return new WaitForSeconds(0.05f);
			x += dx;
			if (i > AnimationFramesCount / 5)
			{
				break;
			}
		}
		UnityEngine.Debug.Log("翻角完成：左到右");
		ControledBook.TestReleasePage();
	}
}
