using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaiJiaLe_LoadPanel : MonoBehaviour
{
	public Image SliderFill;

	public float _sliderValue;

	public static BaiJiaLe_LoadPanel instance;

	public bool IsActive;

	public void Awake()
	{
		instance = this;
	}

	public void OnEnable()
	{
		StartCoroutine(RunLoading());
	}

	private IEnumerator RunLoading()
	{
		while (true)
		{
			if (_sliderValue < 0.85f)
			{
				_sliderValue += 0.01f;
			}
			else if (IsActive)
			{
				_sliderValue += 0.01f;
				if (_sliderValue >= 1f)
				{
					break;
				}
			}
			SliderFill.fillAmount = _sliderValue;
			yield return new WaitForSeconds(0.01f);
		}
		_sliderValue = 0f;
		base.gameObject.SetActive(value: false);
	}
}
