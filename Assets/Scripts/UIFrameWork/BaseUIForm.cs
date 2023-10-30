using UnityEngine;

namespace UIFrameWork
{
	public class BaseUIForm : MonoBehaviour
	{
		public UIType uiType = new UIType();

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
