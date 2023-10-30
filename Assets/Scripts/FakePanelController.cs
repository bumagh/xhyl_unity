using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FakePanelController : MonoBehaviour
{
	private int m_rechargeNum;

	public GameObject rankButton;

	public static int rankButtonCount = 10;

	public Transform grid;

	private void Awake()
	{
		grid = base.transform.Find("Rank_bg/GameTypeButton/GameTypeButton_bg/Grid");
	}

	private void Start()
	{
		for (int i = 0; i < rankButtonCount; i++)
		{
			GameObject go = UnityEngine.Object.Instantiate(rankButton, Vector3.zero, Quaternion.identity);
			go.transform.parent = grid;
			go.transform.localScale = new Vector3(1f, 1f, 1f);
			go.transform.localPosition = new Vector3(0f, 0f, 0f);
			if (i == 0)
			{
				Action action = delegate
				{
					UnityEngine.Debug.Log("localScale: " + go.transform.localScale);
					UnityEngine.Debug.Log("position: " + go.transform.position);
					UnityEngine.Debug.Log("localPosition: " + go.transform.localPosition);
				};
				Action<int> act2 = delegate(int a)
				{
					UnityEngine.Debug.Log(a);
				};
				act2(3);
				action();
				StartCoroutine(DelayCall(0.1f, delegate
				{
					act2(4);
				}));
			}
		}
		Transform transform = base.transform.Find("title");
		if (transform != null)
		{
			transform.GetComponent<Text>().text = base.gameObject.name;
		}
	}

	private IEnumerator DelayCall(float delay, Action action)
	{
		yield return new WaitForSeconds(delay);
		action();
	}

	public void OnBtnClick_Close()
	{
		UnityEngine.Debug.Log(base.gameObject.name + ">>OnBtnClick_Close:");
		base.gameObject.SetActive(value: false);
	}

	public void OnBtnClick_Sure()
	{
		base.gameObject.SetActive(value: false);
	}
}
