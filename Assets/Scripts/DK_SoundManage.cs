using GameConfig;
using UnityEngine;

public class DK_SoundManage : MonoBehaviour
{
	private static DK_SoundManage instance;

	private DK_GameInfo mGameInfo;

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

	public static DK_SoundManage getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		setBgMusic(isPlay: true);
		mGameInfo = DK_GameInfo.getInstance();
	}

	private void Update()
	{
		if (!DK_TipManager.getInstance().isSomethingError && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (mGameInfo.currentState == DK_GameState.On_SelectRoom || mGameInfo.currentState == DK_GameState.On_SelectTable)
			{
				mGameInfo.UIScene.ClickBtnBack();
			}
			else if (mGameInfo.currentState == DK_GameState.On_Game)
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
			if (DK_GameInfo.getInstance().currentState == DK_GameState.On_Game)
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

	public void playButtonMusic(DK_ButtonMusicType type)
	{
		if (!mIsButtonMusic)
		{
			return;
		}
		switch (type)
		{
		case DK_ButtonMusicType.getCoin:
			mButtonClip = getCoinClip;
			break;
		case DK_ButtonMusicType.getTestCoin:
			mButtonClip = getTestCoinClip;
			break;
		case DK_ButtonMusicType.changeTable:
			mButtonClip = changTableClip;
			break;
		case DK_ButtonMusicType.error:
			mButtonClip = errorClip;
			break;
		case DK_ButtonMusicType.common:
			mButtonClip = commonClip;
			break;
		case DK_ButtonMusicType.common2:
			mButtonClip = newcommonClip;
			break;
		case DK_ButtonMusicType.addGun:
			mButtonClip = addGunClip;
			break;
		case DK_ButtonMusicType.coinUp:
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
			DK_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		}
		else
		{
			UnityEngine.Debug.LogError("===mGameInfo为空===");
		}
	}
}
