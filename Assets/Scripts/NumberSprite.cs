using UnityEngine;
using UnityEngine.UI;

public class NumberSprite : MonoBehaviour
{
	private Text _text;

	public bool fixFace;

	private bool _faceDown;

	private void Awake()
	{
		_text = base.transform.Find("Number").GetComponent<Text>();
	}

	public void SetText(string text)
	{
		_text.text = text;
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
