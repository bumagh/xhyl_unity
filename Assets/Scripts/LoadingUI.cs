using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
	private static LoadingUI s_instance;

	public Slider slider;

	public Text sliderPercent;

	[SerializeField]
	private List<Animator> gearAnimators;

	[SerializeField]
	private List<ParticleSystem> effects;

	public static LoadingUI Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Start()
	{
		slider.maxValue = 1f;
		slider.minValue = 0f;
	}

	public void UpdateProgress(float progress)
	{
		progress += HW2_GVars.RandomTime * 100f;
		if (progress >= 100f)
		{
			progress = 100f;
		}
		slider.value = progress / 100f;
		int num = (int)progress;
		sliderPercent.text = "加载" + num + "%";
	}

	public void StopEffect()
	{
		for (int i = 0; i < gearAnimators.Count; i++)
		{
			gearAnimators[i].enabled = false;
		}
		for (int j = 0; j < effects.Count; j++)
		{
			effects[j].Pause(withChildren: true);
		}
	}
}
