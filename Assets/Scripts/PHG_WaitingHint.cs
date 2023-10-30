using UnityEngine;
using UnityEngine.UI;

public class PHG_WaitingHint : PHG_MB_Singleton<PHG_WaitingHint>
{
	private GameObject _goContainer;

	[SerializeField]
	private Image _image;

	private void Awake()
	{
		if (PHG_MB_Singleton<PHG_WaitingHint>._instance == null)
		{
			PHG_MB_Singleton<PHG_WaitingHint>.SetInstance(this);
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
