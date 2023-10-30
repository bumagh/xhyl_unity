using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DP_AnimalColorCtrl : MonoBehaviour
{
	[SerializeField]
	private Material[] materials = new Material[6];

	private MeshRenderer[] mrAnimalColors = new MeshRenderer[24];

	private int[] colorIndexs = new int[24];

	private List<int> listRed = new List<int>();

	private List<int> listGreen = new List<int>();

	private List<int> listYellow = new List<int>();

	[SerializeField]
	private DP_SpinCtrl pointSpinCtrl;

	private WaitForSeconds wait = new WaitForSeconds(0.1f);

	private int pointIndex = 23;

	private WaitForSeconds waitForShine = new WaitForSeconds(0.5f);

	private void Awake()
	{
		for (int i = 0; i < 24; i++)
		{
			mrAnimalColors[i] = base.transform.Find($"{i + 1}/deng/deng_0").GetComponent<MeshRenderer>();
		}
	}

	private void Update()
	{
		if (pointSpinCtrl.spinState != SPIN_STATE.ST_STOP)
		{
			PlayAnimalColorAnimIsSpin();
		}
	}

	public void SetColorIndexs()
	{
		List<int> list = new List<int>();
		listRed.Clear();
		listGreen.Clear();
		listYellow.Clear();
		for (int i = 0; i < 18; i++)
		{
			int item = i % 3;
			list.Add(item);
		}
		for (int j = 0; j < 6; j++)
		{
			int item2 = UnityEngine.Random.Range(0, 3);
			list.Add(item2);
		}
		for (int k = 0; k < 24; k++)
		{
			int index = UnityEngine.Random.Range(0, 24 - k);
			colorIndexs[k] = list[index];
			list.RemoveAt(index);
			if (colorIndexs[k] == 0)
			{
				listRed.Add(k);
			}
			else if (colorIndexs[k] == 1)
			{
				listGreen.Add(k);
			}
			else if (colorIndexs[k] == 2)
			{
				listYellow.Add(k);
			}
		}
		StartCoroutine("SetAnimalColors");
	}

	public void SetColorIndexs(int[] colors)
	{
		for (int i = 0; i < 24; i++)
		{
			colorIndexs[i] = colors[i];
		}
		StartCoroutine("SetAnimalColors");
	}

	private IEnumerator SetAnimalColors()
	{
		int ponitIndex = GetCurPointIndex();
		mrAnimalColors[ponitIndex].material = materials[colorIndexs[ponitIndex] + 3];
		for (int i = 1; i < 24; i++)
		{
			int curIndex = (i + ponitIndex) % 24;
			yield return wait;
			mrAnimalColors[curIndex].material = materials[colorIndexs[curIndex]];
		}
	}

	public int GetPointIndex(int animalColor)
	{
		int result = 0;
		switch (animalColor)
		{
		case 0:
			result = listRed[Random.Range(0, listRed.Count)];
			break;
		case 1:
			result = listGreen[Random.Range(0, listGreen.Count)];
			break;
		case 2:
			result = listYellow[Random.Range(0, listYellow.Count)];
			break;
		}
		return result;
	}

	public int GetPointIndexColor(int index)
	{
		return colorIndexs[index];
	}

	public void PlayAnimalColorAnimIsSpin()
	{
		int curPointIndex = GetCurPointIndex();
		if (pointIndex != curPointIndex)
		{
			mrAnimalColors[pointIndex].material = materials[colorIndexs[pointIndex]];
			pointIndex = curPointIndex;
			mrAnimalColors[pointIndex].material = materials[colorIndexs[pointIndex] + 3];
		}
	}

	public IEnumerator PlayAnimalColorAnimIsStop(float spinTime)
	{
		yield return new WaitForSeconds(spinTime);
		int index = GetCurPointIndex();
		for (int i = 0; i < 5; i++)
		{
			yield return waitForShine;
			mrAnimalColors[index].material = materials[colorIndexs[index]];
			yield return waitForShine;
			mrAnimalColors[index].material = materials[colorIndexs[index] + 3];
		}
	}

	private int GetCurPointIndex()
	{
		Vector3 localEulerAngles = pointSpinCtrl.transform.localEulerAngles;
		float num = (localEulerAngles.y + 360f) % 360f;
		return (int)((num + 7.5f) / 15f) % 24;
	}

	public void SetPointToIndex(int index)
	{
		index %= 24;
		pointSpinCtrl.SetAglToNo(index);
	}
}
