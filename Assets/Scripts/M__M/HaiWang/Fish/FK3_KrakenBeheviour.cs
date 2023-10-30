using DG.Tweening;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Message;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_KrakenBeheviour : FK3_FishBehaviour
	{
		protected FK3_Task _playLifeAniTask;

		protected FK3_Task _retractDieTask;

		[SerializeField]
		private FK3_CollisionReceiver _collisionReceiver;

		[SerializeField]
		private Transform[] _boomPositions;

		[SerializeField]
		private Transform _centerTrans;

		[SerializeField]
		private Transform _blinkCircle;

		private float _blinkCircleEffectInterval = 1f;

		private float _blinkCircleEffectCount = 2f;

		private float remainTime = 1000f;

		public override event EventHandler_FishOnHit Event_FishOnHit_Handler;

		public void SetPositionAndRemainTime(int pos, float time)
		{
			remainTime = time;
			int num = pos + 1;
			Transform krakenBornPosList = FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().krakenBornPosList;
			Transform transform = krakenBornPosList.Find("pos" + num);
			base.transform.SetParent(transform);
			base.transform.localRotation = Quaternion.identity;
			Transform transform2 = krakenBornPosList.parent.Find("TagPos");
			base.transform.localPosition = Vector3.zero;
			transform.transform.SetSiblingIndex(krakenBornPosList.childCount);
			Vector3 position = base.transform.parent.position;
			Vector3 position2 = transform2.position;
			Vector3 toDirection = position2 - position;
			Vector3 position3 = (position + position2) / 2f;
			base.transform.position = position3;
			base.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);
			float num2 = 1f;
			int num3;
			switch (num)
			{
			case 1:
				num3 = -803;
				num2 = 1.3f;
				break;
			case 2:
				num3 = -806;
				num2 = 1.3f;
				break;
			case 3:
				num3 = -853;
				num2 = 1f;
				break;
			case 4:
				num3 = -856;
				num2 = 1f;
				break;
			case 5:
				num3 = -903;
				num2 = 1.1f;
				break;
			case 6:
				num3 = -906;
				num2 = 1.1f;
				break;
			default:
				num3 = -803;
				num2 = 1f;
				break;
			}
			base.transform.localScale = new Vector3(num2, num2, -1f);
			_blinkCircle.GetComponent<FK3_SortingLayerSetter>().OrderInLayer = num3 + 1;
		}

		private void Awake()
		{
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
			if (_collisionReceiver == null)
			{
				_collisionReceiver = GetComponent<FK3_CollisionReceiver>();
			}
			if (_collisionReceiver == null)
			{
				_collisionReceiver = base.transform.GetComponentInChildren<FK3_CollisionReceiver>();
			}
			_collisionReceiver.actionOnTriggerEnter = delegate(Transform _transform, Collider _collider)
			{
				if (_collider.CompareTag("Bullet"))
				{
					UnityEngine.Debug.LogError(_transform.gameObject.name + " 碰撞到了: " + _collider.gameObject.name);
					string name2 = base.transform.parent.name;
					FK3_EffectMgr.Get().PlayBlinkCirle(_blinkCircle, int.Parse(name2.Substring(name2.Length - 1, 1)));
					if (Event_FishOnHit_Handler != null)
					{
						Event_FishOnHit_Handler(this, _collider);
					}
				}
			};
			_collisionReceiver.actionOnMouseDown = delegate
			{
				string name = base.transform.parent.name;
				FK3_EffectMgr.Get().PlayBlinkCirle(_blinkCircle, int.Parse(name.Substring(name.Length - 1, 1)));
				OnMouseDown();
			};
		}

		public override void Update()
		{
			base.Update();
			if (_blinkCircle.gameObject.activeInHierarchy)
			{
				_blinkCircle.Rotate(Vector3.forward * 2f);
			}
			if (base.Locked)
			{
				_blinkCircleEffectCount += Time.deltaTime;
				if (_blinkCircleEffectCount > _blinkCircleEffectInterval)
				{
					string name = base.transform.parent.name;
					FK3_EffectMgr.Get().PlayBlinkCirle(_blinkCircle, int.Parse(name.Substring(name.Length - 1, 1)));
					_blinkCircleEffectCount = 0f;
				}
			}
			if (base.State == FK3_FishState.Live)
			{
				remainTime -= Time.deltaTime;
				if (remainTime <= 0f)
				{
					Retract();
					remainTime = 1000f;
				}
			}
		}

		private void CleanKraken(FK3_KeyValueInfo keyValueInfo)
		{
			if (base.State == FK3_FishState.Live)
			{
				Die();
			}
		}

		public override void Prepare()
		{
			_blinkCircleEffectCount = 2f;
			if (animator == null)
			{
				animator = base.transform.GetChild(0).GetComponent<Animator>();
			}
			if (animator == null)
			{
				animator = base.transform.GetComponent<Animator>();
			}
			animator.enabled = true;
			animator.ResetTrigger("Birth");
			animator.ResetTrigger("Life");
			animator.ResetTrigger("Die");
			animator.SetTrigger("Birth");
			_playLifeAniTask = new FK3_Task(PlayLifeAniAfterBirth());
			DOTween.Kill(base.transform);
			_collisionReceiver.xCollider.enabled = false;
			_blinkCircle.gameObject.SetActive(value: true);
		}

		private IEnumerator PlayLifeAniAfterBirth()
		{
			yield return new WaitForSeconds(0.5f);
			_collisionReceiver.xCollider.enabled = true;
			animator.ResetTrigger("Birth");
			animator.ResetTrigger("Life");
			animator.ResetTrigger("Die");
			animator.SetTrigger("Life");
		}

		public override void Retract()
		{
			StopTask();
			base.State = FK3_FishState.Dying;
			FK3_MessageCenter.SendMessage("CheckAutoLock", null);
			_retractDieTask = new FK3_Task(RetractDie());
		}

		private IEnumerator RetractDie()
		{
			try
			{
				animator.ResetTrigger("Birth");
				animator.ResetTrigger("Life");
				animator.ResetTrigger("Die");
				animator.SetTrigger("Die");
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("============动画播放错误==========" + arg);
			}
			yield return new WaitForSeconds(1.2f);
			Die();
		}

		public override void Die()
		{
			base.Die();
		}

		public override void EnableCollider(bool value)
		{
			if (_collisionReceiver.xCollider != null)
			{
				_collisionReceiver.xCollider.enabled = value;
			}
			_blinkCircle.gameObject.SetActive(value);
		}

		private void StopTask()
		{
			if (_retractDieTask != null && _retractDieTask.isRunning)
			{
				_retractDieTask.Stop();
			}
			if (_playLifeAniTask != null && _playLifeAniTask.isRunning)
			{
				_playLifeAniTask.Stop();
			}
		}

		public override void Dying(bool playAni = true)
		{
			StopTask();
			base.State = FK3_FishState.Dying;
			dyingAgeInSec = ageInSec;
			EnableCollider(value: false);
			MovePause();
			animator.enabled = false;
			if (Event_FishDying_Handler != null)
			{
				Event_FishDying_Handler(this);
			}
		}

		public override void LeaveScene()
		{
			EnableCollider(value: false);
			animator.ResetTrigger("Birth");
			animator.ResetTrigger("Life");
			animator.ResetTrigger("Die");
			animator.SetTrigger("Die");
			UnityEngine.Debug.LogError("LeaveScene");
			base.gameObject.transform.DOScale(Vector3.zero, 1f).OnComplete(delegate
			{
				Die();
			});
		}

		public override Transform[] GetBoomPositions()
		{
			return _boomPositions;
		}

		public override Transform GetCenterTransform()
		{
			return _centerTrans;
		}
	}
}
