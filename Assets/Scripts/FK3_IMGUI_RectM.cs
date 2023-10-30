using UnityEngine;

public class FK3_IMGUI_RectM
{
	private float m_width;

	private float m_height;

	private float m_marginLeft;

	private float m_marginRight;

	private float m_marginTop;

	private float m_marginDown;

	private Rect m_screen;

	private FK3_IMGUI_Layout_Type m_layout;

	public FK3_IMGUI_RectM()
	{
		m_screen = _GetScreenRect();
	}

	private Rect _GetScreenRect()
	{
		return new Rect(0f, 0f, Screen.width, Screen.height);
	}

	public FK3_IMGUI_RectM SetScreen(Rect rect)
	{
		m_screen = rect;
		return this;
	}

	public FK3_IMGUI_RectM UpdateScreen()
	{
		if (m_screen.width != (float)Screen.width || m_screen.height != (float)Screen.height)
		{
		}
		m_screen.width = Screen.width;
		m_screen.height = Screen.height;
		return this;
	}

	public FK3_IMGUI_RectM SetSize(float width, float height)
	{
		m_width = width;
		m_height = height;
		return this;
	}

	public FK3_IMGUI_RectM SetMarginLeft(float marginLeft)
	{
		m_marginLeft = marginLeft;
		return this;
	}

	public FK3_IMGUI_RectM SetMarginRight(float marginRight)
	{
		m_marginRight = marginRight;
		return this;
	}

	public FK3_IMGUI_RectM SetMarginTop(float marginTop)
	{
		m_marginTop = marginTop;
		return this;
	}

	public FK3_IMGUI_RectM SetMarginDown(float marginDown)
	{
		m_marginDown = marginDown;
		return this;
	}

	public Rect GetRect(FK3_IMGUI_Layout_Type layoutType)
	{
		Rect result = default(Rect);
		result.x = m_screen.x;
		result.y = m_screen.y;
		result.width = m_width;
		result.height = m_height;
		switch (layoutType)
		{
		case FK3_IMGUI_Layout_Type.TopLeft:
			result.x += m_marginLeft;
			result.y += m_marginTop;
			break;
		case FK3_IMGUI_Layout_Type.TopRight:
			result.x += m_screen.width - m_width - m_marginRight;
			result.y += m_marginTop;
			break;
		case FK3_IMGUI_Layout_Type.TopCenter:
			result.x += (m_screen.width - m_width) / 2f;
			result.y += m_marginTop;
			break;
		case FK3_IMGUI_Layout_Type.MiddleLeft:
			result.x += m_marginLeft;
			result.y += (m_screen.height - m_height) / 2f + m_marginTop;
			break;
		case FK3_IMGUI_Layout_Type.MiddleRight:
			result.x += m_screen.width - m_width - m_marginRight;
			result.y += (m_screen.height - m_height) / 2f + m_marginTop;
			break;
		case FK3_IMGUI_Layout_Type.MiddleCenter:
			result.x += (m_screen.width - m_width) / 2f;
			result.y += (m_screen.height - m_height) / 2f + m_marginTop;
			break;
		case FK3_IMGUI_Layout_Type.BottomLeft:
			result.x += m_marginLeft;
			result.y += m_screen.height - m_height - m_marginDown;
			break;
		case FK3_IMGUI_Layout_Type.BottomRight:
			result.x += m_screen.width - m_width - m_marginRight;
			result.y += m_screen.height - m_height - m_marginDown;
			break;
		case FK3_IMGUI_Layout_Type.BottomCenter:
			result.x += (m_screen.width - m_width) / 2f;
			result.y += m_screen.height - m_height - m_marginDown;
			break;
		}
		return result;
	}

	public Rect GetRect()
	{
		return GetRect(m_layout);
	}

	public Rect GetOffsetRect(float x, float y)
	{
		Rect rect = GetRect(m_layout);
		rect.x += x;
		rect.y += y;
		return rect;
	}

	public FK3_IMGUI_RectM SetLayout(FK3_IMGUI_Layout_Type layoutType)
	{
		m_layout = layoutType;
		return this;
	}

	public FK3_IMGUI_RectM Clone()
	{
		FK3_IMGUI_RectM fK3_IMGUI_RectM = new FK3_IMGUI_RectM();
		fK3_IMGUI_RectM.m_width = m_width;
		fK3_IMGUI_RectM.m_height = m_height;
		fK3_IMGUI_RectM.m_marginLeft = m_marginLeft;
		fK3_IMGUI_RectM.m_marginRight = m_marginRight;
		fK3_IMGUI_RectM.m_screen = new Rect(m_screen);
		fK3_IMGUI_RectM.m_layout = m_layout;
		return fK3_IMGUI_RectM;
	}
}
