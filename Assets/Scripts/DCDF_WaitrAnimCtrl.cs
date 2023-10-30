using DG.Tweening;
using UnityEngine;

public class DCDF_WaitrAnimCtrl : MonoBehaviour
{
	private int index;

	private int nextIndex;

	private float[] posX = new float[4]
	{
		-1f,
		5f,
		12.5f,
		20f
	};

	private Animator anim;

	private AnimationClip acIdle1;

	private string strLastAction = "Feiwen";

	private Transform tfBodyControl;

	private bool bRight;

	private float speed = 2f;

	private int count;

	private void Awake()
	{
		anim = base.transform.GetComponent<Animator>();
		acIdle1 = anim.runtimeAnimatorController.animationClips[0];
		acIdle1.events = null;
		AddAnimEvent();
		tfBodyControl = base.transform.GetChild(0);
	}

	private void OnEnable()
	{
		index = 0;
		strLastAction = "Feiwen";
		tfBodyControl.DOKill();
		base.transform.DOKill();
		base.transform.localPosition = Vector3.right * posX[index];
		tfBodyControl.localEulerAngles = Vector3.up * 180f;
		count = 0;
	}

	private void AddAnimEvent()
	{
		AnimationEvent animationEvent = new AnimationEvent();
		animationEvent.functionName = "PlayAction";
		animationEvent.time = acIdle1.length;
		acIdle1.AddEvent(animationEvent);
	}

	public void PlayAction()
	{
		if (index == 0 || index == 3)
		{
			bRight = ((index == 0) ? true : false);
			if (index == 0)
			{
				tfBodyControl.DORotate(Vector3.up * 90f, 0.2f);
			}
			else
			{
				tfBodyControl.DORotate(Vector3.up * -90f, 0.2f);
			}
			anim.Play("Move");
			nextIndex = UnityEngine.Random.Range(1, 3);
			float duration = Mathf.Abs(posX[nextIndex] - posX[index]) / speed;
			base.transform.DOLocalMoveX(posX[nextIndex], duration).SetEase(Ease.Linear).OnComplete(delegate
			{
				index = nextIndex;
				tfBodyControl.DORotate(Vector3.up * 180f, 0.2f).OnComplete(delegate
				{
					anim.Play("Idle0");
				});
			});
		}
		else
		{
			if (index != 1 && index != 2)
			{
				return;
			}
			count++;
			if (count == 1)
			{
				if (strLastAction.Equals("Feiwen"))
				{
					anim.Play("Zhaoshou");
					strLastAction = "Zhaoshou";
				}
				else
				{
					anim.Play("Feiwen");
					strLastAction = "Feiwen";
				}
				return;
			}
			count = 0;
			if (bRight)
			{
				tfBodyControl.DORotate(Vector3.up * 90f, 0.2f);
				nextIndex = ((index != 1) ? 3 : UnityEngine.Random.Range(2, 4));
			}
			else
			{
				tfBodyControl.DORotate(Vector3.up * -90f, 0.2f);
				nextIndex = ((index != 1) ? UnityEngine.Random.Range(0, 2) : 0);
			}
			anim.Play("Move");
			float duration = Mathf.Abs(posX[nextIndex] - posX[index]) / speed;
			base.transform.DOLocalMoveX(posX[nextIndex], duration).SetEase(Ease.Linear).OnComplete(delegate
			{
				index = nextIndex;
				tfBodyControl.DORotate(Vector3.up * 180f, 0.2f).OnComplete(delegate
				{
					anim.Play("Idle0");
				});
			});
		}
	}
}
