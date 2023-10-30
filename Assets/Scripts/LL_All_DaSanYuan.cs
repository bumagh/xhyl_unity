using LL_GameCommon;
using UnityEngine;

public class LL_All_DaSanYuan : MonoBehaviour
{
	public GameObject[] mAnimalArr;

	public Material[] mMaterialArr;

	private bool _bShineAnimal = true;

	private float fAnimTime = 0.5f;

	private float fCurTime;

	private int _nShineIndex;

	private bool _isFirstRun;

	private void Start()
	{
		mAnimalArr[0].tag = "rabbit";
		mAnimalArr[1].tag = "monkey";
		mAnimalArr[2].tag = "panda";
		mAnimalArr[3].tag = "lion";
		_showAnimal(0);
		for (int i = 0; i < mAnimalArr.Length; i++)
		{
			mAnimalArr[i].transform.Find("Animal").GetComponent<Renderer>().material = mMaterialArr[i];
			mAnimalArr[i].GetComponent<LL_AnimalScript>().SetAnimation(LL_RoleAnimationScript.AnimationType.AT_STOP);
		}
		_isFirstRun = true;
	}

	private void Update()
	{
		updateAnimal();
	}

	public void Show()
	{
		Reset();
		iTween.ScaleFrom(base.gameObject, Vector3.zero, 1.5f);
	}

	public void ShowOneAnimal(int index)
	{
		_bShineAnimal = false;
		_showAnimal(index);
		for (int i = 0; i < mAnimalArr.Length; i++)
		{
			mAnimalArr[i].transform.eulerAngles = Vector3.zero;
		}
		mAnimalArr[index].GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Play_Win);
	}

	private void updateAnimal()
	{
		if (!_bShineAnimal)
		{
			return;
		}
		fCurTime += Time.deltaTime;
		if (fCurTime > fAnimTime)
		{
			fCurTime = 0f;
			_nShineIndex = (_nShineIndex + 1) % 4;
			_showAnimal(_nShineIndex);
		}
		base.transform.Find("Dsy_Animal").transform.Rotate(Vector3.up * 180f * Time.deltaTime);
		if (_isFirstRun)
		{
			for (int i = 0; i < mAnimalArr.Length; i++)
			{
				mAnimalArr[i].transform.Find("Animal").GetComponent<Renderer>().material = mMaterialArr[i];
				mAnimalArr[i].GetComponent<LL_AnimalScript>().SetAnimation(LL_RoleAnimationScript.AnimationType.AT_STOP);
			}
			_isFirstRun = false;
		}
	}

	private void _showAnimal(int index)
	{
		for (int i = 0; i < mAnimalArr.Length; i++)
		{
			if (i == index)
			{
				mAnimalArr[i].transform.Find("Animal").GetComponent<Renderer>().enabled = true;
			}
			else
			{
				mAnimalArr[i].transform.Find("Animal").GetComponent<Renderer>().enabled = false;
			}
		}
	}

	public void Reset()
	{
		_bShineAnimal = true;
		iTween.Stop(base.gameObject);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		for (int i = 0; i < mAnimalArr.Length; i++)
		{
			mAnimalArr[i].transform.eulerAngles = Vector3.zero;
		}
		_isFirstRun = true;
	}
}
