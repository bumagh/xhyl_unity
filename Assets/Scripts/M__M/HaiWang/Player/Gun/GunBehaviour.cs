using DG.Tweening;
using FullInspector;
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
	public class GunBehaviour : BaseBehavior<FullSerializerSerializer>
	{
		public delegate void EventHandler_RotateByInput(GunBehaviour gun, Transform begin, Vector3 end, float angle);

		public delegate void EventHandler_Shoot(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId);

		public delegate void EventHandler_ChangeAuto(GunBehaviour gun, bool isAuto);

		public delegate void EventHandler_ShootEM(float gunAngle);

		public delegate void EventHandler_RotateEM(float gunAngle);

		[Serializable]
		public class FishTypeSprite
		{
			public FishType fishType;

			public Sprite sprite;
		}

		[SerializeField]
		private Image m_labelLeft;

		[SerializeField]
		[ShowInInspector]
		[NotSerialized]
		private Image m_labelRight;

		[SerializeField]
		private SpriteButton m_btnPlusPower;

		[SerializeField]
		private SpriteButton m_btnMinusPower;

		[SerializeField]
		private Transform m_transBase;

		[SerializeField]
		private Transform m_powerWindow;

		[SerializeField]
		private Transform m_transAmmunition;

		[SerializeField]
		private Transform m_transNormalBarrel;

		[SerializeField]
		private Transform m_transLockingBarrel;

		[SerializeField]
		private Image m_LockingFishSprite;

		[SerializeField]
		private Transform m_transEMBarrel;

		[SerializeField]
		private Transform m_EMBornPos;

		private Animator emAnimator;

		[SerializeField]
		public Transform m_transEMBarrel_Attach;

		private Transform m_laserRange;

		private Transform m_laserStorage;

		private Transform m_laserFire;

		[SerializeField]
		private Transform m_EMBackCountTr;

		private Animator m_laserFireAni;

		private bool m_emCanFire;

		private bool m_isShowingEM;

		private GunType m_preGunType;

		private bool pre_allowShoot;

		private Sequence m_enterEMGunsequence;

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
		private GunConfig m_config;

		[InspectorDisabled]
		[ShowInInspector]
		private GunPlayerData m_data = new GunPlayerData();

		private bool m_allowShoot;

		private bool m_running;

		private static float shootInterval = 0.28f;

		private IntervalTimer m_shootTimer = new IntervalTimer(shootInterval);

		private IntervalTimer m_rotateEMTimer = new IntervalTimer(0.2f);

		private Animator m_normalBarrelAnimator;

		private Animator m_baseAnimator;

		private Animator m_ammunitionAnimator;

		private GunType m_curType;

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

		private bool emShowFireWhenRelease;

		public EventHandler_ShootEM Event_ShootEM_Handler;

		public EventHandler_RotateEM Event_RotateEM_Handler;

		public Action Event_LaserCrabFire;

		public Action Event_StopLaserCrab;

		private Vector3 m_EMBarrelTargetPos;

		private Task lockingFishTask;

		private LockChainController chain;

		private bool canTouchCancelLocking = true;

		private Coroutine autoLockCoroutine;

		[SerializeField]
		private List<FishTypeSprite> fishTypeSprites;

		private Dictionary<FishType, Sprite> fishTypeSpriteDictionary = new Dictionary<FishType, Sprite>();

		public List<Transform> m_targetPoses;

		private Dictionary<int, List<Transform>> posTras = new Dictionary<int, List<Transform>>();

		private int m_occupiedCount;

		public bool allowShoot
		{
			get
			{
				return m_allowShoot;
			}
			set
			{
				m_allowShoot = value;
			}
		}

		public bool emCanFire
		{
			get
			{
				return m_emCanFire;
			}
			set
			{
				m_emCanFire = value;
			}
		}

		public bool isShowingEM
		{
			get
			{
				return m_isShowingEM;
			}
			set
			{
				m_isShowingEM = value;
			}
		}

		public event EventHandler_RotateByInput Event_RotateByInput_Handler;

		public void Reset_EventHandler()
		{
			this.Event_RotateByInput_Handler = null;
			Event_ShootNormal_Handler = null;
			Event_ScreenBeClick_act = null;
			Event_GunBeClick_act = null;
			Event_ChangeAuto_Handler = null;
			Event_LaserCrabFire = null;
			Event_StopLaserCrab = null;
		}

		public void Reset_Gun()
		{
			m_bulletCount = 0;
			m_bulletLiveCount = 0;
			m_lockingCount = 0;
			m_running = true;
			m_shootTimer.Reset();
			m_rotateEMTimer.Reset();
			m_curType = GunType.NormalGun;
			_DisableGunType(GunType.LockingGun);
			_DisableGunType(GunType.EMGun);
			_ChangeGunType(GunType.NormalGun);
			RotateNormalGun(90f);
			m_data = null;
			UnityEngine.Debug.LogError("======================= Reset_Gun m_isAuto====================");
			m_isAuto = false;
			m_occupiedCount = 0;
			posTras.Clear();
		}

		protected override void Awake()
		{
			base.Awake();
			m_normalBarrelAnimator = m_transNormalBarrel.GetComponent<Animator>();
			m_baseAnimator = m_transBase.GetComponent<Animator>();
			m_ammunitionAnimator = m_transAmmunition.GetComponent<Animator>();
			InitFind();
		}

		private void InitFind()
		{
			m_laserRange = m_transEMBarrel_Attach.Find("LaserRange");
			m_laserRange.GetComponent<Image>().sprite = m_config.laserRangeSprite;
			m_laserStorage = m_transEMBarrel_Attach.Find("LaserStorage");
			m_laserFire = m_transEMBarrel_Attach.Find("LaserFire");
			m_laserFireAni = m_laserFire.GetComponent<Animator>();
			emAnimator = m_transEMBarrel.GetComponent<Animator>();
		}

		private void Start()
		{
			m_EMBarrelTargetPos = m_transEMBarrel.position;
			m_running = true;
			m_curType = GunType.NormalGun;
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

		private void Update()
		{
			if (m_emCanFire && SimpleSingletonBehaviour<UserInput>.Get().IsTouching() && m_curType == GunType.EMGun && IsNative())
			{
				emShowFireWhenRelease = true;
				if (Time.realtimeSinceStartup - emLastTouchTime > 0.2f)
				{
					if (m_allowRotate)
					{
						RotateByInput();
					}
					ShootEM();
					emCanFire = false;
					m_allowRotate = false;
					return;
				}
			}
			if (m_curType == GunType.EMGun && IsNative() && emShowFireWhenRelease && !SimpleSingletonBehaviour<UserInput>.Get().IsTouching())
			{
				emShowFireWhenRelease = false;
				ShootEM();
				emCanFire = false;
				m_allowRotate = false;
				return;
			}
			if (SimpleSingletonBehaviour<UserInput>.Get().IsTouching() && IsNative())
			{
				if (m_allowRotate)
				{
					RotateByInput();
					m_rotateEMTimer.AddTime(Time.deltaTime);
					if (m_curType == GunType.EMGun)
					{
						emLastTouchTime = Time.realtimeSinceStartup;
						if (Event_RotateEM_Handler != null && m_rotateEMTimer.CanDo())
						{
							m_rotateEMTimer.DoOnce();
							Event_RotateEM_Handler(m_angle);
						}
					}
				}
				if (m_data != null && m_data.isNative && Event_ScreenBeClick_act != null)
				{
					Event_ScreenBeClick_act(m_data.score == 0);
				}
			}
			if (m_allowShoot && m_data != null && m_data.isNative)
			{
				m_shootTimer.AddTime(Time.deltaTime);
				if ((m_isAuto || (SimpleSingletonBehaviour<UserInput>.Get().IsTouching() && m_curType != GunType.LockingGun)) && m_bulletLiveCount < 10 && m_shootTimer.CanDo())
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

		public void SetConfig(GunConfig config)
		{
			m_config = config;
		}

		public void DoReset(bool assert = false)
		{
			try
			{
				DoAssert();
				base.transform.localPosition = m_config.pos;
				base.transform.localRotation = Quaternion.Euler(0f, 0f, GunUtils.GetGunDirRotation(m_config.dir));
				m_labelLeft.sprite = m_config.labelSprite;
				m_labelRight.sprite = m_config.labelSprite;
				_AdustEMBornPosPosition();
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

		private void _AdjustBtnPlusPosition()
		{
			bool flag = m_config.id == 0 || m_config.id == 2 || m_config.id == 4;
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
			if (m_curType == GunType.NormalGun)
			{
				RotateNormalGun(angle);
			}
			else if (m_curType == GunType.EMGun)
			{
				RotateEMGun(angle);
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
		}

		public void RotateEMGun(float angle)
		{
			m_transEMBarrel.parent.localEulerAngles = new Vector3(0f, 0f, angle - 90f);
		}

		public void Shoot()
		{
			if (m_curType == GunType.NormalGun)
			{
				m_normalBarrelAnimator.SetTrigger("Shoot");
				m_ammunitionAnimator.SetTrigger("Shoot");
				if (Event_ShootNormal_Handler != null)
				{
					float angleOffset = (!m_data.isNative) ? GetAngleOffset(fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun().m_config.id, m_config.id) : 0f;
					Event_ShootNormal_Handler(m_rotatePivot.position, m_angle, angleOffset, m_data.gunPower, m_config.id);
				}
				m_bulletCount++;
				m_bulletLiveCount++;
				HW2_Singleton<SoundMgr>.Get().PlayClip("子弹发射音效");
				HW2_Singleton<SoundMgr>.Get().SetVolume(HW2_Singleton<SoundMgr>.Get().GetClip("子弹发射音效"), 0.3f);
			}
			else if (m_curType != GunType.LockingGun)
			{
			}
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
					m_btnPlusPower.gameObject.SetActive(isNative);
					m_btnMinusPower.gameObject.SetActive(isNative);
					UnityEngine.Debug.LogError(base.gameObject.name + " isNative: " + m_data.isNative);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		public void SetPlayerData(GunPlayerData data)
		{
			try
			{
				m_data = data;
				m_textPower.text = m_data.gunPower.ToString();
				m_textName.text = m_data.name;
				m_textScore.text = m_data.score.ToString();
				m_btnPlusPower.gameObject.SetActive(m_data.isNative);
				m_btnMinusPower.gameObject.SetActive(m_data.isNative);
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

		public GunPlayerData GetPlayerData()
		{
			return m_data;
		}

		public void DoChangeGun(Action<GunType, GunType> callback)
		{
			if (!isShowingEM)
			{
				SpriteButton component = fiSimpleSingletonBehaviour<GunMgr>.Get().transform.Find("ChangeAndAuto/BtnChangeGun").GetComponent<SpriteButton>();
				component.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/but_game_6_1");
				if (m_curType == GunType.NormalGun)
				{
					component.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/but_game_6_0");
					_DisableGunType(m_curType);
					m_curType = GunType.LockingGun;
					_ChangeGunType(m_curType);
					DOTween.Kill(m_transLockingBarrel);
					m_transLockingBarrel.localScale = Vector3.one;
					m_transLockingBarrel.DOScale(1.3f, 0.3f).OnComplete(delegate
					{
						m_transLockingBarrel.DOScale(1f, 0.2f);
					});
					callback?.Invoke(GunType.NormalGun, GunType.LockingGun);
				}
				else if (m_curType == GunType.LockingGun)
				{
					component.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/InGameUI/but_game_6_1");
					_DisableGunType(m_curType);
					m_curType = GunType.NormalGun;
					_ChangeGunType(m_curType);
					DOTween.Kill(m_transNormalBarrel);
					m_transNormalBarrel.localScale = Vector3.one;
					m_transNormalBarrel.DOScale(1.3f, 0.3f).OnComplete(delegate
					{
						m_transNormalBarrel.DOScale(1f, 0.2f);
					});
					callback?.Invoke(GunType.LockingGun, GunType.NormalGun);
					StopAutoLockCoroutine();
					StopLock();
				}
			}
		}

		public void ChangeGunRemote(GunType targetType)
		{
			if (m_curType == GunType.LockingGun)
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

		private void _DisableGunType(GunType type)
		{
			switch (type)
			{
			case GunType.EMGun:
				ResetEMGun();
				break;
			case GunType.LockingGun:
				HideLockingFishSprite();
				m_transLockingBarrel.gameObject.SetActive(value: false);
				m_powerWindow.gameObject.SetActive(value: false);
				m_textPower.gameObject.SetActive(value: false);
				m_btnPlusPower.gameObject.SetActive(value: false);
				m_btnMinusPower.gameObject.SetActive(value: false);
				break;
			case GunType.NormalGun:
				m_transNormalBarrel.gameObject.SetActive(value: false);
				m_transBase.gameObject.SetActive(value: false);
				m_transAmmunition.gameObject.SetActive(value: false);
				m_powerWindow.gameObject.SetActive(value: false);
				m_textPower.gameObject.SetActive(value: false);
				m_btnPlusPower.gameObject.SetActive(value: false);
				m_btnMinusPower.gameObject.SetActive(value: false);
				break;
			}
		}

		private void _ChangeGunType(GunType type)
		{
			GunUIController gunUIController = null;
			if (HW2_GVars.m_curState == Demo_UI_State.InGame)
			{
				gunUIController = GunUIMgr.Get().GetGunByID(m_config.id);
			}
			switch (type)
			{
			case GunType.EMGun:
				m_transEMBarrel_Attach.gameObject.SetActive(value: true);
				m_transBase.gameObject.SetActive(value: true);
				m_transAmmunition.gameObject.SetActive(value: true);
				if (gunUIController != null)
				{
					gunUIController.txtPower.gameObject.SetActive(value: false);
				}
				break;
			case GunType.LockingGun:
				m_transLockingBarrel.gameObject.SetActive(value: true);
				m_powerWindow.gameObject.SetActive(value: true);
				m_textPower.gameObject.SetActive(value: true);
				m_btnPlusPower.gameObject.SetActive(value: true);
				m_btnMinusPower.gameObject.SetActive(value: true);
				if (gunUIController != null)
				{
					gunUIController.txtPower.gameObject.SetActive(value: true);
				}
				m_allowRotate = false;
				break;
			case GunType.NormalGun:
				m_transNormalBarrel.gameObject.SetActive(value: true);
				m_transBase.gameObject.SetActive(value: true);
				m_transAmmunition.gameObject.SetActive(value: true);
				m_powerWindow.gameObject.SetActive(value: true);
				m_textPower.gameObject.SetActive(value: true);
				m_btnPlusPower.gameObject.SetActive(value: true);
				m_btnMinusPower.gameObject.SetActive(value: true);
				if (gunUIController != null)
				{
					gunUIController.txtPower.gameObject.SetActive(value: true);
				}
				m_allowRotate = true;
				RotateNormalGun(90f);
				break;
			}
		}

		public bool CanLocking()
		{
			return m_curType == GunType.LockingGun && m_allowShoot;
		}

		public Transform GetLockingPoint()
		{
			return m_lockingPoint;
		}

		public GunType GetGunType()
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
			if (m_curType == GunType.LockingGun)
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

		public SpriteButton GetBtnPlusPower()
		{
			return m_btnPlusPower;
		}

		public SpriteButton GetBtnMinsPower()
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

		public void RemoteShootEM()
		{
			if (Event_LaserCrabFire != null)
			{
				Event_LaserCrabFire();
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
				m_transEMBarrel.position = m_EMBarrelTargetPos;
				m_transEMBarrel.localScale = Vector3.one;
				m_transEMBarrel.gameObject.SetActive(value: false);
				m_transEMBarrel_Attach.gameObject.SetActive(value: false);
				m_transBase.gameObject.SetActive(value: false);
				m_transAmmunition.gameObject.SetActive(value: false);
				m_EMBackCountTr.gameObject.SetActive(value: false);
				m_laserFire.SetActive(active: false);
				m_laserRange.localScale = new Vector3(0f, 1f, 1f);
				m_laserRange.SetActive(active: false);
				RotateEMGun(90f);
			}
			catch (Exception arg)
			{
				MonoBehaviour.print("错误: " + arg);
			}
		}

		public void EnterEMGun()
		{
			emLastTouchTime = 0f;
			emShowFireWhenRelease = false;
			m_preGunType = m_curType;
			if (IsNative() && m_curType == GunType.LockingGun)
			{
				StopLock();
			}
			m_btnPlusPower.gameObject.SetActive(value: false);
			m_btnMinusPower.gameObject.SetActive(value: false);
			isShowingEM = true;
			emCanFire = false;
			pre_allowShoot = m_allowShoot;
			m_allowShoot = false;
			m_allowRotate = false;
			RotateNormalGun(90f);
			RotateEMGun(90f);
			m_transEMBarrel.SetActive();
			emAnimator.enabled = false;
			Vector3 position = m_EMBornPos.position;
			m_transEMBarrel.position = position;
			m_transEMBarrel.localScale = new Vector3(2.5f, 2.5f, 2.5f);
			m_transEMBarrel.GetComponent<Animator>().enabled = false;
			m_enterEMGunsequence = DOTween.Sequence();
			Vector3 a = position - m_EMBarrelTargetPos;
			m_enterEMGunsequence.Append(m_transEMBarrel.DOMove(position + a * 0.2f, 1f));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOMove(position, 0.5f));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOMove(position + a * 0.02f, 0.05f).SetLoops(6, LoopType.Yoyo));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOMove(m_EMBarrelTargetPos, 0.5f).OnStart(delegate
			{
				m_transEMBarrel.DOScale(1f, 0.5f);
			}));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOScale(0f, 0.2f));
			m_enterEMGunsequence.Append(m_transEMBarrel.DOScale(1f, 0.15f).OnStart(delegate
			{
				_DisableGunType(m_curType);
				m_curType = GunType.EMGun;
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
					m_EMBackCountTr.Find("Txt").GetComponent<TextMesh>().text = string.Empty + count;
				}));
				sequence.Append(m_EMBackCountTr.DOScale(1f, 0.3f));
				if (count == 0)
				{
					HW2_Singleton<SoundMgr>.Get().PlayClip("倒计时结束音效_01");
					HW2_Singleton<SoundMgr>.Get().SetVolume(HW2_Singleton<SoundMgr>.Get().GetClip("倒计时结束音效_01"), 1f);
				}
				yield return new WaitForSeconds(1f);
				count--;
			}
		}

		public void SetLaserFire()
		{
			m_allowRotate = false;
			m_EMBackCountTr.SetActive(active: false);
			emAnimator.SetTrigger("Shoot");
			m_laserRange.localScale = new Vector3(0f, 1f, 1f);
			m_laserRange.SetActive(active: false);
			m_laserStorage.SetActive(active: false);
			m_laserFire.SetActive();
		}

		public void OutEMGun()
		{
			_DisableGunType(GunType.EMGun);
			m_curType = m_preGunType;
			m_allowShoot = pre_allowShoot;
			UnityEngine.Debug.LogError("==========OutEMGun==========");
			_ChangeGunType(m_curType);
			SetNative(IsNative());
			HW2_Singleton<SoundMgr>.Get().StopClip(HW2_Singleton<SoundMgr>.Get().GetClip("电磁炮聚能音效"));
			if (IsNative() && m_preGunType == GunType.LockingGun)
			{
				CheckAutoLock();
			}
		}

		private void InitLockGun()
		{
			chain = EffectMgr.Get().GetLockChain(GetId());
			InitFishTypeSpriteDictionary();
		}

		private void SetUserInputOnclick()
		{
			if (IsNative())
			{
				SimpleSingletonBehaviour<UserInput>.Get().onClick = ClickSpaceArea;
			}
		}

		public void LockFish(FishBehaviour fish)
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
			lockingFishTask = new Task(LockingFish(fish, chain));
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
			Action<FishBehaviour> wrapAction = delegate(FishBehaviour _fish)
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
			Action<FishBehaviour> dyingAction = delegate(FishBehaviour _fish)
			{
				UnityEngine.Debug.Log($"FishDying[{_fish.identity}]");
				wrapAction(_fish);
			};
			Action<FishBehaviour> dieAction = delegate(FishBehaviour _fish)
			{
				UnityEngine.Debug.Log($"FishDie[{_fish.identity}]");
				wrapAction(_fish);
			};
			Action<FishBehaviour> escapeScreenCall = delegate(FishBehaviour _fish)
			{
				UnityEngine.Debug.Log($"FishEscapeScreen[{_fish.identity}]");
				wrapAction(_fish);
			};
			FishBehaviour fishBehaviour = fish;
			fishBehaviour.Event_FishDying_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.Event_FishDying_Handler, dyingAction);
			FishBehaviour fishBehaviour2 = fish;
			fishBehaviour2.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, dieAction);
			FishBehaviour fishBehaviour3 = fish;
			fishBehaviour3.Event_FishEscapeScreen_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour3.Event_FishEscapeScreen_Handler, escapeScreenCall);
			Action event_OnChangeTarget = delegate
			{
				FishBehaviour fishBehaviour4 = fish;
				fishBehaviour4.Event_FishDying_Handler = (Action<FishBehaviour>)Delegate.Remove(fishBehaviour4.Event_FishDying_Handler, dyingAction);
				FishBehaviour fishBehaviour5 = fish;
				fishBehaviour5.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Remove(fishBehaviour5.Event_FishDie_Handler, dieAction);
				FishBehaviour fishBehaviour6 = fish;
				fishBehaviour6.Event_FishEscapeScreen_Handler = (Action<FishBehaviour>)Delegate.Remove(fishBehaviour6.Event_FishEscapeScreen_Handler, escapeScreenCall);
			};
			chain.Event_OnChangeTarget = event_OnChangeTarget;
		}

		private IEnumerator LockingFish(FishBehaviour fish, LockChainController chain)
		{
			if (!IsNative())
			{
				yield break;
			}
			int fishId = fish.id;
			int fishType = (int)fish.type;
			HW2_MessageCenter.SendMessage("LockGunSelectFish", new KeyValueInfo("fishId", JsonUtility.ToJson(new LockGunSelectInfo(HW2_GVars.lobby.curSeatId, fish.id, GetPower()))));
			bool stopLocking;
			while (true)
			{
				int gunValue = GetPower();
				yield return new WaitForSeconds(shootInterval);
				int curScore = SimpleSingletonBehaviour<SaveAndTakeScores>.Get().GetContext().curScore;
				if (curScore < GetPower())
				{
					gunValue = curScore;
				}
				if (curScore != 0)
				{
					if (m_curType == GunType.LockingGun)
					{
						if (fish.State == FishState.Live && fish.inScreen)
						{
							if (fishId == fish.id)
							{
								int gunAngle = 0;
								int gunType = 1;
								HW2_MessageCenter.SendMessage("LockGunShoot", new KeyValueInfo("LockGunShootInfo", JsonUtility.ToJson(new LockGunShootInfo(gunValue, gunAngle, gunType, fishId, fishType))));
								fish.Locked = true;
								continue;
							}
							UnityEngine.Debug.Log(HW2_LogHelper.Lime("LockingFish[id:{0}] dead and changed, newId:{1}", fishId, fish.id));
							stopLocking = true;
							break;
						}
						UnityEngine.Debug.Log(HW2_LogHelper.Lime("LockingFish[id:{0}] dead", fishId));
						stopLocking = true;
						break;
					}
					stopLocking = true;
					break;
				}
				stopLocking = true;
				SimpleSingletonBehaviour<OptionController>.Get().Show_InOutPanel();
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

		public void UnLockFish(FishBehaviour fish, LockChainController chain)
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

		public void AutoLock(LockChainController chain)
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

		private IEnumerator AutoLockIE(LockChainController chain)
		{
			List<FishBehaviour> fishList;
			while (true)
			{
				fishList = FishMgr.Get().GetScreenFishList((FishBehaviour _fish) => _fish.gameObject.activeInHierarchy && _fish.State == FishState.Live && _fish.inScreen);
				if (fishList.Count != 0)
				{
					break;
				}
				yield return null;
			}
			fishList.Sort((FishBehaviour a, FishBehaviour b) => b.GetFishRateWeight() - a.GetFishRateWeight());
			FishBehaviour fish = fishList[0];
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

		public void ShowLockingFishSprite(FishType fishType)
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
							item.GetComponent<Effect_Logo_Crystal>().Stop();
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
			StopCoroutine(SimpleSingletonBehaviour<SaveAndTakeScores>.Get().GunMoveDelay());
			StartCoroutine(SimpleSingletonBehaviour<SaveAndTakeScores>.Get().GunMoveDelay());
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
			MainGameLogic3_UI.MainCamera.transform.localEulerAngles = Vector3.zero;
			_input.GetComponent<BoxCollider2D>().enabled = true;
		}
	}
}
