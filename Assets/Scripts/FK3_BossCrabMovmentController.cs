using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using System;
using System.Collections;
using UnityEngine;

public class FK3_BossCrabMovmentController : MonoBehaviour
{
	public Action onFinish;

	[SerializeField]
	private AnimationCurve m_curve;

	private float m_startArg;

	[SerializeField]
	private float m_lifeTime = 20f;

	private int m_max = 1000;

	public static bool plot = true;

	private bool _plot;

	private float tempAddSpeed;

	private void Start()
	{
		_plot = plot;
		plot = false;
		if (!_plot)
		{
		}
	}

	private void OnEnable()
	{
		tempAddSpeed = UnityEngine.Random.Range(32f, 40f);
	}

	private void OnApplicationQuit()
	{
	}

	public void Play()
	{
		StartCoroutine(IE_Move_BGCurve());
	}

	public void SetStartArg(float arg)
	{
		m_startArg = arg;
	}

	private IEnumerator IE_Move_BGCurve()
	{
		UnityEngine.Debug.LogError("=================IE_Move_BGCurve===============");
		UnityEngine.Debug.LogError("m_startArg: " + m_startArg);
		float pathDuration = 300f;
		float t = m_startArg % pathDuration;
		FK3_FishBehaviour fish = base.transform.gameObject.GetComponent<FK3_FishBehaviour>();
		fish.transform.localScale = Vector3.one * 200f;
		FK3_CurveUsage curveUsage = FK3_BGPathMgr.Get().GetCurveUsageById(999);
		FK3_CursorUsage cursorUsage = curveUsage.GetCursorUsage(fish.id);
		cursorUsage.translate.ObjectToManipulate = base.transform;
		cursorUsage.rotate.ObjectToManipulate = base.transform;
		cursorUsage.rotate.OffsetAngle = new Vector3(270f, 90f, 0f);
		Action<FK3_FishBehaviour> removeMovementAction = delegate
		{
			cursorUsage.Free();
		};
		FK3_FishBehaviour fishBehaviour3 = fish;
		fishBehaviour3.RemoveMovementAction = (Action<FK3_FishBehaviour>)Delegate.Combine(fishBehaviour3.RemoveMovementAction, removeMovementAction);
		FK3_FishBehaviour fishBehaviour2 = fish;
		fishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, removeMovementAction);
		int count = 0;
		bool isFirst = true;
		float speed = 2f;
		int AddNum = 0;
		int AddSpeed = 1;
		float tempSpeedNum = tempAddSpeed;
		while (fish.IsLive())
		{
			t %= pathDuration;
			float tt = t / pathDuration;
			float ttt = GetPercent(tt);
			float tmpSpeed = (GetPercent(tt) - GetPercent(tt + 0.001f)) / 0.06f * 10f;
			speed = ((!(tmpSpeed <= 10f)) ? speed : tmpSpeed) * tempAddSpeed / 15f;
			count++;
			if (UnityEngine.Input.GetKeyDown(KeyCode.L))
			{
				count = 0;
				UnityEngine.Debug.LogError($"======霸王蟹>t {t},tt {tt},ttt {ttt},speed {speed}======");
			}
			if (ttt >= 0.998f)
			{
				AddNum++;
				tempAddSpeed *= UnityEngine.Random.Range(0.6f, 1.2f);
			}
			if (AddNum >= 3)
			{
				AddNum = 0;
				AddSpeed *= -1;
				tempAddSpeed = tempSpeedNum;
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
