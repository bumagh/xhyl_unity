using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DarkAnim : MonoBehaviour
{
	private Text text;

	private Coroutine showText;

	private void Start()
	{
		text = GetComponent<Text>();
	}

	private void OnEnable()
	{
		if (text == null)
		{
			text = GetComponent<Text>();
		}
		if (showText != null)
		{
			StopCoroutine(showText);
		}
		showText = StartCoroutine(ShowText(text, ZH2_GVars.ShowTip("请等待上一轮结束", "Please wait round over", string.Empty)));
	}

	private IEnumerator ShowText(Text text, string str)
	{
		if (text == null)
		{
			text = GetComponent<Text>();
		}
		text.text = str;
		string tempStr = str;
		int tempNum = 0;
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			str += ".";
			tempNum++;
			if (tempNum > 5)
			{
				str = tempStr;
				tempNum = 0;
			}
			text.text = str;
		}
	}
}
