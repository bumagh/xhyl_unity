using System.Collections;
using UnityEngine;

public class STWM_ScrollAnim : MonoBehaviour
{
	private float speed = 14f;

	private float progress;

	private bool bRoll;

	private float[] posScroll = new float[9]
	{
		15f,
		805f,
		565f,
		685f,
		230f,
		925f,
		345f,
		125f,
		455f
	};

	private void Update()
	{
		if (bRoll)
		{
			base.transform.localPosition = Vector3.down * progress;
			progress += Time.deltaTime * speed * 100f;
			progress %= 1024f;
		}
	}

	public IEnumerator AniStart(STWM_CellType startType)
	{
		yield return null;
		bRoll = true;
		float acc = 25f;
		speed = 1.5f;
		float accTime = 0.5f;
		float timer = 0f;
		progress = posScroll[Random.Range(0, 9)];
		while (timer < accTime)
		{
			speed += acc * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator AniStart2(int startpos)
	{
		yield return null;
		bRoll = true;
		float acc = 25f;
		speed = 1.5f;
		float accTime = 0.5f;
		float timer = 0f;
		progress = posScroll[posScroll.Length - startpos];
		while (timer < accTime)
		{
			speed += acc * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator AniStop(STWM_CellType endType)
	{
		yield return null;
		float acc = 10f;
		float accTime = 0.3f;
		float timer = 0f;
		while (timer < accTime)
		{
			speed -= acc * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
		bRoll = false;
		progress = posScroll[(int)(endType - 1)];
		base.transform.localPosition = Vector3.down * progress;
		base.gameObject.SetActive(value: false);
	}
}
