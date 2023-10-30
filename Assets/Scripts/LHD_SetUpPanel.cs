using UnityEngine;
using UnityEngine.UI;

public class LHD_SetUpPanel : MonoBehaviour
{
	public Toggle bg;

	public Toggle eff;

	private void Awake()
	{
		bg.onValueChanged.AddListener(delegate(bool value)
		{
			SetValue(isBg: true, value ? 1 : 0);
		});
		eff.onValueChanged.AddListener(delegate(bool value)
		{
			SetValue(isBg: false, value ? 1 : 0);
		});
	}

	private void OnEnable()
	{
		bg.isOn = (PlayerPrefs.GetFloat("LHDOpenBG", 1f) == 1f);
		eff.isOn = (PlayerPrefs.GetFloat("LHDOpenEff", 1f) == 1f);
		SetValue(isBg: true, bg.isOn ? 1 : 0);
		SetValue(isBg: false, eff.isOn ? 1 : 0);
	}

	private void SetValue(bool isBg, float value)
	{
		LHD_AudioManger.instance.SetValue(isBg, value);
		if (isBg)
		{
			PlayerPrefs.SetFloat("LHDOpenBG", value);
		}
		else
		{
			PlayerPrefs.SetFloat("LHDOpenEff", value);
		}
		PlayerPrefs.Save();
	}
}
