using DG.Tweening;
using M__M.HaiWang.FSM;
using M__M.HaiWang.Player.GunState;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Player.Gun
{
	public class GunController : MonoBehaviour
	{
		public delegate void EventHandler_RotateByInput(GunController gun, Vector3 begin, Vector3 end, float angle);

		public delegate void EventHandler_Shoot(GunController gun, Vector3 pos, bool faceDown, float angle, int gunValue, int seatId);

		[SerializeField]
		private GameObject m_gunObj;

		[SerializeField]
		private Animator m_animator;

		private bool m_isFaceDown;

		private float m_angle;

		public int seatId;

		public bool seatTake;

		public bool isNativePlayer;

		public int gunPower;

		public bool shootable;

		public int lockingFishId;

		public FSM<GunController> gunFSM;

		private GameObject _normalGunGmObj;

		private GameObject _lockingGunGmObj;

		private GameObject _EMgunGmObj;

		private ParticleSystem _EMStoreEngergyParticle;

		private ParticleSystem _EMShootParticle;

		private GameObject _EMLogoGmObj;

		private GameObject _EMRangeGmObj;

		private GameObject _FireTimeCountDownGmObj;

		public EventHandler_Shoot Event_Shoot_Handler;

		private bool m_debug_rotateByInput;

		private bool m_debug_ray = true;

		public event EventHandler_RotateByInput Event_RotateByInput_Handler;

		private void Awake()
		{
		}

		private void Start()
		{
			Transform transform = base.transform.Find("Gun");
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(value: false);
			}
			_normalGunGmObj = base.transform.Find("Gun/GatlingGun").gameObject;
			_lockingGunGmObj = base.transform.Find("Gun/LockingGun").gameObject;
			_EMgunGmObj = base.transform.Find("Gun/EMGun").gameObject;
			_EMStoreEngergyParticle = _EMgunGmObj.transform.Find("VFX_Bullet_LaserBullet_Fire_XuLi").GetComponent<ParticleSystem>();
			_EMShootParticle = _EMgunGmObj.transform.Find("VFX_Bullet_LaserBullet_Fire").GetComponent<ParticleSystem>();
			_FireTimeCountDownGmObj = base.transform.Find("Gun/FireTime_CountDown").gameObject;
			_EMLogoGmObj = _EMgunGmObj.transform.Find("Laser_Logo").gameObject;
			_EMRangeGmObj = _EMgunGmObj.transform.Find("Laser_Range").gameObject;
			_EMRangeGmObj.SetActive(value: true);
			gunFSM = new FSM<GunController>(this);
			gunFSM.ChangeState<NormalGun>();
		}

		private void Update()
		{
			if (gunFSM != null)
			{
				gunFSM.CurrentState.OnUpdate();
			}
		}

		public void RotateByInput()
		{
			(gunFSM.CurrentState as GunState<GunController>)?.RotateByInput();
		}

		private IEnumerator FireTimeCountDown()
		{
			_FireTimeCountDownGmObj.SetActive(value: true);
			int count = 10;
			float interval = 1f;
			while (count > 0)
			{
				count--;
				_FireTimeCountDownGmObj.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.25f);
				_FireTimeCountDownGmObj.transform.Find("NumText").GetComponent<TextMesh>().text = count.ToString();
				yield return new WaitForSeconds(interval * 0.25f);
				_FireTimeCountDownGmObj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.15f);
				yield return new WaitForSeconds(interval * 0.75f);
			}
		}

		public void Shoot()
		{
			(gunFSM.CurrentState as GunState<GunController>)?.Shoot();
		}

		public void RemoteShoot(float angle, int gunValue, int bulletType)
		{
			_rotateByAngle(angle);
			Shoot();
		}

		public void UserSwitchGun()
		{
			if (gunFSM.IsInState<NormalGun>())
			{
				gunFSM.ChangeState<LockingGun>();
			}
			else if (gunFSM.IsInState<LockingGun>())
			{
				gunFSM.ChangeState<NormalGun>();
			}
		}

		public void TurnToNormalGun()
		{
			_normalGunGmObj.SetActive(value: true);
			m_animator = _normalGunGmObj.GetComponent<Animator>();
		}

		public void TurnFromNormalGun()
		{
		}

		public void NormalGunShoot()
		{
			UnityEngine.Debug.Log("Shoot");
			int gunValue = 100;
			Vector3 position = base.transform.position;
			if (gunFSM.IsInState<NormalGun>())
			{
				m_animator.SetTrigger("Shoot");
				if (Event_Shoot_Handler != null)
				{
					Event_Shoot_Handler(this, position, m_isFaceDown, m_angle, gunValue, seatId);
				}
			}
		}

		public void NormalGunRotateByInput()
		{
			if (m_debug_rotateByInput)
			{
				UnityEngine.Debug.Log("mouse screen pos " + UnityEngine.Input.mousePosition);
			}
			Camera main = Camera.main;
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			Vector3 position = Camera.main.transform.position;
			Vector3 vector = main.ScreenToWorldPoint(mousePosition + position.z * Vector3.back);
			if (m_debug_rotateByInput)
			{
				UnityEngine.Debug.Log("mouse world pos " + vector);
			}
			Vector3 position2 = m_gunObj.transform.position;
			Vector3 vector2 = new Vector3(vector.x, vector.y, position2.z);
			float d = m_isFaceDown ? (-1f) : 1f;
			float num = Vector3.Angle(Vector3.up * d, vector2 - position2) * (float)((!(vector.x <= position2.x)) ? 1 : (-1));
			if (m_debug_rotateByInput)
			{
				UnityEngine.Debug.Log("angle " + num);
			}
			_rotateByAngle(num);
			if (this.Event_RotateByInput_Handler != null)
			{
				this.Event_RotateByInput_Handler(this, position2, vector2, num);
			}
		}

		public void TurnToLockingGun()
		{
			_lockingGunGmObj.SetActive(value: true);
			m_animator = _lockingGunGmObj.GetComponent<Animator>();
			_normalGunGmObj.SetActive(value: false);
		}

		public void TurnFromLockingGun()
		{
			_normalGunGmObj.SetActive(value: true);
			_lockingGunGmObj.SetActive(value: false);
		}

		public void TurnToEMGun(Vector3 pos, Action callBack)
		{
			_rotateByAngle(0f);
			_EMgunGmObj.SetActive(value: true);
			m_animator = _EMgunGmObj.GetComponent<Animator>();
			m_animator.enabled = false;
			Transform transform = _EMgunGmObj.transform;
			float x = pos.x;
			float y = pos.y;
			Vector3 position = m_gunObj.transform.position;
			transform.position = new Vector3(x, y, position.z);
			_EMgunGmObj.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
			float num = 0.6f;
			Sequence sequence = DOTween.Sequence();
			sequence.Append(_EMgunGmObj.transform.DOMoveY(num, 0.4f).SetRelative().SetEase(Ease.OutQuad));
			sequence.Insert(0.5f, _EMgunGmObj.transform.DOMoveY(-1.25f * num, 0.3f).SetRelative().SetEase(Ease.InQuad));
			sequence.Insert(0.8f, _EMgunGmObj.transform.DOMoveY(0.5f * num, 0.15f).SetRelative().SetEase(Ease.OutQuad));
			sequence.Insert(0.95f, _EMgunGmObj.transform.DOMoveY(-0.375f * num, 0.15f).SetRelative().SetEase(Ease.InQuad));
			sequence.Insert(1.1f, _EMgunGmObj.transform.DOMoveY(0.25f * num, 0.15f).SetRelative().SetEase(Ease.OutQuad));
			sequence.Insert(1.25f, _EMgunGmObj.transform.DOMoveY(-0.25f * num, 0.1f).SetRelative().SetEase(Ease.InQuad));
			sequence.Insert(1.35f, _EMgunGmObj.transform.DOMove(_normalGunGmObj.transform.position + Vector3.back * 0.1f, 1.2f).SetEase(Ease.InQuad));
			sequence.Insert(1.35f, _EMgunGmObj.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 1.2f).SetEase(Ease.InQuad));
			sequence.OnComplete(delegate
			{
				m_animator.enabled = true;
				_normalGunGmObj.SetActive(value: false);
				_EMgunGmObj.transform.localPosition = new Vector3(0f, 0.37f, -0.1f);
				_EMgunGmObj.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
				_EMgunGmObj.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.8f).OnComplete(delegate
				{
					callBack();
				});
			});
		}

		public void TurnToEMGunPhase2()
		{
			_EMRangeGmObj.SetActive(value: true);
			_EMLogoGmObj.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 1f).SetEase(Ease.InOutBack);
			_EMRangeGmObj.transform.DOScale(new Vector3(4f, 3.5f, 1f), 1f).SetEase(Ease.OutCubic);
			_EMStoreEngergyParticle.gameObject.SetActive(value: true);
			_EMStoreEngergyParticle.Play();
		}

		public void EMGunPhase3Update()
		{
			UnityEngine.Debug.Log("FireTimeCountDown");
			StartCoroutine(FireTimeCountDown());
		}

		public void EMGunPhase4Update()
		{
			if (_EMShootParticle.isStopped)
			{
				_EMShootParticle.gameObject.SetActive(value: false);
				gunFSM.ChangeState<NormalGun>();
				_EMLogoGmObj.transform.localScale = Vector3.zero;
				_EMRangeGmObj.transform.localScale = new Vector3(0f, 3.5f, 1f);
			}
		}

		public void TurnFromEmGun()
		{
			_EMgunGmObj.SetActive(value: false);
		}

		public void EMGunShoot()
		{
			_FireTimeCountDownGmObj.SetActive(value: false);
			_EMRangeGmObj.SetActive(value: false);
			_EMStoreEngergyParticle.Stop();
			_EMStoreEngergyParticle.gameObject.SetActive(value: false);
			_EMShootParticle.gameObject.SetActive(value: true);
			_EMShootParticle.Play();
		}

		public void EMGunRotateByInput()
		{
			if ((gunFSM.CurrentState as EMGun).phase == EMGun.EMGunPhase.Phase2)
			{
				_rotateByAngle(_calculateAngleByInput());
			}
		}

		private void _rotateByAngle(float angle)
		{
			float d = m_isFaceDown ? (-1f) : 1f;
			angle = Mathf.Clamp(angle, -80f, 80f);
			m_gunObj.transform.rotation = Quaternion.Euler(Vector3.back * d * (angle + (m_isFaceDown ? 180f : 0f)));
			m_angle = angle;
		}

		private float _calculateAngleByInput()
		{
			if (m_debug_rotateByInput)
			{
				UnityEngine.Debug.Log("mouse screen pos " + UnityEngine.Input.mousePosition);
			}
			Camera main = Camera.main;
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			Vector3 position = Camera.main.transform.position;
			Vector3 vector = main.ScreenToWorldPoint(mousePosition + position.z * Vector3.back);
			if (m_debug_rotateByInput)
			{
				UnityEngine.Debug.Log("mouse world pos " + vector);
			}
			Vector3 position2 = m_gunObj.transform.position;
			Vector3 a = new Vector3(vector.x, vector.y, position2.z);
			float d = m_isFaceDown ? (-1f) : 1f;
			float num = Vector3.Angle(Vector3.up * d, a - position2) * (float)((!(vector.x <= position2.x)) ? 1 : (-1));
			if (m_debug_rotateByInput)
			{
				UnityEngine.Debug.Log("angle " + num);
			}
			return num;
		}
	}
}
