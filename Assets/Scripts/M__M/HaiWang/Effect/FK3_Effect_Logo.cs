using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_Logo : MonoBehaviour
	{
		protected FK3_Task showTask;

		protected FK3_Task overTask;

		public virtual event Action<FK3_Effect_Logo> Event_Over_Handler;

		public virtual void Reset_EventHandler()
		{
			this.Event_Over_Handler = null;
		}

		public virtual void Reset_Logo()
		{
			if (showTask != null)
			{
				showTask.Stop();
				showTask = null;
			}
			if (overTask != null)
			{
				overTask.Stop();
				overTask = null;
			}
		}

		public virtual void Play(Vector3 pos, Vector3 posAdd, int seatId, params int[] values)
		{
		}

		public virtual void Over()
		{
			if (this.Event_Over_Handler != null)
			{
				this.Event_Over_Handler(this);
			}
			Reset_EventHandler();
		}

		public virtual void DoFade()
		{
		}

		public virtual IEnumerator ExecuteOver(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			Over();
		}

		public virtual void SetLayerOrder(int order)
		{
		}
	}
}
