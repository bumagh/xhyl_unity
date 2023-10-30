using GameConfig;
using UnityEngine;

public class STMF_SoundManage : MonoBehaviour
{
	private static STMF_SoundManage instance;

	private STMF_GameInfo mGameInfo;

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

	public static STMF_SoundManage getInstance()
	{
		return instance;
	}

	private void Start()
	{
		instance = this;
		setBgMusic(isPlay: true);
		mGameInfo = STMF_GameInfo.getInstance();
	}

	private void Update()
	{
		if (!STMF_TipManager.getInstance().isSomethingError && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (mGameInfo.currentState == STMF_GameState.On_SelectRoom || mGameInfo.currentState == STMF_GameState.On_SelectSeat || mGameInfo.currentState == STMF_GameState.On_SelectTable)
			{
				mGameInfo.UIScene.ClickBtnBack();
			}
			else if (mGameInfo.currentState == STMF_GameState.On_Game)
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

	public void playButtonMusic(STMF_ButtonMusicType type)
	{
		if (!mIsButtonMusic)
		{
			return;
		}
		switch (type)
		{
		case STMF_ButtonMusicType.getCoin:
			mButtonClip = getCoinClip;
			break;
		case STMF_ButtonMusicType.getTestCoin:
			mButtonClip = getTestCoinClip;
			break;
		case STMF_ButtonMusicType.changeTable:
			mButtonClip = changTableClip;
			break;
		case STMF_ButtonMusicType.error:
			mButtonClip = errorClip;
			break;
		case STMF_ButtonMusicType.common:
			mButtonClip = commonClip;
			break;
		case STMF_ButtonMusicType.common2:
			mButtonClip = newcommonClip;
			break;
		case STMF_ButtonMusicType.addGun:
			mButtonClip = addGunClip;
			break;
		case STMF_ButtonMusicType.coinUp:
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
}
