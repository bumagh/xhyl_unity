using System.Collections;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/Common/Self-Despawn")]
	public class FK3_SelfDespawn : MonoBehaviour
	{
		protected ParticleSystem particles;

		protected AudioSource audioSource;

		protected string poolName = string.Empty;

		protected void Awake()
		{
			particles = GetComponent<ParticleSystem>();
			audioSource = GetComponent<AudioSource>();
		}

		protected void OnEnable()
		{
			if ((bool)particles)
			{
				StartCoroutine(ListenToDespawn(particles));
			}
			if ((bool)audioSource)
			{
				StartCoroutine(ListenToDespawn(audioSource));
			}
		}

		protected IEnumerator ListenToDespawn(ParticleSystem emitter)
		{
			yield return new WaitForSeconds(emitter.main.startDelay.constantMax + 0.25f);
			while (emitter.IsAlive(withChildren: true))
			{
				if (!emitter.gameObject.activeInHierarchy)
				{
					emitter.Clear(withChildren: true);
					yield break;
				}
				yield return null;
			}
			FK3_InstanceManager.Despawn(poolName, emitter.transform);
		}

		protected IEnumerator ListenToDespawn(AudioSource src)
		{
			yield return null;
			while (src.isPlaying)
			{
				yield return null;
			}
			FK3_InstanceManager.Despawn(poolName, src.transform);
		}
	}
}
