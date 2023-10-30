using M__M.HaiWang.Fish;
using PathologicalGames;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class Effect_ScoreMgr : MonoBehaviour
	{
		private static Effect_ScoreMgr s_intance;

		private HW2_SpawnPool m_scorePool;

		[SerializeField]
		private Transform[] m_prefabScores;

		[SerializeField]
		private Transform m_bossScore;

		private HW2_SpawnPool m_scoreBackPool;

		private List<Effect_Score> m_scoreList = new List<Effect_Score>();

		private List<Transform> m_scoreBackList = new List<Transform>();

		public static Effect_ScoreMgr Get()
		{
			return s_intance;
		}

		private void Awake()
		{
			s_intance = this;
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			_InitBasic();
		}

		private void _InitBasic()
		{
			m_scorePool = HW2_PoolManager.Pools["Score"];
			m_scoreBackPool = HW2_PoolManager.Pools["ScoreBack"];
		}

		private void Update()
		{
		}

		public Effect_Score SpawnScore_old(int num, int seatId, Vector3 pos, bool BigScore = false, Action next = null)
		{
			Transform prefab = m_scorePool.prefabs["Score"];
			Transform transform = m_scorePool.transform;
			Transform transform2 = m_scorePool.Spawn(prefab, transform);
			if (seatId == 1 || seatId == 2)
			{
				transform2.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
			else
			{
				transform2.localRotation = Quaternion.Euler(0f, 0f, 180f);
			}
			Effect_Score component = transform2.GetComponent<Effect_Score>();
			component.Reset_EventHandler();
			component.Event_Over_Handler += DespawnScore;
			component.Play(num, FishType.Unknown, 0, seatId, new FollowData(pos), isBoss: false, BigScore, scoreFollow: false, null, next);
			return component;
		}

		public Effect_Score SpawnScore()
		{
			Transform prefab = m_scorePool.prefabs["Score"];
			Transform transform = m_scorePool.transform;
			Transform transform2 = m_scorePool.Spawn(prefab, transform);
			return transform2.GetComponent<Effect_Score>();
		}

		public Effect_Score DoPlayScore(int num, FishType fishType, int bulletPower, int seatId, FollowData followData, bool isBoss, bool bigScore, bool scoreFollow, bool showScoreBack, int deadWay, Action next = null)
		{
			Transform transform = m_scorePool.transform;
			Transform transform2;
			if (bigScore)
			{
				Transform bossScore = m_bossScore;
				transform2 = m_scorePool.Spawn(bossScore, transform);
			}
			else
			{
				Transform prefab = m_prefabScores[0];
				transform2 = m_scorePool.Spawn(prefab, transform);
			}
			if (seatId == 1 || seatId == 2)
			{
				if (HW2_GVars.lobby.curSeatId == 1 || HW2_GVars.lobby.curSeatId == 2)
				{
					transform2.localRotation = Quaternion.Euler(0f, 0f, 0f);
				}
				else
				{
					transform2.localRotation = Quaternion.Euler(0f, 0f, 180f);
				}
			}
			else if (HW2_GVars.lobby.curSeatId == 3 || HW2_GVars.lobby.curSeatId == 4)
			{
				transform2.localRotation = Quaternion.Euler(0f, 0f, 180f);
			}
			else
			{
				transform2.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
			Effect_Score component = transform2.GetComponent<Effect_Score>();
			component.Reset_EventHandler();
			component.Event_Over_Handler += DespawnScore;
			m_scoreList.Add(component);
			Transform scoreBackInst = null;
			if (showScoreBack)
			{
				Transform prefab2 = m_scoreBackPool.prefabs["ScoreBack"];
				transform = m_scoreBackPool.transform;
				scoreBackInst = m_scoreBackPool.Spawn(prefab2, transform);
				component.SetScoreBackTrans(scoreBackInst);
				m_scoreBackList.Add(scoreBackInst);
				next = (Action)Delegate.Combine(next, (Action)delegate
				{
					m_scoreBackPool.Despawn(scoreBackInst.transform);
					m_scoreBackList.Remove(scoreBackInst);
				});
			}
			component.Play(num, fishType, bulletPower, seatId, followData, isBoss, bigScore, scoreFollow, scoreBackInst, next);
			return component;
		}

		private void DespawnScore(Effect_Score scoreInst)
		{
			scoreInst.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			m_scorePool.Despawn(scoreInst.transform);
			m_scoreList.Remove(scoreInst);
		}

		public void RemoveAllScores()
		{
			foreach (Effect_Score score in m_scoreList)
			{
				score.Reset_EventHandler();
				score.Reset_Score();
				m_scorePool.Despawn(score.transform);
			}
			m_scoreList.Clear();
			foreach (Transform scoreBack in m_scoreBackList)
			{
				m_scoreBackPool.Despawn(scoreBack);
			}
			m_scoreBackList.Clear();
			Effect_Score.bibiCount = 0;
		}
	}
}
