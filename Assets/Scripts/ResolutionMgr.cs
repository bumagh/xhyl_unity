using UnityEngine;

[ExecuteInEditMode]
public class ResolutionMgr : MonoBehaviour
{
	private Camera m_camera;

	[SerializeField]
	private int m_designWidth = 1280;

	[SerializeField]
	private int m_designHeight = 720;

	private float m_desginAspect;

	private int m_lastScreenWidth;

	private int m_lastScreenHeight;

	private void Awake()
	{
		m_camera = GetComponent<Camera>();
		m_desginAspect = (float)m_designWidth * 1f / (float)m_designHeight;
		Init();
	}

	private void Init()
	{
		m_lastScreenHeight = Screen.height;
		m_lastScreenWidth = Screen.width;
		AdaptScreen();
	}

	private void Start()
	{
	}

	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			Init();
		}
	}

	private void Update()
	{
		if (m_lastScreenHeight != Screen.height || m_lastScreenWidth != Screen.width)
		{
			AdaptScreen();
		}
	}

	private void AdaptScreen()
	{
		float num = (float)m_designHeight / 200f;
		float num2 = (float)Screen.width * 1f / (float)Screen.height;
		num = (((float)Mathf.Abs(Screen.width * m_designHeight - Screen.height * m_designWidth) * 1f / (float)(Screen.width * m_designHeight) < 0.01f) ? ((float)m_designHeight / 200f) : ((!(num2 > m_desginAspect)) ? ((float)m_designHeight * m_desginAspect / (200f * num2)) : ((float)m_designHeight / 200f)));
		if (m_camera != null)
		{
			m_camera.orthographicSize = num;
		}
	}
}
