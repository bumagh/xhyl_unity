using System;
using System.Collections;
using UnityEngine;

public class LL_LuckyRibbonAnimScript : MonoBehaviour
{
	public GameObject mLuckyPrizeShine;

	public GameObject[] mParticleChild = new GameObject[5];

	private void Start()
	{
		mParticleChild[0] = mLuckyPrizeShine.transform.Find("holyFlash").gameObject;
		int num = 1;
		IEnumerator enumerator = mParticleChild[0].transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				mParticleChild[num++] = transform.gameObject;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	private void Update()
	{
	}

	public void OnAnimationEnd()
	{
		LL_AppUIMngr.GetSingleton().mHudManager.ShowLuckyMachineAnimation();
		if (mLuckyPrizeShine != null)
		{
			mLuckyPrizeShine.SetActive(value: true);
		}
		for (int i = 0; i < mParticleChild.Length; i++)
		{
			mParticleChild[i].GetComponent<ParticleSystem>().Stop();
			mParticleChild[i].GetComponent<ParticleSystem>().enableEmission = true;
			mParticleChild[i].GetComponent<ParticleSystem>().Play();
		}
	}
}
