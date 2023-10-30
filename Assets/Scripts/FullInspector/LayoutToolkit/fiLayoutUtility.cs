namespace FullInspector.LayoutToolkit
{
	public static class fiLayoutUtility
	{
		public static fiLayout Margin(float margin, fiLayout layout)
		{
			fiHorizontalLayout fiHorizontalLayout = new fiHorizontalLayout();
			fiHorizontalLayout.Add(margin);
			fiHorizontalLayout.Add(new fiVerticalLayout
			{
				margin,
				layout,
				margin
			});
			fiHorizontalLayout.Add(margin);
			return fiHorizontalLayout;
		}
	}
}
