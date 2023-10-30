using System;
using UnityEngine;
using UnityEngine.UI;

public class Test0 : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		base.transform.GetComponent<Text>().text = $"日期：{DateTime.Now.Year}-{DateTime.Now.Month:D2}-{DateTime.Now.Day:D2}";
	}
}
