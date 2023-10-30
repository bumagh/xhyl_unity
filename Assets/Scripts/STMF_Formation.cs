using GameCommon;
using System;
using UnityEngine;

public class STMF_Formation : MonoBehaviour
{
	private const int HEIGHT = 768;

	private const int WEIGHT = 1366;

	public static STMF_Formation G_Formation;

	private STMF_FISH_TYPE FishType;

	private STMF_FORMATION FormationType;

	private Vector3 rotVector;

	public static STMF_Formation GetSingleton()
	{
		return G_Formation;
	}

	private void Awake()
	{
		if (G_Formation == null)
		{
			G_Formation = this;
		}
	}

	private void CreateAFish(STMF_FORMATION formationType, float x, float y, float rot, STMF_FISH_TYPE FishType, int i, int index = 0)
	{
		Transform transform = STMF_FishPoolMngr.GetSingleton().CreateFishForBig(FishType, i);
		rotVector = transform.gameObject.transform.eulerAngles;
		rotVector.x = rot / (float)Math.PI * 180f;
		transform.gameObject.transform.eulerAngles = rotVector;
		if (FormationType == STMF_FORMATION.Formation_RedCarpet)
		{
			if (FishType == STMF_FISH_TYPE.Fish_Knife_Butterfly_Group)
			{
				transform.GetComponent<STMF_GroupFish>().InitNormalFish(FormationType, x, y, rot, index, FishType);
				return;
			}
			transform.GetComponent<STMF_NormalFish>().InitNormalFish(FormationType, x, y, rot, index, FishType);
			transform.GetComponent<STMF_NormalFish>().StartMove(move: true);
		}
		else if (FishType == STMF_FISH_TYPE.Fish_Knife_Butterfly_Group)
		{
			transform.GetComponent<STMF_GroupFish>().InitNormalFish(FormationType, x, y, rot, i, FishType);
		}
		else
		{
			transform.GetComponent<STMF_NormalFish>().InitNormalFish(FormationType, x, y, rot, i, FishType);
			transform.GetComponent<STMF_NormalFish>().StartMove(move: true);
		}
	}

	public void ShowFormation(STMF_FORMATION type)
	{
		switch (type)
		{
		case STMF_FORMATION.Formation_BigFishes:
			CreateFormationBigFishes();
			break;
		case STMF_FORMATION.Formation_RedCarpet:
			CreateFormationRedCarpet();
			break;
		case STMF_FORMATION.Formation_YaoQianShuL:
			CreateYaoQianShu(0);
			break;
		case STMF_FORMATION.Formation_YaoQianShuR:
			CreateYaoQianShu(1);
			break;
		}
	}

