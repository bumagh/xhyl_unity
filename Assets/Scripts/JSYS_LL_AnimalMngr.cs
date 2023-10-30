using JSYS_LL_GameCommon;
using System;
using System.Collections;
using UnityEngine;

public class JSYS_LL_AnimalMngr : MonoBehaviour
{
	public GameObject m_TurnPlatform_Prefab;

	public TextMesh m_Text;

	private GameObject[] _AnimalPrefabArray = new GameObject[4];

	private ArrayList _AnimalList = new ArrayList();

	private ArrayList _AnimalRingList = new ArrayList();

	public Material mGoldMat;

	private void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			_AnimalPrefabArray[i] = (GameObject)Resources.Load(JSYS_LL_Parameter.G_AnimalPrefabNameArray[i]);
		}
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
				gameObject.transform.localScale = JSYS_LL_Parameter.G_AnimalMinScale;
				gameObject.transform.parent = transform;
				gameObject.GetComponent<JSYS_LL_AnimalScript>().mGoldMat = mGoldMat;
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
				JSYS_LL_AnimalScript component = GetAnimal(num).GetComponent<JSYS_LL_AnimalScript>();
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
		_AnimalList.Add(gameObject);
		return gameObject;
	}

	public GameObject NewAnimalRing(Transform trans)
	{
		GameObject original = (GameObject)Resources.Load("Scene/dongwu_SpinRing");
		GameObject gameObject = UnityEngine.Object.Instantiate(original, trans.position, Quaternion.identity);
		_AnimalRingList.Add(gameObject);
		return gameObject;
	}

	public GameObject GetAnimal(int index)
	{
		if (index < JSYS_LL_Parameter.G_nAnimalNumber)
		{
			return (GameObject)_AnimalList[index];
		}
		return null;
	}

	public void SetAllAnimalState(Animal_Action_State state)
	{
		for (int i = 0; i < _AnimalList.Count; i++)
		{
			GameObject gameObject = (GameObject)_AnimalList[i];
			if (gameObject != null)
			{
				JSYS_LL_AnimalScript component = gameObject.GetComponent<JSYS_LL_AnimalScript>();
				component.SetAnimalActionState(state);
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
				JSYS_LL_AnimalScript component = gameObject.GetComponent<JSYS_LL_AnimalScript>();
				component.ShowGoldShine(isShine);
			}
		}
	}

	public void SetColorOfAnimalRingByIndex(int index, AnimalColor color)
	{
		GetAnimal(index).GetComponent<JSYS_LL_AnimalScript>().SetColorOfAnimalRing(color);
	}
}
