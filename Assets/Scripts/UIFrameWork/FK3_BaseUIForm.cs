using UnityEngine;

namespace UIFrameWork
{
	public class FK3_BaseUIForm : MonoBehaviour
	{
		public FK3_UIType uiType = new FK3_UIType();

		public virtual void Display()
		{
			base.gameObject.SetActive(value: true);
		}

		public virtual void hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
