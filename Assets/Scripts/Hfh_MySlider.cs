using UnityEngine;
using UnityEngine.UI;

public class Hfh_MySlider : MonoBehaviour
{
	public enum SliderType
	{
		SoundVolume,
		MusicVolume
	}

	public SliderType sliderType;

	private Slider _slider;

	private Image Fill;
}
