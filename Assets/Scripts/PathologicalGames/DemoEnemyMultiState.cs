using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	public class DemoEnemyMultiState : MonoBehaviour
	{
		protected enum STATES
		{
			Dead,
			NotDetected,
			Detected,
			ActiveTarget
		}

		public int life = 100;

		public ParticleSystem explosion;

		protected STATES currentState = STATES.NotDetected;

		protected bool isUpdateWhileTrackedRunning;

		protected Vector3 activeTargetScale = new Vector3(2f, 2f, 2f);

		protected Color startingColor;

		protected Targetable targetable;

		protected List<TargetTracker> detectingTrackers = new List<TargetTracker>();

		protected void Awake()
		{
			startingColor = GetComponent<Renderer>().material.color;
			targetable = GetComponent<Targetable>();
			targetable.AddOnDetectedDelegate(OnDetected);
			targetable.AddOnNotDetectedDelegate(OnNotDetected);
			targetable.AddOnHitDelegate(OnHit);
		}

		protected void OnHit(EventInfoList infoList, Target target)
		{
			if (currentState != 0)
			{
				if (target.collider != null)
				{
					UnityEngine.Debug.Log(base.name + " was hit by 3D collider on " + target.collider.name);
				}
				if (target.collider2D != null)
				{
					UnityEngine.Debug.Log(base.name + " was hit by 2D collider on " + target.collider2D.name);
				}
				foreach (EventInfo info in infoList)
				{
					EventInfo current = info;
					string name = current.name;
					if (name != null && name == "Damage")
					{
						life -= (int)current.value;
					}
				}
				if (life <= 0)
				{
					SetState(STATES.Dead);
					Object.Instantiate(explosion.gameObject, base.transform.position, base.transform.rotation);
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		protected void OnDetected(TargetTracker source)
		{
			detectingTrackers.Add(source);
			if (!isUpdateWhileTrackedRunning)
			{
				StartCoroutine(UpdateWhileTracked());
			}
		}

		protected void OnNotDetected(TargetTracker source)
		{
			detectingTrackers.Remove(source);
		}

		protected void SetState(STATES state)
		{
			if (currentState != state && currentState != 0)
			{
				switch (state)
				{
				case STATES.NotDetected:
					base.transform.localScale = Vector3.one;
					GetComponent<Renderer>().material.color = startingColor;
					break;
				case STATES.Detected:
					GetComponent<Renderer>().material.color = Color.yellow;
					base.transform.localScale = activeTargetScale * 0.75f;
					break;
				case STATES.ActiveTarget:
					GetComponent<Renderer>().material.color = Color.green;
					base.transform.localScale = activeTargetScale;
					break;
				}
				currentState = state;
			}
		}

		protected IEnumerator UpdateWhileTracked()
		{
			isUpdateWhileTrackedRunning = true;
			while (detectingTrackers.Count > 0)
			{
				if (currentState == STATES.Dead)
				{
					yield break;
				}
				bool switchedToActive = false;
				foreach (TargetTracker detectingTracker in detectingTrackers)
				{
					if (detectingTracker.targets.Contains(new Target(targetable, detectingTracker)))
					{
						SetState(STATES.ActiveTarget);
						switchedToActive = true;
						break;
					}
				}
				if (!switchedToActive)
				{
					SetState(STATES.Detected);
				}
				yield return null;
			}
			SetState(STATES.NotDetected);
			isUpdateWhileTrackedRunning = false;
		}
	}
}
