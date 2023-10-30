using UnityEngine;
using UnityEngine.UI;

public class Translation_Game : MonoBehaviour
{
	private Text text;

	private UILabel label;

	private Image img;

	private UI2DSprite uI2DSprite;

	public string[] str = new string[2];

	public Sprite[] spis = new Sprite[2];

	private void OnEnable()
	{
		Init();
	}

	public void Init()
	{
		if (base.transform.GetComponent<Text>() != null)
		{
			text = base.transform.GetComponent<Text>();
		}
		if (base.transform.GetComponent<UILabel>() != null)
		{
			label = base.transform.GetComponent<UILabel>();
		}
		if (base.transform.GetComponent<Image>() != null)
		{
			img = base.transform.GetComponent<Image>();
		}
		if (base.transform.GetComponent<UI2DSprite>() != null)
		{
			uI2DSprite = base.transform.GetComponent<UI2DSprite>();
		}
		Refresh((int)ZH2_GVars.language_enum);
	}

	public void Refresh(int lang)
	{
		if (lang == 0)
		{
			if (str.Length >= 1)
			{
				if (text != null)
				{
					text.text = str[0];
				}
				if (label != null)
				{
					label.text = str[0];
				}
			}
			if (spis.Length >= 1)
			{
				if (img != null)
				{
					img.sprite = spis[0];
				}
				if (uI2DSprite != null)
				{
					uI2DSprite.sprite2D = spis[0];
				}
			}
			return;
		}
		if (str.Length > 1)
		{
			if (text != null)
			{
				text.text = str[1];
			}
			if (label != null)
			{
				label.text = str[1];
			}
		}
		if (spis.Length > 1)
		{
			if (img != null)
			{
				img.sprite = spis[1];
			}
			if (uI2DSprite != null)
			{
				uI2DSprite.sprite2D = spis[1];
			}
		}
	}
}
