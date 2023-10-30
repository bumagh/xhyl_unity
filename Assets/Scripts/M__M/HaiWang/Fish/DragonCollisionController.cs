using FullInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	[fiInspectorOnly]
	public class DragonCollisionController : MonoBehaviour
	{
		[SerializeField]
		private List<CollisionReceiver> _collisionReceiverList;

		[SerializeField]
		private DragonBehaviour dragonBehaviour;

		public Action<Transform, Collider> actionOnTriggerEnter;

		private int frameBulletCollisionCount;

		private int count;

		private void Awake()
		{
			dragonBehaviour = GetComponent<DragonBehaviour>();
		}

		private void Start()
		{
		}

		private void Update()
		{
			frameBulletCollisionCount = 0;
			count++;
			count %= 30;
			if (count != 0)
			{
			}
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
			foreach (CollisionReceiver collisionReceiver in _collisionReceiverList)
			{
				collisionReceiver.actionOnTriggerEnter = action;
			}
		}

		public void ColliderCallBack(Action call)
		{
			foreach (CollisionReceiver collisionReceiver in _collisionReceiverList)
			{
				collisionReceiver.actionOnMouseDown = call;
			}
		}

		public void EnableCollider(bool value)
		{
			foreach (CollisionReceiver collisionReceiver in _collisionReceiverList)
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
				CollisionReceiver collisionReceiver = collider.gameObject.GetComponent<CollisionReceiver>();
				if (collisionReceiver == null)
				{
					collisionReceiver = collider.gameObject.AddComponent<CollisionReceiver>();
				}
				_collisionReceiverList.Add(collisionReceiver);
			}
		}
	}
}
