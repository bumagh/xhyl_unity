using UnityEngine;
using UnityEngine.UI;

public class Translation : MonoBehaviour
{
	private Text text;

	public string[] str = new string[3];

	private Image img;

	public Sprite[] spis = new Sprite[3];

	public void Init()
	{
		if (base.transform.GetComponent<Text>() != null)
		{
			text = base.transform.GetComponent<Text>();
			str[0] = text.text;
		}
		if (base.transform.GetComponent<Image>() != null)
		{
			img = base.transform.GetComponent<Image>();
			img.sprite = spis[0];
		}
	}

	public void Refresh(string lang)
	{
		if (base.transform.GetComponent<Text>() != null)
		{
			text = base.transform.GetComponent<Text>();
			if (lang == "0")
			{
				if (str.Length < 1)
				{
					return;
				}
				text.text = str[0];
			}
			else if (lang == "1")
			{
				if (str.Length < 2)
				{
					return;
				}
				text.text = str[1];
			}
			else if (lang == "2")
			{
				if (str.Length < 3)
				{
					text.text = str[1];
					return;
				}
				text.text = str[2];
			}
		}
		if (!(base.transform.GetComponent<Image>() != null))
		{
			return;
		}
		img = base.transform.GetComponent<Image>();
		if (lang == "0")
		{
			if (spis.Length >= 1)
			{
				img.sprite = spis[0];
			}
		}
		else if (lang == "1")
		{
			if (spis.Length >= 2)
			{
				img.sprite = spis[1];
			}
		}
		else if (lang == "2")
		{
			if (spis.Length < 3)
			{
				img.sprite = spis[1];
			}
			else
			{
				img.sprite = spis[2];
			}
		}
	}
}
