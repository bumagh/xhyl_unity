using System.Collections;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestDirector : MonoBehaviour
	{
		private static readonly Color NightColor = Color.black;

		private static readonly Color DayColor = new Color32(176, 224, 240, byte.MaxValue);

		public Light SunLight;

		public Light DirectionalLight;

		public ParticleSystem SunParticles;

		public Animator MoonAnimator;

		public Light MoonLight;

		public ParticleSystem StarsParticles;

		public GameObject Stars;

		public void Sun(int point)
		{
			switch (point)
			{
			case 0:
				StartCoroutine(ChangeBackColor(NightColor, DayColor));
				StartCoroutine(ChangeDirectLightIntensity(0f, 0.8f));
				SunParticles.Play();
				break;
			case 1:
				SunLight.intensity = 1f;
				Stars.transform.localPosition += new Vector3(0f, -20f);
				break;
			case 3:
				Stars.transform.localPosition -= new Vector3(0f, -20f);
				SunLight.intensity = 0f;
				SunParticles.Stop();
				break;
			}
		}

		public void Moon(int point)
		{
			switch (point)
			{
			case 0:
				StartCoroutine(ChangeBackColor(DayColor, NightColor));
				StartCoroutine(ChangeDirectLightIntensity(0.8f, 0f));
				StarsParticles.Play();
				break;
			case 1:
				MoonAnimator.SetBool("play", value: true);
				MoonLight.intensity = 1f;
				break;
			case 2:
				StarsParticles.Stop();
				break;
			case 3:
				MoonAnimator.SetBool("play", value: false);
				MoonLight.intensity = 0f;
				break;
			}
		}

		private IEnumerator ChangeBackColor(Color from, Color to)
		{
			float started = Time.time;
			while (Time.time - started < 1f)
			{
				Camera.main.backgroundColor = Color.Lerp(from, to, (Time.time - started) / 1f);
				yield return null;
			}
		}

		private IEnumerator ChangeDirectLightIntensity(float from, float to)
		{
			float started = Time.time;
			while (Time.time - started < 1f)
			{
				DirectionalLight.intensity = Mathf.Lerp(from, to, (Time.time - started) / 1f);
				yield return null;
			}
		}
	}
}
