using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_Logo_Crystal : FK3_Effect_Logo
	{
		private Image m_renderer;

		private void Awake()
		{
			m_renderer = GetComponent<Image>();
			Reset_Logo();
		}

		private void ResetRender()
		{
			Vector4 v = m_renderer.color;
			v.w = 1f;
			m_renderer.color = v;
		}

		public override void Reset_Logo()
		{
			base.Reset_Logo();
			DOTween.Kill(base.transform);
			DOTween.Kill(m_renderer);
			base.transform.localScale = Vector3.zero;
			ResetRender();
		}

		public void Stop()
		{
			if (showTask != null)
			{
				showTask.Stop();
				showTask = null;
			}
			base.transform.localScale = new Vector3(2f, 2f, 1f);
			DOTween.Kill(base.transform);
			DOTween.Kill(m_renderer);
		}

		public override void Play(Vector3 pos, Vector3 targetPos, int seatId, int[] values)
		{
			pos.x = Mathf.Clamp(pos.x, -4f, 4f);
			pos.y = Mathf.Clamp(pos.y, -2f, 2f);
			Vector3 position = base.transform.position;
			float num = pos.z = position.z;
			base.gameObject.SetActive(value: true);
			base.transform.localEulerAngles = new Vector3(0f, 0f, (seatId <= 2) ? 0f : 180f);
			base.transform.position = pos;
			showTask = new FK3_Task(Show(seatId, targetPos));
		}

		private IEnumerator Show(int seatId, Vector3 targetPos)
		{
			base.transform.DOScale(new Vector3(2f, 2f, 1f), 0.3f);
			yield return new WaitForSeconds(1f);
			targetPos.y += ((seatId == 1 || seatId == 2) ? 0.4f : (-0.4f));
			base.transform.DOMove(targetPos, 2f);
		}

		public override void SetLayerOrder(int order)
		{
		}

		public override void DoFade()
		{
			m_renderer.DOFade(0f, 0.5f);
			overTask = new FK3_Task(ExecuteOver(0.5f));
		}
	}
}
