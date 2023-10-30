using UnityEngine;

namespace PathologicalGames
{
	[RequireComponent(typeof(EventTrigger))]
	public class GrowWithShockwave : MonoBehaviour
	{
		public EventTrigger eventTrigger;

		private void Update()
		{
			Vector3 localScale = eventTrigger.range * 2.1f;
			localScale.y *= 0.2f;
			base.transform.localScale = localScale;
			Color color = GetComponent<Renderer>().material.GetColor("_TintColor");
			Vector3 range = eventTrigger.range;
			color.a = Mathf.Lerp(0.7f, 0f, range.x / eventTrigger.endRange.x);
			GetComponent<Renderer>().material.SetColor("_TintColor", color);
		}
	}
}
