using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DCDF_Roll : MonoBehaviour
{
	[HideInInspector]
	public Transform tfScroll;

	public int indexRoll;

	[HideInInspector]
	public float speed = 14f;

	[HideInInspector]
	public float progress;

	private bool bRoll;

	[HideInInspector]
	public bool bStop;

	private void Awake()
	{
		tfScroll = base.transform.Find("ScrollRoll");
		bStop = true;
	}

	private void Update()
	{
		Loop();
	}

	public void Loop()
	{
		if (bRoll)
		{
			tfScroll.localPosition = Vector3.up * progress;
			progress -= Time.deltaTime * speed * 100f;
		}
	}

	public IEnumerator NormalRoll()
	{
		bStop = false;
		bRoll = true;
		float acc = 40f;
		speed = 0f;
		float accTime = 0.5f;
		float timer2 = 0f;
		while (timer2 < accTime)
		{
			speed += acc * Time.deltaTime;
			timer2 += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(1.5f + 0.3f * (float)indexRoll);
		float acc2 = 10f;
		float timer = 0f;
		while (progress >= 140f)
		{
			speed -= acc2 * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
		bRoll = false;
		tfScroll.DOLocalMoveY(125f, 0.2f);
		tfScroll.DOLocalMoveY(140f, 0.2f).SetDelay(0.2f).OnComplete(delegate
		{
			bStop = true;
			DCDF_SoundManager.Instance.PlayRollEndAudio();
		});
	}

	public IEnumerator SpeedUpRoll()
	{
		bRoll = true;
		float acc = 40f;
		speed = 0f;
		float accTime = 0.5f;
		float timer2 = 0f;
		while (timer2 < accTime)
		{
			speed += acc * Time.deltaTime;
			timer2 += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(1.2f);
		float acc2 = 10f;
		float timer = 0f;
		while (progress >= 140f)
		{
			speed -= acc2 * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
		bRoll = false;
		tfScroll.DOLocalMoveY(125f, 0.2f);
		tfScroll.DOLocalMoveY(140f, 0.2f).SetDelay(0.2f).OnComplete(delegate
		{
			bStop = true;
		});
	}

	public void NormalResetElements(int[] elements, bool bFree)
	{
		for (int i = 3; i < tfScroll.childCount; i++)
		{
			DCDF_ElementPool.GetSingleton().GiveBackElement(tfScroll.GetChild(i).gameObject);
		}
		for (int num = tfScroll.childCount - 1; num >= 0; num--)
		{
			tfScroll.GetChild(num).localPosition = Vector3.up * (-num - 36 - indexRoll * 4) * 140f;
		}
		for (int j = 3; j < 36 + indexRoll * 4; j++)
		{
			int num2 = bFree ? UnityEngine.Random.Range(0, 14) : UnityEngine.Random.Range(0, 11);
			if (num2 >= 11)
			{
				num2++;
			}
			GameObject element = DCDF_ElementPool.GetSingleton().GetElement(num2);
			element.transform.SetParent(tfScroll);
			element.transform.SetAsFirstSibling();
			element.transform.localScale = Vector3.one;
			element.transform.localPosition = Vector3.up * -j * 140f;
		}
		for (int num3 = 2; num3 >= 0; num3--)
		{
			int i2 = elements[num3];
			GameObject element2 = DCDF_ElementPool.GetSingleton().GetElement(i2);
			element2.transform.SetParent(tfScroll);
			element2.transform.SetAsFirstSibling();
			element2.transform.localScale = Vector3.one;
			element2.transform.localPosition = Vector3.up * -num3 * 140f;
		}
		tfScroll.localPosition = Vector3.up * (37 + indexRoll * 4) * 140f;
		progress = (float)(37 + indexRoll * 4) * 140f;
	}

	public void SpeedUpResetElements(int[] elements, bool bFree)
	{
		for (int i = 3; i < tfScroll.childCount; i++)
		{
			DCDF_ElementPool.GetSingleton().GiveBackElement(tfScroll.GetChild(i).gameObject);
		}
		for (int num = tfScroll.childCount - 1; num >= 0; num--)
		{
			tfScroll.GetChild(num).localPosition = Vector3.up * (-num - 24) * 140f;
		}
		for (int j = 3; j < 24; j++)
		{
			int num2 = bFree ? UnityEngine.Random.Range(0, 14) : UnityEngine.Random.Range(0, 11);
			if (num2 >= 11)
			{
				num2++;
			}
			GameObject element = DCDF_ElementPool.GetSingleton().GetElement(num2);
			element.transform.SetParent(tfScroll);
			element.transform.SetAsFirstSibling();
			element.transform.localScale = Vector3.one;
			element.transform.localPosition = Vector3.up * -j * 140f;
		}
		for (int num3 = 2; num3 >= 0; num3--)
		{
			int i2 = elements[num3];
			GameObject element2 = DCDF_ElementPool.GetSingleton().GetElement(i2);
			element2.transform.SetParent(tfScroll);
			element2.transform.SetAsFirstSibling();
			element2.transform.localScale = Vector3.one;
			element2.transform.localPosition = Vector3.up * -num3 * 140f;
		}
		tfScroll.localPosition = Vector3.up * 25f * 140f;
		progress = 3500f;
	}

	public void Init()
	{
		if (tfScroll.childCount > 0)
		{
			for (int i = 0; i < tfScroll.childCount; i++)
			{
				DCDF_ElementPool.GetSingleton().GiveBackElement(tfScroll.GetChild(i).gameObject);
			}
		}
		for (int num = 2; num >= 0; num--)
		{
			int i2 = UnityEngine.Random.Range(0, 11);
			GameObject element = DCDF_ElementPool.GetSingleton().GetElement(i2);
			element.transform.SetParent(tfScroll);
			element.transform.SetAsFirstSibling();
			element.transform.localScale = Vector3.one;
			element.transform.localPosition = Vector3.up * -num * 140f;
		}
		tfScroll.localPosition = Vector3.up * 140f;
		progress = 140f;
	}
}
