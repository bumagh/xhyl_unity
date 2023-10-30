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
	public class FishSpawnMaster : fiSimpleSingletonBehaviour<FishSpawnMaster>
	{
		public bool replaceProcessor = true;

		public bool restrainAutoPlayOnStart = true;

		[InspectorRange(0f, 5f, float.NaN)]
		public float fishSpeed = 2f;

		public bool printLog;

		public EffectConfig effectConfig;

		[NotSerialized]
		public Action<FishBehaviour> setupFishAction;

		protected override void Awake()
		{
			base.Awake();
			if (replaceProcessor)
			{
				FishSpawnerBehaviour.useStaticProcessor = true;
				FishSpawnerBehaviour.fishProcessAction = FishProcess;
				UnityEngine.Debug.Log(HW2_LogHelper.Aqua("FishSpawnerBehaviour.fishProcessAction is replaced by FishProcess"));
			}
			if (restrainAutoPlayOnStart)
			{
				FishSpawnerBehaviour.playOnStartFunc = PlayOnStart_Delegation;
				UnityEngine.Debug.Log(HW2_LogHelper.Aqua("FishSpawnerBehaviour.autoPlayOnStart is restrained"));
			}
		}

		private void Start()
		{
			CheckDependence();
			FishMgr.Get().Event_OnFishCreate_Handler = HookFishCreate;
		}

		private void FishProcess_ForTest(ProcessData<FishType> data)
		{
			if (printLog)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Orange("FishSpawnMaster.FishProcess> spawn[{0}] fish[type:{1},index:{2}]", data.info, data.value, data.index));
			}
		}

		private void FishProcess(ProcessData<FishType> data)
		{
			if (data == null)
			{
				return;
			}
			FishSpawnerBehaviour manager = data.spawner.GetManager<FishSpawnerBehaviour>();
			if (data.ignore)
			{
				manager.DoAfterProcess(data);
				return;
			}
			int num = data.spawner.startIndex + data.index;
			FishType value = data.value;
			if (printLog)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Orange("FishSpawnMaster.FishProcess> spawn[{0}], fish[type:{1}, id:{2}], index[main:{3}, sub:{4}]", data.info, data.value, num, data.spawner.startIndex, data.index));
			}
			FishMgr.Get().SetFishIndex(num++);
			FishBehaviour fishBehaviour = FishMgr.Get().SpawnSingleFish(value);
			if (setupFishAction != null)
			{
				setupFishAction(fishBehaviour);
			}
			data.objBehaviour = fishBehaviour;
			data.obj = fishBehaviour.gameObject;
			if (data.onDespawn != null)
			{
				FishBehaviour fishBehaviour2 = fishBehaviour;
				fishBehaviour2.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
				{
					data.onDespawn(data);
				});
			}
			SetupFishMovement_BGCurve(fishBehaviour, data);
			manager.DoAfterProcess(data);
		}

		private void SetupFishMovement_BGCurve(FishBehaviour fish, ProcessData<FishType> data)
		{
			FishSpawnerBehaviour manager = data.spawner.GetManager<FishSpawnerBehaviour>();
			CurveUsage curveUsage = manager.GetCurveUsage();
			CursorUsage cursorUsage = curveUsage.GetCursorUsage(fish.id);
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
			Action<FishBehaviour> b = delegate
			{
				cursorUsage.Free();
			};
			FishBehaviour fishBehaviour = fish;
			fishBehaviour.RemoveMovementAction = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.RemoveMovementAction, b);
			FishBehaviour fishBehaviour2 = fish;
			fishBehaviour2.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, b);
			FishBehaviour fishBehaviour3 = fish;
			fishBehaviour3.Event_FishDying_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour3.Event_FishDying_Handler, b);
			cursorUsage.ListenOnReachEnd(delegate
			{
				fish.Die();
			});
			cursorUsage.translate.ForceUpdate();
			cursorUsage.rotate.ForceUpdate();
			cursorUsage.linear.FirePointReachedStartPoint();
		}

		private void Setup_Delay(CurveUsage curveUsage, CursorUsage cursorUsage)
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

		private void OnPointReached(object sender, BGCcCursorChangeLinear.PointReachedArgs args, FishBehaviour fish, CurveUsage curveUsage, CursorUsage cursorUsage, ProcessData<FishType> data)
		{
			int pointIndex = args.PointIndex;
			BGCcCursorChangeLinear bGCcCursorChangeLinear = sender as BGCcCursorChangeLinear;
			BGCurve curve = bGCcCursorChangeLinear.Curve;
			Process_DelaySpeed(curve, bGCcCursorChangeLinear, fish, pointIndex);
			Process_Rotate(curve, bGCcCursorChangeLinear, cursorUsage.rotate, fish, pointIndex);
		}

		private void Process_DelaySpeed(BGCurve curve, BGCcCursorChangeLinear linear, FishBehaviour fish, int index)
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

		private void Process_Rotate(BGCurve curve, BGCcCursorChangeLinear linear, BGCcCursorObjectRotate rotate, FishBehaviour fish, int index)
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

		private IEnumerator IE_DelaySpeedControl(float delay, float speed, BGCcCursorChangeLinear linear, FishBehaviour fish)
		{
			UnityEngine.Debug.Log($"{fish.identity} execute DelaySpeedControl. delay:{delay}, speed:{speed}");
			yield return new WaitForSeconds(delay);
			if (fish != null && fish.IsLive())
			{
				linear.Speed = speed;
			}
		}

		private bool PlayOnStart_Delegation(FishSpawnerBehaviour spawner, bool value)
		{
			return false;
		}

		private void InspectorBtn_ReplaceProcess_Test()
		{
			FishSpawnerBehaviour.fishProcessAction = FishProcess_ForTest;
			FishSpawnerBehaviour.useStaticProcessor = true;
			UnityEngine.Debug.Log(HW2_LogHelper.Aqua("FishSpawnerBehaviour.fishProcessAction is replaced by FishProcess_ForTest"));
		}

		private void InspectorBtn_ReplaceProcess_Formal()
		{
			FishSpawnerBehaviour.fishProcessAction = FishProcess;
			FishSpawnerBehaviour.useStaticProcessor = true;
			UnityEngine.Debug.Log(HW2_LogHelper.Aqua("FishSpawnerBehaviour.fishProcessAction is replaced by FishProcess"));
		}

		public bool CheckDependence(bool throwException = false)
		{
			if (!(FishMgr.Get() == null))
			{
				return true;
			}
			if (throwException)
			{
				throw new Exception("FishMgr not ready");
			}
			return false;
		}

		private void HookFishCreate(FishBehaviour fish)
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
				fish.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fish.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
				{
					UnityEngine.Object.DestroyImmediate(gmObj);
				});
			}
		}
	}
}
