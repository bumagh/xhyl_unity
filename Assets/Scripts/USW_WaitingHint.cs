using UnityEngine;
using UnityEngine.UI;

public class USW_WaitingHint : USW_MB_Singleton<USW_WaitingHint>
{
	private GameObject _goContainer;

	[SerializeField]
	private Image _image;

	private void Awake()
	{
		if (USW_MB_Singleton<USW_WaitingHint>._instance == null)
		{
			USW_MB_Singleton<USW_WaitingHint>.SetInstance(this);
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
