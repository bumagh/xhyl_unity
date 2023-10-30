using UnityEngine;
using UnityEngine.UI;

public class ESP_WaitingHint : ESP_MB_Singleton<ESP_WaitingHint>
{
	private GameObject _goContainer;

	[SerializeField]
	private Image _image;

	private void Awake()
	{
		if (ESP_MB_Singleton<ESP_WaitingHint>._instance == null)
		{
			ESP_MB_Singleton<ESP_WaitingHint>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		_goContainer = base.gameObject;
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	private void Update()
	{
		if (_image != null)
		{
			_image.transform.Rotate(Vector3.forward, 360f * Time.deltaTime);
		}
	}
}
