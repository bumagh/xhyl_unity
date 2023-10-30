using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Dzb_Timer : MonoBehaviour
{
	private Dzb_Seat _Seat;

	private Thread TimeRun;

	public void Activate(Dzb_Seat seat)
	{
		_Seat = seat;
		if (TimeRun == null)
		{
			TimeRun = new Thread(TimeMethod);
			TimeRun.Start();
		}
		UnityEngine.Debug.Log("??" + base.transform.parent.name);
	}

	private void Update()
	{
		if (_Seat != null)
		{
			base.transform.Find("Text").GetComponent<Text>().text = _Seat.hour + ":" + _Seat.minute + ":" + _Seat.second;
		}
	}

	private void TimeMethod()
	{
		while (true)
		{
			Thread.Sleep(1000);
			if (_Seat == null)
			{
				break;
			}
			if (_Seat.second < 0)
			{
				continue;
			}
			if (_Seat.second == 0 && _Seat.minute >= 0)
			{
				if (_Seat.minute == 0 && _Seat.hour > 0)
				{
					_Seat.minute = 60;
					_Seat.hour--;
				}
				_Seat.second = 60;
				_Seat.minute--;
			}
			_Seat.second--;
		}
	}

	private void OnDestroy()
	{
		if (TimeRun != null)
		{
			TimeRun.Abort();
		}
	}
}
