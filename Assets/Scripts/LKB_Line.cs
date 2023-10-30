using UnityEngine;

public class LKB_Line : MonoBehaviour
{
	private SpriteRenderer _render;

	public int index;

	private static float[,] LineTypeLeftOffsetMap = new float[9, 6]
	{
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		},
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		},
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		},
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		},
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		},
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		},
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		},
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		},
		{
			0f,
			0.6f,
			0.8f,
			0f,
			0f,
			1f
		}
	};

	private static float[,] LineTypeRightOffsetMap = new float[9, 6]
	{
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		},
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		},
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		},
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		},
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		},
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		},
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		},
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		},
		{
			1f,
			1f,
			1f,
			0.4f,
			0.2f,
			0f
		}
	};

	private void Awake()
	{
		_render = GetComponent<SpriteRenderer>();
	}

	public void Show(LKB_WinLineType lineType = LKB_WinLineType.Full)
	{
		base.gameObject.SetActive(value: true);
		_render.material.SetFloat("_Left", LineTypeLeftOffsetMap[index, (int)lineType]);
		_render.material.SetFloat("_Right", LineTypeRightOffsetMap[index, (int)lineType]);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
