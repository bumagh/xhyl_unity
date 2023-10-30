using UnityEngine;

namespace PathologicalGames
{
	[RequireComponent(typeof(EventTrigger))]
	[RequireComponent(typeof(SmoothLookAtConstraint))]
	public class DemoSeeker : MonoBehaviour
	{
		public float maxVelocity = 500f;

		public float acceleration = 75f;

		protected EventTrigger projectile;

		protected Rigidbody rbd;

		protected Rigidbody2D rbd2D;

		protected float minDrag = 10f;

		protected float drag = 40f;

		protected void Awake()
		{
			rbd = GetComponent<Rigidbody>();
			rbd2D = GetComponent<Rigidbody2D>();
			projectile = GetComponent<EventTrigger>();
			projectile.AddOnListenStartDelegate(OnLaunched);
			projectile.AddOnListenUpdateDelegate(OnLaunchedUpdate);
			projectile.AddOnFireDelegate(OnEventTriggerFire);
		}

		protected void OnLaunched()
		{
			if (rbd != null)
			{
				rbd.drag = drag;
			}
			else if (rbd2D != null)
			{
				rbd2D.drag = drag;
			}
		}

		protected void OnLaunchedUpdate()
		{
			if (!projectile.target.isSpawned)
			{
				projectile.hitMode = EventTrigger.HIT_MODES.HitLayers;
			}
			else
			{
				projectile.hitMode = EventTrigger.HIT_MODES.TargetOnly;
			}
			if (rbd != null)
			{
				if (rbd.drag > minDrag)
				{
					rbd.drag -= acceleration * 0.01f;
				}
				rbd.AddForce(base.transform.forward * maxVelocity);
			}
			else if (rbd2D != null)
			{
				if (rbd2D.drag > minDrag)
				{
					rbd2D.drag -= acceleration * 0.01f;
				}
				rbd2D.AddForce(base.transform.up * maxVelocity);
			}
		}

		protected void OnEventTriggerFire(TargetList targets)
		{
		}
	}
}
