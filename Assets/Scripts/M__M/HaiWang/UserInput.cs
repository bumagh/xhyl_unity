using System;

namespace M__M.HaiWang
{
	public class UserInput : SimpleSingletonBehaviour<UserInput>
	{
		private bool m_touching;

		public bool allowInput = true;

		public Action onClick;

		private void Awake()
		{
			SimpleSingletonBehaviour<UserInput>.s_instance = this;
		}

		private void OnMouseUpAsButton()
		{
			if (onClick != null)
			{
				onClick();
			}
		}

		private void OnMouseDown()
		{
			m_touching = true;
		}

		private void OnMouseUp()
		{
			m_touching = false;
		}

		private void OnMouseDrag()
		{
		}

		public bool IsTouching()
		{
			return allowInput && m_touching;
		}
	}
}
