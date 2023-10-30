using LL_GameCommon;
using System;
using System.Collections;
using UnityEngine;

public class LL_AnimalMngr : MonoBehaviour
{
	public GameObject m_TurnPlatform_Prefab;

	public GameObject[] _AnimalPrefabArray = new GameObject[4];

	private ArrayList _AnimalList = new ArrayList();

	private ArrayList _AnimalRingList = new ArrayList();

	public Material mGoldMat;

	public GameObject original;

	private void Start()
	{
		Create24Animal();
		Create24AnimalRing();
	}

	private void Update()
	{
	}

	public void Create24Animal()
	{
		int num = 0;
		IEnumerator enumerator = m_TurnPlatform_Prefab.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				GameObject gameObject = NewAnimal((AnimalType)num, transform);
				num++;
				num %= 4;
				gameObject.transform.position += Vector3.up * 0.2f;
				Transform transform2 = gameObject.transform;
				Vector3 up = Vector3.up;
				Vector3 position = gameObject.transform.position;
				transform2.LookAt(up * position.y);
				gameObject.transform.localScale = LL_Parameter.G_AnimalMinScale;
				gameObject.transform.parent = transform;
				gameObject.GetComponent<LL_AnimalScript>().mGoldMat = mGoldMat;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void Create24AnimalRing()
	{
		int num = 0;
		IEnumerator enumerator = m_TurnPlatform_Prefab.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				GameObject gameObject = NewAnimalRing(transform);
				gameObject.transform.position -= Vector3.up * 0.85f;
				gameObject.transform.parent = transform;
				LL_AnimalScript component = GetAnimal(num).GetComponent<LL_AnimalScript>();
				component.mAnimalRing = gameObject;
				gameObject.SetActive(value: false);
				num++;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public GameObject NewAnimal(AnimalType type, Transform trans)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(_AnimalPrefabArray[(int)type], trans.position, Quaternion.identity);
		switch (type)
		{
		case AnimalType.Lion:
			gameObject.tag = "lion";
			gameObject.transform.Find("Animal").GetComponent<Renderer>().material.SetFloat("_Cutoff", 0.1f);
			break;
		case AnimalType.Monkey:
			gameObject.tag = "monkey";
			break;
		case AnimalType.Panda:
			gameObject.tag = "panda";
			break;
		case AnimalType.Rabbit:
			gameObject.tag = "rabbit";
			break;
		}
		_AnimalList.Add(gameObject);
		return gameObject;
	}

	public GameObject NewAnimalRing(Transform trans)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(original, trans.position, Quaternion.identity);
		_AnimalRingList.Add(gameObject);
		return gameObject;
	}

	public GameObject GetAnimal(int index)
	{
		if (index < LL_Parameter.G_nAnimalNumber)
		{
			return (GameObject)_AnimalList[index];
		}
		UnityEngine.Debug.Log("GetAnimal wrong index");
		return null;
	}

	public void SetAllAnimalState(Animal_Action_State state)
	{
		for (int i = 0; i < _AnimalList.Count; i++)
		{
			GameObject gameObject = (GameObject)_AnimalList[i];
			if (gameObject != null)
			{
				LL_AnimalScript component = gameObject.GetComponent<LL_AnimalScript>();
				component.SetAnimalActionState(state);
			}
			else
			{
				UnityEngine.Debug.Log("SetAllAnimalState _AnimalList[i] null error!");
			}
		}
	}

	public void ShineAllAnimal(bool isShine)
	{
		for (int i = 0; i < _AnimalList.Count; i++)
		{
			GameObject gameObject = (GameObject)_AnimalList[i];
			if (gameObject != null)
			{
				LL_AnimalScript component = gameObject.GetComponent<LL_AnimalScript>();
				component.ShowGoldShine(isShine);
			}
			else
			{
				UnityEngine.Debug.Log("SetAllAnimalState _AnimalList[i] null error!");
			}
		}
	}

	public void SetColorOfAnimalRingByIndex(int index, AnimalColor color)
	{
		GetAnimal(index).GetComponent<LL_AnimalScript>().SetColorOfAnimalRing(color);
	}
}
