using GameCommon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STMF_PlayerEffectCtrl : MonoBehaviour
{
	public int mPlayerIndex;

	private Vector3 _playerPos;

	[SerializeField]
	private GameObject objCoinSilver;

	[SerializeField]
	private GameObject objCoinGold;

	[SerializeField]
	private GameObject _Score;

	[SerializeField]
	private GameObject _Lizi;

	[SerializeField]
	private GameObject _prizePlate;

	private SpriteRenderer srCoin;

	private SpriteRenderer srLizi;

	private Text txtScore;

	private Canvas cvScore;

	private float _showTimePlate = 5f;

	private float _maxShowTimePlate = 5f;

	private Queue<int> _prizePlateQue = new Queue<int>();

	private GameObject _curPrizePlateObj;

	private float _fLiziTime;

	private Vector3 vecScale = new Vector3(0.56f, 0.56f, 1f);

	private void Awake()
	{
		if (mPlayerIndex == 1)
		{
			_playerPos = Vector3.left * 3.8f + Vector3.down * 3.6f + Vector3.forward;
		}
		else if (mPlayerIndex == 2)
		{
			_playerPos = Vector3.right * 3.2f + Vector3.down * 3.6f + Vector3.forward;
		}
		else if (mPlayerIndex == 3)
		{
			_playerPos = Vector3.right * 3.8f + Vector3.up * 3.6f + Vector3.forward;
		}
		else
		{
			_playerPos = Vector3.left * 3.2f + Vector3.up * 3.6f + Vector3.forward;
		}
		srCoin = objCoinGold.GetComponent<SpriteRenderer>();
		srCoin.sortingOrder = 644;
		txtScore = _Score.GetComponent<Text>();
		cvScore = _Score.GetComponent<Canvas>();
		cvScore.sortingOrder = 645;
		srLizi = _Lizi.GetComponent<SpriteRenderer>();
		srLizi.sortingOrder = 644;
	}

	public void ShowCoin(STMF_FISH_TYPE fishType, Vector3 pos)
	{
		if (fishType < STMF_FISH_TYPE.Fish_Turtle || (fishType < STMF_FISH_TYPE.Fish_Same_Turtle && fishType >= STMF_FISH_TYPE.Fish_Same_Shrimp))
		{
			_showSilver(STMF_EffectMngr.GetSingleton().GetFlyCoinNum(fishType), pos);
			STMF_MusicMngr.GetSingleton().PlayGameSound(STMF_MusicMngr.GAME_SOUND.SOUND_SILVER_COIN);
		}
		else
		{
			_showGold(STMF_EffectMngr.GetSingleton().GetFlyCoinNum(fishType), pos);
			STMF_MusicMngr.GetSingleton().PlayGameSound(STMF_MusicMngr.GAME_SOUND.SOUND_GOLD_COIN);
		}
	}

	public void ShowLiziCard(Vector3 pos)
	{
		Transform transform = STMF_EffectMngr.GetSingleton().CreateObj(_Lizi.transform);
		if (pos.x < -6f)
		{
			pos = Vector3.right * -6f + Vector3.up * pos.y + Vector3.forward * pos.z;
		}
		if (pos.x > 6f)
		{
			pos = Vector3.right * 6f + Vector3.up * pos.y + Vector3.forward * pos.z;
		}
		if (pos.y > 2f)
		{
			pos = Vector3.right * pos.x + Vector3.up * 2f + Vector3.forward * pos.z;
		}
		if (pos.y < -2f)
		{
			pos = Vector3.right * pos.x + Vector3.up * -2f + Vector3.forward * pos.z;
		}
		transform.position = pos;
		transform.GetComponent<STMF_FlyObj>().AnimatedFlyTo(_playerPos);
		_fLiziTime += 20f;
		STMF_GameInfo.getInstance().GameScene.PlayEnergyGun(mPlayerIndex, isPlay: true);
	}

	public void ShowFishScore(Vector3 pos, int nScore)
	{
		if (nScore > 0)
		{
			Transform transform = STMF_EffectMngr.GetSingleton().CreateUIObj(_Score.transform);
			transform.localScale = vecScale;
			if (pos.x < -6f)
			{
				pos = Vector3.right * -6f + Vector3.up * pos.y + Vector3.forward * pos.z;
			}
			if (pos.x > 6f)
			{
				pos = Vector3.right * 6f + Vector3.up * pos.y + Vector3.forward * pos.z;
			}
			if (pos.y > 2f)
			{
				pos = Vector3.right * pos.x + Vector3.up * 2f + Vector3.forward * pos.z;
			}
			if (pos.y < -2f)
			{
				pos = Vector3.right * pos.x + Vector3.up * -2f + Vector3.forward * pos.z;
			}
			transform.position = pos;
			transform.GetComponent<Text>().text = nScore.ToString();
			transform.GetComponent<STMF_FlyObj>().AnimatedShow(_playerPos);
		}
	}

	public void ShowPrizePlate(int nScore)
	{
		_prizePlateQue.Enqueue(nScore);
	}

	private void _showGold(int nCoinNum, Vector3 pos)
	{
		for (int i = 0; i < nCoinNum; i++)
		{
			Transform transform = STMF_EffectMngr.GetSingleton().CreateObj(objCoinGold.transform);
			float d = -1f;
			Vector3 vector = pos - _playerPos;
			if (vector.y > 0f)
			{
				d = 1f;
			}
			transform.position = pos + 0.8f * Vector3.left * nCoinNum / 2f + Vector3.right * i * 0.8f + Vector3.up * i * 0.1f * d;
			transform.GetComponent<STMF_FlyObj>().FlyTo(_playerPos);
		}
	}

	private void _showSilver(int nCoinNum, Vector3 pos)
	{
		for (int i = 0; i < nCoinNum; i++)
		{
			Transform transform = STMF_EffectMngr.GetSingleton().CreateObj(objCoinSilver.transform);
			transform.position = pos + 0.5f * Vector3.left * nCoinNum / 2f + Vector3.right * i * 0.5f + Vector3.up * i * 0.1f;
			transform.GetComponent<STMF_FlyObj>().FlyTo(_playerPos);
		}
	}

	private void _updateBigPrizePlate()
	{
		_showTimePlate += Time.deltaTime;
		if (_prizePlateQue.Count >= 1 && _showTimePlate >= 1f)
		{
			if (_curPrizePlateObj != null)
			{
				_curPrizePlateObj.GetComponent<STMF_PrizePlate>().DestroyObj();
			}
			int nNum = _prizePlateQue.Dequeue();
			_curPrizePlateObj = STMF_EffectMngr.GetSingleton().CreateUIObj(_prizePlate.transform).gameObject;
			_curPrizePlateObj.GetComponent<STMF_PrizePlate>().ShowScore(nNum, _playerPos, mPlayerIndex);
			_showTimePlate = 0f;
			STMF_MusicMngr.GetSingleton().PlayGameSound(STMF_MusicMngr.GAME_SOUND.SOUND_SCORE_PANEL);
		}
	}

	private void _updateLiziBarBette()
	{
		if (_fLiziTime > 0f)
		{
			if (STMF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
			{
				_fLiziTime -= Time.deltaTime;
			}
			if (_fLiziTime <= 0f)
			{
				STMF_GameInfo.getInstance().GameScene.PlayEnergyGun(mPlayerIndex, isPlay: false);
			}
		}
		else
		{
			_fLiziTime = 0f;
		}
	}

	public void SetLiziReset()
	{
		_fLiziTime = 0f;
	}

	private void Update()
	{
		_updateBigPrizePlate();
		_updateLiziBarBette();
	}
}
