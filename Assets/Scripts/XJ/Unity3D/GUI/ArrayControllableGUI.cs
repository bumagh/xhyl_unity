namespace XJ.Unity3D.GUI
{
	public abstract class ArrayControllableGUI : BaseGUI
	{
		private bool horizontalArray;

		public bool HorizontalArray
		{
			get
			{
				return horizontalArray;
			}
			set
			{
				horizontalArray = value;
			}
		}

		public ArrayControllableGUI(string title = null, bool boldTitle = false, bool horizontalArray = true)
			: base(title, boldTitle)
		{
			this.horizontalArray = horizontalArray;
		}
	}
}
