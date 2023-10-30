using UnityEngine;

public class DP_Test : MonoBehaviour
{
	public DP_SpinCtrl pointSpinCtrl;

	public DP_SpinCtrl animalSpinCtrl;

	public DP_AnimalColorCtrl animalColorCtrl;

	private int pointIndex;

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

	private int animalIndex;

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

	private int animalType;

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.A))
		{
			PointToAnimal(1, 3);
		}
	}

	private void GetAnimalIndex(int animalType)
	{
		switch (animalType)
		{
		case 0:
			animalIndex = rabbitArr[Random.Range(0, 9)];
			break;
		case 1:
			animalIndex = monkeyArr[Random.Range(0, 7)];
			break;
		case 2:
			animalIndex = pandaArr[Random.Range(0, 4)];
			break;
		default:
			animalIndex = lionArr[Random.Range(0, 4)];
			break;
		}
	}

	private void PointToAnimal(int animalColor, int animalType)
	{
		int num = animalColorCtrl.GetPointIndex(animalColor);
		pointSpinCtrl.SpinTo(num, 12f);
		GetAnimalIndex(animalType);
		int nNo = (24 + num - animalIndex) % 24;
		animalSpinCtrl.SpinTo(nNo, 15f, isClockwise: false);
		StartCoroutine(animalColorCtrl.PlayAnimalColorAnimIsStop(12f));
	}
}
