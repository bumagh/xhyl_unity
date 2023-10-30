using UnityEngine;

public class FK3_ShowFPS_OnGUI : MonoBehaviour
{
	public float fpsMeasuringDelta = 2f;

	private float timePassed;

	private int m_FrameCount;

	private float m_FPS;

	private void Start()
	{
		timePassed = 0f;
	}

	private void Update()
	{
		m_FrameCount++;
		timePassed += Time.deltaTime;
		if (timePassed > fpsMeasuringDelta)
		{
			m_FPS = (float)m_FrameCount / timePassed;
			timePassed = 0f;
			m_FrameCount = 0;
		}
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.background = null;
		gUIStyle.normal.textColor = new Color(1f, 0.5f, 0f);
		gUIStyle.fontSize = 40;
		GUI.Label(new Rect(0f, 0f, 100f, 100f), $"{m_FPS}", gUIStyle);
	}
}
