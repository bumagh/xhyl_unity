using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHD_UpdateLuDan : MonoBehaviour
{
	public enum LuDanType
	{
		珠盘路,
		大路,
		大眼路,
		小眼路,
		曱甴路
	}

	public LuDanType ludanType;

	public GameObject LuDanObject;

	public int lastNum;

	private int posy;

	private int posx;

	private List<int> reslut = new List<int>();

	private int posx_DLu;

	private int posy_DLu;

	private List<int> reslut_DLu = new List<int>();

	private void Awake()
	{
		LuDanObject = base.gameObject;
	}

	private void OnEnable()
	{
		LHD_GameInfo instance = LHD_GameInfo.getInstance();
		instance.updateLuDan = (Action)Delegate.Combine(instance.updateLuDan, new Action(UpdateLuDan));
	}

	private void OnDisable()
	{
		LHD_GameInfo instance = LHD_GameInfo.getInstance();
		instance.updateLuDan = (Action)Delegate.Remove(instance.updateLuDan, new Action(UpdateLuDan));
	}

	private void Update()
	{
	}

	public void UpdateLuDan()
	{
		for (int i = 0; i < LuDanObject.transform.childCount; i++)
		{
			LuDanObject.transform.GetChild(i).gameObject.SetActive(value: false);
		}
		switch (ludanType)
		{
		case LuDanType.珠盘路:
			if (LuDanObject.transform.childCount >= LHD_LuDanManager.instance.LuDan1_NowCount)
			{
				for (int m = 0; m < LuDanObject.transform.childCount; m++)
				{
					if (m < LHD_LuDanManager.instance.LuDan1_NowCount)
					{
						LuDanObject.transform.GetChild(m).gameObject.SetActive(value: true);
						LuDanObject.transform.GetChild(m).GetComponent<Image>().sprite = LHD_LuDanManager.instance.珠盘路Sprites[LHD_LuDanManager.instance.Ludan1[m]];
					}
					else
					{
						LuDanObject.transform.GetChild(m).gameObject.SetActive(value: false);
					}
				}
			}
			else
			{
				LHD_LuDanManager.instance.Ludan1.Remove(LHD_LuDanManager.instance.Ludan1[0]);
				UpdateLuDan();
			}
			break;
		case LuDanType.大路:
		{
			int crosswisNum3 = 0;
			GridLayoutGroup component3 = GetComponent<GridLayoutGroup>();
			if (component3 != null)
			{
				crosswisNum3 = component3.constraintCount;
			}
			posy_DLu = 0;
			posx_DLu = 0;
			reslut_DLu = new List<int>();
			for (int l = 0; l < LHD_LuDanManager.instance.LuDan2_NowCount; l++)
			{
				AddLuDan_DLu(LHD_LuDanManager.instance.Ludan2[l], base.transform, crosswisNum3, LHD_LuDanManager.instance.大路Sprites);
			}
			break;
		}
		case LuDanType.大眼路:
		{
			int crosswisNum2 = 0;
			GridLayoutGroup component2 = GetComponent<GridLayoutGroup>();
			if (component2 != null)
			{
				crosswisNum2 = component2.constraintCount;
			}
			posy = 0;
			posx = 0;
			reslut = new List<int>();
			for (int k = 0; k < LHD_LuDanManager.instance.LuDan3_NowCount; k++)
			{
				AddDaLuLuDan(LHD_LuDanManager.instance.Ludan3[k], base.transform, crosswisNum2, LHD_LuDanManager.instance.大眼路Sprites);
			}
			break;
		}
		case LuDanType.小眼路:
		{
			int crosswisNum4 = 0;
			GridLayoutGroup component4 = GetComponent<GridLayoutGroup>();
			if (component4 != null)
			{
				crosswisNum4 = component4.constraintCount;
			}
			posy = 0;
			posx = 0;
			reslut = new List<int>();
			for (int n = 0; n < LHD_LuDanManager.instance.LuDan4_NowCount; n++)
			{
				AddDaLuLuDan(LHD_LuDanManager.instance.Ludan4[n], base.transform, crosswisNum4, LHD_LuDanManager.instance.小眼路Sprites);
			}
			break;
		}
		case LuDanType.曱甴路:
		{
			int crosswisNum = 0;
			GridLayoutGroup component = GetComponent<GridLayoutGroup>();
			if (component != null)
			{
				crosswisNum = component.constraintCount;
			}
			posy = 0;
			posx = 0;
			reslut = new List<int>();
			for (int j = 0; j < LHD_LuDanManager.instance.LuDan5_NowCount; j++)
			{
				AddDaLuLuDan(LHD_LuDanManager.instance.Ludan5[j], base.transform, crosswisNum, LHD_LuDanManager.instance.曱甴路Sprites);
			}
			break;
		}
		}
	}

	private void AddDaLuLuDan(int num, Transform luDanTr, int crosswisNum, Sprite[] sprites)
	{
		reslut.Add(num);
		int count = reslut.Count;
		if (count < 2)
		{
			posy = 0;
			posx = 0;
		}
		else if (count >= 2 && (reslut[count - 1] == reslut[count - 2] || reslut[count - 1] == 1 || (count >= 3 && reslut[count - 2] == 1 && reslut[count - 3] == reslut[count - 1])))
		{
			posy++;
			if (posy >= 6)
			{
				posy = 0;
				posx++;
			}
		}
		else
		{
			posy = 0;
			posx++;
			if (posx >= crosswisNum)
			{
				List<int> list = new List<int>();
				int num2 = -1;
				if (reslut.Count >= 1)
				{
					num2 = reslut[0];
				}
				for (int i = 0; i < reslut.Count; i++)
				{
					if (num2 == -1)
					{
						break;
					}
					if (num2 != reslut[i] && reslut[i] != 1)
					{
						break;
					}
					reslut.Remove(reslut[i]);
				}
				for (int j = 0; j < reslut.Count; j++)
				{
					list.Add(reslut[j]);
				}
				reslut = new List<int>();
				posy = 0;
				posx = 0;
				for (int k = 0; k < luDanTr.childCount; k++)
				{
					luDanTr.GetChild(k).gameObject.SetActive(value: false);
				}
				for (int l = 0; l < list.Count; l++)
				{
					AddDaLuLuDan(list[l], luDanTr, crosswisNum, sprites);
				}
				return;
			}
		}
		int num3 = posx + posy * crosswisNum;
		if (num3 < luDanTr.childCount)
		{
			int latsIndex = 0;
			try
			{
				latsIndex = ((count <= 1 || reslut.Count < 1) ? num : reslut[count - 2]);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("e: " + arg);
			}
			luDanTr.GetChild(num3).GetComponent<Image>().sprite = GetSprite(latsIndex, num, sprites);
			luDanTr.GetChild(num3).gameObject.SetActive(value: true);
		}
		else
		{
			UnityEngine.Debug.LogError("====未处理: " + num3);
		}
	}

	private Sprite GetSprite(int latsIndex, int index, Sprite[] sprites)
	{
		int num = 0;
		switch (index)
		{
		case 0:
			num = 0;
			break;
		case 2:
			num = 1;
			break;
		default:
			num = ((latsIndex != 0) ? 3 : 2);
			break;
		}
		return sprites[num];
	}

	private void AddLuDan_DLu(int num, Transform luDanTr, int crosswisNum, Sprite[] sprites)
	{
		reslut_DLu.Add(num);
		int count = reslut_DLu.Count;
		if (count >= 2 && reslut_DLu[count - 1] == 1)
		{
			switch (reslut_DLu[count - 2])
			{
			case 0:
				reslut_DLu[count - 2] = -1;
				break;
			case 2:
				reslut_DLu[count - 2] = -2;
				break;
			}
		}
		if (count >= 2 && reslut_DLu[count - 1] == 1 && reslut_DLu[count - 2] == 1)
		{
			reslut_DLu.Remove(num);
			return;
		}
		count = reslut_DLu.Count;
		if (count < 2)
		{
			posy_DLu = 0;
			posx_DLu = 0;
		}
		else if ((count >= 2 && reslut_DLu[count - 2] >= 0 && (reslut_DLu[count - 1] == reslut_DLu[count - 2] || (count >= 3 && IsHe(reslut_DLu[count - 2]) && reslut_DLu[count - 3] == reslut_DLu[count - 1]) || (count >= 3 && reslut_DLu[count - 1] == 2 && reslut_DLu[count - 3] == -2) || (count >= 3 && reslut_DLu[count - 1] == 0 && reslut_DLu[count - 3] == -1))) || (count >= 3 && reslut_DLu[count - 1] == 1 && reslut_DLu[count - 3] == 2 && reslut_DLu[count - 2] == -2) || (count >= 3 && reslut_DLu[count - 1] == 1 && reslut_DLu[count - 3] == 1 && reslut_DLu[count - 2] == -1))
		{
			posy_DLu++;
			if (posy_DLu >= 6)
			{
				posy_DLu = 0;
				posx_DLu++;
			}
		}
		else if ((count < 2 || reslut_DLu[count - 2] >= 0) && (count < 3 || reslut_DLu[count - 2] >= 0 || (reslut_DLu[count - 3] != reslut_DLu[count - 1] && (reslut_DLu[count - 3] != -1 || reslut_DLu[count - 1] != 0) && (reslut_DLu[count - 3] != -2 || reslut_DLu[count - 1] != 2) && (reslut_DLu[count - 3] != 1 || reslut_DLu[count - 1] != 1))))
		{
			if (count == 2 && IsHe(reslut_DLu[count - 2]))
			{
				posy_DLu++;
				if (posy_DLu >= 6)
				{
					posy_DLu = 0;
					posx_DLu++;
				}
			}
			else
			{
				posy_DLu = 0;
				posx_DLu++;
				if (posx_DLu >= crosswisNum)
				{
					List<int> list = new List<int>();
					int num2 = -100;
					if (reslut_DLu.Count >= 1)
					{
						num2 = reslut_DLu[0];
					}
					for (int i = 0; i < reslut_DLu.Count; i++)
					{
						if (num2 == -100)
						{
							break;
						}
						if (num2 != reslut_DLu[i] && !IsHe(reslut_DLu[i]) && (num2 != -2 || reslut_DLu[i] == 0) && (num2 != -1 || reslut_DLu[i] == 2))
						{
							break;
						}
						reslut_DLu[i] = -1000000;
					}
					for (int j = 0; j < reslut_DLu.Count; j++)
					{
						if (reslut_DLu[j] != -1000000)
						{
							list.Add(reslut_DLu[j]);
						}
					}
					reslut_DLu = new List<int>();
					posy_DLu = 0;
					posx_DLu = 0;
					for (int k = 0; k < luDanTr.childCount; k++)
					{
						luDanTr.GetChild(k).gameObject.SetActive(value: false);
					}
					for (int l = 0; l < list.Count; l++)
					{
						AddLuDan_DLu(list[l], luDanTr, crosswisNum, sprites);
					}
					return;
				}
			}
		}
		int num3 = posx_DLu + posy_DLu * crosswisNum;
		if (num3 < luDanTr.childCount)
		{
			int latsIndex = 0;
			num = reslut_DLu[count - 1];
			try
			{
				latsIndex = ((count <= 1 || reslut_DLu.Count < 1) ? num : reslut_DLu[count - 2]);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("e: " + arg);
			}
			luDanTr.GetChild(num3).GetComponent<Image>().sprite = GetSprite_DLu(latsIndex, num, sprites);
			luDanTr.GetChild(num3).gameObject.SetActive(value: true);
		}
		else
		{
			UnityEngine.Debug.LogError("====未处理: " + num3);
		}
	}

	private bool IsHe(int num)
	{
		if (num == 1 || num < 0)
		{
			return true;
		}
		return false;
	}

	private Sprite GetSprite_DLu(int latsIndex, int index, Sprite[] sprites)
	{
		int num = 0;
		switch (index)
		{
		case 0:
			num = 0;
			break;
		case 2:
			num = 1;
			break;
		default:
			num = ((latsIndex != 0 && latsIndex != -1) ? 3 : 2);
			break;
		}
		return sprites[num];
	}
}
