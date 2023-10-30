using DG.Tweening;
using FullInspector;
using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Fish
{
	[fiInspectorOnly]
	[AddComponentMenu("Game/Fish/FishBehaviour")]
	public class FK3_FishBehaviour : MonoBehaviour
	{
		public delegate void EventHandler_FishOnHit(FK3_FishBehaviour fish, Collider other);

		public delegate void EventHandler_FishOnSpawned(FK3_FishBehaviour fish, FK3_SpawnPool pool);

		public delegate void EventHandler_FishOnDespawned(FK3_FishBehaviour fish, FK3_SpawnPool pool);

		public int id;

		public FK3_FishType type;

		public FK3_FishType originalType;

		public bool isLightning;

		public float depth;

		public float radius = 0.8f;

		[NonSerialized]
		public bool inScreen;

		[NonSerialized]
		public float ageInSec;

		[NonSerialized]
		public float dyingAgeInSec;

		[NonSerialized]
		public float deadAgeInSec;

		[HideInInspector]
		public Animator animator;

		private Collider m_collider;

		protected Image m_Image;

		protected SpriteRenderer m_renderer;

		protected HashSet<string> animatorTriggers = new HashSet<string>();

		public Action<FK3_FishBehaviour> Event_FishDying_Handler;

		public Action<FK3_FishBehaviour> Event_FishDie_Handler;

		public Action<FK3_FishBehaviour> Event_FishDieFinish_Handler;

		public Action<FK3_FishBehaviour> Event_FishOnUpdate_Handler;

		public Action<FK3_FishBehaviour> Event_FishOnDoubleClick_Handler;

		public Action<FK3_FishBehaviour> Event_FishMovePause_Handler;

		public Action<FK3_FishBehaviour> Event_FishEscapeScreen_Handler;

		public Action<FK3_FishBehaviour, string> Event_FishOnAniEvent_Handler;

		public Action Event_ShowSize_Handler;

		public Action<FK3_FishBehaviour> RemoveMovementAction;

		public static Action<FK3_FishBehaviour> SEvent_FishOnStart_Handler;

		public static Action<FK3_FishBehaviour> SEvent_FishOnAwake_Handler;

		public bool useLog;

		private Vector3 m_scale = Vector3.one;

		[NotSerialized]
		[ShowInInspector]
		public Dictionary<string, object> lifetimeDic = new Dictionary<string, object>();

		[NotSerialized]
		[ShowInInspector]
		public Dictionary<string, object> permanentDic = new Dictionary<string, object>();

		private float m_lastClickTimer;

		private Coroutine waitToOver;

		private bool isDie;

		private Image imageFish;

		private Image component;

		private SpriteRenderer renderFish;

		private SpriteRenderer componentFish;

		public int displayOrder
		{
			get;
			private set;
		}

		public bool Hitable
		{
			get;
			set;
		}

		public bool Locked
		{
			get;
			set;
		}

		public FK3_FishState State
		{
			get;
			set;
		}

		public float OpenTime
		{
			get;
			set;
		}

		public string identity => $"[id:{id},type:{type},hash:{GetHashCode()}]";

		public virtual event EventHandler_FishOnHit Event_FishOnHit_Handler;

		public event EventHandler_FishOnSpawned Event_FishOnSpawned_Handler;

		public event EventHandler_FishOnDespawned Event_FishOnDespawned_Handler;

		public void Reset_Basic()
		{
		}

		public void Reset_EventHandler()
		{
			this.Event_FishOnHit_Handler = null;
			this.Event_FishOnSpawned_Handler = null;
			this.Event_FishOnDespawned_Handler = null;
			Event_FishDying_Handler = null;
			Event_FishDie_Handler = null;
			Event_FishDieFinish_Handler = null;
			Event_FishOnUpdate_Handler = null;
			Event_FishMovePause_Handler = null;
			Event_FishEscapeScreen_Handler = null;
			Event_FishOnDoubleClick_Handler = null;
			Event_FishOnAniEvent_Handler = null;
			RemoveMovementAction = null;
			Event_ShowSize_Handler = null;
		}

		[InspectorButton]
		public string Debug_Analyze_EventHandler(bool print = true)
		{
			string text = $"fish:{identity}\nOnhit:[{_GetEventListenerCount(this.Event_FishOnHit_Handler)}]\nOnSpawned:[{_GetEventListenerCount(this.Event_FishOnSpawned_Handler)}]\nDying:[{_GetEventListenerCount(Event_FishDying_Handler)}]\nDie:[{_GetEventListenerCount(Event_FishDie_Handler)}]\nDieFinish:[{_GetEventListenerCount(Event_FishDieFinish_Handler)}]\nMovePause:[{_GetEventListenerCount(Event_FishMovePause_Handler)}]\nEscapeScreen:[{_GetEventListenerCount(Event_FishEscapeScreen_Handler)}]";
			if (print)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Orange(text));
			}
			return text;
		}

		private int _GetEventListenerCount(MulticastDelegate delegateFun)
		{
			return ((object)delegateFun != null) ? delegateFun.GetInvocationList().Length : 0;
		}

		private void Awake()
		{
			try
			{
				if (animator == null)
				{
					animator = GetComponent<Animator>();
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			Reset_EventHandler();
			if (SEvent_FishOnAwake_Handler != null)
			{
				SEvent_FishOnAwake_Handler(this);
			}
			m_collider = GetComponent<Collider>();
			m_Image = GetComponent<Image>();
			m_renderer = GetComponent<SpriteRenderer>();
			if (type == FK3_FishType.烈焰龟)
			{
				m_renderer = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
			}
			if (m_collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)m_collider;
			}
			if (animator != null)
			{
				animatorTriggers.Clear();
				AnimatorControllerParameter[] parameters = animator.parameters;
				foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
				{
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Trigger)
					{
						animatorTriggers.Add(animatorControllerParameter.name);
					}
				}
			}
			m_scale = base.transform.localScale;
		}

		private void Start()
		{
			if (SEvent_FishOnStart_Handler != null)
			{
				SEvent_FishOnStart_Handler(this);
			}
		}

		public virtual void Update()
		{
			m_lastClickTimer += Time.deltaTime;
			ageInSec += Time.deltaTime;
			if (Event_FishOnUpdate_Handler != null)
			{
				Event_FishOnUpdate_Handler(this);
			}
			if (OpenTime > 0f && ageInSec > OpenTime && IsLive() && m_collider.enabled)
			{
				OpenTime = 0f;
				try
				{
					UnityEngine.Debug.LogError("===============Size==============");
					PlayAni("Size");
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("============动画播放错误==========" + arg);
				}
				if (Event_ShowSize_Handler != null)
				{
					Event_ShowSize_Handler();
				}
			}
			if (((State == FK3_FishState.Dead && !isDie) || ((bool)m_collider && !m_collider.enabled)) && base.gameObject.activeInHierarchy && type != FK3_FishType.烈焰龟)
			{
				if (waitToOver != null)
				{
					StopCoroutine(waitToOver);
				}
				waitToOver = StartCoroutine(WaitToOver());
				isDie = true;
			}
		}

		private IEnumerator WaitToOver()
		{
			yield return new WaitForSeconds(2.5f);
			if (base.gameObject != null && base.gameObject.activeInHierarchy)
			{
				UnityEngine.Debug.LogError("强制删除");
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (useLog)
			{
				UnityEngine.Debug.Log(EventLog("OnTriggerEnter"));
			}
			if (other.CompareTag("Bullet") && this.Event_FishOnHit_Handler != null)
			{
				this.Event_FishOnHit_Handler(this, other);
			}
		}

		protected void OnMouseDown()
		{
			if (State == FK3_FishState.Live)
			{
				if (m_lastClickTimer < 0.5f && Event_FishOnDoubleClick_Handler != null)
				{
					Event_FishOnDoubleClick_Handler(this);
				}
				m_lastClickTimer = 0f;
			}
		}

		private void OnMouseUp()
		{
			if (State == FK3_FishState.Live)
			{
			}
		}

		private void OnEnable()
		{
			isDie = false;
		}

		private void OnDisable()
		{
			isDie = false;
		}

		private void OnDestroy()
		{
			if (useLog)
			{
				UnityEngine.Debug.Log(EventLog("OnDestroy"));
			}
		}

		public void OnAniEvent(string aniEventName)
		{
			if (Event_FishOnAniEvent_Handler != null)
			{
				Event_FishOnAniEvent_Handler(this, aniEventName);
			}
		}

		public void Reset_OnSpawned()
		{
			ageInSec = 0f;
			deadAgeInSec = 0f;
			dyingAgeInSec = 0f;
			inScreen = true;
			Hitable = true;
			Locked = false;
		}

		private void OnSpawned(FK3_SpawnPool pool)
		{
			if (this.Event_FishOnSpawned_Handler != null)
			{
				this.Event_FishOnSpawned_Handler(this, pool);
			}
		}

		private void OnDespawned(FK3_SpawnPool pool)
		{
			if (this.Event_FishOnDespawned_Handler != null)
			{
				this.Event_FishOnDespawned_Handler(this, pool);
			}
		}

		public void MovePause()
		{
			if (useLog)
			{
				UnityEngine.Debug.Log(EventLog("MovePause"));
			}
			if (Event_FishMovePause_Handler != null)
			{
				Event_FishMovePause_Handler(this);
			}
		}

		public virtual void EnableCollider(bool value)
		{
			if (m_collider != null)
			{
				m_collider.enabled = value;
			}
		}

		public virtual void Dying(bool playAni = true)
		{
			State = FK3_FishState.Dying;
			dyingAgeInSec = ageInSec;
			EnableCollider(value: false);
			MovePause();
			if (RemoveMovementAction != null)
			{
				RemoveMovementAction(this);
			}
			if (playAni)
			{
				try
				{
					PlayAni("Die");
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("============动画播放错误==========" + arg);
				}
			}
			if (Event_FishDying_Handler != null)
			{
				Event_FishDying_Handler(this);
			}
		}

		public virtual void Retract()
		{
		}

		public virtual void LeaveScene()
		{
			EnableCollider(value: false);
			imageFish = GetFishImage();
			renderFish = GetFishRenderer();
			if (isLightning)
			{
				component = base.transform.GetChild(0).GetComponent<Image>();
				if ((bool)component)
				{
					component.DOFade(0f, 3f);
				}
				else
				{
					componentFish = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
					if ((bool)componentFish)
					{
						componentFish.DOFade(0f, 3f);
					}
				}
			}
			if (imageFish != null)
			{
				imageFish.DOFade(0f, 3f).OnComplete(delegate
				{
					Die();
				});
			}
			if (renderFish != null)
			{
				renderFish.DOFade(0f, 3f).OnComplete(delegate
				{
					Die();
				});
			}
			else
			{
				Die();
			}
		}

		public virtual void Die()
		{
			if (type == FK3_FishType.Boss_Dorgan_冰封暴龙 || type == FK3_FishType.Boss_Dorgan_狂暴火龙)
			{
				GetComponent<FK3_DragonBehaviour>().unityArmature.animation.timeScale = 0.1f;
			}
			State = FK3_FishState.Dead;
			deadAgeInSec = ageInSec;
			if (!(base.gameObject == null) && base.gameObject.activeSelf)
			{
				if (Event_FishDie_Handler != null)
				{
					Event_FishDie_Handler(this);
				}
				if (Event_FishDieFinish_Handler != null)
				{
					Event_FishDieFinish_Handler(this);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		public virtual void Die(float time)
		{
			State = FK3_FishState.Dead;
			deadAgeInSec = ageInSec;
			if (!(base.gameObject == null) && base.gameObject.activeSelf)
			{
				if (Event_FishDie_Handler != null)
				{
					Event_FishDie_Handler(this);
				}
				if (Event_FishDieFinish_Handler != null)
				{
					Event_FishDieFinish_Handler(this);
					return;
				}
				UnityEngine.Debug.LogError("Destroy: " + base.gameObject.name);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		public void PlayAni(string name)
		{
			foreach (string animatorTrigger in animatorTriggers)
			{
				animator.ResetTrigger(animatorTrigger);
			}
			if (!animatorTriggers.Contains(name))
			{
				return;
			}
			animator.SetTrigger(name);
			if (name == "Life")
			{
				AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
				if (currentAnimatorClipInfo != null && currentAnimatorClipInfo.Length != 0)
				{
					AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
					animator.Play(animatorClipInfo.clip.name, 0, 0f);
				}
			}
		}

		public void PlayAni2(string name)
		{
			try
			{
				animator.SetTrigger(name);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		public void PlayAni(string name, Animator tempAnimator, float speed = 1f)
		{
			UnityEngine.Debug.LogError(base.gameObject.name + " 播放: " + name);
			try
			{
				foreach (string animatorTrigger in animatorTriggers)
				{
					tempAnimator.ResetTrigger(animatorTrigger);
					tempAnimator.speed = speed;
				}
				if (animatorTriggers.Contains(name))
				{
					tempAnimator.SetTrigger(name);
					if (name == "Life")
					{
						AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
						if (currentAnimatorClipInfo != null && currentAnimatorClipInfo.Length != 0)
						{
							AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
							tempAnimator.Play(animatorClipInfo.clip.name, 0, 0f);
							tempAnimator.speed = speed;
						}
					}
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		public void SetLayerOrder(int order)
		{
			if (order > 14500)
			{
			}
			displayOrder = order;
		}

		public Image GetFishImage()
		{
			if (m_Image != null)
			{
				return m_Image;
			}
			return null;
		}

		public SpriteRenderer GetFishRenderer()
		{
			if (m_renderer != null)
			{
				return m_renderer;
			}
			return null;
		}

		public virtual int GetSpriteOrder()
		{
			return 0;
		}

		public Image GetMeshRenderer()
		{
			if (m_Image != null)
			{
				return m_Image;
			}
			return null;
		}

		private void _AdjustZDepth()
		{
			Vector3 position = base.transform.position;
			position.z = depth;
			base.transform.position = position;
		}

		public void SetPosition(Vector3 pos, bool adjustZDepth = true)
		{
			base.transform.position = new Vector3(pos.x, pos.y, adjustZDepth ? depth : pos.z);
		}

		public virtual void Prepare()
		{
			foreach (string animatorTrigger in animatorTriggers)
			{
				animator.ResetTrigger(animatorTrigger);
			}
			try
			{
				PlayAni("Life");
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("============动画播放错误==========" + arg);
			}
			EnableCollider(value: true);
			DOTween.Kill(base.transform);
			lifetimeDic.Clear();
		}

		public virtual Transform[] GetBoomPositions()
		{
			return new Transform[1]
			{
				base.transform
			};
		}

		public virtual Transform GetCenterTransform()
		{
			return base.transform;
		}

		public virtual void DoFade()
		{
		}

		public virtual void DoMoveByPath()
		{
		}

		public virtual void DoEscape()
		{
		}

		public virtual void DoEscapeScreen()
		{
			if (Event_FishEscapeScreen_Handler != null)
			{
				Event_FishEscapeScreen_Handler(this);
			}
		}

		public virtual void DoCaptured()
		{
		}

		public string EventLog(string name)
		{
			return $"Fish.{name} {identity}";
		}

		public static void SetAlpha(Image renderer, float value)
		{
			Color color = renderer.color;
			color.a = value;
			renderer.color = color;
		}

		public static void SetAlpha(SpriteRenderer renderer, float value)
		{
			Color color = renderer.color;
			color.a = value;
			renderer.color = color;
		}

		public bool IsLive()
		{
			return State == FK3_FishState.Live && base.gameObject.activeInHierarchy;
		}

		public void Reset_Scale()
		{
			base.transform.localScale = m_scale;
		}
	}
}
