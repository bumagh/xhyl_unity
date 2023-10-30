using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonEvent : MonoBehaviour
{
	public int id;

	private Button btnLevel;

	public Button BtnLevel => btnLevel ?? GetComponent<Button>();

	public event Action<int> onLevelButtonOnClick;

	private void Start()
	{
		BtnLevel.onClick.AddListener(ButtonOnclick);
	}

	public void ButtonOnclick()
	{
		if (this.onLevelButtonOnClick != null)
		{
			this.onLevelButtonOnClick(id);
		}
	}
}
