using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("I2/I2LocalizeText")]
public class DPR_I2LocalizeText : MonoBehaviour
{
	private bool _started;

	private Dictionary<string, string> _dics;

	[SerializeField]
	[HideInInspector]
	private List<string> _keys;

	[HideInInspector]
	[SerializeField]
	private List<string> _values;

	private void OnEnable()
	{
		if (_started)
		{
			OnLocalize();
		}
	}

	private void Awake()
	{
		PreInit();
	}

	private void Start()
	{
		Refresh();
		_started = true;
		OnLocalize();
	}

	private void PreInit()
	{
		_dics = new Dictionary<string, string>();
		if (_keys == null)
		{
		}
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
		_keys.AddRange(DPR_I2Localization.GetAllLanguages());
		_values = new List<string>();
		int i = 0;
		for (int count = _keys.Count; i < count; i++)
		{
			_values.Add(string.Empty);
		}
	}

	public void OnLocalize()
	{
	}

	public List<string> GetKeys()
	{
		return _keys;
	}

	public void Refresh()
	{
		Text component = GetComponent<Text>();
		component.text = _getValue();
	}

	private string _getValue()
	{
		int index = 0;
		int i = 0;
		for (int count = _keys.Count; i < count; i++)
		{
			if (DPR_I2Localization.GetCurLanguage() == _keys[i])
			{
				index = i;
				break;
			}
		}
		return _values[index];
	}

	public void SetValues(string zh, string en)
	{
		_values[0] = zh;
		_values[1] = en;
		Refresh();
	}
}
