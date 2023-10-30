using System.Collections;
using UnityEngine;

public class ESP_ShaderAni
{
	public float speed = 14f;

	public float progress;

	public Material mat;

	private bool isRun;

	private float _nUseShader;

	private static int[] cellScrollMap = new int[9]
	{
		4,
		2,
		0,
		1,
		6,
		3,
		7,
		5,
		8
	};

	public ESP_ShaderAni(Material mat)
	{
		this.mat = mat;
	}

	public void Loop()
	{
		if (_nUseShader > 0f)
		{
			mat.SetFloat("_Index", progress);
			progress += Time.deltaTime * speed;
			progress %= 9f;
		}
	}

	public void SetUseShader(int use)
	{
		_nUseShader = use;
		mat.SetFloat("_UseShaderAni", use);
	}

	public IEnumerator AniStart(ESP_CellType startType)
	{
		yield return null;
		SetUseShader(1);
		float acc = 25f;
		speed = 1.5f;
		float accTime = 0.5f;
		float timer = 0f;
		progress = cellScrollMap[(int)(startType - 1)];
		while (timer < accTime)
		{
			speed += acc * Time.deltaTime * 1f;
			timer += Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator AniStop(ESP_CellType endType)
	{
		yield return null;
		float acc = 10f;
		float accTime = 0.3f;
		float timer = 0f;
		while (timer < accTime)
		{
			speed -= acc * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
		SetUseShader(0);
	}
}
