using System;
using UnityEngine;

public class FK3_GatlingBullet_Boom : MonoBehaviour
{
	public int seatId;

	private FK3_BulletBoom k3_BulletBoom;

	public event Action<FK3_GatlingBullet_Boom> Event_Over_Handler;

	private void OnDisable()
	{
		Over();
	}

	public void OnAniEvent(string name)
	{
		if (name.Equals("Finish"))
		{
			Over();
		}
	}

	public void Reset_EventHandler()
	{
		this.Event_Over_Handler = null;
	}

	private void Over()
	{
		if (this.Event_Over_Handler != null)
		{
			this.Event_Over_Handler(this);
			seatId = 0;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Play(Vector3 pos, int seatId)
	{
		Vector3 position = base.transform.position;
		pos.z = position.z;
		base.transform.position = pos;
		base.gameObject.SetActive(value: true);
		k3_BulletBoom = GetComponent<FK3_BulletBoom>();
		if ((bool)k3_BulletBoom)
		{
			k3_BulletBoom.SpriteRenderer.sprite = k3_BulletBoom.sprites[seatId];
		}
	}
}
