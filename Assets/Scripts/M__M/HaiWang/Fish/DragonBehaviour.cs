using DG.Tweening;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Fish
{
	public class DragonBehaviour : FishBehaviour
	{
		public Animator animator2;

		public DragonCollisionController collisionController;

		[SerializeField]
		private Transform[] _boomPositions;

		[SerializeField]
		private Transform _centerTrans;

		public override event EventHandler_FishOnHit Event_FishOnHit_Handler;

		private void Awake()
		{
			m_renderer = GetComponent<Image>();
			collisionController = GetComponent<DragonCollisionController>();
			collisionController.ListeningColliderTriggerEvent(Handle_DragonOnHit);
			collisionController.ColliderCallBack(base.OnMouseDown);
		}

		private void Handle_DragonOnHit(Collider collider)
		{
			UnityEngine.Debug.Log("Handle_DragonOnHit");
			if (Event_FishOnHit_Handler != null)
			{
				Event_FishOnHit_Handler(this, collider);
			}
		}

		public void DoDying()
		{
		}

		public override void Dying(bool playAni = true)
		{
			base.State = FishState.Dying;
			dyingAgeInSec = ageInSec;
			MovePause();
			animator.enabled = false;
			animator2.enabled = false;
			collisionController.EnableCollider(value: false);
			if (Event_FishDying_Handler != null)
			{
				Event_FishDying_Handler(this);
			}
		}

		public override void Prepare()
		{
			if (animator == null)
			{
				animator = GetComponent<Animator>();
			}
			if (animator2 == null)
			{
				animator = GetComponent<Animator>();
			}
			animator.enabled = true;
			animator2.enabled = true;
			try
			{
				animator.Play(string.Empty);
				animator2.Play(string.Empty);
				animator.Play("Dorgan_Life");
				animator2.Play("Take 001", -1, 0f);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			collisionController.EnableCollider(value: true);
			DOTween.Kill(base.transform);
			StopAllCoroutines();
			StartCoroutine(IE_LifetimeControl());
		}

		private IEnumerator IE_LifetimeControl()
		{
			yield return new WaitForSeconds(8f);
			UnityEngine.Debug.Log("IE_LifetimeControl");
			if (base.gameObject.activeSelf)
			{
				Die();
			}
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
