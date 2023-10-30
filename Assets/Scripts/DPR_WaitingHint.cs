using UnityEngine;
using UnityEngine.UI;

public class DPR_WaitingHint : DPR_MB_Singleton<DPR_WaitingHint>
{
	private GameObject _goContainer;

	[SerializeField]
	private Image _image;

	private void Awake()
	{
		if (DPR_MB_Singleton<DPR_WaitingHint>._instance == null)
		{
			DPR_MB_Singleton<DPR_WaitingHint>.SetInstance(this);
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
