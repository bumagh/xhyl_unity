using UnityEngine;

public class DP_AnimalCtrl : MonoBehaviour
{
	private int[] animalTypes = new int[24]
	{
		1,
		3,
		2,
		1,
		0,
		1,
		3,
		0,
		1,
		0,
		2,
		0,
		1,
		3,
		2,
		0,
		1,
		0,
		3,
		0,
		1,
		0,
		2,
		0
	};

	private int[] rabbitArr = new int[9]
	{
		4,
		7,
		9,
		11,
		15,
		17,
		19,
		21,
		23
	};

	private int[] monkeyArr = new int[7]
	{
		0,
		3,
		5,
		8,
		12,
		16,
		20
	};

	private int[] pandaArr = new int[4]
	{
		2,
		10,
		14,
		22
	};

	private int[] lionArr = new int[4]
	{
		1,
		6,
		13,
		18
	};

	[HideInInspector]
	public DP_AnimalAnimCtrl[] animalAnimCtrls = new DP_AnimalAnimCtrl[24];

	private void Awake()
	{
		for (int i = 0; i < 24; i++)
		{
			animalAnimCtrls[i] = base.transform.Find($"Animal/animal{i + 1}").GetComponent<DP_AnimalAnimCtrl>();
			animalAnimCtrls[i].Init();
		}
	}

	public int GetAnimalIndex(int animalType)
	{
		int num = 0;
		switch (animalType)
		{
		case 3:
			return rabbitArr[Random.Range(0, 9)];
		case 2:
			return monkeyArr[Random.Range(0, 7)];
		case 1:
			return pandaArr[Random.Range(0, 4)];
		default:
			return lionArr[Random.Range(0, 4)];
		}
	}

	public void ResetAllAnimals()
	{
		for (int i = 0; i < 24; i++)
		{
			animalAnimCtrls[i].Reset();
		}
	}
}
