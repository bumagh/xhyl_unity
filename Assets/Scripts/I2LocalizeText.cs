using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("I2/I2LocalizeText")]
public class I2LocalizeText : MonoBehaviour
{
	private bool _started;

	private Dictionary<string, string> _dics;

	[HideInInspector]
	[SerializeField]
	private List<string> _keys;

	[SerializeField]
	[HideInInspector]
	private List<string> _values;

	[SerializeField]
	private int _zh_fontSize;

	[SerializeField]
	private int _en_fontSize;

	[SerializeField]
	private Font _zh_font;

	[SerializeField]
	private Font _en_font;

	private void Awake()
	{
		PreInit();
	}

	private void OnEnable()
	{
		Refresh();
	}

	private void Start()
	{
		Refresh();
		_started = true;
	}

	private void PreInit()
	{
		_dics = new Dictionary<string, string>();
		InitKeysAndValues();
		if (_keys != null && _keys.Count > 0)
		{
			if (_values != null && _values.Count == _keys.Count)
			{
				int count = _keys.Count;
				for (int i = 0; i < count; i++)
				{
					_dics[_keys[i]] = _values[i];
				}
			}
			else
			{
				UnityEngine.Debug.LogError("keys and values do not match");
			}
		}
		else
		{
			UnityEngine.Debug.LogError("_keys have no value");
		}
	}

	public void InitKeysAndValues()
	{
		UnityEngine.Debug.Log("create keys and values");
		_keys = new List<string>();
		_keys.AddRange(I2Localization.GetAllLanguages());
		_values = new List<string>();
		int i = 0;
		for (int count = _keys.Count; i < count; i++)
		{
			_values.Add(string.Empty);
		}
	}

	public void SetValues(string zh, string en)
	{
		_values[0] = zh;
		_values[1] = en;
		Refresh();
	}

	public void Refresh()
	{
		Text component = GetComponent<Text>();
		component.text = _getValue();
		if (_zh_fontSize > 0 && I2Localization.GetCurLanguage() == "zh")
		{
			component.fontSize = _zh_fontSize;
		}
		if (_en_fontSize > 0 && I2Localization.GetCurLanguage() == "en")
		{
			component.fontSize = _en_fontSize;
		}
		if (_zh_font != null && I2Localization.GetCurLanguage() == "zh")
		{
			component.font = _zh_font;
		}
		if (_en_font != null && I2Localization.GetCurLanguage() == "en")
		{
			component.font = _en_font;
		}
	}

	private string _getValue()
	{
		int index = 0;
		int i = 0;
		for (int count = _keys.Count; i < count; i++)
		{
			if (I2Localization.GetCurLanguage() == _keys[i])
			{
				index = i;
				break;
			}
		}
		return _values[index];
	}

	public List<string> GetKeys()
	{
		return _keys;
	}
}
