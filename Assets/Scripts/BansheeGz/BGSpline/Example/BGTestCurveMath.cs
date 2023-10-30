using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestCurveMath : MonoBehaviour
	{
		private abstract class CurveDataAbstract
		{
			private readonly List<GameObject> objectsToMove = new List<GameObject>();

			private readonly Material objectToMoveMaterial;

			private readonly LineRenderer lineRenderer;

			public readonly Material LineRendererMaterial;

			protected readonly GameObject GameObject;

			protected BGCurveBaseMath Math;

			private BGCurve curve;

			public BGCurve Curve
			{
				get
				{
					return curve;
				}
				protected set
				{
					curve = value;
					curve.Changed += delegate
					{
						UpdateLineRenderer();
					};
				}
			}

			protected CurveDataAbstract(GameObject gameObject, Material lineRendererMaterial, Color color)
			{
				GameObject = gameObject;
				LineRendererMaterial = lineRendererMaterial;
				objectToMoveMaterial = UnityEngine.Object.Instantiate(lineRendererMaterial);
				objectToMoveMaterial.SetColor("_TintColor", color);
				lineRenderer = gameObject.AddComponent<LineRenderer>();
				lineRenderer.material = lineRendererMaterial;
				LineRenderer obj = lineRenderer;
				float num = 0.05f;
				lineRenderer.endWidth = num;
				obj.startWidth = num;
				LineRenderer obj2 = lineRenderer;
				lineRenderer.endColor = color;
				obj2.startColor = color;
			}

			private void UpdateLineRenderer()
			{
				Vector3[] array = new Vector3[100];
				for (int i = 0; i < 100; i++)
				{
					float distanceRatio = (float)i / 99f;
					array[i] = Math.CalcByDistanceRatio(BGCurveBaseMath.Field.Position, distanceRatio);
				}
				lineRenderer.positionCount = 100;
				lineRenderer.SetPositions(array);
			}

			protected void AddObjects(int count, MeshRenderer pattern, Transform parent)
			{
				for (int i = 0; i < count; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(pattern.gameObject);
					gameObject.transform.parent = parent;
					AddObject(gameObject);
				}
			}

			protected void AddObject(GameObject obj)
			{
				obj.GetComponent<MeshRenderer>().sharedMaterial = objectToMoveMaterial;
				objectsToMove.Add(obj.gameObject);
			}

			protected void UpdateObjects(List<float> distanceRatios)
			{
				for (int i = 0; i < objectsToMove.Count; i++)
				{
					Vector3 vector = Math.CalcByDistanceRatio(BGCurveBaseMath.Field.Position, distanceRatios[i]);
					Vector3 b = Math.CalcByDistanceRatio(BGCurveBaseMath.Field.Tangent, distanceRatios[i]);
					objectsToMove[i].transform.position = vector;
					objectsToMove[i].transform.LookAt(vector + b);
				}
			}

			public abstract void Update();
		}

		private sealed class TestCurves : CurveDataAbstract
		{
			public readonly List<float> DistanceRatios = new List<float>();

			public readonly MeshRenderer ObjectToMove;

			private readonly List<CurveData> curves = new List<CurveData>();

			private float startTime = -6f;

			private Quaternion fromRotation;

			private Quaternion toRotation;

			private int currentCurveIndex = -1;

			public TestCurves(BGCurve curve, BGCurveBaseMath.Config config, MeshRenderer objectToMove, Material lineRendererMaterial)
				: base(curve.gameObject, lineRendererMaterial, Color.green)
			{
				base.Curve = curve;
				Math = new BGCurveBaseMath(curve, config);
				ObjectToMove = objectToMove;
				AddObject(objectToMove.gameObject);
				AddObjects(3, objectToMove, curve.transform);
				for (int i = 0; i < 4; i++)
				{
					DistanceRatios.Add((float)i * 0.25f);
				}
			}

			public void MoveRight()
			{
				currentCurveIndex++;
				if (currentCurveIndex == curves.Count)
				{
					currentCurveIndex = 0;
				}
			}

			public void MoveLeft()
			{
				currentCurveIndex--;
				if (currentCurveIndex < 0)
				{
					currentCurveIndex = curves.Count - 1;
				}
			}

			public override void Update()
			{
				if (Time.time - startTime > 3f)
				{
					startTime = Time.time;
					fromRotation = base.Curve.transform.rotation;
					toRotation = Quaternion.Euler(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
				}
				float t = (Time.time - startTime) / 3f;
				base.Curve.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t);
				for (int i = 0; i < DistanceRatios.Count; i++)
				{
					List<float> distanceRatios;
					int index;
					(distanceRatios = DistanceRatios)[index = i] = distanceRatios[index] + 0.3f * Time.deltaTime;
					if (DistanceRatios[i] > 1f)
					{
						DistanceRatios[i] = 0f;
					}
				}
				UpdateObjects(DistanceRatios);
				foreach (CurveData curf in curves)
				{
					curf.Update();
				}
			}

			public bool IsCurrent(CurveData curve)
			{
				return currentCurveIndex >= 0 && currentCurveIndex < curves.Count && curves[currentCurveIndex] == curve;
			}

			public void Add(CurveData curveData)
			{
				curves.Add(curveData);
			}

			public string CurrentToString()
			{
				return (currentCurveIndex >= 0) ? curves[currentCurveIndex].Description : "None";
			}
		}

		private sealed class CurveData : CurveDataAbstract
		{
			public enum MathTypeEnum
			{
				Base,
				Formula,
				Adaptive
			}

			private readonly Vector3 origin;

			private readonly TestCurves testCurves;

			private readonly Vector3 originalScale = new Vector3(0.7f, 0.7f, 0.7f);

			private readonly string description;

			public string Description => description;

			public CurveData(TestCurves testCurves, string name, string description, Vector3 position, BGCurveBaseMath.Config config, MathTypeEnum mathType)
				: base(new GameObject(name), testCurves.LineRendererMaterial, Color.magenta)
			{
				this.testCurves = testCurves;
				this.description = description;
				GameObject.transform.position = position;
				origin = position;
				base.Curve = GameObject.AddComponent<BGCurve>();
				base.Curve.Closed = testCurves.Curve.Closed;
				for (int i = 0; i < testCurves.Curve.PointsCount; i++)
				{
					BGCurvePointI bGCurvePointI = testCurves.Curve[i];
					BGCurvePoint point = new BGCurvePoint(base.Curve, bGCurvePointI.PositionLocal, bGCurvePointI.ControlType, bGCurvePointI.ControlFirstLocal, bGCurvePointI.ControlSecondLocal);
					base.Curve.AddPoint(point);
				}
				switch (mathType)
				{
				case MathTypeEnum.Base:
					Math = new BGCurveBaseMath(base.Curve, config);
					break;
				case MathTypeEnum.Formula:
					Math = new BGCurveFormulaMath(base.Curve, config);
					break;
				case MathTypeEnum.Adaptive:
					Math = new BGCurveAdaptiveMath(base.Curve, (BGCurveAdaptiveMath.ConfigAdaptive)config);
					break;
				default:
					throw new ArgumentOutOfRangeException("mathType", mathType, null);
				}
				AddObjects(4, testCurves.ObjectToMove, GameObject.transform);
				GameObject.transform.localScale = originalScale;
			}

			public override void Update()
			{
				Transform transform = base.Curve.gameObject.transform;
				Transform transform2 = testCurves.Curve.transform;
				transform.rotation = transform2.rotation;
				float num = 10f * Time.deltaTime;
				bool flag = testCurves.IsCurrent(this);
				transform.position = Vector3.MoveTowards(transform.position, (!flag) ? origin : transform2.position, num);
				transform.localScale = Vector3.MoveTowards(transform.localScale, (!flag) ? originalScale : transform2.transform.localScale, num / 4f);
				UpdateObjects(testCurves.DistanceRatios);
			}
		}

		[Tooltip("Material to use with LineRenderer")]
		public Material LineRendererMaterial;

		[Tooltip("Object to move along a curve")]
		public MeshRenderer ObjectToMove;

		private const float Period = 3f;

		private const int ObjectsCount = 4;

		private const float ObjectsSpeed = 0.3f;

		private TestCurves testCurves;

		private GUIStyle style;

		private void Start()
		{
			testCurves = new TestCurves(GetComponent<BGCurve>(), new BGCurveBaseMath.Config(BGCurveBaseMath.Fields.PositionAndTangent), ObjectToMove, LineRendererMaterial);
			testCurves.Add(new CurveData(testCurves, "BGBaseStraightLines", "Base, OptimizeStraightLines = true", base.transform.position + new Vector3(-4f, 1f), new BGCurveBaseMath.Config(BGCurveBaseMath.Fields.PositionAndTangent)
			{
				OptimizeStraightLines = true
			}, CurveData.MathTypeEnum.Base));
			testCurves.Add(new CurveData(testCurves, "BGBasePos2Tangents", "Base, UsePointPositionsToCalcTangents = true", base.transform.position + new Vector3(-4f, 4f), new BGCurveBaseMath.Config(BGCurveBaseMath.Fields.PositionAndTangent)
			{
				UsePointPositionsToCalcTangents = true
			}, CurveData.MathTypeEnum.Base));
			testCurves.Add(new CurveData(testCurves, "BGAdaptive", "Adaptive", base.transform.position + new Vector3(4f, 4f), new BGCurveAdaptiveMath.ConfigAdaptive(BGCurveBaseMath.Fields.PositionAndTangent), CurveData.MathTypeEnum.Adaptive));
			testCurves.Add(new CurveData(testCurves, "BGFormula", "Formula", base.transform.position + new Vector3(4f, 1f), new BGCurveBaseMath.Config(BGCurveBaseMath.Fields.PositionAndTangent), CurveData.MathTypeEnum.Formula));
		}

		private void Update()
		{
			testCurves.Update();
			if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				testCurves.MoveLeft();
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				testCurves.MoveRight();
			}
		}

		private void OnGUI()
		{
			if (style == null)
			{
				style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 18
				};
			}
			GUI.Label(new Rect(0f, 24f, 800f, 30f), "Left Arrow - move left, Right Arrow - move right", style);
			GUI.Label(new Rect(0f, 48f, 800f, 30f), "Comparing with: " + testCurves.CurrentToString(), style);
		}
	}
}
