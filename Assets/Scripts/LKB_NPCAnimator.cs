using System.Collections.Generic;
using UnityEngine;

public class LKB_NPCAnimator : MonoBehaviour
{
	[SerializeField]
	private LKB_ImageAnim[] anims;

	[SerializeField]
	private List<string> strAnims;

	private int animIndex;

	private void Start()
	{
		Play("normal");
	}

	private void Update()
	{
		if (!anims[animIndex].bPlaying && animIndex != 0)
		{
			anims[0].Play();
		}
	}

	public void Play(string str)
	{
		anims[0].Stop();
		anims[animIndex].Stop();
		if (strAnims.Contains(str))
		{
			animIndex = strAnims.IndexOf(str);
			anims[animIndex].Play();
		}
	}

	public void Stop(string str)
	{
		if (strAnims.Contains(str))
		{
			animIndex = strAnims.IndexOf(str);
			anims[animIndex].Stop();
		}
	}
}
