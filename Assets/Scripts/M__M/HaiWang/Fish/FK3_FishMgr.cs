using DG.Tweening;
using FullInspector;
using M__M.HaiWang.Bullet;
using M__M.HaiWang.Message;
using PathologicalGames;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Fish
{
	[fiInspectorOnly]
	public class FK3_FishMgr : MonoBehaviour
	{
		private static FK3_FishMgr s_instance;

		public bool debugFish;

		public int basicDisplayOrder = 100;

		[SerializeField]
		private List<FK3_FishBehaviour> m_fishPrefabList = new List<FK3_FishBehaviour>();

		private Dictionary<FK3_FishType, FK3_FishBehaviour> m_fishTypeMap = new Dictionary<FK3_FishType, FK3_FishBehaviour>();

		private Dictionary<int, FK3_FishBehaviour> m_fishIdMap = new Dictionary<int, FK3_FishBehaviour>();

		private Dictionary<FK3_FishType, List<FK3_FishBehaviour>> m_fishTypeListMap = new Dictionary<FK3_FishType, List<FK3_FishBehaviour>>();

		private List<FK3_FishBehaviour> m_lightningFishList = new List<FK3_FishBehaviour>();

		private Dictionary<FK3_FishType, FK3_FishOrder> m_fishOrderMap = new Dictionary<FK3_FishType, FK3_FishOrder>();

		private List<FK3_FishBehaviour> m_frameDeadFishList = new List<FK3_FishBehaviour>();

		private List<FK3_FishBehaviour> m_frameStartDyingFishList = new List<FK3_FishBehaviour>();

		private int m_fishGenIndex;

		private const int c_beginFishIndex = 0;

		private const int c_maxFishIndex = 5000;

		private FK3_SpawnPool m_fishPool;

		public Action<FK3_FishCreationInfo, FK3_FishBehaviour> SetupFishMovement_Case_SplineMove_Action;

		public Action<FK3_FishBehaviour> Event_OnFishCreate_Handler;

		private Rect m_screenRect = new Rect(-6.4f, -3.6f, 12.8f, 7.2f);

		private Material fishMaterial;

		public static HashSet<FK3_FishType> bossFishSet = new HashSet<FK3_FishType>
		{
			FK3_FishType.Boss_Dorgan_狂暴火龙,
			FK3_FishType.Boss_Dorgan_冰封暴龙,
			FK3_FishType.Boss_Crocodil_史前巨鳄,
			FK3_FishType.Boss_Kraken_深海八爪鱼,
			FK3_FishType.Boss_Crab_霸王蟹,
			FK3_FishType.Boss_Lantern_暗夜炬兽
		};

		public static HashSet<FK3_FishType> bigFishSet = new HashSet<FK3_FishType>
		{
			FK3_FishType.Big_Clown_巨型小丑鱼,
			FK3_FishType.Big_Puffer_巨型河豚,
			FK3_FishType.Big_Rasbora_巨型鲽鱼,
			FK3_FishType.Shark_鲨鱼,
			FK3_FishType.KillerWhale_杀人鲸
		};

		private int[] bigFishRates = new int[6]
		{
			10,
			12,
			14,
			20,
			25,
			30
		};

		private int[] sharkRates = new int[6]
		{
			20,
			30,
			40,
			45,
			50,
			60
		};

		private int[] killerWhaleRates = new int[6]
		{
			30,
			50,
			70,
			80,
			90,
			100
		};

		private int[] bossDragonRates = new int[6]
		{
			100,
			120,
			150,
			180,
			200,
			250
		};

		private int[] bossKrakenRates = new int[6]
		{
			100,
			150,
			200,
			300,
			400,
			500
		};

		private FK3_FishBehaviour fishValue;

		private int tempKey;

		private Rect screenRect;

		private bool havIn;

		private int tempId;

		private int id;

		private bool isRunning => Application.isPlaying;

		public static FK3_FishMgr Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
			fishMaterial = (Material)Resources.Load("FishMaterial/fishMaterial");
			FK3_MessageCenter.RegisterHandle("ShowAllBigPufferSize", ShowAllBigPufferSize);
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			InitFishOrderMap();
			_InitBasic();
			_AnalyzePrefab();
		}

		private void _InitBasic()
		{
			m_fishPool = FK3_PoolManager.Pools["Fish"];
			int num = 34;
			for (int i = 1; i <= num; i++)
			{
				m_fishTypeListMap[(FK3_FishType)i] = new List<FK3_FishBehaviour>();
			}
		}

		[InspectorShowIf("isRunning")]
		[InspectorButton]
		[InspectorName("活鱼统计")]
		private void Print_LiveFishStats()
		{
			_Print_LiveFishStats();
		}

		[InspectorName("活鱼统计_详细")]
		[InspectorShowIf("isRunning")]
		[InspectorButton]
		private void Print_LiveFishStats_Detail()
		{
			_Print_LiveFishStats(verbose: true);
		}

		public void _Print_LiveFishStats(bool verbose = false)
		{
			if (m_fishTypeListMap != null)
			{
				UnityEngine.Debug.Log($"LiveFishStats begin");
				List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
				foreach (KeyValuePair<FK3_FishType, List<FK3_FishBehaviour>> item in m_fishTypeListMap)
				{
					FK3_FishType key = item.Key;
					List<FK3_FishBehaviour> value = item.Value;
					if (value != null)
					{
						list.Add(new KeyValuePair<string, int>(key.ToString(), value.Count));
					}
				}
				if (verbose)
				{
					list.Sort((KeyValuePair<string, int> a, KeyValuePair<string, int> b) => a.Value - b.Value);
					list.ForEach(delegate(KeyValuePair<string, int> _)
					{
						UnityEngine.Debug.Log($"{_.Key}.count:{_.Value}");
					});
				}
				UnityEngine.Debug.LogError($"LiveFishStats> total:{m_fishIdMap.Count}");
			}
		}

		[InspectorShowIf("isRunning")]
		[InspectorName("鱼池统计")]
		[InspectorButton]
		private void Print_FishPoolStats()
		{
			_Print_FishPoolStats();
		}

		[InspectorShowIf("isRunning")]
		[InspectorName("鱼池统计_详细")]
		[InspectorButton]
		private void Print_FishPoolStats_Detail()
		{
			_Print_FishPoolStats(verbose: true);
		}

		public void _Print_FishPoolStats(bool verbose = false)
		{
			m_fishPool = FK3_PoolManager.Pools["Fish"];
			if (!(m_fishPool == null))
			{
				UnityEngine.Debug.Log($"FishPoolStats begin");
				int num = 0;
				List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
				foreach (KeyValuePair<string, FK3_PrefabPool> prefabPool in m_fishPool.prefabPools)
				{
					string key = prefabPool.Key;
					FK3_PrefabPool value = prefabPool.Value;
					list.Add(new KeyValuePair<string, int>(key, value.totalCount));
					num += value.totalCount;
				}
				if (verbose)
				{
					list.Sort((KeyValuePair<string, int> a, KeyValuePair<string, int> b) => a.Value - b.Value);
					list.ForEach(delegate(KeyValuePair<string, int> _)
					{
						UnityEngine.Debug.Log($"{_.Key}.count:{_.Value}");
					});
				}
				UnityEngine.Debug.Log($"FishPoolStats> total:{num}");
			}
		}

		private void _AnalyzePrefab()
		{
			UnityEngine.Debug.Log("==================FishMgr._AnalyzePrefab================");
			for (int i = 0; i < m_fishPrefabList.Count; i++)
			{
				FK3_FishBehaviour fK3_FishBehaviour = m_fishPrefabList[i];
				m_fishTypeMap[fK3_FishBehaviour.type] = fK3_FishBehaviour;
			}
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(1) && UnityEngine.Input.GetKey(KeyCode.LeftControl))
			{
				List<FK3_FishBehaviour> screenFishList = Get().GetScreenFishList();
				UnityEngine.Debug.Log(screenFishList.Count);
			}
		}

		private void LateUpdate()
		{
			if ((bool)FK3_BulletMgr.Get().bulletBorder && m_screenRect != FK3_BulletMgr.Get().bulletBorder.rect)
			{
				m_screenRect = FK3_BulletMgr.Get().bulletBorder.rect;
			}
			screenRect = m_screenRect;
			for (int i = 0; i < m_fishIdMap.Count; i++)
			{
				tempKey = m_fishIdMap.ElementAt(i).Key;
				if (m_fishIdMap.ContainsKey(tempKey))
				{
					fishValue = m_fishIdMap[tempKey];
					if (fishValue != null && fishValue.gameObject.activeSelf && fishValue.State == FK3_FishState.Live && fishValue.ageInSec > ((fishValue.type == FK3_FishType.Boss_Dorgan_狂暴火龙 || fishValue.type == FK3_FishType.Boss_Dorgan_冰封暴龙) ? 4f : Mathf.Clamp(10f / FK3_FishQueenMgr.Get().GetSpeedByFishType(fishValue.type.BasicType()), 5f, 10f)) && fishValue.inScreen && !screenRect.Contains(fishValue.GetCenterTransform().position))
					{
						fishValue.inScreen = false;
						fishValue.DoEscapeScreen();
					}
				}
			}
		}

		public void SetFishIndex(int index)
		{
			m_fishGenIndex = index;
		}

		public int IncreaseFishIndex()
		{
			m_fishGenIndex++;
			if (m_fishGenIndex > 5000)
			{
				m_fishGenIndex = 0;
			}
			return m_fishGenIndex;
		}

		public FK3_FishBehaviour GenNewFish(FK3_FishCreationInfo info)
		{
			FK3_FishBehaviour fK3_FishBehaviour = SpawnSingleFish(info.fishType);
			SetupFishMovement(info, fK3_FishBehaviour);
			return fK3_FishBehaviour;
		}

		private void GenFishExamine(FK3_FishType fishType, FK3_FishBehaviour fish)
		{
			if (fishType == FK3_FishType.Boss_Dorgan_狂暴火龙 || fishValue.type != FK3_FishType.Boss_Dorgan_冰封暴龙)
			{
				fish.useLog = true;
				UnityEngine.Debug.Log($"fish spawn: fish{fish.identity}");
				List<FK3_FishBehaviour> list = m_fishTypeListMap[fishType].FindAll((FK3_FishBehaviour _) => _.IsLive());
				return;
			}
			switch (fishType)
			{
			case FK3_FishType.CrabLaser_电磁蟹:
			case FK3_FishType.CrabBoom_连环炸弹蟹:
			{
				fish.useLog = true;
				UnityEngine.Debug.Log($"fish spawn: fish{fish.identity}");
				List<FK3_FishBehaviour> list3 = m_fishTypeListMap[FK3_FishType.CrabBoom_连环炸弹蟹].FindAll((FK3_FishBehaviour _) => _.IsLive());
				List<FK3_FishBehaviour> list4 = m_fishTypeListMap[FK3_FishType.CrabLaser_电磁蟹].FindAll((FK3_FishBehaviour _) => _.IsLive());
				return;
			}
			case FK3_FishType.Boss_Kraken_深海八爪鱼:
			{
				fish.useLog = true;
				UnityEngine.Debug.Log($"fish spawn: fish{fish.identity}");
				List<FK3_FishBehaviour> list2 = m_fishTypeListMap[FK3_FishType.Boss_Kraken_深海八爪鱼].FindAll((FK3_FishBehaviour _) => _.IsLive());
				return;
			}
			}
			if (fish.isLightning)
			{
				fish.useLog = true;
				UnityEngine.Debug.Log($"fish spawn: fish{fish.identity}");
				List<FK3_FishBehaviour> list5 = m_lightningFishList.FindAll((FK3_FishBehaviour _) => _.IsLive());
			}
		}

		public FK3_FishBehaviour SpawnSingleFish(FK3_FishType fishType)
		{
			int fishGenIndex = m_fishGenIndex;
			IncreaseFishIndex();
			FK3_FishBehaviour value = null;
			m_fishTypeMap.TryGetValue(fishType, out value);
			if (value == null)
			{
				return null;
			}
			Transform prefab = m_fishPool.prefabs[value.name];
			Transform transform = m_fishPool.transform;
			Transform transform2 = m_fishPool.Spawn(prefab, transform);
			FK3_FishBehaviour fish = transform2.GetComponent<FK3_FishBehaviour>();
			transform2.name = $"[id:{fishGenIndex}]";
			switch (fish.type)
			{
			case FK3_FishType.Gurnard_迦魶鱼:
				transform2.localScale = Vector3.one * 135f;
				break;
			case FK3_FishType.Clown_小丑鱼:
				transform2.localScale = Vector3.one * 120f;
				break;
			case FK3_FishType.Flounder_比目鱼:
				transform2.localScale = Vector3.one * 100f;
				break;
			case FK3_FishType.Lobster_龙虾:
				transform2.localScale = Vector3.one * 100f;
				break;
			case FK3_FishType.Octopus_章鱼:
				transform2.localScale = Vector3.one * 140f;
				break;
			case FK3_FishType.Lantern_灯笼鱼:
				transform2.localScale = Vector3.one * 100f;
				break;
			case FK3_FishType.Boss_Crab_霸王蟹:
				transform2.localScale = Vector3.one * 200f;
				break;
			case FK3_FishType.Boss_Lantern_暗夜炬兽:
				transform2.localScale = Vector3.one * 200f;
				break;
			case FK3_FishType.烈焰龟:
				transform2.localScale = Vector3.one * 200f;
				break;
			}
			fish.Reset_OnSpawned();
			fish.id = fishGenIndex;
			fish.useLog = debugFish;
			FK3_FishOrder fK3_FishOrder = m_fishOrderMap[fishType];
			fish.SetLayerOrder(fK3_FishOrder.GetOrder());
			fish.Reset_EventHandler();
			fish.Prepare();
			fish.State = FK3_FishState.Live;
			fish.gameObject.SetActive(value: true);
			fish.lifetimeDic["despawned"] = 0;
			Image fishImage = fish.GetFishImage();
			if (fishImage != null)
			{
				fish.Hitable = false;
				FK3_FishBehaviour.SetAlpha(fishImage, 0f);
				fishImage.DOFade(1f, 0.3f).OnComplete(delegate
				{
					fish.Hitable = true;
				});
			}
			else
			{
				SpriteRenderer fishRenderer = fish.GetFishRenderer();
				if (fishRenderer != null)
				{
					fish.Hitable = false;
					FK3_FishBehaviour.SetAlpha(fishRenderer, 0f);
					fishRenderer.DOFade(1f, 0.3f).OnComplete(delegate
					{
						fish.Hitable = true;
					});
				}
			}
			if (fish.useLog)
			{
				UnityEngine.Debug.Log($"SpawnSingleFish fish:{fish.identity}, bind event");
			}
			fish.Event_FishOnDespawned_Handler += delegate(FK3_FishBehaviour _fish, FK3_SpawnPool _pool)
			{
				int num = (int)fish.lifetimeDic["despawned"];
				fish.lifetimeDic["despawned"] = num + 1;
				UnRegisterFish(_fish.id);
			};
			FK3_FishBehaviour fK3_FishBehaviour = fish;
			fK3_FishBehaviour.Event_FishDieFinish_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDieFinish_Handler, (Action<FK3_FishBehaviour>)delegate(FK3_FishBehaviour _fish)
			{
				m_fishPool.Despawn(_fish.transform);
			});
			if (Event_OnFishCreate_Handler != null)
			{
				Event_OnFishCreate_Handler(fish);
			}
			RegisterFish(fish);
			return fish;
		}

		private void SetupFishMovement(FK3_FishCreationInfo info, FK3_FishBehaviour fish)
		{
			if (info.movementTpye == FK3_FishMovementType.None)
			{
				_SetupFishMovement_Case_None(info, fish);
			}
			else if (info.movementTpye == FK3_FishMovementType.SplineMove)
			{
				_SetupFishMovement_Case_SplineMove(info, fish);
			}
		}

		private void _SetupFishMovement_Case_None(FK3_FishCreationInfo info, FK3_FishBehaviour fish)
		{
		}

		private void _SetupFishMovement_Case_SplineMove(FK3_FishCreationInfo info, FK3_FishBehaviour fish)
		{
			if (SetupFishMovement_Case_SplineMove_Action != null)
			{
				SetupFishMovement_Case_SplineMove_Action(info, fish);
			}
		}

		private void RegisterFish(FK3_FishBehaviour fish)
		{
			id = fish.id;
			m_fishIdMap[id] = fish;
			m_fishIdMap[id].id = id;
			if (fish.isLightning)
			{
				m_lightningFishList.Add(fish);
			}
		}

		private void UnRegisterFish(int fishId)
		{
			if (m_fishIdMap.ContainsKey(fishId))
			{
				FK3_FishBehaviour fK3_FishBehaviour = m_fishIdMap[fishId];
				m_fishIdMap.Remove(fishId);
				if (fK3_FishBehaviour != null && fK3_FishBehaviour.isLightning)
				{
					m_lightningFishList.Remove(fK3_FishBehaviour);
				}
			}
		}

		public FK3_FishBehaviour GetFishById(int fishId)
		{
			FK3_FishBehaviour value = null;
			m_fishIdMap.TryGetValue(fishId, out value);
			return value;
		}

		public FK3_FishBehaviour GetFishFin(int fishId)
		{
			FK3_FishBehaviour result = null;
			try
			{
				result = m_fishPool.transform.Find("[id:" + fishId + "]").GetComponent<FK3_FishBehaviour>();
				return result;
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
				return result;
			}
		}

		public List<FK3_FishBehaviour> GetScreenFishList(Func<FK3_FishBehaviour, bool> filter = null)
		{
			return GetInBoundFishList(m_screenRect, filter);
		}

		public List<FK3_FishBehaviour> GetInBoundFishList(Rect rect, Func<FK3_FishBehaviour, bool> filter = null)
		{
			List<FK3_FishBehaviour> list = new List<FK3_FishBehaviour>();
			if (filter != null)
			{
				for (int i = 0; i < m_fishIdMap.Count; i++)
				{
					if (rect.Contains(m_fishIdMap.ElementAt(i).Value.GetCenterTransform().position) && filter(m_fishIdMap.ElementAt(i).Value))
					{
						list.Add(m_fishIdMap.ElementAt(i).Value);
					}
				}
			}
			else
			{
				for (int j = 0; j < m_fishIdMap.Count; j++)
				{
					if (rect.Contains(m_fishIdMap.ElementAt(j).Value.GetCenterTransform().position))
					{
						list.Add(m_fishIdMap.ElementAt(j).Value);
					}
				}
			}
			return list;
		}

		private float GetCross(Vector2 p1, Vector2 p2, Vector2 p)
		{
			return (p2.x - p1.x) * (p.y - p1.y) - (p.x - p1.x) * (p2.y - p1.y);
		}

		private bool JudgePointIn4Dots(Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
		{
			return GetCross(p1, p2, p) * GetCross(p3, p4, p) >= 0f && GetCross(p2, p3, p) * GetCross(p4, p1, p) >= 0f;
		}

		public List<FK3_FishBehaviour> GetInLaserFishList(Vector3 point, float length, Vector3 dot1, Vector3 dot2, Vector3 dot3, Vector3 dot4, Rect rect, Func<FK3_FishBehaviour, bool> filter = null)
		{
			List<FK3_FishBehaviour> list = new List<FK3_FishBehaviour>();
			if (filter != null)
			{
				for (int i = 0; i < m_fishIdMap.Count; i++)
				{
					if (rect.Contains(m_fishIdMap.ElementAt(i).Value.GetCenterTransform().localPosition) && (JudgePointIn4Dots(m_fishIdMap.ElementAt(i).Value.GetCenterTransform().position, dot1, dot2, dot3, dot4) || Vector2.Distance(point, m_fishIdMap.ElementAt(i).Value.GetCenterTransform().position) < length) && filter(m_fishIdMap.ElementAt(i).Value))
					{
						list.Add(m_fishIdMap.ElementAt(i).Value);
					}
				}
			}
			else
			{
				for (int j = 0; j < m_fishIdMap.Count; j++)
				{
					if (rect.Contains(m_fishIdMap.ElementAt(j).Value.GetCenterTransform().position) && (JudgePointIn4Dots(m_fishIdMap.ElementAt(j).Value.GetCenterTransform().position, dot1, dot2, dot3, dot4) || Vector2.Distance(point, m_fishIdMap.ElementAt(j).Value.GetCenterTransform().position) < length))
					{
						list.Add(m_fishIdMap.ElementAt(j).Value);
					}
				}
			}
			return list;
		}

		public List<FK3_FishBehaviour> GetInLaserFishList(Collider rectFride, Rect rect, Func<FK3_FishBehaviour, bool> filter)
		{
			List<FK3_FishBehaviour> list = new List<FK3_FishBehaviour>();
			Bounds bounds = rectFride.bounds;
			for (int i = 0; i < m_fishIdMap.Count; i++)
			{
				if (rect.Contains(m_fishIdMap.ElementAt(i).Value.GetCenterTransform().localPosition) && bounds.Contains(m_fishIdMap.ElementAt(i).Value.GetCenterTransform().position) && filter(m_fishIdMap.ElementAt(i).Value))
				{
					list.Add(m_fishIdMap.ElementAt(i).Value);
				}
			}
			return list;
		}

		public List<FK3_FishBehaviour> GetAllLiveFishList()
		{
			List<FK3_FishBehaviour> list = new List<FK3_FishBehaviour>();
			for (int i = 0; i < m_fishIdMap.Count; i++)
			{
				list.Add(m_fishIdMap.ElementAt(i).Value);
			}
			return list;
		}

		public FK3_FishBehaviour Test_GetTopFish()
		{
			using (Dictionary<int, FK3_FishBehaviour>.Enumerator enumerator = m_fishIdMap.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.Value;
				}
			}
			return null;
		}

		[Obsolete]
		private int CalcFishRenderOrder(FK3_FishType fishType)
		{
			return 10;
		}

		[Obsolete]
		private int GetFishTypeBaseOrder(FK3_FishType fishType)
		{
			int result = 10;
			switch (fishType)
			{
			case FK3_FishType.Gurnard_迦魶鱼:
				result = 9000;
				break;
			case FK3_FishType.Clown_小丑鱼:
				result = 8500;
				break;
			case FK3_FishType.Rasbora_鲽鱼:
				result = 8000;
				break;
			case FK3_FishType.Puffer_河豚:
				result = 7800;
				break;
			case FK3_FishType.Grouper_狮子鱼:
				result = 7600;
				break;
			case FK3_FishType.Flounder_比目鱼:
				result = 7400;
				break;
			case FK3_FishType.Lobster_龙虾:
				result = 7200;
				break;
			case FK3_FishType.Swordfish_旗鱼:
				result = 7000;
				break;
			case FK3_FishType.Octopus_章鱼:
				result = 6800;
				break;
			case FK3_FishType.Lantern_灯笼鱼:
				result = 6600;
				break;
			case FK3_FishType.Tortoise_海龟:
				result = 6400;
				break;
			case FK3_FishType.Sawfish_锯齿鲨:
				result = 6200;
				break;
			case FK3_FishType.Mobula_蝠魟:
				result = 6000;
				break;
			case FK3_FishType.GoldShark_霸王鲸:
				result = 5800;
				break;
			case FK3_FishType.Shark_鲨鱼:
				result = 5600;
				break;
			case FK3_FishType.KillerWhale_杀人鲸:
				result = 5400;
				break;
			case FK3_FishType.Big_Clown_巨型小丑鱼:
				result = 5200;
				break;
			case FK3_FishType.Big_Rasbora_巨型鲽鱼:
				result = 5000;
				break;
			case FK3_FishType.Big_Puffer_巨型河豚:
				result = 4800;
				break;
			case FK3_FishType.Boss_Dorgan_狂暴火龙:
				result = 4780;
				break;
			case FK3_FishType.Boss_Crab_霸王蟹:
				result = 4760;
				break;
			case FK3_FishType.Boss_Kraken_深海八爪鱼:
				result = 4740;
				break;
			case FK3_FishType.Boss_Lantern_暗夜炬兽:
				result = 4720;
				break;
			case FK3_FishType.CrabLaser_电磁蟹:
				result = 4600;
				break;
			case FK3_FishType.CrabBoom_连环炸弹蟹:
				result = 4580;
				break;
			case FK3_FishType.Lightning_Gurnard_闪电迦魶鱼:
				result = 9400;
				break;
			case FK3_FishType.Lightning_Clown_闪电小丑鱼:
				result = 8900;
				break;
			case FK3_FishType.Lightning_Rasbora_闪电鲽鱼:
				result = 8150;
				break;
			case FK3_FishType.Lightning_Puffer_闪电河豚:
				result = 7950;
				break;
			case FK3_FishType.Lightning_Grouper_闪电狮子鱼:
				result = 7750;
				break;
			case FK3_FishType.Lightning_Flounder_闪电比目鱼:
				result = 7550;
				break;
			case FK3_FishType.Lightning_Lobster_闪电龙虾:
				result = 7350;
				break;
			case FK3_FishType.Lightning_Swordfish_闪电旗鱼:
				result = 7150;
				break;
			case FK3_FishType.Lightning_Octopus_闪电章鱼:
				result = 6950;
				break;
			}
			return result;
		}

		private void InitFishOrderMap()
		{
			m_fishOrderMap[FK3_FishType.Gurnard_迦魶鱼] = new FK3_FishOrder(FK3_FishType.Gurnard_迦魶鱼, 9000, 400);
			m_fishOrderMap[FK3_FishType.Clown_小丑鱼] = new FK3_FishOrder(FK3_FishType.Clown_小丑鱼, 8500, 400);
			m_fishOrderMap[FK3_FishType.Rasbora_鲽鱼] = new FK3_FishOrder(FK3_FishType.Rasbora_鲽鱼, 9000, 400);
			m_fishOrderMap[FK3_FishType.Puffer_河豚] = new FK3_FishOrder(FK3_FishType.Puffer_河豚, 8800, 150);
			m_fishOrderMap[FK3_FishType.Grouper_狮子鱼] = new FK3_FishOrder(FK3_FishType.Grouper_狮子鱼, 8600, 150);
			m_fishOrderMap[FK3_FishType.Flounder_比目鱼] = new FK3_FishOrder(FK3_FishType.Flounder_比目鱼, 8400, 150);
			m_fishOrderMap[FK3_FishType.Lobster_龙虾] = new FK3_FishOrder(FK3_FishType.Lobster_龙虾, 8200, 150);
			m_fishOrderMap[FK3_FishType.Swordfish_旗鱼] = new FK3_FishOrder(FK3_FishType.Swordfish_旗鱼, 8000, 150);
			m_fishOrderMap[FK3_FishType.Octopus_章鱼] = new FK3_FishOrder(FK3_FishType.Octopus_章鱼, 7800, 150);
			m_fishOrderMap[FK3_FishType.Lantern_灯笼鱼] = new FK3_FishOrder(FK3_FishType.Lantern_灯笼鱼, 7600, 150);
			m_fishOrderMap[FK3_FishType.Tortoise_海龟] = new FK3_FishOrder(FK3_FishType.Tortoise_海龟, 7400, 150);
			m_fishOrderMap[FK3_FishType.Sawfish_锯齿鲨] = new FK3_FishOrder(FK3_FishType.Sawfish_锯齿鲨, 7200, 150);
			m_fishOrderMap[FK3_FishType.Mobula_蝠魟] = new FK3_FishOrder(FK3_FishType.Mobula_蝠魟, 7000, 150);
			m_fishOrderMap[FK3_FishType.GoldShark_霸王鲸] = new FK3_FishOrder(FK3_FishType.GoldShark_霸王鲸, 6800, 150);
			m_fishOrderMap[FK3_FishType.Shark_鲨鱼] = new FK3_FishOrder(FK3_FishType.Shark_鲨鱼, 6500, 100);
			m_fishOrderMap[FK3_FishType.KillerWhale_杀人鲸] = new FK3_FishOrder(FK3_FishType.KillerWhale_杀人鲸, 6400, 100);
			m_fishOrderMap[FK3_FishType.Big_Clown_巨型小丑鱼] = new FK3_FishOrder(FK3_FishType.Big_Clown_巨型小丑鱼, 6300, 100);
			m_fishOrderMap[FK3_FishType.Big_Rasbora_巨型鲽鱼] = new FK3_FishOrder(FK3_FishType.Big_Rasbora_巨型鲽鱼, 6200, 100);
			m_fishOrderMap[FK3_FishType.Big_Puffer_巨型河豚] = new FK3_FishOrder(FK3_FishType.Big_Puffer_巨型河豚, 6100, 100);
			m_fishOrderMap[FK3_FishType.Boss_Dorgan_狂暴火龙] = new FK3_FishOrder(FK3_FishType.Boss_Dorgan_狂暴火龙, 6050, 50);
			m_fishOrderMap[FK3_FishType.Boss_Dorgan_冰封暴龙] = new FK3_FishOrder(FK3_FishType.Boss_Dorgan_冰封暴龙, 6050, 50);
			m_fishOrderMap[FK3_FishType.Boss_Crab_霸王蟹] = new FK3_FishOrder(FK3_FishType.Boss_Crab_霸王蟹, 6000, 50);
			m_fishOrderMap[FK3_FishType.Boss_Kraken_深海八爪鱼] = new FK3_FishOrder(FK3_FishType.Boss_Kraken_深海八爪鱼, 5950, 50);
			m_fishOrderMap[FK3_FishType.Boss_Crocodil_史前巨鳄] = new FK3_FishOrder(FK3_FishType.Boss_Crocodil_史前巨鳄, 5900, 50);
			m_fishOrderMap[FK3_FishType.Boss_Lantern_暗夜炬兽] = new FK3_FishOrder(FK3_FishType.Boss_Lantern_暗夜炬兽, 5900, 50);
			m_fishOrderMap[FK3_FishType.CrabLaser_电磁蟹] = new FK3_FishOrder(FK3_FishType.CrabLaser_电磁蟹, 5850, 50);
			m_fishOrderMap[FK3_FishType.CrabBoom_连环炸弹蟹] = new FK3_FishOrder(FK3_FishType.CrabBoom_连环炸弹蟹, 5800, 50);
			m_fishOrderMap[FK3_FishType.CrabDrill_钻头蟹] = new FK3_FishOrder(FK3_FishType.烈焰龟, 7950, 50);
			m_fishOrderMap[FK3_FishType.CrabStorm_暴风蟹] = new FK3_FishOrder(FK3_FishType.烈焰龟, 7950, 50);
			m_fishOrderMap[FK3_FishType.烈焰龟] = new FK3_FishOrder(FK3_FishType.烈焰龟, 7950, 50);
			m_fishOrderMap[FK3_FishType.Lightning_Gurnard_闪电迦魶鱼] = new FK3_FishOrder(FK3_FishType.Lightning_Gurnard_闪电迦魶鱼, 9400, 100);
			m_fishOrderMap[FK3_FishType.Lightning_Clown_闪电小丑鱼] = new FK3_FishOrder(FK3_FishType.Lightning_Clown_闪电小丑鱼, 8900, 100);
			m_fishOrderMap[FK3_FishType.Lightning_Rasbora_闪电鲽鱼] = new FK3_FishOrder(FK3_FishType.Lightning_Rasbora_闪电鲽鱼, 8400, 100);
			m_fishOrderMap[FK3_FishType.Lightning_Puffer_闪电河豚] = new FK3_FishOrder(FK3_FishType.Lightning_Puffer_闪电河豚, 8950, 50);
			m_fishOrderMap[FK3_FishType.Lightning_Grouper_闪电狮子鱼] = new FK3_FishOrder(FK3_FishType.Lightning_Grouper_闪电狮子鱼, 8750, 50);
			m_fishOrderMap[FK3_FishType.Lightning_Flounder_闪电比目鱼] = new FK3_FishOrder(FK3_FishType.Lightning_Flounder_闪电比目鱼, 8550, 50);
			m_fishOrderMap[FK3_FishType.Lightning_Lobster_闪电龙虾] = new FK3_FishOrder(FK3_FishType.Lightning_Lobster_闪电龙虾, 8350, 50);
			m_fishOrderMap[FK3_FishType.Lightning_Swordfish_闪电旗鱼] = new FK3_FishOrder(FK3_FishType.Lightning_Swordfish_闪电旗鱼, 8150, 50);
			m_fishOrderMap[FK3_FishType.Lightning_Octopus_闪电章鱼] = new FK3_FishOrder(FK3_FishType.Lightning_Octopus_闪电章鱼, 7950, 50);
		}

		public int GetFishRate(FK3_FishType fishType, int index)
		{
			int result = 0;
			switch (fishType)
			{
			case FK3_FishType.Shark_鲨鱼:
				result = ((index < sharkRates.Length) ? sharkRates[index] : sharkRates[sharkRates.Length - 1]);
				break;
			case FK3_FishType.KillerWhale_杀人鲸:
				result = ((index < killerWhaleRates.Length) ? killerWhaleRates[index] : killerWhaleRates[killerWhaleRates.Length - 1]);
				break;
			case FK3_FishType.Big_Clown_巨型小丑鱼:
			case FK3_FishType.Big_Rasbora_巨型鲽鱼:
			case FK3_FishType.Big_Puffer_巨型河豚:
				UnityEngine.Debug.LogError("位置集合大小:" + bigFishRates.Length);
				try
				{
					return (index < bigFishRates.Length) ? bigFishRates[index] : bigFishRates[bigFishRates.Length - 1];
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("错误: " + arg);
					return 30;
				}
			case FK3_FishType.Boss_Dorgan_狂暴火龙:
			case FK3_FishType.Boss_Dorgan_冰封暴龙:
				result = ((index < bossDragonRates.Length) ? bossDragonRates[index] : bossDragonRates[bossDragonRates.Length - 1]);
				break;
			case FK3_FishType.Boss_Crab_霸王蟹:
			case FK3_FishType.Boss_Kraken_深海八爪鱼:
			case FK3_FishType.Boss_Crocodil_史前巨鳄:
			case FK3_FishType.Boss_Lantern_暗夜炬兽:
				result = ((index < bossKrakenRates.Length) ? bossKrakenRates[index] : bossKrakenRates[bossKrakenRates.Length - 1]);
				break;
			}
			return result;
		}

		private void ShowAllBigPufferSize(FK3_KeyValueInfo keyValueInfo)
		{
			List<FK3_FishBehaviour> screenFishList = Get().GetScreenFishList((FK3_FishBehaviour _fish) => _fish.IsLive() && _fish.type == FK3_FishType.Big_Puffer_巨型河豚);
			for (int i = 0; i < screenFishList.Count; i++)
			{
				try
				{
					screenFishList[i].PlayAni("Size");
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("============动画播放错误==========" + arg);
				}
				if (screenFishList[i].Event_ShowSize_Handler != null)
				{
					screenFishList[i].Event_ShowSize_Handler();
				}
			}
		}

		[InspectorButton]
		private void FishLeaveScene()
		{
			Get().GetAllLiveFishList().ForEach(delegate(FK3_FishBehaviour _fish)
			{
				if (_fish.IsLive())
				{
					_fish.LeaveScene();
				}
			});
		}
	}
}
