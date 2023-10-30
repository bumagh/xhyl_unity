using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LLD_Border : MonoBehaviour
{
	[SerializeField]
	private CSF_SpiBorder border;

	private Image img;

	private bool _hasPreInited;

	private static HashSet<string> stateSet = new HashSet<string>
	{
		"green",
		"blue",
		"pink",
		"color"
	};

	private WaitForSeconds wait = new WaitForSeconds(0.05f);

	private Coroutine coroutine;

	private string state;

	private void Awake()
	{
		_preInit();
	}

	private void _preInit()
	{
		if (!_hasPreInited)
		{
			_hasPreInited = true;
			img = GetComponent<Image>();
		}
	}

	public void PlaySate(string state)
	{
		if (!_hasPreInited)
		{
			_preInit();
		}
		if (!stateSet.Contains(state))
		{
			UnityEngine.Debug.LogError($"state:[{state}] not exist");
		}
		else
		{
			Play(state);
		}
	}

	public void Show(bool isShow)
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}
		base.gameObject.SetActive(isShow);
	}

	private void Play(string state)
	{
		this.state = state;
		if (base.gameObject.activeInHierarchy)
		{
			coroutine = StartCoroutine(PlayColorAnim());
		}
	}

	private IEnumerator PlayColorAnim()
	{
		int index2 = 0;
		while (true)
		{
			img.sprite = border.spis[index2];
			yield return wait;
			index2++;
			index2 %= border.spis.Length;
			yield return wait;
		}
	}

	private IEnumerator PlayOtherAnim()
	{
		int index = 0;
		if (state == "green")
		{
			index = 4;
		}
		else if (state == "blue")
		{
			index = 2;
		}
		else if (state == "pink")
		{
			index = 0;
		}
		img.sprite = border.spis[index];
		yield return wait;
		img.enabled = false;
		yield return wait;
		img.enabled = true;
	}
}
