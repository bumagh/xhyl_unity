using M__M.HaiWang.Fish;
using PathologicalGames;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class Effect_LogoMgr : MonoBehaviour
	{
		public enum LogoType
		{
			Normal,
			Score,
			Boss
		}

		public enum LogoScoreTypes
		{
			ScoreLighting,
			ScoreLaserCrab,
			ScoreBombCrab
		}

		public class LogoOrder
		{
			public LogoType type;

			public int baseOrder;

			public int capacity;

			public int index;

			public LogoOrder(LogoType type, int baseOrder, int capacity)
			{
				this.type = type;
				this.baseOrder = baseOrder;
				this.capacity = capacity;
				index = 0;
			}

			public int GetOrder()
			{
				index++;
				index %= capacity;
				return baseOrder + index * 3;
			}
		}

		private static Effect_LogoMgr s_instance;

		private HW2_SpawnPool m_logoPool;

		private List<Effect_Logo> m_logoList = new List<Effect_Logo>();

		private Dictionary<LogoType, LogoOrder> m_logoOrderMap = new Dictionary<LogoType, LogoOrder>();

		private int logo_BossOrder;

		private void InitFishOrderMap()
		{
			m_logoOrderMap[LogoType.Normal] = new LogoOrder(LogoType.Normal, 16200, 30);
			m_logoOrderMap[LogoType.Score] = new LogoOrder(LogoType.Score, 18000, 30);
			m_logoOrderMap[LogoType.Boss] = new LogoOrder(LogoType.Boss, 18200, 30);
		}

		public static Effect_LogoMgr Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
		}

		private void Start()
		{
			InitFishOrderMap();
			Init();
		}

		private void Init()
		{
			m_logoPool = HW2_PoolManager.Pools["Logo"];
		}

		public Effect_Logo SpawnGoldSharkLogo(Vector3 pos, int seatId)
		{
			Transform prefab = m_logoPool.prefabs["LogoGoldShark"];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_GoldShark>();
			component.SetLayerOrder(m_logoOrderMap[LogoType.Normal].GetOrder());
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_GoldShark)
			{
				_logo_GoldShark.Reset_Logo();
				m_logoPool.Despawn(_logo_GoldShark.transform);
				m_logoList.Remove(_logo_GoldShark);
			};
			component.Play(pos, Vector3.zero, seatId);
			return component;
		}

		public Effect_Logo SpawnLightingLogo(Vector3 pos, int seatId, int bulletPower)
		{
			Transform prefab = m_logoPool.prefabs["LogoLighting"];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_Lighting>();
			component.SetLayerOrder(m_logoOrderMap[LogoType.Normal].GetOrder());
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_Lighting)
			{
				_logo_Lighting.Reset_Logo();
				m_logoPool.Despawn(_logo_Lighting.transform);
				m_logoList.Remove(_logo_Lighting);
			};
			component.Play(pos, Vector3.zero, seatId, bulletPower);
			return component;
		}

		public string GetScorePrebName(LogoScoreTypes logScoreType)
		{
			string result = string.Empty;
			switch (logScoreType)
			{
			case LogoScoreTypes.ScoreLighting:
				result = "LogoLightingScore";
				break;
			case LogoScoreTypes.ScoreLaserCrab:
				result = "LogoLaserCrabScore";
				break;
			case LogoScoreTypes.ScoreBombCrab:
				result = "LogoBombCrabScore";
				break;
			}
			return result;
		}

		public Effect_Logo SpawnLogoScore(Vector3 pos, int seatId, int totalScore, LogoScoreTypes logoScoreType)
		{
			Transform prefab = m_logoPool.prefabs[GetScorePrebName(logoScoreType)];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_Score>();
			component.SetLayerOrder(m_logoOrderMap[LogoType.Score].GetOrder());
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_LightingScore)
			{
				_logo_LightingScore.Reset_Logo();
				m_logoPool.Despawn(_logo_LightingScore.transform);
				m_logoList.Remove(_logo_LightingScore);
			};
			component.Play(pos, Vector3.zero, seatId, totalScore);
			return component;
		}

		public Effect_Logo SpawnLaserGunLogo(Vector3 pos, int seatId, int bulletPower)
		{
			Transform prefab = m_logoPool.prefabs["LogoLaserGun"];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_LaserGun>();
			component.SetLayerOrder(m_logoOrderMap[LogoType.Normal].GetOrder());
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_LaserGun)
			{
				_logo_LaserGun.Reset_Logo();
				m_logoPool.Despawn(_logo_LaserGun.transform);
				m_logoList.Remove(_logo_LaserGun);
			};
			component.Play(pos, Vector3.zero, seatId, bulletPower);
			return component;
		}

		public Effect_Logo SpawnBombCrabLogo(Vector3 pos, int seatId, int bulletPower)
		{
			Transform prefab = m_logoPool.prefabs["LogoBombCrab"];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_BombCrab>();
			component.SetLayerOrder(m_logoOrderMap[LogoType.Normal].GetOrder());
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_BombCrab)
			{
				_logo_BombCrab.Reset_Logo();
				m_logoPool.Despawn(_logo_BombCrab.transform);
				m_logoList.Remove(_logo_BombCrab);
			};
			component.Play(pos, Vector3.zero, seatId, bulletPower);
			return component;
		}

		public Effect_Logo SpawnBossCrabShadow(Vector3 pos)
		{
			Transform prefab = m_logoPool.prefabs["BossCrabShadow"];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_BossCrabShadow>();
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_BossCrabShadow)
			{
				_logo_BossCrabShadow.Reset_Logo();
				m_logoPool.Despawn(_logo_BossCrabShadow.transform);
				m_logoList.Remove(_logo_BossCrabShadow);
			};
			component.Play(pos, Vector3.zero, 0, default(int));
			return component;
		}

		public Effect_Logo SpawnBossKrakenShadow(Vector3 pos)
		{
			UnityEngine.Debug.LogError("生成池: " + m_logoPool.name);
			Transform prefab = m_logoPool.prefabs["BossKrakenShadow"];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_BossKrankenShadow>();
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_BossKrakenShadow)
			{
				_logo_BossKrakenShadow.Reset_Logo();
				m_logoPool.Despawn(_logo_BossKrakenShadow.transform);
				m_logoList.Remove(_logo_BossKrakenShadow);
			};
			component.Play(pos, Vector3.zero, 0, default(int));
			return component;
		}

		public Effect_Logo SpawnBossCrystalFire(Vector3 pos)
		{
			pos.x = Mathf.Clamp(pos.x, -4f, 4f);
			pos.y = Mathf.Clamp(pos.y, -2f, 2f);
			Transform prefab = m_logoPool.prefabs["FireWorks_Crystal"];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_CrystalFire>();
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_BossCrystalFire)
			{
				_logo_BossCrystalFire.Reset_Logo();
				m_logoPool.Despawn(_logo_BossCrystalFire.transform);
				m_logoList.Remove(_logo_BossCrystalFire);
			};
			component.Play(pos, Vector3.zero, 0, default(int));
			return component;
		}

		private string[] GetBossPrefabCrastalAndLogoName(FishType fishType)
		{
			string[] result = new string[2]
			{
				"UI_Crystal_BossDorgan",
				"Logo_BossDragon"
			};
			switch (fishType)
			{
			case FishType.Boss_Dorgan_狂暴火龙:
				result = new string[2]
				{
					"UI_Crystal_BossDorgan",
					"Logo_BossDragon"
				};
				break;
			case FishType.Boss_Crab_霸王蟹:
				result = new string[2]
				{
					"UI_Crystal_HairyCrabs",
					"Logo_HairyCrabs"
				};
				break;
			case FishType.Boss_Kraken_深海八爪鱼:
				result = new string[2]
				{
					"UI_Crystal_BossKraken",
					"Logo_BossKraken"
				};
				break;
			case FishType.Boss_Lantern_暗夜炬兽:
				result = new string[2]
				{
					"UI_Crystal_BossLantern",
					"Logo_BossLatern"
				};
				break;
			}
			return result;
		}

		public Effect_Logo SpawnBossCrystal(FishType fishType)
		{
			Transform prefab = m_logoPool.prefabs[GetBossPrefabCrastalAndLogoName(fishType)[0]];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_Crystal>();
			component.SetLayerOrder(logo_BossOrder + 1);
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_BossCrystalDragon)
			{
				_logo_BossCrystalDragon.Reset_Logo();
				m_logoPool.Despawn(_logo_BossCrystalDragon.transform);
				m_logoList.Remove(_logo_BossCrystalDragon);
			};
			return component;
		}

		public Effect_Logo SpawnBossLogo(FishType fishType)
		{
			Transform prefab = m_logoPool.prefabs[GetBossPrefabCrastalAndLogoName(fishType)[1]];
			Transform transform = m_logoPool.transform;
			Transform transform2 = m_logoPool.Spawn(prefab, transform);
			Effect_Logo component = transform2.GetComponent<Effect_Logo_Boss>();
			logo_BossOrder = m_logoOrderMap[LogoType.Boss].GetOrder();
			component.SetLayerOrder(logo_BossOrder);
			m_logoList.Add(component);
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(Effect_Logo _logo_Boss)
			{
				_logo_Boss.Reset_Logo();
				m_logoPool.Despawn(_logo_Boss.transform);
				m_logoList.Remove(_logo_Boss);
			};
			return component;
		}

		public void RemoveAllLogos()
		{
			foreach (Effect_Logo logo in m_logoList)
			{
				logo.Reset_EventHandler();
				logo.Reset_Logo();
				m_logoPool.Despawn(logo.transform);
			}
			m_logoList.Clear();
		}
	}
}
