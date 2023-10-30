using UnityEngine;
using UnityEngine.UI;

public class IntSliderUI : MonoBehaviour
{
	private Slider _slider;

	public Text valueText;

	public int value
	{
		get
		{
			if (_slider != null)
			{
				return (int)_slider.value;
			}
			return 0;
		}
		set
		{
			if (_slider != null)
			{
				_slider.value = value;
			}
		}
	}

	private void Awake()
	{
		_slider = GetComponent<Slider>();
	}

	private void Update()
	{
		if (!(_slider == null))
		{
			valueText.text = _slider.value.ToString();
		}
	}
}
