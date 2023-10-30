using System.Collections;
using UnityEngine;

public class SetLight : MonoBehaviour
{
	private ParticleSystem particleSystem;

	public float minA;

	public float maxA = 0.25f;

	public float time = 0.1f;

	private void Awake()
	{
		particleSystem = GetComponent<ParticleSystem>();
	}

	private void OnEnable()
	{
		StartCoroutine(WaitSetLight());
	}

	private IEnumerator WaitSetLight()
	{
		bool isAdd = true;
		while (true)
		{
			Color startColor = particleSystem.startColor;
			float curA3 = startColor.a;
			if (isAdd)
			{
				curA3 += 0.05f;
				ParticleSystem obj = particleSystem;
				Color startColor2 = particleSystem.startColor;
				float r = startColor2.r;
				Color startColor3 = particleSystem.startColor;
				float g = startColor3.g;
				Color startColor4 = particleSystem.startColor;
				obj.startColor = new Color(r, g, startColor4.b, curA3);
				if (curA3 >= maxA)
				{
					isAdd = false;
				}
			}
			else
			{
				curA3 -= 0.05f;
				ParticleSystem obj2 = particleSystem;
				Color startColor5 = particleSystem.startColor;
				float r2 = startColor5.r;
				Color startColor6 = particleSystem.startColor;
				float g2 = startColor6.g;
				Color startColor7 = particleSystem.startColor;
				obj2.startColor = new Color(r2, g2, startColor7.b, curA3);
				if (curA3 <= minA)
				{
					isAdd = true;
				}
			}
			yield return new WaitForSeconds(time);
		}
	}
}
