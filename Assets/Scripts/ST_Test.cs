using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

public class ST_Test : MonoBehaviour
{
	public Transform A;

	public Transform B;

	private Vector3[] points = new Vector3[4]
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(5f, 0f, 0f),
		new Vector3(5f, 2f, 0f),
		new Vector3(-5f, 1f, 0f)
	};

	private TweenerCore<Vector3, Path, PathOptions> _TC_Path;

	private float time;

	private bool bDown;

	public Animator anim;

	public AnimationClip walkAnim;

	public AnimationClip idleAnim;

	public bool crossfade;

	private Vector3[] vec = new Vector3[7]
	{
		new Vector3(1f, 0f, 0f),
		new Vector3(5f, 1f, 0f),
		new Vector3(4f, 4f, 0f),
		new Vector3(1f, 3.5f, 0f),
		new Vector3(-5f, 2.5f, 0f),
		new Vector3(-3.5f, -1.5f, 0f),
		new Vector3(-1f, -2f, 0f)
	};

	private void Dic1()
	{
		Vector3 vector = Camera.main.WorldToScreenPoint(A.transform.position);
		Vector3 vector2 = Camera.main.WorldToScreenPoint(B.transform.position);
		float num = Mathf.Sqrt((vector.x - vector2.x) * (vector.x - vector2.x) + (vector.y - vector2.y) * (vector.y - vector2.y));
		float num2 = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
		MonoBehaviour.print(num / num2);
	}

	private void Start()
	{
		Dic1();
	}

	private void Update()
	{
	}

	public void PlayEffect()
	{
	}

	public void TestClickSprite()
	{
		MonoBehaviour.print("Click");
	}

	public void OnMouseDown()
	{
		MonoBehaviour.print("MouseDown");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		MonoBehaviour.print(collision.name);
	}

	private IEnumerator ScaleAnim(Vector3 vecEnd, float duration)
	{
		while (duration >= 0f)
		{
			Vector3 vecStep = (vecEnd - base.transform.localScale) * Time.deltaTime / duration;
			duration -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
			base.transform.localScale += vecStep;
		}
	}

	public void Down()
	{
		bDown = true;
	}

	public void Up()
	{
		bDown = false;
		time = 0f;
	}

	public void TestButton()
	{
		MonoBehaviour.print("Down");
	}

	public void DoStart()
	{
		base.transform.DOPath(vec, 5f, PathType.CatmullRom).SetOptions(closePath: true).SetLookAt(0f)
			.SetEase(Ease.Linear)
			.SetLoops(-1, LoopType.Restart);
	}

	private void MyCallback(int i)
	{
		Vector3 toDirection = vec[i] - base.transform.position;
		toDirection.z = 0f;
		Quaternion rotation = Quaternion.FromToRotation(Vector3.right, toDirection);
		base.transform.rotation = rotation;
	}

	private void InitTweenPath()
	{
	}

	internal void PlayIdle()
	{
		if ((bool)idleAnim)
		{
			if (crossfade)
			{
				anim.CrossFade(idleAnim.name, 0.2f);
				MonoBehaviour.print("Death");
			}
			else
			{
				anim.Play(idleAnim.name);
			}
		}
	}

	internal void PlayWalk()
	{
		if ((bool)walkAnim)
		{
			if (crossfade)
			{
				anim.CrossFade(walkAnim.name, 0.2f);
				MonoBehaviour.print("Walk");
			}
			else
			{
				anim.Play(walkAnim.name);
			}
		}
	}

	public void OnOtherCollision(Collider2D other)
	{
		if (other.name == "Bullet")
		{
			MonoBehaviour.print(base.transform.name + "中弹");
		}
		else
		{
			MonoBehaviour.print(other.name);
		}
		other.gameObject.SetActive(value: false);
		anim.SetBool("bDeath", value: true);
	}

	public void StopHoMove()
	{
		_TC_Path.Pause();
	}

	public void ResumeHoMove()
	{
		_TC_Path.Play();
	}
}
