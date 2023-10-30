using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WGM.LLD
{
	public class JackpotManger : MonoBehaviour
	{
		public List<Text> Jakpottextlist;

		private int radombnum;

		private long timenum = 66000L;

		private long maxlong = 66000000L;

		private Sequence seq;

		private void OnEnable()
		{
			try
			{
				timenum = long.Parse(PlayerPrefs.GetString("timenum", 66000.ToString()));
			}
			catch (Exception)
			{
				timenum = 66000L;
			}
			StopCoroutine("WaitSettime");
			StartCoroutine(WaitSettime());
		}

		private void Awake()
		{
		}

		private IEnumerator WaitSettime()
		{
			while (true)
			{
				radombnum = UnityEngine.Random.Range(1024, 102400);
				Jakpottextlist[0].text = radombnum.ToString();
				long nowbun = timenum;
				if (timenum < maxlong)
				{
					timenum += UnityEngine.Random.Range(20, 30);
					Jakpottextlist[1].Animatornum(nowbun, timenum, 1f);
				}
				else
				{
					timenum = 66000L;
				}
				yield return new WaitForSeconds(1f);
			}
		}

		private void OnDisable()
		{
			PlayerPrefs.SetString("timenum", timenum.ToString());
		}

		private void Settime()
		{
			radombnum = UnityEngine.Random.Range(666, 102400);
			Jakpottextlist[0].text = radombnum + string.Empty;
			long from = timenum;
			if (timenum < maxlong)
			{
				timenum += 19L;
				Jakpottextlist[1].Animatornum(from, timenum, 0.98f);
			}
		}
	}
}
