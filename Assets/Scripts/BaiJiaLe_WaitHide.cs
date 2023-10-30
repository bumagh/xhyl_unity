using UnityEngine;
using UnityEngine.UI;

public class BaiJiaLe_WaitHide : MonoBehaviour
{
	private float _time;

	public void Show(string msg)
	{
		_time = 2f;
		base.gameObject.SetActive(value: true);
		base.transform.Find("Text").GetComponent<Text>().text = msg;
	}

	private void Update()
	{
		if (_time < 0f)
		{
			base.gameObject.SetActive(value: false);
		}
		_time -= Time.deltaTime;
	}
}
