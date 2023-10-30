using DG.Tweening;
using M__M.HaiWang.Player.Gun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_Logo_BombCrab : FK3_Effect_Logo
	{
		[SerializeField]
		private Text m_power;

		private Material m_powerMaterial;

		[SerializeField]
		private Image m_renderer;

		private void Awake()
		{
			m_powerMaterial = m_power.GetComponent<Text>().material;
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
			base.gameObject.SetActive(value: true);
			base.transform.DOLocalRotate((seatId == 1 || seatId == 2) ? Vector3.zero : new Vector3(0f, 0f, 180f), 0.1f);
			base.transform.DOMove(pos, 0.1f);
			showTask = new FK3_Task(Show(seatId));
		}

		private IEnumerator Show(int seatId)
		{
			base.transform.DOScale(1.2f, 1f);
			yield return new WaitForSeconds(1f);
			base.transform.DOScale(1f, 0.2f);
			yield return new WaitForSeconds(0.2f);
			Vector3 targetPos = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatId).transform.position;
			targetPos.y += 1.5f;
			base.transform.DOMove(targetPos, 1f);
			yield return new WaitForSeconds(30f);
			Over();
		}

		public override void SetLayerOrder(int order)
		{
			if (m_renderer != null)
			{
			}
			if (!(m_power != null))
			{
			}
		}

		public override void DoFade()
		{
			m_renderer.DOFade(0f, 1f);
			m_powerMaterial.DOFade(0f, 1f);
		}
	}
}
