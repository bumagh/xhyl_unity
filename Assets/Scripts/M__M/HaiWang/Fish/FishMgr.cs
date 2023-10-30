using DG.Tweening;
using FullInspector;
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
	public class FishMgr : MonoBehaviour
	{
		private static FishMgr s_instance;

		public bool debugFish;

		public int basicDisplayOrder = 100;

		[SerializeField]
		private List<FishBehaviour> m_fishPrefabList = new List<FishBehaviour>();

		private Dictionary<FishType, FishBehaviour> m_fishTypeMap = new Dictionary<FishType, FishBehaviour>();

		private Dictionary<int, FishBehaviour> m_fishIdMap = new Dictionary<int, FishBehaviour>();

		private Dictionary<FishType, List<FishBehaviour>> m_fishTypeListMap = new Dictionary<FishType, List<FishBehaviour>>();

		private List<FishBehaviour> m_lightningFishList = new List<FishBehaviour>();

		private Dictionary<FishType, FishOrder> m_fishOrderMap = new Dictionary<FishType, FishOrder>();

		private List<FishBehaviour> m_frameDeadFishList = new List<FishBehaviour>();

		private List<FishBehaviour> m_frameStartDyingFishList = new List<FishBehaviour>();

		private int m_fishGenIndex;

		private const int c_beginFishIndex = 0;

		private const int c_maxFishIndex = 5000;

		private HW2_SpawnPool m_fishPool;

		public Action<FishCreationInfo, FishBehaviour> SetupFishMovement_Case_SplineMove_Action;

		public Action<FishBehaviour> Event_OnFishCreate_Handler;

		private Rect m_screenRect = new Rect(-6.4f, -3.6f, 12.8f, 7.2f);

		private Material fishMaterial;

		public static HashSet<FishType> bossFishSet = new HashSet<FishType>
		{
			FishType.Boss_Dorgan_狂暴火龙,
			FishType.Boss_Kraken_深海八爪鱼,
			FishType.Boss_Crab_霸王蟹,
			FishType.Boss_Lantern_暗夜炬兽
		};

		public static HashSet<FishType> bigFishSet = new HashSet<FishType>
		{
			FishType.Big_Clown_巨型小丑鱼,
			FishType.Big_Puffer_巨型河豚,
			FishType.Big_Rasbora_巨型鲽鱼,
			FishType.Shark_鲨鱼,
			FishType.KillerWhale_杀人鲸
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

		private FishBehaviour fishValue;

		private int tempKey;

		private Rect screenRect;

		private bool havIn;

		private int tempId;

		private int id;

		private FishBehaviour fishBehaviour;

		private bool isRunning => Application.isPlaying;

		public static FishMgr Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
			fishMaterial = (Material)Resources.Load("FishMaterial/fishMaterial");
			HW2_MessageCenter.RegisterHandle("ShowAllBigPufferSize", ShowAllBigPufferSize);
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
			m_fishPool = HW2_PoolManager.Pools["Fish"];
			int num = 34;
			for (int i = 1; i <= num; i++)
			{
				m_fishTypeListMap[(FishType)i] = new List<FishBehaviour>();
			}
		}

		[InspectorButton]
		[InspectorShowIf("isRunning")]
		[InspectorName("活鱼统计")]
		private void Print_LiveFishStats()
		{
			_Print_LiveFishStats();
		}

		[InspectorName("活鱼统计_详细")]
		[InspectorButton]
		[InspectorShowIf("isRunning")]
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
				foreach (KeyValuePair<FishType, List<FishBehaviour>> item in m_fishTypeListMap)
				{
					FishType key = item.Key;
					List<FishBehaviour> value = item.Value;
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

		[InspectorName("鱼池统计")]
		[InspectorShowIf("isRunning")]
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
			m_fishPool = HW2_PoolManager.Pools["Fish"];
			if (!(m_fishPool == null))
			{
				UnityEngine.Debug.Log($"FishPoolStats begin");
				int num = 0;
				List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
				foreach (KeyValuePair<string, HW2_PrefabPool> prefabPool in m_fishPool.prefabPools)
				{
					string key = prefabPool.Key;
					HW2_PrefabPool value = prefabPool.Value;
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
				FishBehaviour fishBehaviour = m_fishPrefabList[i];
				UnityEngine.Debug.LogError("fishBehaviour.type: " + fishBehaviour.type);
				m_fishTypeMap[fishBehaviour.type] = fishBehaviour;
			}
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(1) && UnityEngine.Input.GetKey(KeyCode.LeftControl))
			{
				List<FishBehaviour> screenFishList = Get().GetScreenFishList();
				UnityEngine.Debug.Log(screenFishList.Count);
			}
		}

		private void LateUpdate()
		{
			screenRect = m_screenRect;
			for (int i = 0; i < m_fishIdMap.Count; i++)
			{
				tempKey = m_fishIdMap.ElementAt(i).Key;
				if (m_fishIdMap.ContainsKey(tempKey))
				{
					fishValue = m_fishIdMap[tempKey];
					if (fishValue != null && fishValue.gameObject.activeSelf && fishValue.State == FishState.Live && fishValue.ageInSec > ((fishValue.type == FishType.Boss_Dorgan_狂暴火龙) ? 4f : Mathf.Clamp(10f / FishQueenMgr.Get().GetSpeedByFishType(fishValue.type.BasicType()), 5f, 10f)) && fishValue.inScreen && !screenRect.Contains(fishValue.GetCenterTransform().position))
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

		public FishBehaviour GenNewFish(FishCreationInfo info)
		{
			FishBehaviour fishBehaviour = SpawnSingleFish(info.fishType);
			SetupFishMovement(info, fishBehaviour);
			return fishBehaviour;
		}

		private void GenFishExamine(FishType fishType, FishBehaviour fish)
		{
			switch (fishType)
			{
			case FishType.Boss_Dorgan_狂暴火龙:
			{
				fish.useLog = true;
				UnityEngine.Debug.Log($"fish spawn: fish{fish.identity}");
				List<FishBehaviour> list4 = m_fishTypeListMap[fishType].FindAll((FishBehaviour _) => _.IsLive());
				return;
			}
			case FishType.CrabLaser_电磁蟹:
			case FishType.CrabBoom_连环炸弹蟹:
			{
				fish.useLog = true;
				UnityEngine.Debug.Log($"fish spawn: fish{fish.identity}");
				List<FishBehaviour> list2 = m_fishTypeListMap[FishType.CrabBoom_连环炸弹蟹].FindAll((FishBehaviour _) => _.IsLive());
				List<FishBehaviour> list3 = m_fishTypeListMap[FishType.CrabLaser_电磁蟹].FindAll((FishBehaviour _) => _.IsLive());
				return;
			}
			case FishType.Boss_Kraken_深海八爪鱼:
			{
				fish.useLog = true;
				UnityEngine.Debug.Log($"fish spawn: fish{fish.identity}");
				List<FishBehaviour> list = m_fishTypeListMap[FishType.Boss_Kraken_深海八爪鱼].FindAll((FishBehaviour _) => _.IsLive());
				return;
			}
			}
			if (fish.isLightning)
			{
				fish.useLog = true;
				UnityEngine.Debug.Log($"fish spawn: fish{fish.identity}");
				List<FishBehaviour> list5 = m_lightningFishList.FindAll((FishBehaviour _) => _.IsLive());
			}
		}

		public FishBehaviour SpawnSingleFish(FishType fishType)
		{
			int fishGenIndex = m_fishGenIndex;
			IncreaseFishIndex();
			FishBehaviour value = null;
			m_fishTypeMap.TryGetValue(fishType, out value);
			if (value == null)
			{
				UnityEngine.Debug.LogError($"FishMgr不存在[{fishType}]");
				return null;
			}
			Transform transform = m_fishPool.prefabs[value.name];
			FishBehaviour component = transform.GetComponent<FishBehaviour>();
			component.id = fishGenIndex;
			Transform transform2 = m_fishPool.transform;
			Transform transform3 = m_fishPool.Spawn(transform, transform2);
			FishBehaviour fish = transform3.GetComponent<FishBehaviour>();
			transform3.name = $"{transform.name}[id:{fishGenIndex}]";
			switch (fish.type)
			{
			case FishType.Unknown:
			case FishType.Gurnard_迦魶鱼:
			case FishType.Clown_小丑鱼:
			case FishType.Lobster_龙虾:
			case FishType.Octopus_章鱼:
			case FishType.Tortoise_海龟:
				transform3.localScale = Vector3.one * 140f;
				break;
			case FishType.Rasbora_鲽鱼:
				transform3.localScale = Vector3.one * 40f;
				break;
			case FishType.Puffer_河豚:
				transform3.localScale = Vector3.one * 180f;
				break;
			case FishType.Grouper_狮子鱼:
				transform3.localScale = Vector3.one * 120f;
				break;
			case FishType.Flounder_比目鱼:
				transform3.localScale = Vector3.one * 110f;
				break;
			case FishType.Big_Rasbora_巨型鲽鱼:
				transform3.localScale = Vector3.one * 100f;
				break;
			}
			fish.Reset_OnSpawned();
			fish.id = fishGenIndex;
			fish.useLog = debugFish;
			FishOrder fishOrder = m_fishOrderMap[fishType];
			fish.SetLayerOrder(fishOrder.GetOrder());
			fish.Reset_EventHandler();
			fish.Prepare();
			fish.State = FishState.Live;
			fish.gameObject.SetActive(value: true);
			fish.lifetimeDic["despawned"] = 0;
			Image spriteRenderer = fish.GetSpriteRenderer();
			if (spriteRenderer != null)
			{
				fish.Hitable = false;
				FishBehaviour.SetAlpha(spriteRenderer, 0f);
				spriteRenderer.DOFade(1f, 0.3f).OnComplete(delegate
				{
					fish.Hitable = true;
				});
			}
			if (fish.useLog)
			{
				UnityEngine.Debug.Log($"SpawnSingleFish fish:{fish.identity}, bind event");
			}
			fish.Event_FishOnDespawned_Handler += delegate(FishBehaviour _fish, HW2_SpawnPool _pool)
			{
				int num = (int)fish.lifetimeDic["despawned"];
				if (num > 0)
				{
					UnityEngine.Debug.Log(HW2_LogHelper.Red("{0} despawned repeat. {1}", _fish.identity, num));
				}
				fish.lifetimeDic["despawned"] = num + 1;
				if (_fish.useLog)
				{
					UnityEngine.Debug.Log($"Fish.OnDespawned x id:{_fish.id}, type:{_fish.type}, lifetime:{_fish.ageInSec}");
				}
				UnRegisterFish(_fish.id);
			};
			FishBehaviour fishBehaviour = fish;
			fishBehaviour.Event_FishDieFinish_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.Event_FishDieFinish_Handler, (Action<FishBehaviour>)delegate(FishBehaviour _fish)
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

		public void KillFish()
		{
			try
			{
				for (int i = 0; i < m_fishPool.transform.childCount; i++)
				{
					if (m_fishPool.transform.GetChild(i).gameObject.activeInHierarchy)
					{
						m_fishPool.transform.GetChild(i).gameObject.SetActive(value: false);
					}
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		private void SetupFishMovement(FishCreationInfo info, FishBehaviour fish)
		{
			if (info.movementTpye == FishMovementType.None)
			{
				_SetupFishMovement_Case_None(info, fish);
			}
			else if (info.movementTpye == FishMovementType.SplineMove)
			{
				_SetupFishMovement_Case_SplineMove(info, fish);
			}
		}

		private void _SetupFishMovement_Case_None(FishCreationInfo info, FishBehaviour fish)
		{
		}

		private void _SetupFishMovement_Case_SplineMove(FishCreationInfo info, FishBehaviour fish)
		{
			if (SetupFishMovement_Case_SplineMove_Action != null)
			{
				SetupFishMovement_Case_SplineMove_Action(info, fish);
			}
		}

		private void RegisterFish(FishBehaviour fish)
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
				fishBehaviour = m_fishIdMap[fishId];
				m_fishIdMap.Remove(fishId);
				if (fishBehaviour != null && fishBehaviour.isLightning)
				{
					m_lightningFishList.Remove(fishBehaviour);
				}
			}
		}

		private void UnRegisterFish(FishBehaviour fish)
		{
			UnRegisterFish(fish.id);
		}

		public FishBehaviour GetFishById(int fishId)
		{
			FishBehaviour value = null;
			m_fishIdMap.TryGetValue(fishId, out value);
			return value;
		}

		public List<FishBehaviour> GetScreenFishList(Func<FishBehaviour, bool> filter = null)
		{
			return GetInBoundFishList(m_screenRect, filter);
		}

		public List<FishBehaviour> GetInBoundFishList(Rect rect, Func<FishBehaviour, bool> filter = null)
		{
			List<FishBehaviour> list = new List<FishBehaviour>();
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

		public List<FishBehaviour> GetInLaserFishList(Vector3 point, float length, Vector3 dot1, Vector3 dot2, Vector3 dot3, Vector3 dot4, Rect rect, Func<FishBehaviour, bool> filter = null)
		{
			List<FishBehaviour> list = new List<FishBehaviour>();
			if (filter != null)
			{
				for (int i = 0; i < m_fishIdMap.Count; i++)
				{
					if (rect.Contains(m_fishIdMap.ElementAt(i).Value.GetCenterTransform().position) && (JudgePointIn4Dots(m_fishIdMap.ElementAt(i).Value.GetCenterTransform().position, dot1, dot2, dot3, dot4) || Vector2.Distance(point, m_fishIdMap.ElementAt(i).Value.GetCenterTransform().position) < length) && filter(m_fishIdMap.ElementAt(i).Value))
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

		public List<FishBehaviour> GetAllLiveFishList()
		{
			List<FishBehaviour> list = new List<FishBehaviour>();
			for (int i = 0; i < m_fishIdMap.Count; i++)
			{
				list.Add(m_fishIdMap.ElementAt(i).Value);
			}
			return list;
		}

		public FishBehaviour Test_GetTopFish()
		{
			using (Dictionary<int, FishBehaviour>.Enumerator enumerator = m_fishIdMap.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.Value;
				}
			}
			return null;
		}

		[Obsolete]
		private int CalcFishRenderOrder(FishType fishType)
		{
			return 10;
		}

		[Obsolete]
		private int GetFishTypeBaseOrder(FishType fishType)
		{
			int result = 10;
			switch (fishType)
			{
			case FishType.Gurnard_迦魶鱼:
				result = 9000;
				break;
			case FishType.Clown_小丑鱼:
				result = 8500;
				break;
			case FishType.Rasbora_鲽鱼:
				result = 8000;
				break;
			case FishType.Puffer_河豚:
				result = 7800;
				break;
			case FishType.Grouper_狮子鱼:
				result = 7600;
				break;
			case FishType.Flounder_比目鱼:
				result = 7400;
				break;
			case FishType.Lobster_龙虾:
				result = 7200;
				break;
			case FishType.Swordfish_旗鱼:
				result = 7000;
				break;
			case FishType.Octopus_章鱼:
				result = 6800;
				break;
			case FishType.Lantern_灯笼鱼:
				result = 6600;
				break;
			case FishType.Tortoise_海龟:
				result = 6400;
				break;
			case FishType.Sawfish_锯齿鲨:
				result = 6200;
				break;
			case FishType.Mobula_蝠魟:
				result = 6000;
				break;
			case FishType.GoldShark_霸王鲸:
				result = 5800;
				break;
			case FishType.Shark_鲨鱼:
				result = 5600;
				break;
			case FishType.KillerWhale_杀人鲸:
				result = 5400;
				break;
			case FishType.Big_Clown_巨型小丑鱼:
				result = 5200;
				break;
			case FishType.Big_Rasbora_巨型鲽鱼:
				result = 5000;
				break;
			case FishType.Big_Puffer_巨型河豚:
				result = 4800;
				break;
			case FishType.Boss_Dorgan_狂暴火龙:
				result = 4780;
				break;
			case FishType.Boss_Crab_霸王蟹:
				result = 4760;
				break;
			case FishType.Boss_Kraken_深海八爪鱼:
				result = 4740;
				break;
			case FishType.Boss_Lantern_暗夜炬兽:
				result = 4720;
				break;
			case FishType.CrabLaser_电磁蟹:
				result = 4600;
				break;
			case FishType.CrabBoom_连环炸弹蟹:
				result = 4580;
				break;
			case FishType.Lightning_Gurnard_闪电迦魶鱼:
				result = 9400;
				break;
			case FishType.Lightning_Clown_闪电小丑鱼:
				result = 8900;
				break;
			case FishType.Lightning_Rasbora_闪电鲽鱼:
				result = 8150;
				break;
			case FishType.Lightning_Puffer_闪电河豚:
				result = 7950;
				break;
			case FishType.Lightning_Grouper_闪电狮子鱼:
				result = 7750;
				break;
			case FishType.Lightning_Flounder_闪电比目鱼:
				result = 7550;
				break;
			case FishType.Lightning_Lobster_闪电龙虾:
				result = 7350;
				break;
			case FishType.Lightning_Swordfish_闪电旗鱼:
				result = 7150;
				break;
			case FishType.Lightning_Octopus_闪电章鱼:
				result = 6950;
				break;
			}
			return result;
		}

		private void InitFishOrderMap()
		{
			m_fishOrderMap[FishType.Gurnard_迦魶鱼] = new FishOrder(FishType.Gurnard_迦魶鱼, 9000, 400);
			m_fishOrderMap[FishType.Clown_小丑鱼] = new FishOrder(FishType.Clown_小丑鱼, 8500, 400);
			m_fishOrderMap[FishType.Rasbora_鲽鱼] = new FishOrder(FishType.Rasbora_鲽鱼, 9000, 400);
			m_fishOrderMap[FishType.Puffer_河豚] = new FishOrder(FishType.Puffer_河豚, 8800, 150);
			m_fishOrderMap[FishType.Grouper_狮子鱼] = new FishOrder(FishType.Grouper_狮子鱼, 8600, 150);
			m_fishOrderMap[FishType.Flounder_比目鱼] = new FishOrder(FishType.Flounder_比目鱼, 8400, 150);
			m_fishOrderMap[FishType.Lobster_龙虾] = new FishOrder(FishType.Lobster_龙虾, 8200, 150);
			m_fishOrderMap[FishType.Swordfish_旗鱼] = new FishOrder(FishType.Swordfish_旗鱼, 8000, 150);
			m_fishOrderMap[FishType.Octopus_章鱼] = new FishOrder(FishType.Octopus_章鱼, 7800, 150);
			m_fishOrderMap[FishType.Lantern_灯笼鱼] = new FishOrder(FishType.Lantern_灯笼鱼, 7600, 150);
			m_fishOrderMap[FishType.Tortoise_海龟] = new FishOrder(FishType.Tortoise_海龟, 7400, 150);
			m_fishOrderMap[FishType.Sawfish_锯齿鲨] = new FishOrder(FishType.Sawfish_锯齿鲨, 7200, 150);
			m_fishOrderMap[FishType.Mobula_蝠魟] = new FishOrder(FishType.Mobula_蝠魟, 7000, 150);
			m_fishOrderMap[FishType.GoldShark_霸王鲸] = new FishOrder(FishType.GoldShark_霸王鲸, 6800, 150);
			m_fishOrderMap[FishType.Shark_鲨鱼] = new FishOrder(FishType.Shark_鲨鱼, 6500, 100);
			m_fishOrderMap[FishType.KillerWhale_杀人鲸] = new FishOrder(FishType.KillerWhale_杀人鲸, 6400, 100);
			m_fishOrderMap[FishType.Big_Clown_巨型小丑鱼] = new FishOrder(FishType.Big_Clown_巨型小丑鱼, 6300, 100);
			m_fishOrderMap[FishType.Big_Rasbora_巨型鲽鱼] = new FishOrder(FishType.Big_Rasbora_巨型鲽鱼, 6200, 100);
			m_fishOrderMap[FishType.Big_Puffer_巨型河豚] = new FishOrder(FishType.Big_Puffer_巨型河豚, 6100, 100);
			m_fishOrderMap[FishType.Boss_Dorgan_狂暴火龙] = new FishOrder(FishType.Boss_Dorgan_狂暴火龙, 6050, 50);
			m_fishOrderMap[FishType.Boss_Crab_霸王蟹] = new FishOrder(FishType.Boss_Crab_霸王蟹, 6000, 50);
			m_fishOrderMap[FishType.Boss_Kraken_深海八爪鱼] = new FishOrder(FishType.Boss_Kraken_深海八爪鱼, 5950, 50);
			m_fishOrderMap[FishType.Boss_Lantern_暗夜炬兽] = new FishOrder(FishType.Boss_Lantern_暗夜炬兽, 5900, 50);
			m_fishOrderMap[FishType.CrabLaser_电磁蟹] = new FishOrder(FishType.CrabLaser_电磁蟹, 5850, 50);
			m_fishOrderMap[FishType.CrabBoom_连环炸弹蟹] = new FishOrder(FishType.CrabBoom_连环炸弹蟹, 5800, 50);
			m_fishOrderMap[FishType.Lightning_Gurnard_闪电迦魶鱼] = new FishOrder(FishType.Lightning_Gurnard_闪电迦魶鱼, 9400, 100);
			m_fishOrderMap[FishType.Lightning_Clown_闪电小丑鱼] = new FishOrder(FishType.Lightning_Clown_闪电小丑鱼, 8900, 100);
			m_fishOrderMap[FishType.Lightning_Rasbora_闪电鲽鱼] = new FishOrder(FishType.Lightning_Rasbora_闪电鲽鱼, 8400, 100);
			m_fishOrderMap[FishType.Lightning_Puffer_闪电河豚] = new FishOrder(FishType.Lightning_Puffer_闪电河豚, 8950, 50);
			m_fishOrderMap[FishType.Lightning_Grouper_闪电狮子鱼] = new FishOrder(FishType.Lightning_Grouper_闪电狮子鱼, 8750, 50);
			m_fishOrderMap[FishType.Lightning_Flounder_闪电比目鱼] = new FishOrder(FishType.Lightning_Flounder_闪电比目鱼, 8550, 50);
			m_fishOrderMap[FishType.Lightning_Lobster_闪电龙虾] = new FishOrder(FishType.Lightning_Lobster_闪电龙虾, 8350, 50);
			m_fishOrderMap[FishType.Lightning_Swordfish_闪电旗鱼] = new FishOrder(FishType.Lightning_Swordfish_闪电旗鱼, 8150, 50);
			m_fishOrderMap[FishType.Lightning_Octopus_闪电章鱼] = new FishOrder(FishType.Lightning_Octopus_闪电章鱼, 7950, 50);
		}

		public int GetFishRate(FishType fishType, int index)
		{
			int result = 0;
			switch (fishType)
			{
			case FishType.Shark_鲨鱼:
				result = ((index < sharkRates.Length) ? sharkRates[index] : sharkRates[sharkRates.Length - 1]);
				break;
			case FishType.KillerWhale_杀人鲸:
				result = ((index < killerWhaleRates.Length) ? killerWhaleRates[index] : killerWhaleRates[killerWhaleRates.Length - 1]);
				break;
			case FishType.Big_Clown_巨型小丑鱼:
			case FishType.Big_Rasbora_巨型鲽鱼:
			case FishType.Big_Puffer_巨型河豚:
				result = ((index < bigFishRates.Length) ? bigFishRates[index] : bigFishRates[bigFishRates.Length - 1]);
				break;
			case FishType.Boss_Dorgan_狂暴火龙:
				result = ((index < bossDragonRates.Length) ? bossDragonRates[index] : bossDragonRates[bossDragonRates.Length - 1]);
				break;
			case FishType.Boss_Crab_霸王蟹:
			case FishType.Boss_Kraken_深海八爪鱼:
			case FishType.Boss_Lantern_暗夜炬兽:
				result = ((index < bossKrakenRates.Length) ? bossKrakenRates[index] : bossKrakenRates[bossKrakenRates.Length - 1]);
				break;
			}
			return result;
		}

		private void ShowAllBigPufferSize(KeyValueInfo keyValueInfo)
		{
			List<FishBehaviour> screenFishList = Get().GetScreenFishList((FishBehaviour _fish) => _fish.IsLive() && _fish.type == FishType.Big_Puffer_巨型河豚);
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
			Get().GetAllLiveFishList().ForEach(delegate(FishBehaviour _fish)
			{
				if (_fish.IsLive())
				{
					_fish.LeaveScene();
				}
			});
		}
	}
}
