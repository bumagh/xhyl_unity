using UnityEngine;
using UnityEngine.UI;

public class LightEffect : MonoBehaviour
{
	private Image image;

	private float time;

	public float delay;

	private void Awake()
	{
		image = base.transform.GetComponent<Image>();
	}

	private void Update()
	{
		time += Time.deltaTime;
		if (time - delay >= 0f)
		{
			if (time - delay < 1f)
			{
				image.color = new Color(1f, 1f, 1f, time - delay);
			}
			else if (time - delay >= 1f && time - delay < 2f)
			{
				image.color = new Color(1f, 1f, 1f, 2f - time + delay);
			}
			else
			{
				time = 0f;
			}
		}
	}
}
