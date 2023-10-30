using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeekTimeGif : MonoBehaviour
{
	private Text text0;

	private Text text1;

	private Text text2;

	private int dayNum;

	private int weekNum;

	private Coroutine changText;

	private Coroutine showResiduTime;

	private bool isShowTime;

	private DateTime nowTime;

	private DateTime endTime;

	private void Awake()
	{
		text0 = base.transform.Find("Text0").GetComponent<Text>();
		text1 = base.transform.Find("Text1").GetComponent<Text>();
		text2 = base.transform.Find("Text2").GetComponent<Text>();
		dayNum = UnityEngine.Random.Range(359410, 700000);
		weekNum = UnityEngine.Random.Range(359410, 700000);
	}

	private void OnEnable()
	{
		if (changText != null)
		{
			StopCoroutine(changText);
		}
		if (showResiduTime != null)
		{
			StopCoroutine(showResiduTime);
		}
		changText = StartCoroutine(ChangText());
		showResiduTime = StartCoroutine(ShowResiduTime());
	}

	private IEnumerator ChangText()
	{
		while (true)
		{
			isShowTime = false;
			yield return new WaitForSeconds(1f);
			ShowText("WEEKLY", weekNum.ToString(), RemainingDays() + " Day");
			IsShowText(isShow: true);
			yield return new WaitForSeconds(8f);
			IsShowText(isShow: false);
			yield return new WaitForSeconds(1f);
			isShowTime = true;
			ShowText("DAILY", dayNum.ToString(), RemainingTimr() + " S");
			IsShowText(isShow: true);
			yield return new WaitForSeconds(8f);
			IsShowText(isShow: false);
		}
	}

	private void IsShowText(bool isShow)
	{
		if (isShow)
		{
			text0.transform.DOScale(Vector3.one, 1f);
			text1.transform.DOScale(Vector3.one, 1f);
			text2.transform.DOScale(Vector3.one, 1f);
		}
		else
		{
			text0.transform.DOScale(Vector3.zero, 1f);
			text1.transform.DOScale(Vector3.zero, 1f);
			text2.transform.DOScale(Vector3.zero, 1f);
		}
	}

	private void ShowText(string txt0, string txt1, string txt2)
	{
		text0.text = txt0;
		text1.text = txt1;
		text2.text = txt2;
	}

	private IEnumerator ShowResiduTime()
	{
		while (true)
		{
			if (isShowTime)
			{
				text2.text = RemainingTimr().ToString() + " S";
			}
			yield return new WaitForSeconds(1f);
		}
	}

	private int RemainingDays()
	{
		switch (DateTime.Now.DayOfWeek.ToString())
		{
		case "Monday":
			return 1;
		case "Tuesday":
			return 2;
		case "Wednesday":
			return 3;
		case "Thursday":
			return 4;
		case "Friday":
			return 5;
		case "Saturday":
			return 6;
		case "Sunday":
			return 7;
		default:
			return 7;
		}
	}

	private string RemainingTimr()
	{
		nowTime = DateTime.Now;
		endTime = GetWeeHours(DateTime.Now);
		TimeSpan timeSpan = endTime.Subtract(nowTime);
		return timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds;
	}

	private DateTime GetWeeHours(DateTime nowTime)
	{
		int num = nowTime.Day + 1;
		int num2 = nowTime.Year;
		int num3 = nowTime.Month;
		int dayOfMonth = GetDayOfMonth(num, num2);
		if (num > dayOfMonth)
		{
			num = 1;
			num3++;
			if (num3 > 12)
			{
				num3 = 1;
				num2++;
			}
		}
		string value = $"{num3}/{num}/{num2} 12:00:00 AM";
		return Convert.ToDateTime(value);
	}

	private int GetDayOfMonth(int month, int year)
	{
		switch (month)
		{
		case 1:
		case 3:
		case 5:
		case 7:
		case 8:
		case 10:
		case 12:
			return 31;
		case 2:
			return (!IsLeapYear(year)) ? 28 : 29;
		case 4:
		case 6:
		case 9:
		case 11:
			return 30;
		default:
			return 30;
		}
	}

	private bool IsLeapYear(int year)
	{
		if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
		{
			return true;
		}
		return false;
	}
}
