using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("I2/I2LocalizeImage")]
public class LKB_I2LocalizeImage : MonoBehaviour
{
	private Dictionary<string, string> _dics;

	[SerializeField]
	[HideInInspector]
	private List<string> _keys;

	[HideInInspector]
	[SerializeField]
	private List<Sprite> _values;

	private Image _image;

	private void Awake()
	{
		PreInit();
	}

	private void Start()
	{
		Refresh();
	}

	private void PreInit()
	{
		_image = GetComponent<Image>();
	}

	public void InitKeysAndValues()
	{
		UnityEngine.Debug.Log("create keys and values");
		_keys = new List<string>();
		_keys.AddRange(LKB_I2Localization.GetAllLanguages());
		_values = new List<Sprite>();
		int i = 0;
		for (int count = _keys.Count; i < count; i++)
		{
			_values.Add(null);
		}
	}

	public List<string> GetKeys()
	{
		return _keys;
	}

	public void Refresh()
	{
		_image = GetComponent<Image>();
		_image.sprite = _getValue();
	}

	private Sprite _getValue()
	{
		int index = 0;
		int i = 0;
		for (int count = _keys.Count; i < count; i++)
		{
			if (LKB_I2Localization.GetCurLanguage() == _keys[i])
			{
				index = i;
				break;
			}
		}
		return _values[index];
	}
}
