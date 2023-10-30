using GameCommon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_PlayerEffectCtrl : MonoBehaviour
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

	public void ShowCoin(DNTG_FISH_TYPE fishType, Vector3 pos)
	{
		if (fishType < DNTG_FISH_TYPE.Fish_Turtle || (fishType < DNTG_FISH_TYPE.Fish_FixBomb && fishType >= DNTG_FISH_TYPE.Fish_Same_Shrimp))
		{
			_showSilver(DNTG_EffectMngr.GetSingleton().GetFlyCoinNum(fishType), pos);
			DNTG_MusicMngr.GetSingleton().PlayGameSound(DNTG_MusicMngr.GAME_SOUND.SOUND_SILVER_COIN);
		}
		else
		{
			_showGold(DNTG_EffectMngr.GetSingleton().GetFlyCoinNum(fishType), pos);
			DNTG_MusicMngr.GetSingleton().PlayGameSound(DNTG_MusicMngr.GAME_SOUND.SOUND_GOLD_COIN);
		}
	}

	public void ShowLiziCard(Vector3 pos)
	{
	}

	public void ShowFishScore(Vector3 pos, int nScore)
	{
		if (nScore > 0)
		{
			Transform transform = DNTG_EffectMngr.GetSingleton().CreateEffectObj(_Score.transform);
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
			transform.GetComponent<DNTG_FlyObj>().AnimatedShow(_playerPos);
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
			Transform transform = DNTG_EffectMngr.GetSingleton().CreateEffectObj(objCoinGold.transform);
			float d = -1f;
			Vector3 vector = pos - _playerPos;
			if (vector.y > 0f)
			{
				d = 1f;
			}
			transform.position = pos + 0.8f * Vector3.left * nCoinNum / 2f + Vector3.right * i * 0.8f + Vector3.up * i * 0.1f * d;
			transform.GetComponent<DNTG_FlyObj>().FlyTo(_playerPos);
		}
	}

	private void _showSilver(int nCoinNum, Vector3 pos)
	{
		for (int i = 0; i < nCoinNum; i++)
		{
			Transform transform = DNTG_EffectMngr.GetSingleton().CreateEffectObj(objCoinSilver.transform);
			transform.position = pos + 0.5f * Vector3.left * nCoinNum / 2f + Vector3.right * i * 0.5f + Vector3.up * i * 0.1f;
			transform.GetComponent<DNTG_FlyObj>().FlyTo(_playerPos);
		}
	}

	private void _updateBigPrizePlate()
	{
		_showTimePlate += Time.deltaTime;
		if (_prizePlateQue.Count >= 1 && _showTimePlate >= 1f)
		{
			if (_curPrizePlateObj != null)
			{
				_curPrizePlateObj.GetComponent<DNTG_PrizePlate>().DestroyObj();
			}
			int nNum = _prizePlateQue.Dequeue();
			_curPrizePlateObj = DNTG_EffectMngr.GetSingleton().CreateEffectObj(_prizePlate.transform).gameObject;
			_curPrizePlateObj.GetComponent<DNTG_PrizePlate>().ShowScore(nNum, _playerPos, mPlayerIndex);
			_showTimePlate = 0f;
			DNTG_MusicMngr.GetSingleton().PlayGameSound(DNTG_MusicMngr.GAME_SOUND.SOUND_SCORE_PANEL);
		}
	}

	private void Update()
	{
		_updateBigPrizePlate();
	}
}
