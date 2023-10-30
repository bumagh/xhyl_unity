using System;
using UnityEngine;

public class GatlingBullet_Boom : MonoBehaviour
{
	public event Action<GatlingBullet_Boom> Event_Over_Handler;

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
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Play(Vector3 pos)
	{
		Vector3 position = base.transform.position;
		pos.z = position.z;
		base.transform.position = pos;
		base.gameObject.SetActive(value: true);
	}
}
