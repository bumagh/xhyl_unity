using UnityEngine;

public class DP_SoundManager : MonoBehaviour
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

	protected static DP_SoundManager gSingleton;

	public AudioSource mBackgroundSource;

	public AudioClip mGameHallClip;

	public AudioClip acGameBGM;

	public AudioSource mButtonSource;

	public AudioClip mButtonClip;

	public AudioClip mBetFailClip;

	public AudioClip mBetSuccessClip;

	public AudioClip mGivingCoinsClip;

	public AudioClip mCoinInClip;

	public AudioClip mChangeTableClip;

	protected float mButtonVolume = 1f;

	protected float mMusicVolume = 1f;

	public static DP_SoundManager GetSingleton()
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
			mBackgroundSource.volume = mMusicVolume;
			mBackgroundSource.Play();
		}
		else if (mBackgroundSource == null)
		{
			DP_ErrorManager.GetSingleton().AddError("game hall background source null");
		}
		else
		{
			DP_ErrorManager.GetSingleton().AddError("game hall background clip null");
		}
	}

	public void PlayGameMusic()
	{
		if ((bool)mBackgroundSource && (bool)acGameBGM)
		{
			if (mBackgroundSource.isPlaying)
			{
				mBackgroundSource.Stop();
			}
			mBackgroundSource.loop = true;
			mBackgroundSource.clip = acGameBGM;
			mBackgroundSource.volume = mMusicVolume;
			mBackgroundSource.Play();
		}
		else
		{
			DP_ErrorManager.GetSingleton().AddError("game hall background source null");
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
			mButtonSource.volume = mButtonVolume;
			mButtonSource.Play();
		}
		else if (mButtonSource == null)
		{
			DP_ErrorManager.GetSingleton().AddError("button source null");
		}
		else
		{
			DP_ErrorManager.GetSingleton().AddError("button clip null");
		}
	}

	public void SetMusicVolume(float fVolume)
	{
		if (fVolume > 1f || fVolume < 0f)
		{
			DP_ErrorManager.GetSingleton().AddError("Error:音量设置错误" + fVolume);
			fVolume = 0.5f;
		}
		mMusicVolume = fVolume;
		mBackgroundSource.volume = fVolume;
	}

	public void SetSoundVolume(float fVolume)
	{
		if (fVolume > 1f || fVolume < 0f)
		{
			DP_ErrorManager.GetSingleton().AddError("Error:音量设置错误" + fVolume);
			fVolume = 0.5f;
		}
		mButtonVolume = fVolume;
		mButtonSource.volume = fVolume;
	}
}
