using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_CoinMgr : MonoBehaviour
	{
		private static FK3_Effect_CoinMgr s_intance;

		private FK3_SpawnPool m_coinPool;

		[SerializeField]
		private FK3_BigFishCoinsSetting bigFishCoinsSetting;

		private List<FK3_Effect_Coin> m_coinList = new List<FK3_Effect_Coin>();

		public static FK3_Effect_CoinMgr Get()
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
			m_coinPool = FK3_PoolManager.Pools["Coin"];
		}

		private void Update()
		{
		}

		public FK3_Effect_Coin SpawnCoin(Vector3 begin, Vector3 end)
		{
			Transform prefab = m_coinPool.prefabs["Coin"];
			Transform transform = m_coinPool.transform;
			Transform transform2 = m_coinPool.Spawn(prefab, transform);
			FK3_Effect_Coin component = transform2.GetComponent<FK3_Effect_Coin>();
			m_coinList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(FK3_Effect_Coin _coin)
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
				FK3_CoinSetting fK3_CoinSetting = bigFishCoinsSetting.coinSettings[i];
				Vector3 position = fK3_CoinSetting.pos.position;
				if (seatId == 3 || seatId == 4)
				{
					position.x = 0f - position.x;
					position.y = 0f - position.y;
				}
				StartCoroutine(SpawnBigFishCoin(begin + position, end, fK3_CoinSetting.waitTime));
			}
		}

		private IEnumerator SpawnBigFishCoin(Vector3 begin, Vector3 end, float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			SpawnCoin(begin, end);
		}

		public void RemoveAllCoins()
		{
			foreach (FK3_Effect_Coin coin in m_coinList)
			{
				coin.Reset_EventHandler();
				coin.Reset_Coin();
				m_coinPool.Despawn(coin.transform);
			}
			m_coinList.Clear();
		}
	}
}
