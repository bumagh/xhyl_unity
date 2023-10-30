using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using HW3L;
using Rapid.Tools;
using SWS;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Tests.DOTweenPathUsage
{
	public class FK3_DOTweenPathUsage_Main : MonoBehaviour
	{
		[SerializeField]
		private Transform m_obj1;

		[SerializeField]
		private Transform m_obj2;

		private Tween m_tween1;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_percent1;

		[SerializeField]
		private AnimationCurve m_curve1;

		[SerializeField]
		private float mod = 0.5f;

		private string m_test = string.Empty;

		private bool m_plotGraph;

		private void Start()
		{
			Graph.Initialize();
			Test_2();
		}

		private void OnApplicationQuit()
		{
			Graph.Dispose();
		}

		private void Test_1()
		{
			m_test = "Test_1";
			PathManager pathManager = WaypointManager.Paths["1"];
			splineMove component = m_obj1.gameObject.GetComponent<splineMove>();
			component = m_obj1.gameObject.AddComponent<splineMove>();
			component.SetPath(pathManager);
			component.StartMove();
			UnityEngine.Debug.Log(pathManager);
		}

		private void Test_2()
		{
			m_test = "Test_2";
			PathManager pathManager = WaypointManager.Paths["1"];
			float lookAhead = 0f;
			bool closePath = false;
			PathType pathType = PathType.CatmullRom;
			PathMode pathMode = PathMode.Full3D;
			Ease ease = Ease.Linear;
			AxisConstraint lockPosition = AxisConstraint.None;
			AxisConstraint lockRotation = AxisConstraint.None;
			float duration = 2f;
			TweenParams tweenParams = new TweenParams();
			tweenParams.SetSpeedBased();
			tweenParams.SetEase(ease);
			Vector3[] pathPoints = pathManager.GetPathPoints();
			UnityEngine.Debug.Log(pathPoints.JoinStrings());
			m_obj1.transform.position = pathPoints[0];
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = m_obj1.transform.DOPath(pathPoints, duration, pathType, pathMode).SetAs(tweenParams).SetOptions(closePath, lockPosition, lockRotation)
				.SetLookAt(lookAhead);
			tweenerCore.GotoWaypoint(0, andPlay: true);
			tweenerCore.Pause();
			m_tween1 = tweenerCore;
			StartCoroutine(IE_PlotCurve());
		}

		private IEnumerator IE_PlotCurve()
		{
			float duration = 50f;
			float t = 0f;
			while (t < duration)
			{
				t += Time.deltaTime;
				m_percent1 = t / duration;
				yield return 0;
			}
		}

		private void Update()
		{
			if (m_test.Equals("Test_2") && m_obj1 != null && m_tween1 != null && m_curve1 != null)
			{
				float num = m_curve1.Evaluate(m_percent1);
				num /= mod;
				num %= 1f;
				if (num < 0f)
				{
					num += 1f;
				}
				m_obj1.position = m_tween1.PathGetPoint(num);
				Graph.Log("plot", num);
			}
		}

		private void PlotGraph()
		{
			if (m_test.Equals("Test_2") && m_obj1 != null && m_tween1 != null && m_curve1 != null)
			{
				int num = 2000;
				for (int i = 0; i < num; i++)
				{
					float num2 = (float)i / 2000f;
					num2 *= 100f;
					FK3_GraphLogger.AddPoint(num2);
				}
			}
		}

		private void OnGUI()
		{
			bool plotGraph = m_plotGraph;
			m_plotGraph = GUILayout.Toggle(m_plotGraph, "Plot Graph");
			if (m_plotGraph != plotGraph && m_plotGraph)
			{
				PlotGraph();
			}
		}
	}
}
