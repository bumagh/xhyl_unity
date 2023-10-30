using DragonBones;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FK3_CrocodileBeheviour : FK3_FishBehaviour
{
	public UnityArmatureComponent unityArmature;

	public Action onFinish;

	private int curveId;

	public int[] curveUsageId = new int[2]
	{
		444,
		333
	};

	private float m_startArg;

	private float ttt;

	[SerializeField]
	private AnimationCurve m_curve;

	private float tempAddSpeed;

	[SerializeField]
	private FK3_CollisionReceiver _collisionReceiver;

	public Image image;

	public Color color;

	private void OnEnable()
	{
		_collisionReceiver = GetComponent<FK3_CollisionReceiver>();
		_collisionReceiver.xCollider.enabled = true;
		tempAddSpeed = 10f;
		base.transform.localScale = Vector3.one;
		ttt = 0f;
	}

	public void SetPhtch(int id)
	{
		curveId = id;
		if (curveId >= curveUsageId.Length)
		{
			curveId = 0;
		}
	}

	public void Play()
	{
		StartCoroutine(IE_Move_BGCurve());
	}

	private IEnumerator IE_Move_BGCurve()
	{
		UnityEngine.Debug.LogError("=================IE_Move_BGCurve===============");
		float pathDuration = 300f;
		float t = m_startArg % pathDuration;
		FK3_FishBehaviour fish = base.transform.gameObject.GetComponent<FK3_FishBehaviour>();
		FK3_CurveUsage curveUsage = FK3_BGPathMgr.Get().GetCurveUsageById(curveUsageId[curveId]);
		FK3_CursorUsage cursorUsage = curveUsage.GetCursorUsage(fish.id);
		cursorUsage.translate.ObjectToManipulate = base.transform;
		cursorUsage.rotate.ObjectToManipulate = base.transform;
		Action<FK3_FishBehaviour> removeMovementAction = delegate
		{
			cursorUsage.Free();
		};
		FK3_FishBehaviour fishBehaviour = fish;
		fishBehaviour.RemoveMovementAction = (Action<FK3_FishBehaviour>)Delegate.Combine(fishBehaviour.RemoveMovementAction, removeMovementAction);
		FK3_FishBehaviour fishBehaviour2 = fish;
		fishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, removeMovementAction);
		bool isFirst = true;
		float speed = 2f;
		int AddSpeed = 1;
		PlayAnim("swim");
		bool one = false;
		bool two = false;
		bool thr = false;
		bool four = false;
		while (fish.IsLive() && ttt < 0.99f)
		{
			t %= pathDuration;
			float tt = t / pathDuration;
			ttt = GetPercent(tt);
			float tmpSpeed = (GetPercent(tt) - GetPercent(tt + 0.001f)) / 0.06f * 10f;
			speed = ((!(tmpSpeed <= 10f)) ? speed : tmpSpeed) * tempAddSpeed / 15f;
			if (ttt >= 0f && !one)
			{
				one = true;
				Attack((curveId != 0) ? 3 : 2);
			}
			else if (ttt >= 0.25f && !two)
			{
				two = true;
				Attack((curveId != 0) ? 2 : 3);
			}
			else if (ttt >= 0.45f && !thr)
			{
				thr = true;
				Attack((curveId == 0) ? 1 : 0);
			}
			else if (ttt >= 0.65f && !four)
			{
				four = true;
				Attack((curveId != 0) ? 2 : 0);
			}
			cursorUsage.cursor.DistanceRatio = ttt;
			if (isFirst)
			{
				isFirst = false;
				cursorUsage.translate.ForceUpdate();
			}
			fish.animator.speed = Mathf.Clamp(0.3f + Mathf.Abs(speed), 0f, 1.2f);
			yield return null;
			if (t <= 0f)
			{
				AddSpeed = 1;
			}
			t += Time.deltaTime * tempAddSpeed * (float)AddSpeed;
		}
		if (ttt >= 0.99f)
		{
			UnityEngine.Debug.LogError("死鱼");
			fish.Die();
		}
	}

	private FK3_GunBehaviour GetActiveGun(int id)
	{
		if (FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().m_guns[id].gameObject.activeInHierarchy)
		{
			return FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().m_guns[id];
		}
		return null;
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

	public void ShakeScreen(int num = 2)
	{
		FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(num);
	}

	public override void Update()
	{
		if ((bool)image && image.color != color)
		{
			image.color = color;
		}
	}

	private void PlayAnim(string name)
	{
		unityArmature.animation.Play(name);
	}

	public override void Prepare()
	{
		PlayAnim("swim");
	}

	private void Attack(int id = 0)
	{
		if ((bool)GetActiveGun(id) && (GetActiveGun(id).GetGunType() == FK3_GunType.FK3_GunGunn || GetActiveGun(id).GetGunType() == FK3_GunType.FK3_NormalGun) && UnityEngine.Random.Range(0, 4) <= 2)
		{
			StartCoroutine(WaitAttack(GetActiveGun(id)));
		}
	}

	private IEnumerator WaitAttack(FK3_GunBehaviour gun)
	{
		tempAddSpeed = 15f;
		yield return new WaitForSeconds(0.5f);
		tempAddSpeed = 5f;
		PlayAnim("attack");
		yield return new WaitForSeconds(0.8f);
		gun.ShakeScreen();
		FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(2);
		yield return new WaitForSeconds(0.5f);
		PlayAnim("swim");
		tempAddSpeed = 10f;
	}

	public override void Die()
	{
		base.Die();
	}

	public override void LeaveScene()
	{
		EnableCollider(value: false);
	}
}
