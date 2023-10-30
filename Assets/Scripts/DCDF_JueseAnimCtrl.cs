using UnityEngine;

public class DCDF_JueseAnimCtrl : MonoBehaviour
{
	private Animator anim;

	private string[] strAnims = new string[3]
	{
		"Idle1",
		"Idle2",
		"Idle3"
	};

	private AnimationClip acIdle1;

	private int count;

	private int changeCount;

	private void Awake()
	{
		count = 0;
		changeCount = UnityEngine.Random.Range(1, 5);
		anim = base.transform.GetComponent<Animator>();
		acIdle1 = anim.runtimeAnimatorController.animationClips[0];
		acIdle1.events = null;
		AddAnimEvent();
	}

	private void AddAnimEvent()
	{
		AnimationEvent animationEvent = new AnimationEvent();
		animationEvent.functionName = "PlayIdle";
		animationEvent.time = acIdle1.length;
		acIdle1.AddEvent(animationEvent);
	}

	public void PlayIdle()
	{
		count++;
		if (count >= changeCount)
		{
			count = 0;
			changeCount = UnityEngine.Random.Range(1, 5);
			int num = UnityEngine.Random.Range(0, 3);
			anim.Play(strAnims[num]);
		}
	}
}
