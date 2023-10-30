using GameConfig;
using UnityEngine;

public class TF_SoundManage : MonoBehaviour
{
	private static TF_SoundManage instance;

	private TF_GameInfo mGameInfo;

	public AudioSource bgMusicSource;

	public AudioClip bgMusicClip;

	public AudioClip acGameMusicClip;

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

	public static TF_SoundManage getInstance()
	{
		return instance;
	}

	private void Start()
	{
		instance = this;
		setBgMusic(isPlay: true);
		mGameInfo = TF_GameInfo.getInstance();
	}

	private void Update()
	{
		if (!TF_TipManager.getInstance().isSomethingError && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (mGameInfo.currentState == TF_GameState.On_SelectRoom || mGameInfo.currentState == TF_GameState.On_SelectTable)
			{
				mGameInfo.UIScene.ClickBtnBack();
			}
			else if (mGameInfo.currentState == TF_GameState.On_Game)
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
			if (TF_GameInfo.getInstance().currentState == TF_GameState.On_Game)
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

	public void playButtonMusic(TF_ButtonMusicType type)
	{
		if (!mIsButtonMusic)
		{
			return;
		}
		switch (type)
		{
		case TF_ButtonMusicType.getCoin:
			mButtonClip = getCoinClip;
			break;
		case TF_ButtonMusicType.getTestCoin:
			mButtonClip = getTestCoinClip;
			break;
		case TF_ButtonMusicType.changeTable:
			mButtonClip = changTableClip;
			break;
		case TF_ButtonMusicType.error:
			mButtonClip = errorClip;
			break;
		case TF_ButtonMusicType.common:
			mButtonClip = commonClip;
			break;
		case TF_ButtonMusicType.addGun:
			mButtonClip = addGunClip;
			break;
		case TF_ButtonMusicType.coinUp:
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
