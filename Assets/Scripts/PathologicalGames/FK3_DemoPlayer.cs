using UnityEngine;

namespace PathologicalGames
{
	[RequireComponent(typeof(FK3_Targetable))]
	public class FK3_DemoPlayer : MonoBehaviour
	{
		public int life = 100;

		public ParticleSystem explosion;

		protected bool isDead;

		protected void Awake()
		{
			FK3_Targetable component = GetComponent<FK3_Targetable>();
			component.AddOnHitDelegate(OnHit);
		}

		protected void OnHit(FK3_EventInfoList infoList, FK3_Target target)
		{
			if (!isDead)
			{
				foreach (FK3_EventInfo info in infoList)
				{
					FK3_EventInfo current = info;
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