	private void CreateFormationBigFishes()
	{
		float num = 650f;
		float num2 = 350f;
		float num3 = -200f;
		for (int i = 0; i < 64; i++)
		{
			float rot;
			float num4;
			float num5;
			if (i < 8)
			{
				FishType = STMF_FISH_TYPE.Fish_SilverDragon;
				num4 = num3 - num - (float)(i * 600 * 1366 / 1567);
				num5 = 750f - num2 + (float)(i * 600 * 768 / 1567);
				rot = Mathf.Acos(0.871729434f);
			}
			else if (i < 16)
			{
				FishType = STMF_FISH_TYPE.Fish_Beauty;
				num4 = num3 - num + 150f - (float)((i - 8) * 600 * 1366 / 1567);
				num5 = 900f - num2 + (float)((i - 8) * 600 * 768 / 1567);
				rot = Mathf.Acos(0.871729434f);
			}
			else if (i < 24)
			{
				FishType = STMF_FISH_TYPE.Fish_Knife_Butterfly_Group;
				num4 = num3 - num - (float)((i - 16) * 600 * 1366 / 1567);
				num5 = -412.44f - (float)((i - 16) * 600 * 768 / 1567);
				rot = 0f - Mathf.Acos(0.871729434f);
			}
			else if (i < 32)
			{
				FishType = STMF_FISH_TYPE.Fish_Butterfly;
				num4 = num3 - num + 160f - (float)((i - 24) * 600 * 1366 / 1567);
				num5 = -650 - (i - 24) * 600 * 768 / 1567;
				rot = 0f - Mathf.Acos(0.871729434f);
			}
			else if (i < 40)
			{
				FishType = STMF_FISH_TYPE.Fish_Arrow;
				num4 = 1466f - num + (float)((i - 32) * 600 * 1366 / 1567);
				num5 = -550 - (i - 32) * 600 * 768 / 1567;
				rot = -(float)Math.PI + Mathf.Acos(0.871729434f);
			}
			else if (i < 48)
			{
				FishType = STMF_FISH_TYPE.Fish_GoldenShark;
				num4 = 1566f - num + (float)((i - 40) * 600 * 1366 / 1567);
				num5 = -400 - (i - 40) * 600 * 768 / 1567;
				rot = -(float)Math.PI + Mathf.Acos(0.871729434f);
			}
			else if (i < 56)
			{
				FishType = STMF_FISH_TYPE.Fish_SilverShark;
				num4 = 1616f - num + (float)((i - 48) * 600 * 1366 / 1567);
				num5 = 800f - num2 + (float)((i - 48) * 600 * 768 / 1567);
				rot = (float)Math.PI - Mathf.Acos(0.871729434f);
			}
			else
			{
				FishType = STMF_FISH_TYPE.Fish_Bat;
				num4 = 1466f - num + (float)((i - 56) * 600 * 1366 / 1567);
				num5 = 950f - num2 + (float)((i - 56) * 600 * 768 / 1567);
				rot = (float)Math.PI - Mathf.Acos(0.871729434f);
			}
			num4 /= 100f;
			num5 /= 100f;
			FormationType = STMF_FORMATION.Formation_BigFishes;
			CreateAFish(FormationType, num4, num5, rot, FishType, i);
		}
	}

	private void CreateFormationRedCarpet()
	{
		for (int i = 0; i < 160; i++)
		{
			FishType = STMF_FISH_TYPE.Fish_Shrimp;
			float rot;
			int index;
			float num;
			float num2;
			if (i < 80)
			{
				if (i % 2 == 0)
				{
					num = -620f + (float)i * 1200f / 80f + (float)i;
					num2 = -350f - (float)(i % 6) * 200f / 6f - (float)i;
				}
				else
				{
					num = -620f + (float)i * 1200f / 80f - (float)i;
					num2 = -350f - (float)(i % 6) * 200f / 6f + (float)i;
				}
				rot = -(float)Math.PI / 2f;
				index = i;
			}
			else
			{
				if (i % 2 == 0)
				{
					num = -620f + (float)(i - 80) * 1200f / 80f - (float)(i - 80);
					num2 = 350f + (float)((i - 80) % 6) * 200f / 6f + (float)(i - 80);
				}
				else
				{
					num = -620f + (float)(i - 80) * 1200f / 80f + (float)(i - 80);
					num2 = 350f + (float)((i - 80) % 6) * 200f / 6f - (float)(i - 80);
				}
				rot = (float)Math.PI / 2f;
				index = i - 80;
			}
			num /= 100f;
			num2 /= 100f;
			int i2 = i;
			FormationType = STMF_FORMATION.Formation_RedCarpet;
			CreateAFish(FormationType, num, num2, rot, FishType, i2, index);
		}
		for (int j = 0; j < 22; j++)
		{
			float rot2;
			float num4;
			float num5;
			if (j < 11)
			{
				float num3 = -700f;
				if (j < 3)
				{
					FishType = STMF_FISH_TYPE.Fish_Bat;
					num4 = num3 - (float)(j * 300);
				}
				else if (j < 6)
				{
					FishType = STMF_FISH_TYPE.Fish_SilverShark;
					num4 = num3 - 900f - (float)((j - 3) * 400);
				}
				else if (j < 9)
				{
					FishType = STMF_FISH_TYPE.Fish_GoldenShark;
					num4 = num3 - 900f - 1200f - (float)((j - 6) * 400);
				}
				else if (j < 10)
				{
					FishType = STMF_FISH_TYPE.Fish_Knife_Butterfly_Group;
					num4 = num3 - 900f - 1200f - 1200f - 100f;
				}
				else
				{
					FishType = STMF_FISH_TYPE.Fish_SilverDragon;
					num4 = num3 - 900f - 1200f - 1200f - 100f - 500f;
				}
				num5 = -120f;
				rot2 = 0f;
			}
			else
			{
				float num6 = 700f;
				if (j < 14)
				{
					FishType = STMF_FISH_TYPE.Fish_Bat;
					num4 = num6 + (float)((j - 11) * 300);
				}
				else if (j < 17)
				{
					FishType = STMF_FISH_TYPE.Fish_SilverShark;
					num4 = num6 + 900f + (float)((j - 14) * 400);
				}
				else if (j < 20)
				{
					FishType = STMF_FISH_TYPE.Fish_GoldenShark;
					num4 = num6 + 900f + 1200f + (float)((j - 17) * 400);
				}
				else if (j < 21)
				{
					FishType = STMF_FISH_TYPE.Fish_Knife_Butterfly_Group;
					num4 = num6 + 900f + 1200f + 1200f + 100f;
				}
				else
				{
					FishType = STMF_FISH_TYPE.Fish_SilverDragon;
					num4 = num6 + 900f + 1200f + 1200f + 100f + 500f;
				}
				num5 = 120f;
				rot2 = (float)Math.PI;
			}
			num4 /= 100f;
			num5 /= 100f;
			int i3 = 160 + j;
			FormationType = STMF_FORMATION.Formation_RedCarpet;
			CreateAFish(FormationType, num4, num5, rot2, FishType, i3);
		}
	}

