using GameCommon;
using System;
using System.Security.Cryptography;
using UnityEngine;

public class STMF_FishGo : MonoBehaviour
{
	public static STMF_FishGo G_FishGo = null;

	private static float[] fRandomBigFishTime = new float[20];

	private static bool[] bIsFishAlreadyGo = new bool[20];

	private static float s_fTime = 0f;

	public static STMF_FishGo GetSingleton()
	{
		return G_FishGo;
	}

	private static int GetRandomSeed()
	{
		byte[] array = new byte[4];
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		rNGCryptoServiceProvider.GetBytes(array);
		return BitConverter.ToInt32(array, 0);
	}

	private void Awake()
	{
		if (G_FishGo == null)
		{
			G_FishGo = this;
		}
		UnityEngine.Random.InitState(GetRandomSeed());
	}

	public STMF_FISH_TYPE CheckTimeForBigFish(float fTime)
	{
		if (fTime == 0f)
		{
			for (int i = 0; i < fRandomBigFishTime.Length; i++)
			{
				fRandomBigFishTime[i] = UnityEngine.Random.Range(60f, 290f);
				bIsFishAlreadyGo[i] = false;
			}
			return STMF_FISH_TYPE.Fish_TYPE_NONE;
		}
		for (int j = 0; j < fRandomBigFishTime.Length; j++)
		{
			if (!bIsFishAlreadyGo[j] && fTime >= fRandomBigFishTime[j])
			{
				bIsFishAlreadyGo[j] = true;
				return (STMF_FISH_TYPE)(j + 16);
			}
		}
		return STMF_FISH_TYPE.Fish_TYPE_NONE;
	}

