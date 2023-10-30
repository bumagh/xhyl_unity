using UnityEngine;

namespace PathologicalGames
{
	[RequireComponent(typeof(Targetable))]
	public class DemoPlayer : MonoBehaviour
	{
		public int life = 100;

		public ParticleSystem explosion;

		protected bool isDead;

		protected void Awake()
		{
			Targetable component = GetComponent<Targetable>();
			component.AddOnHitDelegate(OnHit);
		}

		protected void OnHit(EventInfoList infoList, Target target)
		{
			if (!isDead)
			{
				foreach (EventInfo info in infoList)
				{
					EventInfo current = info;
					switch (current.name)
					{
					case "Damage":
						life -= (int)current.value;
						break;
					case "Life":
						life += (int)current.value;
						break;
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

		protected void OnGUI()
		{
			GUI.Label(new Rect(10f, 10f, 100f, 20f), "LIFE: " + life);
		}
	}
}
