using System;
using UnityEngine;

public class PTM_NPC : MonoBehaviour
{
	public Animator animator;

	public Action<string> onAniEvent;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void OnAniEvent(string aniEvent)
	{
		if (onAniEvent != null)
		{
			onAniEvent(aniEvent);
		}
	}
}
