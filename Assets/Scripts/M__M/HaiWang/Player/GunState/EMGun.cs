using M__M.HaiWang.Player.Gun;
using UnityEngine;

namespace M__M.HaiWang.Player.GunState
{
	public class EMGun : GunState<GunController>
	{
		public enum EMGunPhase
		{
			Phase1,
			Phase2,
			Phase3,
			Phase4
		}

		public EMGunPhase phase;

		private float _fStoreEngergyInterval = 3f;

		private float _fShootInterval = 10f;

		private float _fTimeToEnterPhase3 = -1f;

		private float _fTimeToEnterPhase4 = -1f;

		public override void OnEnter(ArgList args)
		{
			UnityEngine.Debug.Log("电磁炮一阶段，炮管飞入炮台并屏蔽操作");
			phase = EMGunPhase.Phase1;
			Vector3 at = args.GetAt<Vector3>(0);
			Owner.TurnToEMGun(at, delegate
			{
				phase = EMGunPhase.Phase2;
				UnityEngine.Debug.Log("电磁炮二阶段，充能阶段，可以调整发射角度");
				Owner.TurnToEMGunPhase2();
				_fTimeToEnterPhase3 = Time.time + _fStoreEngergyInterval;
			});
		}

		public override void OnExit()
		{
			Owner.TurnFromEmGun();
			UnityEngine.Debug.Log("电磁炮结束");
		}

		public override void OnUpdate()
		{
			if (phase == EMGunPhase.Phase2 && _fTimeToEnterPhase3 > 0f && Time.time > _fTimeToEnterPhase3)
			{
				UnityEngine.Debug.Log("电磁炮三阶段，倒计时阶段，可以发射");
				phase = EMGunPhase.Phase3;
				Owner.EMGunPhase3Update();
				_fTimeToEnterPhase4 = Time.time + _fShootInterval;
			}
			else if (phase == EMGunPhase.Phase3 && _fTimeToEnterPhase4 > 0f && Time.time > _fTimeToEnterPhase4)
			{
				Shoot();
			}
			else if (phase == EMGunPhase.Phase4)
			{
				Owner.EMGunPhase4Update();
			}
		}

		public override void RotateByInput()
		{
			Owner.EMGunRotateByInput();
		}

		public override void Shoot()
		{
			if (phase == EMGunPhase.Phase3)
			{
				UnityEngine.Debug.Log("电磁炮四阶段，发射中。。。");
				Owner.EMGunShoot();
				phase = EMGunPhase.Phase4;
			}
		}
	}
}
