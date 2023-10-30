using UnityEngine;
using UnityEngine.UI;

public class DZb_MySlider : MonoBehaviour
{
	public enum SliderType
	{
		SoundVolume,
		MusicVolume
	}

	public SliderType sliderType;

	private Slider _slider;

	private Image Fill;

	private void Start()
	{
		_slider = GetComponent<Slider>();
		Fill = base.transform.Find("Fill").GetComponent<Image>();
		Fill.fillAmount = _slider.value;
		switch (sliderType)
		{
		case SliderType.SoundVolume:
			if (PlayerPrefs.HasKey(SliderType.SoundVolume.ToString()))
			{
				_slider.value = PlayerPrefs.GetFloat(SliderType.SoundVolume.ToString());
				Fill.fillAmount = PlayerPrefs.GetFloat(SliderType.SoundVolume.ToString());
			}
			break;
		case SliderType.MusicVolume:
			if (PlayerPrefs.HasKey(SliderType.MusicVolume.ToString()))
			{
				_slider.value = PlayerPrefs.GetFloat(SliderType.MusicVolume.ToString());
				Fill.fillAmount = PlayerPrefs.GetFloat(SliderType.MusicVolume.ToString());
			}
			break;
		}
		_slider.onValueChanged.AddListener(delegate
		{
			Fill.fillAmount = _slider.value;
			switch (sliderType)
			{
			case SliderType.SoundVolume:
				Dzb_GameInfo.SoundVolume = _slider.value;
				PlayerPrefs.SetFloat(SliderType.SoundVolume.ToString(), Dzb_GameInfo.SoundVolume);
				break;
			case SliderType.MusicVolume:
				Dzb_GameInfo.MusicVolume = _slider.value;
				PlayerPrefs.SetFloat(SliderType.MusicVolume.ToString(), Dzb_GameInfo.MusicVolume);
				break;
			}
		});
	}
}
