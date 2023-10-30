using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class Effect_Logo_LaserGun : Effect_Logo
	{
		[SerializeField]
		private TextMesh m_power;

		private Material m_powerMaterial;

		[SerializeField]
		private SpriteRenderer m_renderer;

		private void Awake()
		{
			m_powerMaterial = m_power.GetComponent<MeshRenderer>().material;
		}

		private void ResetRender()
		{
			Vector4 v = m_renderer.color;
			v.w = 1f;
			m_renderer.color = v;
			Vector4 v2 = m_powerMaterial.color;
			v2.w = 1f;
			m_powerMaterial.color = v2;
		}

		public override void Reset_Logo()
		{
			base.Reset_Logo();
			DOTween.Kill(base.transform);
			DOTween.Kill(m_renderer);
			DOTween.Kill(m_powerMaterial);
			ResetRender();
		}

		public override void Play(Vector3 pos, Vector3 posAdd, int seatId, int[] values)
		{
			Vector3 position = base.transform.position;
			float num = pos.z = position.z;
			m_power.text = string.Empty + values[0];
			if (m_power.text.Length == 2)
			{
				m_power.characterSize = 0.045f;
			}
			if (m_power.text.Length == 3)
			{
				m_power.characterSize = 0.035f;
			}
			if (m_power.text.Length == 4)
			{
				m_power.characterSize = 0.03f;
			}
			base.gameObject.SetActive(value: true);
			base.transform.rotation = Quaternion.Euler(0f, 0f, (seatId != 1 && seatId != 2) ? 180 : 0);
			Vector3 vector = pos;
			pos.y = vector.y + ((seatId == 1 || seatId == 2) ? 0.9f : (-0.9f));
			base.transform.position = pos;
			showTask = new Task(Show(seatId));
		}

		private IEnumerator Show(int seatId)
		{
			base.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.35f);
			yield break;
		}

		public override void SetLayerOrder(int order)
		{
			if (m_renderer != null)
			{
				m_renderer.sortingOrder = order;
			}
			if (m_power != null)
			{
				m_power.GetComponent<SortingLayerSetter>().OrderInLayer = order + 1;
			}
		}

		public override void DoFade()
		{
			m_renderer.DOFade(0f, 1f);
			m_powerMaterial.DOFade(0f, 1f);
		}
	}
}
