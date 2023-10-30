using GameConfig;
using UnityEngine;

public class STTF_SoundManage : MonoBehaviour
{
	private static STTF_SoundManage instance;

	private STTF_GameInfo mGameInfo;

	public AudioSource bgMusicSource;

	public AudioClip bgMusicClip;

	public AudioClip acGameMusicClip;

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

	public static STTF_SoundManage getInstance()
	{
		return instance;
	}

	private void Start()
	{
		instance = this;
		setBgMusic(isPlay: true);
		mGameInfo = STTF_GameInfo.getInstance();
	}

	private void Update()
	{
		if (!STTF_TipManager.getInstance().isSomethingError && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (mGameInfo.currentState == STTF_GameState.On_SelectRoom || mGameInfo.currentState == STTF_GameState.On_SelectTable)
			{
				mGameInfo.UIScene.ClickBtnBack();
			}
			else if (mGameInfo.currentState == STTF_GameState.On_Game)
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
			if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_Game)
			{
				bgMusicSource.clip = acGameMusicClip;
			}
			else
			{
				bgMusicSource.clip = bgMusicClip;
			}
			bgMusicSource.volume = 1f;
			bgMusicSource.Play();
		}
		else
		{
			bgMusicSource.Stop();
		}
	}

	public void playButtonMusic(STTF_ButtonMusicType type)
	{
		if (!mIsButtonMusic)
		{
			return;
		}
		switch (type)
		{
		case STTF_ButtonMusicType.getCoin:
			mButtonClip = getCoinClip;
			break;
		case STTF_ButtonMusicType.getTestCoin:
			mButtonClip = getTestCoinClip;
			break;
		case STTF_ButtonMusicType.changeTable:
			mButtonClip = changTableClip;
			break;
		case STTF_ButtonMusicType.error:
			mButtonClip = errorClip;
			break;
		case STTF_ButtonMusicType.common:
			mButtonClip = commonClip;
			break;
		case STTF_ButtonMusicType.common2:
			mButtonClip = newcommonClip;
			break;
		case STTF_ButtonMusicType.addGun:
			mButtonClip = addGunClip;
			break;
		case STTF_ButtonMusicType.coinUp:
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

	private void OnApplicationQuit()
	{
		if (mGameInfo != null)
		{
			mGameInfo.ClearGameInfo();
			STTF_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		}
	}
}
