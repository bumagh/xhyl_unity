using UnityEngine;

public class DCDF_CaishenAnimCtrl : MonoBehaviour
{
	private Animator anim;

	private string[] strAnims = new string[3]
	{
		"Idle2",
		"Idle3",
		"Idle4"
	};

	private AnimationClip acIdle1;

	private void Awake()
	{
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
		int num = UnityEngine.Random.Range(0, 3);
		anim.Play(strAnims[num]);
	}

	public void PlayWin()
	{
		anim.Play("Win");
	}

	public void PlayFail()
	{
		anim.Play("Fail");
	}
}
