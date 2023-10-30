using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/FK3_UnityConstraints/Constraint - Transform (Postion, Rotation, Scale)")]
	public class FK3_TransformConstraint : FK3_ConstraintBaseClass
	{
		public bool constrainPosition = true;

		public bool outputPosX = true;

		public bool outputPosY = true;

		public bool outputPosZ = true;

		public bool constrainRotation;

		public FK3_UnityConstraints.OUTPUT_ROT_OPTIONS output;

		public bool constrainScale;

		internal Transform parXform;

		internal static Transform scaleCalculator;

		protected override void Awake()
		{
			base.Awake();
			parXform = base.transform.parent;
		}

		protected override void OnConstraintUpdate()
		{
			if (constrainScale)
			{
				SetWorldScale(base.target);
			}
			if (constrainRotation)
			{
				base.transform.rotation = base.target.rotation;
				FK3_UnityConstraints.MaskOutputRotations(base.transform, output);
			}
			if (constrainPosition)
			{
				base.pos = base.transform.position;
				if (outputPosX)
				{
					ref Vector3 pos = ref base.pos;
					Vector3 position = base.target.position;
					pos.x = position.x;
				}
				if (outputPosY)
				{
					ref Vector3 pos2 = ref base.pos;
					Vector3 position2 = base.target.position;
					pos2.y = position2.y;
				}
				if (outputPosZ)
				{
					ref Vector3 pos3 = ref base.pos;
					Vector3 position3 = base.target.position;
					pos3.z = position3.z;
				}
				base.transform.position = base.pos;
			}
		}

		protected override void NoTargetDefault()
		{
			if (constrainScale)
			{
				base.transform.localScale = Vector3.one;
			}
			if (constrainRotation)
			{
				base.transform.rotation = Quaternion.identity;
			}
			if (constrainPosition)
			{
				base.transform.position = Vector3.zero;
			}
		}

		public virtual void SetWorldScale(Transform sourceXform)
		{
			base.transform.localScale = GetTargetLocalScale(sourceXform);
		}

		internal Vector3 GetTargetLocalScale(Transform sourceXform)
		{
			if (scaleCalculator == null)
			{
				string name = "TransformConstraint_spaceCalculator";
				GameObject gameObject = GameObject.Find(name);
				if (gameObject != null)
				{
					scaleCalculator = gameObject.transform;
				}
				else
				{
					GameObject gameObject2 = new GameObject(name);
					gameObject2.gameObject.hideFlags = HideFlags.HideAndDontSave;
					scaleCalculator = gameObject2.transform;
				}
			}
			Transform transform = scaleCalculator;
			transform.parent = sourceXform;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			transform.parent = parXform;
			return transform.localScale;
		}
	}
}
