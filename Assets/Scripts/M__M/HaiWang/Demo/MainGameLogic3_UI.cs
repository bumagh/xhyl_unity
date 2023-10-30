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
	public class MainGameLogic3_UI : MonoBehaviour
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

		private InGameUIContext m_uiContext;

		private GameContext m_context = new GameContext();

		private LightingFishAction m_lightingFishAction;

		private BombCrabAction m_bombCrabAction;

		private LaserCrabAction m_laserCrabAction;

		public EffectConfig effectConfig;

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

		private SpriteButton m_btnAuto;

		private SpriteButton btnChangeGun;

		private GunBehaviour nativeGun;

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

		private NewFishGroupItemInfo item;

		private bool isPlayFishGroup;

		private FishQueenData fishQueenData;

		private WaitForSeconds wait = new WaitForSeconds(1f);

		private WaitForSeconds wait2 = new WaitForSeconds(17f);

		private Coroutine waitFishGroup;

		private Coroutine waitFishGroup2;

		public static GameObject MainCamera
		{
			get;
			set;
		}

		private void Awake()
		{
			HW2_MessageCenter.RegisterHandle("SwithInCleanSceneLong", SwithInCleanSceneLong);
			HW2_MessageCenter.RegisterHandle("SwithInCleanSceneShort", SwithInCleanSceneShort);
			HW2_MessageCenter.RegisterHandle("LockGunShoot", GunLogic_Handle_GunLockShoot);
			HW2_MessageCenter.RegisterHandle("LockGunSelectFish", GunLogic_Handle_GunLockSelect);
			HW2_MessageCenter.RegisterHandle("StopHandleNewFish", StopHandleNewFish);
			AppSceneMgr.RegisterScene(sceneName);
			AppSceneMgr.RegisterAction("Game.EnterGame", EnterGame);
			if (AppSceneMgr.isFirstScene(sceneName))
			{
				FishBehaviour.SEvent_FishOnStart_Handler = delegate
				{
				};
			}
		}

		private void Start()
		{
			InitUI();
			InitSettingState();
			HW2_AppManager hW2_AppManager = HW2_MB_Singleton<HW2_AppManager>.Get();
			hW2_AppManager.InGame_NetDown_Handler = (Action)Delegate.Combine(hW2_AppManager.InGame_NetDown_Handler, new Action(InGameDropped));
			HW2_AppManager hW2_AppManager2 = HW2_MB_Singleton<HW2_AppManager>.Get();
			hW2_AppManager2.InGame_ReconnectSuccess_Handler = (Action)Delegate.Combine(hW2_AppManager2.InGame_ReconnectSuccess_Handler, new Action(InGameConnectSuccess));
			if (AppSceneMgr.isFirstScene(sceneName))
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
			if (UIIngameManager.GetInstance() == null)
			{
				UnityEngine.Debug.LogError("UIIngameManager为空!");
				return;
			}
			UIIngameManager.GetInstance().OpenUI("HeadTitle");
			UIIngameManager.GetInstance().OpenUI("GunUI");
			UIIngameManager.GetInstance().OpenUI("OpScore");
			UIIngameManager.GetInstance().OpenUI("ChatRoot");
			Option.SetActive(value: true);
		}

		private void InitSettingState()
		{
			if (PlayerPrefs.HasKey("BgSound"))
			{
				int @int = PlayerPrefs.GetInt("BgSound");
				HW2_Singleton<SoundMgr>.Get().SetActive(@int == 1);
			}
			if (PlayerPrefs.HasKey("ScreenBright"))
			{
				int int2 = PlayerPrefs.GetInt("ScreenBright");
				Screen.sleepTimeout = ((int2 == 1) ? (-1) : (-2));
			}
			if (PlayerPrefs.HasKey("GroupChat"))
			{
				int int3 = PlayerPrefs.GetInt("GroupChat");
				HW2_GVars.isShutChatGroup = (int3 == 1);
			}
			if (PlayerPrefs.HasKey("PrivateChat"))
			{
				int int4 = PlayerPrefs.GetInt("PrivateChat");
				HW2_GVars.isShutChatPrivate = (int4 == 1);
			}
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.P))
			{
				object[] args = new object[0];
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/changScenarios", args);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && HW2_GVars.m_curState == Demo_UI_State.InGame)
			{
				HW2_AlertDialog.Get().ShowDialog("是否退出游戏？", showOkCancel: true, Handle_BackToLobby);
			}
			if (Input.GetMouseButtonDown(0))
			{
				SimpleSingletonBehaviour<OptionController>.Get().HideOption();
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
			m_lightingFishAction = new LightingFishAction();
			m_lightingFishAction.Init();
			if (m_bombCrabAction == null)
			{
				m_bombCrabAction = base.gameObject.AddComponent<BombCrabAction>();
			}
			m_bombCrabAction.Init();
			m_laserCrabAction = base.gameObject.GetComponent<LaserCrabAction>();
			FishTeamMgr.Get().DisableAllTeams();
			base.gameObject.AddComponent<KillFishMgr>();
			SimpleSingletonBehaviour<KillFishMgr>.Get().Init();
		}

		private void PreInit_NetMsgHandler()
		{
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("newFishPush", HandleNetMsg_NewFish);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("newFishGroupPush", HandleNetMsg_NewFishGroup);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("newFishFormationPush", HandleNetMsg_NewFishFormation);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("newFishPufferPush", HandleNetMsg_NewPuffer);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("newKingCrabBossPush", HandleNetMsg_NewKingCrabBoss);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("newDeepSeaOctopusBossPush", HandleNetMsg_NewDeepSeaOctopusBossPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("changeStagePush", HandleNetMsg_ChangeStage);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userCoinIn", HandleNetMsg_UserCoinIn);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userCoinOut", HandleNetMsg_UserCoinOut);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("gunHitFish", delegate(object[] _)
			{
				HandleNetMsg_Default("gunHitFish", _);
			});
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("askForFishFormation", delegate(object[] _)
			{
				HandleNetMsg_Default("gunHitFish", _);
			});
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("playerScenePush", HandleNetMsg_PlayerScenePush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("gunHitFishInAction", HandleNetMsg_GunHitFishInAction);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("bulletHitFishInActionPush", HandleNetMsg_GunHitFishInActionPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("leaveSeat", HandleNetMsg_LeaveSeat);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("quitToDeskPush", HandleNetMsg_QuitToDesk);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("updateDeskInfoPush", HandleNetMsg_UpdateDeskInfoPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("changeGun", HandleNetMsg_ChangeGun);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("changeGunPush", HandleNetMsg_ChangeGunPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userFired", HandleNetMsg_UserFired);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userFiredPush", HandleNetMsg_UserFiredPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userFiredEM", HandleNetMsg_UserFiredEM);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userFiredEMPush", HandleNetMsg_UserFiredEMPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userRotateEM", HandleNetMsg_UserRotateEM);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userRotateEMPush", HandleNetMsg_UserRotateEMPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("bulletHitFishPush", HandleNetMsg_BulletHitFish);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("gunHitLockFish", HandleNetMsg_GunHitLockFish);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("bulletHitLockFishPush", HandleNetMsg_BulletHitLockFishPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("lockSelectFish", HandleNetMsg_LockSelectFish);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("lockSelectFishPush", HandleNetMsg_LockSelectFishPush);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("bombCrabNextPoint", HandleNetMsg_BombCrabNextPoint);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("bombCrabNextPointPush", HandleNetMsg_BombCrabNextPointPush);
		}

		private void PreInit_FormationV1()
		{
			FishFormationFactory.Get().createFun = delegate(FishType _fishType, int id)
			{
				FishMgr.Get().SetFishIndex(id);
				FishCreationInfo info = new FishCreationInfo(_fishType);
				FishBehaviour fishBehaviour = FishMgr.Get().GenNewFish(info);
				if (id != fishBehaviour.id)
				{
					UnityEngine.Debug.LogError($"Formation.Create> expect id:{id}, actual id:{fishBehaviour.id}");
				}
				fishBehaviour.SetPosition(new Vector3(-3f, 0f));
				FishBindEventHandler(fishBehaviour);
				NavPathAgent navPathAgent = fishBehaviour.gameObject.GetComponent<NavPathAgent>();
				if (navPathAgent == null)
				{
					navPathAgent = fishBehaviour.gameObject.AddComponent<NavPathAgent>();
				}
				else
				{
					navPathAgent.enabled = true;
				}
				bool doneOnce = false;
				Action<FishBehaviour> b = delegate(FishBehaviour _fish)
				{
					if (!doneOnce)
					{
						doneOnce = true;
						AgentData<FishType> agentData = _fish.GetComponent<NavPathAgent>().userData as AgentData<FishType>;
						agentData?.formation.RemoveObject(agentData.agent);
					}
				};
				FishBehaviour fishBehaviour2 = fishBehaviour;
				fishBehaviour2.Event_FishDying_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDying_Handler, b);
				FishBehaviour fishBehaviour3 = fishBehaviour;
				fishBehaviour3.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour3.Event_FishDie_Handler, b);
				FishBehaviour fishBehaviour4 = fishBehaviour;
				fishBehaviour4.Event_FishMovePause_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour4.Event_FishMovePause_Handler, b);
				return navPathAgent;
			};
			FishFormationFactory.Get().destoryAction = delegate(NavPathAgent _navPathAgent, FishType _fishType)
			{
				FishBehaviour component = _navPathAgent.GetComponent<FishBehaviour>();
				if (component.State == FishState.Live)
				{
					component.Die();
					_navPathAgent.enabled = false;
				}
			};
			Formation<FishType>.forbiddenPlayOnStart = true;
		}

		private void PreInit_FormationV2()
		{
			fiSimpleSingletonBehaviour<FishSpawnMaster>.Get().setupFishAction = FishBindEventHandler;
		}

		private void PreInit_UI()
		{
			btnChangeGun = fiSimpleSingletonBehaviour<GunMgr>.Get().transform.Find("ChangeAndAuto/BtnChangeGun").GetComponent<SpriteButton>();
			m_btnAuto = fiSimpleSingletonBehaviour<GunMgr>.Get().transform.Find("ChangeAndAuto/BtnAuto").GetComponent<SpriteButton>();
			btnChangeGun.onClick = Handle_UI_BtnChangeGun;
			m_btnAuto.onClick = Handle_UI_BtnAuto;
		}

		private void Init_Gun()
		{
			_input.GetComponent<BoxCollider2D>().enabled = true;
			Assert.raiseExceptions = true;
			fiSimpleSingletonBehaviour<GunMgr>.Get().ForeachGun(delegate(GunBehaviour gun)
			{
				gun.gameObject.SetActive(value: false);
			});
			bool rotate = HW2_GVars.lobby.curSeatId > 2;
			SceneRotateControl(rotate);
			if (HW2_GVars.lobby.curSeatId == 1 || HW2_GVars.lobby.curSeatId == 3)
			{
				GunUIMgr.Get().StopCoroutine(GunUIMgr.Get().pointMove());
				GunUIMgr.Get().point1.gameObject.SetActive(value: true);
				GunUIMgr.Get().point2.gameObject.SetActive(value: false);
				GunUIMgr.Get().StartCoroutine(GunUIMgr.Get().pointMove());
			}
			else if (HW2_GVars.lobby.curSeatId == 2 || HW2_GVars.lobby.curSeatId == 4)
			{
				GunUIMgr.Get().StopCoroutine(GunUIMgr.Get().pointMove());
				GunUIMgr.Get().point1.gameObject.SetActive(value: false);
				GunUIMgr.Get().point2.gameObject.SetActive(value: true);
				GunUIMgr.Get().StartCoroutine(GunUIMgr.Get().pointMove());
			}
			GunUIMgr.Get().SetRotate(rotate);
			DeskInfo curDesk = HW2_GVars.lobby.GetCurDesk();
			List<SeatInfo2> inGameSeats = HW2_GVars.lobby.inGameSeats;
			foreach (SeatInfo2 item2 in inGameSeats)
			{
				SetPlayerGun(curDesk, item2, forceUpdate: true);
			}
			BulletMgr.Get().debugAllowShoot = true;
			nativeGun = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			GunBehaviour gunBehaviour = nativeGun;
			gunBehaviour.Event_ShootNormal_Handler = (GunBehaviour.EventHandler_Shoot)Delegate.Combine(gunBehaviour.Event_ShootNormal_Handler, new GunBehaviour.EventHandler_Shoot(GunLogic_Handle_GunShoot2));
			GunBehaviour gunBehaviour2 = nativeGun;
			gunBehaviour2.Event_ShootEM_Handler = (GunBehaviour.EventHandler_ShootEM)Delegate.Combine(gunBehaviour2.Event_ShootEM_Handler, new GunBehaviour.EventHandler_ShootEM(GunLogic_Handle_GunShootEM));
			GunBehaviour gunBehaviour3 = nativeGun;
			gunBehaviour3.Event_RotateEM_Handler = (GunBehaviour.EventHandler_RotateEM)Delegate.Combine(gunBehaviour3.Event_RotateEM_Handler, new GunBehaviour.EventHandler_RotateEM(GunLogic_Handle_GunRotateEM));
			GunBehaviour gunBehaviour4 = nativeGun;
			gunBehaviour4.Event_ScreenBeClick_act = (Action<bool>)Delegate.Combine(gunBehaviour4.Event_ScreenBeClick_act, new Action<bool>(ShowInOutScorePanel));
			GunBehaviour gunBehaviour5 = nativeGun;
			gunBehaviour5.Event_ChangeAuto_Handler = (GunBehaviour.EventHandler_ChangeAuto)Delegate.Combine(gunBehaviour5.Event_ChangeAuto_Handler, new GunBehaviour.EventHandler_ChangeAuto(GunLogic_Handle_OnChangeAuto));
			if (HW2_GVars.lobby.curSeatId == 1 || HW2_GVars.lobby.curSeatId == 4)
			{
				nativeGun.GunMove(odd: true);
			}
			else if (HW2_GVars.lobby.curSeatId == 2 || HW2_GVars.lobby.curSeatId == 3)
			{
				nativeGun.GunMove(odd: false);
			}
			SpriteButton btnPlusPower = nativeGun.GetBtnPlusPower();
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
			SpriteButton btnMinsPower = nativeGun.GetBtnMinsPower();
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
			BulletMgr bulletMgr = BulletMgr.Get();
			bulletMgr.Event_BulletOver_Handler = (Action<BulletController>)Delegate.Combine(bulletMgr.Event_BulletOver_Handler, (Action<BulletController>)delegate
			{
				nativeGun.OnBulletOver();
			});
			SetBtnAuto(isAuto: false);
			btnChangeGun.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/but_game_6_1");
			UIIngameManager.GetInstance().OpenUI("UserInfoRoot", show: false);
			HW2_UserInfoInGame hW2_UserInfoInGame = SimpleSingletonBehaviour<HW2_UserInfoInGame>.Get();
			hW2_UserInfoInGame.Event_PrivateChatClick_act = (Action<int>)Delegate.Combine(hW2_UserInfoInGame.Event_PrivateChatClick_act, new Action<int>(OpenChatWindow));
			HW2_MessageCenter.RegisterHandle("CheckAutoLock", CheckAutoLock);
		}

		private void SetPlayerGun(DeskInfo desk, SeatInfo2 seat, bool forceUpdate = false)
		{
			UnityEngine.Debug.LogError("desk: " + desk.name + " seat: " + seat.id + " forceUpdate: " + forceUpdate);
			int id = seat.id;
			GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(id);
			GunUIController gunByID = GunUIMgr.Get().GetGunByID(id);
			if (forceUpdate)
			{
				UnityEngine.Debug.LogError("forceUpdate");
				gunById.Reset_EventHandler();
				gunById.Reset_Gun();
				if (id != HW2_GVars.lobby.curSeatId)
				{
					GunBehaviour gunBehaviour = gunById;
					gunBehaviour.Event_ShootNormal_Handler = (GunBehaviour.EventHandler_Shoot)Delegate.Combine(gunBehaviour.Event_ShootNormal_Handler, new GunBehaviour.EventHandler_Shoot(BulletMgr.Get().Handle_Shoot2));
					GunBehaviour gunBehaviour2 = gunById;
					gunBehaviour2.Event_GunBeClick_act = (Action<int>)Delegate.Combine(gunBehaviour2.Event_GunBeClick_act, new Action<int>(ShowUserInfo));
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
					if (gunById.GetGunType() == GunType.LockingGun)
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
				GunPlayerData gunPlayerData = new GunPlayerData();
				gunPlayerData.gunPower = desk.minGunValue;
				gunPlayerData.name = seat.user.nickname;
				gunPlayerData.score = seat.user.gameScore;
				gunPlayerData.isNative = (seat.id == HW2_GVars.lobby.curSeatId);
				gunPlayerData.minGunValue = desk.minGunValue;
				gunPlayerData.maxGunValue = desk.maxGunValue;
				gunPlayerData.addstepGunValue = desk.addstepGunValue;
				gunPlayerData.minGold = desk.minGold;
				gunPlayerData.exchange = desk.exchange;
				gunPlayerData.onceExchangeValue = desk.onceExchangeValue;
				try
				{
					UnityEngine.Debug.LogError("初始化枪数据: " + JsonMapper.ToJson(gunPlayerData));
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("错误: " + arg);
				}
				UnityEngine.Debug.LogError("初始化的枪: " + gunById.name);
				gunById.SetPlayerData(gunPlayerData);
				gunByID.SetPlayerData(gunPlayerData);
				UnityEngine.Debug.Log(string.Format("Seat[{0}] is {1}", seat.id, gunPlayerData.isNative ? "native" : "not native"));
			}
		}

		private void Init_Game()
		{
			FishMgr.Get().Event_OnFishCreate_Handler = HookFishCreate;
			RefreshNativeUserGameNumber(userGameScore, HW2_GVars.user.expeGold, HW2_GVars.user.gameGold);
			Judge_GameScore_AllowShoot();
			Judge_GameScore_CoinInOut();
			StartCoroutine(IE_Test_Crab());
		}

		private void ShowUserInfo(int index)
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Red("index: " + index));
			if (HW2_GVars.lobby.curSeatId != index)
			{
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/playerInfo", new object[2]
				{
					HW2_GVars.lobby.curDeskId,
					index
				});
			}
		}

		private void OpenChatWindow(int seatId)
		{
			SimpleSingletonBehaviour<SendChatController>.Get().ShowChatWindow(seatId);
		}

		private void ShowInOutScorePanel(bool isShow)
		{
			if (isShow)
			{
				SimpleSingletonBehaviour<OptionController>.Get().Show_InOutPanel();
			}
		}

		private void HookFishCreate(FishBehaviour fish)
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
				fish.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fish.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
				{
					UnityEngine.Object.DestroyImmediate(gmObj);
				});
			}
		}

		private void Init_Context()
		{
			HW2_GVars.SetGameContext(m_context);
			m_context.curDesk = HW2_GVars.lobby.GetCurDesk();
			m_context.curSeatId = HW2_GVars.lobby.curSeatId;
			m_uiContext = SimpleSingletonBehaviour<SaveAndTakeScores>.Get().GetContext();
			UnityEngine.Debug.Log("HW2_GVars.lobby.curRoomId:" + HW2_GVars.lobby.curRoomId);
			m_uiContext.isExpeGold = (HW2_GVars.lobby.curRoomId == 1);
		}

		private void Judge_GameScore_AllowShoot()
		{
			GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			if (!(gunBehaviour == null))
			{
				if (m_uiContext.curScore == 0)
				{
					gunBehaviour.allowShoot = false;
				}
				else
				{
					gunBehaviour.allowShoot = true;
				}
			}
		}

		private void Judge_GameScore_CoinInOut()
		{
			if (m_uiContext.curScore == 0)
			{
				SimpleSingletonBehaviour<SaveAndTakeScores>.Get().EventHandler_UI_PanelCoinInOut_OnHide = delegate
				{
					Judge_GameScore_AllowShoot();
				};
				Option.SetActive(value: true);
			}
		}

		private void PreInit_UIEventHandler()
		{
			SimpleSingletonBehaviour<SaveAndTakeScores>.Get().Reset_EventHandler();
			SimpleSingletonBehaviour<SaveAndTakeScores>.Get().EventHandler_UI_OnBtnCoinInClick += Handle_UI_BtnCoinIn;
			SimpleSingletonBehaviour<SaveAndTakeScores>.Get().EventHandler_UI_OnBtnCoinOutClick += Handle_UI_BtnCoinOut;
			SimpleSingletonBehaviour<OptionController>.Get().Reset_EventHandler();
			OptionController optionController = SimpleSingletonBehaviour<OptionController>.Get();
			optionController.Event_OnBackToLobby = (Action)Delegate.Combine(optionController.Event_OnBackToLobby, new Action(Handle_BackToLobby));
		}

		private void _NetMsg_CheckErrorLog(string method, NetMsgInfoBase info)
		{
			if (!info.valid || info.code != 0)
			{
				UnityEngine.Debug.LogError($"{method}: code:[{info.code}], message:[{info.message}]");
			}
		}

		private void HandleNetMsg_Default(string method, object[] args)
		{
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			_NetMsg_CheckErrorLog(method, netMsgInfoBase);
		}

		private void StopHandleNewFish(KeyValueInfo keyValueInfo)
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
			NetMsgInfo_RecvNewFish netMsgInfo_RecvNewFish = new NetMsgInfo_RecvNewFish(args);
			try
			{
				netMsgInfo_RecvNewFish.Parse();
				if (newFishCount++ >= int.MaxValue)
				{
					return;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (netMsgInfo_RecvNewFish.valid && netMsgInfo_RecvNewFish.code == 0)
			{
				for (int i = 0; i < netMsgInfo_RecvNewFish.items.Count; i++)
				{
					FishQueenData fishQueenData = new FishQueenData();
					FP_Sequence fP_Sequence = new FP_Sequence();
					FG_SingleType<FishType> fG_SingleType = new FG_SingleType<FishType>();
					fG_SingleType.type = (FishType)netMsgInfo_RecvNewFish.items[i].fishType;
					fG_SingleType.count = 1;
					fishQueenData.delay = 0f;
					fishQueenData.generator = fG_SingleType;
					fishQueenData.speed = FishQueenMgr.Get().GetSpeedByFishType(fG_SingleType.type.BasicType());
					fP_Sequence.pathId = netMsgInfo_RecvNewFish.items[i].pathID;
					FishQueen fishQueen = FishQueenMgr.Get().GetFishQueen();
					fishQueen.setupFishAction = FishBindEventHandler;
					fishQueen.Setup(fishQueenData, fP_Sequence, 1, netMsgInfo_RecvNewFish.items[i].fishID);
					fishQueen.Play();
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("newFish", netMsgInfo_RecvNewFish);
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
				UnityEngine.Debug.LogError("新的鱼组: " + JsonMapper.ToJson(args));
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
			NetMsgInfo_RecvNewFishGroup netMsgInfo_RecvNewFishGroup = new NetMsgInfo_RecvNewFishGroup(args);
			netMsgInfo_RecvNewFishGroup.Parse();
			if (netMsgInfo_RecvNewFishGroup.valid && netMsgInfo_RecvNewFishGroup.code == 0)
			{
				item = netMsgInfo_RecvNewFishGroup.item;
				if (item.groupID == 1 || item.groupID == 2)
				{
					UnityEngine.Debug.LogError("item.groupID == 1 || item.groupID == 2");
					fishQueenData = new FishQueenData();
					FP_Team fP_Team = new FP_Team();
					if (item.groupID == 1)
					{
						UnityEngine.Debug.LogError("item.groupID == 1 迦魶鱼");
						fishQueenData.generator = new FG_SingleType<FishType>
						{
							type = FishType.Gurnard_迦魶鱼,
							count = item.fishCount
						};
					}
					else
					{
						UnityEngine.Debug.LogError("item.groupID == 2 闪电迦魶鱼");
						FG_FixedSequence<FishType> fG_FixedSequence = new FG_FixedSequence<FishType>();
						FishType fishType = FishType.Lightning_Gurnard_闪电迦魶鱼;
						FishType fishType2 = FishType.Gurnard_迦魶鱼;
						List<FishType> list = new List<FishType>();
						list.Add(fishType);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						list.Add(fishType2);
						List<FishType> list2 = fG_FixedSequence.types = list;
						fishQueenData.generator = fG_FixedSequence;
					}
					try
					{
						fishQueenData.delay = 0f;
						fishQueenData.speed = FishQueenMgr.Get().GetSpeedByFishType(FishType.Gurnard_迦魶鱼);
						fP_Team.pathId = item.pathID;
						fP_Team.teamId = 1;
						FishQueen fishQueen = FishQueenMgr.Get().GetFishQueen();
						fishQueen.setupFishAction = FishBindEventHandler;
						fishQueen.Setup(fishQueenData, fP_Team, item.fishCount, item.startFishID);
						fishQueen.Play();
					}
					catch (Exception arg2)
					{
						UnityEngine.Debug.LogError("错误: " + arg2);
					}
				}
				else if (item.groupID == 3 || item.groupID == 4)
				{
					UnityEngine.Debug.LogError("item.groupID == 3 || item.groupID == 4");
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
				else
				{
					UnityEngine.Debug.LogError($"鱼队 [{item.groupID}] 没有实现");
				}
			}
			else
			{
				UnityEngine.Debug.LogError("======================鱼队错误===================");
				_NetMsg_CheckErrorLog("newFishGroup", netMsgInfo_RecvNewFishGroup);
			}
		}

		private IEnumerator WaitFishGroup(NewFishGroupItemInfo item)
		{
			yield return wait;
			Func<int, FishType, FishType> func = (int _index, FishType _type) => (_index == 0) ? FishType.Lightning_Rasbora_闪电鲽鱼 : _type;
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
				NetMsgInfo_RecvNewFishFormation netMsgInfo_RecvNewFishFormation = new NetMsgInfo_RecvNewFishFormation(args);
				netMsgInfo_RecvNewFishFormation.Parse();
				if (netMsgInfo_RecvNewFishFormation.valid && netMsgInfo_RecvNewFishFormation.code == 0)
				{
					for (int i = 0; i < netMsgInfo_RecvNewFishFormation.items.Count; i++)
					{
						bool useOffset = false;
						Vector3 offset = Vector3.zero;
						if (netMsgInfo_RecvNewFishFormation.items[i].posX != 0f || netMsgInfo_RecvNewFishFormation.items[i].posY != 0f)
						{
							useOffset = true;
							offset = new Vector3(netMsgInfo_RecvNewFishFormation.items[i].posX, netMsgInfo_RecvNewFishFormation.items[i].posY, 0f);
						}
						PlayFormation(2, netMsgInfo_RecvNewFishFormation.items[i].startFishID, netMsgInfo_RecvNewFishFormation.startTime, useOffset, offset);
					}
				}
				else
				{
					_NetMsg_CheckErrorLog("newFishFormation", netMsgInfo_RecvNewFishFormation);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			yield return wait2;
			isPlayFishGroup = false;
		}

		private void PlayFormation(int formationId, int startId, float startTime = 0f, Func<int, FishType, FishType> generatorFunc = null)
		{
			PlayFormation(formationId, startId, startTime, useOffset: false, Vector3.zero, generatorFunc);
		}

		private void PlayFormation(int formationId, int startId, float startTime, bool useOffset, Vector3 offset, Func<int, FishType, FishType> generatorFunc = null)
		{
			fiSimpleSingletonBehaviour<FormationMgr<FishType>>.Get().PlayFormation(new FormationPlayParam<FishType>
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
			NetMsgInfo_RecvNewPuffer netMsgInfo_RecvNewPuffer = new NetMsgInfo_RecvNewPuffer(args);
			try
			{
				netMsgInfo_RecvNewPuffer.Parse();
				if (newFishCount++ >= int.MaxValue)
				{
					return;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (netMsgInfo_RecvNewPuffer.valid && netMsgInfo_RecvNewPuffer.code == 0)
			{
				for (int i = 0; i < netMsgInfo_RecvNewPuffer.items.Count; i++)
				{
					FishQueenData fishQueenData = new FishQueenData();
					FP_Sequence fP_Sequence = new FP_Sequence();
					FG_SingleType<FishType> fG_SingleType = new FG_SingleType<FishType>();
					fG_SingleType.type = (FishType)netMsgInfo_RecvNewPuffer.items[i].fishType;
					fG_SingleType.count = 1;
					fishQueenData.delay = 0f;
					fishQueenData.generator = fG_SingleType;
					fishQueenData.speed = FishQueenMgr.Get().GetSpeedByFishType(fG_SingleType.type.BasicType());
					fP_Sequence.pathId = netMsgInfo_RecvNewPuffer.items[i].pathID;
					FishQueen fishQueen = FishQueenMgr.Get().GetFishQueen();
					fishQueen.setupFishAction = FishBindEventHandler;
					fishQueen.Setup(fishQueenData, fP_Sequence, 1, netMsgInfo_RecvNewPuffer.items[i].fishID, netMsgInfo_RecvNewPuffer.items[i].openTime);
					fishQueen.Play();
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("newFish", netMsgInfo_RecvNewPuffer);
			}
		}

		private void HandleNetMsg_NewKingCrabBoss(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_NewKingCrabBoss");
			if (debugFish.IgnoreNewKingCrab)
			{
				return;
			}
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			try
			{
				netMsgInfoBase.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int num = (int)netMsgInfoBase.basicDic["fishId"];
				if (!(FishMgr.Get().GetFishById(num) != null))
				{
					int num2 = (int)netMsgInfoBase.basicDic["seed"];
					int num3 = (int)netMsgInfoBase.basicDic["survivalTime"];
					FishMgr.Get().SetFishIndex(num);
					FishBehaviour fishBehaviour = FishMgr.Get().SpawnSingleFish(FishType.Boss_Crab_霸王蟹);
					FishBindEventHandler(fishBehaviour);
					BossCrabMovmentController controller = fishBehaviour.gameObject.GetComponent<BossCrabMovmentController>();
					controller.enabled = true;
					float num4 = (float)num2 + (float)num3 / 1000f;
					UnityEngine.Debug.Log($"seed:[{num2}],survivalTime:[{num3}],arg:[{num4}]");
					controller.SetStartArg(num4);
					FishBehaviour fishBehaviour2 = fishBehaviour;
					fishBehaviour2.Event_FishDying_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDying_Handler, (Action<FishBehaviour>)delegate
					{
						controller.enabled = false;
					});
					controller.Play();
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("NewKingCrabBoss", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_NewDeepSeaOctopusBossPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_NewDeepSeaOctopusBossPush");
			if (debugFish.IgnoreNewDeepSeaOctopusBoss)
			{
				return;
			}
			NetMsgInfo_RecvNewDeepSeaOctopus netMsgInfo_RecvNewDeepSeaOctopus = new NetMsgInfo_RecvNewDeepSeaOctopus(args);
			try
			{
				netMsgInfo_RecvNewDeepSeaOctopus.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (netMsgInfo_RecvNewDeepSeaOctopus.valid && netMsgInfo_RecvNewDeepSeaOctopus.code == 0)
			{
				int fishID = netMsgInfo_RecvNewDeepSeaOctopus.item.fishID;
				if (!(FishMgr.Get().GetFishById(fishID) != null))
				{
					int pathID = netMsgInfo_RecvNewDeepSeaOctopus.item.pathID;
					int remainTime = netMsgInfo_RecvNewDeepSeaOctopus.item.remainTime;
					float num = (float)remainTime / 1000f;
					num -= 2f;
					if (!(num <= 0f))
					{
						FishMgr.Get().SetFishIndex(fishID);
						UnityEngine.Debug.Log($"seed:[{fishID}],survivalTime:[{pathID}],arg:[{num}]");
						FishBehaviour fishBehaviour = FishMgr.Get().SpawnSingleFish(FishType.Boss_Kraken_深海八爪鱼);
						FishBindEventHandler(fishBehaviour);
						num -= 1f;
						fishBehaviour.GetComponent<KrakenBeheviour>().SetPositionAndRemainTime(pathID, num);
					}
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("newDeepSeaOctopusBossPush", netMsgInfo_RecvNewDeepSeaOctopus);
			}
		}

		private IEnumerator IE_Test_Crab()
		{
			yield break;
		}

		private void GenCrab(int fishId, int seed, int survivalTime)
		{
			FishMgr.Get().SetFishIndex(fishId);
			FishBehaviour fishBehaviour = FishMgr.Get().SpawnSingleFish(FishType.Boss_Crab_霸王蟹);
			FishBindEventHandler(fishBehaviour);
			BossCrabMovmentController controller = fishBehaviour.gameObject.GetComponent<BossCrabMovmentController>();
			controller.enabled = true;
			FishBehaviour fishBehaviour2 = fishBehaviour;
			fishBehaviour2.Event_FishDying_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDying_Handler, (Action<FishBehaviour>)delegate
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
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int newScenarioType = (int)netMsgInfoBase.basicDic["scene"];
				int stage = (int)netMsgInfoBase.basicDic["stage"];
				int stageType = (int)netMsgInfoBase.basicDic["stageType"];
				GunStatusControl(stage);
				fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().ScenarioLogic((ScenarioType)newScenarioType, stage, stageType);
			}
			else
			{
				_NetMsg_CheckErrorLog("changeStage", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_PlayerScenePush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_PlayerScenePush");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int num = (int)netMsgInfoBase.basicDic["scene"];
				int stage = (int)netMsgInfoBase.basicDic["stage"];
				int stageType = (int)netMsgInfoBase.basicDic["stageType"];
				int num2 = (int)netMsgInfoBase.basicDic["sceneDuration"];
				int stageDuration = (int)netMsgInfoBase.basicDic["stageDuration"];
				GunStatusControl(stage, num2);
				fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().RefreshScenario((ScenarioType)num, stage, stageType);
				fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().SetStage((ScenarioType)num, stage, stageType, num2, stageDuration);
			}
			else
			{
				_NetMsg_CheckErrorLog("playerScenePush", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserFired(object[] args)
		{
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int gameScore = (int)netMsgInfoBase.basicDic["gameScore"];
				RefreshUserScore(m_context.curSeatId, gameScore);
			}
			else
			{
				_NetMsg_CheckErrorLog("userFired", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserRotateEM(object[] args)
		{
			HandleNetMsg_Default("userRotateEM", args);
		}

		private void HandleNetMsg_UserRotateEMPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UserRotateEMPush");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				float angle = float.Parse(string.Empty + netMsgInfoBase.basicDic["angle"]);
				int id = (int)netMsgInfoBase.basicDic["seatId"];
				GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(id);
				gunById.RotateEMGun(angle);
			}
			else
			{
				_NetMsg_CheckErrorLog("userRotateEMPush", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserFiredEM(object[] args)
		{
			HandleNetMsg_Default("userFiredEM", args);
		}

		private void HandleNetMsg_UserFiredEMPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UserFiredEMPush");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				float angle = float.Parse(string.Empty + netMsgInfoBase.basicDic["angle"]);
				int id = (int)netMsgInfoBase.basicDic["seatId"];
				GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(id);
				gunById.RotateEMGun(angle);
				gunById.RemoteShootEM();
			}
			else
			{
				_NetMsg_CheckErrorLog("userFiredEMPush", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserFiredPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UserFiredPush");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int gameScore = (int)netMsgInfoBase.basicDic["gameScore"];
				string text = (string)netMsgInfoBase.basicDic["fired"];
				string[] array = text.Split('#');
				int num = int.Parse(array[0]);
				int power = int.Parse(array[1]);
				float angle = float.Parse(array[2]);
				int num2 = int.Parse(array[3]);
				int num3 = int.Parse(array[4]);
				RefreshUserScore(num3, gameScore);
				GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(num3);
				GunUIController gunByID = GunUIMgr.Get().GetGunByID(num3);
				otherGunValue = power;
				gunById.SetPower(power);
				gunByID.SetPower(power);
				gunById.RotateNormalGun(angle);
				if (gunById.GetGunType() != 0)
				{
					gunById.ChangeGunRemote(GunType.NormalGun);
				}
				gunById.Shoot();
			}
			else
			{
				_NetMsg_CheckErrorLog("userFiredPush", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_BulletHitFish(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_BulletHitFish");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				string text = (string)netMsgInfoBase.basicDic["deadFishs"];
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
					string[] array2 = text2.Split('-');
					string[] array3 = array2;
					foreach (string text3 in array3)
					{
						string[] array4 = text3.Split('#');
						int fishId = int.Parse(array4[0]);
						int fishType = int.Parse(array4[1]);
						int fishRate = int.Parse(array4[2]);
						DoFishDead(new FishDeadInfo
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
				_NetMsg_CheckErrorLog("bulletHitFish", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_GameGold(object[] args)
		{
			UnityEngine.Debug.Log("InGame HandleNetMsg_UserAward");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int curGold = (int)netMsgInfoBase.basicDic["gameGold"];
				m_uiContext.curGold = curGold;
				RefreshNativeUserGameNumber(m_uiContext.curScore, HW2_GVars.user.expeGold, m_uiContext.curGold);
			}
			else
			{
				_NetMsg_CheckErrorLog("gameGold", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserCoinIn(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UserCoinIn");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			try
			{
				netMsgInfoBase.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int gameScore = (int)netMsgInfoBase.basicDic["gameScore"];
				int expeGold = (int)netMsgInfoBase.basicDic["expeGold"];
				int gameGold = (int)netMsgInfoBase.basicDic["gameGold"];
				RefreshNativeUserGameNumber(gameScore, expeGold, gameGold);
				HW2_Singleton<SoundMgr>.Get().PlayClip("玩家上分音效");
			}
			else
			{
				_NetMsg_CheckErrorLog("userCoinIn", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_UserCoinOut(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UserCoinOut");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			try
			{
				netMsgInfoBase.Parse();
				if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
				{
					int gameScore = (int)netMsgInfoBase.basicDic["gameScore"];
					int expeGold = (int)netMsgInfoBase.basicDic["expeGold"];
					int gameGold = (int)netMsgInfoBase.basicDic["gameGold"];
					RefreshNativeUserGameNumber(gameScore, expeGold, gameGold);
					HW2_Singleton<SoundMgr>.Get().PlayClip("玩家上分音效");
				}
				else
				{
					_NetMsg_CheckErrorLog("userCoinOut", netMsgInfoBase);
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		private void HandleNetMsg_GunHitFishInAction(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_GunHitFishInAction");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int num = (int)netMsgInfoBase.basicDic["actionId"];
				int num2 = (int)netMsgInfoBase.basicDic["actionFlag"];
				string text = (string)netMsgInfoBase.basicDic["deadFishs"];
				List<DeadFishData> list = new List<DeadFishData>();
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
						string[] array2 = text2.Split('-');
						string[] array3 = array2;
						foreach (string text3 in array3)
						{
							string[] array4 = text3.Split('#');
							int fishId = int.Parse(array4[0]);
							FishType fishType = (FishType)int.Parse(array4[1]);
							int num5 = int.Parse(array4[2]);
							FishBehaviour fishById = FishMgr.Get().GetFishById(fishId);
							if (!(null == fishById))
							{
								int score = num5 * num3;
								DeadFishData deadFishData = new DeadFishData(fishById, num3, num5, score);
								list.Add(deadFishData);
							}
						}
					}
					RefreshUserScore(seatId, gameScore);
				}
				switch (num)
				{
				case 10:
					foreach (DeadFishData item2 in list)
					{
						m_bombCrabAction.totalScore += item2.score;
						if (item2.fish.type == FishType.GoldShark_霸王鲸 || FishMgr.bigFishSet.Contains(item2.fish.type))
						{
							m_bombCrabAction.waitBigFishShowTime = 9f;
						}
					}
					m_bombCrabAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 11:
					foreach (DeadFishData item3 in list)
					{
						m_lightingFishAction.totalScore += item3.score;
						if (item3.fish.type == FishType.GoldShark_霸王鲸 || FishMgr.bigFishSet.Contains(item3.fish.type))
						{
							m_lightingFishAction.hasBigFish = true;
						}
					}
					m_lightingFishAction.OnActionMsgReturn(num2 == 1, list);
					break;
				case 12:
					foreach (DeadFishData item4 in list)
					{
						m_laserCrabAction.totalScore += item4.score;
						if (FishMgr.bossFishSet.Contains(item4.fish.type))
						{
							m_laserCrabAction.waitBigFishOrBossShowTime = 11.5f + (float)(item4.fishRate - 100) / 30f * 0.5f;
							m_laserCrabAction.hasBoss = true;
						}
						else if ((item4.fish.type == FishType.GoldShark_霸王鲸 || FishMgr.bigFishSet.Contains(item4.fish.type)) && !m_laserCrabAction.hasBoss)
						{
							m_laserCrabAction.waitBigFishOrBossShowTime = 9f;
							m_laserCrabAction.hasBigFish = true;
						}
					}
					m_laserCrabAction.OnActionMsgReturn(num2 == 1, list);
					break;
				}
			}
			else
			{
				m_lightingFishAction.OnActionMsgReturn(canContinue: false, new List<DeadFishData>());
				_NetMsg_CheckErrorLog("gunHitFishInAction", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_GunHitFishInActionPush(object[] args)
		{
			HandleNetMsg_GunHitFishInAction(args);
		}

		private void HandleNetMsg_LeaveSeat(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_LeaveSeat");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int num = (int)netMsgInfoBase.basicDic["gameGold"];
				int num2 = (int)netMsgInfoBase.basicDic["expeGold"];
				int gameScore = (int)netMsgInfoBase.basicDic["gameScore"];
				RefreshUserGold(num, num2);
				RefreshNativeUserGameNumber(gameScore, num2, num);
				QuitToRoom();
			}
			else
			{
				_NetMsg_CheckErrorLog("leaveSeat", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_QuitToDesk(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_QuitToDesk");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int num = (int)netMsgInfoBase.basicDic["gameGold"];
				int num2 = (int)netMsgInfoBase.basicDic["expeGold"];
				int gameScore = (int)netMsgInfoBase.basicDic["gameScore"];
				int num3 = (int)netMsgInfoBase.basicDic["quitType"];
				RefreshUserGold(num, num2);
				RefreshNativeUserGameNumber(gameScore, num2, num);
				switch (num3)
				{
				case 1:
					HW2_AlertDialog.Get().ShowDialog("该桌已被删除，请重新选桌", showOkCancel: false, QuitToRoom);
					break;
				case 2:
					HW2_AlertDialog.Get().ShowDialog("连接异常，请重新选桌", showOkCancel: false, QuitToRoom);
					break;
				case 3:
					HW2_AlertDialog.Get().ShowDialog("由于您长时间未操作，已自动退出该桌", showOkCancel: false, QuitToRoom);
					break;
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("quitToDesk", netMsgInfoBase);
			}
		}

		private void RefreshUserGold(int gold, int exp)
		{
			HW2_GVars.user.gameGold = gold;
			HW2_GVars.user.expeGold = exp;
		}

		private void QuitToRoom()
		{
			SimpleSingletonBehaviour<HW2_UserInfoInGame>.GetInstance().Hide();
			CleanScene();
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().ResetScenario();
			HW2_Singleton<SoundMgr>.Get().StopAllSounds();
			HW2_MessageCenter.UnRegisterHandle("CheckAutoLock", CheckAutoLock);
			HW2_MessageCenter.UnRegisterHandle("SetCursorUsageLinearSpeed", OnSpeedUpAll);
		}

		private void HandleNetMsg_UpdateDeskInfoPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_UpdateDeskInfoPush");
			NetMsgInfo_UpdateDeskInfoPush netMsgInfo_UpdateDeskInfoPush = new NetMsgInfo_UpdateDeskInfoPush(args);
			netMsgInfo_UpdateDeskInfoPush.Parse();
			if (netMsgInfo_UpdateDeskInfoPush.valid && netMsgInfo_UpdateDeskInfoPush.code == 0)
			{
				HW2_GVars.lobby.inGameSeats = netMsgInfo_UpdateDeskInfoPush.seats;
				DeskInfo curDesk = HW2_GVars.lobby.GetCurDesk();
				foreach (SeatInfo2 seat in netMsgInfo_UpdateDeskInfoPush.seats)
				{
					SetPlayerGun(curDesk, seat);
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("updateDeskInfoPush", netMsgInfo_UpdateDeskInfoPush);
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
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int id = (int)netMsgInfoBase.basicDic["seatId"];
				int targetType = (int)netMsgInfoBase.basicDic["gunType"];
				GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(id);
				if (!gunById.IsNative())
				{
					gunById.ChangeGunRemote((GunType)targetType);
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("changeGunPush", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_GunHitLockFish(object[] args)
		{
			HandleNetMsg_Default("gunHitLockFish", args);
		}

		private void HandleNetMsg_BulletHitLockFishPush(object[] args)
		{
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				string text = netMsgInfoBase.basicDic["deadFishs"] as string;
				string[] array = text.Split(',');
				int bulletId = int.Parse(array[0]);
				int bulletPower = int.Parse(array[1]);
				int num = int.Parse(array[2]);
				int addScore = int.Parse(array[3]);
				int num2 = int.Parse(array[4]);
				RefreshUserScore(num, num2);
				GunUIController gunByID = GunUIMgr.Get().GetGunByID(num);
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
						DoFishDead(new FishDeadInfo
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
				_NetMsg_CheckErrorLog("changeGunPush", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_LockSelectFish(object[] args)
		{
			HandleNetMsg_Default("lockSelectFish", args);
		}

		private void HandleNetMsg_LockSelectFishPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_LockSelectFishPush");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				int id = (int)netMsgInfoBase.basicDic["seatId"];
				int fishId = (int)netMsgInfoBase.basicDic["fishId"];
				GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(id);
				FishBehaviour fishById = FishMgr.Get().GetFishById(fishId);
				GunUIController gunByID = GunUIMgr.Get().GetGunByID(id);
				int num = (int)netMsgInfoBase.basicDic["gunValue"];
				gunByID.SetPower(num);
				gunById.SetPower(num);
				UnityEngine.Debug.LogError("锁定炮值:" + num);
				if (!(gunById == null) && !(fishById == null) && !gunById.IsNative())
				{
					if (gunById.GetGunType() != GunType.LockingGun)
					{
						gunById.ChangeGunRemote(GunType.LockingGun);
					}
					gunById.LockFish(fishById);
					HW2_Singleton<SoundMgr>.Get().PlayClip("锁定炮连接2", loop: true);
					HW2_Singleton<SoundMgr>.Get().SetVolume("锁定炮连接2", 0.25f);
				}
			}
			else
			{
				_NetMsg_CheckErrorLog("lockSelectFishPush", netMsgInfoBase);
			}
		}

		private void HandleNetMsg_BombCrabNextPoint(object[] args)
		{
			HandleNetMsg_Default("bombCrabNextPoint", args);
		}

		private void HandleNetMsg_BombCrabNextPointPush(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_BombCrabNextPointPush");
			NetMsgInfoBase netMsgInfoBase = new NetMsgInfoBase(args);
			netMsgInfoBase.Parse();
			if (netMsgInfoBase.valid && netMsgInfoBase.code == 0)
			{
				float x = float.Parse(netMsgInfoBase.basicDic["x"].ToString());
				float y = float.Parse(netMsgInfoBase.basicDic["y"].ToString());
				int num = (int)netMsgInfoBase.basicDic["seatId"];
				m_bombCrabAction.OnMsgBombCrabNextPointPush(x, y);
			}
			else
			{
				_NetMsg_CheckErrorLog("bombCrabNextPointPush", netMsgInfoBase);
			}
		}

		private void FishBindEventHandler(FishBehaviour fish)
		{
			fish.Event_FishOnDoubleClick_Handler = (Action<FishBehaviour>)Delegate.Combine(fish.Event_FishOnDoubleClick_Handler, new Action<FishBehaviour>(Handle_FishOnDoubleClick));
			fish.Event_FishOnHit_Handler += Handle_FishHitByBullet;
		}

		private void DoFishDead(FishDeadInfo deadInfo)
		{
			UnityEngine.Debug.Log($"DoFishDead fishId:[{deadInfo.fishId}], fishType:[{(FishType)deadInfo.fishType}], killerId:[{deadInfo.killerSeatId}]");
			FishBehaviour fishById = FishMgr.Get().GetFishById(deadInfo.fishId);
			if (fishById == null)
			{
				UnityEngine.Debug.Log($"fish[id:{deadInfo.fishId}] not find.Ignore!");
			}
			else if (fishById.type == (FishType)deadInfo.fishType)
			{
				if (fishById.isLightning)
				{
					Action<FishBehaviour, FishDeadInfo> fishDeadAction = delegate(FishBehaviour _fish, FishDeadInfo _deadInfo)
					{
						KillFishController controllerById4 = SimpleSingletonBehaviour<KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
						controllerById4.DoDeadFish(new KillFishData
						{
							fish = _fish,
							fishId = _deadInfo.fishId,
							seatId = deadInfo.killerSeatId,
							fishType = _fish.type
						}, _deadInfo);
					};
					m_lightingFishAction.Play(fishById, fishDeadAction, deadInfo.killerSeatId, deadInfo.bulletPower);
				}
				else if (fishById.type == FishType.CrabBoom_连环炸弹蟹)
				{
					Action<FishBehaviour, FishDeadInfo> fishDeadAction2 = delegate(FishBehaviour _fish, FishDeadInfo _deadInfo)
					{
						KillFishController controllerById3 = SimpleSingletonBehaviour<KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
						controllerById3.DoDeadFish(new KillFishData
						{
							fish = _fish,
							bulletPower = _deadInfo.bulletPower,
							fishId = _deadInfo.fishId,
							seatId = deadInfo.killerSeatId,
							fishType = _fish.type
						}, _deadInfo);
					};
					m_bombCrabAction.Play(fishById, fishDeadAction2, deadInfo.killerSeatId, deadInfo.bulletPower);
				}
				else if (fishById.type == FishType.CrabLaser_电磁蟹)
				{
					Action<FishBehaviour, FishDeadInfo> fishDeadAction3 = delegate(FishBehaviour _fish, FishDeadInfo _deadInfo)
					{
						KillFishController controllerById2 = SimpleSingletonBehaviour<KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
						controllerById2.DoDeadFish(new KillFishData
						{
							fish = _fish,
							bulletPower = _deadInfo.bulletPower,
							fishId = _deadInfo.fishId,
							seatId = deadInfo.killerSeatId,
							fishType = _fish.type
						}, _deadInfo);
					};
					m_laserCrabAction.Play(fishById, fishDeadAction3, deadInfo.killerSeatId, deadInfo.bulletPower);
				}
				else
				{
					KillFishController controllerById = SimpleSingletonBehaviour<KillFishMgr>.Get().GetControllerById(deadInfo.killerSeatId);
					controllerById.DoDeadFish(new KillFishData
					{
						fish = fishById,
						fishId = deadInfo.fishId,
						seatId = deadInfo.killerSeatId,
						fishType = fishById.type
					}, deadInfo);
				}
			}
		}

		private void Handle_FishHitByBullet(FishBehaviour fish, Collider bullet)
		{
			if (!m_context.isPlaying)
			{
				UnityEngine.Debug.LogError("================isPlaying: " + m_context.isPlaying);
				return;
			}
			BulletController component = bullet.GetComponent<BulletController>();
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
				UnityEngine.Debug.LogError(fish.name + " fish.State != FishState.Live");
				return;
			}
			component.used = true;
			Effect_ExplodeMgr.Get().SpawnExplode(bullet.transform.position);
			component.Over();
			if (m_useNet && component.seatId == fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun().GetId())
			{
				int bulletId = component.bulletId;
				string text = $"{fish.id}#{(int)fish.type}";
				object[] args = new object[2]
				{
					bulletId,
					text
				};
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/gunHitFish", args);
			}
		}

		private void GunLogic_Handle_GunRotateEM(float gunAngle)
		{
			object[] args = new object[1]
			{
				gunAngle
			};
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/userRotateEM", args);
		}

		private void GunLogic_Handle_GunShootEM(float gunAngle)
		{
			object[] args = new object[1]
			{
				gunAngle
			};
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/userFiredEM", args);
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
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/userFired", args);
				BulletMgr.Get().Shoot(pos, gunAngle, angleOffset, gunValue, seatId, _bulletId);
				RefreshUserScore(m_context.curSeatId, m_uiContext.curScore - gunValue);
				Judge_GameScore_AllowShoot();
			}
		}

		private void GunLogic_Handle_GunLockShoot(KeyValueInfo keyValueInfo)
		{
			if (m_context.isPlaying)
			{
				LockGunShootInfo lockGunShootInfo = JsonUtility.FromJson<LockGunShootInfo>(keyValueInfo._value.ToString());
				_bulletId++;
				string text = $"{_bulletId},{lockGunShootInfo._gunValue},{lockGunShootInfo._gunAngle},{lockGunShootInfo._gunType}";
				string text2 = $"{lockGunShootInfo._fishId}#{lockGunShootInfo._fishType}";
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/gunHitLockFish", new object[2]
				{
					text,
					text2
				});
			}
		}

		private void GunLogic_Handle_GunLockSelect(KeyValueInfo keyValueInfo)
		{
			LockGunSelectInfo lockGunSelectInfo = JsonUtility.FromJson<LockGunSelectInfo>(keyValueInfo._value.ToString());
			HW2_Singleton<SoundMgr>.Get().PlayClip("锁定炮连接2", loop: true);
			HW2_Singleton<SoundMgr>.Get().SetVolume("锁定炮连接2", 0.25f);
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/lockSelectFish", new object[2]
			{
				lockGunSelectInfo._fishId,
				lockGunSelectInfo._gunValue
			});
		}

		private void CheckAutoLock(KeyValueInfo keyVauleInfo)
		{
			if (nativeGun != null)
			{
				nativeGun.CheckAutoLock();
			}
		}

		private void PlayerLogic_Handle_PlayerChangeGunPower(PlayerUI playerUI)
		{
			PlayerController player = playerUI.GetPlayer();
			player.GetGun().UserSwitchGun();
		}

		private void GunLogic_Handle_OnChangeAuto(GunBehaviour gun, bool isAuto)
		{
			UnityEngine.Debug.Log("GunLogic_Handle_OnChangeAuto");
			SetBtnAuto(isAuto);
			if (gun.CanLocking() && isAuto)
			{
				LockChainController lockChain = EffectMgr.Get().GetLockChain(gun.GetId());
				gun.AutoLock(lockChain);
			}
		}

		private void SetBtnAuto(bool isAuto)
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("选座选厅自动发炮");
			HW2_Singleton<SoundMgr>.Get().SetVolume("选座选厅自动发炮", 1f);
			if (isAuto)
			{
				m_btnAuto.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/quXiaoZiDong");
				m_btnAuto.transform.Find("VFX_Automatic").gameObject.SetActive(value: true);
			}
			else
			{
				m_btnAuto.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/zidong2");
				m_btnAuto.transform.Find("VFX_Automatic").gameObject.SetActive(value: false);
			}
		}

		private void Handle_UI_BtnChangeGun()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("切换炮值");
			UnityEngine.Debug.Log("Handle_UI_BtnChangeGun");
			fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun().DoChangeGun(delegate(GunType from, GunType to)
			{
				if (from == GunType.LockingGun)
				{
					fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun().StopLock();
				}
				if (to == GunType.LockingGun)
				{
					GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
					LockChainController lockChain = EffectMgr.Get().GetLockChain(gunBehaviour.GetId());
					gunBehaviour.CheckAutoLock();
				}
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/changeGun", new object[1]
				{
					(to != 0) ? 1 : 0
				});
			});
		}

		private void Handle_UI_BtnAuto()
		{
			UnityEngine.Debug.LogError("点击自动发pao");
			if (m_uiContext.curScore == 0)
			{
				SimpleSingletonBehaviour<OptionController>.Get().Show_InOutPanel();
				return;
			}
			GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			if (gunBehaviour.IsAuto())
			{
				gunBehaviour.DoCancelAuto();
			}
			else
			{
				gunBehaviour.DoAuto();
			}
		}

		private bool GunLogic_CanAuto()
		{
			GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			return true;
		}

		private void Handle_UI_BtnPlusPower(SpriteButton btn)
		{
			UnityEngine.Debug.Log("Handle_UI_BtnPlusPower");
			GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			GunUIController gunByID = GunUIMgr.Get().GetGunByID(HW2_GVars.lobby.curSeatId);
			int power = gunBehaviour.GetPower();
			GunPlayerData playerData = gunBehaviour.GetPlayerData();
			if (power < playerData.minGunValue)
			{
				gunBehaviour.SetPower(playerData.minGunValue);
				gunByID.SetPower(playerData.minGunValue);
			}
			else if (power == playerData.maxGunValue)
			{
				gunBehaviour.SetPower(playerData.minGunValue);
				gunByID.SetPower(playerData.minGunValue);
			}
			else
			{
				int addstepGunValue = playerData.addstepGunValue;
				int num = power + addstepGunValue;
				num = ((num > playerData.maxGunValue) ? playerData.maxGunValue : num);
				gunBehaviour.SetPower(num);
				gunByID.SetPower(num);
			}
			if (power + playerData.addstepGunValue == playerData.maxGunValue)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("炮值加到最大时的音效");
			}
			else
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("切换炮值");
			}
		}

		private void Handle_UI_BtnMinusPower(SpriteButton btn)
		{
			GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			GunUIController gunByID = GunUIMgr.Get().GetGunByID(HW2_GVars.lobby.curSeatId);
			int power = gunBehaviour.GetPower();
			GunPlayerData playerData = gunBehaviour.GetPlayerData();
			if (power < playerData.minGunValue)
			{
				gunBehaviour.SetPower(playerData.minGunValue);
				gunByID.SetPower(playerData.minGunValue);
			}
			else if (power == playerData.minGunValue)
			{
				gunBehaviour.SetPower(playerData.maxGunValue);
				gunByID.SetPower(playerData.maxGunValue);
			}
			else
			{
				int num = -playerData.addstepGunValue;
				int num2 = power + num;
				num2 = ((num2 < playerData.minGunValue) ? playerData.minGunValue : num2);
				gunBehaviour.SetPower(num2);
				gunByID.SetPower(num2);
			}
			if (power == playerData.minGunValue)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("炮值加到最大时的音效");
			}
			else
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("切换炮值");
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
			GunBehaviour nativeGun = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			if (nativeGun == null)
			{
				yield return null;
				nativeGun = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			}
			if (wait > 0f)
			{
				try
				{
					nativeGun.allowShoot = false;
					if (nativeGun.GetGunType() == GunType.LockingGun)
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
				GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
				gunBehaviour.allowShoot = false;
				if (gunBehaviour.GetGunType() == GunType.LockingGun)
				{
					gunBehaviour.StopLock();
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		private void InGameConnectSuccess()
		{
			UnityEngine.Debug.Log("重置炮台状态");
			GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			Judge_GameScore_AllowShoot();
			if (gunBehaviour.allowShoot)
			{
				gunBehaviour.CheckAutoLock();
			}
			CleanBossCrabsAndDeepSeaOcean();
		}

		private void CleanBossCrabsAndDeepSeaOcean(bool send = false)
		{
			FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FishBehaviour _fish)
			{
				if (_fish.type == FishType.Boss_Crab_霸王蟹 || _fish.type == FishType.Boss_Kraken_深海八爪鱼)
				{
					_fish.Die();
				}
			});
			if (send)
			{
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/askForBossInfo", new object[0]);
			}
		}

		private void RefreshNativeUserGameNumber(int gameScore, int expeGold, int gameGold)
		{
			m_uiContext.curScore = gameScore;
			m_uiContext.curGold = (m_uiContext.isExpeGold ? expeGold : gameGold);
			if (!m_uiContext.isExpeGold)
			{
				HW2_GVars.lobby.user.gameGold = gameGold;
			}
			else
			{
				HW2_GVars.lobby.user.expeGold = expeGold;
			}
			RefreshUserScore(m_context.curSeatId, gameScore);
			UnityEngine.Debug.Log($"seatid:{m_context.curSeatId},gameScore:{gameScore}");
		}

		private void RefreshUserScore(int seatId, int gameScore)
		{
			GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(seatId);
			GunUIController gunByID = GunUIMgr.Get().GetGunByID(seatId);
			GunBehaviour gunById2 = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(seatId);
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
				SimpleSingletonBehaviour<OptionController>.Get().Show_InOutPanel();
			}
		}

		private void Handle_FishOnDoubleClick(FishBehaviour fish)
		{
			UnityEngine.Debug.Log($"Handle_FishOnDoubleClick> fish:{fish.identity}");
			GunBehaviour gunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			if (fish.State == FishState.Live && gunBehaviour.CanLocking() && fish.inScreen)
			{
				gunBehaviour.LockFish(fish);
			}
		}

		private void OnSpeedUpAll(KeyValueInfo keyValueInfo)
		{
			fiSimpleSingletonBehaviour<FormationMgr<FishType>>.Get().StopRunningFormations();
		}

		private void Handle_UI_BtnReturn()
		{
			object[] args = new object[0];
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/quitGame", args);
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
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/userCoinIn", args);
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
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/userCoinOut", args);
		}

		private void Handle_BackToLobby()
		{
			Reset_Option();
			object[] args = new object[2]
			{
				m_context.curDesk.id,
				m_context.curSeatId
			};
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/leaveSeat", args);
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
					HW2_GVars.dontDestroyOnLoadList.Add(item2.gameObject);
				}
			}
			HW2_MB_Singleton<GetDaTingRoot>.GetInstance().KeepOnLoad();
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
			HW2_MB_Singleton<GetDaTingRoot>.GetInstance().DisableScene();
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
			HW2_MB_Singleton<GetDaTingRoot>.GetInstance().EnableScene();
			UnityEngine.Debug.LogError("加载提示组件");
			UIIngameManager.GetInstance().OpenUI("AlertDialog2");
			UIIngameManager.GetInstance().CloseUI("AlertDialog2");
			UIIngameManager.GetInstance().OpenUI("Notice");
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
			bool faceDown = HW2_GVars.lobby.curSeatId > 2;
			m_bombCrabAction.DoReset();
			m_bombCrabAction.SetFaceDown(faceDown);
			m_lightingFishAction.SetFaceDown(faceDown);
			EnableScene();
			Init_Context();
			Init_Gun();
			Init_Game();
			Init_HeadTitle();
			HW2_GVars.m_curState = Demo_UI_State.InGame;
			UnityEngine.Debug.Log("StoryScenarioMgr.Get(): " + fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get());
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().Init(HW2_GVars.lobby.curSeatId > 2);
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().SetScenario((ScenarioType)num);
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().SetStage((ScenarioType)num, stage, stageType, num2, stageDuration);
			HW2_Singleton<SoundMgr>.Get().PlayClip("水流声背景音乐", loop: true);
			HW2_Singleton<SoundMgr>.Get().SetVolume("水流声背景音乐", 1f);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("gameGoldPush", HandleNetMsg_GameGold);
			HW2_MB_Singleton<SynchronizeHint>.Get().Show();
			HW2_MessageCenter.RegisterHandle("SetCursorUsageLinearSpeed", OnSpeedUpAll);
		}

		private IEnumerator WaitStart()
		{
			while (ZH2_GVars.isCanSenEnterGame)
			{
				yield return new WaitForSeconds(2.5f);
				try
				{
					WebSocket2.GetInstance().SenEnterGame(isEnterGame: true, ZH2_GVars.GameType.fish_king_desk3, FK3_GVars.lobby.curDeskId.ToString(), FK3_GVars.lobby.user.gameScore.ToString());
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("错误: " + arg);
				}
			}
		}

		private void TestLogic_Add_AutoKeepScoreLogic()
		{
			UnityEngine.Debug.Log("TestLogic_Add_AutoKeepScoreLogic");
			AutoKeepScoreLogic autoKeepScoreLogic = GetComponent<AutoKeepScoreLogic>();
			if (autoKeepScoreLogic == null)
			{
				autoKeepScoreLogic = base.gameObject.AddComponent<AutoKeepScoreLogic>();
			}
			UnityEngine.Debug.Log($"uiContext:{m_uiContext},context:{m_context}");
			if (!autoKeepScoreLogic.CheckValid())
			{
				autoKeepScoreLogic.Init(m_uiContext, m_context);
			}
		}

		private void Init_HeadTitle()
		{
			SimpleSingletonBehaviour<HeadTitleController>.Get().Init();
		}

		private void Reset_Option()
		{
			SimpleSingletonBehaviour<OptionController>.Get().Init();
			SimpleSingletonBehaviour<SendChatController>.Get().CloseChatWindow();
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

		private void SwithInCleanSceneLong(KeyValueInfo keyValueInfo)
		{
			m_lightingFishAction.Stop();
			m_laserCrabAction.Stop();
			m_bombCrabAction.Stop();
			FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FishBehaviour _fish)
			{
				_fish.Die();
			});
			Effect_CoinMgr.Get().RemoveAllCoins();
			Effect_ScoreMgr.Get().RemoveAllScores();
			BulletMgr.Get().RemoveAllBullets();
			SimpleSingletonBehaviour<KillFishMgr>.Get().StopAllCoroutines();
			EffectMgr.Get().ResetAllLockChains();
			Effect_LogoMgr.Get().RemoveAllLogos();
			_bulletId = 0;
			CleanBossCrabsAndDeepSeaOcean(send: true);
		}

		private void SwithInCleanSceneShort(KeyValueInfo keyValueInfo)
		{
			CleanBossCrabsAndDeepSeaOcean(send: true);
		}

		private void CleanScene()
		{
			m_context.isPlaying = false;
			DisableScene();
			m_lightingFishAction.Stop();
			m_laserCrabAction.Stop();
			m_bombCrabAction.Stop();
			FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FishBehaviour _fish)
			{
				_fish.Die();
			});
			fiSimpleSingletonBehaviour<GunMgr>.Get().ForeachGun(delegate(GunBehaviour _gun)
			{
				_gun.Reset_EventHandler();
				_gun.Reset_Gun();
			});
			GunUIMgr.Get().ClearGunsUI();
			Effect_CoinMgr.Get().RemoveAllCoins();
			Effect_ScoreMgr.Get().RemoveAllScores();
			BulletMgr.Get().RemoveAllBullets();
			SimpleSingletonBehaviour<KillFishMgr>.Get().StopAllCoroutines();
			EffectMgr.Get().ResetAllLockChains();
			Effect_LogoMgr.Get().RemoveAllLogos();
			_bulletId = 0;
			base.gameObject.SetActive(value: false);
			AppSceneMgr.RunAction("Lobby.EnterLobby", null, delegate
			{
			});
		}
	}
}
