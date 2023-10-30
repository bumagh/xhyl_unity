using BCBM_GameCommon;
using System;
using System.Collections;
using UnityEngine;

public class BCBM_ColorBoard : MonoBehaviour
{
	public enum LIGHT_TYPE
	{
		LightType_Red,
		LightType_Green,
		LightType_Yellow
	}

	public enum LIGHT_STATE
	{
		ST_LightSpin,
		ST_LightQuicklyShine,
		ST_LightSlowlyShine,
		ST_LightReset
	}

	private System.Random _SysRandom;

	public Texture[] mLightRGB;

	public Texture[] mLightShineRGB;

	private GameObject[] _LightObjArray = new GameObject[24];

	private GameObject[] _LightShineObjArray = new GameObject[24];

	private int[] _LightArr = new int[24];

	private int m_nLastLightNo = -1;

	private ArrayList m_LastLightArr = new ArrayList();

	public BCBM_SpinScript m_LightPoiter;

	private int _nLightOffset = 5;

	private float _fCurStateTime;

	public LIGHT_STATE mLightState = LIGHT_STATE.ST_LightReset;

	private bool _isSpecialAllShine;

	private float _fShineTime;

	private bool _isLight = true;

	private void Start()
	{
		_SysRandom = new System.Random((int)DateTime.Now.Ticks & 0xFFFF);
		for (int i = 0; i < 24; i++)
		{
			string n = "red_gre_yel" + (i + 1).ToString();
			_LightObjArray[i] = base.transform.Find(n).gameObject;
			n = "red_gre_yel_light" + (i + 1).ToString();
			_LightShineObjArray[i] = _LightObjArray[i].transform.Find(n).gameObject;
			_LightShineObjArray[i].GetComponent<Renderer>().enabled = false;
		}
		GetLampArray(ref _LightArr);
		SetLightsColorImmediate(_LightArr);
		m_LightPoiter = GameObject.Find("BallPointer").GetComponent<BCBM_SpinScript>();
		Reset();
	}

	private void Update()
	{
		TestKey();
		if (m_LightPoiter.IsStop() && mLightState == LIGHT_STATE.ST_LightSpin && !_isSpecialAllShine)
		{
			mLightState = LIGHT_STATE.ST_LightQuicklyShine;
			SetLightShineQuickly(m_nLastLightNo);
		}
		if (mLightState == LIGHT_STATE.ST_LightQuicklyShine)
		{
			_fCurStateTime += Time.deltaTime;
			if (_fCurStateTime > 1f)
			{
				mLightState = LIGHT_STATE.ST_LightSlowlyShine;
			}
			_fShineTime += Time.deltaTime;
			if (_fShineTime > 71f / (339f * (float)Math.PI))
			{
				_fShineTime = 0f;
				for (int i = 0; i < m_LastLightArr.Count; i++)
				{
					_LightShineObjArray[(int)m_LastLightArr[i]].GetComponent<Renderer>().enabled = _isLight;
				}
				_isLight = !_isLight;
			}
		}
		if (mLightState != LIGHT_STATE.ST_LightSlowlyShine)
		{
			return;
		}
		_fCurStateTime += Time.deltaTime;
		_fShineTime += Time.deltaTime;
		if (_fShineTime > 0.5f)
		{
			_fShineTime = 0f;
			for (int j = 0; j < m_LastLightArr.Count; j++)
			{
				_LightShineObjArray[(int)m_LastLightArr[j]].GetComponent<Renderer>().enabled = _isLight;
			}
			_isLight = !_isLight;
		}
	}

	public void SetPonterToLight(int nNo)
	{
		int aglToNo = (nNo - _nLightOffset + 24) % 24;
		m_LightPoiter.SetAglToNo(aglToNo);
	}

	public void GoPointer_ToLightNo(int nLightNumber, float time)
	{
		m_nLastLightNo = nLightNumber;
		mLightState = LIGHT_STATE.ST_LightSpin;
		int nNo = (nLightNumber - _nLightOffset + 24) % 24;
		m_LightPoiter.SpinTo(nNo, time, isClockwise: false);
		StopLightShine();
		m_LastLightArr.Clear();
	}

	public AnimalColor GetLightColor2(int nLightNo)
	{
		return (AnimalColor)_LightArr[nLightNo];
	}

