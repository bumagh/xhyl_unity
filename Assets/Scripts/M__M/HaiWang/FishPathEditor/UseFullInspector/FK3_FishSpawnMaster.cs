using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using M__M.HaiWang.Effect.Define;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_FishSpawnMaster : FK3_fiSimpleSingletonBehaviour<FK3_FishSpawnMaster>
	{
		public bool replaceProcessor = true;

		public bool restrainAutoPlayOnStart = true;

		[InspectorRange(0f, 5f, float.NaN)]
		public float fishSpeed = 2f;

		public bool printLog;

		public FK3_EffectConfig effectConfig;

		[NotSerialized]
		public Action<FK3_FishBehaviour> setupFishAction;

		protected override void Awake()
		{
			base.Awake();
			if (replaceProcessor)
			{
				FK3_FishSpawnerBehaviour.useStaticProcessor = true;
				FK3_FishSpawnerBehaviour.fishProcessAction = FishProcess;
			}
			if (restrainAutoPlayOnStart)
			{
				FK3_FishSpawnerBehaviour.playOnStartFunc = PlayOnStart_Delegation;
			}
		}

		private void Start()
		{
			CheckDependence();
			FK3_FishMgr.Get().Event_OnFishCreate_Handler = HookFishCreate;
		}

		private void FishProcess_ForTest(FK3_ProcessData<FK3_FishType> data)
		{
			if (printLog)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Orange("FK3_FishSpawnMaster.FishProcess> spawn[{0}] fish[type:{1},index:{2}]", data.info, data.value, data.index));
			}
		}

		private void FishProcess(FK3_ProcessData<FK3_FishType> data)
		{
			if (data == null)
			{
				return;
			}
			FK3_FishSpawnerBehaviour manager = data.spawner.GetManager<FK3_FishSpawnerBehaviour>();
			if (data.ignore)
			{
				manager.DoAfterProcess(data);
				return;
			}
			int num = data.spawner.startIndex + data.index;
			FK3_FishType value = data.value;
			if (printLog)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Orange("FK3_FishSpawnMaster.FishProcess> spawn[{0}], fish[type:{1}, id:{2}], index[main:{3}, sub:{4}]", data.info, data.value, num, data.spawner.startIndex, data.index));
			}
			FK3_FishMgr.Get().SetFishIndex(num++);
			FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().SpawnSingleFish(value);
			if (setupFishAction != null)
			{
				setupFishAction(fK3_FishBehaviour);
			}
			data.objBehaviour = fK3_FishBehaviour;
			data.obj = fK3_FishBehaviour.gameObject;
			if (data.onDespawn != null)
			{
				FK3_FishBehaviour fK3_FishBehaviour2 = fK3_FishBehaviour;
				fK3_FishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
				{
					data.onDespawn(data);
				});
			}
			SetupFishMovement_BGCurve(fK3_FishBehaviour, data);
			manager.DoAfterProcess(data);
		}

		private void SetupFishMovement_BGCurve(FK3_FishBehaviour fish, FK3_ProcessData<FK3_FishType> data)
		{
			FK3_FishSpawnerBehaviour manager = data.spawner.GetManager<FK3_FishSpawnerBehaviour>();
			FK3_CurveUsage curveUsage = manager.GetCurveUsage();
			FK3_CursorUsage cursorUsage = curveUsage.GetCursorUsage(fish.id);
			cursorUsage.translate.ObjectToManipulate = fish.transform;
			cursorUsage.rotate.ObjectToManipulate = fish.transform;
			cursorUsage.rotate.UpMode = BGCcCursorObjectRotate.RotationUpEnum.WorldCustom;
			cursorUsage.rotate.UpCustom = Vector3.forward;
			cursorUsage.rotate.OffsetAngle = new Vector3(90f, -90f, 0f);
			Setup_Delay(curveUsage, cursorUsage);
			cursorUsage.linear.PointReached += delegate(object _sender, BGCcCursorChangeLinear.PointReachedArgs _args)
			{
				OnPointReached(_sender, _args, fish, curveUsage, cursorUsage, data);
			};
			Action<FK3_FishBehaviour> b = delegate
			{
				cursorUsage.Free();
			};
			FK3_FishBehaviour fK3_FishBehaviour = fish;
			fK3_FishBehaviour.RemoveMovementAction = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.RemoveMovementAction, b);
			FK3_FishBehaviour fK3_FishBehaviour2 = fish;
			fK3_FishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDie_Handler, b);
			FK3_FishBehaviour fK3_FishBehaviour3 = fish;
			fK3_FishBehaviour3.Event_FishDying_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour3.Event_FishDying_Handler, b);
			cursorUsage.ListenOnReachEnd(delegate
			{
				fish.Die();
			});
			cursorUsage.translate.ForceUpdate();
			cursorUsage.rotate.ForceUpdate();
			cursorUsage.linear.FirePointReachedStartPoint();
		}

		private void Setup_Delay(FK3_CurveUsage curveUsage, FK3_CursorUsage cursorUsage)
		{
			cursorUsage.linear.CheckStartDelay = true;
			string name = "delay";
			if (curveUsage.curve.HasField(name))
			{
				int num = curveUsage.curve.IndexOfFieldValue(name);
				BGCurvePointField delayField = curveUsage.curve.Fields[num];
				cursorUsage.linear.DelayField = delayField;
			}
		}

		private void OnPointReached(object sender, BGCcCursorChangeLinear.PointReachedArgs args, FK3_FishBehaviour fish, FK3_CurveUsage curveUsage, FK3_CursorUsage cursorUsage, FK3_ProcessData<FK3_FishType> data)
		{
			int pointIndex = args.PointIndex;
			BGCcCursorChangeLinear bGCcCursorChangeLinear = sender as BGCcCursorChangeLinear;
			BGCurve curve = bGCcCursorChangeLinear.Curve;
			Process_DelaySpeed(curve, bGCcCursorChangeLinear, fish, pointIndex);
			Process_Rotate(curve, bGCcCursorChangeLinear, cursorUsage.rotate, fish, pointIndex);
		}

		private void Process_DelaySpeed(BGCurve curve, BGCcCursorChangeLinear linear, FK3_FishBehaviour fish, int index)
		{
			if (!curve.HasField("delaySpeed") || !curve.HasField("delaySpeedValue"))
			{
				return;
			}
			BGCurvePointI bGCurvePointI = curve[index];
			if (bGCurvePointI.GetField<bool>("delaySpeed"))
			{
				Vector3 field = curve[index].GetField<Vector3>("delaySpeedValue");
				float x = field.x;
				float y = field.y;
				float z = field.z;
				linear.Speed = y;
				if (fish != null && fish.IsLive())
				{
					fish.StartCoroutine(IE_DelaySpeedControl(x, z, linear, fish));
				}
			}
		}

		private void Process_Rotate(BGCurve curve, BGCcCursorChangeLinear linear, BGCcCursorObjectRotate rotate, FK3_FishBehaviour fish, int index)
		{
			if (curve.HasField("rotate") && !(rotate == null))
			{
				BGCurvePointI bGCurvePointI = curve[index];
				float field = bGCurvePointI.GetField<float>("rotate");
				Vector3 b = Vector3.up * field;
				Vector3 vector = rotate.OffsetAngle + b;
				if (Mathf.Abs(field) > 0.01f)
				{
					Quaternion rhs = Quaternion.Inverse(Quaternion.Euler(rotate.OffsetAngle));
					rotate.ObjectToManipulate.rotation = rotate.ObjectToManipulate.rotation * rhs * Quaternion.Euler(vector);
				}
				rotate.OffsetAngle = vector;
			}
		}

		private IEnumerator IE_DelaySpeedControl(float delay, float speed, BGCcCursorChangeLinear linear, FK3_FishBehaviour fish)
		{
			UnityEngine.Debug.Log($"{fish.identity} execute DelaySpeedControl. delay:{delay}, speed:{speed}");
			yield return new WaitForSeconds(delay);
			if (fish != null && fish.IsLive())
			{
				linear.Speed = speed;
			}
		}

		private bool PlayOnStart_Delegation(FK3_FishSpawnerBehaviour spawner, bool value)
		{
			return false;
		}

		private void InspectorBtn_ReplaceProcess_Test()
		{
			FK3_FishSpawnerBehaviour.fishProcessAction = FishProcess_ForTest;
			FK3_FishSpawnerBehaviour.useStaticProcessor = true;
			UnityEngine.Debug.Log(FK3_LogHelper.Aqua("FK3_FishSpawnerBehaviour.fishProcessAction is replaced by FishProcess_ForTest"));
		}

		private void InspectorBtn_ReplaceProcess_Formal()
		{
			FK3_FishSpawnerBehaviour.fishProcessAction = FishProcess;
			FK3_FishSpawnerBehaviour.useStaticProcessor = true;
			UnityEngine.Debug.Log(FK3_LogHelper.Aqua("FK3_FishSpawnerBehaviour.fishProcessAction is replaced by FishProcess"));
		}

		public bool CheckDependence(bool throwException = false)
		{
			if (!(FK3_FishMgr.Get() == null))
			{
				return true;
			}
			if (throwException)
			{
				throw new Exception("FishMgr not ready");
			}
			return false;
		}

		private void HookFishCreate(FK3_FishBehaviour fish)
		{
			if (fish.isLightning)
			{
				GameObject gmObj = UnityEngine.Object.Instantiate(effectConfig.LightingYellowBall, fish.transform);
				gmObj.transform.SetParent(fish.transform);
				gmObj.transform.localPosition = Vector3.zero;
				Vector3 position = fish.GetPosition();
				gmObj.transform.SetPosition(position);
				float num = Mathf.Clamp(fish.radius * 2.5f, 0.6f, 9f);
				gmObj.transform.localScale = num * 0.7f * Vector3.one;
				gmObj.name = "LightingYellowBall";
				fish.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fish.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
				{
					UnityEngine.Object.DestroyImmediate(gmObj);
				});
			}
		}
	}
}
