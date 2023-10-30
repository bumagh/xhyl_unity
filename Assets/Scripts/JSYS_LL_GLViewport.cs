using UnityEngine;

public class JSYS_LL_GLViewport : MonoBehaviour
{
	public Material mat;

	private bool stretch;

	private void Update()
	{
	}

	private void OnPostRender()
	{
		if ((bool)mat)
		{
			GL.PushMatrix();
			mat.SetPass(0);
			GL.LoadPixelMatrix();
			if (stretch)
			{
				GL.Viewport(new Rect(0f, 0f, Screen.width, Screen.height));
			}
			else
			{
				GL.Viewport(new Rect(0f, 0f, Screen.width / 2, Screen.height));
			}
			GL.Color(Color.red);
			GL.Begin(7);
			GL.Vertex3(0f, 0f, 0f);
			GL.Vertex3(0f, Screen.height, 0f);
			GL.Vertex3(Screen.width, Screen.height, 0f);
			GL.Vertex3(Screen.width, 0f, 0f);
			GL.Color(Color.yellow);
			GL.End();
			GL.Begin(4);
			GL.Vertex3(Screen.width / 2, Screen.height / 4, 1f);
			GL.Vertex3(Screen.width / 4, Screen.height / 2, 1f);
			GL.Vertex3(Screen.width - Screen.width / 4, Screen.height / 2, 1f);
			GL.End();
			GL.PopMatrix();
		}
	}
}
