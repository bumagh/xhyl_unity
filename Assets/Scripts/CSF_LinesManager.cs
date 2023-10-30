using UnityEngine;

public class CSF_LinesManager : MonoBehaviour
{
	[SerializeField]
	private CSF_Lines lines;

	private float width = 1135f;

	private float height = 470f;

	private Vector2 vec = new Vector2(1135f, 470f);

	private static float[] LineTypeLeftOffsetMap = new float[6]
	{
		0f,
		0f,
		0f,
		0.4f,
		0.2f,
		0f
	};

	private static float[] LineTypeRightOffsetMap = new float[6]
	{
		0f,
		0.4f,
		0.2f,
		0f,
		0f,
		0f
	};

	public void ShowLine(int line, CSF_WinLineType lineType = CSF_WinLineType.None)
	{
		lines.imgLines[line].SetActive(value: true);
	}

	public void ShowLines(int total = 9)
	{
		for (int i = 0; i < total; i++)
		{
			lines.imgLines[i].SetActive(value: true);
		}
	}

	public void HideLine(int line)
	{
		lines.imgLines[line].SetActive(value: false);
	}

	public void HideAllLines()
	{
		for (int i = 0; i < 9; i++)
		{
			lines.imgLines[i].SetActive(value: false);
		}
	}
}
