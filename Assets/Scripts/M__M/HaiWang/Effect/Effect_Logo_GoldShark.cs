using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Effect
{
	public class Effect_Logo_GoldShark : Effect_Logo
	{
		[SerializeField]
		private List<Sprite> sprits;

		private Image m_renderer;

		private void Awake()
		{
			base.transform.localScale = Vector3.zero;
			m_renderer = GetComponent<Image>();
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
			m_renderer.sprite = sprits[0];
			ResetRender();
		}

		public override void Play(Vector3 pos, Vector3 posAdd, int seatId, int[] values)
		{
			Vector3 position = base.transform.position;
			float num = pos.z = position.z;
			base.gameObject.SetActive(value: true);
			base.transform.rotation = Quaternion.Euler(0f, 0f, (seatId != 1 && seatId != 2) ? 180 : 0);
			Vector3 vector = pos;
			pos.y = vector.y + ((seatId == 1 || seatId == 2) ? 1.4f : (-1.4f));
			base.transform.position = pos;
			showTask = new Task(Show());
		}

		private IEnumerator Show()
		{
			base.transform.DOScale(1.2f, 1f);
			yield return new WaitForSeconds(1f);
			Tweener tween = base.transform.DOScale(1f, 0.2f);
			tween.OnComplete(delegate
			{
				m_renderer.sprite = sprits[1];
			});
			yield return new WaitForSeconds(0.4f);
			m_renderer.sprite = sprits[2];
			yield return new WaitForSeconds(6.5f);
			DoFade();
		}

		public override void SetLayerOrder(int order)
		{
		}

		public override void DoFade()
		{
			m_renderer.DOFade(0f, 1f);
			overTask = new Task(ExecuteOver(0.5f));
		}
	}
}
