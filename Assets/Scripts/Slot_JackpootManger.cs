using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_JackpootManger : MonoBehaviour
{
	public List<Text> Jakpottextlist;

	private long radombnum = 676909L;

	private long timenum = 68971732L;

	private long maxlong = 100000000L;

	private Sequence seq;

	private Coroutine waitSettime;

	private void OnEnable()
	{
		if (waitSettime != null)
		{
			StopCoroutine(waitSettime);
		}
		waitSettime = StartCoroutine(WaitSettime());
	}

	private IEnumerator WaitSettime()
	{
		while (true)
		{
			long nowradom = radombnum;
			long nowbun = timenum;
			int temp = UnityEngine.Random.Range(0, 4);
			if (temp == 1)
			{
				if (radombnum < maxlong)
				{
					radombnum += UnityEngine.Random.Range(10, 20);
					Jakpottextlist[0].Animatornum(nowradom, radombnum, 1f);
				}
				else
				{
					radombnum = 676909L;
				}
			}
			if (timenum < maxlong)
			{
				timenum += 10L;
				Jakpottextlist[1].Animatornum(nowbun, timenum, 1f);
			}
			else
			{
				timenum = 68971732L;
			}
			yield return new WaitForSeconds(1f);
		}
	}
}
