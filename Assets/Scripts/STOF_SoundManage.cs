using GameConfig;
using UnityEngine;

public class STOF_SoundManage : MonoBehaviour
{
	private static STOF_SoundManage instance;

	private STOF_GameInfo mGameInfo;

	public AudioSource bgMusicSource;

	public AudioClip bgMusicClip;

	public AudioSource buttonMusicSource;

	public AudioClip getCoinClip;

	public AudioClip getTestCoinClip;

	public AudioClip changTableClip;

	public AudioClip errorClip;

	public AudioClip commonClip;

	public AudioClip newcommonClip;

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

	public static STOF_SoundManage getInstance()
	{
		return instance;
	}

	private void Start()
	{
		instance = this;
		setBgMusic(isPlay: true);
		mGameInfo = STOF_GameInfo.getInstance();
	}

	private void Update()
	{
		if (!STOF_TipManager.getInstance().isSomethingError && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (mGameInfo.currentState == STOF_GameState.On_SelectRoom || mGameInfo.currentState == STOF_GameState.On_SelectTable)
			{
				mGameInfo.UIScene.ClickBtnBack();
			}
			else if (mGameInfo.currentState == STOF_GameState.On_Game)
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

	public void playButtonMusic(STOF_ButtonMusicType type)
	{
		if (!mIsButtonMusic)
		{
			return;
		}
		switch (type)
		{
		case STOF_ButtonMusicType.getCoin:
			mButtonClip = getCoinClip;
			break;
		case STOF_ButtonMusicType.getTestCoin:
			mButtonClip = getTestCoinClip;
			break;
		case STOF_ButtonMusicType.changeTable:
			mButtonClip = changTableClip;
			break;
		case STOF_ButtonMusicType.error:
			mButtonClip = errorClip;
			break;
		case STOF_ButtonMusicType.common:
			mButtonClip = commonClip;
			break;
		case STOF_ButtonMusicType.common2:
			mButtonClip = newcommonClip;
			break;
		case STOF_ButtonMusicType.addGun:
			mButtonClip = addGunClip;
			break;
		case STOF_ButtonMusicType.coinUp:
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
