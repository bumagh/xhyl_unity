using UnityEngine;

public class STTF_LockingFlag : MonoBehaviour
{
	private bool bCountTime;

	private float mTime;

	public void HideLockingFlag()
	{
		if (!bCountTime)
		{
			mTime = 0f;
			bCountTime = true;
		}
	}

	private void Update()
	{
		if (bCountTime)
		{
			mTime += Time.deltaTime;
			if (mTime >= 0.5f)
			{
				bCountTime = false;
				mTime = 0f;
				base.gameObject.SetActive(value: false);
			}
		}
	}
}
