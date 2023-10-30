using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/UnityConstraints/Constraint - Billboard")]
	public class BillboardConstraint : LookAtBaseClass
	{
		public bool vertical = true;

		protected override void Awake()
		{
			base.Awake();
			Camera[] array = Object.FindObjectsOfType(typeof(Camera)) as Camera[];
			Camera[] array2 = array;
			int num = 0;
			Camera camera;
			while (true)
			{
				if (num < array2.Length)
				{
					camera = array2[num];
					if ((camera.cullingMask & (1 << base.gameObject.layer)) > 0)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			base.target = camera.transform;
		}

		protected override void OnConstraintUpdate()
		{
			Vector3 a = base.transform.position + base.target.rotation * Vector3.back;
			Vector3 upwards = Vector3.up;
			if (vertical)
			{
				upwards = base.target.rotation * Vector3.up;
			}
			else
			{
				Vector3 position = base.transform.position;
				a.y = position.y;
			}
			Vector3 forward = a - base.transform.position;
			Quaternion lookRot = Quaternion.LookRotation(forward, upwards);
			base.transform.rotation = GetUserLookRotation(lookRot);
		}
	}
}
