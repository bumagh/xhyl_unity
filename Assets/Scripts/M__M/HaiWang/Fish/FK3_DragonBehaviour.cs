using DG.Tweening;
using DragonBones;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Fish
{
	public class FK3_DragonBehaviour : FK3_FishBehaviour
	{
		public UnityArmatureComponent unityArmature;

		public FK3_DragonCollisionController collisionController;

		[SerializeField]
		private UnityEngine.Transform[] _boomPositions;

		[SerializeField]
		private UnityEngine.Transform _centerTrans;

		public override event EventHandler_FishOnHit Event_FishOnHit_Handler;

		private void Awake()
		{
			m_Image = GetComponent<Image>();
			m_renderer = GetComponent<SpriteRenderer>();
			if (type == FK3_FishType.烈焰龟)
			{
				m_renderer = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
			}
			collisionController = GetComponent<FK3_DragonCollisionController>();
			collisionController.ListeningColliderTriggerEvent(Handle_DragonOnHit);
			collisionController.ColliderCallBack(base.OnMouseDown);
		}

		private void OnEnable()
		{
			base.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
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
			base.State = FK3_FishState.Dying;
			dyingAgeInSec = ageInSec;
			MovePause();
			if ((bool)animator)
			{
				animator.enabled = false;
			}
			collisionController.EnableCollider(value: false);
			if (Event_FishDying_Handler != null)
			{
				Event_FishDying_Handler(this);
			}
		}

		public override void Prepare()
		{
			unityArmature.animation.timeScale = 1.2f;
			unityArmature.animation.Play();
			collisionController.EnableCollider(value: true);
			DOTween.Kill(base.transform);
			StopAllCoroutines();
			StartCoroutine(IE_LifetimeControl());
		}

		private IEnumerator IE_LifetimeControl()
		{
			yield return new WaitForSeconds(5f);
			if (base.gameObject.activeSelf)
			{
				collisionController.EnableCollider(value: false);
				Die();
				unityArmature.animation.timeScale = 0.1f;
				unityArmature.animation.Stop();
			}
		}

		public override UnityEngine.Transform[] GetBoomPositions()
		{
			return _boomPositions;
		}

		public override UnityEngine.Transform GetCenterTransform()
		{
			return _centerTrans;
		}
	}
}
