using UnityEngine;

public class JSYS_LL_LuckyLion_SoundManager : MonoBehaviour
{
	public enum EUIBtnSoundType
	{
		BtnSound,
		BetFail,
		BetSuccess,
		GivingCoins,
		CoinIn,
		ChangeTable
	}

	protected static JSYS_LL_LuckyLion_SoundManager gSingleton;

	public AudioSource mBackgroundSource;

	public AudioClip mGameHallClip;

	public AudioSource mButtonSource;

	public AudioClip mButtonClip;

	public AudioClip mBetFailClip;

	public AudioClip mBetSuccessClip;

	public AudioClip mGivingCoinsClip;

	public AudioClip mCoinInClip;

	public AudioClip mChangeTableClip;

	protected float mButtonVolume = 1f;

	protected float mMusicVolume = 1f;

	public static JSYS_LL_LuckyLion_SoundManager GetSingleton()
	{
		return gSingleton;
	}

	private void Awake()
	{
		if (gSingleton == null)
		{
			gSingleton = this;
		}
	}

	private void Start()
	{
		PlayGameHallMusic();
	}

	private void Update()
	{
	}

	public void PlayGameHallMusic()
	{
		if ((bool)mBackgroundSource && (bool)mGameHallClip)
		{
			if (mBackgroundSource.isPlaying)
			{
				mBackgroundSource.Stop();
			}
			mBackgroundSource.loop = true;
			mBackgroundSource.clip = mGameHallClip;
			mBackgroundSource.volume = PlayerPrefs.GetFloat("setBGVolume");
			mBackgroundSource.Play();
		}
		else if (mBackgroundSource == null)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("game hall background source null");
		}
		else
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("game hall background clip null");
		}
	}

	public void StopGameHallMusic()
	{
		if ((bool)mBackgroundSource)
		{
			mBackgroundSource.Stop();
		}
		else
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("game hall background source null");
		}
	}

	public void playButtonSound(EUIBtnSoundType soundType = EUIBtnSoundType.BtnSound)
	{
		AudioClip audioClip = null;
		switch (soundType)
		{
		case EUIBtnSoundType.BtnSound:
			audioClip = mButtonClip;
			break;
		case EUIBtnSoundType.BetFail:
			audioClip = mBetFailClip;
			break;
		case EUIBtnSoundType.BetSuccess:
			audioClip = mBetSuccessClip;
			break;
		case EUIBtnSoundType.GivingCoins:
			audioClip = mGivingCoinsClip;
			break;
		case EUIBtnSoundType.CoinIn:
			audioClip = mCoinInClip;
			break;
		case EUIBtnSoundType.ChangeTable:
			audioClip = mChangeTableClip;
			break;
		}
		if (mButtonSource != null && audioClip != null)
		{
			if (mButtonSource.isPlaying)
			{
				mButtonSource.Stop();
			}
			mButtonSource.loop = false;
			mButtonSource.clip = audioClip;
			mButtonSource.volume = PlayerPrefs.GetFloat("setSoundVolume");
			mButtonSource.Play();
		}
		else if (mButtonSource == null)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("button source null");
		}
		else
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("button clip null");
		}
	}

	public void SetMusicVolume(float fVolume)
	{
		if (fVolume > 1f || fVolume < 0f)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:音量设置错误" + fVolume);
			fVolume = 0.5f;
		}
		mMusicVolume = fVolume;
		mBackgroundSource.volume = fVolume;
	}

	public void SetSoundVolume(float fVolume)
	{
		if (fVolume > 1f || fVolume < 0f)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:音量设置错误" + fVolume);
			fVolume = 0.5f;
		}
		mButtonVolume = fVolume;
		mButtonSource.volume = fVolume;
	}
}