	public STMF_FISH_TYPE FishOut(out int nFishNumber, float fTime, bool isDeadFish, int nAllFishNumber)
	{
		int num = 70;
		if (!isDeadFish)
		{
			s_fTime += 71f / (678f * (float)Math.PI);
			if (s_fTime < 0.4285f)
			{
				nFishNumber = 0;
				return STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
			s_fTime = 0f;
			if (nAllFishNumber > num)
			{
				nFishNumber = 0;
				return STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		if (nAllFishNumber > num)
		{
			nFishNumber = 0;
			return STMF_FISH_TYPE.Fish_TYPE_NONE;
		}
		int num2 = UnityEngine.Random.Range(0, int.MaxValue) % 1001;
		nFishNumber = 1;
		STMF_FISH_TYPE sTMF_FISH_TYPE;
		if (num2 > 999)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_Turtle;
		}
		else if (num2 > 998)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_Lamp;
		}
		else if (num2 > 996)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_BlueAlgae;
		}
		else if (num2 > 994)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_Hedgehog;
		}
		else if (num2 > 992)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_Ugly;
		}
		else if (num2 > 990)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_YellowSpot;
		}
		else if (num2 > 988)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_BigEars;
		}
		else if (num2 > 986)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_Zebra;
		}
		else if (num2 > 983)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_Grass;
		}
		else if (num2 > 980)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Same_Shrimp;
		}
		else if (num2 > 978)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_LimitedBomb;
		}
		else if (num2 > 976)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_AllBomb;
		}
		else if (num2 > 973)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_CoralReefs;
		}
		else if (num2 > 967)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Bat;
		}
		else if (num2 > 960)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Arrow;
		}
		else if (num2 > 953)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Beauty;
		}
		else if (num2 > 951)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_GoldenShark;
		}
		else if (num2 > 945)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_SilverShark;
		}
		else if (num2 > 943)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_GoldenDragon;
		}
		else if (num2 > 941)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_SilverDragon;
		}
		else if (num2 > 939)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_GreenDragon;
		}
		else if (num2 > 931)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Butterfly;
			float num3 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num3 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num3 >= 0.0833333358f && num3 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num3 >= 11f / 96f && num3 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num3 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 905)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Trailer;
			float num4 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num4 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num4 >= 0.0833333358f && num4 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num4 >= 11f / 96f && num4 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num4 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 874)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Turtle;
			float num5 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num5 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num5 >= 0.0833333358f && num5 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num5 >= 11f / 96f && num5 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num5 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 814)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Lamp;
			float num6 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num6 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num6 >= 0.0833333358f && num6 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num6 >= 11f / 96f && num6 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num6 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 765)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_BlueAlgae;
			float num7 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num7 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num7 >= 0.0833333358f && num7 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num7 >= 11f / 96f && num7 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num7 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 714)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Hedgehog;
			float num8 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num8 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num8 >= 0.0833333358f && num8 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num8 >= 11f / 96f && num8 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num8 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 613)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Ugly;
			float num9 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num9 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num9 >= 0.0833333358f && num9 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num9 >= 11f / 96f && num9 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num9 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 513)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_YellowSpot;
			float num10 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num10 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num10 >= 0.0833333358f && num10 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num10 >= 11f / 96f && num10 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num10 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 459)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_BigEars;
			int num11 = UnityEngine.Random.Range(0, 1001);
			if (num11 < 180)
			{
				nFishNumber = 5;
			}
			else if (num11 > 900)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 > 309)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Zebra;
		}
		else if (num2 > 159)
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Grass;
			int num12 = UnityEngine.Random.Range(0, 1001);
			if (num12 < 30)
			{
				nFishNumber = 5;
			}
			else if (num12 > 30 && num12 <= 60)
			{
				nFishNumber = 5;
			}
			else if (num12 > 60 && num12 <= 90)
			{
				nFishNumber = 5;
			}
			else if (num12 > 500)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num2 <= 9)
		{
			sTMF_FISH_TYPE = ((num2 > 6) ? STMF_FISH_TYPE.Fish_DragonBeauty_Group : ((num2 <= 3) ? STMF_FISH_TYPE.Fish_Knife_Butterfly_Group : STMF_FISH_TYPE.Fish_GoldenArrow_Group));
		}
		else
		{
			sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_Shrimp;
			int num13 = UnityEngine.Random.Range(0, 1001);
			if (num13 < 53)
			{
				nFishNumber = 5;
			}
			else if (num13 >= 53 && num13 < 106)
			{
				nFishNumber = 5;
			}
			else if (num13 >= 106 && num13 < 160)
			{
				nFishNumber = 5;
			}
			else if (num13 > 500)
			{
				nFishNumber = 1;
			}
			else
			{
				sTMF_FISH_TYPE = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		STMF_FISH_TYPE sTMF_FISH_TYPE2 = CheckTimeForBigFish(fTime);
		if (sTMF_FISH_TYPE2 != STMF_FISH_TYPE.Fish_TYPE_NONE)
		{
			sTMF_FISH_TYPE = sTMF_FISH_TYPE2;
		}
		if (sTMF_FISH_TYPE != STMF_FISH_TYPE.Fish_TYPE_NONE && sTMF_FISH_TYPE >= STMF_FISH_TYPE.Fish_GoldenShark && sTMF_FISH_TYPE <= STMF_FISH_TYPE.Fish_Knife_Butterfly_Group)
		{
			bIsFishAlreadyGo[(int)(sTMF_FISH_TYPE - 16)] = true;
		}
		return sTMF_FISH_TYPE;
	}

	public STMF_FISH_TYPE FishOut(out int nFishNumber)
	{
		int num = UnityEngine.Random.Range(0, int.MaxValue) % 1001;
		nFishNumber = 1;
		STMF_FISH_TYPE result;
		if (num > 999)
		{
			result = STMF_FISH_TYPE.Fish_Same_Turtle;
		}
		else if (num > 998)
		{
			result = STMF_FISH_TYPE.Fish_Same_Lamp;
		}
		else if (num > 996)
		{
			result = STMF_FISH_TYPE.Fish_Same_BlueAlgae;
		}
		else if (num > 994)
		{
			result = STMF_FISH_TYPE.Fish_Same_Hedgehog;
		}
		else if (num > 992)
		{
			result = STMF_FISH_TYPE.Fish_Same_Ugly;
		}
		else if (num > 990)
		{
			result = STMF_FISH_TYPE.Fish_Same_YellowSpot;
		}
		else if (num > 988)
		{
			result = STMF_FISH_TYPE.Fish_Same_BigEars;
		}
		else if (num > 986)
		{
			result = STMF_FISH_TYPE.Fish_Same_Zebra;
		}
		else if (num > 983)
		{
			result = STMF_FISH_TYPE.Fish_Same_Grass;
		}
		else if (num > 980)
		{
			result = STMF_FISH_TYPE.Fish_Same_Shrimp;
		}
		else if (num > 978)
		{
			result = STMF_FISH_TYPE.Fish_LimitedBomb;
		}
		else if (num > 976)
		{
			result = STMF_FISH_TYPE.Fish_AllBomb;
		}
		else if (num > 973)
		{
			result = STMF_FISH_TYPE.Fish_CoralReefs;
		}
		else if (num > 967)
		{
			result = STMF_FISH_TYPE.Fish_Bat;
		}
		else if (num > 960)
		{
			result = STMF_FISH_TYPE.Fish_Arrow;
		}
		else if (num > 953)
		{
			result = STMF_FISH_TYPE.Fish_Beauty;
		}
		else if (num > 951)
		{
			result = STMF_FISH_TYPE.Fish_GoldenShark;
		}
		else if (num > 945)
		{
			result = STMF_FISH_TYPE.Fish_SilverShark;
		}
		else if (num > 943)
		{
			result = STMF_FISH_TYPE.Fish_GoldenDragon;
		}
		else if (num > 941)
		{
			result = STMF_FISH_TYPE.Fish_SilverDragon;
		}
		else if (num > 939)
		{
			result = STMF_FISH_TYPE.Fish_GreenDragon;
		}
		else if (num > 931)
		{
			result = STMF_FISH_TYPE.Fish_Butterfly;
			float num2 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num2 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num2 >= 0.0833333358f && num2 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num2 >= 11f / 96f && num2 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num2 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 905)
		{
			result = STMF_FISH_TYPE.Fish_Trailer;
			float num3 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num3 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num3 >= 0.0833333358f && num3 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num3 >= 11f / 96f && num3 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num3 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 874)
		{
			result = STMF_FISH_TYPE.Fish_Turtle;
			float num4 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num4 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num4 >= 0.0833333358f && num4 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num4 >= 11f / 96f && num4 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num4 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 814)
		{
			result = STMF_FISH_TYPE.Fish_Lamp;
			float num5 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num5 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num5 >= 0.0833333358f && num5 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num5 >= 11f / 96f && num5 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num5 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 765)
		{
			result = STMF_FISH_TYPE.Fish_BlueAlgae;
			float num6 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num6 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num6 >= 0.0833333358f && num6 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num6 >= 11f / 96f && num6 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num6 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 714)
		{
			result = STMF_FISH_TYPE.Fish_Hedgehog;
			float num7 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num7 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num7 >= 0.0833333358f && num7 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num7 >= 11f / 96f && num7 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num7 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 613)
		{
			result = STMF_FISH_TYPE.Fish_Ugly;
			float num8 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num8 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num8 >= 0.0833333358f && num8 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num8 >= 11f / 96f && num8 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num8 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 513)
		{
			result = STMF_FISH_TYPE.Fish_YellowSpot;
			float num9 = UnityEngine.Random.Range(0f, 1001f) / 1000f;
			if (num9 < 0.0833333358f)
			{
				nFishNumber = 3;
			}
			else if (num9 >= 0.0833333358f && num9 < 11f / 96f)
			{
				nFishNumber = 4;
			}
			else if (num9 >= 11f / 96f && num9 < 13f / 120f)
			{
				nFishNumber = 5;
			}
			else if (num9 > 0.5f)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 459)
		{
			result = STMF_FISH_TYPE.Fish_BigEars;
			int num10 = UnityEngine.Random.Range(0, 1001);
			if (num10 < 180)
			{
				nFishNumber = 5;
			}
			else if (num10 > 900)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num > 309)
		{
			result = STMF_FISH_TYPE.Fish_Zebra;
		}
		else if (num > 159)
		{
			result = STMF_FISH_TYPE.Fish_Grass;
			int num11 = UnityEngine.Random.Range(0, 1001);
			if (num11 < 30)
			{
				nFishNumber = 5;
			}
			else if (num11 > 30 && num11 <= 60)
			{
				nFishNumber = 5;
			}
			else if (num11 > 60 && num11 <= 90)
			{
				nFishNumber = 5;
			}
			else if (num11 > 500)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else if (num <= 9)
		{
			result = ((num > 6) ? STMF_FISH_TYPE.Fish_DragonBeauty_Group : ((num <= 3) ? STMF_FISH_TYPE.Fish_Knife_Butterfly_Group : STMF_FISH_TYPE.Fish_GoldenArrow_Group));
		}
		else
		{
			result = STMF_FISH_TYPE.Fish_Shrimp;
			int num12 = UnityEngine.Random.Range(0, 1001);
			if (num12 < 53)
			{
				nFishNumber = 5;
			}
			else if (num12 >= 53 && num12 < 106)
			{
				nFishNumber = 5;
			}
			else if (num12 >= 106 && num12 < 160)
			{
				nFishNumber = 5;
			}
			else if (num12 > 500)
			{
				nFishNumber = 1;
			}
			else
			{
				result = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		return result;
	}
}
