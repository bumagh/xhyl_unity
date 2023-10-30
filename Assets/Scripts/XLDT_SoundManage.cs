using STDT_GameConfig;
using UnityEngine;

public class XLDT_SoundManage : MonoBehaviour
{
	private static XLDT_SoundManage instance;

	private AudioSource[] mGameMusic = new AudioSource[10];

	private AudioSource[] mbuttonMusic = new AudioSource[10];

	private XLDT_GameInfo mGameInfo;

	public AudioSource bgGameMS;

	public AudioSource publicSource;

	public AudioClip bgMusicClip;

	public AudioClip[] bgGameMusicClip;

	public AudioClip getCoinClip;

	public AudioClip getTestCoinClip;

	public AudioClip changTableClip;

	public AudioClip errorClip;

	public AudioClip commonClip;

	public AudioClip coinUpClip;

	public AudioClip bonusClip;

	public AudioClip counttimeClip;

	public AudioClip countime0Clip;

	public AudioClip YaZhuClip;

	public AudioClip[] numClip;

	public AudioClip[] colorClip;

	public AudioClip startYZClip;

	public AudioClip stopYZClip;

	private AudioClip mButtonClip;

	private bool mIsButtonMusic;

	private bool mIsGameMusic;

	public bool IsButtonMusic
	{
		set
		{
			mIsButtonMusic = value;
			if (!mIsButtonMusic)
			{
				for (int i = 0; i < 10; i++)
				{
					mbuttonMusic[i].Stop();
				}
			}
		}
	}

	public bool IsGameMusic
	{
		set
		{
			if (!mIsGameMusic)
			{
				mIsGameMusic = value;
				if (mIsGameMusic)
				{
					if (bgGameMS.clip != null)
					{
						bgGameMS.volume = 1f;
						bgGameMS.Play();
						bgGameMS.loop = false;
					}
					else
					{
						int num = UnityEngine.Random.Range(0, 6);
						bgGameMS.clip = bgGameMusicClip[num];
						bgGameMS.volume = 1f;
						bgGameMS.Play();
						bgGameMS.loop = false;
					}
					if (XLDT_GameInfo.getInstance().CurAward.awardType == XLDT_EAwardType.ColorPublic || XLDT_GameInfo.getInstance().CurAward.awardType == XLDT_EAwardType.FlowerPublic)
					{
						publicSource.Play();
						bgGameMS.volume = 0f;
					}
				}
				return;
			}
			mIsGameMusic = value;
			if (!mIsGameMusic)
			{
				publicSource.Stop();
				bgGameMS.Stop();
				for (int i = 0; i < 10; i++)
				{
					mGameMusic[i].Stop();
				}
			}
		}
	}

