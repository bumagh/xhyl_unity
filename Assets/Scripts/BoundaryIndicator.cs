using UnityEngine;

public class BoundaryIndicator : BoundaryWireframe
{
	public BoundaryIndicator()
	{
		boundary = new Rect(-6.4f, -3.6f, 12.8f, 7.2f);
	}
}
