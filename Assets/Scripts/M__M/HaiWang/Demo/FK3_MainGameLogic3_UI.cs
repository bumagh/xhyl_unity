using HW3L;
using LitJson;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Bullet;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Effect.Define;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.KillFish;
using M__M.HaiWang.Message;
using M__M.HaiWang.NetMsgDefine;
using M__M.HaiWang.Player;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.Scenario;
using M__M.HaiWang.UIDefine;
using PathSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_MainGameLogic3_UI : MonoBehaviour
	{
		[Serializable]
		public class DebugFishSettings
		{
			[SerializeField]
			private bool _ignoreFish;

			[SerializeField]
			private bool _ignoreNewFish;

			[SerializeField]
			private bool _ignoreNewFormation;

			[SerializeField]
			private bool _ignoreNewGroup;

			[SerializeField]
			private bool _ignoreNewKingCrab;

			[SerializeField]
			private bool _ignoreNewDeepSeaOctopusBoss;

			public bool IgnoreFish => _ignoreFish;

			public bool IgnoreNewFish => _ignoreFish || _ignoreNewFish;

			public bool IgnoreNewFormation => _ignoreFish || _ignoreNewFormation;

			public bool IgnoreNewGroup => _ignoreFish || _ignoreNewGroup;

			public bool IgnoreNewKingCrab => _ignoreFish || _ignoreNewKingCrab;

			public bool IgnoreNewDeepSeaOctopusBoss => _ignoreFish || _ignoreNewDeepSeaOctopusBoss;
		}

		public string sceneName;

		public List<GameObject> roots;

		public GameObject Option;

		private bool m_useNet = true;

		[SerializeField]
		private DebugFishSettings debugFish = new DebugFishSettings();

		private FK3_InGameUIContext m_uiContext;

		private FK3_GameContext m_context = new FK3_GameContext();

		private FK3_LightingFishAction m_lightingFishAction;

		private FK3_BombCrabAction m_bombCrabAction;

		private FK3_CaymanAction m_caymanAction;

		private FK3_BossCrabAction m_bossCrabAction;

		private FK3_BossLanternAction m_bossLanternAction;

		private FK3_BossCrocodileAction m_bossCrocodileAction;

		private FK3_BossKrakenAction m_bossKrakenAction;

		private FK3_BossDorganAction m_bossDorganAction;

		private FK3_LaserCrabAction m_laserCrabAction;

		private FK3_DrillCrabAction m_drillCrabAction;

		public FK3_EffectConfig effectConfig;

		private bool isLongPressBtnPlusPower;

		private bool isPressBtnPlusPower;

		private float _lastDdownTime;

		private float _PlusPowerInterval = 1f;

		private bool isLongPressBtnMinusPower;

		private bool isPressBtnMinusPower;

		private float _lastMinusDownTime;

		private float _MinusPoerInterval = 1f;

		[SerializeField]
		private GameObject _input;

		private FK3_SpriteButton m_btnAuto;

		private FK3_SpriteButton btnChangeGun;

		private FK3_GunBehaviour nativeGun;

		private int newFishCount;

		private int newFishGroupCount;

		private int newFishFormationCount;

		private const int maxNewFishNum = int.MaxValue;

		public static bool stopHandleNewFish;

		private float stopHandleInterval = 2f;

		public static int otherGunValue;

		private int _bulletId;

		private Coroutine _coGunStatusControl;

		private int userGameScore;

		public Transform transformSlel;

		private int sceneNum = 1;

		private FK3_NewFishGroupItemInfo item;

		private bool isPlayFishGroup;

		private FK3_FishQueenData fishQueenData;

		private WaitForSeconds wait = new WaitForSeconds(1f);

		private WaitForSeconds wait2 = new WaitForSeconds(17f);

		private Coroutine waitFishGroup;

		private Coroutine waitFishGroup2;

		private int numUserFired;

		private int powerUserFired;

		private int num2UserFired;

		private int num3UserFired;

		private float angleUserFired;

		private FK3_GunBehaviour gunBehaviourStopFrid;

		public static GameObject MainCamera
		{
			get;
			set;
		}

		private void Awake()
		{
			FK3_MessageCenter.RegisterHandle("SwithInCleanSceneLong", SwithInCleanSceneLong);
			FK3_MessageCenter.RegisterHandle("SwithInCleanSceneShort", SwithInCleanSceneShort);
			FK3_MessageCenter.RegisterHandle("LockGunShoot", GunLogic_Handle_GunLockShoot);
			FK3_MessageCenter.RegisterHandle("LockGunSelectFish", GunLogic_Handle_GunLockSelect);
			FK3_MessageCenter.RegisterHandle("StopHandleNewFish", StopHandleNewFish);
			FK3_AppSceneMgr.RegisterScene(sceneName);
			FK3_AppSceneMgr.RegisterAction("Game.EnterGame", EnterGame);
			if (FK3_AppSceneMgr.isFirstScene(sceneName))
			{
				FK3_FishBehaviour.SEvent_FishOnStart_Handler = delegate
				{
				};
			}
			else
			{
				transformSlel = base.transform;
			}
		}

		private void Start()
		{
			InitUI();
			InitSettingState();
			FK3_AppManager fK3_AppManager = FK3_MB_Singleton<FK3_AppManager>.Get();
			fK3_AppManager.InGame_NetDown_Handler = (Action)Delegate.Combine(fK3_AppManager.InGame_NetDown_Handler, new Action(InGameDropped));
			FK3_AppManager fK3_AppManager2 = FK3_MB_Singleton<FK3_AppManager>.Get();
			fK3_AppManager2.InGame_ReconnectSuccess_Handler = (Action)Delegate.Combine(fK3_AppManager2.InGame_ReconnectSuccess_Handler, new Action(InGameConnectSuccess));
			if (FK3_AppSceneMgr.isFirstScene(sceneName))
			{
				PreInit();
				if (!m_useNet)
				{
				}
			}
			else
			{
				PreInit();
				DisableScene();
			}
		}

		private void InitUI()
		{
			if (FK3_UIIngameManager.GetInstance() == null)
			{
				UnityEngine.Debug.LogError("FK3_UIIngameManager为空!");
				return;
			}
			FK3_UIIngameManager.GetInstance().OpenUI("HeadTitle");
			FK3_UIIngameManager.GetInstance().OpenUI("GunUI");
			FK3_UIIngameManager.GetInstance().OpenUI("OpScore");
			FK3_UIIngameManager.GetInstance().OpenUI("ChatRoot");
			Option.SetActive(value: true);
		}

		private void InitSettingState()
		{
			if (PlayerPrefs.HasKey("BgSound"))
			{
				int @int = PlayerPrefs.GetInt("BgSound");
				FK3_Singleton<FK3_SoundMgr>.Get().SetActive(@int == 1);
			}
			if (PlayerPrefs.HasKey("ScreenBright"))
			{
				int int2 = PlayerPrefs.GetInt("ScreenBright");
				Screen.sleepTimeout = ((int2 == 1) ? (-1) : (-2));
			}
			if (PlayerPrefs.HasKey("GroupChat"))
			{
				int int3 = PlayerPrefs.GetInt("GroupChat");
				FK3_GVars.isShutChatGroup = (int3 == 1);
			}
			if (PlayerPrefs.HasKey("PrivateChat"))
			{
				int int4 = PlayerPrefs.GetInt("PrivateChat");
				FK3_GVars.isShutChatPrivate = (int4 == 1);
			}
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.P))
			{
				object[] array = new object[1]
				{
					21
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.Boss_Dorgan_冰封暴龙 + JsonMapper.ToJson(array));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.W))
			{
				object[] array2 = new object[1]
				{
					24
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.Boss_Crocodil_史前巨鳄 + JsonMapper.ToJson(array2));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array2);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
			{
				object[] array3 = new object[1]
				{
					27
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.CrabBoom_连环炸弹蟹 + JsonMapper.ToJson(array3));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array3);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.E))
			{
				object[] array4 = new object[1]
				{
					28
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.CrabDrill_钻头蟹 + JsonMapper.ToJson(array4));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array4);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.R))
			{
				object[] array5 = new object[1]
				{
					29
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.CrabStorm_暴风蟹 + JsonMapper.ToJson(array5));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array5);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.T))
			{
				object[] array6 = new object[1]
				{
					30
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.烈焰龟 + JsonMapper.ToJson(array6));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array6);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
			{
				object[] array7 = new object[1]
				{
					sceneNum
				};
				sceneNum++;
				if (sceneNum > 4)
				{
					sceneNum = 1;
				}
				UnityEngine.Debug.LogError("切换场景: " + JsonMapper.ToJson(array7));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/SwitchScene", array7);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.U))
			{
				object[] array8 = new object[1]
				{
					22
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.Boss_Crab_霸王蟹 + JsonMapper.ToJson(array8));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array8);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.I))
			{
				object[] array9 = new object[1]
				{
					20
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.Boss_Dorgan_狂暴火龙 + JsonMapper.ToJson(array9));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array9);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.O))
			{
				object[] array10 = new object[1]
				{
					24
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.Boss_Crocodil_史前巨鳄 + JsonMapper.ToJson(array10));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array10);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.A))
			{
				object[] array11 = new object[1]
				{
					25
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.Boss_Lantern_暗夜炬兽 + JsonMapper.ToJson(array11));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array11);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.S))
			{
				object[] array12 = new object[1]
				{
					37
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.Lightning_Lobster_闪电龙虾 + JsonMapper.ToJson(array12));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array12);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.B))
			{
				object[] array13 = new object[1]
				{
					23
				};
				UnityEngine.Debug.LogError("当前出鱼: " + FK3_FishType.Boss_Kraken_深海八爪鱼 + JsonMapper.ToJson(array13));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendFish", array13);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && FK3_GVars.m_curState == FK3_Demo_UI_State.InGame)
			{
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("是否退出游戏？", "是否退出游戏？", string.Empty), showOkCancel: true, Handle_BackToLobby);
			}
			if (Input.GetMouseButtonDown(0))
			{
				FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get().HideOption();
			}
			if (isPressBtnPlusPower && Time.time - _lastDdownTime > _PlusPowerInterval)
			{
				if (_PlusPowerInterval > 0.5f)
				{
					isLongPressBtnPlusPower = true;
				}
				Handle_UI_BtnPlusPower(null);
				_lastDdownTime = Time.time;
				_PlusPowerInterval = 0.33f;
			}
			if (isPressBtnMinusPower && Time.time - _lastMinusDownTime > _MinusPoerInterval)
			{
				if (_MinusPoerInterval > 0.5f)
				{
					isLongPressBtnMinusPower = true;
				}
				Handle_UI_BtnMinusPower(null);
				_lastMinusDownTime = Time.time;
				_MinusPoerInterval = 0.33f;
			}
			if (stopHandleNewFish)
			{
				stopHandleInterval -= Time.deltaTime;
				if (stopHandleInterval < 0f)
				{
					stopHandleNewFish = false;
				}
			}
		}

		private void CheckPressPlusPowerTime()
		{
			float num = Time.time - _lastDdownTime;
			UnityEngine.Debug.Log($"pressTime:{num}, intverval:{_PlusPowerInterval}");
			if (!isLongPressBtnPlusPower && num < _PlusPowerInterval)
			{
				Handle_UI_BtnPlusPower(null);
			}
		}

		private void CheckPressMinusPowerTime()
		{
			float num = Time.time - _lastMinusDownTime;
			if (!isLongPressBtnMinusPower && num < _MinusPoerInterval)
			{
				Handle_UI_BtnMinusPower(null);
			}
		}

		private void PreInit()
		{
			PreInit_NetMsgHandler();
			PreInit_FormationV2();
			PreInit_UIEventHandler();
			PreInit_UI();
			m_lightingFishAction = new FK3_LightingFishAction();
			m_lightingFishAction.Init();
			m_bombCrabAction = base.gameObject.GetComponent<FK3_BombCrabAction>();
			m_bombCrabAction.Init();
			m_laserCrabAction = base.gameObject.GetComponent<FK3_LaserCrabAction>();
			m_drillCrabAction = base.gameObject.GetComponent<FK3_DrillCrabAction>();
			m_caymanAction = base.gameObject.GetComponent<FK3_CaymanAction>();
			m_bossCrabAction = base.gameObject.GetComponent<FK3_BossCrabAction>();
			m_bossLanternAction = base.gameObject.GetComponent<FK3_BossLanternAction>();
			m_bossCrocodileAction = base.gameObject.GetComponent<FK3_BossCrocodileAction>();
			m_bossKrakenAction = base.gameObject.GetComponent<FK3_BossKrakenAction>();
			m_bossDorganAction = base.gameObject.GetComponent<FK3_BossDorganAction>();
			FK3_FishTeamMgr.Get().DisableAllTeams();
			base.gameObject.AddComponent<FK3_KillFishMgr>();
			FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().Init();
		}

		private void PreInit_NetMsgHandler()
		{
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("newFishPush", HandleNetMsg_NewFish);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("newFishGroupPush", HandleNetMsg_NewFishGroup);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("newFishFormationPush", HandleNetMsg_NewFishFormation);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("newFishPufferPush", HandleNetMsg_NewPuffer);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("newKingCrabBossPush", HandleNetMsg_NewKingCrabBoss);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("newDeepSeaOctopusBossPush", HandleNetMsg_NewDeepSeaOctopusBossPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("changeStagePush", HandleNetMsg_ChangeStage);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userCoinIn", HandleNetMsg_UserCoinIn);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userCoinOut", HandleNetMsg_UserCoinOut);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("gunHitFish", delegate(object[] _)
			{
				HandleNetMsg_Default("gunHitFish", _);
			});
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("askForFishFormation", delegate(object[] _)
			{
				HandleNetMsg_Default("gunHitFish", _);
			});
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("playerScenePush", HandleNetMsg_PlayerScenePush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("gunHitFishInAction", HandleNetMsg_GunHitFishInAction);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("bulletHitFishInActionPush", HandleNetMsg_GunHitFishInActionPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("leaveSeat", HandleNetMsg_LeaveSeat);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("quitToDeskPush", HandleNetMsg_QuitToDesk);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("updateDeskInfoPush", HandleNetMsg_UpdateDeskInfoPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("changeGun", HandleNetMsg_ChangeGun);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("changeSkin", HandleNetMsg_ChangeSkin);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("changeGunPush", HandleNetMsg_ChangeGunPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("changeSkinPush", HandleNetMsg_ChangeSkinPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userFired", HandleNetMsg_UserFired);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("notfired", HandleNetMsg_NotFired);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userFiredPush", HandleNetMsg_UserFiredPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userFiredEM", HandleNetMsg_UserFiredEM);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userFiredDrill", HandleNetMsg_UserFiredDR);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userFiredEMPush", HandleNetMsg_UserFiredEMPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userFiredDrillPush", HandleNetMsg_UserFiredDRPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userRotateEM", HandleNetMsg_UserRotateEM);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userRotateDrill", HandleNetMsg_UserRotateDR);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userRotateEMPush", HandleNetMsg_UserRotateEMPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userRotateDrillPush", HandleNetMsg_UserRotateDRPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("bulletHitFishPush", HandleNetMsg_BulletHitFish);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("gunHitLockFish", HandleNetMsg_GunHitLockFish);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("bulletHitLockFishPush", HandleNetMsg_BulletHitLockFishPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("lockSelectFish", HandleNetMsg_LockSelectFish);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("lockSelectFishPush", HandleNetMsg_LockSelectFishPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("bombCrabNextPoint", HandleNetMsg_BombCrabNextPoint);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("bombCrabNextPointPush", HandleNetMsg_BombCrabNextPointPush);
		}

		private void PreInit_FormationV1()
		{
			FK3_FishFormationFactory.Get().createFun = delegate(FK3_FishType _fishType, int id)
			{
				FK3_FishMgr.Get().SetFishIndex(id);
				FK3_FishCreationInfo info = new FK3_FishCreationInfo(_fishType);
				FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().GenNewFish(info);
				if (id != fK3_FishBehaviour.id)
				{
					UnityEngine.Debug.LogError($"Formation.Create> expect id:{id}, actual id:{fK3_FishBehaviour.id}");
				}
				fK3_FishBehaviour.SetPosition(new Vector3(-3f, 0f));
				FishBindEventHandler(fK3_FishBehaviour);
				FK3_NavPathAgent fK3_NavPathAgent = fK3_FishBehaviour.gameObject.GetComponent<FK3_NavPathAgent>();
				if (fK3_NavPathAgent == null)
				{
					fK3_NavPathAgent = fK3_FishBehaviour.gameObject.AddComponent<FK3_NavPathAgent>();
				}
				else
				{
					fK3_NavPathAgent.enabled = true;
				}
				bool doneOnce = false;
				Action<FK3_FishBehaviour> b = delegate(FK3_FishBehaviour _fish)
				{
					if (!doneOnce)
					{
						doneOnce = true;
						FK3_AgentData<FK3_FishType> fK3_AgentData = _fish.GetComponent<FK3_NavPathAgent>().userData as FK3_AgentData<FK3_FishType>;
						fK3_AgentData?.formation.RemoveObject(fK3_AgentData.agent);
					}
				};
				FK3_FishBehaviour fK3_FishBehaviour2 = fK3_FishBehaviour;
				fK3_FishBehaviour2.Event_FishDying_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDying_Handler, b);
				FK3_FishBehaviour fK3_FishBehaviour3 = fK3_FishBehaviour;
				fK3_FishBehaviour3.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour3.Event_FishDie_Handler, b);
				FK3_FishBehaviour fK3_FishBehaviour4 = fK3_FishBehaviour;
				fK3_FishBehaviour4.Event_FishMovePause_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour4.Event_FishMovePause_Handler, b);
				return fK3_NavPathAgent;
			};
			FK3_FishFormationFactory.Get().destoryAction = delegate(FK3_NavPathAgent _navPathAgent, FK3_FishType _fishType)
			{
				FK3_FishBehaviour component = _navPathAgent.GetComponent<FK3_FishBehaviour>();
				if (component.State == FK3_FishState.Live)
				{
					component.Die();
					_navPathAgent.enabled = false;
				}
			};
			FK3_Formation<FK3_FishType>.forbiddenPlayOnStart = true;
		}

		private void PreInit_FormationV2()
		{
			FK3_fiSimpleSingletonBehaviour<FK3_FishSpawnMaster>.Get().setupFishAction = FishBindEventHandler;
		}

		private void PreInit_UI()
		{
			btnChangeGun = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().transform.Find("ChangeAndAuto/BtnChangeGun").GetComponent<FK3_SpriteButton>();
			m_btnAuto = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().transform.Find("ChangeAndAuto/BtnAuto").GetComponent<FK3_SpriteButton>();
			btnChangeGun.onClick = Handle_UI_BtnChangeGun;
			m_btnAuto.onClick = Handle_UI_BtnAuto;
		}

		private void Init_Gun()
		{
			_input.GetComponent<BoxCollider2D>().enabled = true;
			Assert.raiseExceptions = true;
			FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().ForeachGun(delegate(FK3_GunBehaviour gun)
			{
				gun.gameObject.SetActive(value: false);
			});
			bool rotate = FK3_GVars.lobby.curSeatId > 2;
			SceneRotateControl(rotate);
			if (FK3_GVars.lobby.curSeatId == 1 || FK3_GVars.lobby.curSeatId == 3)
			{
				FK3_GunUIMgr.Get().StopCoroutine(FK3_GunUIMgr.Get().pointMove());
				FK3_GunUIMgr.Get().point1.gameObject.SetActive(value: true);
				FK3_GunUIMgr.Get().point2.gameObject.SetActive(value: false);
				FK3_GunUIMgr.Get().StartCoroutine(FK3_GunUIMgr.Get().pointMove());
			}
			else if (FK3_GVars.lobby.curSeatId == 2 || FK3_GVars.lobby.curSeatId == 4)
			{
				FK3_GunUIMgr.Get().StopCoroutine(FK3_GunUIMgr.Get().pointMove());
				FK3_GunUIMgr.Get().point1.gameObject.SetActive(value: false);
				FK3_GunUIMgr.Get().point2.gameObject.SetActive(value: true);
				FK3_GunUIMgr.Get().StartCoroutine(FK3_GunUIMgr.Get().pointMove());
			}
			FK3_GunUIMgr.Get().SetRotate(rotate);
			FK3_DeskInfo curDesk = FK3_GVars.lobby.GetCurDesk();
			List<FK3_SeatInfo2> inGameSeats = FK3_GVars.lobby.inGameSeats;
			foreach (FK3_SeatInfo2 item2 in inGameSeats)
			{
				SetPlayerGun(curDesk, item2, forceUpdate: true);
			}
			FK3_BulletMgr.Get().debugAllowShoot = true;
			nativeGun = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			FK3_GunBehaviour fK3_GunBehaviour = nativeGun;
			fK3_GunBehaviour.Event_ShootNormal_Handler = (FK3_GunBehaviour.EventHandler_Shoot)Delegate.Combine(fK3_GunBehaviour.Event_ShootNormal_Handler, new FK3_GunBehaviour.EventHandler_Shoot(GunLogic_Handle_GunShoot2));
			FK3_GunBehaviour fK3_GunBehaviour2 = nativeGun;
			fK3_GunBehaviour2.Event_ShootEM_Handler = (FK3_GunBehaviour.EventHandler_ShootEM)Delegate.Combine(fK3_GunBehaviour2.Event_ShootEM_Handler, new FK3_GunBehaviour.EventHandler_ShootEM(GunLogic_Handle_GunShootEM));
			FK3_GunBehaviour fK3_GunBehaviour3 = nativeGun;
			fK3_GunBehaviour3.Event_RotateEM_Handler = (FK3_GunBehaviour.EventHandler_RotateEM)Delegate.Combine(fK3_GunBehaviour3.Event_RotateEM_Handler, new FK3_GunBehaviour.EventHandler_RotateEM(GunLogic_Handle_GunRotateEM));
			FK3_GunBehaviour fK3_GunBehaviour4 = nativeGun;
			fK3_GunBehaviour4.Event_RotateDR_Handler = (FK3_GunBehaviour.EventHandler_RotateEM)Delegate.Combine(fK3_GunBehaviour4.Event_RotateDR_Handler, new FK3_GunBehaviour.EventHandler_RotateEM(GunLogic_Handle_GunRotateDR));
			FK3_GunBehaviour fK3_GunBehaviour5 = nativeGun;
			fK3_GunBehaviour5.Event_ShootDR_Handler = (FK3_GunBehaviour.EventHandler_ShootDR)Delegate.Combine(fK3_GunBehaviour5.Event_ShootDR_Handler, new FK3_GunBehaviour.EventHandler_ShootDR(GunLogic_Handle_GunShootDR));
			FK3_GunBehaviour fK3_GunBehaviour6 = nativeGun;
			fK3_GunBehaviour6.Event_ScreenBeClick_act = (Action<bool>)Delegate.Combine(fK3_GunBehaviour6.Event_ScreenBeClick_act, new Action<bool>(ShowInOutScorePanel));
			FK3_GunBehaviour fK3_GunBehaviour7 = nativeGun;
			fK3_GunBehaviour7.Event_ChangeAuto_Handler = (FK3_GunBehaviour.EventHandler_ChangeAuto)Delegate.Combine(fK3_GunBehaviour7.Event_ChangeAuto_Handler, new FK3_GunBehaviour.EventHandler_ChangeAuto(GunLogic_Handle_OnChangeAuto));
			if (FK3_GVars.lobby.curSeatId == 1 || FK3_GVars.lobby.curSeatId == 4)
			{
				nativeGun.GunMove(odd: true);
			}
			else if (FK3_GVars.lobby.curSeatId == 2 || FK3_GVars.lobby.curSeatId == 3)
			{
				nativeGun.GunMove(odd: false);
			}
			FK3_SpriteButton btnPlusPower = nativeGun.GetBtnPlusPower();
			btnPlusPower.onClick = delegate
			{
				Debuger.Log("btnPlusPower.onClick");
				isPressBtnPlusPower = false;
				CheckPressPlusPowerTime();
			};
			btnPlusPower.onPress = delegate
			{
				UnityEngine.Debug.Log("btnPlusPower.onPress");
				isPressBtnPlusPower = true;
				_PlusPowerInterval = 1f;
				_lastDdownTime = Time.time;
			};
			btnPlusPower.onExit = delegate
			{
				UnityEngine.Debug.Log("btnPlusPower.onExit");
				isPressBtnPlusPower = false;
				isLongPressBtnPlusPower = false;
			};
			FK3_SpriteButton btnMinsPower = nativeGun.GetBtnMinsPower();
			btnMinsPower.onClick = delegate
			{
				isPressBtnMinusPower = false;
				CheckPressMinusPowerTime();
			};
			btnMinsPower.onPress = delegate
			{
				isPressBtnMinusPower = true;
				_MinusPoerInterval = 1f;
				_lastMinusDownTime = Time.time;
			};
			btnMinsPower.onExit = delegate
			{
				isPressBtnMinusPower = false;
				isLongPressBtnMinusPower = false;
			};
			FK3_BulletMgr fK3_BulletMgr = FK3_BulletMgr.Get();
			fK3_BulletMgr.Event_BulletOver_Handler = (Action<FK3_BulletController>)Delegate.Combine(fK3_BulletMgr.Event_BulletOver_Handler, (Action<FK3_BulletController>)delegate
			{
				nativeGun.OnBulletOver();
			});
			SetBtnAuto(isAuto: false);
			btnChangeGun.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/lock_0");
			FK3_UIIngameManager.GetInstance().OpenUI("UserInfoRoot", show: false);
			FK3_UserInfoInGame fK3_UserInfoInGame = FK3_SimpleSingletonBehaviour<FK3_UserInfoInGame>.Get();
			fK3_UserInfoInGame.Event_PrivateChatClick_act = (Action<int>)Delegate.Combine(fK3_UserInfoInGame.Event_PrivateChatClick_act, new Action<int>(OpenChatWindow));
			FK3_MessageCenter.RegisterHandle("CheckAutoLock", CheckAutoLock);
		}

		private void SetPlayerGun(FK3_DeskInfo desk, FK3_SeatInfo2 seat, bool forceUpdate = false)
		{
			UnityEngine.Debug.LogError("desk: " + desk.name + " seat: " + seat.id + " forceUpdate: " + forceUpdate);
			int id = seat.id;
			FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(id);
			FK3_GunUIController gunByID = FK3_GunUIMgr.Get().GetGunByID(id);
			if (forceUpdate)
			{
				UnityEngine.Debug.LogError("forceUpdate");
				gunById.Reset_EventHandler();
				gunById.Reset_Gun();
				if (id != FK3_GVars.lobby.curSeatId)
				{
					FK3_GunBehaviour fK3_GunBehaviour = gunById;
					fK3_GunBehaviour.Event_ShootNormal_Handler = (FK3_GunBehaviour.EventHandler_Shoot)Delegate.Combine(fK3_GunBehaviour.Event_ShootNormal_Handler, new FK3_GunBehaviour.EventHandler_Shoot(FK3_BulletMgr.Get().Handle_Shoot2));
					FK3_GunBehaviour fK3_GunBehaviour2 = gunById;
					fK3_GunBehaviour2.Event_GunBeClick_act = (Action<int>)Delegate.Combine(fK3_GunBehaviour2.Event_GunBeClick_act, new Action<int>(ShowUserInfo));
				}
			}
			if (seat.isFree)
			{
				UnityEngine.Debug.LogError("isFree");
				if (gunById.gameObject.activeSelf)
				{
					if (gunById.Event_StopLaserCrab != null)
					{
						gunById.Event_StopLaserCrab();
						gunById.OutEMGun();
					}
					if (gunById.Event_StopDrillCrab != null)
					{
						gunById.Event_StopDrillCrab();
						gunById.OutDRGun();
					}
					if (gunById.GetGunType() == FK3_GunType.FK3_LockingGun)
					{
						gunById.StopLock();
					}
					gunById.gameObject.SetActive(value: false);
					gunByID.ClearGunUI();
				}
			}
			else if (forceUpdate || !gunById.gameObject.activeSelf || (gunById.gameObject.activeSelf && !seat.user.nickname.Equals(gunByID.GetName())))
			{
				UnityEngine.Debug.LogError("!isFree");
				gunById.gameObject.SetActive(value: true);
				gunById.Reset_Gun();
				FK3_GunPlayerData fK3_GunPlayerData = new FK3_GunPlayerData();
				fK3_GunPlayerData.gunPower = desk.minGunValue;
				fK3_GunPlayerData.name = seat.user.nickname;
				fK3_GunPlayerData.score = seat.user.gameScore;
				fK3_GunPlayerData.isNative = (seat.id == FK3_GVars.lobby.curSeatId);
				fK3_GunPlayerData.minGunValue = desk.minGunValue;
				fK3_GunPlayerData.maxGunValue = desk.maxGunValue;
				fK3_GunPlayerData.addstepGunValue = desk.addstepGunValue;
				fK3_GunPlayerData.minGold = desk.minGold;
				fK3_GunPlayerData.exchange = desk.exchange;
				fK3_GunPlayerData.onceExchangeValue = desk.onceExchangeValue;
				try
				{
					UnityEngine.Debug.LogError("初始化枪数据: " + JsonMapper.ToJson(fK3_GunPlayerData));
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("错误: " + arg);
				}
				UnityEngine.Debug.LogError("初始化的枪: " + gunById.name);
				gunById.SetPlayerData(fK3_GunPlayerData);
				gunByID.SetPlayerData(fK3_GunPlayerData);
				UnityEngine.Debug.Log(string.Format("Seat[{0}] is {1}", seat.id, fK3_GunPlayerData.isNative ? "native" : "not native"));
			}
		}

		private void Init_Game()
		{
			FK3_FishMgr.Get().Event_OnFishCreate_Handler = HookFishCreate;
			RefreshNativeUserGameNumber(userGameScore, FK3_GVars.user.expeGold, FK3_GVars.user.gameGold);
			Judge_GameScore_AllowShoot();
			Judge_GameScore_CoinInOut();
			StartCoroutine(IE_Test_Crab());
		}

		private void ShowUserInfo(int index)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Red("index: " + index));
			if (FK3_GVars.lobby.curSeatId != index)
			{
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/playerInfo", new object[2]
				{
					FK3_GVars.lobby.curDeskId,
					index
				});
			}
		}

		private void OpenChatWindow(int seatId)
		{
			FK3_SimpleSingletonBehaviour<FK3_SendChatController>.Get().ShowChatWindow(seatId);
		}

		private void ShowInOutScorePanel(bool isShow)
		{
			if (isShow)
			{
				FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get().Show_InOutPanel();
			}
		}

		private void HookFishCreate(FK3_FishBehaviour fish)
		{
			if (fish.isLightning)
			{
				GameObject gmObj = UnityEngine.Object.Instantiate(effectConfig.LightingYellowBall, fish.transform);
				gmObj.transform.SetParent(fish.transform);
				gmObj.transform.localPosition = Vector3.zero;
				Vector3 position = fish.GetPosition();
				gmObj.transform.SetPosition(position);
				float num = Mathf.Clamp(fish.radius * 2.5f, 0.6f, 9f);
				gmObj.transform.localScale = num * 0.7f * Vector3.one;
				gmObj.name = "LightingYellowBall";
				fish.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fish.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
				{
					UnityEngine.Object.DestroyImmediate(gmObj);
				});
			}
		}

		private void Init_Context()
		{
			FK3_GVars.SetGameContext(m_context);
			m_context.curDesk = FK3_GVars.lobby.GetCurDesk();
			m_context.curSeatId = FK3_GVars.lobby.curSeatId;
			m_uiContext = FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().GetContext();
			UnityEngine.Debug.LogError("=========curRoomId:" + FK3_GVars.lobby.curRoomId);
			m_uiContext.isExpeGold = (FK3_GVars.lobby.curRoomId == 1);
		}

		private void Judge_GameScore_AllowShoot()
		{
			FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			if (!(fK3_GunBehaviour == null))
			{
				if (m_uiContext.curScore == 0)
				{
					fK3_GunBehaviour.allowShoot = false;
				}
				else
				{
					fK3_GunBehaviour.allowShoot = true;
				}
			}
		}

		private void Judge_GameScore_CoinInOut()
		{
			if (m_uiContext.curScore == 0)
			{
				FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().EventHandler_UI_PanelCoinInOut_OnHide = delegate
				{
					Judge_GameScore_AllowShoot();
				};
				Option.SetActive(value: true);
			}
		}

		private void PreInit_UIEventHandler()
		{
			FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().Reset_EventHandler();
			FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().EventHandler_UI_OnBtnCoinInClick += Handle_UI_BtnCoinIn;
			FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().EventHandler_UI_OnBtnCoinOutClick += Handle_UI_BtnCoinOut;
			FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get().Reset_EventHandler();
			FK3_OptionController fK3_OptionController = FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get();
			fK3_OptionController.Event_OnBackToLobby = (Action)Delegate.Combine(fK3_OptionController.Event_OnBackToLobby, new Action(Handle_BackToLobby));
		}

		private void _NetMsg_CheckErrorLog(string method, FK3_NetMsgInfoBase info)
		{
			if (!info.valid || info.code != 0)
			{
				UnityEngine.Debug.LogError($"{method}: code:[{info.code}], message:[{info.message}]");
			}
		}

		private void HandleNetMsg_Default(string method, object[] args)
		{
			if (method == "userService/askForFishFormation")
			{
				UnityEngine.Debug.LogError("炸弹蟹2下位置 " + JsonMapper.ToJson(args));
			}
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			_NetMsg_CheckErrorLog(method, fK3_NetMsgInfoBase);
		}

		private void StopHandleNewFish(FK3_KeyValueInfo keyValueInfo)
		{
			stopHandleNewFish = true;
			stopHandleInterval = 2f;
		}

		private void HandleNetMsg_NewFish(object[] args)
		{
			if (stopHandleNewFish || debugFish.IgnoreNewFish)
			{
				return;
			}
			FK3_NetMsgInfo_RecvNewFish fK3_NetMsgInfo_RecvNewFish = new FK3_NetMsgInfo_RecvNewFish(args);
			try
			{
				fK3_NetMsgInfo_RecvNewFish.Parse();
				if (newFishCount++ >= int.MaxValue)
				{
					return;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (fK3_NetMsgInfo_RecvNewFish.valid && fK3_NetMsgInfo_RecvNewFish.code == 0)
			{
				for (int i = 0; i < fK3_NetMsgInfo_RecvNewFish.items.Count; i++)
				{
					FK3_FishQueenData fK3_FishQueenData = new FK3_FishQueenData();
					FK3_FP_Sequence fK3_FP_Sequence = new FK3_FP_Sequence();
					FK3_FG_SingleType<FK3_FishType> fK3_FG_SingleType = new FK3_FG_SingleType<FK3_FishType>();
					fK3_FG_SingleType.type = (FK3_FishType)fK3_NetMsgInfo_RecvNewFish.items[i].fishType;
					fK3_FG_SingleType.count = 1;
					fK3_FishQueenData.delay = 0f;
					fK3_FishQueenData.generator = fK3_FG_SingleType;
					fK3_FishQueenData.speed = FK3_FishQueenMgr.Get().GetSpeedByFishType(fK3_FG_SingleType.type.BasicType());
					fK3_FP_Sequence.pathId = fK3_NetMsgInfo_RecvNewFish.items[i].pathID;
					FK3_FishQueen fishQueen = FK3_FishQueenMgr.Get().GetFishQueen();
					fishQueen.setupFishAction = FishBindEventHandler;
					fishQueen.Setup(fK3_FishQueenData, fK3_FP_Sequence, 1, fK3_NetMsgInfo_RecvNewFish.items[i].fishID);
					fishQueen.Play();
				}
				CanFride();
			}
			else
			{
				_NetMsg_CheckErrorLog("newFish", fK3_NetMsgInfo_RecvNewFish);
			}
		}

		private void HandleNetMsg_NewFishGroup(object[] args)
		{
			if (stopHandleNewFish)
			{
				return;
			}
			try
			{
				CanFride();
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			if (debugFish.IgnoreNewGroup)
			{
				UnityEngine.Debug.LogError("====================被返回了!!===================");
				return;
			}
			FK3_NetMsgInfo_RecvNewFishGroup fK3_NetMsgInfo_RecvNewFishGroup = new FK3_NetMsgInfo_RecvNewFishGroup(args);
			fK3_NetMsgInfo_RecvNewFishGroup.Parse();
			if (fK3_NetMsgInfo_RecvNewFishGroup.valid && fK3_NetMsgInfo_RecvNewFishGroup.code == 0)
			{
				item = fK3_NetMsgInfo_RecvNewFishGroup.item;
				if (item.groupID == 1 || item.groupID == 2)
				{
					fishQueenData = new FK3_FishQueenData();
					FK3_FP_Team fK3_FP_Team = new FK3_FP_Team();
					if (item.groupID == 1)
					{
						UnityEngine.Debug.LogError("item.groupID == 1 迦魶鱼");
						fishQueenData.generator = new FK3_FG_SingleType<FK3_FishType>
						{
							type = FK3_FishType.Gurnard_迦魶鱼,
							count = item.fishCount
						};
					}
					else
					{
						FK3_FG_FixedSequence<FK3_FishType> fK3_FG_FixedSequence = new FK3_FG_FixedSequence<FK3_FishType>();
						FK3_FishType fK3_FishType = FK3_FishType.Lightning_Gurnard_闪电迦魶鱼;
						FK3_FishType fK3_FishType2 = FK3_FishType.Gurnard_迦魶鱼;
						List<FK3_FishType> list = new List<FK3_FishType>();
						list.Add(fK3_FishType);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						list.Add(fK3_FishType2);
						List<FK3_FishType> list2 = fK3_FG_FixedSequence.types = list;
						fishQueenData.generator = fK3_FG_FixedSequence;
					}
					try
					{
						fishQueenData.delay = 0f;
						fishQueenData.speed = FK3_FishQueenMgr.Get().GetSpeedByFishType(FK3_FishType.Gurnard_迦魶鱼);
						fK3_FP_Team.pathId = item.pathID;
						fK3_FP_Team.teamId = 1;
						FK3_FishQueen fishQueen = FK3_FishQueenMgr.Get().GetFishQueen();
						fishQueen.setupFishAction = FishBindEventHandler;
						fishQueen.Setup(fishQueenData, fK3_FP_Team, item.fishCount, item.startFishID);
						fishQueen.Play();
					}
					catch (Exception arg2)
					{
						UnityEngine.Debug.LogError("错误: " + arg2);
					}
				}
				else
				{
					if (item.groupID != 3 && item.groupID != 4)
					{
						return;
					}
					if (!isPlayFishGroup)
					{
						isPlayFishGroup = true;
						GC.Collect();
						if (waitFishGroup != null)
						{
							StopCoroutine(waitFishGroup);
						}
						waitFishGroup = StartCoroutine(WaitFishGroup(item));
					}
					else
					{
						UnityEngine.Debug.LogError("鱼阵执行中,稍后执行");
					}
				}
			}
			else
			{
				UnityEngine.Debug.LogError("======================鱼队错误===================");
				_NetMsg_CheckErrorLog("newFishGroup", fK3_NetMsgInfo_RecvNewFishGroup);
			}
		}

		private IEnumerator WaitFishGroup(FK3_NewFishGroupItemInfo item)
		{
			yield return wait;
			Func<int, FK3_FishType, FK3_FishType> func = (int _index, FK3_FishType _type) => (_index == 0) ? FK3_FishType.Lightning_Rasbora_闪电鲽鱼 : _type;
			PlayFormation(item.pathID, item.startFishID, 0f, (item.groupID == 4) ? func : null);
			yield return wait2;
			isPlayFishGroup = false;
		}

		private void HandleNetMsg_NewFishFormation(object[] args)
		{
			if (!stopHandleNewFish)
			{
				try
				{
					UnityEngine.Debug.LogError("新的鱼组2: " + JsonMapper.ToJson(args));
					CanFride();
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("错误: " + arg);
				}
				GC.Collect();
				if (waitFishGroup2 != null)
				{
					StopCoroutine(waitFishGroup2);
				}
				waitFishGroup2 = StartCoroutine(WaitNewFishFormation2(args));
				isPlayFishGroup = true;
			}
		}

		private IEnumerator WaitNewFishFormation2(object[] args)
		{
			yield return wait;
			try
			{
				FK3_NetMsgInfo_RecvNewFishFormation fK3_NetMsgInfo_RecvNewFishFormation = new FK3_NetMsgInfo_RecvNewFishFormation(args);
				fK3_NetMsgInfo_RecvNewFishFormation.Parse();
				if (fK3_NetMsgInfo_RecvNewFishFormation.valid && fK3_NetMsgInfo_RecvNewFishFormation.code == 0)
				{
					for (int i = 0; i < fK3_NetMsgInfo_RecvNewFishFormation.items.Count; i++)
					{
						bool useOffset = false;
						Vector3 offset = Vector3.zero;
						if (fK3_NetMsgInfo_RecvNewFishFormation.items[i].posX != 0f || fK3_NetMsgInfo_RecvNewFishFormation.items[i].posY != 0f)
						{
							useOffset = true;
							offset = new Vector3(fK3_NetMsgInfo_RecvNewFishFormation.items[i].posX, fK3_NetMsgInfo_RecvNewFishFormation.items[i].posY, 0f);
						}
						PlayFormation(2, fK3_NetMsgInfo_RecvNewFishFormation.items[i].startFishID, fK3_NetMsgInfo_RecvNewFishFormation.startTime, useOffset, offset);
					}
				}
				else
				{
					_NetMsg_CheckErrorLog("newFishFormation", fK3_NetMsgInfo_RecvNewFishFormation);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			yield return wait2;
			isPlayFishGroup = false;
		}

		private void PlayFormation(int formationId, int startId, float startTime = 0f, Func<int, FK3_FishType, FK3_FishType> generatorFunc = null)
		{
			PlayFormation(formationId, startId, startTime, useOffset: false, Vector3.zero, generatorFunc);
		}

		private void PlayFormation(int formationId, int startId, float startTime, bool useOffset, Vector3 offset, Func<int, FK3_FishType, FK3_FishType> generatorFunc = null)
		{
			FK3_fiSimpleSingletonBehaviour<FK3_FormationMgr<FK3_FishType>>.Get().PlayFormation(new FK3_FormationPlayParam<FK3_FishType>
			{
				formationId = formationId,
				startId = startId,
				hasOffset = useOffset,
				offset = offset,
				generatorFunc = generatorFunc
			});
		}

		private void HandleNetMsg_NewPuffer(object[] args)
		{
			if (stopHandleNewFish)
			{
				return;
			}
			UnityEngine.Debug.Log("HandleNetMsg_NewPuffer");
			if (debugFish.IgnoreNewFish)
			{
				return;
			}
			FK3_NetMsgInfo_RecvNewPuffer fK3_NetMsgInfo_RecvNewPuffer = new FK3_NetMsgInfo_RecvNewPuffer(args);
			try
			{
				fK3_NetMsgInfo_RecvNewPuffer.Parse();
				if (newFishCount++ >= int.MaxValue)
				{
					return;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (fK3_NetMsgInfo_RecvNewPuffer.valid && fK3_NetMsgInfo_RecvNewPuffer.code == 0)
			{
				for (int i = 0; i < fK3_NetMsgInfo_RecvNewPuffer.items.Count; i++)
				{
					FK3_FishQueenData fK3_FishQueenData = new FK3_FishQueenData();
					FK3_FP_Sequence fK3_FP_Sequence = new FK3_FP_Sequence();
					FK3_FG_SingleType<FK3_FishType> fK3_FG_SingleType = new FK3_FG_SingleType<FK3_FishType>();
					fK3_FG_SingleType.type = (FK3_FishType)fK3_NetMsgInfo_RecvNewPuffer.items[i].fishType;
					fK3_FG_SingleType.count = 1;
					fK3_FishQueenData.delay = 0f;
					fK3_FishQueenData.generator = fK3_FG_SingleType;
					fK3_FishQueenData.speed = FK3_FishQueenMgr.Get().GetSpeedByFishType(fK3_FG_SingleType.type.BasicType());
					fK3_FP_Sequence.pathId = fK3_NetMsgInfo_RecvNewPuffer.items[i].pathID;
					FK3_FishQueen fishQueen = FK3_FishQueenMgr.Get().GetFishQueen();
					fishQueen.setupFishAction = FishBindEventHandler;
					fishQueen.Setup(fK3_FishQueenData, fK3_FP_Sequence, 1, fK3_NetMsgInfo_RecvNewPuffer.items[i].fishID, fK3_NetMsgInfo_RecvNewPuffer.items[i].openTime);
					fishQueen.Play();
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("newFish", fK3_NetMsgInfo_RecvNewPuffer);
			}
		}

		private void HandleNetMsg_NewKingCrabBoss(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_NewKingCrabBoss");
			if (debugFish.IgnoreNewKingCrab)
			{
				return;
			}
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			try
			{
				fK3_NetMsgInfoBase.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int num = (int)fK3_NetMsgInfoBase.basicDic["fishId"];
				if (!(FK3_FishMgr.Get().GetFishById(num) != null))
				{
					int num2 = (int)fK3_NetMsgInfoBase.basicDic["seed"];
					int num3 = (int)fK3_NetMsgInfoBase.basicDic["survivalTime"];
					FK3_FishMgr.Get().SetFishIndex(num);
					FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().SpawnSingleFish(FK3_FishType.Boss_Crab_霸王蟹);
					FishBindEventHandler(fK3_FishBehaviour);
					FK3_BossCrabMovmentController controller = fK3_FishBehaviour.gameObject.GetComponent<FK3_BossCrabMovmentController>();
					controller.enabled = true;
					float num4 = (float)num2 + (float)num3 / 1000f;
					UnityEngine.Debug.Log($"seed:[{num2}],survivalTime:[{num3}],arg:[{num4}]");
					controller.SetStartArg(num4);
					FK3_FishBehaviour fK3_FishBehaviour2 = fK3_FishBehaviour;
					fK3_FishBehaviour2.Event_FishDying_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDying_Handler, (Action<FK3_FishBehaviour>)delegate
					{
						controller.enabled = false;
					});
					controller.Play();
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("NewKingCrabBoss", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_NewDeepSeaOctopusBossPush(object[] args)
		{
			if (debugFish.IgnoreNewDeepSeaOctopusBoss)
			{
				return;
			}
			FK3_NetMsgInfo_RecvNewDeepSeaOctopus fK3_NetMsgInfo_RecvNewDeepSeaOctopus = new FK3_NetMsgInfo_RecvNewDeepSeaOctopus(args);
			try
			{
				fK3_NetMsgInfo_RecvNewDeepSeaOctopus.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (fK3_NetMsgInfo_RecvNewDeepSeaOctopus.valid && fK3_NetMsgInfo_RecvNewDeepSeaOctopus.code == 0)
			{
				int fishID = fK3_NetMsgInfo_RecvNewDeepSeaOctopus.item.fishID;
				if (!(FK3_FishMgr.Get().GetFishById(fishID) != null))
				{
					int pathID = fK3_NetMsgInfo_RecvNewDeepSeaOctopus.item.pathID;
					int remainTime = fK3_NetMsgInfo_RecvNewDeepSeaOctopus.item.remainTime;
					float num = (float)remainTime / 1000f;
					num -= 2f;
					if (!(num <= 0f))
					{
						FK3_FishMgr.Get().SetFishIndex(fishID);
						FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().SpawnSingleFish(FK3_FishType.Boss_Kraken_深海八爪鱼);
						FishBindEventHandler(fK3_FishBehaviour);
						num -= 1f;
						fK3_FishBehaviour.GetComponent<FK3_KrakenBeheviour>().SetPositionAndRemainTime(pathID, num);
					}
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("newDeepSeaOctopusBossPush", fK3_NetMsgInfo_RecvNewDeepSeaOctopus);
			}
		}

		private IEnumerator IE_Test_Crab()
		{
			yield break;
		}

		private void GenCrab(int fishId, int seed, int survivalTime)
		{
			FK3_FishMgr.Get().SetFishIndex(fishId);
			FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().SpawnSingleFish(FK3_FishType.Boss_Crab_霸王蟹);
			FishBindEventHandler(fK3_FishBehaviour);
			FK3_BossCrabMovmentController controller = fK3_FishBehaviour.gameObject.GetComponent<FK3_BossCrabMovmentController>();
			controller.enabled = true;
			FK3_FishBehaviour fK3_FishBehaviour2 = fK3_FishBehaviour;
			fK3_FishBehaviour2.Event_FishDying_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDying_Handler, (Action<FK3_FishBehaviour>)delegate
			{
				controller.enabled = false;
			});
			float startArg = (float)seed + (float)survivalTime / 1000f;
			controller.SetStartArg(startArg);
			controller.Play();
		}

		private void HandleNetMsg_ChangeStage(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_ChangeStage");
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int newScenarioType = (int)fK3_NetMsgInfoBase.basicDic["scene"];
				int stage = (int)fK3_NetMsgInfoBase.basicDic["stage"];
				int stageType = (int)fK3_NetMsgInfoBase.basicDic["stageType"];
				GunStatusControl(stage);
				FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ScenarioLogic((FK3_ScenarioType)newScenarioType, stage, stageType);
			}
			else
			{
				_NetMsg_CheckErrorLog("changeStage", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_PlayerScenePush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_PlayerScenePush");
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int num = (int)fK3_NetMsgInfoBase.basicDic["scene"];
				int stage = (int)fK3_NetMsgInfoBase.basicDic["stage"];
				int stageType = (int)fK3_NetMsgInfoBase.basicDic["stageType"];
				int num2 = (int)fK3_NetMsgInfoBase.basicDic["sceneDuration"];
				int stageDuration = (int)fK3_NetMsgInfoBase.basicDic["stageDuration"];
				GunStatusControl(stage, num2);
				FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().RefreshScenario((FK3_ScenarioType)num, stage, stageType);
				FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().SetStage((FK3_ScenarioType)num, stage, stageType, num2, stageDuration);
			}
			else
			{
				_NetMsg_CheckErrorLog("playerScenePush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserFired(object[] args)
		{
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int gameScore = (int)fK3_NetMsgInfoBase.basicDic["gameScore"];
				RefreshUserScore(m_context.curSeatId, gameScore);
			}
			else
			{
				_NetMsg_CheckErrorLog("userFired", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_NotFired(object[] args)
		{
			UnityEngine.Debug.LogError("停止开炮 " + JsonMapper.ToJson(args));
			InGameDropped();
		}

		private void HandleNetMsg_UserRotateEM(object[] args)
		{
			HandleNetMsg_Default("userRotateEM", args);
		}

		private void HandleNetMsg_UserRotateDR(object[] args)
		{
			HandleNetMsg_Default("userRotateDR", args);
		}

		private void HandleNetMsg_UserRotateEMPush(object[] args)
		{
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				float angle = float.Parse(string.Empty + fK3_NetMsgInfoBase.basicDic["angle"]);
				int id = (int)fK3_NetMsgInfoBase.basicDic["seatId"];
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(id);
				gunById.RotateEMGun(angle);
			}
			else
			{
				_NetMsg_CheckErrorLog("userRotateEMPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserRotateDRPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UserRotateDRPush");
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				float angle = float.Parse(string.Empty + fK3_NetMsgInfoBase.basicDic["angle"]);
				int id = (int)fK3_NetMsgInfoBase.basicDic["seatId"];
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(id);
				gunById.RotateDRGun(angle);
			}
			else
			{
				_NetMsg_CheckErrorLog("userRotateDRPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserFiredEM(object[] args)
		{
			HandleNetMsg_Default("userFiredEM", args);
		}

		private void HandleNetMsg_UserFiredDR(object[] args)
		{
			HandleNetMsg_Default("userFiredDrill", args);
		}

		private void HandleNetMsg_UserFiredEMPush(object[] args)
		{
			UnityEngine.Debug.LogError("电磁炮开炮: " + JsonMapper.ToJson(args));
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				float angle = float.Parse(string.Empty + fK3_NetMsgInfoBase.basicDic["angle"]);
				int id = (int)fK3_NetMsgInfoBase.basicDic["seatId"];
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(id);
				gunById.RotateEMGun(angle);
				gunById.RemoteShootEM();
			}
			else
			{
				_NetMsg_CheckErrorLog("userFiredEMPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserFiredDRPush(object[] args)
		{
			UnityEngine.Debug.LogError("钻头炮开炮: " + JsonMapper.ToJson(args));
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				float angle = float.Parse(string.Empty + fK3_NetMsgInfoBase.basicDic["angle"]);
				int id = (int)fK3_NetMsgInfoBase.basicDic["seatId"];
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(id);
				FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
				gunById.RotateDRGun(angle);
				gunById.RemoteShootDR();
				if (gunById.GetId() != fK3_GunBehaviour.GetId())
				{
					UnityEngine.Debug.LogError("开炮!");
					gunById.Event_ShootDRPush_Handler = (FK3_GunBehaviour.EventHandler_ShootDR)Delegate.Combine(gunById.Event_ShootDRPush_Handler, new FK3_GunBehaviour.EventHandler_ShootDR(GunLogic_Handle_GunShootDRPush));
					gunById.ShootDRPush(angle, id);
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("userFiredDrillPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserFiredPush(object[] args)
		{
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int gameScore = (int)fK3_NetMsgInfoBase.basicDic["gameScore"];
				string text = (string)fK3_NetMsgInfoBase.basicDic["fired"];
				string[] array = text.Split('#');
				numUserFired = int.Parse(array[0]);
				powerUserFired = int.Parse(array[1]);
				angleUserFired = float.Parse(array[2]);
				num2UserFired = int.Parse(array[3]);
				num3UserFired = int.Parse(array[4]);
				RefreshUserScore(num3UserFired, gameScore);
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(num3UserFired);
				FK3_GunUIController gunByID = FK3_GunUIMgr.Get().GetGunByID(num3UserFired);
				otherGunValue = powerUserFired;
				gunById.SetPower(powerUserFired);
				gunByID.SetPower(powerUserFired);
				gunById.RotateNormalGun(angleUserFired);
				FK3_GVars.NowGunSkin[num3UserFired - 1] = num2UserFired;
				if (gunById.GetGunType() != 0 && FK3_GVars.NowGunSkin[num3UserFired - 1] == 0)
				{
					gunById.ChangeGunRemote(FK3_GunType.FK3_NormalGun);
				}
				if (FK3_GVars.NowGunSkin[num3UserFired - 1] != 0 && gunById.GetGunType() != FK3_GunType.FK3_GunGunn)
				{
					gunById.ChangeGunRemote(FK3_GunType.FK3_GunGunn);
				}
				gunById.Shoot();
			}
			else
			{
				_NetMsg_CheckErrorLog("userFiredPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_BulletHitFish(object[] args)
		{
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				string text = (string)fK3_NetMsgInfoBase.basicDic["deadFishs"];
				string[] array = text.Split(',');
				int bulletId = int.Parse(array[0]);
				int bulletPower = int.Parse(array[1]);
				int num = int.Parse(array[2]);
				int addScore = int.Parse(array[3]);
				int num2 = int.Parse(array[4]);
				RefreshUserScore(num, num2);
				if (array.Length == 6 && array[5].Length > 0)
				{
					string text2 = array[5];
					string[] array2 = text2.Split('|');
					string[] array3 = array2;
					foreach (string text3 in array3)
					{
						string[] array4 = text3.Split('#');
						int fishId = int.Parse(array4[0]);
						int fishType = int.Parse(array4[1]);
						int fishRate = int.Parse(array4[2]);
						DoFishDead(new FK3_FishDeadInfo
						{
							fishId = fishId,
							fishType = fishType,
							fishRate = fishRate,
							killerSeatId = num,
							bulletId = bulletId,
							bulletPower = bulletPower,
							addScore = addScore,
							newScore = num2,
							deadWay = 1
						});
					}
				}
				else
				{
					UnityEngine.Debug.LogWarning("死鱼失败！");
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("bulletHitFish", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_GameGold(object[] args)
		{
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int curGold = (int)fK3_NetMsgInfoBase.basicDic["gameGold"];
				m_uiContext.curGold = curGold;
				RefreshNativeUserGameNumber(m_uiContext.curScore, FK3_GVars.user.expeGold, m_uiContext.curGold);
			}
			else
			{
				_NetMsg_CheckErrorLog("gameGold", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserCoinIn(object[] args)
		{
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			try
			{
				fK3_NetMsgInfoBase.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int gameScore = (int)fK3_NetMsgInfoBase.basicDic["gameScore"];
				int expeGold = (int)fK3_NetMsgInfoBase.basicDic["expeGold"];
				int gameGold = (int)fK3_NetMsgInfoBase.basicDic["gameGold"];
				RefreshNativeUserGameNumber(gameScore, expeGold, gameGold);
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("玩家上分音效");
			}
			else
			{
				_NetMsg_CheckErrorLog("userCoinIn", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserCoinOut(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UserCoinOut");
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			try
			{
				fK3_NetMsgInfoBase.Parse();
				if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
				{
					int gameScore = (int)fK3_NetMsgInfoBase.basicDic["gameScore"];
					int expeGold = (int)fK3_NetMsgInfoBase.basicDic["expeGold"];
					int gameGold = (int)fK3_NetMsgInfoBase.basicDic["gameGold"];
					RefreshNativeUserGameNumber(gameScore, expeGold, gameGold);
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("玩家上分音效");
				}
				else
				{
					_NetMsg_CheckErrorLog("userCoinOut", fK3_NetMsgInfoBase);
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		private void HandleNetMsg_GunHitFishInAction(object[] args)
		{
			UnityEngine.Debug.LogError("连锁锁鱼 " + JsonMapper.ToJson(args));
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int num = (int)fK3_NetMsgInfoBase.basicDic["actionId"];
				int num2 = (int)fK3_NetMsgInfoBase.basicDic["actionFlag"];
				string text = (string)fK3_NetMsgInfoBase.basicDic["deadFishs"];
				List<FK3_DeadFishData> list = new List<FK3_DeadFishData>();
				if (text.Length > 0)
				{
					string[] array = text.Split(',');
					int seatId = int.Parse(array[0]);
					int num3 = int.Parse(array[1]);
					int num4 = int.Parse(array[2]);
					int gameScore = int.Parse(array[3]);
					string text2 = array[4];
					if (text2.Length > 0)
					{
						string[] array2 = text2.Split('|');
						string[] array3 = array2;
						foreach (string text3 in array3)
						{
							string[] array4 = text3.Split('#');
							UnityEngine.Debug.LogError(" 锁鱼信息2 " + JsonMapper.ToJson(array4));
							int fishId = int.Parse(array4[0]);
							FK3_FishType fK3_FishType = (FK3_FishType)int.Parse(array4[1]);
							int num5 = int.Parse(array4[2]);
							FK3_FishBehaviour fishById = FK3_FishMgr.Get().GetFishById(fishId);
							if (!(null == fishById))
							{
								int score = num5 * num3;
								FK3_DeadFishData fK3_DeadFishData = new FK3_DeadFishData(fishById, num3, num5, score);
								list.Add(fK3_DeadFishData);
							}
						}
					}
					RefreshUserScore(seatId, gameScore);
				}
				switch (num)
				{
				case 10:
					foreach (FK3_DeadFishData item2 in list)
					{
						m_bombCrabAction.totalScore += item2.score;
						if (item2.fish.type == FK3_FishType.GoldShark_霸王鲸 || FK3_FishMgr.bigFishSet.Contains(item2.fish.type))
						{
							m_bombCrabAction.waitBigFishShowTime = 9f;
						}
					}
					m_bombCrabAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 15:
					foreach (FK3_DeadFishData item3 in list)
					{
						m_caymanAction.totalScore += item3.score;
						if (item3.fish.type == FK3_FishType.GoldShark_霸王鲸 || FK3_FishMgr.bigFishSet.Contains(item3.fish.type))
						{
							m_caymanAction.waitBigFishShowTime = 9f;
						}
					}
					m_caymanAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 16:
					m_bossCrabAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 17:
					m_bossLanternAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 18:
					m_bossCrocodileAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 19:
					m_bossKrakenAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 20:
					m_bossDorganAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 11:
					foreach (FK3_DeadFishData item4 in list)
					{
						m_lightingFishAction.totalScore += item4.score;
						if (item4.fish.type == FK3_FishType.GoldShark_霸王鲸 || FK3_FishMgr.bigFishSet.Contains(item4.fish.type))
						{
							m_lightingFishAction.hasBigFish = true;
						}
					}
					m_lightingFishAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 12:
					foreach (FK3_DeadFishData item5 in list)
					{
						m_laserCrabAction.totalScore += item5.score;
						if (FK3_FishMgr.bossFishSet.Contains(item5.fish.type))
						{
							m_laserCrabAction.waitBigFishOrBossShowTime = 11.5f + (float)(item5.fishRate - 100) / 30f * 0.5f;
							m_laserCrabAction.hasBoss = true;
						}
						else if ((item5.fish.type == FK3_FishType.GoldShark_霸王鲸 || FK3_FishMgr.bigFishSet.Contains(item5.fish.type)) && !m_laserCrabAction.hasBoss)
						{
							m_laserCrabAction.waitBigFishOrBossShowTime = 9f;
							m_laserCrabAction.hasBigFish = true;
						}
					}
					m_laserCrabAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 13:
					foreach (FK3_DeadFishData item6 in list)
					{
						m_drillCrabAction.totalScore += item6.score;
						if (FK3_FishMgr.bossFishSet.Contains(item6.fish.type))
						{
							m_drillCrabAction.waitBigFishOrBossShowTime = 11.5f + (float)(item6.fishRate - 100) / 30f * 0.5f;
							m_drillCrabAction.hasBoss = true;
						}
						else if ((item6.fish.type == FK3_FishType.GoldShark_霸王鲸 || FK3_FishMgr.bigFishSet.Contains(item6.fish.type)) && !m_drillCrabAction.hasBoss)
						{
							m_drillCrabAction.waitBigFishOrBossShowTime = 9f;
							m_drillCrabAction.hasBigFish = true;
						}
					}
					m_drillCrabAction.OnActionMsgReturn(num2 == 1, list);
					break;
				}
			}
			else
			{
				m_lightingFishAction.OnActionMsgReturn(canContinue: false, new List<FK3_DeadFishData>());
				_NetMsg_CheckErrorLog("gunHitFishInAction", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_GunHitFishInActionPush(object[] args)
		{
			HandleNetMsg_GunHitFishInAction(args);
		}

		private void HandleNetMsg_LeaveSeat(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_LeaveSeat");
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int num = (int)fK3_NetMsgInfoBase.basicDic["gameGold"];
				int num2 = (int)fK3_NetMsgInfoBase.basicDic["expeGold"];
				int gameScore = (int)fK3_NetMsgInfoBase.basicDic["gameScore"];
				RefreshUserGold(num, num2);
				RefreshNativeUserGameNumber(gameScore, num2, num);
				QuitToRoom();
			}
			else
			{
				_NetMsg_CheckErrorLog("leaveSeat", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_QuitToDesk(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_QuitToDesk");
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int num = (int)fK3_NetMsgInfoBase.basicDic["gameGold"];
				int num2 = (int)fK3_NetMsgInfoBase.basicDic["expeGold"];
				int gameScore = (int)fK3_NetMsgInfoBase.basicDic["gameScore"];
				int num3 = (int)fK3_NetMsgInfoBase.basicDic["quitType"];
				RefreshUserGold(num, num2);
				RefreshNativeUserGameNumber(gameScore, num2, num);
				switch (num3)
				{
				case 1:
					FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("该桌已被删除，请重新选桌", "The table has been deleted, please choose a new table", string.Empty), showOkCancel: false, QuitToRoom);
					break;
				case 2:
					FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("连接异常，请重新选桌", "The connection is abnormal. Please select a new table", string.Empty), showOkCancel: false, QuitToRoom);
					break;
				case 3:
					FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("由于您长时间未操作，已自动退出该桌", "Because you have not operated for a long time, you have automatically quit the table", string.Empty), showOkCancel: false, QuitToRoom);
					break;
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("quitToDesk", fK3_NetMsgInfoBase);
			}
		}

		private void RefreshUserGold(int gold, int exp)
		{
			FK3_GVars.user.gameGold = gold;
			FK3_GVars.user.expeGold = exp;
		}

		private void QuitToRoom()
		{
			FK3_SimpleSingletonBehaviour<FK3_UserInfoInGame>.GetInstance().Hide();
			CleanScene();
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ResetScenario();
			FK3_Singleton<FK3_SoundMgr>.Get().StopAllSounds();
			FK3_MessageCenter.UnRegisterHandle("CheckAutoLock", CheckAutoLock);
			FK3_MessageCenter.UnRegisterHandle("SetCursorUsageLinearSpeed", OnSpeedUpAll);
		}

		private void HandleNetMsg_UpdateDeskInfoPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UpdateDeskInfoPush");
			FK3_NetMsgInfo_UpdateDeskInfoPush fK3_NetMsgInfo_UpdateDeskInfoPush = new FK3_NetMsgInfo_UpdateDeskInfoPush(args);
			fK3_NetMsgInfo_UpdateDeskInfoPush.Parse();
			if (fK3_NetMsgInfo_UpdateDeskInfoPush.valid && fK3_NetMsgInfo_UpdateDeskInfoPush.code == 0)
			{
				FK3_GVars.lobby.inGameSeats = fK3_NetMsgInfo_UpdateDeskInfoPush.seats;
				FK3_DeskInfo curDesk = FK3_GVars.lobby.GetCurDesk();
				foreach (FK3_SeatInfo2 seat in fK3_NetMsgInfo_UpdateDeskInfoPush.seats)
				{
					SetPlayerGun(curDesk, seat);
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("updateDeskInfoPush", fK3_NetMsgInfo_UpdateDeskInfoPush);
			}
		}

		private void HandleNetMsg_AskForFishFormation(object[] args)
		{
			HandleNetMsg_Default("askForFishFormation", args);
		}

		private void HandleNetMsg_ChangeGun(object[] args)
		{
			HandleNetMsg_Default("changeGun", args);
		}

		private void HandleNetMsg_ChangeGunPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_ChangeGunPush");
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int id = (int)fK3_NetMsgInfoBase.basicDic["seatId"];
				int targetType = (int)fK3_NetMsgInfoBase.basicDic["gunType"];
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(id);
				if (!gunById.IsNative())
				{
					gunById.ChangeGunRemote((FK3_GunType)targetType);
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("changeGunPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_ChangeSkin(object[] args)
		{
			UnityEngine.Debug.LogError("收到换皮" + JsonMapper.ToJson(args));
		}

		private void HandleNetMsg_ChangeSkinPush(object[] args)
		{
			UnityEngine.Debug.LogError("收到换皮Push" + JsonMapper.ToJson(args));
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int id = (int)fK3_NetMsgInfoBase.basicDic["seatId"];
				int num = (int)fK3_NetMsgInfoBase.basicDic["gunType"];
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(id);
				if (!gunById.IsNative())
				{
					gunById.ChangeSkinRemote((num != 0) ? FK3_GunType.FK3_LockingGun : FK3_GunType.FK3_NormalGun, id);
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("changeSkinPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_GunHitLockFish(object[] args)
		{
			HandleNetMsg_Default("gunHitLockFish", args);
		}

		private void HandleNetMsg_BulletHitLockFishPush(object[] args)
		{
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				string text = fK3_NetMsgInfoBase.basicDic["deadFishs"] as string;
				string[] array = text.Split(',');
				int bulletId = int.Parse(array[0]);
				int bulletPower = int.Parse(array[1]);
				int num = int.Parse(array[2]);
				int addScore = int.Parse(array[3]);
				int num2 = int.Parse(array[4]);
				RefreshUserScore(num, num2);
				FK3_GunUIController gunByID = FK3_GunUIMgr.Get().GetGunByID(num);
				if (array.Length != 6 || array[5].Length <= 0)
				{
					return;
				}
				string text2 = array[5];
				string[] array2 = text2.Split('-');
				string[] array3 = array2;
				foreach (string text3 in array3)
				{
					string[] array4 = text3.Split('#');
					if (array4.Length == 3)
					{
						int fishId = int.Parse(array4[0]);
						int fishType = int.Parse(array4[1]);
						int fishRate = int.Parse(array4[2]);
						DoFishDead(new FK3_FishDeadInfo
						{
							fishId = fishId,
							fishType = fishType,
							fishRate = fishRate,
							killerSeatId = num,
							bulletId = bulletId,
							bulletPower = bulletPower,
							addScore = addScore,
							newScore = num2,
							deadWay = 2
						});
					}
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("changeGunPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_LockSelectFish(object[] args)
		{
			HandleNetMsg_Default("lockSelectFish", args);
		}

		private void HandleNetMsg_LockSelectFishPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_LockSelectFishPush");
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				int id = (int)fK3_NetMsgInfoBase.basicDic["seatId"];
				int fishId = (int)fK3_NetMsgInfoBase.basicDic["fishId"];
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(id);
				FK3_FishBehaviour fishById = FK3_FishMgr.Get().GetFishById(fishId);
				FK3_GunUIController gunByID = FK3_GunUIMgr.Get().GetGunByID(id);
				int num = (int)fK3_NetMsgInfoBase.basicDic["gunValue"];
				gunByID.SetPower(num);
				UnityEngine.Debug.LogError("锁定炮值:" + num);
				if (!(gunById == null) && !(fishById == null) && !gunById.IsNative())
				{
					if (gunById.GetGunType() != FK3_GunType.FK3_LockingGun)
					{
						gunById.ChangeGunRemote(FK3_GunType.FK3_LockingGun);
					}
					gunById.LockFish(fishById);
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("锁定炮连接2", loop: true);
					FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("锁定炮连接2", 0.25f);
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("lockSelectFishPush", fK3_NetMsgInfoBase);
			}
		}

		private void HandleNetMsg_BombCrabNextPoint(object[] args)
		{
			UnityEngine.Debug.LogError("炸弹蟹下一位置 " + JsonMapper.ToJson(args));
			HandleNetMsg_Default("bombCrabNextPoint", args);
		}

		private void HandleNetMsg_BombCrabNextPointPush(object[] args)
		{
			UnityEngine.Debug.LogError("返回位置: " + JsonMapper.ToJson(args));
			FK3_NetMsgInfoBase fK3_NetMsgInfoBase = new FK3_NetMsgInfoBase(args);
			fK3_NetMsgInfoBase.Parse();
			if (fK3_NetMsgInfoBase.valid && fK3_NetMsgInfoBase.code == 0)
			{
				float x = float.Parse(fK3_NetMsgInfoBase.basicDic["x"].ToString());
				float y = float.Parse(fK3_NetMsgInfoBase.basicDic["y"].ToString());
				int num = (int)fK3_NetMsgInfoBase.basicDic["seatId"];
				m_bombCrabAction.OnMsgBombCrabNextPointPush(x, y);
			}
			else
			{
				_NetMsg_CheckErrorLog("bombCrabNextPointPush", fK3_NetMsgInfoBase);
			}
		}

		private void FishBindEventHandler(FK3_FishBehaviour fish)
		{
			fish.Event_FishOnDoubleClick_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fish.Event_FishOnDoubleClick_Handler, new Action<FK3_FishBehaviour>(Handle_FishOnDoubleClick));
			fish.Event_FishOnHit_Handler += Handle_FishHitByBullet;
		}

		private void DoFishDead(FK3_FishDeadInfo deadInfo)
		{
			FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().GetFishById(deadInfo.fishId);
			if (fK3_FishBehaviour == null)
			{
				UnityEngine.Debug.LogError($"fish[id:{deadInfo.fishId}] 未找到1.忽视!");
				fK3_FishBehaviour = FK3_FishMgr.Get().GetFishFin(deadInfo.fishId);
				if (fK3_FishBehaviour == null)
				{
					UnityEngine.Debug.LogError($"fish[id:{deadInfo.fishId}] 未找到2.忽视!");
					return;
				}
				UnityEngine.Debug.LogError($"fish[id:{deadInfo.fishId}] 找到了!");
			}
			if (fK3_FishBehaviour.type != (FK3_FishType)deadInfo.fishType)
			{
				UnityEngine.Debug.LogError("死鱼类型不匹配! " + fK3_FishBehaviour.type);
			}
			if (fK3_FishBehaviour.isLightning)
			{
				Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction = delegate(FK3_FishBehaviour _fish, FK3_FishDeadInfo _deadInfo)
				{
					FK3_KillFishController controllerById6 = FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
					controllerById6.DoDeadFish(new FK3_KillFishData
					{
						fish = _fish,
						fishId = _deadInfo.fishId,
						seatId = deadInfo.killerSeatId,
						fishType = _fish.type
					}, _deadInfo);
				};
				m_lightingFishAction.Play(fK3_FishBehaviour, fishDeadAction, deadInfo.killerSeatId, deadInfo.bulletPower);
				return;
			}
			if (fK3_FishBehaviour.type == FK3_FishType.CrabBoom_连环炸弹蟹)
			{
				Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction2 = delegate(FK3_FishBehaviour _fish, FK3_FishDeadInfo _deadInfo)
				{
					FK3_KillFishController controllerById5 = FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
					controllerById5.DoDeadFish(new FK3_KillFishData
					{
						fish = _fish,
						bulletPower = _deadInfo.bulletPower,
						fishId = _deadInfo.fishId,
						seatId = deadInfo.killerSeatId,
						fishType = _fish.type
					}, _deadInfo);
				};
				m_bombCrabAction.Play(fK3_FishBehaviour, fishDeadAction2, deadInfo.killerSeatId, deadInfo.bulletPower);
				return;
			}
			if (fK3_FishBehaviour.type == FK3_FishType.CrabLaser_电磁蟹)
			{
				Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction3 = delegate(FK3_FishBehaviour _fish, FK3_FishDeadInfo _deadInfo)
				{
					FK3_KillFishController controllerById4 = FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
					controllerById4.DoDeadFish(new FK3_KillFishData
					{
						fish = _fish,
						bulletPower = _deadInfo.bulletPower,
						fishId = _deadInfo.fishId,
						seatId = deadInfo.killerSeatId,
						fishType = _fish.type
					}, _deadInfo);
				};
				m_laserCrabAction.Play(fK3_FishBehaviour, fishDeadAction3, deadInfo.killerSeatId, deadInfo.bulletPower);
				return;
			}
			if (fK3_FishBehaviour.type == FK3_FishType.CrabDrill_钻头蟹)
			{
				Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction4 = delegate(FK3_FishBehaviour _fish, FK3_FishDeadInfo _deadInfo)
				{
					FK3_KillFishController controllerById3 = FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
					controllerById3.DoDeadFish(new FK3_KillFishData
					{
						fish = _fish,
						bulletPower = _deadInfo.bulletPower,
						fishId = _deadInfo.fishId,
						seatId = deadInfo.killerSeatId,
						fishType = _fish.type
					}, _deadInfo);
				};
				m_drillCrabAction.Play(fK3_FishBehaviour, fishDeadAction4, deadInfo.killerSeatId, deadInfo.bulletPower);
				return;
			}
			if (fK3_FishBehaviour.type == FK3_FishType.烈焰龟)
			{
				Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction5 = delegate(FK3_FishBehaviour _fish, FK3_FishDeadInfo _deadInfo)
				{
					FK3_KillFishController controllerById2 = FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
					controllerById2.DoDeadFish(new FK3_KillFishData
					{
						fish = _fish,
						bulletPower = _deadInfo.bulletPower,
						fishId = _deadInfo.fishId,
						seatId = deadInfo.killerSeatId,
						fishType = _fish.type
					}, _deadInfo);
				};
				m_caymanAction.Play(fK3_FishBehaviour, fishDeadAction5, deadInfo.killerSeatId, deadInfo.bulletPower);
				return;
			}
			FK3_KillFishController controllerById = FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
			controllerById.DoDeadFish(new FK3_KillFishData
			{
				fish = fK3_FishBehaviour,
				fishId = deadInfo.fishId,
				seatId = deadInfo.killerSeatId,
				fishType = fK3_FishBehaviour.type
			}, deadInfo);
			if (fK3_FishBehaviour.type == FK3_FishType.Boss_Crab_霸王蟹)
			{
				UnityEngine.Debug.LogError("特殊鱼 霸王蟹");
				m_bossCrabAction.Play(fK3_FishBehaviour, controllerById, deadInfo.killerSeatId, deadInfo.bulletPower);
			}
			else if (fK3_FishBehaviour.type == FK3_FishType.Boss_Lantern_暗夜炬兽)
			{
				m_bossLanternAction.Play(fK3_FishBehaviour, controllerById, deadInfo.killerSeatId, deadInfo.bulletPower);
			}
			else if (fK3_FishBehaviour.type == FK3_FishType.Boss_Crocodil_史前巨鳄)
			{
				m_bossCrocodileAction.Play(fK3_FishBehaviour, controllerById, deadInfo.killerSeatId, deadInfo.bulletPower);
			}
			else if (fK3_FishBehaviour.type == FK3_FishType.Boss_Kraken_深海八爪鱼)
			{
				m_bossKrakenAction.Play(fK3_FishBehaviour, controllerById, deadInfo.killerSeatId, deadInfo.bulletPower);
			}
			else if (fK3_FishBehaviour.type == FK3_FishType.Boss_Dorgan_冰封暴龙 || fK3_FishBehaviour.type == FK3_FishType.Boss_Dorgan_狂暴火龙)
			{
				m_bossDorganAction.Play(fK3_FishBehaviour, controllerById, deadInfo.killerSeatId, deadInfo.bulletPower);
			}
		}

		private void Handle_FishHitByBullet(FK3_FishBehaviour fish, Collider bullet)
		{
			if (!m_context.isPlaying)
			{
				UnityEngine.Debug.LogError("================isPlaying: " + m_context.isPlaying);
				return;
			}
			FK3_BulletController component = bullet.GetComponent<FK3_BulletController>();
			if (component == null)
			{
				UnityEngine.Debug.LogError("component为空!");
				return;
			}
			if (component.used)
			{
				UnityEngine.Debug.LogError("used: " + component.used);
				return;
			}
			if (fish.State != 0)
			{
				UnityEngine.Debug.Log(fish.name + " fish.State != FishState.Live");
				return;
			}
			component.used = true;
			FK3_Effect_ExplodeMgr.Get().SpawnExplode(bullet.transform.position, component);
			component.Over();
			if (m_useNet && component.seatId == FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun().GetId())
			{
				int bulletId = component.bulletId;
				string text = $"{fish.id}#{(int)fish.type}";
				object[] args = new object[2]
				{
					bulletId,
					text
				};
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFish", args);
			}
		}

		private void GunLogic_Handle_GunRotateEM(float gunAngle)
		{
			object[] args = new object[1]
			{
				gunAngle
			};
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/userRotateEM", args);
		}

		private void GunLogic_Handle_GunShootEM(float gunAngle)
		{
			object[] args = new object[1]
			{
				gunAngle
			};
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/userFiredEM", args);
		}

		private void GunLogic_Handle_GunRotateDR(float gunAngle)
		{
			object[] args = new object[1]
			{
				gunAngle
			};
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/userRotateDrill", args);
		}

		private void GunLogic_Handle_GunShootDR(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId)
		{
			object[] args = new object[1]
			{
				gunAngle
			};
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/userFiredDrill", args);
			FK3_BulletMgr.Get().ShootDR(pos, gunAngle, angleOffset, gunValue, seatId, _bulletId);
		}

		private void GunLogic_Handle_GunShootDRPush(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId)
		{
			object[] array = new object[1]
			{
				gunAngle
			};
			FK3_BulletMgr.Get().ShootDR(pos, gunAngle, angleOffset, gunValue, seatId, _bulletId);
		}

		private void GunLogic_Handle_GunShoot2(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId)
		{
			if (m_context.isPlaying && m_useNet)
			{
				_bulletId++;
				if (gunValue > m_uiContext.curScore)
				{
					gunValue = m_uiContext.curScore;
				}
				int num = 0;
				string text = $"{_bulletId},{gunValue},{gunAngle},{num}";
				object[] args = new object[1]
				{
					text
				};
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/userFired", args);
				FK3_BulletMgr.Get().Shoot(pos, gunAngle, angleOffset, gunValue, seatId, _bulletId);
				RefreshUserScore(m_context.curSeatId, m_uiContext.curScore - gunValue);
				Judge_GameScore_AllowShoot();
			}
		}

		private void GunLogic_Handle_GunLockShoot(FK3_KeyValueInfo keyValueInfo)
		{
			if (m_context.isPlaying)
			{
				FK3_LockGunShootInfo fK3_LockGunShootInfo = JsonUtility.FromJson<FK3_LockGunShootInfo>(keyValueInfo._value.ToString());
				_bulletId++;
				string text = $"{_bulletId},{fK3_LockGunShootInfo._gunValue},{fK3_LockGunShootInfo._gunAngle},{fK3_LockGunShootInfo._gunType}";
				string text2 = $"{fK3_LockGunShootInfo._fishId}#{fK3_LockGunShootInfo._fishType}";
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitLockFish", new object[2]
				{
					text,
					text2
				});
			}
		}

		private void GunLogic_Handle_GunLockSelect(FK3_KeyValueInfo keyValueInfo)
		{
			FK3_LockGunSelectInfo fK3_LockGunSelectInfo = JsonUtility.FromJson<FK3_LockGunSelectInfo>(keyValueInfo._value.ToString());
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("锁定炮连接2", loop: true);
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("锁定炮连接2", 0.25f);
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/lockSelectFish", new object[2]
			{
				fK3_LockGunSelectInfo._fishId,
				fK3_LockGunSelectInfo._gunValue
			});
		}

		private void CheckAutoLock(FK3_KeyValueInfo keyVauleInfo)
		{
			if (nativeGun != null)
			{
				nativeGun.CheckAutoLock();
			}
		}

		private void PlayerLogic_Handle_PlayerChangeGunPower(FK3_PlayerUI playerUI)
		{
			FK3_PlayerController player = playerUI.GetPlayer();
			player.GetGun().UserSwitchGun();
		}

		private void GunLogic_Handle_OnChangeAuto(FK3_GunBehaviour gun, bool isAuto)
		{
			UnityEngine.Debug.Log("GunLogic_Handle_OnChangeAuto");
			SetBtnAuto(isAuto);
			if (gun.CanLocking() && isAuto)
			{
				FK3_LockChainController lockChain = FK3_EffectMgr.Get().GetLockChain(gun.GetId());
				gun.AutoLock(lockChain);
			}
		}

		private void SetBtnAuto(bool isAuto)
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("选座选厅自动发炮");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("选座选厅自动发炮", 1f);
			if (isAuto)
			{
				m_btnAuto.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/auto_1");
				m_btnAuto.transform.Find("VFX_Automatic").gameObject.SetActive(value: true);
			}
			else
			{
				m_btnAuto.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/auto_0");
				m_btnAuto.transform.Find("VFX_Automatic").gameObject.SetActive(value: false);
			}
		}

		private void Handle_UI_BtnChangeGun()
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("切换炮值");
			UnityEngine.Debug.Log("Handle_UI_BtnChangeGun");
			FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun().DoChangeGun(delegate(FK3_GunType from, FK3_GunType to)
			{
				if (from == FK3_GunType.FK3_LockingGun)
				{
					FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun().StopLock();
				}
				if (to == FK3_GunType.FK3_LockingGun)
				{
					FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
					FK3_LockChainController lockChain = FK3_EffectMgr.Get().GetLockChain(fK3_GunBehaviour.GetId());
					fK3_GunBehaviour.CheckAutoLock();
				}
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/changeGun", new object[1]
				{
					(to != 0 || to != FK3_GunType.FK3_GunGunn) ? 1 : 0
				});
			});
		}

		private void Handle_UI_BtnAuto()
		{
			UnityEngine.Debug.LogError("点击自动发pao");
			if (m_uiContext.curScore == 0)
			{
				FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get().Show_InOutPanel();
				return;
			}
			FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			if (fK3_GunBehaviour.IsAuto())
			{
				fK3_GunBehaviour.DoCancelAuto();
			}
			else
			{
				fK3_GunBehaviour.DoAuto();
			}
		}

		private bool GunLogic_CanAuto()
		{
			FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			return true;
		}

		private void Handle_UI_BtnPlusPower(FK3_SpriteButton btn)
		{
			UnityEngine.Debug.Log("Handle_UI_BtnPlusPower");
			FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			FK3_GunUIController gunByID = FK3_GunUIMgr.Get().GetGunByID(FK3_GVars.lobby.curSeatId);
			int power = fK3_GunBehaviour.GetPower();
			FK3_GunPlayerData playerData = fK3_GunBehaviour.GetPlayerData();
			if (power < playerData.minGunValue)
			{
				fK3_GunBehaviour.SetPower(playerData.minGunValue);
				gunByID.SetPower(playerData.minGunValue);
			}
			else if (power == playerData.maxGunValue)
			{
				fK3_GunBehaviour.SetPower(playerData.minGunValue);
				gunByID.SetPower(playerData.minGunValue);
			}
			else
			{
				int addstepGunValue = playerData.addstepGunValue;
				int num = power + addstepGunValue;
				num = ((num > playerData.maxGunValue) ? playerData.maxGunValue : num);
				fK3_GunBehaviour.SetPower(num);
				gunByID.SetPower(num);
			}
			if (power + playerData.addstepGunValue == playerData.maxGunValue)
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("炮值加到最大时的音效");
			}
			else
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("切换炮值");
			}
		}

		private void Handle_UI_BtnMinusPower(FK3_SpriteButton btn)
		{
			FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			FK3_GunUIController gunByID = FK3_GunUIMgr.Get().GetGunByID(FK3_GVars.lobby.curSeatId);
			int power = fK3_GunBehaviour.GetPower();
			FK3_GunPlayerData playerData = fK3_GunBehaviour.GetPlayerData();
			if (power < playerData.minGunValue)
			{
				fK3_GunBehaviour.SetPower(playerData.minGunValue);
				gunByID.SetPower(playerData.minGunValue);
			}
			else if (power == playerData.minGunValue)
			{
				fK3_GunBehaviour.SetPower(playerData.maxGunValue);
				gunByID.SetPower(playerData.maxGunValue);
			}
			else
			{
				int num = -playerData.addstepGunValue;
				int num2 = power + num;
				num2 = ((num2 < playerData.minGunValue) ? playerData.minGunValue : num2);
				fK3_GunBehaviour.SetPower(num2);
				gunByID.SetPower(num2);
			}
			if (power == playerData.minGunValue)
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("炮值加到最大时的音效");
			}
			else
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("切换炮值");
			}
		}

		private void GunStatusControl(int stage, float afterTime = 0f)
		{
			if (stage == 1)
			{
				if (_coGunStatusControl != null)
				{
					StopCoroutine(_coGunStatusControl);
				}
				_coGunStatusControl = StartCoroutine(IE_GunStatusControl(afterTime));
			}
		}

		private IEnumerator IE_GunStatusControl(float afterTime)
		{
			float wait = 6f;
			wait -= afterTime;
			FK3_GunBehaviour nativeGun = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			if (nativeGun == null)
			{
				yield return null;
				nativeGun = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			}
			if (wait > 0f)
			{
				try
				{
					nativeGun.allowShoot = false;
					if (nativeGun.GetGunType() == FK3_GunType.FK3_LockingGun)
					{
						nativeGun.StopLock();
					}
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("错误: " + arg);
				}
				yield return new WaitForSeconds(wait);
				try
				{
					Judge_GameScore_AllowShoot();
					if (nativeGun.allowShoot)
					{
						nativeGun.CheckAutoLock();
					}
				}
				catch (Exception arg2)
				{
					UnityEngine.Debug.LogError("错误: " + arg2);
				}
			}
		}

		private void InGameDropped()
		{
			UnityEngine.Debug.Log("炮台停止发炮");
			try
			{
				gunBehaviourStopFrid = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
				gunBehaviourStopFrid.allowShoot = false;
				if (gunBehaviourStopFrid.GetGunType() == FK3_GunType.FK3_LockingGun)
				{
					gunBehaviourStopFrid.StopLock();
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		private void CanFride()
		{
			if ((bool)gunBehaviourStopFrid && m_uiContext.curScore > 0)
			{
				gunBehaviourStopFrid.allowShoot = true;
			}
		}

		private void InGameConnectSuccess()
		{
			UnityEngine.Debug.Log("重置炮台状态");
			FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			Judge_GameScore_AllowShoot();
			if (fK3_GunBehaviour.allowShoot)
			{
				fK3_GunBehaviour.CheckAutoLock();
			}
			CleanBossCrabsAndDeepSeaOcean();
		}

		private void CleanBossCrabsAndDeepSeaOcean(bool send = false)
		{
			FK3_FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FK3_FishBehaviour _fish)
			{
				if (_fish.type == FK3_FishType.Boss_Crab_霸王蟹 || _fish.type == FK3_FishType.Boss_Kraken_深海八爪鱼 || _fish.type == FK3_FishType.Boss_Crocodil_史前巨鳄)
				{
					_fish.Die();
				}
			});
			if (send)
			{
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/askForBossInfo", new object[0]);
			}
		}

		private void RefreshNativeUserGameNumber(int gameScore, int expeGold, int gameGold)
		{
			if (m_uiContext == null)
			{
				m_uiContext = FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().GetContext();
			}
			m_uiContext.curScore = gameScore;
			m_uiContext.curGold = (m_uiContext.isExpeGold ? expeGold : gameGold);
			if (!m_uiContext.isExpeGold)
			{
				FK3_GVars.lobby.user.gameGold = gameGold;
			}
			else
			{
				FK3_GVars.lobby.user.expeGold = expeGold;
			}
			RefreshUserScore(m_context.curSeatId, gameScore);
		}

		private void RefreshUserScore(int seatId, int gameScore)
		{
			FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatId);
			FK3_GunUIController gunByID = FK3_GunUIMgr.Get().GetGunByID(seatId);
			FK3_GunBehaviour gunById2 = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatId);
			if (gunById == null)
			{
				UnityEngine.Debug.Log($"gun[id:{seatId}] not found");
				return;
			}
			if (seatId == m_context.curSeatId)
			{
				m_uiContext.curScore = gameScore;
				Judge_GameScore_AllowShoot();
			}
			gunById2.SetScore(gameScore);
			gunByID.SetScore(gameScore);
			if (m_uiContext.curScore == 0 && gunById.IsAuto())
			{
				gunById.DoCancelAuto();
				FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get().Show_InOutPanel();
			}
		}

		private void RefreshNotScore(int seatId, int gameScore)
		{
			FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatId);
			FK3_GunUIController gunByID = FK3_GunUIMgr.Get().GetGunByID(seatId);
			FK3_GunBehaviour gunById2 = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatId);
			if (gunById == null)
			{
				UnityEngine.Debug.Log($"gun[id:{seatId}] not found");
				return;
			}
			if (seatId == m_context.curSeatId)
			{
				m_uiContext.curScore = gameScore;
			}
			gunById2.SetScore(gameScore);
			gunByID.SetScore(gameScore);
			if (m_uiContext.curScore == 0 && gunById.IsAuto())
			{
				gunById.DoCancelAuto();
				FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get().Show_InOutPanel();
			}
		}

		private void Handle_FishOnDoubleClick(FK3_FishBehaviour fish)
		{
			UnityEngine.Debug.Log($"Handle_FishOnDoubleClick> fish:{fish.identity}");
			FK3_GunBehaviour fK3_GunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			if (fish.State == FK3_FishState.Live && fK3_GunBehaviour.CanLocking() && fish.inScreen)
			{
				fK3_GunBehaviour.LockFish(fish);
			}
		}

		private void OnSpeedUpAll(FK3_KeyValueInfo keyValueInfo)
		{
			FK3_fiSimpleSingletonBehaviour<FK3_FormationMgr<FK3_FishType>>.Get().StopRunningFormations();
		}

		private void Handle_UI_BtnReturn()
		{
			object[] args = new object[0];
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/quitGame", args);
			Application.Quit();
		}

		private void Handle_UI_BtnCoinIn()
		{
			int num = m_context.curDesk.onceExchangeValue;
			if (m_uiContext.curGold < m_context.curDesk.onceExchangeValue)
			{
				num = m_uiContext.curGold;
			}
			object[] args = new object[1]
			{
				num
			};
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/userCoinIn", args);
		}

		private void Handle_UI_BtnCoinOut()
		{
			int num = m_context.curDesk.onceExchangeValue * m_context.curDesk.exchange;
			if (m_uiContext.curScore < m_context.curDesk.onceExchangeValue * m_context.curDesk.exchange)
			{
				num = m_uiContext.curScore;
			}
			object[] args = new object[1]
			{
				num
			};
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/userCoinOut", args);
		}

		private void Handle_BackToLobby()
		{
			Reset_Option();
			object[] args = new object[2]
			{
				m_context.curDesk.id,
				m_context.curSeatId
			};
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/leaveSeat", args);
			m_context.isPlaying = false;
			_input.GetComponent<BoxCollider2D>().enabled = false;
			PlayerPrefs.Save();
		}

		private void KeepOnLoad()
		{
			List<GameObject> list = _GetRoots();
			foreach (GameObject item2 in list)
			{
				if (item2 != null)
				{
					UnityEngine.Object.DontDestroyOnLoad(item2.gameObject);
					FK3_GVars.dontDestroyOnLoadList.Add(item2.gameObject);
				}
			}
			FK3_MB_Singleton<FK3_GetDaTingRoot>.GetInstance().KeepOnLoad();
		}

		private void SceneRotateControl(bool rotate)
		{
			List<GameObject> list = _GetRoots();
			Vector3 vector = new Vector3(0f, 0f, 180f);
			Vector3 vector2 = new Vector3(0f, 0f, 0f);
			GameObject gameObject = list.Find((GameObject _) => _ != null && _.name == "GameRoot");
			GameObject gameObject2 = list.Find((GameObject _) => _ != null && _.name == "Canvas");
			GameObject gameObject3 = list.Find((GameObject _) => _ != null && _.name == "Main Camera");
			GameObject gameObject4 = list.Find((GameObject _) => _ != null && _.name == "ScriptsMgr");
			GameObject gameObject5 = list.Find((GameObject _) => _ != null && _.name == "ForMation路径Path");
			gameObject2.transform.localEulerAngles = (rotate ? vector : vector2);
			gameObject3.transform.localEulerAngles = (rotate ? vector : vector2);
			gameObject4.transform.localEulerAngles = (rotate ? vector : vector2);
			gameObject5.transform.localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("GunRoot/ChangeAndAuto").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("ScenarioRoot").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("ScenarioRoot/StoryScenario/LogoBossArr").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("ScenarioRoot/StoryScenario/LogoGame").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("GunRoot/Gun_1/TextRoot1").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("GunRoot/Gun_1/TextRoot2").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("GunRoot/Gun_2/TextRoot1").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("GunRoot/Gun_2/TextRoot2").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("GunRoot/Gun_3/TextRoot1").localEulerAngles = (rotate ? vector2 : vector);
			gameObject.transform.Find("GunRoot/Gun_3/TextRoot2").localEulerAngles = (rotate ? vector2 : vector);
			gameObject.transform.Find("GunRoot/Gun_4/TextRoot1").localEulerAngles = (rotate ? vector2 : vector);
			gameObject.transform.Find("GunRoot/Gun_4/TextRoot2").localEulerAngles = (rotate ? vector2 : vector);
			gameObject.transform.Find("EffectLayer").localEulerAngles = (rotate ? vector : vector2);
			gameObject.transform.Find("GunRoot").localEulerAngles = (rotate ? vector : vector2);
			MainCamera = gameObject3;
		}

		private void DisableScene()
		{
			List<GameObject> list = _GetRoots();
			foreach (GameObject item2 in list)
			{
				if (item2 != null)
				{
					item2.SetActive(value: false);
				}
			}
			FK3_MB_Singleton<FK3_GetDaTingRoot>.GetInstance().DisableScene();
		}

		private void EnableScene()
		{
			List<GameObject> list = _GetRoots();
			foreach (GameObject item2 in list)
			{
				if (item2 != null)
				{
					item2.SetActive(value: true);
				}
			}
			FK3_MB_Singleton<FK3_GetDaTingRoot>.GetInstance().EnableScene();
			UnityEngine.Debug.LogError("加载提示组件");
			FK3_UIIngameManager.GetInstance().OpenUI("AlertDialog2");
			FK3_UIIngameManager.GetInstance().CloseUI("AlertDialog2");
			FK3_UIIngameManager.GetInstance().OpenUI("Notice");
		}

		public void EnterGame(object arg, Action<object> next)
		{
			UnityEngine.Debug.LogError("===============" + sceneName + ".EnterGame =============== ");
			m_context.isPlaying = true;
			base.gameObject.SetActive(value: true);
			userGameScore = 0;
			object[] array = arg as object[];
			int num = (int)array[0];
			int stage = (int)array[1];
			int stageType = (int)array[2];
			int num2 = (int)array[3];
			int stageDuration = (int)array[4];
			userGameScore = (int)array[5];
			GunStatusControl(stage, num2);
			bool faceDown = FK3_GVars.lobby.curSeatId > 2;
			m_bombCrabAction.DoReset();
			m_bombCrabAction.SetFaceDown(faceDown);
			m_lightingFishAction.SetFaceDown(faceDown);
			EnableScene();
			Init_Context();
			Init_Gun();
			Init_Game();
			Init_HeadTitle();
			FK3_GVars.m_curState = FK3_Demo_UI_State.InGame;
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().Init(FK3_GVars.lobby.curSeatId > 2);
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().SetScenario((FK3_ScenarioType)num);
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().SetStage((FK3_ScenarioType)num, stage, stageType, num2, stageDuration);
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("水流声背景音乐", loop: true);
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("水流声背景音乐", 1f);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("gameGoldPush", HandleNetMsg_GameGold);
			FK3_MB_Singleton<FK3_SynchronizeHint>.Get().Show();
			FK3_MessageCenter.RegisterHandle("SetCursorUsageLinearSpeed", OnSpeedUpAll);
		}

		private void TestLogic_Add_AutoKeepScoreLogic()
		{
			UnityEngine.Debug.Log("TestLogic_Add_AutoKeepScoreLogic");
			FK3_AutoKeepScoreLogic fK3_AutoKeepScoreLogic = GetComponent<FK3_AutoKeepScoreLogic>();
			if (fK3_AutoKeepScoreLogic == null)
			{
				fK3_AutoKeepScoreLogic = base.gameObject.AddComponent<FK3_AutoKeepScoreLogic>();
			}
			UnityEngine.Debug.Log($"uiContext:{m_uiContext},context:{m_context}");
			if (!fK3_AutoKeepScoreLogic.CheckValid())
			{
				fK3_AutoKeepScoreLogic.Init(m_uiContext, m_context);
			}
		}

		private void Init_HeadTitle()
		{
			FK3_SimpleSingletonBehaviour<FK3_HeadTitleController>.Get().Init();
		}

		private void Reset_Option()
		{
			FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get().Init();
			FK3_SimpleSingletonBehaviour<FK3_SendChatController>.Get().CloseChatWindow();
		}

		private List<GameObject> _GetRoots()
		{
			return roots;
		}

		private void DisplayBack()
		{
			if (m_useNet && GUI.Button(new Rect(0f, 0f, 100f, 25f), "返回"))
			{
				Handle_UI_BtnReturn();
			}
		}

		private void SwithInCleanSceneLong(FK3_KeyValueInfo keyValueInfo)
		{
			UnityEngine.Debug.LogError("SwithInCleanSceneLong");
			m_lightingFishAction.Stop();
			m_laserCrabAction.Stop();
			m_bombCrabAction.Stop();
			m_drillCrabAction.Stop();
			m_caymanAction.Stop();
			m_bossCrabAction.Stop();
			m_bossLanternAction.Stop();
			m_bossCrocodileAction.Stop();
			m_bossKrakenAction.Stop();
			m_bossDorganAction.Stop();
			FK3_FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FK3_FishBehaviour _fish)
			{
				_fish.Die();
			});
			FK3_Effect_CoinMgr.Get().RemoveAllCoins();
			FK3_Effect_ScoreMgr.Get().RemoveAllScores();
			FK3_BulletMgr.Get().RemoveAllBullets();
			FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().StopAllCoroutines();
			FK3_EffectMgr.Get().ResetAllLockChains();
			FK3_Effect_LogoMgr.Get().RemoveAllLogos();
			_bulletId = 0;
			CleanBossCrabsAndDeepSeaOcean(send: true);
		}

		private void SwithInCleanSceneShort(FK3_KeyValueInfo keyValueInfo)
		{
			CleanBossCrabsAndDeepSeaOcean(send: true);
		}

		private void CleanScene()
		{
			UnityEngine.Debug.LogError("CleanScene");
			m_context.isPlaying = false;
			DisableScene();
			m_lightingFishAction.Stop();
			m_laserCrabAction.Stop();
			m_bombCrabAction.Stop();
			m_drillCrabAction.Stop();
			m_caymanAction.Stop();
			m_bossCrabAction.Stop();
			m_bossLanternAction.Stop();
			m_bossCrocodileAction.Stop();
			m_bossKrakenAction.Stop();
			m_bossDorganAction.Stop();
			FK3_FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FK3_FishBehaviour _fish)
			{
				_fish.Die();
			});
			FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().ForeachGun(delegate(FK3_GunBehaviour _gun)
			{
				_gun.Reset_EventHandler();
				_gun.Reset_Gun();
			});
			FK3_GunUIMgr.Get().ClearGunsUI();
			FK3_Effect_CoinMgr.Get().RemoveAllCoins();
			FK3_Effect_ScoreMgr.Get().RemoveAllScores();
			FK3_BulletMgr.Get().RemoveAllBullets();
			FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.Get().StopAllCoroutines();
			FK3_EffectMgr.Get().ResetAllLockChains();
			FK3_Effect_LogoMgr.Get().RemoveAllLogos();
			_bulletId = 0;
			base.gameObject.SetActive(value: false);
			FK3_AppSceneMgr.RunAction("Lobby.EnterLobby", null, delegate
			{
			});
		}
	}
}
