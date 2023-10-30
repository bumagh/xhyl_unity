using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_DragonColliderControl : MonoBehaviour
	{
		public delegate void EventHandler_DragonOnHit(Collider other);

		public event EventHandler_DragonOnHit Event_DragonOnHit_Handler;

		private void OnEnable()
		{
			this.Event_DragonOnHit_Handler = null;
		}

		private void OnTriggerEnter(Collider other)
		{
			UnityEngine.Debug.Log("Name: " + base.name + "be collider");
			if (other.gameObject.tag == "bullet")
			{
				this.Event_DragonOnHit_Handler(other);
			}
		}
	}
}
