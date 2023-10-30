using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/FK3_UnityConstraints/Constraint - Turntable")]
	public class FK3_TurntableConstraint : FK3_ConstraintFrameworkBaseClass
	{
		public float speed = 1f;

		public bool randomStart;

		protected override void OnEnable()
		{
			base.OnEnable();
			if (Application.isPlaying && randomStart)
			{
				base.transform.Rotate(0f, Random.value * 360f, 0f);
			}
		}

		protected override void OnConstraintUpdate()
		{
			base.transform.Rotate(0f, speed, 0f);
		}
	}
}
