using UnityEngine;

public class STWM_LinesManager : MonoBehaviour
{
	[SerializeField]
	private STWM_Lines[] lines = new STWM_Lines[2];

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

	public void ShowLine(int line, STWM_WinLineType lineType = STWM_WinLineType.None)
	{
		lines[0].imgLines[line].SetActive(value: true);
		lines[1].imgLines[line].SetActive(value: true);
	}

	public void ShowLines(int total = 9)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < total; j++)
			{
				lines[i].imgLines[j].SetActive(value: true);
			}
		}
	}

	public void HideLine(int line)
	{
		for (int i = 0; i < 2; i++)
		{
			lines[i].imgLines[line].SetActive(value: false);
		}
	}

	public void HideAllLines()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				lines[i].imgLines[j].SetActive(value: false);
			}
		}
	}
}
