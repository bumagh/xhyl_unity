using System;
using UnityEngine;

public class ToggleController : MonoBehaviour
{
	public GameObject toggleOn;

	public GameObject toggleOff;

	public Action<bool> onToggle;

	public void OnBtnClick_Toggle()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		toggleOn.SetActive(!toggleOn.activeSelf);
		toggleOff.SetActive(!toggleOff.activeSelf);
		if (onToggle != null)
		{
			onToggle(!toggleOn.activeSelf);
		}
	}

	public void SetValue(bool value)
	{
		toggleOn.SetActive(value);
		toggleOff.SetActive(!value);
	}
}
