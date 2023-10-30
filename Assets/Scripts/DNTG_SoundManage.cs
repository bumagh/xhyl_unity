using GameConfig;
using UnityEngine;

public class DNTG_SoundManage : MonoBehaviour
{
	private static DNTG_SoundManage instance;

	private DNTG_GameInfo mGameInfo;

	public AudioSource bgMusicSource;

	public AudioClip bgMusicClip;

	public AudioSource buttonMusicSource;

	public AudioClip getCoinClip;

	public AudioClip getTestCoinClip;

	public AudioClip changTableClip;

	public AudioClip errorClip;

	public AudioClip commonClip;

	public AudioClip addGunClip;

	public AudioClip coinUpClip;

	private AudioClip mButtonClip;

	private bool mIsButtonMusic;

	public bool IsButtonMusic
	{
		set
		{
			mIsButtonMusic = value;
		}
	}

	public static DNTG_SoundManage getInstance()
	{
		return instance;
	}

	private void Start()
	{
		instance = this;
		setBgMusic(isPlay: true);
		mGameInfo = DNTG_GameInfo.getInstance();
	}

	private void Update()
	{
		if (!DNTG_TipManager.getInstance().isSomethingError && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (mGameInfo.currentState == DNTG_GameState.On_SelectRoom || mGameInfo.currentState == DNTG_GameState.On_SelectTable)
			{
				mGameInfo.UIScene.ClickBtnBack();
			}
			else if (mGameInfo.currentState == DNTG_GameState.On_Game)
			{
				mGameInfo.GameScene.ClickBtnBack();
			}
		}
	}

	public void setBgMusic(bool isPlay)
	{
		if (isPlay)
		{
			if (bgMusicSource.isPlaying)
			{
				bgMusicSource.Stop();
			}
			bgMusicSource.loop = true;
			bgMusicSource.clip = bgMusicClip;
			bgMusicSource.volume = 1f;
			bgMusicSource.Play();
		}
		else
		{
			bgMusicSource.Stop();
		}
	}

	public void playButtonMusic(DNTG_ButtonMusicType type)
	{
		if (!mIsButtonMusic)
		{
			return;
		}
		switch (type)
		{
		case DNTG_ButtonMusicType.getCoin:
			mButtonClip = getCoinClip;
			break;
		case DNTG_ButtonMusicType.getTestCoin:
			mButtonClip = getTestCoinClip;
			break;
		case DNTG_ButtonMusicType.changeTable:
			mButtonClip = changTableClip;
			break;
		case DNTG_ButtonMusicType.error:
			mButtonClip = errorClip;
			break;
		case DNTG_ButtonMusicType.common:
			mButtonClip = commonClip;
			break;
		case DNTG_ButtonMusicType.addGun:
			mButtonClip = addGunClip;
			break;
		case DNTG_ButtonMusicType.coinUp:
			mButtonClip = coinUpClip;
			break;
		default:
			mButtonClip = null;
			break;
		}
		if (mButtonClip != null)
		{
			if (buttonMusicSource.isPlaying)
			{
				buttonMusicSource.Stop();
			}
			buttonMusicSource.clip = mButtonClip;
			buttonMusicSource.loop = false;
			buttonMusicSource.volume = 1f;
			buttonMusicSource.Play();
		}
	}

	public void SoundManageDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
