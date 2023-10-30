using UnityEngine;
using UnityEngine.UI;

public class FK3_NumberSprite : MonoBehaviour
{
	private Text _text;

	private TextMesh _text2;

	public bool fixFace;

	private bool _faceDown;

	private void Awake()
	{
		_text = base.transform.Find("Number").GetComponent<Text>();
		_text2 = base.transform.Find("Number").GetComponent<TextMesh>();
	}

	public void SetText(string text)
	{
		if ((bool)_text)
		{
			_text.text = text;
		}
		if ((bool)_text2)
		{
			_text2.text = text;
		}
	}

	private void Update()
	{
		if (fixFace)
		{
			base.transform.rotation = (_faceDown ? Quaternion.Euler(0f, 0f, 180f) : Quaternion.Euler(0f, 0f, 0f));
		}
	}

	public void SetFaceDown(bool faceDown)
	{
		_faceDown = faceDown;
	}
}
