using System;
using UnityEngine;

public class DK_EffectLoadShader : MonoBehaviour
{
	private ParticleSystem[] particls;

	private Material material;

	private void Awake()
	{
		material = null;
		material = Resources.Load<Material>("FX Rain Drop");
	}

	private void OnEnable()
	{
		if (material == null)
		{
			material = Resources.Load<Material>("FX Rain Drop");
		}
		particls = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		for (int i = 0; i < particls.Length; i++)
		{
			try
			{
				particls[i].GetComponent<Renderer>().material = material;
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("=======es=======" + arg);
			}
		}
		try
		{
			GetComponent<ParticleSystem>().GetComponent<Renderer>().material = material;
		}
		catch (Exception arg2)
		{
			UnityEngine.Debug.LogError("=======es=======" + arg2);
		}
	}
}
