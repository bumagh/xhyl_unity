using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class Effect_Logo_CrystalFire : Effect_Logo
	{
		[SerializeField]
		private Animator animator;

		public override void Reset_Logo()
		{
			base.Reset_Logo();
			DOTween.Kill(base.transform);
		}

		public override void Play(Vector3 pos, Vector3 posAdd, int seatId, int[] values)
		{
			Vector3 position = base.transform.position;
			float num = pos.z = position.z;
			base.gameObject.SetActive(value: true);
			if (animator != null)
			{
				animator.ResetTrigger("show");
				animator.SetTrigger("show");
			}
			base.transform.rotation = Quaternion.Euler(0f, 0f, (seatId != 1 && seatId != 2) ? 180 : 0);
			base.transform.position = pos;
			showTask = new Task(Show(seatId));
		}

		private IEnumerator Show(int seatId)
		{
			yield return new WaitForSeconds(2f);
			Over();
		}
	}
}