	private void _setColor(GameObject obj, Texture tx)
	{
		obj.transform.GetComponent<Renderer>().material.mainTexture = tx;
		if (obj.GetComponent<Renderer>().material.IsKeywordEnabled("_EMISSION"))
		{
			obj.GetComponent<Renderer>().material.SetTexture("_EmissionMap", tx);
		}
	}

	public void UpdateLightColor(int[] arry)
	{
		BCBM_MusicMngr.GetSingleton().PlayGameMusic(BCBM_MusicMngr.MUSIC_GAME_MUSIC.GAME_BG);
		BCBM_MusicMngr.GetSingleton().PlaySceneSound(BCBM_MusicMngr.MUSIC_SCENE_MUSIC.SCENE_RESET_COLOR);
		StopCoroutine("SetLightsColor");
		GetLampArray(arry);
		GrayAllLight();
		StartCoroutine("SetLightsColor", arry);
	}

	public IEnumerator SetLightsColor(int[] lampArray)
	{
		for (int i = 0; i < 24; i++)
		{
			_setColor(_LightObjArray[i], mLightRGB[lampArray[i]]);
			_setColor(_LightShineObjArray[i], mLightShineRGB[lampArray[i]]);
			yield return new WaitForSeconds(0.05f);
		}
	}

	public void SetLightsColorImmediate(int[] lampArray)
	{
		for (int i = 0; i < 24; i++)
		{
			_setColor(_LightObjArray[i], mLightRGB[lampArray[i]]);
			_setColor(_LightShineObjArray[i], mLightShineRGB[lampArray[i]]);
		}
	}

	public LIGHT_TYPE GetLightColor(int index)
	{
		return (LIGHT_TYPE)_LightArr[index];
	}

	public void ShineMoreLightsQuickly(int index)
	{
		mLightState = LIGHT_STATE.ST_LightQuicklyShine;
		m_LastLightArr.Add(index);
	}

	public void SetSpecial()
	{
		_isSpecialAllShine = true;
	}

	public void SetLightShineQuickly(int index)
	{
		ShineMoreLightsQuickly(index);
	}

	public void StopLightShine()
	{
		for (int i = 0; i < _LightShineObjArray.Length; i++)
		{
			_LightShineObjArray[i].GetComponent<Renderer>().enabled = false;
		}
	}

	public void Reset()
	{
		_isSpecialAllShine = false;
		_fCurStateTime = 0f;
		m_LastLightArr.Clear();
		StopCoroutine("ShineLightQuickly");
		mLightState = LIGHT_STATE.ST_LightReset;
		StopLightShine();
		m_LightPoiter.Reset();
	}

	private void GrayAllLight()
	{
		for (int i = 0; i < 24; i++)
		{
			_setColor(_LightObjArray[i], mLightRGB[0]);
		}
	}

	private void TestKey()
	{
	}

