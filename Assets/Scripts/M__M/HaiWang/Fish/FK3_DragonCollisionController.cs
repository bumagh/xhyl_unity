using FullInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	[fiInspectorOnly]
	public class FK3_DragonCollisionController : MonoBehaviour
	{
		[SerializeField]
		private List<FK3_CollisionReceiver> _collisionReceiverList;

		[SerializeField]
		private FK3_DragonBehaviour dragonBehaviour;

		public Action<Transform, Collider> actionOnTriggerEnter;

		private int frameBulletCollisionCount;

		private void Awake()
		{
			dragonBehaviour = GetComponent<FK3_DragonBehaviour>();
		}

		private void Start()
		{
		}

		private void Update()
		{
			frameBulletCollisionCount = 0;
		}

		public void ListeningColliderTriggerEvent(Action<Collider> hander)
		{
			Action<Transform, Collider> action = delegate(Transform _transform, Collider _collider)
			{
				if (_collider.CompareTag("Bullet"))
				{
					if (frameBulletCollisionCount == 0)
					{
						hander(_collider);
					}
					frameBulletCollisionCount++;
				}
			};
			foreach (FK3_CollisionReceiver collisionReceiver in _collisionReceiverList)
			{
				collisionReceiver.actionOnTriggerEnter = action;
			}
		}

		public void ColliderCallBack(Action call)
		{
			foreach (FK3_CollisionReceiver collisionReceiver in _collisionReceiverList)
			{
				collisionReceiver.actionOnMouseDown = call;
			}
		}

		public void EnableCollider(bool value)
		{
			foreach (FK3_CollisionReceiver collisionReceiver in _collisionReceiverList)
			{
				collisionReceiver.xCollider.enabled = value;
			}
		}

		[InspectorButton]
		private void CheckSubCollisionReceivers()
		{
			_collisionReceiverList.Clear();
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				FK3_CollisionReceiver fK3_CollisionReceiver = collider.gameObject.GetComponent<FK3_CollisionReceiver>();
				if (fK3_CollisionReceiver == null)
				{
					fK3_CollisionReceiver = collider.gameObject.AddComponent<FK3_CollisionReceiver>();
				}
				_collisionReceiverList.Add(fK3_CollisionReceiver);
			}
		}
	}
}
