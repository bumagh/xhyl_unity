using DG.Tweening;
using System;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class Effect_Coin : MonoBehaviour
	{
		public event Action<Effect_Coin> Event_Over_Handler;

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void Reset_EventHandler()
		{
			this.Event_Over_Handler = null;
		}

		public void Reset_Coin()
		{
			DOTween.Kill(base.transform);
		}

		public void Play(Vector3 begin, Vector3 end)
		{
			Vector3 position = base.transform.position;
			end.z = (begin.z = position.z);
			base.gameObject.SetActive(value: true);
			base.transform.position = begin;
			Vector3 a = begin - end;
			Tweener t = base.transform.DOMove(begin + a * 0.3f, 1f);
			Tweener t2 = base.transform.DOMove(end, 1f);
			t2.OnComplete(delegate
			{
				Over();
			});
			Sequence s = DOTween.Sequence();
			s.Append(t);
			s.Append(t2);
		}

		private void Over()
		{
			if (this.Event_Over_Handler != null)
			{
				this.Event_Over_Handler(this);
			}
		}
	}
}
