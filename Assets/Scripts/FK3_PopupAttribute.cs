using UnityEngine;

public class FK3_PopupAttribute : PropertyAttribute
{
	public object[] list;

	public FK3_PopupAttribute(params object[] list)
	{
		this.list = list;
	}
}
