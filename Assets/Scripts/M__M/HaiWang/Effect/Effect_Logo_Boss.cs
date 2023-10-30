using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Effect
{
	public class Effect_Logo_Boss : Effect_Logo
	{
		private Image m_renderer;

		private void ResetRender()
		{
			Vector4 v = m_renderer.color;
			v.w = 0f;
			m_renderer.color = v;
		}

		private void Awake()
		{
			m_renderer = GetComponent<Image>();
			ResetRender();
		}

		public override void Reset_Logo()
		{
			base.Reset_Logo();
			DOTween.Kill(m_renderer);
			ResetRender();
		}

		public override void Play(Vector3 pos, Vector3 posAdd, int seatId, int[] values)
		{
			Vector3 position = base.transform.position;
			float num = pos.z = position.z;
			base.gameObject.SetActive(value: true);
			base.transform.localEulerAngles = new Vector3(0f, 0f, (seatId <= 2) ? 0f : 180f);
			base.transform.position = pos;
			showTask = new Task(Show(seatId));
		}

		private IEnumerator Show(int seatId)
		{
			m_renderer.DOFade(1f, 2f);
			yield break;
		}

		public override void SetLayerOrder(int order)
		{
		}

		public override void DoFade()
		{
			m_renderer.DOFade(0f, 0.5f);
			overTask = new Task(ExecuteOver(0.5f));
		}
	}
}