	private void CreateYaoQianShu(int left)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float rot = 0f;
		switch (left)
		{
		case 0:
			rot = 0f;
			left = 1;
			num3 = -1150f;
			break;
		case 1:
			rot = (float)Math.PI;
			left = -1;
			num3 = 720f;
			break;
		}
		FishType = STMF_FISH_TYPE.Fish_Shrimp;
		for (int i = 0; i < 149; i++)
		{
			float num4 = num3;
			float num5 = 100f;
			if (i < 10)
			{
				num = num4 + (float)(i * 18);
				num2 = num5 - 20f;
			}
			else if (i < 32)
			{
				num = num4 + 120f;
				num2 = num5 - 20f + 50f - (float)((i - 10) * 12);
			}
			else if (i < 34)
			{
				num = num4 + 120f - 30f - (float)((i - 32) * 25);
				num2 = num5 - 20f + 50f - 240f - 5f + (float)((i - 32) * 20);
			}
			else if (i < 50)
			{
				num = num4 + 162f - (float)((i - 34) * 10) * Mathf.Cos((float)Math.PI / 5f);
				num2 = num5 - 20f - 30f - (float)((i - 34) * 10) * Mathf.Sin((float)Math.PI / 5f);
			}
			else if (i < 57)
			{
				num = num4 + 250f + (float)((i - 50) * 18) * Mathf.Cos((float)Math.PI / 18f);
				num2 = num5 + 40f + (float)((i - 50) * 10) * Mathf.Sin((float)Math.PI / 18f);
			}
			else if (i < 65)
			{
				num = num4 + 250f + 54f;
				num2 = num5 + 35f - (float)((i - 57) * 8);
			}
			else if (i < 74)
			{
				num = num4 + 250f - 18f + (float)((i - 65) * 18) * Mathf.Cos(1.30899692f);
				num2 = num5 + 40f - (float)((i - 65) * 8) * Mathf.Sin(1.30899692f);
			}
			else if (i < 82)
			{
				num = num4 + 250f + 126f * Mathf.Cos((float)Math.PI / 18f) - (float)((i - 74) * 18) * Mathf.Cos((float)Math.PI * 14f / 33f);
				num2 = num5 + 40f - 5f - (float)((i - 74) * 8) * Mathf.Sin((float)Math.PI * 14f / 33f);
			}
			else if (i < 92)
			{
				num = num4 + 250f - (float)((i - 82) * 12) * Mathf.Cos(1.30899692f);
				num2 = num5 - 50f - (float)((i - 82) * 8) * Mathf.Sin(1.30899692f);
			}
			else if (i < 98)
			{
				num = num4 + 280f + (float)((i - 92) * 18);
				num2 = num5 - 50f;
			}
			else if (i < 105)
			{
				num = num4 + 280f - 12f + (float)((i - 98) * 18);
				num2 = num5 - 100f;
			}
			else if (i < 125)
			{
				num = num4 + 250f + 54f;
				num2 = num5 - 50f - (float)((i - 105) * 8);
			}
			else if (i < 133)
			{
				num = num4 + 250f + (float)((i - 125) * 18);
				num2 = num5 - 50f - 160f;
			}
			else if (i < 141)
			{
				num = num4 + 250f - 28f;
				num2 = num5 - 50f - 160f + 40f - (float)((i - 133) * 8);
			}
			else if (i < 149)
			{
				num = num4 + 250f + 108f + 28f;
				num2 = num5 - 50f - 160f + 40f - (float)((i - 141) * 8);
			}
			num /= 100f;
			num2 /= 100f;
			int i2 = i;
			FormationType = STMF_FORMATION.Formation_YaoQianShuL;
			CreateAFish(FormationType, num, num2, rot, FishType, i2);
		}
		for (int j = 0; j < 94; j++)
		{
			float num6 = 0f;
			switch (left)
			{
			case 1:
				num6 = -350f;
				break;
			case -1:
				num6 = 620f;
				break;
			}
			num6 += num3;
			float num7 = 120f;
			if (j < 10)
			{
				num = num6 - (float)(j * 15) * Mathf.Cos((float)Math.PI / 3f);
				num2 = num7 - (float)(j * 15) * Mathf.Sin((float)Math.PI / 3f);
			}
			else if (j < 18)
			{
				num = num6 + 90f + (float)((j - 10) * 18);
				num2 = num7 - 120f * Mathf.Sin((float)Math.PI / 3f);
			}
			else if (j < 28)
			{
				num = num6 + 25f + (float)((j - 18) * 18) * Mathf.Cos((float)Math.PI / 3f);
				num2 = num7 - (float)((j - 18) * 15) * Mathf.Sin((float)Math.PI / 3f);
			}
			else if (j < 33)
			{
				num = num6 - 10f + (float)((j - 28) * 12);
				num2 = num7 - 75f * Mathf.Sin((float)Math.PI / 3f);
			}
			else if (j < 38)
			{
				num = num6 - 10f + (float)((j - 33) * 12);
				num2 = num7 - 135f * Mathf.Sin((float)Math.PI / 3f);
			}
			else if (j < 41)
			{
				num = num6 + 30f + (float)((j - 38) * 20);
				num2 = num7 - 75f * Mathf.Sin((float)Math.PI / 3f) - 195f + 15f + (float)((j - 38) * 15);
			}
			else if (j < 57)
			{
				num = num6 + 20f;
				num2 = num7 - 75f * Mathf.Sin((float)Math.PI / 3f) - (float)((j - 41) * 13);
			}
			else if (j < 62)
			{
				num = num6 + 120f + (float)((j - 57) * 18);
				num2 = num7 - 60f * Mathf.Sin((float)Math.PI / 3f);
			}
			else if (j < 82)
			{
				num = num6 + 125f + (float)((j - 62) * 15) * Mathf.Cos(1.30899692f);
				num2 = num7 + 30f * Mathf.Sin((float)Math.PI / 3f) - (float)((j - 62) * 15) * Mathf.Sin(1.30899692f);
			}
			else if (j < 84)
			{
				num = num6 + 170f + (float)((j - 82) * 18);
				num2 = num7 + 30f * Mathf.Sin((float)Math.PI / 3f) - (float)((j - 82) * 20);
			}
			else if (j < 94)
			{
				num = num6 + 60f + 144f - (float)((j - 84) * 15) * Mathf.Cos((float)Math.PI / 3f);
				num2 = num7 - 150f * Mathf.Sin((float)Math.PI / 3f) - (float)((j - 84) * 15) * Mathf.Sin((float)Math.PI / 3f);
			}
			num /= 100f;
			num2 /= 100f;
			int i3 = j + 149;
			FormationType = STMF_FORMATION.Formation_YaoQianShuL;
			CreateAFish(FormationType, num, num2, rot, FishType, i3);
		}
		for (int k = 0; k < 97; k++)
		{
			float num8 = -950f;
			float num9 = 70f;
			switch (left)
			{
			case 1:
				num8 = -950f;
				break;
			case -1:
				num8 = 1020f;
				break;
			}
			num8 += num3;
			if (k < 10)
			{
				num = num8 + (float)(k * 12);
				num2 = num9;
			}
			else if (k < 20)
			{
				num = num8 + 48f - (float)((k - 10) * 15) * Mathf.Cos((float)Math.PI / 3f);
				num2 = num9 - (float)((k - 10) * 15) * Mathf.Sin((float)Math.PI / 3f);
			}
			else if (k < 40)
			{
				num = num8 + 60f;
				num2 = num9 + 60f - (float)((k - 20) * 15);
			}
			else if (k < 50)
			{
				num = num8 + 60f + (float)((k - 40) * 15) * Mathf.Cos((float)Math.PI / 3f);
				num2 = num9 - (float)((k - 40) * 15) * Mathf.Sin((float)Math.PI / 3f);
			}
			else if (k < 55)
			{
				num = num8 + 150f + 75f - (float)((k - 50) * 20) * Mathf.Cos((float)Math.PI / 5f);
				num2 = num9 - 30f - (float)((k - 50) * 25) * Mathf.Sin((float)Math.PI / 5f);
			}
			else if (k < 60)
			{
				num = num8 + 150f + 15f + (float)((k - 55) * 15);
				num2 = num9 - 30f;
			}
			else if (k < 65)
			{
				num = num8 + 150f + (float)((k - 60) * 20) * Mathf.Cos((float)Math.PI / 5f);
				num2 = num9 - 30f - (float)((k - 60) * 25) * Mathf.Sin((float)Math.PI / 5f);
			}
			else if (k < 73)
			{
				num = num8 + 160f + 120f + (float)((k - 65) * 15);
				num2 = num9;
			}
			else if (k < 93)
			{
				num = num8 + 160f + 25f + 75f + 105f;
				num2 = num9 + 60f - (float)((k - 73) * 15);
			}
			else if (k < 95)
			{
				num = num8 + 160f + 25f + 75f + 60f - (float)((k - 93) * 20);
				num2 = num9 + 60f - 285f + (float)((k - 93) * 15);
			}
			else if (k < 97)
			{
				num = num8 + 160f + 75f + 45f + (float)((k - 95) * 25);
				num2 = num9 + 60f - 150f + 8f - (float)((k - 95) * 20);
			}
			num /= 100f;
			num2 /= 100f;
			int i4 = k + 149 + 94;
			FormationType = STMF_FORMATION.Formation_YaoQianShuL;
			CreateAFish(FormationType, num, num2, rot, FishType, i4);
		}
		for (int l = 0; l < 6; l++)
		{
			if (l < 2)
			{
				num = 240f + num3;
				num2 = 250 - l * 500;
				FishType = STMF_FISH_TYPE.Fish_GoldenDragon;
			}
			else if (l < 4)
			{
				num = (float)(240 - 500 * left) + num3;
				num2 = 250 - (l - 2) * 500;
				FishType = STMF_FISH_TYPE.Fish_SilverDragon;
			}
			else if (l < 6)
			{
				num = (float)(240 - 500 * left - 500 * left) + num3;
				num2 = 250 - (l - 4) * 500;
				FishType = STMF_FISH_TYPE.Fish_GoldenShark;
			}
			num /= 100f;
			num2 /= 100f;
			int i5 = l + 149 + 94 + 97;
			FormationType = STMF_FORMATION.Formation_YaoQianShuL;
			CreateAFish(FormationType, num, num2, rot, FishType, i5);
		}
	}

	public void ReleaseAFish(GameObject fish)
	{
		STMF_FishPoolMngr.GetSingleton().DestroyFish(fish);
	}
}
