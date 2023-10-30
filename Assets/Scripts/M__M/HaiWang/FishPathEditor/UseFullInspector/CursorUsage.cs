using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using M__M.HaiWang.Message;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class CursorUsage
	{
		public string id;

		internal BGCcCursor cursor;

		internal BGCcCursorChangeLinear linear;

		internal BGCcCursorObjectTranslate translate;

		internal BGCcCursorObjectRotate rotate;

		internal Action<CursorUsage> FreeAction;

		internal int pointCount;

		internal bool ownObj = true;

		internal GameObject gameObj;

		public bool active;

		public static Action<UnityEngine.Object> destroyAction;

		private BGCcMath bGCcMath;

		private BGCurve bGCurve;

		public void Free()
		{
			if (FreeAction != null)
			{
				FreeAction(this);
			}
			if (ownObj)
			{
				DestroyOne(gameObj);
			}
			else
			{
				if (translate != null)
				{
					DestroyOne(translate);
				}
				if (linear != null)
				{
					DestroyOne(linear);
				}
				if (rotate != null)
				{
					DestroyOne(rotate);
				}
				if (cursor != null)
				{
					DestroyOne(cursor);
				}
			}
			translate = null;
			linear = null;
			cursor = null;
			rotate = null;
			active = false;
		}

		private void DestroyOne(UnityEngine.Object obj)
		{
			if (destroyAction == null)
			{
				UnityEngine.Object.Destroy(obj);
			}
			else if (!(obj == null))
			{
				if (obj is MonoBehaviour)
				{
					((MonoBehaviour)obj).enabled = false;
				}
				else if (obj is GameObject)
				{
					((GameObject)obj).SetActive(value: false);
				}
				destroyAction(obj);
			}
		}

		public void ListenOnReachEnd(Action OnFinish)
		{
			linear.PointReached += delegate(object _sender, BGCcCursorChangeLinear.PointReachedArgs _args)
			{
				BGCcCursorChangeLinear bGCcCursorChangeLinear = _sender as BGCcCursorChangeLinear;
				if (_args.PointIndex == pointCount - 1)
				{
					OnFinish();
				}
			};
		}

		private void Linear_PointReached(object sender, BGCcCursorChangeLinear.PointReachedArgs e)
		{
			throw new NotImplementedException();
		}

		private GameObject SpawnBullet()
		{
			Transform prefab = BGPathMgr.Get().PathPool.prefabs["1"];
			Transform transform = BGPathMgr.Get().PathPool.Spawn(prefab, BGPathMgr.Get().PathPool.transform);
			return transform.gameObject;
		}

		public void DespawnBullet(GameObject _bullet)
		{
			if (!(_bullet == null))
			{
				BGPathMgr.Get().PathPool.Despawn(_bullet.transform);
			}
		}

		internal void Prepare(BGCurve curve, string parentId, string subId, float speed, float startDelay, bool createObj = true)
		{
			id = subId;
			if (createObj)
			{
				try
				{
					ownObj = createObj;
					gameObj = new GameObject();
					gameObj.name = $"路径[{subId}]";
					gameObj.transform.SetParent(curve.transform);
					cursor = gameObj.AddComponent<BGCcCursor>();
					linear = gameObj.AddComponent<BGCcCursorChangeLinear>();
					translate = gameObj.AddComponent<BGCcCursorObjectTranslate>();
					rotate = gameObj.AddComponent<BGCcCursorObjectRotate>();
					bGCcMath = gameObj.GetComponent<BGCcMath>();
					bGCcMath.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
					bGCurve = gameObj.GetComponent<BGCurve>();
					if (bGCurve == null)
					{
						bGCurve = gameObj.AddComponent<BGCurve>();
					}
					bGCurve.AddPoints(curve.bgPoints);
					cursor.curveContainer = curve.gameObject;
					linear.curveContainer = curve.gameObject;
					translate.curveContainer = curve.gameObject;
					rotate.curveContainer = curve.gameObject;
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("错误: " + arg);
				}
			}
			else
			{
				cursor = curve.gameObject.AddComponent<BGCcCursor>();
				cursor.AddedInEditor();
				linear = curve.gameObject.AddComponent<BGCcCursorChangeLinear>();
				translate = curve.gameObject.AddComponent<BGCcCursorObjectTranslate>();
				rotate = curve.gameObject.AddComponent<BGCcCursorObjectRotate>();
			}
			try
			{
				cursor.CcName = $"路径[{subId}]";
				linear.Cursor = cursor;
				translate.Cursor = cursor;
				rotate.Cursor = cursor;
				linear.OverflowControl = BGCcCursorChangeLinear.OverflowControlEnum.Stop;
				linear.Speed = speed;
				linear.CoerceStartDelay = startDelay;
				pointCount = cursor.Curve.PointsCount;
				rotate.UpMode = BGCcCursorObjectRotate.RotationUpEnum.WorldCustom;
				rotate.UpCustom = Vector3.forward;
				rotate.OffsetAngle = new Vector3(90f, -90f, 0f);
				cursor.FireChangedParams();
				active = true;
			}
			catch (Exception arg2)
			{
				UnityEngine.Debug.LogError("错误: " + arg2);
			}
		}

		private void SetCursorUsageLinearSpeed(KeyValueInfo keyValueInfo)
		{
			linear.Speed = 4f;
		}
	}
}
