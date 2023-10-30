using System;
using UnityEngine;

namespace M__M.HaiWang.Bullet
{
	public class FK3_BulletController : MonoBehaviour
	{
		[SerializeField]
		private Vector3 m_speed;

		[SerializeField]
		public float m_speedValue = 2f;

		public bool isCanMove = true;

		public static Rect boundary = new Rect(-8.5f, -4.5f, 16.8f, 9.2f);

		public int seatId;

		public int gunTypeId;

		public int gunValue;

		public int bulletId;

		public bool used;

		public static Action<FK3_BulletController> OnStartAction;

		public Action<FK3_BulletController> Event_BulletOver_Handler;

		private Vector3 position;

		private Vector3 position2;

		public Vector3 direction
		{
			get;
			set;
		}

		private void Awake()
		{
			float num = 30f;
			float f = num * ((float)Math.PI / 180f);
			m_speed = new Vector3(m_speedValue * Mathf.Sin(f), m_speedValue * Mathf.Cos(f), 0f);
			base.transform.rotation = Quaternion.Euler(Vector3.back * num);
			Transform transform = base.transform;
			Vector3 vector = base.transform.position;
			float x = vector.x;
			Vector3 vector2 = base.transform.position;
			transform.position = new Vector3(x, vector2.y, 0f);
			boundary = FK3_BulletMgr.Get().bulletBorder.rect;
		}

		private void Start()
		{
			if (OnStartAction != null)
			{
				OnStartAction(this);
			}
		}

		private void Update()
		{
			Vector3 localPosition = base.transform.localPosition;
			if (localPosition.z != 0.001f)
			{
				Transform transform = base.transform;
				Vector3 localPosition2 = base.transform.localPosition;
				float x = localPosition2.x;
				Vector3 localPosition3 = base.transform.localPosition;
				transform.localPosition = new Vector3(x, localPosition3.y, 0.001f);
			}
		}

		private void FixedUpdate()
		{
			if (isCanMove)
			{
				position = base.transform.localPosition;
				if (m_speed.x > 0f && position.x > boundary.xMax)
				{
					m_speed.x *= -1f;
					Rotate(m_speed);
				}
				else if (m_speed.x < 0f && position.x < boundary.xMin)
				{
					m_speed.x *= -1f;
					Rotate(m_speed);
				}
				else if (m_speed.y > 0f && position.y > boundary.yMax)
				{
					m_speed.y *= -1f;
					Rotate(m_speed);
				}
				else if (m_speed.y < 0f && position.y < boundary.yMin)
				{
					m_speed.y *= -1f;
					Rotate(m_speed);
				}
				position2 = position + m_speed * Time.fixedDeltaTime;
				base.transform.localPosition = position2;
			}
		}

		public void Launch(Vector3 startPos, float angle)
		{
			float f = angle * ((float)Math.PI / 180f);
			m_speed = new Vector3(m_speedValue * Mathf.Sin(f), m_speedValue * Mathf.Cos(f), 0f);
			base.transform.position = startPos;
			used = false;
			Rotate(angle);
		}

		private void Rotate(Vector3 direction)
		{
			base.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(direction.x, direction.y, 0f));
		}

		private void Rotate(float angle)
		{
			base.transform.rotation = Quaternion.Euler(Vector3.back * angle);
		}

		public void Over()
		{
			if (Event_BulletOver_Handler != null)
			{
				Event_BulletOver_Handler(this);
			}
		}
	}
}
