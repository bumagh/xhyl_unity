using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_Logo_BossCrabShadow : FK3_Effect_Logo
	{
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
			base.transform.position = pos;
			showTask = new FK3_Task(Show(pos));
		}

		private IEnumerator Show(Vector3 pos)
		{
			Vector3 vector = pos;
			pos.x = 0f - vector.x;
			Tweener t = base.transform.DOMove(pos, 9f);
			t.OnComplete(delegate
			{
				Over();
			});
			yield break;
		}
	}
}
