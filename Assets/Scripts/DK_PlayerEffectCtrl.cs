using GameCommon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DK_PlayerEffectCtrl : MonoBehaviour
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

	private Text txtScore;

	private float _showTimePlate = 5f;

	private float _maxShowTimePlate = 5f;

	private Queue<int> _prizePlateQue = new Queue<int>();

	private GameObject _curPrizePlateObj;

	private float _fLiziTime;

	private Vector3 vecScale = Vector3.one;

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
		txtScore = _Score.GetComponent<Text>();
	}

	public void ShowCoin(DK_FISH_TYPE fishType, Vector3 pos)
	{
		if (fishType < DK_FISH_TYPE.Fish_Turtle || (fishType < DK_FISH_TYPE.Fish_Same_Turtle && fishType >= DK_FISH_TYPE.Fish_Same_Shrimp))
		{
			_showSilver(DK_EffectMngr.GetSingleton().GetFlyCoinNum(fishType), pos);
			DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_SILVER_COIN);
		}
		else
		{
			_showGold(DK_EffectMngr.GetSingleton().GetFlyCoinNum(fishType), pos);
			DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_GOLD_COIN);
		}
	}

	public void ShowCoin(DK_FISH_TYPE fishType, Vector3 pos, int bet = -1)
	{
		if (bet != -1)
		{
			if (bet > 10)
			{
				int num = bet / 5;
				if (num >= 10)
				{
					num = 10;
				}
				_showGold(num, pos);
				DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_GOLD_COIN);
			}
		}
		else if (fishType < DK_FISH_TYPE.Fish_Turtle || (fishType < DK_FISH_TYPE.Fish_Same_Turtle && fishType >= DK_FISH_TYPE.Fish_Same_Shrimp))
		{
			_showSilver(DK_EffectMngr.GetSingleton().GetFlyCoinNum(fishType), pos);
			DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_SILVER_COIN);
		}
		else
		{
			_showGold(DK_EffectMngr.GetSingleton().GetFlyCoinNum(fishType), pos);
			DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_GOLD_COIN);
		}
	}

	public void ShowLiziCard(Vector3 pos)
	{
		Transform transform = DK_EffectMngr.GetSingleton().CreateEffectObj(_Lizi.transform);
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
		transform.GetComponent<DK_FlyObj>().AnimatedFlyTo(_playerPos);
		_fLiziTime += 20f;
		DK_GameInfo.getInstance().GameScene.PlayEnergyGun(mPlayerIndex, isPlay: true);
	}

	public void ShowFishScore(Vector3 pos, int nScore)
	{
		if (nScore > 0)
		{
			Transform transform = DK_EffectMngr.GetSingleton().CreateEffectObj(_Score.transform);
			transform.localScale = vecScale * 0.8f;
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
			transform.GetComponent<DK_FlyObj>().AnimatedShow(_playerPos);
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
			Transform transform = DK_EffectMngr.GetSingleton().CreateEffectObj(objCoinGold.transform);
			float d = -1f;
			Vector3 vector = pos - _playerPos;
			if (vector.y > 0f)
			{
				d = 1f;
			}
			transform.position = pos + 0.8f * Vector3.left * nCoinNum / 2f + Vector3.right * i * 0.8f + Vector3.up * i * 0.1f * d;
			transform.GetComponent<DK_FlyObj>().FlyTo(_playerPos);
		}
	}

	private void _showSilver(int nCoinNum, Vector3 pos)
	{
		for (int i = 0; i < nCoinNum; i++)
		{
			Transform transform = DK_EffectMngr.GetSingleton().CreateEffectObj(objCoinSilver.transform);
			transform.position = pos + 0.5f * Vector3.left * nCoinNum / 2f + Vector3.right * i * 0.5f + Vector3.up * i * 0.1f;
			transform.GetComponent<DK_FlyObj>().FlyTo(_playerPos);
		}
	}

	private void _updateBigPrizePlate()
	{
		_showTimePlate += Time.deltaTime;
		if (_prizePlateQue.Count >= 1 && _showTimePlate >= 1f)
		{
			if (_curPrizePlateObj != null)
			{
				_curPrizePlateObj.GetComponent<DK_PrizePlate>().DestroyObj();
			}
			int nNum = _prizePlateQue.Dequeue();
			_curPrizePlateObj = DK_EffectMngr.GetSingleton().CreateEffectObj(_prizePlate.transform).gameObject;
			_curPrizePlateObj.GetComponent<DK_PrizePlate>().ShowScore(nNum, _playerPos, mPlayerIndex);
			_showTimePlate = 0f;
			DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_SCORE_PANEL);
		}
	}

	private void _updateLiziBarBette()
	{
		if (_fLiziTime > 0f)
		{
			if (DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
			{
				_fLiziTime -= Time.deltaTime;
			}
			if (_fLiziTime <= 0f)
			{
				DK_GameInfo.getInstance().GameScene.PlayEnergyGun(mPlayerIndex, isPlay: false);
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
