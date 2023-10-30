using UnityEngine;

namespace PathologicalGames
{
	public class DemoEnemy : MonoBehaviour
	{
		public int life = 100;

		public ParticleSystem explosion;

		protected Color startingColor;

		protected bool isDead;

		protected void Awake()
		{
			startingColor = GetComponent<Renderer>().material.color;
			Targetable component = GetComponent<Targetable>();
			component.AddOnDetectedDelegate(MakeMeBig);
			component.AddOnDetectedDelegate(MakeMeGreen);
			component.AddOnNotDetectedDelegate(MakeMeNormal);
			component.AddOnNotDetectedDelegate(ResetColor);
			component.AddOnHitDelegate(OnHit);
		}

		protected void OnHit(EventInfoList infoList, Target target)
		{
			if (!isDead)
			{
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
					isDead = true;
					Object.Instantiate(explosion.gameObject, base.transform.position, base.transform.rotation);
					base.gameObject.SetActive(value: false);
				}
			}
		}

		protected void MakeMeGreen(TargetTracker source)
		{
			if (!isDead)
			{
				GetComponent<Renderer>().material.color = Color.green;
			}
		}

		protected void ResetColor(TargetTracker source)
		{
			if (!isDead)
			{
				GetComponent<Renderer>().material.color = startingColor;
			}
		}

		protected void MakeMeBig(TargetTracker source)
		{
			if (!isDead)
			{
				base.transform.localScale = new Vector3(2f, 2f, 2f);
			}
		}

		protected void MakeMeNormal(TargetTracker source)
		{
			if (!isDead)
			{
				base.transform.localScale = Vector3.one;
			}
		}
	}
}
