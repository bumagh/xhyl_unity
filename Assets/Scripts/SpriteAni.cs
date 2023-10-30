using UnityEngine;
using UnityEngine.UI;

public class SpriteAni : MonoBehaviour
{
	public UI2DSprite sprite2d;

	public Image imageS;

	public Sprite[] sp;

	private int num;

	private float waittime;

	private void Update()
	{
		waittime += Time.deltaTime;
		if (waittime > 0.1f)
		{
			waittime = 0f;
			if (num < sp.Length - 1)
			{
				num++;
			}
			else
			{
				num = 0;
			}
			if (sprite2d == null)
			{
				imageS.sprite = sp[num];
			}
			else
			{
				sprite2d.sprite2D = sp[num];
			}
		}
	}
}
