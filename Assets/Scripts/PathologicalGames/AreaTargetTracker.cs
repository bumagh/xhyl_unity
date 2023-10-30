using System;
using System.Collections;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Area TargetTracker")]
	public class AreaTargetTracker : TargetTracker
	{
		public enum AREA_SHAPES
		{
			Capsule,
			Box,
			Sphere,
			Box2D,
			Circle2D
		}

		[SerializeField]
		protected Vector3 _range = Vector3.one;

		[SerializeField]
		protected AREA_SHAPES _areaShape = AREA_SHAPES.Sphere;

		[SerializeField]
		protected Vector3 _areaPositionOffset = Vector3.zero;

		[SerializeField]
		protected Vector3 _areaRotationOffset = Vector3.zero;

		[SerializeField]
		protected int _areaLayer;

		protected bool areaColliderEnabledAtStart = true;

		public Vector3 range
		{
			get
			{
				return _range;
			}
			set
			{
				_range = value;
				if (area != null)
				{
					UpdateAreaRange();
				}
			}
		}

		public bool is2D
		{
			get
			{
				int num = 7;
				return Enum.IsDefined(typeof(AREA_SHAPES), num);
			}
		}

		public AREA_SHAPES areaShape
		{
			get
			{
				return _areaShape;
			}
			set
			{
				_areaShape = value;
				if (!(area == null))
				{
					StartCoroutine(UpdateAreaShape());
				}
			}
		}

		public Vector3 areaPositionOffset
		{
			get
			{
				return _areaPositionOffset;
			}
			set
			{
				_areaPositionOffset = value;
				if (!(area == null))
				{
					area.transform.localPosition = value;
				}
			}
		}

		public Vector3 areaRotationOffset
		{
			get
			{
				return _areaRotationOffset;
			}
			set
			{
				_areaRotationOffset = value;
				if (!(area == null))
				{
					area.transform.localRotation = Quaternion.Euler(value);
				}
			}
		}

		public int areaLayer
		{
			get
			{
				return _areaLayer;
			}
			set
			{
				_areaLayer = value;
				if (!(area == null))
				{
					area.gameObject.layer = value;
				}
			}
		}

		public override Area area
		{
			get;
			protected set;
		}

		public bool areaColliderEnabled
		{
			get
			{
				if (area != null)
				{
					if (area.coll != null)
					{
						return area.coll.enabled;
					}
					if (area.coll2D != null)
					{
						return area.coll2D.enabled;
					}
				}
				return false;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			GameObject gameObject = new GameObject(base.name + "_Area");
			gameObject.transform.parent = base.transform;
			gameObject.SetActive(value: false);
			gameObject.SetActive(value: true);
			gameObject.transform.localPosition = areaPositionOffset;
			gameObject.transform.localRotation = Quaternion.Euler(areaRotationOffset);
			gameObject.layer = areaLayer;
			StartCoroutine(UpdateAreaShape(gameObject));
			setAreaColliderEnabled(gameObject, areaColliderEnabledAtStart);
			area = gameObject.AddComponent<Area>();
			area.targetTracker = this;
			UpdateAreaRange();
		}

		protected IEnumerator UpdateAreaShape()
		{
			StartCoroutine(UpdateAreaShape(area.go));
			yield break;
		}

		protected IEnumerator UpdateAreaShape(GameObject areaGO)
		{
			Collider oldCol = areaGO.GetComponent<Collider>();
			Collider2D oldCol2D = areaGO.GetComponent<Collider2D>();
			switch (areaShape)
			{
			case AREA_SHAPES.Capsule:
				if (oldCol is CapsuleCollider)
				{
					yield break;
				}
				break;
			case AREA_SHAPES.Box:
				if (oldCol is BoxCollider)
				{
					yield break;
				}
				break;
			case AREA_SHAPES.Sphere:
				if (oldCol is SphereCollider)
				{
					yield break;
				}
				break;
			case AREA_SHAPES.Box2D:
				if (oldCol2D is BoxCollider2D)
				{
					yield break;
				}
				break;
			case AREA_SHAPES.Circle2D:
				if (oldCol2D is CircleCollider2D)
				{
					yield break;
				}
				break;
			default:
				throw new Exception("Unsupported collider type.");
			}
			if (oldCol != null)
			{
				UnityEngine.Object.Destroy(oldCol);
			}
			if (oldCol2D != null)
			{
				UnityEngine.Object.Destroy(oldCol2D);
			}
			switch (areaShape)
			{
			case AREA_SHAPES.Capsule:
			case AREA_SHAPES.Box:
			case AREA_SHAPES.Sphere:
				if (areaGO.GetComponent<Rigidbody2D>() != null)
				{
					UnityEngine.Object.Destroy(areaGO.GetComponent<Rigidbody2D>());
					yield return null;
				}
				if (areaGO.GetComponent<Rigidbody>() == null)
				{
					Rigidbody rigidbody = areaGO.AddComponent<Rigidbody>();
					rigidbody.isKinematic = true;
				}
				break;
			case AREA_SHAPES.Box2D:
			case AREA_SHAPES.Circle2D:
				if (areaGO.GetComponent<Rigidbody>() != null)
				{
					UnityEngine.Object.Destroy(areaGO.GetComponent<Rigidbody>());
					yield return null;
				}
				if (areaGO.GetComponent<Rigidbody2D>() == null)
				{
					Rigidbody2D rigidbody2D = areaGO.AddComponent<Rigidbody2D>();
					rigidbody2D.isKinematic = true;
				}
				break;
			}
			Collider coll = null;
			Collider2D coll2D = null;
			switch (areaShape)
			{
			case AREA_SHAPES.Capsule:
				coll = areaGO.AddComponent<CapsuleCollider>();
				coll.isTrigger = true;
				break;
			case AREA_SHAPES.Box:
				coll = areaGO.AddComponent<BoxCollider>();
				coll.isTrigger = true;
				break;
			case AREA_SHAPES.Sphere:
				coll = areaGO.AddComponent<SphereCollider>();
				coll.isTrigger = true;
				break;
			case AREA_SHAPES.Box2D:
				coll2D = areaGO.AddComponent<BoxCollider2D>();
				coll2D.isTrigger = true;
				break;
			case AREA_SHAPES.Circle2D:
				coll2D = areaGO.AddComponent<CircleCollider2D>();
				coll2D.isTrigger = true;
				break;
			default:
				throw new Exception("Unsupported collider type.");
			}
			if (coll != null)
			{
				coll.isTrigger = true;
				if (area != null)
				{
					area.coll = coll;
				}
			}
			else if (coll2D != null)
			{
				coll2D.isTrigger = true;
				if (area != null)
				{
					area.coll2D = coll2D;
				}
			}
			UpdateAreaRange(coll, coll2D);
		}

		protected void UpdateAreaRange()
		{
			Collider coll = area.coll;
			Collider2D coll2D = area.coll2D;
			UpdateAreaRange(coll, coll2D);
		}

		protected void UpdateAreaRange(Collider col, Component col2D)
		{
			Vector3 normalizedRange = GetNormalizedRange();
			if (col is SphereCollider)
			{
				SphereCollider sphereCollider = (SphereCollider)col;
				sphereCollider.radius = normalizedRange.x;
				return;
			}
			if (col is BoxCollider)
			{
				BoxCollider boxCollider = (BoxCollider)col;
				boxCollider.size = normalizedRange;
				return;
			}
			if (col is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)col;
				capsuleCollider.radius = normalizedRange.x;
				capsuleCollider.height = normalizedRange.y;
				return;
			}
			if (col2D is CircleCollider2D)
			{
				CircleCollider2D circleCollider2D = (CircleCollider2D)col2D;
				circleCollider2D.radius = normalizedRange.x;
				return;
			}
			if (col2D is BoxCollider2D)
			{
				BoxCollider2D boxCollider2D = (BoxCollider2D)col2D;
				boxCollider2D.size = new Vector2(normalizedRange.x, normalizedRange.y);
				return;
			}
			string name;
			string name2;
			if (col != null)
			{
				name = col.GetType().Name;
				name2 = col.name;
			}
			else
			{
				if (!(col2D != null))
				{
					throw new NullReferenceException("No Collider Found!");
				}
				name = col2D.GetType().Name;
				name2 = col2D.name;
			}
			UnityEngine.Debug.LogWarning($"Unsupported collider type '{name}' for collider '{name2}'.");
		}

		public Vector3 GetNormalizedRange()
		{
			Vector3 result = Vector3.zero;
			switch (areaShape)
			{
			case AREA_SHAPES.Capsule:
				result = new Vector3(_range.x, _range.y * 2f, _range.x);
				break;
			case AREA_SHAPES.Box:
				result = new Vector3(_range.x * 2f, _range.y, _range.z * 2f);
				break;
			case AREA_SHAPES.Sphere:
				result = new Vector3(_range.x, _range.x, _range.x);
				break;
			case AREA_SHAPES.Box2D:
				result = new Vector3(_range.x * 2f, _range.y * 2f, 0f);
				break;
			case AREA_SHAPES.Circle2D:
				result = new Vector3(_range.x, _range.x, 0f);
				break;
			}
			return result;
		}

		public void setAreaColliderEnabled(bool enable)
		{
			setAreaColliderEnabled(area.go, enable);
		}

		protected void setAreaColliderEnabled(GameObject areaGO, bool enable)
		{
			Collider collider = (!(area != null)) ? areaGO.GetComponent<Collider>() : area.coll;
			if (collider != null)
			{
				collider.enabled = enable;
				return;
			}
			Collider2D collider2D = (!(area != null)) ? areaGO.GetComponent<Collider2D>() : area.coll2D;
			if (collider2D != null)
			{
				collider2D.enabled = enable;
				return;
			}
			throw new Exception("Unexpected Error: No area collider found.");
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (area != null)
			{
				area.go.SetActive(value: true);
			}
		}

		protected virtual void OnDisable()
		{
			if (!(area == null))
			{
				area.Clear();
				area.go.SetActive(value: false);
			}
		}
	}
}
