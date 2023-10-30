using System;
using UnityEngine;

public class IconButtonsInfo : MonoBehaviour
{
	public int index;

	private void Awake()
	{
		try
		{
			string s = base.transform.parent.name.Replace("btn_", string.Empty).Trim();
			index = int.Parse(s);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("es: " + arg);
		}
	}

	public void OnBtnClick_Icon()
	{
		UnityEngine.Debug.Log(index);
		UserInfoViewController.Get().ChangeIcon(this);
	}
}
