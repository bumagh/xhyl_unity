using DG.Tweening;
using FullInspector;
using HW3L;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Demo;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.Message;
using M__M.HaiWang.Tests.Refactoring.Misc;
using M__M.HaiWang.UIDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Player.Gun
{
	public class FK3_GunBehaviour : BaseBehavior<FullSerializerSerializer>
	{
		public delegate void EventHandler_RotateByInput(FK3_GunBehaviour gun, Transform begin, Vector3 end, float angle);

		public delegate void EventHandler_Shoot(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId);

		public delegate void EventHandler_ShootDR(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId);

		public delegate void EventHandler_ChangeAuto(FK3_GunBehaviour gun, bool isAuto);

		public delegate void EventHandler_ShootEM(float gunAngle);

		public delegate void EventHandler_RotateEM(float gunAngle);

		[Serializable]
		public class FishTypeSprite
		{
			public FK3_FishType fishType;

			public Sprite sprite;
		}

		[SerializeField]
		private Image m_labelLeft;

		[NotSerialized]
		[ShowInInspector]
		[SerializeField]
		private Image m_labelRight;

		[SerializeField]
		private FK3_SpriteButton m_btnPlusPower;

		[SerializeField]
		private FK3_SpriteButton m_btnMinusPower;

		[SerializeField]
		private Transform m_transBase;

		private Transform baseEffect1;

		private Transform baseEffect2;

		[SerializeField]
		private Transform m_powerWindow;

		private FK3_SpriteButton btnChangeSkin;

		[SerializeField]
		private Transform m_transNormalBarrel;

		[SerializeField]
		private Transform m_transGunBarrel;

		[SerializeField]
		private Transform m_transLockingBarrel;

		[SerializeField]
		private Image m_LockingFishSprite;

		[SerializeField]
		private Transform m_transEMBarrel;

		[SerializeField]
		public Transform m_transDRBarrel;

		private Transform m_transDR_Projectile;

		[SerializeField]
		private Transform m_transEMBarrelTagPos;

		[SerializeField]
		private Transform m_transDRBarrelTagPos;

		[SerializeField]
		private Transform m_EMBornPos;

		[SerializeField]
		private Transform m_DRBornPos;

		private Animator emAnimator;

		private Animator drAnimator;

		[SerializeField]
		public Transform m_transEMBarrel_Attach;

		private Transform m_laserRange;

		private Transform m_laserStorage;

		private Transform m_laserFire;

		[SerializeField]
		private Transform m_EMBackCountTr;

		[SerializeField]
		private Transform m_DRBackCountTr;

		private Animator m_laserFireAni;

		private FK3_GunType m_preGunType;

		private bool pre_allowShoot;

		private Sequence m_enterEMGunsequence;

		private Sequence m_enterDRGunsequence;

		[SerializeField]
		private Transform m_lockingPoint;

		[SerializeField]
		private Transform m_rotatePivot;

		[SerializeField]
		private Text m_textScore;

		[SerializeField]
		private Text m_textName;

		[SerializeField]
		private Text m_textPower;

		[SerializeField]
		private GameObject _barrelRoot;

		[SerializeField]
		private GameObject _input;

		private Vector3 SelfPosition = new Vector3(0f, 0f, 0f);

		private Vector3 FarPosition = new Vector3(-360f, 320f, 0f);

		private Vector3 FarPosition2 = new Vector3(360f, 320f, 0f);

		[ShowInInspector]
		[InspectorDisabled]
		[SerializeField]
		private FK3_GunConfig m_config;

		[ShowInInspector]
		[InspectorDisabled]
		private FK3_GunPlayerData m_data = new FK3_GunPlayerData();

		private bool m_running;

		private static float shootInterval = 0.28f;

		private FK3_IntervalTimer m_shootTimer = new FK3_IntervalTimer(shootInterval);

		private FK3_IntervalTimer m_rotateEMTimer = new FK3_IntervalTimer(0.2f);

		private FK3_IntervalTimer m_rotateDRTimer = new FK3_IntervalTimer(0.2f);

		private Animator m_normalBarrelAnimator;

		private Animator m_gunBarrelAnimator;

		private Animator m_baseAnimator;

		private FK3_GunType m_curType;

		private bool m_allowRotate;

		private bool m_isAuto;

		private int m_bulletCount;

		private int m_bulletLiveCount;

		private int m_lockingCount;

		public const int Max_Gun_bullet_Num = 10;

		public EventHandler_Shoot Event_ShootNormal_Handler;

		public Action<bool> Event_ScreenBeClick_act;

		public Action<int> Event_GunBeClick_act;

		public EventHandler_ChangeAuto Event_ChangeAuto_Handler;

		private Vector3 m_mouseGunPlanePos = Vector3.zero;

		private Vector3 m_mouseWorldPos = Vector3.zero;

		private float m_angle = 90f;

		private float emLastTouchTime;

		private float drLastTouchTime;

		private bool emShowFireWhenRelease;

		private bool drShowFireWhenRelease;

		public EventHandler_ShootEM Event_ShootEM_Handler;

		public EventHandler_ShootDR Event_ShootDR_Handler;

		public EventHandler_ShootDR Event_ShootDRPush_Handler;

		public EventHandler_RotateEM Event_RotateEM_Handler;

		public EventHandler_RotateEM Event_RotateDR_Handler;

		public Action Event_LaserCrabFire;

		public Action Event_DrillCrabFire;

		public Action Event_StopLaserCrab;

		public Action Event_StopDrillCrab;

		private FK3_Task lockingFishTask;

		private FK3_LockChainController chain;

		private bool canTouchCancelLocking = true;

		private Coroutine autoLockCoroutine;

		[SerializeField]
		private List<FishTypeSprite> fishTypeSprites;

		private Dictionary<FK3_FishType, Sprite> fishTypeSpriteDictionary = new Dictionary<FK3_FishType, Sprite>();

		public List<Transform> m_targetPoses;

		private Dictionary<int, List<Transform>> posTras = new Dictionary<int, List<Transform>>();

		private int m_occupiedCount;

		private int DRBackCountNum;

		public bool canAutoDrillFire;

		public bool allowShoot
		{
			get;
			set;
		}

		public bool emCanFire
		{
			get;
			set;
		}

		public bool isShowingEM
		{
			get;
			set;
		}

		public bool drCanFire
		{
			get;
			set;
		}

		public bool isShowingDR
		{
			get;
			set;
		}

		public event EventHandler_RotateByInput Event_RotateByInput_Handler;

		public void Reset_EventHandler()
		{
			this.Event_RotateByInput_Handler = null;
			Event_ShootNormal_Handler = null;
			Event_ShootDR_Handler = null;
			Event_ShootDRPush_Handler = null;
			Event_ScreenBeClick_act = null;
			Event_GunBeClick_act = null;
			Event_ChangeAuto_Handler = null;
			Event_LaserCrabFire = null;
			Event_DrillCrabFire = null;
			Event_StopLaserCrab = null;
			Event_StopDrillCrab = null;
		}

		public void Reset_Gun()
		{
			m_bulletCount = 0;
			m_bulletLiveCount = 0;
			m_lockingCount = 0;
			m_running = true;
			m_shootTimer.Reset();
			m_rotateEMTimer.Reset();
			m_rotateDRTimer.Reset();
			m_curType = FK3_GunType.FK3_NormalGun;
			_DisableGunType(FK3_GunType.FK3_LockingGun);
			_DisableGunType(FK3_GunType.FK3_GunGunn);
			_DisableGunType(FK3_GunType.FK3_EMGun);
			_DisableGunType(FK3_GunType.FK3_DRGun);
			_ChangeGunType(FK3_GunType.FK3_NormalGun);
			RotateNormalGun(90f);
			m_data = null;
			m_isAuto = false;
			m_occupiedCount = 0;
			posTras.Clear();
		}

		protected override void Awake()
		{
			base.Awake();
			m_normalBarrelAnimator = m_transNormalBarrel.GetComponent<Animator>();
			m_gunBarrelAnimator = m_transGunBarrel.GetComponent<Animator>();
			m_baseAnimator = m_transBase.GetComponent<Animator>();
			InitFind();
		}

		private void InitFind()
		{
			m_laserRange = m_transEMBarrel_Attach.Find("LaserRange");
			if (m_config.laserRangeSprite != null)
			{
				m_laserRange.GetComponent<Image>().sprite = m_config.laserRangeSprite;
			}
			m_laserStorage = m_transEMBarrel_Attach.Find("LaserStorage");
			m_laserFire = m_transEMBarrel_Attach.Find("LaserFire");
			m_laserFireAni = m_laserFire.GetComponent<Animator>();
			emAnimator = m_transEMBarrel.GetComponent<Animator>();
			drAnimator = m_transDRBarrel.GetComponent<Animator>();
			m_transDR_Projectile = m_transDRBarrel.Find("projectile");
			if (!m_btnPlusPower)
			{
				m_btnPlusPower = base.transform.parent.Find("ChangeAndAuto/BtnPlusPower").GetComponent<FK3_SpriteButton>();
			}
			if (!m_btnMinusPower)
			{
				m_btnMinusPower = base.transform.parent.Find("ChangeAndAuto/BtnMinusPower").GetComponent<FK3_SpriteButton>();
			}
			btnChangeSkin = base.transform.Find("BtnChangeSkin").GetComponent<FK3_SpriteButton>();
			btnChangeSkin.onClick = Handel_UI_BtnChangeSkin;
			baseEffect1 = m_transBase.Find("Effect1");
			baseEffect2 = m_transBase.Find("Effect2");
		}

		private void Handel_UI_BtnChangeSkin()
		{
			UnityEngine.Debug.LogError("切换炮的皮肤");
			DoChangeSkin(delegate(FK3_GunType from, FK3_GunType to)
			{
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/changeSkin", new object[1]
				{
					(to != 0) ? 1 : 0
				});
			});
		}

		private void Start()
		{
			m_running = true;
			m_curType = FK3_GunType.FK3_NormalGun;
			_ChangeGunType(m_curType);
			InitLockGun();
		}

		private void OnMouseDown()
		{
			if (Event_GunBeClick_act != null)
			{
				Event_GunBeClick_act(GetId());
			}
		}

		public void ShakeScreen()
		{
			if (m_curType == FK3_GunType.FK3_GunGunn || m_curType == FK3_GunType.FK3_NormalGun)
			{
				baseEffect1.gameObject.SetActive(value: true);
				baseEffect2.gameObject.SetActive(value: true);
			}
		}

		private void Update()
		{
			if ((bool)btnChangeSkin && (m_curType == FK3_GunType.FK3_GunGunn || m_curType == FK3_GunType.FK3_NormalGun))
			{
				if (IsNative())
				{
					if (!btnChangeSkin.gameObject.activeInHierarchy)
					{
						btnChangeSkin.gameObject.SetActive(value: true);
					}
				}
				else if (btnChangeSkin.gameObject.activeInHierarchy)
				{
					btnChangeSkin.gameObject.SetActive(value: false);
				}
			}
			if (emCanFire && FK3_SimpleSingletonBehaviour<UserInput>.Get().IsTouching() && m_curType == FK3_GunType.FK3_EMGun && IsNative())
			{
				emShowFireWhenRelease = true;
				if (Time.realtimeSinceStartup - emLastTouchTime > 0.2f)
				{
					if (m_allowRotate)
					{
						RotateByInput();
					}
					UnityEngine.Debug.LogError("电磁炮开炮1");
					ShootEM();
					emCanFire = false;
					m_allowRotate = false;
					return;
				}
			}
			if (drCanFire && FK3_SimpleSingletonBehaviour<UserInput>.Get().IsTouching() && m_curType == FK3_GunType.FK3_DRGun && IsNative())
			{
				drShowFireWhenRelease = true;
				if (Time.realtimeSinceStartup - drLastTouchTime > 0.2f)
				{
					if (m_allowRotate)
					{
						RotateByInput();
					}
					UnityEngine.Debug.LogError("钻头炮开炮1");
					ShootDR();
					drCanFire = false;
					m_allowRotate = false;
					return;
				}
			}
			if (drCanFire && m_curType == FK3_GunType.FK3_DRGun && IsNative() && canAutoDrillFire && DRBackCountNum <= 2 && m_DRBackCountTr.gameObject.activeInHierarchy)
			{
				if (m_allowRotate)
				{
					RotateByInput();
				}
				UnityEngine.Debug.LogError("钻头炮自动开炮");
				canAutoDrillFire = false;
				ShootDR();
				drCanFire = false;
				m_allowRotate = false;
				return;
			}
			if (m_curType == FK3_GunType.FK3_EMGun && IsNative() && emShowFireWhenRelease && !FK3_SimpleSingletonBehaviour<UserInput>.Get().IsTouching())
			{
				emShowFireWhenRelease = false;
				UnityEngine.Debug.LogError("电磁炮开炮2");
				ShootEM();
				emCanFire = false;
				m_allowRotate = false;
				return;
			}
			if (drCanFire && m_curType == FK3_GunType.FK3_DRGun && IsNative() && drShowFireWhenRelease && !FK3_SimpleSingletonBehaviour<UserInput>.Get().IsTouching())
			{
				drShowFireWhenRelease = false;
				UnityEngine.Debug.LogError("钻头炮开炮2");
				ShootDR();
				drCanFire = false;
				m_allowRotate = false;
				return;
			}
			if (FK3_SimpleSingletonBehaviour<UserInput>.Get().IsTouching() && IsNative())
			{
				if (m_allowRotate)
				{
					RotateByInput();
					m_rotateEMTimer.AddTime(Time.deltaTime);
					m_rotateDRTimer.AddTime(Time.deltaTime);
					if (m_curType == FK3_GunType.FK3_EMGun)
					{
						emLastTouchTime = Time.realtimeSinceStartup;
						if (Event_RotateEM_Handler != null && m_rotateEMTimer.CanDo())
						{
							m_rotateEMTimer.DoOnce();
							Event_RotateEM_Handler(m_angle);
						}
					}
					else if (m_curType == FK3_GunType.FK3_DRGun)
					{
						drLastTouchTime = Time.realtimeSinceStartup;
						if (Event_RotateDR_Handler != null && m_rotateDRTimer.CanDo())
						{
							m_rotateDRTimer.DoOnce();
							Event_RotateDR_Handler(m_angle);
						}
					}
				}
				if (m_data != null && m_data.isNative && Event_ScreenBeClick_act != null)
				{
					Event_ScreenBeClick_act(m_data.score == 0);
				}
			}
			if (allowShoot && m_data != null && m_data.isNative)
			{
				m_shootTimer.AddTime(Time.deltaTime);
				if ((m_isAuto || (FK3_SimpleSingletonBehaviour<UserInput>.Get().IsTouching() && m_curType != FK3_GunType.FK3_LockingGun)) && m_bulletLiveCount < 10 && m_shootTimer.CanDo())
				{
					m_shootTimer.DoOnce();
					Shoot();
				}
			}
			else
			{
				m_shootTimer.isFirstTime = true;
			}
		}

		public void SetConfig(FK3_GunConfig config)
		{
			m_config = config;
		}

		public void DoReset(bool assert = false)
		{
			try
			{
				DoAssert();
				base.transform.localPosition = m_config.pos;
				base.transform.localRotation = Quaternion.Euler(0f, 0f, FK3_GunUtils.GetGunDirRotation(m_config.dir));
				m_labelLeft.sprite = m_config.labelSprite;
				m_labelRight.sprite = m_config.labelSprite;
				_AdustEMBornPosPosition();
				_AdustDRBornPosPosition();
				_AdjustBtnPlusPosition();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			_AdjustTextOrientation();
		}

		private void _AdustEMBornPosPosition()
		{
			bool flag = m_config.id == 0 || m_config.id == 2 || m_config.id == 4;
			Vector3 localPosition = m_EMBornPos.localPosition;
			localPosition.x = Mathf.Abs(localPosition.x) * ((!flag) ? (-1f) : 1f);
			m_EMBornPos.localPosition = localPosition;
		}

		private void _AdustDRBornPosPosition()
		{
			bool flag = m_config.id == 0 || m_config.id == 2 || m_config.id == 4;
			Vector3 localPosition = m_DRBornPos.localPosition;
			localPosition.x = Mathf.Abs(localPosition.x) * ((!flag) ? (-1f) : 1f);
			m_DRBornPos.localPosition = localPosition;
		}

		private void _AdjustBtnPlusPosition()
		{
			bool flag = m_config.id == 0 || m_config.id == 1 || m_config.id == 3;
			Vector3 localPosition = m_btnPlusPower.transform.localPosition;
			Vector3 localPosition2 = m_btnMinusPower.transform.localPosition;
			m_btnPlusPower.transform.localPosition = localPosition;
			m_btnMinusPower.transform.localPosition = localPosition2;
		}

		private void _AdjustTextOrientation()
		{
			bool flag = m_config.id == 3 || m_config.id == 4;
			Transform parent = m_textName.transform.parent;
			Transform parent2 = m_textPower.transform.parent;
			if (flag)
			{
				parent.localRotation = Quaternion.Euler(0f, 0f, 180f);
				parent2.localRotation = Quaternion.Euler(0f, 0f, 180f);
			}
			else
			{
				parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
				parent2.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}

		public void SetParentQuater(int id)
		{
			if (id >= 3)
			{
				base.transform.parent.localRotation = Quaternion.Euler(0f, 0f, 180f);
			}
			else
			{
				base.transform.parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}

		private void DoAssert()
		{
		}

		public void Init()
		{
			DoAssert();
			DoReset();
		}

		public float CalculateAngleByInput()
		{
			Camera main = Camera.main;
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			Vector3 position = Camera.main.transform.position;
			Vector3 vector = main.ScreenToWorldPoint(mousePosition + position.z * Vector3.back);
			Vector3 vector2 = m_mouseGunPlanePos = m_rotatePivot.InverseTransformPoint(vector);
			m_mouseWorldPos = vector;
			return Vector2.Angle(Vector2.right, new Vector2(vector2.x, vector2.y));
		}

		public void RotateByInput()
		{
			float angle = m_angle = CalculateAngleByInput();
			if (m_curType == FK3_GunType.FK3_NormalGun || m_curType == FK3_GunType.FK3_GunGunn)
			{
				RotateNormalGun(angle);
			}
			else if (m_curType == FK3_GunType.FK3_EMGun)
			{
				RotateEMGun(angle);
			}
			else if (m_curType == FK3_GunType.FK3_DRGun)
			{
				RotateDRGun(angle);
			}
			if (this.Event_RotateByInput_Handler != null)
			{
				this.Event_RotateByInput_Handler(this, m_rotatePivot, m_mouseWorldPos, angle);
			}
		}

		public void RotateNormalGun(float angle)
		{
			m_angle = angle;
			if (m_transNormalBarrel.gameObject.activeInHierarchy)
			{
				m_transNormalBarrel.localEulerAngles = new Vector3(0f, 0f, angle - 90f);
			}
			if (m_transGunBarrel.gameObject.activeInHierarchy)
			{
				m_transGunBarrel.localEulerAngles = new Vector3(0f, 0f, angle - 90f);
			}
		}

		public void RotateEMGun(float angle)
		{
			m_transEMBarrel.parent.localEulerAngles = new Vector3(0f, 0f, angle - 90f);
		}

		public void RotateDRGun(float angle)
		{
			m_transDRBarrel.parent.localEulerAngles = new Vector3(0f, 0f, angle - 90f);
		}

		private float GetAngleOffset(int configId, int fridId)
		{
			if (configId <= 2)
			{
				if (fridId <= 2)
				{
					return 0f;
				}
				return 180f;
			}
			if (fridId <= 2)
			{
				return 180f;
			}
			return 0f;
		}

		public void Shoot()
		{
			if (m_curType == FK3_GunType.FK3_NormalGun)
			{
				m_normalBarrelAnimator.SetTrigger("Shoot");
				if (Event_ShootNormal_Handler != null)
				{
					float angleOffset = (!m_data.isNative) ? GetAngleOffset(FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun().m_config.id, m_config.id) : 0f;
					Event_ShootNormal_Handler(m_rotatePivot.position, m_angle, angleOffset, m_data.gunPower, m_config.id);
				}
				m_bulletCount++;
				m_bulletLiveCount++;
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("子弹发射音效");
				FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("子弹发射音效"), 0.3f);
			}
			else if (m_curType == FK3_GunType.FK3_GunGunn)
			{
				m_gunBarrelAnimator.SetTrigger("Shoot");
				if (Event_ShootNormal_Handler != null)
				{
					float angleOffset2 = (!m_data.isNative) ? GetAngleOffset(FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun().m_config.id, m_config.id) : 0f;
					Event_ShootNormal_Handler(m_rotatePivot.position, m_angle, angleOffset2, m_data.gunPower, m_config.id);
				}
				m_bulletCount++;
				m_bulletLiveCount++;
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("子弹发射音效");
				FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("子弹发射音效"), 0.3f);
			}
			else if (m_curType != FK3_GunType.FK3_LockingGun)
			{
			}
		}

		public void ShootDRPush(float m_angle, int id)
		{
			if (Event_ShootDRPush_Handler != null)
			{
				Event_ShootDRPush_Handler(m_rotatePivot.position, m_angle, 0f, m_data.gunPower, id);
			}
		}

		public void SetPower(int power)
		{
			m_data.gunPower = power;
			m_textPower.text = power.ToString();
			TweenCallback action = delegate
			{
				m_textPower.transform.localScale = Vector3.one;
			};
			m_textPower.transform.DOScale(1.5f, 0.1f).SetLoops(2, LoopType.Yoyo).OnKill(action)
				.OnComplete(action);
		}

		public int GetPower()
		{
			return m_data.gunPower;
		}

		public void SetName(string name)
		{
			m_data.name = name;
			m_textName.text = name;
		}

		public void SetScore(int score)
		{
			m_data.score = score;
			m_textScore.text = score.ToString();
		}

		public void SetNative(bool isNative)
		{
			UnityEngine.Debug.LogError(base.gameObject.name + " 是否是自己的炮: " + isNative);
			try
			{
				if (m_data == null)
				{
					UnityEngine.Debug.LogError(base.gameObject.name + " data为空!");
				}
				else
				{
					m_data.isNative = isNative;
					UnityEngine.Debug.LogError(base.gameObject.name + " isNative: " + m_data.isNative);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		public void SetPlayerData(FK3_GunPlayerData data)
		{
			try
			{
				m_data = data;
				m_textPower.text = m_data.gunPower.ToString();
				m_textName.text = m_data.name;
				m_textScore.text = m_data.score.ToString();
				SetUserInputOnclick();
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		public bool HasUser()
		{
			return m_data != null;
		}

		public int GetId()
		{
			return m_config.id;
		}

		public bool IsNative()
		{
			return m_data.isNative;
		}

		public int GetScore()
		{
			return m_data.score;
		}

		public FK3_GunPlayerData GetPlayerData()
		{
			return m_data;
		}

		public void DoChangeGun(Action<FK3_GunType, FK3_GunType> callback)
		{
			if (!isShowingEM)
			{
				FK3_SpriteButton component = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().transform.Find("ChangeAndAuto/BtnChangeGun").GetComponent<FK3_SpriteButton>();
				component.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/lock_0");
				if (m_curType == FK3_GunType.FK3_NormalGun || m_curType == FK3_GunType.FK3_GunGunn)
				{
					component.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/lock_1");
					_DisableGunType(m_curType);
					m_curType = FK3_GunType.FK3_LockingGun;
					_ChangeGunType(m_curType);
					DOTween.Kill(m_transLockingBarrel);
					m_transLockingBarrel.localScale = Vector3.one;
					m_transLockingBarrel.DOScale(1.3f, 0.3f).OnComplete(delegate
					{
						m_transLockingBarrel.DOScale(1f, 0.2f);
					});
					callback?.Invoke(FK3_GunType.FK3_NormalGun, FK3_GunType.FK3_LockingGun);
				}
				else if (m_curType == FK3_GunType.FK3_LockingGun)
				{
					component.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/lock_0");
					_DisableGunType(m_curType);
					m_curType = FK3_GunType.FK3_NormalGun;
					_ChangeGunType(m_curType);
					DOTween.Kill(m_transNormalBarrel);
					m_transNormalBarrel.localScale = Vector3.one;
					m_transNormalBarrel.DOScale(1.3f, 0.3f).OnComplete(delegate
					{
						m_transNormalBarrel.DOScale(1f, 0.2f);
					});
					DOTween.Kill(m_transGunBarrel);
					m_transGunBarrel.localScale = Vector3.one;
					m_transGunBarrel.DOScale(1.3f, 0.3f).OnComplete(delegate
					{
						m_transGunBarrel.DOScale(1f, 0.2f);
					});
					callback?.Invoke(FK3_GunType.FK3_LockingGun, FK3_GunType.FK3_NormalGun);
					StopAutoLockCoroutine();
					StopLock();
				}
				FK3_GVars.NowGunSkin[GetId() - 1] = FK3_GVars.GetCurSkinType(m_curType);
			}
		}

		public void DoChangeSkin(Action<FK3_GunType, FK3_GunType> callback)
		{
			if (!isShowingEM)
			{
				if (m_curType == FK3_GunType.FK3_NormalGun)
				{
					_DisableGunType(m_curType);
					m_curType = FK3_GunType.FK3_GunGunn;
					_ChangeGunType(m_curType);
					DOTween.Kill(m_transGunBarrel);
					m_transGunBarrel.localScale = Vector3.one;
					m_transGunBarrel.DOScale(1.3f, 0.3f).OnComplete(delegate
					{
						m_transGunBarrel.DOScale(1f, 0.2f);
					});
					callback?.Invoke(FK3_GunType.FK3_NormalGun, FK3_GunType.FK3_GunGunn);
				}
				else if (m_curType == FK3_GunType.FK3_GunGunn)
				{
					_DisableGunType(m_curType);
					m_curType = FK3_GunType.FK3_NormalGun;
					_ChangeGunType(m_curType);
					DOTween.Kill(m_transNormalBarrel);
					m_transNormalBarrel.localScale = Vector3.one;
					m_transNormalBarrel.DOScale(1.3f, 0.3f).OnComplete(delegate
					{
						m_transNormalBarrel.DOScale(1f, 0.2f);
					});
					callback?.Invoke(FK3_GunType.FK3_GunGunn, FK3_GunType.FK3_NormalGun);
				}
				FK3_GVars.NowGunSkin[GetId() - 1] = FK3_GVars.GetCurSkinType(m_curType);
			}
		}

		public void ChangeSkinRemote(FK3_GunType targetType, int id)
		{
			if (m_curType != targetType)
			{
				_DisableGunType(m_curType);
				m_curType = targetType;
				_ChangeGunType(m_curType);
				FK3_GVars.NowGunSkin[id - 1] = FK3_GVars.GetCurSkinType(m_curType);
				UnityEngine.Debug.LogError(id + "号位切枪 是否为自己的枪" + IsNative() + " 换成: " + m_curType);
				SetNative(IsNative());
			}
		}

		public void ChangeGunRemote(FK3_GunType targetType)
		{
			if (m_curType == FK3_GunType.FK3_LockingGun)
			{
				StopLock();
			}
			if (m_curType != targetType)
			{
				_DisableGunType(m_curType);
				m_curType = targetType;
				_ChangeGunType(m_curType);
				UnityEngine.Debug.LogError("切枪: " + IsNative());
				SetNative(IsNative());
			}
		}

		private void _DisableGunType(FK3_GunType type)
		{
			if (!m_btnPlusPower)
			{
				m_btnPlusPower = base.transform.parent.Find("ChangeAndAuto/BtnPlusPower").GetComponent<FK3_SpriteButton>();
			}
			if (!m_btnMinusPower)
			{
				m_btnMinusPower = base.transform.parent.Find("ChangeAndAuto/BtnMinusPower").GetComponent<FK3_SpriteButton>();
			}
			switch (type)
			{
			case FK3_GunType.FK3_EMGun:
				ResetEMGun();
				break;
			case FK3_GunType.FK3_DRGun:
				ResetDRGun();
				break;
			case FK3_GunType.FK3_LockingGun:
				HideLockingFishSprite();
				m_transLockingBarrel.gameObject.SetActive(value: false);
				m_powerWindow.gameObject.SetActive(value: false);
				m_textPower.gameObject.SetActive(value: false);
				break;
			case FK3_GunType.FK3_NormalGun:
			case FK3_GunType.FK3_GunGunn:
				m_transNormalBarrel.gameObject.SetActive(value: false);
				m_transGunBarrel.gameObject.SetActive(value: false);
				m_transBase.gameObject.SetActive(value: false);
				m_powerWindow.gameObject.SetActive(value: false);
				m_textPower.gameObject.SetActive(value: false);
				break;
			}
		}

		private void _ChangeGunType(FK3_GunType type)
		{
			FK3_GunUIController fK3_GunUIController = null;
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.InGame)
			{
				fK3_GunUIController = FK3_GunUIMgr.Get().GetGunByID(m_config.id);
			}
			switch (type)
			{
			case FK3_GunType.FK3_EMGun:
				m_transEMBarrel_Attach.gameObject.SetActive(value: true);
				m_transBase.gameObject.SetActive(value: true);
				m_powerWindow.gameObject.SetActive(value: true);
				m_textPower.gameObject.SetActive(value: true);
				break;
			case FK3_GunType.FK3_DRGun:
				m_transBase.gameObject.SetActive(value: true);
				m_powerWindow.gameObject.SetActive(value: true);
				m_textPower.gameObject.SetActive(value: true);
				break;
			case FK3_GunType.FK3_LockingGun:
				m_transLockingBarrel.gameObject.SetActive(value: true);
				m_powerWindow.gameObject.SetActive(value: true);
				m_textPower.gameObject.SetActive(value: true);
				if (fK3_GunUIController != null)
				{
					fK3_GunUIController.txtPower.gameObject.SetActive(value: true);
				}
				m_allowRotate = false;
				break;
			case FK3_GunType.FK3_NormalGun:
			case FK3_GunType.FK3_GunGunn:
				if (type == FK3_GunType.FK3_NormalGun)
				{
					m_transNormalBarrel.gameObject.SetActive(value: true);
					m_transGunBarrel.gameObject.SetActive(value: false);
				}
				else
				{
					m_transGunBarrel.gameObject.SetActive(value: true);
					m_transNormalBarrel.gameObject.SetActive(value: false);
				}
				m_transBase.gameObject.SetActive(value: true);
				m_powerWindow.gameObject.SetActive(value: true);
				m_textPower.gameObject.SetActive(value: true);
				if (fK3_GunUIController != null)
				{
					fK3_GunUIController.txtPower.gameObject.SetActive(value: true);
				}
				m_allowRotate = true;
				RotateNormalGun(90f);
				break;
			}
		}

		public bool CanLocking()
		{
			return m_curType == FK3_GunType.FK3_LockingGun && allowShoot;
		}

		public Transform GetLockingPoint()
		{
			return m_lockingPoint;
		}

		public FK3_GunType GetGunType()
		{
			return m_curType;
		}

		public bool IsAuto()
		{
			return m_isAuto;
		}

		public void DoAuto()
		{
			if (Event_ChangeAuto_Handler != null)
			{
				Event_ChangeAuto_Handler(this, isAuto: true);
			}
			m_isAuto = true;
		}

		public void DoCancelAuto()
		{
			if (Event_ChangeAuto_Handler != null)
			{
				Event_ChangeAuto_Handler(this, isAuto: false);
			}
			UnityEngine.Debug.LogError("======================= DoCancelAuto m_isAuto====================");
			m_isAuto = false;
			if (m_curType == FK3_GunType.FK3_LockingGun)
			{
				StopAutoLockCoroutine();
			}
		}

		public void OnBulletOver()
		{
			if (m_bulletLiveCount > 0)
			{
				m_bulletLiveCount--;
			}
		}

		public FK3_SpriteButton GetBtnPlusPower()
		{
			return m_btnPlusPower;
		}

		public FK3_SpriteButton GetBtnMinsPower()
		{
			return m_btnMinusPower;
		}

		public void ShootEM()
		{
			if (Event_LaserCrabFire != null)
			{
				Event_LaserCrabFire();
			}
			if (Event_ShootEM_Handler != null)
			{
				Event_ShootEM_Handler(m_angle);
			}
		}

		public void ShootDR()
		{
			if (Event_DrillCrabFire != null)
			{
				Event_DrillCrabFire();
			}
			if (Event_ShootDR_Handler != null)
			{
				Event_ShootDR_Handler(m_rotatePivot.position, m_angle, 0f, m_data.gunPower, m_config.id);
			}
		}

		public void RemoteShootEM()
		{
			if (Event_LaserCrabFire != null)
			{
				Event_LaserCrabFire();
			}
		}

		public void RemoteShootDR()
		{
			if (Event_DrillCrabFire != null)
			{
				Event_DrillCrabFire();
			}
		}

		public void KillEnterEMGunSequence()
		{
			if (m_enterEMGunsequence != null)
			{
				m_enterEMGunsequence.Kill();
			}
		}

		private void ResetEMGun()
		{
			try
			{
				if (m_laserFire == null || m_laserRange == null)
				{
					InitFind();
				}
				isShowingEM = false;
				m_transEMBarrel.localPosition = m_transEMBarrelTagPos.localPosition;
				m_transEMBarrel.localScale = Vector3.one;
				m_transEMBarrel.gameObject.SetActive(value: false);
				m_transEMBarrel_Attach.gameObject.SetActive(value: false);
				m_transBase.gameObject.SetActive(value: false);
				m_EMBackCountTr.gameObject.SetActive(value: false);
				m_laserFire.SetActive(active: false);
				m_laserRange.localScale = new Vector3(0f, 2f, 1f);
				m_laserRange.SetActive(active: false);
				RotateEMGun(90f);
			}
			catch (Exception arg)
			{
				MonoBehaviour.print("错误: " + arg);
			}
		}

		private void ResetDRGun()
		{
			try
			{
				InitFind();
				isShowingDR = false;
				m_transDRBarrel.localPosition = m_transDRBarrelTagPos.localPosition;
				m_transDRBarrel.localScale = Vector3.one;
				m_transDRBarrel.gameObject.SetActive(value: false);
				m_transBase.gameObject.SetActive(value: false);
				m_DRBackCountTr.gameObject.SetActive(value: false);
				RotateDRGun(90f);
			}
			catch (Exception arg)
			{
				MonoBehaviour.print("错误: " + arg);
			}
		}

		public void EnterDRGun()
		{
			drLastTouchTime = 0f;
			drShowFireWhenRelease = false;
			m_preGunType = m_curType;
			if (IsNative() && m_curType == FK3_GunType.FK3_LockingGun)
			{
				StopLock();
			}
			isShowingDR = true;
			drCanFire = false;
			pre_allowShoot = allowShoot;
			allowShoot = false;
			m_allowRotate = false;
			RotateNormalGun(90f);
			RotateEMGun(90f);
			RotateDRGun(90f);
			m_transDRBarrel.SetActive();
			m_transDR_Projectile.SetActive(active: false);
			SetDrillIdel();
			drAnimator.enabled = false;
			Vector3 localPosition = m_DRBornPos.localPosition;
			m_transDRBarrel.localPosition = localPosition;
			m_transDRBarrel.localScale = Vector3.one * 1.5f;
			m_transDRBarrel.GetComponent<Animator>().enabled = false;
			m_enterDRGunsequence = DOTween.Sequence();
			Vector3 a = localPosition - m_transDRBarrelTagPos.localPosition;
			m_enterDRGunsequence.Append(m_transDRBarrel.DOLocalMove(localPosition + a * 0.2f, 1f));
			m_enterDRGunsequence.Append(m_transDRBarrel.DOLocalMove(localPosition, 0.5f));
			m_enterDRGunsequence.Append(m_transDRBarrel.DOLocalMove(localPosition + a * 0.02f, 0.05f).SetLoops(6, LoopType.Yoyo));
			m_enterDRGunsequence.Append(m_transDRBarrel.DOLocalMove(m_transDRBarrelTagPos.localPosition, 0.5f).OnStart(delegate
			{
				m_transDRBarrel.DOScale(1f, 0.5f);
			}));
			m_enterDRGunsequence.Append(m_transDRBarrel.DOScale(0f, 0.2f));
			m_enterDRGunsequence.Append(m_transDRBarrel.DOScale(1f, 0.15f).OnStart(delegate
			{
				_DisableGunType(m_curType);
				m_curType = FK3_GunType.FK3_DRGun;
				m_transDR_Projectile.SetActive();
				_ChangeGunType(m_curType);
			}));
			m_enterDRGunsequence.Append(m_transDRBarrel.DOScale(1.2f, 0.05f).OnStart(delegate
			{
				m_allowRotate = true;
			}));
			Tween t = m_transDRBarrel.DOScale(1f, 0.3f);
			t.OnComplete(delegate
			{
				drAnimator.enabled = true;
			});
			m_enterDRGunsequence.Append(t);
		}

		public void EnterEMGun()
		{
			emLastTouchTime = 0f;
			emShowFireWhenRelease = false;
			m_preGunType = m_curType;
			if (IsNative() && m_curType == FK3_GunType.FK3_LockingGun)
			{
				StopLock();
			}
			isShowingEM = true;
			emCanFire = false;
			pre_allowShoot = allowShoot;
			allowShoot = false;
			m_allowRotate = false;
			RotateNormalGun(90f);
			RotateEMGun(90f);
			RotateDRGun(90f);
			m_transEMBarrel.SetActive();
			emAnimator.enabled = false;
			Vector3 localPosition = m_EMBornPos.localPosition;
			m_transEMBarrel.localPosition = localPosition;
			m_transEMBarrel.localScale = Vector3.one * 1.5f;
			m_transEMBarrel.GetComponent<Animator>().enabled = false;
			m_enterEMGunsequence = DOTween.Sequence();
			Vector3 a = localPosition - m_transEMBarrelTagPos.localPosition;
			m_enterEMGunsequence.Append(m_transEMBarrel.DOLocalMove(localPosition + a * 0.2f, 1f));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOLocalMove(localPosition, 0.5f));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOLocalMove(localPosition + a * 0.02f, 0.05f).SetLoops(6, LoopType.Yoyo));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOLocalMove(m_transEMBarrelTagPos.localPosition, 0.5f).OnStart(delegate
			{
				m_transEMBarrel.DOScale(1f, 0.5f);
			}));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOScale(0f, 0.2f));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOScale(1f, 0.15f).OnStart(delegate
			{
				_DisableGunType(m_curType);
				m_curType = FK3_GunType.FK3_EMGun;
				_ChangeGunType(m_curType);
			}));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOScale(1.2f, 0.05f).OnStart(delegate
			{
				m_transEMBarrel_Attach.SetActive();
				m_laserRange.SetActive();
				m_allowRotate = true;
				m_laserRange.DOScaleX(4f, 0.35f);
			}));
			Tween t = m_transEMBarrel.DOScale(1f, 0.3f);
			t.OnComplete(delegate
			{
				emAnimator.enabled = true;
			});
			m_enterEMGunsequence.Append(t);
		}

		public void SetStorageCapacity()
		{
			m_laserStorage.SetActive();
		}

		public IEnumerator SetEMBackCount()
		{
			if (!isShowingEM)
			{
				yield return null;
				yield break;
			}
			int count = 9;
			m_EMBackCountTr.SetActive();
			while (count >= 0)
			{
				Sequence sequence = DOTween.Sequence();
				sequence.Append(m_EMBackCountTr.DOScale(1.3f, 0.3f).OnStart(delegate
				{
					m_EMBackCountTr.Find("Txt").GetComponent<Text>().text = count.ToString("00");
				}));
				sequence.Append(m_EMBackCountTr.DOScale(1f, 0.3f));
				if (count == 0)
				{
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("倒计时结束音效_01");
					FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("倒计时结束音效_01"), 1f);
				}
				yield return new WaitForSeconds(1f);
				count--;
			}
		}

		public IEnumerator SetDRBackCount()
		{
			if (!isShowingDR)
			{
				yield return null;
				yield break;
			}
			DRBackCountNum = 9;
			m_DRBackCountTr.SetActive();
			while (DRBackCountNum >= 0)
			{
				Sequence sequence = DOTween.Sequence();
				sequence.Append(m_DRBackCountTr.DOScale(1.3f, 0.3f).OnStart(delegate
				{
					m_DRBackCountTr.Find("Txt").GetComponent<Text>().text = DRBackCountNum.ToString("00");
				}));
				sequence.Append(m_DRBackCountTr.DOScale(1f, 0.3f));
				if (DRBackCountNum == 0)
				{
					canAutoDrillFire = true;
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("倒计时结束音效_01");
					FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("倒计时结束音效_01"), 1f);
				}
				yield return new WaitForSeconds(1f);
				DRBackCountNum--;
			}
		}

		public void SetLaserFire()
		{
			m_allowRotate = false;
			m_EMBackCountTr.SetActive(active: false);
			emAnimator.SetTrigger("Shoot");
			m_laserRange.localScale = new Vector3(0f, 2f, 1f);
			m_laserRange.SetActive(active: false);
			m_laserStorage.SetActive(active: false);
			m_laserFire.SetActive();
		}

		public void SetDrillFire()
		{
			m_allowRotate = false;
			m_DRBackCountTr.SetActive(active: false);
		}

		public void SetDrillIdel()
		{
		}

		public void OutEMGun()
		{
			_DisableGunType(FK3_GunType.FK3_EMGun);
			m_curType = m_preGunType;
			allowShoot = pre_allowShoot;
			UnityEngine.Debug.LogError("==========OutEMGun==========");
			_ChangeGunType(m_curType);
			SetNative(IsNative());
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("电磁炮聚能音效"));
			if (IsNative() && m_preGunType == FK3_GunType.FK3_LockingGun)
			{
				CheckAutoLock();
			}
		}

		public void OutDRGun()
		{
			_DisableGunType(FK3_GunType.FK3_DRGun);
			m_curType = m_preGunType;
			allowShoot = pre_allowShoot;
			UnityEngine.Debug.LogError("==========OutDRGun==========");
			_ChangeGunType(m_curType);
			SetNative(IsNative());
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("电磁炮聚能音效"));
			if (IsNative() && m_preGunType == FK3_GunType.FK3_LockingGun)
			{
				CheckAutoLock();
			}
		}

		private void InitLockGun()
		{
			chain = FK3_EffectMgr.Get().GetLockChain(GetId());
			InitFishTypeSpriteDictionary();
		}

		private void SetUserInputOnclick()
		{
			if (IsNative())
			{
				FK3_SimpleSingletonBehaviour<UserInput>.Get().onClick = ClickSpaceArea;
			}
		}

		public void LockFish(FK3_FishBehaviour fish)
		{
			ShowLockingFishSprite(fish.type);
			StartCoroutine(CancelControl());
			chain.SetActive();
			chain.DoChangeTarget(fish.id);
			chain.SetTarget(GetLockingPoint(), fish.GetCenterTransform(), fish.GetSpriteOrder());
			if (lockingFishTask != null)
			{
				lockingFishTask.Stop();
			}
			lockingFishTask = new FK3_Task(LockingFish(fish, chain));
			chain.Event_OnStopLocking = delegate
			{
				if (lockingFishTask != null)
				{
					lockingFishTask.Stop();
				}
				lockingFishTask = null;
			};
			bool doOnce = false;
			int fishId = fish.id;
			Action<FK3_FishBehaviour> wrapAction = delegate(FK3_FishBehaviour _fish)
			{
				if (chain.Event_OnStopLocking != null)
				{
					chain.Event_OnStopLocking();
				}
				if (fishId != _fish.id)
				{
					_fish.Debug_Analyze_EventHandler();
				}
				else if (!doOnce)
				{
					doOnce = true;
					UnLockFish(fish, chain);
					CheckAutoLock();
				}
			};
			Action<FK3_FishBehaviour> dyingAction = delegate(FK3_FishBehaviour _fish)
			{
				UnityEngine.Debug.Log($"FishDying[{_fish.identity}]");
				wrapAction(_fish);
			};
			Action<FK3_FishBehaviour> dieAction = delegate(FK3_FishBehaviour _fish)
			{
				UnityEngine.Debug.Log($"FishDie[{_fish.identity}]");
				wrapAction(_fish);
			};
			Action<FK3_FishBehaviour> escapeScreenCall = delegate(FK3_FishBehaviour _fish)
			{
				UnityEngine.Debug.Log($"FishEscapeScreen[{_fish.identity}]");
				wrapAction(_fish);
			};
			FK3_FishBehaviour fK3_FishBehaviour = fish;
			fK3_FishBehaviour.Event_FishDying_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDying_Handler, dyingAction);
			FK3_FishBehaviour fK3_FishBehaviour2 = fish;
			fK3_FishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDie_Handler, dieAction);
			FK3_FishBehaviour fK3_FishBehaviour3 = fish;
			fK3_FishBehaviour3.Event_FishEscapeScreen_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour3.Event_FishEscapeScreen_Handler, escapeScreenCall);
			Action event_OnChangeTarget = delegate
			{
				FK3_FishBehaviour fK3_FishBehaviour4 = fish;
				fK3_FishBehaviour4.Event_FishDying_Handler = (Action<FK3_FishBehaviour>)Delegate.Remove(fK3_FishBehaviour4.Event_FishDying_Handler, dyingAction);
				FK3_FishBehaviour fK3_FishBehaviour5 = fish;
				fK3_FishBehaviour5.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Remove(fK3_FishBehaviour5.Event_FishDie_Handler, dieAction);
				FK3_FishBehaviour fK3_FishBehaviour6 = fish;
				fK3_FishBehaviour6.Event_FishEscapeScreen_Handler = (Action<FK3_FishBehaviour>)Delegate.Remove(fK3_FishBehaviour6.Event_FishEscapeScreen_Handler, escapeScreenCall);
			};
			chain.Event_OnChangeTarget = event_OnChangeTarget;
		}

		private IEnumerator LockingFish(FK3_FishBehaviour fish, FK3_LockChainController chain)
		{
			if (!IsNative())
			{
				yield break;
			}
			int fishId = fish.id;
			int fishType = (int)fish.type;
			FK3_MessageCenter.SendMessage("LockGunSelectFish", new FK3_KeyValueInfo("fishId", JsonUtility.ToJson(new FK3_LockGunSelectInfo(FK3_GVars.lobby.curSeatId, fish.id, GetPower()))));
			bool stopLocking;
			while (true)
			{
				int gunValue = GetPower();
				yield return new WaitForSeconds(shootInterval);
				int curScore = FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().GetContext().curScore;
				if (curScore < GetPower())
				{
					gunValue = curScore;
				}
				if (curScore != 0)
				{
					if (m_curType == FK3_GunType.FK3_LockingGun)
					{
						if (fish.State == FK3_FishState.Live && fish.inScreen)
						{
							if (fishId == fish.id)
							{
								int gunAngle = 0;
								int gunType = 1;
								FK3_MessageCenter.SendMessage("LockGunShoot", new FK3_KeyValueInfo("FK3_LockGunShootInfo", JsonUtility.ToJson(new FK3_LockGunShootInfo(gunValue, gunAngle, gunType, fishId, fishType))));
								fish.Locked = true;
								continue;
							}
							UnityEngine.Debug.Log(FK3_LogHelper.Lime("LockingFish[id:{0}] dead and changed, newId:{1}", fishId, fish.id));
							stopLocking = true;
							break;
						}
						UnityEngine.Debug.Log(FK3_LogHelper.Lime("LockingFish[id:{0}] dead", fishId));
						stopLocking = true;
						break;
					}
					stopLocking = true;
					break;
				}
				stopLocking = true;
				FK3_SimpleSingletonBehaviour<FK3_OptionController>.Get().Show_InOutPanel();
				break;
			}
			if (stopLocking)
			{
				UnLockFish(fish, chain);
			}
		}

		public void StopLock()
		{
			UnLockFish(null, chain);
		}

		public void UnLockFish(FK3_FishBehaviour fish, FK3_LockChainController chain)
		{
			if (lockingFishTask != null)
			{
				lockingFishTask.Stop();
			}
			lockingFishTask = null;
			HideLockingFishSprite();
			chain.Hide();
			chain.UnlockTarget();
			if (fish != null)
			{
				fish.Locked = false;
			}
		}

		public void CheckAutoLock()
		{
			if (CanLocking() && IsAuto())
			{
				AutoLock(chain);
			}
		}

		public void AutoLock(FK3_LockChainController chain)
		{
			StopAutoLockCoroutine();
			autoLockCoroutine = StartCoroutine(AutoLockIE(chain));
		}

		private void StopAutoLockCoroutine()
		{
			if (autoLockCoroutine != null)
			{
				StopCoroutine(autoLockCoroutine);
			}
		}

		private IEnumerator AutoLockIE(FK3_LockChainController chain)
		{
			List<FK3_FishBehaviour> fishList;
			while (true)
			{
				fishList = FK3_FishMgr.Get().GetScreenFishList((FK3_FishBehaviour _fish) => _fish.gameObject.activeInHierarchy && _fish.State == FK3_FishState.Live && _fish.inScreen);
				if (fishList.Count != 0)
				{
					break;
				}
				yield return null;
			}
			fishList.Sort((FK3_FishBehaviour a, FK3_FishBehaviour b) => b.GetFishRateWeight() - a.GetFishRateWeight());
			FK3_FishBehaviour fish = fishList[0];
			UnityEngine.Debug.Log($"DoAutoLocking> find fish[{fish.identity}]");
			LockFish(fish);
		}

		private IEnumerator CancelControl()
		{
			canTouchCancelLocking = false;
			yield return new WaitForSeconds(0.2f);
			canTouchCancelLocking = true;
		}

		private void ClickSpaceArea()
		{
			if (canTouchCancelLocking && CanLocking() && !IsAuto() && IsNative())
			{
				UnLockFish(null, chain);
			}
		}

		private void InitFishTypeSpriteDictionary()
		{
			for (int i = 0; i < fishTypeSprites.Count; i++)
			{
				fishTypeSpriteDictionary.Add(fishTypeSprites[i].fishType, fishTypeSprites[i].sprite);
			}
		}

		public void ShowLockingFishSprite(FK3_FishType fishType)
		{
			if (fishTypeSpriteDictionary.ContainsKey(fishType))
			{
				m_LockingFishSprite.sprite = fishTypeSpriteDictionary[fishType];
				m_LockingFishSprite.SetNativeSize();
				m_LockingFishSprite.SetActive();
			}
			else
			{
				m_LockingFishSprite.SetActive(active: false);
			}
		}

		public void HideLockingFishSprite()
		{
			m_LockingFishSprite.SetActive(active: false);
		}

		public Transform GetTargetdPos(List<Transform> trs)
		{
			Transform result = m_targetPoses[(m_occupiedCount < m_targetPoses.Count) ? m_occupiedCount : (m_targetPoses.Count - 1)];
			posTras[m_occupiedCount] = trs;
			m_occupiedCount++;
			return result;
		}

		public void ResetOccupiedTargetPoses()
		{
			for (int i = 0; i < m_occupiedCount - 1; i++)
			{
				posTras[i] = posTras[i + 1];
				if (i < m_targetPoses.Count - 1)
				{
					foreach (Transform item in posTras[i])
					{
						Vector3 position = m_targetPoses[i].position;
						if (item.name.Contains("UI_Crystal_"))
						{
							item.GetComponent<FK3_Effect_Logo_Crystal>().Stop();
							position.y += ((m_config.id == 1 || m_config.id == 2) ? 0.4f : (-0.4f));
						}
						if (item.name.Contains("GoldShark_霸王鲸"))
						{
							position.y += ((m_config.id == 1 || m_config.id == 2) ? 0.4f : (-0.4f));
						}
						DOTween.Kill(item);
						item.DOMove(position, 1f);
					}
				}
			}
			posTras.Remove(m_occupiedCount - 1);
			m_occupiedCount--;
		}

		public void GunMove(bool odd)
		{
			_input.GetComponent<BoxCollider2D>().enabled = false;
			StopCoroutine(GunMoveDelay());
			StopCoroutine(FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().GunMoveDelay());
			StartCoroutine(FK3_SimpleSingletonBehaviour<FK3_SaveAndTakeScores>.Get().GunMoveDelay());
			SelfPosition = _barrelRoot.transform.position;
			if (odd)
			{
				_barrelRoot.transform.localPosition = FarPosition;
			}
			else
			{
				_barrelRoot.transform.localPosition = FarPosition2;
			}
			StartCoroutine(GunMoveDelay());
		}

		private IEnumerator GunMoveDelay()
		{
			yield return new WaitForSeconds(0.2f);
			_barrelRoot.transform.DOScale(1.5f, 0.3f).SetLoops(2, LoopType.Yoyo);
			yield return new WaitForSeconds(0.5f);
			_barrelRoot.transform.DOMove(SelfPosition, 1f);
			yield return new WaitForSeconds(1f);
			FK3_MainGameLogic3_UI.MainCamera.transform.localEulerAngles = Vector3.zero;
			_input.GetComponent<BoxCollider2D>().enabled = true;
		}
	}
}