	public bool Test()
	{
		int[] array = new int[24];
		int num = rand() % 4;
		bool flag = false;
		for (int i = 0; i < 24; i++)
		{
			array[i] = num;
			num = (num + 1) % 4;
		}
		int[] lampArray = new int[24];
		GetLampArray(ref lampArray);
		bool[] array2 = new bool[5];
		for (int j = 0; j < 24; j++)
		{
			if (lampArray[j] == 0)
			{
				if (array[j] == 0)
				{
					array2[0] = true;
				}
				if (array[j] == 1)
				{
					array2[1] = true;
				}
				if (array[j] == 2)
				{
					array2[2] = true;
				}
				if (array[j] == 3)
				{
					array2[3] = true;
				}
			}
			if (array2[0] && array2[1] && array2[2] && array2[3])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		flag = false;
		array2[0] = false;
		array2[1] = false;
		array2[2] = false;
		array2[3] = false;
		for (int k = 0; k < 24; k++)
		{
			if (lampArray[k] == 1)
			{
				if (array[k] == 0)
				{
					array2[0] = true;
				}
				if (array[k] == 1)
				{
					array2[1] = true;
				}
				if (array[k] == 2)
				{
					array2[2] = true;
				}
				if (array[k] == 3)
				{
					array2[3] = true;
				}
			}
			if (array2[0] && array2[1] && array2[2] && array2[3])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		flag = false;
		array2[0] = false;
		array2[1] = false;
		array2[2] = false;
		array2[3] = false;
		for (int l = 0; l < 24; l++)
		{
			if (lampArray[l] == 2)
			{
				if (array[l] == 0)
				{
					array2[0] = true;
				}
				if (array[l] == 1)
				{
					array2[1] = true;
				}
				if (array[l] == 2)
				{
					array2[2] = true;
				}
				if (array[l] == 3)
				{
					array2[3] = true;
				}
			}
			if (array2[0] && array2[1] && array2[2] && array2[3])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		return flag;
	}

	public int rand()
	{
		return _SysRandom.Next();
	}

	public void GetLampArray(int[] lampArray)
	{
		if (lampArray.Length >= 24)
		{
			for (int i = 0; i < lampArray.Length; i++)
			{
				_LightArr[i] = lampArray[i];
			}
		}
	}

	public void GetLampArray(ref int[] lampArray)
	{
		int num = rand() % 6;
		int num2 = rand() % 6;
		int num3 = rand() % 6;
		while (num2 == num)
		{
			num2 = rand() % 6;
		}
		while (num3 == num2 || num3 == num)
		{
			num3 = rand() % 6;
		}
		lampArray[4 * num] = 0;
		lampArray[4 * num2] = 1;
		lampArray[4 * num3] = 2;
		for (int i = 0; i < 6; i++)
		{
			if (i != num && i != num2 && i != num3)
			{
				switch (rand() % 3)
				{
				case 0:
					lampArray[4 * i] = 0;
					break;
				case 1:
					lampArray[4 * i] = 1;
					break;
				case 2:
					lampArray[4 * i] = 2;
					break;
				default:
					lampArray[4 * i] = 0;
					break;
				}
			}
		}
		num = rand() % 6;
		num2 = rand() % 6;
		num3 = rand() % 6;
		while (num2 == num)
		{
			num2 = rand() % 6;
		}
		while (num3 == num2 || num3 == num)
		{
			num3 = rand() % 6;
		}
		lampArray[4 * num + 1] = 0;
		lampArray[4 * num2 + 1] = 1;
		lampArray[4 * num3 + 1] = 2;
		for (int j = 0; j < 6; j++)
		{
			if (j != num && j != num2 && j != num3)
			{
				switch (rand() % 3)
				{
				case 0:
					lampArray[4 * j + 1] = 0;
					break;
				case 1:
					lampArray[4 * j + 1] = 1;
					break;
				case 2:
					lampArray[4 * j + 1] = 2;
					break;
				default:
					lampArray[4 * j + 1] = 0;
					break;
				}
			}
		}
		num = rand() % 6;
		num2 = rand() % 6;
		num3 = rand() % 6;
		while (num2 == num)
		{
			num2 = rand() % 6;
		}
		while (num3 == num2 || num3 == num)
		{
			num3 = rand() % 6;
		}
		lampArray[4 * num + 2] = 0;
		lampArray[4 * num2 + 2] = 1;
		lampArray[4 * num3 + 2] = 2;
		for (int k = 0; k < 6; k++)
		{
			if (k != num && k != num2 && k != num3)
			{
				switch (rand() % 3)
				{
				case 0:
					lampArray[4 * k + 2] = 0;
					break;
				case 1:
					lampArray[4 * k + 2] = 1;
					break;
				case 2:
					lampArray[4 * k + 2] = 2;
					break;
				default:
					lampArray[4 * k + 2] = 0;
					break;
				}
			}
		}
		num = rand() % 6;
		num2 = rand() % 6;
		num3 = rand() % 6;
		while (num2 == num)
		{
			num2 = rand() % 6;
		}
		while (num3 == num2 || num3 == num)
		{
			num3 = rand() % 6;
		}
		lampArray[4 * num + 3] = 0;
		lampArray[4 * num2 + 3] = 1;
		lampArray[4 * num3 + 3] = 2;
		for (int l = 0; l < 6; l++)
		{
			if (l != num && l != num2 && l != num3)
			{
				switch (rand() % 3)
				{
				case 0:
					lampArray[4 * l + 3] = 0;
					break;
				case 1:
					lampArray[4 * l + 3] = 1;
					break;
				case 2:
					lampArray[4 * l + 3] = 2;
					break;
				default:
					lampArray[4 * l + 3] = 0;
					break;
				}
			}
		}
	}
}
