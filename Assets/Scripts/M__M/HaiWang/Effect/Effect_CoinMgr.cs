using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class Effect_CoinMgr : MonoBehaviour
	{
		private static Effect_CoinMgr s_intance;

		private HW2_SpawnPool m_coinPool;

		[SerializeField]
		private BigFishCoinsSetting bigFishCoinsSetting;

		private List<Effect_Coin> m_coinList = new List<Effect_Coin>();

		public static Effect_CoinMgr Get()
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
			m_coinPool = HW2_PoolManager.Pools["Coin"];
		}

		private void Update()
		{
		}

		public Effect_Coin SpawnCoin(Vector3 begin, Vector3 end)
		{
			Transform prefab = m_coinPool.prefabs["Coin"];
			Transform transform = m_coinPool.transform;
			Transform transform2 = m_coinPool.Spawn(prefab, transform);
			Effect_Coin component = transform2.GetComponent<Effect_Coin>();
			m_coinList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Coin _coin)
			{
				_coin.Reset_Coin();
				m_coinPool.Despawn(_coin.transform);
				m_coinList.Remove(_coin);
			};
			component.Play(begin, end);
			return component;
		}

		public void SpawnBigFishCoins(int rate, Vector3 begin, Vector3 end, int seatId)
		{
			int num = Math.Min((int)((float)rate * 0.5f), bigFishCoinsSetting.coinSettings.Count);
			for (int i = 0; i < num; i++)
			{
				CoinSetting coinSetting = bigFishCoinsSetting.coinSettings[i];
				Vector3 position = coinSetting.pos.position;
				if (seatId == 3 || seatId == 4)
				{
					position.x = 0f - position.x;
					position.y = 0f - position.y;
				}
				StartCoroutine(SpawnBigFishCoin(begin + position, end, coinSetting.waitTime));
			}
		}

		private IEnumerator SpawnBigFishCoin(Vector3 begin, Vector3 end, float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			SpawnCoin(begin, end);
		}

		public void RemoveAllCoins()
		{
			foreach (Effect_Coin coin in m_coinList)
			{
				coin.Reset_EventHandler();
				coin.Reset_Coin();
				m_coinPool.Despawn(coin.transform);
			}
			m_coinList.Clear();
		}
	}
}
