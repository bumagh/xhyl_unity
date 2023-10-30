using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using UnityEngine;

public class FK3_CaymanMovController : MonoBehaviour
{
	public Action onFinish;

	[SerializeField]
	private AnimationCurve m_curve;

	private float tempAddSpeed;

	private float m_startArg;

	public int[] curveUsageId = new int[4]
	{
		888,
		777,
		666,
		555
	};

	[HideInInspector]
	public Animator animator;

	private BoxCollider collider;

	private bool isBrith = true;

	private int curveId;

	private float ttt;

	private void InitGame()
	{
		if (animator == null)
		{
			animator = base.transform.GetChild(0).GetComponent<Animator>();
		}
		if (collider == null)
		{
			collider = base.transform.GetComponent<BoxCollider>();
		}
	}

	private void Update()
	{
		if (!collider)
		{
			return;
		}
		if (isBrith)
		{
			if (collider.enabled)
			{
				collider.enabled = false;
			}
		}
		else if (!collider.enabled)
		{
			collider.enabled = true;
		}
	}

	private void OnEnable()
	{
		InitGame();
		tempAddSpeed = 35f;
		ttt = 0f;
		isBrith = true;
		StartCoroutine(WaitDie());
	}

	private IEnumerator WaitDie()
	{
		yield return new WaitForSeconds(25f);
		if (base.gameObject.activeInHierarchy)
		{
			FK3_FishBehaviour component = base.transform.gameObject.GetComponent<FK3_FishBehaviour>();
			if ((bool)component)
			{
				component.Die();
			}
		}
	}

	public void Play()
	{
		StartCoroutine(IE_Move_BGCurve());
	}

	public void SetPhtch(int id)
	{
		curveId = id;
		if (curveId >= curveUsageId.Length)
		{
			curveId = 1;
		}
	}

	private IEnumerator IE_Move_BGCurve()
	{
		UnityEngine.Debug.LogError("=================IE_Move_BGCurve===============");
		float pathDuration = 300f;
		float t2 = m_startArg % pathDuration;
		FK3_FishBehaviour fish = base.transform.gameObject.GetComponent<FK3_FishBehaviour>();
		fish.transform.localScale = Vector3.one * 200f;
		FK3_CurveUsage curveUsage = FK3_BGPathMgr.Get().GetCurveUsageById(curveUsageId[curveId]);
		FK3_CursorUsage cursorUsage = curveUsage.GetCursorUsage(fish.id);
		cursorUsage.translate.ObjectToManipulate = base.transform;
		cursorUsage.rotate.ObjectToManipulate = base.transform;
		Action<FK3_FishBehaviour> removeMovementAction = delegate
		{
			cursorUsage.Free();
		};
		FK3_FishBehaviour fishBehaviour3 = fish;
		fishBehaviour3.RemoveMovementAction = (Action<FK3_FishBehaviour>)Delegate.Combine(fishBehaviour3.RemoveMovementAction, removeMovementAction);
		FK3_FishBehaviour fishBehaviour2 = fish;
		fishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, removeMovementAction);
		bool isFirst = true;
		float speed = 2f;
		int AddSpeed = 1;
		PlayAnim("Life");
		bool one = false;
		bool two = false;
		bool thr = false;
		bool four = false;
		while (fish.IsLive() && ttt < 0.99f)
		{
			t2 %= pathDuration;
			float tt = t2 / pathDuration;
			ttt = GetPercent(tt);
			float tmpSpeed = (GetPercent(tt) - GetPercent(tt + 0.001f)) / 0.06f * 10f;
			speed = ((!(tmpSpeed <= 10f)) ? speed : tmpSpeed) * tempAddSpeed / 15f;
			if (ttt >= 0.26f && !one)
			{
				one = true;
				ShakeScreen();
			}
			else if (ttt >= 0.4f && !two)
			{
				two = true;
				ShakeScreen();
			}
			else if (ttt >= 0.56f && !thr)
			{
				thr = true;
				ShakeScreen();
			}
			else if (ttt >= 0.72f && !four)
			{
				four = true;
				ShakeScreen();
				tempAddSpeed = 0f;
				PlayAnim("BrithOver");
				UnityEngine.Debug.LogError(ttt);
				yield return new WaitForSeconds(1.5f);
				PlayAnim("Move");
				tempAddSpeed = 1.5f;
				isBrith = false;
			}
			cursorUsage.cursor.DistanceRatio = ttt;
			if (isFirst)
			{
				isFirst = false;
				cursorUsage.translate.ForceUpdate();
			}
			fish.animator.speed = Mathf.Clamp(0.3f + Mathf.Abs(speed), 0f, 1.2f);
			yield return null;
			if (t2 <= 0f)
			{
				AddSpeed = 1;
			}
			t2 += Time.deltaTime * tempAddSpeed * (float)AddSpeed;
		}
		if (ttt >= 0.99f)
		{
			UnityEngine.Debug.LogError("死鱼");
			fish.Die();
		}
	}

	public void ShakeScreen(int num = 2)
	{
		FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(num);
		if (num < 2)
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("爆炸");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("爆炸", 1f);
		}
	}

	private void PlayAnim(string name)
	{
		try
		{
			InitGame();
			animator.SetTrigger(name);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
	}

	private float GetPercent(float t)
	{
		float num = 0.5f;
		float num2 = m_curve.Evaluate(t);
		num2 /= num;
		num2 %= 1f;
		if (num2 < 0f)
		{
			num2 += 1f;
		}
		return num2;
	}
}