	public static XLDT_SoundManage getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		mGameInfo = XLDT_GameInfo.getInstance();
		Transform transform = base.transform.Find("GameMusic");
		Transform transform2 = base.transform.Find("ButtonMusic");
		for (int i = 0; i < 10; i++)
		{
			mGameMusic[i] = transform.GetChild(i).GetComponent<AudioSource>();
			mbuttonMusic[i] = transform2.GetChild(i).GetComponent<AudioSource>();
		}
	}

	private void Update()
	{
		if (mIsGameMusic && (mGameInfo.currentState == XLDT_GameState.On_Game || mGameInfo.currentState == XLDT_GameState.On_EnterGame) && !bgGameMS.isPlaying)
		{
			int num = UnityEngine.Random.Range(0, 6);
			bgGameMS.clip = bgGameMusicClip[num];
			bgGameMS.volume = 1f;
			bgGameMS.loop = false;
			bgGameMS.Play();
		}
	}

	public void PlayPublicMusic(bool isPlay)
	{
		if (isPlay && mIsGameMusic)
		{
			publicSource.loop = true;
			publicSource.volume = 1f;
			bgGameMS.volume = 0f;
			publicSource.Play();
		}
		else
		{
			bgGameMS.volume = 1f;
			publicSource.Stop();
		}
	}

	public void setGameBgMusic(bool isPlay)
	{
		if (mGameInfo.currentState == XLDT_GameState.On_Game || mGameInfo.currentState == XLDT_GameState.On_EnterGame)
		{
			if (isPlay && mIsGameMusic)
			{
				int num = UnityEngine.Random.Range(0, 6);
				if (bgGameMS.isPlaying)
				{
					bgGameMS.Stop();
				}
				bgGameMS.loop = false;
				bgGameMS.clip = bgGameMusicClip[num];
				bgGameMS.volume = 1f;
				bgGameMS.Play();
			}
			else
			{
				bgGameMS.Stop();
			}
			return;
		}
		publicSource.Stop();
		for (int i = 0; i < 10; i++)
		{
			mGameMusic[i].Stop();
		}
		if (isPlay && mIsGameMusic)
		{
			if (bgGameMS.isPlaying)
			{
				bgGameMS.Stop();
			}
			bgGameMS.loop = true;
			bgGameMS.clip = bgMusicClip;
			bgGameMS.volume = 1f;
			bgGameMS.Play();
		}
		else
		{
			bgGameMS.Stop();
		}
	}

	public void playButtonMusic(XLDT_ButtonMusicType type, int num = 0)
	{
		if (num < 0 || num > 14)
		{
			return;
		}
		switch (type)
		{
		case XLDT_ButtonMusicType.getCoin:
			mButtonClip = getCoinClip;
			break;
		case XLDT_ButtonMusicType.getTestCoin:
			mButtonClip = getTestCoinClip;
			break;
		case XLDT_ButtonMusicType.changeTable:
			mButtonClip = changTableClip;
			break;
		case XLDT_ButtonMusicType.error:
			mButtonClip = errorClip;
			break;
		case XLDT_ButtonMusicType.common:
			mButtonClip = commonClip;
			break;
		case XLDT_ButtonMusicType.coinUp:
			mButtonClip = coinUpClip;
			break;
		case XLDT_ButtonMusicType.YaZhu:
			mButtonClip = YaZhuClip;
			break;
		case XLDT_ButtonMusicType.bonus:
			mButtonClip = bonusClip;
			break;
		case XLDT_ButtonMusicType.counttime:
			mButtonClip = counttimeClip;
			break;
		case XLDT_ButtonMusicType.counttime0:
			mButtonClip = countime0Clip;
			break;
		case XLDT_ButtonMusicType.publiccard:
			mButtonClip = null;
			break;
		case XLDT_ButtonMusicType.awardNum:
			mButtonClip = numClip[num];
			break;
		case XLDT_ButtonMusicType.awardColor:
			mButtonClip = colorClip[num];
			break;
		case XLDT_ButtonMusicType.startYZ:
			mButtonClip = startYZClip;
			break;
		case XLDT_ButtonMusicType.stopYZ:
			mButtonClip = stopYZClip;
			break;
		default:
			mButtonClip = null;
			break;
		}
		if ((!mIsButtonMusic && type < XLDT_ButtonMusicType.bonus) || (!mIsGameMusic && type >= XLDT_ButtonMusicType.bonus) || !(mButtonClip != null))
		{
			return;
		}
		int num2 = 0;
		while (true)
		{
			if (num2 >= 10)
			{
				return;
			}
			if (type < XLDT_ButtonMusicType.bonus)
			{
				if (!mbuttonMusic[num2].isPlaying)
				{
					mbuttonMusic[num2].clip = mButtonClip;
					mbuttonMusic[num2].loop = false;
					mbuttonMusic[num2].volume = 1f;
					mbuttonMusic[num2].Play();
					return;
				}
			}
			else if (type >= XLDT_ButtonMusicType.bonus && !mGameMusic[num2].isPlaying)
			{
				break;
			}
			num2++;
		}
		mGameMusic[num2].clip = mButtonClip;
		mGameMusic[num2].loop = false;
		mGameMusic[num2].volume = 1f;
		mGameMusic[num2].Play();
	}
}
