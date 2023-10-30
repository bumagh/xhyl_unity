using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/FontGradient")]
public class FK3_FontGradient : BaseMeshEffect
{
	[SerializeField]
	private Color32 topColor = Color.white;

	[SerializeField]
	private Color32 bottomColor = Color.black;

	public override void ModifyMesh(VertexHelper vh)
	{
		if (IsActive())
		{
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			int count = list.Count;
			ApplyGradient(list, 0, count);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}
	}

	private void ApplyGradient(List<UIVertex> vertexList, int start, int end)
	{
		if (vertexList.Count == 0)
		{
			return;
		}
		UIVertex uIVertex = vertexList[0];
		float num = uIVertex.position.y;
		UIVertex uIVertex2 = vertexList[0];
		float num2 = uIVertex2.position.y;
		for (int i = start; i < end; i++)
		{
			UIVertex uIVertex3 = vertexList[i];
			float y = uIVertex3.position.y;
			if (y > num2)
			{
				num2 = y;
			}
			else if (y < num)
			{
				num = y;
			}
		}
		float num3 = num2 - num;
		for (int j = start; j < end; j++)
		{
			UIVertex value = vertexList[j];
			value.color = Color32.Lerp(bottomColor, topColor, (value.position.y - num) / num3);
			vertexList[j] = value;
		}
	}
}
