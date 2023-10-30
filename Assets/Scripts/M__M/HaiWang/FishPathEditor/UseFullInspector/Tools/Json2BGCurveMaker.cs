using BansheeGz.BGSpline.Curve;
using FullInspector;
using JsonFx.Json;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.Tools
{
	[fiInspectorOnly]
	public class Json2BGCurveMaker : MonoBehaviour
	{
		public string jsonPath = "PathJsonDir/SWSPathPointTable";

		private void Start()
		{
		}

		[InspectorButton]
		private void LoadFromJson()
		{
			UnityEngine.Debug.Log("Path: " + jsonPath);
			bool raiseExceptions = Assert.raiseExceptions;
			Assert.raiseExceptions = true;
			TextAsset textAsset = Resources.Load<TextAsset>(jsonPath);
			UnityEngine.Debug.LogError("textAsset: " + textAsset.name + "  text: " + textAsset.text);
			PathPointTable pathPointTable = null;
			try
			{
				pathPointTable = JsonReader.Deserialize<PathPointTable>(textAsset.text);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			float z = 0f;
			Transform transform = base.transform;
			_ClearNode(transform);
			PathPointData[] data = pathPointTable.data;
			foreach (PathPointData pathPointData in data)
			{
				if (pathPointData.count > 9)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = pathPointData.name;
					gameObject.transform.SetParent(transform);
					BGCurve bGCurve = gameObject.AddComponent<BGCurve>();
					bGCurve.Mode2D = BGCurve.Mode2DEnum.XY;
					BGCurvePoint[] array = new BGCurvePoint[pathPointData.points.Length];
					int j = 0;
					for (int num = pathPointData.points.Length; j < num; j++)
					{
						float[] array2 = pathPointData.points[j];
						if (num == 2)
						{
							Vector3 position = new Vector3(array2[0], array2[1], z);
							array[j] = new BGCurvePoint(bGCurve, position);
							continue;
						}
						Vector3 position2 = new Vector3(array2[0], array2[1], z);
						float[] array3;
						if (j > 0)
						{
							array3 = pathPointData.points[j - 1];
						}
						else
						{
							float[] array4 = new float[2]
							{
								array2[0],
								0f
							};
							array3 = array4;
							array4[1] = array2[1];
						}
						float[] array5 = array3;
						Vector3 vector = new Vector3(array5[0], array5[1], z);
						float[] array6;
						if (j < num - 1)
						{
							array6 = pathPointData.points[j + 1];
						}
						else
						{
							float[] array7 = new float[2]
							{
								array2[0],
								0f
							};
							array6 = array7;
							array7[1] = array2[1];
						}
						float[] array8 = array6;
						Vector3 vector2 = new Vector3(array8[0], array8[1], z);
						Vector3 controlFirst = (j == 0) ? Vector3.zero : ((vector - vector2) * 1f / 6f);
						Vector3 controlSecond = (j == num - 1) ? Vector3.zero : ((vector2 - vector) * 1f / 6f);
						BGCurvePoint.ControlTypeEnum controlType = BGCurvePoint.ControlTypeEnum.BezierIndependant;
						array[j] = new BGCurvePoint(bGCurve, position2, controlType, controlFirst, controlSecond);
					}
					bGCurve.AddPoints(array);
				}
				Assert.raiseExceptions = raiseExceptions;
			}
		}

		[InspectorButton]
		private void ClearChilds()
		{
			ForeachChild(base.transform, delegate(Transform _)
			{
				UnityEngine.Object.DestroyImmediate(_.gameObject);
			});
		}

		[InspectorButton]
		private void EnableAllChilds()
		{
			ForeachChild(base.transform, delegate(Transform _)
			{
				_.gameObject.SetActive(value: true);
			});
		}

		[InspectorButton]
		private void DisableAllChilds()
		{
			ForeachChild(base.transform, delegate(Transform _)
			{
				_.gameObject.SetActive(value: false);
			});
		}

		[InspectorButton]
		private void RemoveAllStraightLineChilds()
		{
			ForeachChild(base.transform, delegate(Transform _)
			{
				BGCurve component = _.GetComponent<BGCurve>();
				if (component.PointsCount == 2)
				{
					UnityEngine.Object.DestroyImmediate(_.gameObject);
				}
			});
		}

		private void ForeachChild(Transform node, Action<Transform> iterationCallback)
		{
			Transform[] array = new Transform[node.childCount];
			int i = 0;
			for (int childCount = node.childCount; i < childCount; i++)
			{
				array[i] = node.GetChild(i);
			}
			Transform[] array2 = array;
			foreach (Transform obj in array2)
			{
				iterationCallback(obj);
			}
		}

		private void _ClearNode(Transform node)
		{
			Transform[] array = new Transform[node.childCount];
			int i = 0;
			for (int childCount = node.childCount; i < childCount; i++)
			{
				array[i] = node.GetChild(i);
			}
			Transform[] array2 = array;
			foreach (Transform transform in array2)
			{
				UnityEngine.Object.DestroyImmediate(transform.gameObject);
			}
		}
	}
}
