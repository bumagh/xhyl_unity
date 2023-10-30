using UnityEngine;

namespace PathologicalGames
{
	public class FK3_DemoEnemy : MonoBehaviour
	{
		public int life = 100;

		public ParticleSystem explosion;

		protected Color startingColor;

		protected bool isDead;

		protected void Awake()
		{
			startingColor = GetComponent<Renderer>().material.color;
			FK3_Targetable component = GetComponent<FK3_Targetable>();
			component.AddOnDetectedDelegate(MakeMeBig);
			component.AddOnDetectedDelegate(MakeMeGreen);
			component.AddOnNotDetectedDelegate(MakeMeNormal);
			component.AddOnNotDetectedDelegate(ResetColor);
			component.AddOnHitDelegate(OnHit);
		}

		protected void OnHit(FK3_EventInfoList infoList, FK3_Target target)
		{
			if (!isDead)
			{
				foreach (FK3_EventInfo info in infoList)
				{
					FK3_EventInfo current = info;
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

		protected void MakeMeGreen(FK3_TargetTracker source)
		{
			if (!isDead)
			{
				GetComponent<Renderer>().material.color = Color.green;
			}
		}

		protected void ResetColor(FK3_TargetTracker source)
		{
			if (!isDead)
			{
				GetComponent<Renderer>().material.color = startingColor;
			}
		}

		protected void MakeMeBig(FK3_TargetTracker source)
		{
			if (!isDead)
			{
				base.transform.localScale = new Vector3(2f, 2f, 2f);
			}
		}

		protected void MakeMeNormal(FK3_TargetTracker source)
		{
			if (!isDead)
			{
				base.transform.localScale = Vector3.one;
			}
		}
	}
}
