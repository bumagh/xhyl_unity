using GameCommon;
using System;
using System.Collections;
using UnityEngine;

public class DNTG_Formation : MonoBehaviour
{
	private const int HEIGHT = 768;

	private const int WEIGHT = 1366;

	private float totalTime;

	private int mHexagonalIndex;

	private bool ShowFormationHex;

	private float[] arr = new float[336]
	{
		-0.07016753f,
		0.1470301f,
		0.2974815f,
		0.2477935f,
		-0.08694784f,
		-0.3125716f,
		0.2342839f,
		0.01400278f,
		-587f / (609f * (float)Math.PI),
		-0.1067391f,
		-0.1984341f,
		0.1786483f,
		0.01829441f,
		0.1204441f,
		-0.3104365f,
		0.214666f,
		-0.07531048f,
		-0.2690267f,
		-0.1286234f,
		0.1188236f,
		0.2563104f,
		0.04943924f,
		0.2176594f,
		0.08831189f,
		0.1625734f,
		-0.247525f,
		-0.005613149f,
		0.04707819f,
		0.2187641f,
		-0.1875107f,
		0.1717521f,
		0.2955899f,
		0.16437f,
		-0.140052f,
		0.08865403f,
		0.2377331f,
		-0.2420434f,
		-0.004326867f,
		-0.04956147f,
		0.1882785f,
		-0.05301615f,
		0.3000908f,
		-0.2974265f,
		-0.2968024f,
		0.08475886f,
		-0.289977f,
		0.2943893f,
		-0.07240812f,
		-0.1023849f,
		-0.03870017f,
		-0.02209093f,
		-0.1710741f,
		0.02665752f,
		-0.1199554f,
		0.2379791f,
		-0.2149891f,
		0.2849169f,
		-0.1207748f,
		0.1239818f,
		0.2294074f,
		0.0989732f,
		0.1529666f,
		-0.1166208f,
		-0.2911412f,
		0.2974453f,
		0.1146155f,
		-0.1120537f,
		0.06912041f,
		-0.07836653f,
		-0.2477419f,
		-0.06909014f,
		0.2555821f,
		-0.2295429f,
		0.249578f,
		0.1716784f,
		0.1331692f,
		0.1414086f,
		-0.07870726f,
		0.3114465f,
		0.155453f,
		0.05647517f,
		0.02120163f,
		0.04604493f,
		0.1374719f,
		-0.2229169f,
		-0.07761408f,
		-0.1978485f,
		-0.1757867f,
		-0.1857033f,
		0.2433183f,
		0.2020769f,
		0.2733218f,
		-0.1444006f,
		0.06842007f,
		-0.1180121f,
		-0.3036411f,
		-0.2071026f,
		-0.09561521f,
		0.09780467f,
		-0.2592242f,
		-0.02847838f,
		0.1534369f,
		-0.2965503f,
		0.1471394f,
		0.2678051f,
		0.2709511f,
		-0.2912002f,
		0.2653232f,
		0.05564212f,
		0.1029283f,
		-0.1925138f,
		0.09087703f,
		-0.04890646f,
		0.1457155f,
		0.03171037f,
		0.05102445f,
		0.1177287f,
		0.06805613f,
		-0.2883594f,
		0.1712627f,
		-0.07016753f,
		0.1470301f,
		0.2974815f,
		0.2477935f,
		-0.08694784f,
		-0.3125716f,
		0.2342839f,
		0.01400278f,
		-587f / (609f * (float)Math.PI),
		-0.1067391f,
		-0.1984341f,
		0.1786483f,
		0.01829441f,
		0.1204441f,
		-0.3104365f,
		0.214666f,
		-0.07531048f,
		-0.2690267f,
		-0.1286234f,
		0.1188236f,
		0.2563104f,
		0.04943924f,
		0.2176594f,
		0.08831189f,
		0.1625734f,
		-0.247525f,
		-0.005613149f,
		0.04707819f,
		0.2187641f,
		-0.1875107f,
		0.1717521f,
		0.2955899f,
		0.16437f,
		-0.140052f,
		0.08865403f,
		0.2377331f,
		-0.2420434f,
		-0.004326867f,
		-0.04956147f,
		0.1882785f,
		-0.05301615f,
		0.3000908f,
		-0.2974265f,
		-0.2968024f,
		0.08475886f,
		-0.289977f,
		0.2943893f,
		-0.07240812f,
		-0.1023849f,
		-0.03870017f,
		-0.02209093f,
		-0.1710741f,
		0.02665752f,
		-0.1199554f,
		0.2379791f,
		-0.2149891f,
		0.2849169f,
		-0.1207748f,
		0.1239818f,
		0.2294074f,
		0.0989732f,
		0.1529666f,
		-0.1166208f,
		-0.2911412f,
		0.2974453f,
		0.1146155f,
		-0.1120537f,
		0.06912041f,
		-0.07836653f,
		-0.2477419f,
		-0.06909014f,
		0.2555821f,
		-0.2295429f,
		0.249578f,
		0.1716784f,
		0.1331692f,
		0.1414086f,
		-0.07870726f,
		0.3114465f,
		0.155453f,
		0.05647517f,
		0.02120163f,
		0.04604493f,
		0.1374719f,
		-0.2229169f,
		-0.07761408f,
		-0.1978485f,
		-0.1757867f,
		-0.1857033f,
		0.2433183f,
		0.2020769f,
		0.2733218f,
		-0.1444006f,
		0.06842007f,
		-0.1180121f,
		-0.3036411f,
		-0.2071026f,
		-0.09561521f,
		0.09780467f,
		-0.2592242f,
		-0.02847838f,
		0.1534369f,
		-0.2965503f,
		0.1471394f,
		0.2678051f,
		0.2709511f,
		-0.2912002f,
		0.2653232f,
		0.05564212f,
		0.1029283f,
		-0.1925138f,
		0.09087703f,
		-0.04890646f,
		0.1457155f,
		0.03171037f,
		0.05102445f,
		0.1177287f,
		0.06805613f,
		-0.2883594f,
		0.1712627f,
		-0.07016753f,
		0.1470301f,
		0.2974815f,
		0.2477935f,
		-0.08694784f,
		-0.3125716f,
		0.2342839f,
		0.01400278f,
		-587f / (609f * (float)Math.PI),
		-0.1067391f,
		-0.1984341f,
		0.1786483f,
		0.01829441f,
		0.1204441f,
		-0.3104365f,
		0.214666f,
		-0.07531048f,
		-0.2690267f,
		-0.1286234f,
		0.1188236f,
		0.2563104f,
		0.04943924f,
		0.2176594f,
		0.08831189f,
		0.1625734f,
		-0.247525f,
		-0.005613149f,
		0.04707819f,
		0.2187641f,
		-0.1875107f,
		0.1717521f,
		0.2955899f,
		0.16437f,
		-0.140052f,
		0.08865403f,
		0.2377331f,
		-0.2420434f,
		-0.004326867f,
		-0.04956147f,
		0.1882785f,
		-0.05301615f,
		0.3000908f,
		-0.2974265f,
		-0.2968024f,
		0.08475886f,
		-0.289977f,
		0.2943893f,
		-0.07240812f,
		-0.1023849f,
		-0.03870017f,
		-0.02209093f,
		-0.1710741f,
		0.02665752f,
		-0.1199554f,
		0.2379791f,
		-0.2149891f,
		0.2849169f,
		-0.1207748f,
		0.1239818f,
		0.2294074f,
		0.0989732f,
		0.1529666f,
		-0.1166208f,
		-0.2911412f,
		0.2974453f,
		0.1146155f,
		-0.1120537f,
		0.06912041f,
		-0.07836653f,
		-0.2477419f,
		-0.06909014f,
		0.2555821f,
		-0.2295429f,
		0.249578f,
		0.1716784f,
		0.1331692f,
		0.1414086f,
		-0.07870726f,
		0.3114465f,
		0.155453f,
		0.05647517f,
		0.02120163f,
		0.04604493f,
		0.1374719f,
		-0.2229169f,
		-0.07761408f,
		-0.1978485f,
		-0.1757867f,
		-0.1857033f,
		0.2433183f,
		0.2020769f,
		0.2733218f,
		-0.1444006f,
		0.06842007f,
		-0.1180121f,
		-0.3036411f
	};

	public static DNTG_Formation G_Formation;

	private DNTG_FISH_TYPE FishType;

	private SpecialFishType SpecialType;

	private DNTG_FORMATION FormationType;

	private Vector3 rotVector = default(Vector3);

	public static DNTG_Formation GetSingleton()
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

	private void Update()
	{
		DynamicCreateFormationHexagonal();
	}

	private void CreateAFish(DNTG_FORMATION formationType, float x, float y, float rot, DNTG_FISH_TYPE FishType, int i, float index = 0f, SpecialFishType specialFish = SpecialFishType.CommonFish, float magnification = -1f)
	{
		Transform transform = DNTG_FishPoolMngr.GetSingleton().CreateFishForBig(FishType, i, specialFish);
		if (transform == null)
		{
			UnityEngine.Debug.LogError(formationType + " 创建鱼阵异常 " + FishType);
			return;
		}
		rotVector = transform.eulerAngles;
		rotVector.x = rot / (float)Math.PI * 180f;
		transform.eulerAngles = rotVector;
		if (magnification > 0f)
		{
			transform.localScale = Vector3.one * magnification;
		}
		if (FishType == DNTG_FISH_TYPE.Fish_Monkey)
		{
			transform.GetComponent<DNTG_ISwimObj>().SetUpDir(isR: false);
		}
		transform.GetComponent<DNTG_NormalFish>().InitNormalFish(FormationType, x, y, rot, index, FishType, i);
		transform.GetComponent<DNTG_NormalFish>().StartMove(move: true);
	}

	public void ShowFormation(DNTG_FORMATION type, Vector2 showPos = default(Vector2), DNTG_FISH_TYPE fishType = DNTG_FISH_TYPE.Fish_TYPE_NONE, DNTG_FISH_TYPE fishKingType = DNTG_FISH_TYPE.Fish_TYPE_NONE, int fishKingIndex = -1)
	{
		ShowFormationHex = false;
		switch (type)
		{
		case DNTG_FORMATION.Formation_BigFishes:
			CreateFormationBigFishes();
			break;
		case DNTG_FORMATION.Formation_Hexagonal:
			CreateFormation_Hexagonal();
			ShowFormationHex = true;
			totalTime = 0f;
			break;
		case DNTG_FORMATION.Formation_NineCircle:
			CreateFormation_NineCircle();
			break;
		case DNTG_FORMATION.Formation_ConcentricCircles:
			CreateFormation_ConcentricCircles();
			break;
		case DNTG_FORMATION.Formation_Fluctuate:
			CreateFormation_Fluctuate();
			break;
		case DNTG_FORMATION.Formation_RedCarpet:
			CreateFormationRedCarpet();
			break;
		case DNTG_FORMATION.Formation_YaoQianShuL:
			CreateYaoQianShu(0);
			break;
		case DNTG_FORMATION.Formation_YaoQianShuR:
			CreateYaoQianShu(1);
			break;
		case DNTG_FORMATION.Formation_FishArray:
			CreateFormation_FishArray(showPos, fishType, fishKingType, fishKingIndex);
			break;
		case DNTG_FORMATION.Formation_MonkeyByCar:
			CreateMonkeyByCar(isLeft: false);
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
				FishType = DNTG_FISH_TYPE.Fish_GoldenDragon;
				num4 = num3 - num - (float)(i * 600 * 1366 / 1567);
				num5 = 750f - num2 + (float)(i * 600 * 768 / 1567);
				rot = Mathf.Acos(0.871729434f);
			}
			else if (i < 16)
			{
				FishType = DNTG_FISH_TYPE.Fish_Beauty;
				num4 = num3 - num + 150f - (float)((i - 8) * 600 * 1366 / 1567);
				num5 = 900f - num2 + (float)((i - 8) * 600 * 768 / 1567);
				rot = Mathf.Acos(0.871729434f);
			}
			else if (i < 24)
			{
				FishType = DNTG_FISH_TYPE.Fish_BlueDolphin;
				num4 = num3 - num - (float)((i - 16) * 600 * 1366 / 1567);
				num5 = -412.44f - (float)((i - 16) * 600 * 768 / 1567);
				rot = 0f - Mathf.Acos(0.871729434f);
			}
			else if (i < 32)
			{
				FishType = DNTG_FISH_TYPE.Fish_Butterfly;
				num4 = num3 - num + 160f - (float)((i - 24) * 600 * 1366 / 1567);
				num5 = -650 - (i - 24) * 600 * 768 / 1567;
				rot = 0f - Mathf.Acos(0.871729434f);
			}
			else if (i < 40)
			{
				FishType = DNTG_FISH_TYPE.Fish_Arrow;
				num4 = 1466f - num + (float)((i - 32) * 600 * 1366 / 1567);
				num5 = -550 - (i - 32) * 600 * 768 / 1567;
				rot = -(float)Math.PI + Mathf.Acos(0.871729434f);
			}
			else if (i < 48)
			{
				FishType = DNTG_FISH_TYPE.Fish_GoldenShark;
				num4 = 1566f - num + (float)((i - 40) * 600 * 1366 / 1567);
				num5 = -400 - (i - 40) * 600 * 768 / 1567;
				rot = -(float)Math.PI + Mathf.Acos(0.871729434f);
			}
			else if (i < 56)
			{
				FishType = DNTG_FISH_TYPE.Fish_SilverShark;
				num4 = 1616f - num + (float)((i - 48) * 600 * 1366 / 1567);
				num5 = 800f - num2 + (float)((i - 48) * 600 * 768 / 1567);
				rot = (float)Math.PI - Mathf.Acos(0.871729434f);
			}
			else
			{
				FishType = DNTG_FISH_TYPE.Fish_Bat;
				num4 = 1466f - num + (float)((i - 56) * 600 * 1366 / 1567);
				num5 = 950f - num2 + (float)((i - 56) * 600 * 768 / 1567);
				rot = (float)Math.PI - Mathf.Acos(0.871729434f);
			}
			num4 /= 110f;
			num5 /= 110f;
			FormationType = DNTG_FORMATION.Formation_BigFishes;
			CreateAFish(FormationType, num4, num5, rot, FishType, i, num3);
		}
	}

	private void CreateFormation_Fluctuate()
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < 250; i++)
		{
			if (i < 70)
			{
				FishType = DNTG_FISH_TYPE.Fish_Shrimp;
				if (i % 2 == 0)
				{
					num = -620f + (float)i * 1500f / 80f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 400f;
				}
				else
				{
					num = -620f + (float)i * 1500f / 80f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 400f;
				}
			}
			else if (i < 130)
			{
				FishType = DNTG_FISH_TYPE.Fish_Zebra;
				if (i % 2 == 0)
				{
					num = -620f + (float)i * 1500f / 70f - 1500f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 500f;
				}
				else
				{
					num = -620f + (float)i * 1500f / 70f - 1500f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 500f;
				}
			}
			else if (i < 180)
			{
				FishType = DNTG_FISH_TYPE.Fish_YellowSpot;
				if (i % 2 == 0)
				{
					num = -620f + (float)i * 1500f / 55f - 3570f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 600f;
				}
				else
				{
					num = -620f + (float)i * 1500f / 55f - 3570f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 600f;
				}
			}
			else if (i < 220)
			{
				FishType = DNTG_FISH_TYPE.Fish_Lamp;
				if (i % 2 == 0)
				{
					num = -620f + (float)i * 1500f / 45f - 6000f;
					num2 = -400f + (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 800f;
				}
				else
				{
					num = -620f + (float)i * 1500f / 45f - 6000f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 240f / 3f - 800f;
				}
			}
			else if (i < 250)
			{
				FishType = DNTG_FISH_TYPE.Fish_Trailer;
				if (i % 2 == 0)
				{
					num = -620f + (float)i * 1500f / 35f - 9450f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 1000f;
				}
				else
				{
					num = -620f + (float)i * 1500f / 35f - 9450f;
					num2 = -400f - (float)((i - 80) * 10) * arr[i] % 6f * 200f / 3f - 1000f;
				}
			}
			float rot = -(float)Math.PI / 2f;
			num /= 110f;
			num2 /= 110f;
			FormationType = DNTG_FORMATION.Formation_Fluctuate;
			CreateAFish(FormationType, num, num2, rot, FishType, i, num);
		}
		for (int j = 250; j < 500; j++)
		{
			if (j < 320)
			{
				FishType = DNTG_FISH_TYPE.Fish_Grass;
				if (j % 2 == 0)
				{
					num = -620f + (float)j * 1500f / 80f - 4700f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 500f;
				}
				else
				{
					num = -620f + (float)j * 1500f / 80f - 4700f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 500f;
				}
			}
			else if (j < 380)
			{
				FishType = DNTG_FISH_TYPE.Fish_BigEars;
				if (j % 2 == 0)
				{
					num = -620f + (float)j * 1500f / 70f - 6850f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 600f;
				}
				else
				{
					num = -620f + (float)j * 1500f / 70f - 6850f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 600f;
				}
			}
			else if (j < 430)
			{
				FishType = DNTG_FISH_TYPE.Fish_Ugly;
				if (j % 2 == 0)
				{
					num = -620f + (float)j * 1500f / 55f - 10450f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 700f;
				}
				else
				{
					num = -620f + (float)j * 1500f / 55f - 10450f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 700f;
				}
			}
			else if (j < 470)
			{
				FishType = DNTG_FISH_TYPE.Fish_Hedgehog;
				if (j % 2 == 0)
				{
					num = -620f + (float)j * 1500f / 45f - 14350f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 800f;
				}
				else
				{
					num = -620f + (float)j * 1500f / 45f - 14350f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 800f;
				}
			}
			else if (j < 500)
			{
				FishType = DNTG_FISH_TYPE.Fish_Turtle;
				if (j % 2 == 0)
				{
					num = -620f + (float)j * 1500f / 35f - 20100f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 850f;
				}
				else
				{
					num = -620f + (float)j * 1500f / 35f - 20100f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 850f;
				}
			}
			float rot2 = (float)Math.PI / 2f;
			num /= 110f;
			num2 /= 110f;
			FormationType = DNTG_FORMATION.Formation_Fluctuate;
			CreateAFish(FormationType, num, num2, rot2, FishType, j, num);
		}
	}

	private void CreateFormation_ConcentricCircles()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		int num5 = 0;
		for (int i = 0; i < 119; i++)
		{
			if (i < 1)
			{
				FishType = DNTG_FISH_TYPE.Fish_Boss;
				num4 = 0f;
				num5 = 1;
			}
			else if (i < 123)
			{
				if (i < 14)
				{
					FishType = DNTG_FISH_TYPE.Fish_Ugly;
					num4 = 200f;
					num5 = i - 1;
					num = 0.483321965f;
				}
				else if (i < 38)
				{
					FishType = DNTG_FISH_TYPE.Fish_BigEars;
					num4 = 250f;
					num5 = i - 14;
					num = (float)Math.PI / 12f;
				}
				else if (i < 78)
				{
					FishType = DNTG_FISH_TYPE.Fish_Zebra;
					num4 = 300f;
					num5 = i - 38;
					num = (float)Math.PI / 20f;
				}
				else
				{
					FishType = DNTG_FISH_TYPE.Fish_Shrimp;
					num4 = 350f;
					num5 = i - 78;
					num = (float)Math.PI / 20f;
				}
			}
			float num6 = (0f - (float)num5) * num + (float)Math.PI / 2f;
			float num7 = num2 + num4 * Mathf.Sin(num6);
			float num8 = num3 + num4 * Mathf.Cos(num6);
			num7 /= 110f;
			num8 /= 110f;
			if (num6 > (float)Math.PI * 2f)
			{
				num6 -= (float)Math.PI * 2f;
			}
			FormationType = DNTG_FORMATION.Formation_ConcentricCircles;
			CreateAFish(FormationType, num7, num8, num6, FishType, i, num4);
		}
	}

	private void CreateFormation_NineCircle()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		int num5 = 0;
		for (int i = 0; i < 160; i++)
		{
			if (i < 60)
			{
				FishType = DNTG_FISH_TYPE.Fish_Shrimp;
				num4 = 100f;
				num = (float)Math.PI / 30f;
				num5 = i;
			}
			else if (i < 90)
			{
				FishType = DNTG_FISH_TYPE.Fish_YellowSpot;
				num4 = 100f;
				num5 = i - 60;
				num = (float)Math.PI / 15f;
			}
			else if (i < 110)
			{
				FishType = DNTG_FISH_TYPE.Fish_Ugly;
				num4 = 80f;
				num5 = i - 90;
				num = (float)Math.PI / 10f;
			}
			else if (i < 125)
			{
				FishType = DNTG_FISH_TYPE.Fish_Hedgehog;
				num4 = 80f;
				num5 = i - 110;
				num = (float)Math.PI * 2f / 15f;
			}
			else if (i < 137)
			{
				FishType = DNTG_FISH_TYPE.Fish_Lamp;
				num4 = 80f;
				num5 = i - 125;
				num = (float)Math.PI / 6f;
			}
			else if (i < 147)
			{
				FishType = DNTG_FISH_TYPE.Fish_Turtle;
				num4 = 60f;
				num5 = i - 137;
				num = (float)Math.PI / 5f;
			}
			else if (i < 153)
			{
				FishType = DNTG_FISH_TYPE.Fish_Trailer;
				num4 = 60f;
				num5 = i - 147;
				num = (float)Math.PI / 3f;
			}
			else if (i < 157)
			{
				FishType = DNTG_FISH_TYPE.Fish_Arrow;
				num4 = 60f;
				num5 = i - 153;
				num = (float)Math.PI / 2f;
			}
			else if (i < 160)
			{
				FishType = DNTG_FISH_TYPE.Fish_SilverShark;
				num4 = 60f;
				num5 = i - 157;
				num = (float)Math.PI * 2f / 3f;
			}
			float num6 = (float)num5 * num;
			float num7 = num2 + num4 * Mathf.Sin(num6);
			float num8 = num3 + num4 * Mathf.Cos(num6);
			num7 /= 110f;
			num8 /= 110f;
			FormationType = DNTG_FORMATION.Formation_NineCircle;
			CreateAFish(FormationType, num7, num8, num6, FishType, i, num4);
		}
	}

	private void CreateFormation_Hexagonal()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = 0;
		int num5 = 0;
		if (mHexagonalIndex == 0)
		{
			FishType = DNTG_FISH_TYPE.Fish_BigEars;
			num4 = 3;
			num5 = 0;
		}
		else if (mHexagonalIndex == 1)
		{
			num4 = 120;
			num5 = 3;
			num3 = 20f;
			FishType = DNTG_FISH_TYPE.Fish_Shrimp;
		}
		else if (mHexagonalIndex == 2)
		{
			num4 = 120;
			num5 = 123;
			num3 = 20f;
			FishType = DNTG_FISH_TYPE.Fish_Zebra;
		}
		else if (mHexagonalIndex == 3)
		{
			num4 = 84;
			num5 = 243;
			num3 = 30f;
			FishType = DNTG_FISH_TYPE.Fish_Ugly;
		}
		else if (mHexagonalIndex == 4)
		{
			num4 = 60;
			num5 = 327;
			num3 = 40f;
			FishType = DNTG_FISH_TYPE.Fish_Hedgehog;
		}
		else if (mHexagonalIndex == 5)
		{
			num4 = 60;
			num5 = 387;
			num3 = 40f;
			FishType = DNTG_FISH_TYPE.Fish_Lamp;
		}
		int num6 = 1;
		for (int i = 0; i < num4; i++)
		{
			num6 *= -1;
			float num7 = Mathf.Cos((float)(i * num4) * 1f) * (float)Math.PI / 15f;
			float num10;
			float num8;
			float num9;
			if (mHexagonalIndex == 0)
			{
				num8 = num;
				num9 = num2;
				num10 = (float)(i * 2) * (float)Math.PI / 3f;
			}
			else if (i < num4 / 6)
			{
				num10 = 3.66519165f;
				num8 = num + 5f + (float)i * num3 * Mathf.Cos(num10);
				num9 = num2 + 8.66f + (float)i * num3 * Mathf.Sin(num10);
			}
			else if (i < 2 * num4 / 6)
			{
				num10 = 4.712389f;
				num8 = num + 10f;
				num9 = num2 - (float)(i - num4 / 6) * num3;
			}
			else if (i < 3 * num4 / 6)
			{
				num10 = 5.759587f;
				num8 = num + 5f + (float)(i - 2 * num4 / 6) * num3 * Mathf.Cos(num10);
				num9 = num2 - 8.66f + (float)(i - 2 * num4 / 6) * num3 * Mathf.Sin(num10);
			}
			else if (i < 4 * num4 / 6)
			{
				num10 = (float)Math.PI * 5f / 6f;
				num8 = num - 5f + (float)(i - 3 * num4 / 6) * num3 * Mathf.Cos(num10);
				num9 = num2 - 8.66f + (float)(i - 3 * num4 / 6) * num3 * Mathf.Sin(num10);
			}
			else if (i < 5 * num4 / 6)
			{
				num10 = (float)Math.PI / 2f;
				num8 = num - 10f;
				num9 = num2 + (float)(i - 4 * num4 / 6) * num3;
			}
			else
			{
				num10 = (float)Math.PI / 6f;
				num8 = num - 5f + (float)(i - 5 * num4 / 6) * num3 * Mathf.Cos(num10);
				num9 = num2 - 8.66f + (float)(i - 5 * num4 / 6) * num3 * Mathf.Sin(num10);
			}
			num10 += num7;
			num8 /= 110f;
			num9 /= 110f;
			FormationType = DNTG_FORMATION.Formation_Hexagonal;
			CreateAFish(FormationType, num8, num9, num10, FishType, num5 + i, num3);
		}
	}

	private void CreateFormation_FishArray(Vector2 showPos, DNTG_FISH_TYPE FishType, DNTG_FISH_TYPE FishKingType = DNTG_FISH_TYPE.Fish_TYPE_NONE, int fishKingIndex = -1)
	{
		StartCoroutine(WaitCreateFormation_FishArray(showPos, FishType, FishKingType, fishKingIndex));
	}

	private IEnumerator WaitCreateFormation_FishArray(Vector2 showPos, DNTG_FISH_TYPE FishType, DNTG_FISH_TYPE FishKingType = DNTG_FISH_TYPE.Fish_TYPE_NONE, int fishKingIndex = -1)
	{
		float showPosX = showPos.x * 131f;
		float showPosY = showPos.y * 127f;
		DNTG_FISH_TYPE tempFishType = FishType;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 30; j++)
			{
				float num6 = 100f;
				float interval = (float)Math.PI / 15f;
				int num5 = j;
				float num7 = (float)num5 * interval;
				float num8 = showPosX + num6 * Mathf.Sin(num7) / 3f;
				float num9 = showPosY + num6 * Mathf.Cos(num7) / 3f;
				num8 /= 130f;
				num9 /= 130f;
				if (i == 2 && fishKingIndex >= 0 && fishKingIndex == j && FishKingType != DNTG_FISH_TYPE.Fish_TYPE_NONE)
				{
					FishType = FishKingType;
					fishKingIndex = -1;
					FishKingType = DNTG_FISH_TYPE.Fish_TYPE_NONE;
				}
				else
				{
					FishType = tempFishType;
				}
				FormationType = DNTG_FORMATION.Formation_FishArray;
				CreateAFish(FormationType, num8, num9, num7, FishType, j, num6);
			}
			yield return new WaitForSeconds(0.85f);
		}
	}

	private void CreateFormationRedCarpet()
	{
		for (int i = 0; i < 160; i++)
		{
			FishType = DNTG_FISH_TYPE.Fish_Shrimp;
			float rot;
			int num3;
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
				num3 = i;
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
				num3 = i - 80;
			}
			num /= 110f;
			num2 /= 110f;
			int i2 = i;
			FormationType = DNTG_FORMATION.Formation_RedCarpet;
			CreateAFish(FormationType, num, num2, rot, FishType, i2, num3);
		}
		for (int j = 0; j < 22; j++)
		{
			float rot2;
			float num5;
			float num6;
			if (j < 11)
			{
				float num4 = -700f;
				if (j < 3)
				{
					FishType = DNTG_FISH_TYPE.Fish_Bat;
					num5 = num4 - (float)(j * 300);
				}
				else if (j < 6)
				{
					FishType = DNTG_FISH_TYPE.Fish_SilverShark;
					num5 = num4 - 900f - (float)((j - 3) * 400);
				}
				else if (j < 9)
				{
					FishType = DNTG_FISH_TYPE.Fish_GoldenShark;
					num5 = num4 - 900f - 1200f - (float)((j - 6) * 400);
				}
				else if (j < 10)
				{
					num5 = num4 - 900f - 1200f - 1200f - 100f;
				}
				else
				{
					FishType = DNTG_FISH_TYPE.Fish_GoldenDragon;
					num5 = num4 - 900f - 1200f - 1200f - 100f - 500f;
				}
				num6 = -120f;
				rot2 = 0f;
			}
			else
			{
				float num7 = 700f;
				if (j < 14)
				{
					FishType = DNTG_FISH_TYPE.Fish_Bat;
					num5 = num7 + (float)((j - 11) * 300);
				}
				else if (j < 17)
				{
					FishType = DNTG_FISH_TYPE.Fish_SilverShark;
					num5 = num7 + 900f + (float)((j - 14) * 400);
				}
				else if (j < 20)
				{
					FishType = DNTG_FISH_TYPE.Fish_GoldenShark;
					num5 = num7 + 900f + 1200f + (float)((j - 17) * 400);
				}
				else if (j < 21)
				{
					num5 = num7 + 900f + 1200f + 1200f + 100f;
				}
				else
				{
					FishType = DNTG_FISH_TYPE.Fish_GoldenDragon;
					num5 = num7 + 900f + 1200f + 1200f + 100f + 500f;
				}
				num6 = 120f;
				rot2 = (float)Math.PI;
			}
			num5 /= 110f;
			num6 /= 110f;
			int i3 = 160 + j;
			FormationType = DNTG_FORMATION.Formation_RedCarpet;
			CreateAFish(FormationType, num5, num6, rot2, FishType, i3, num5);
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
		FishType = DNTG_FISH_TYPE.Fish_Shrimp;
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
			FormationType = DNTG_FORMATION.Formation_YaoQianShuL;
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
			FormationType = DNTG_FORMATION.Formation_YaoQianShuL;
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
			FormationType = DNTG_FORMATION.Formation_YaoQianShuL;
			CreateAFish(FormationType, num, num2, rot, FishType, i4);
		}
		for (int l = 0; l < 6; l++)
		{
			if (l < 2)
			{
				num = 240f + num3;
				num2 = 250 - l * 500;
				FishType = DNTG_FISH_TYPE.Fish_Penguin;
			}
			else if (l < 4)
			{
				num = (float)(240 - 500 * left) + num3;
				num2 = 250 - (l - 2) * 500;
				FishType = DNTG_FISH_TYPE.Fish_GoldenDragon;
			}
			else if (l < 6)
			{
				num = (float)(240 - 500 * left - 500 * left) + num3;
				num2 = 250 - (l - 4) * 500;
				FishType = DNTG_FISH_TYPE.Fish_GoldenShark;
			}
			num /= 100f;
			num2 /= 100f;
			int i5 = l + 149 + 94 + 97;
			FormationType = DNTG_FORMATION.Formation_YaoQianShuL;
			CreateAFish(FormationType, num, num2, rot, FishType, i5);
		}
	}

	private void CreateMonkeyByCar(bool isLeft)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		if (isLeft)
		{
			num4 = 0f;
			num3 = -1150f;
		}
		else
		{
			num4 = (float)Math.PI;
			num3 = 720f;
		}
		FishType = DNTG_FISH_TYPE.Fish_Grass;
		SpecialType = SpecialFishType.CommonFish;
		float magnification = -1f;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		for (int i = 0; i < 90; i++)
		{
			float num8 = num3;
			float num9 = 0f;
			float num10 = 60f;
			float num11 = 50f;
			if (i < 4)
			{
				num = num8 + (float)i * num10;
				num2 = num9;
			}
			else if (i < 7)
			{
				num = num8 + (float)i * num10;
				num2 = num9 - (float)(i - 3) * num11;
			}
			else if (i < 15)
			{
				num = num8 + (float)i * num10;
				num2 = num9 - 3f * num11;
			}
			else if (i < 18)
			{
				num = num8 + (float)i * num10;
				num2 = num9 + (float)(i - 17) * num11;
			}
			else if (i < 22)
			{
				num = num8 + (float)i * num10;
				num2 = num9;
			}
			else if (i < 26)
			{
				num = num8 + 21f * num10;
				num2 = num9 + (float)(i - 21) * num11;
			}
			else if (i < 47)
			{
				num = num8 - (float)(i - 46) * num10;
				num2 = num9 + 4f * num11;
			}
			else if (i < 50)
			{
				num = num8;
				num2 = num9 - (float)(i - 50) * num11;
			}
			else if (i < 52)
			{
				FishType = DNTG_FISH_TYPE.Fish_Trailer;
				num = num8 - 2f * num10;
				num2 = num9 - (float)(i - 52) * (num11 + 15f) - (float)(i - 50) * (num11 + 15f);
			}
			else if (i < 56)
			{
				FishType = DNTG_FISH_TYPE.Fish_Zebra;
				num = num8 + 19f * num10;
				num2 = num9 + (float)(i - 56) * num11;
			}
			else if (i < 57)
			{
				FishType = DNTG_FISH_TYPE.Fish_Monkey;
				SpecialType = SpecialFishType.MonkeyKing;
				num = num8 + 10f * num10;
				num2 = num9 + num11;
			}
			else if (i < 58)
			{
				FishType = DNTG_FISH_TYPE.Fish_Turtle;
				SpecialType = SpecialFishType.CommonFish;
				magnification = 1.3f;
				num = num8 + 2f * num10;
				num2 = num9 + 2f * num11;
			}
			else if (i < 60)
			{
				FishType = DNTG_FISH_TYPE.Fish_Ugly;
				SpecialType = SpecialFishType.CommonFish;
				magnification = 1.3f;
				num = num8 + 7f * num10;
				num2 = num9 - (float)(i - 60) * (num11 + 15f) - (float)(i - 58) * (num11 + 45f);
			}
			else if (i < 61)
			{
				FishType = DNTG_FISH_TYPE.Fish_Ugly;
				SpecialType = SpecialFishType.CommonFish;
				magnification = 1.3f;
				num = num8 + 13f * num10;
				num2 = num9 - (float)(i - 61) * (num11 + 15f) - (num11 + 45f);
			}
			else if (i < 62)
			{
				FishType = DNTG_FISH_TYPE.Fish_Turtle;
				SpecialType = SpecialFishType.CommonFish;
				magnification = 1.3f;
				num = num8 + 18f * num10;
				num2 = num9 + 2f * num11;
			}
			else if (i < 64)
			{
				FishType = ((i % 2 != 0) ? DNTG_FISH_TYPE.Fish_Grass : DNTG_FISH_TYPE.Fish_Shrimp);
				SpecialType = SpecialFishType.HeavenFish;
				magnification = 1f;
				num = num8 + (float)(i - 39) * num10 + (float)(i - 62) * (num11 + 30f);
				num2 = num9 + 2f * (num11 + 15f);
			}
			else if (i < 77)
			{
				FishType = DNTG_FISH_TYPE.Fish_Shrimp;
				SpecialType = SpecialFishType.CommonFish;
				magnification = 1f;
				num = num8 + (float)(9 + num5) * (num10 / 2f);
				num2 = num9 + (9.5f + (float)num6) * (num11 / 2f);
				num5++;
				num7++;
				switch (num7)
				{
				case 2:
					num5 = 0;
					num6++;
					break;
				case 5:
					num5 = 0;
					num6++;
					break;
				case 8:
					num5 = 0;
					num6++;
					break;
				case 11:
					num5 = 0;
					num6++;
					break;
				case 13:
					num5 = 0;
					num6++;
					break;
				}
				if (i == 76)
				{
					num5 = 0;
					num7 = 0;
					num6 = 0;
				}
			}
			else if (i < 90)
			{
				FishType = DNTG_FISH_TYPE.Fish_Shrimp;
				SpecialType = SpecialFishType.CommonFish;
				magnification = 1f;
				num = num8 + (float)(30 + num5) * (num10 / 2f);
				num2 = num9 + (9.5f + (float)num6) * (num11 / 2f);
				num5++;
				num7++;
				switch (num7)
				{
				case 2:
					num5 = 0;
					num6++;
					break;
				case 5:
					num5 = 0;
					num6++;
					break;
				case 8:
					num5 = 0;
					num6++;
					break;
				case 11:
					num5 = 0;
					num6++;
					break;
				case 13:
					num5 = 0;
					num6++;
					break;
				}
			}
			num /= 100f;
			num2 /= 100f;
			int i2 = i;
			FormationType = DNTG_FORMATION.Formation_MonkeyByCar;
			CreateAFish(FormationType, num, num2, num4, FishType, i2, 0f, SpecialType, magnification);
		}
	}

	private void CreateMonkeyByCar2(bool isLeft)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		if (isLeft)
		{
			num4 = 0f;
			num3 = -1150f;
		}
		else
		{
			num4 = (float)Math.PI;
			num3 = 720f;
		}
		FishType = DNTG_FISH_TYPE.Fish_Grass;
		SpecialType = SpecialFishType.CommonFish;
		for (int i = 0; i < 142; i++)
		{
			float num5 = num3;
			float num6 = 200f;
			if (i < 20)
			{
				FishType = DNTG_FISH_TYPE.Fish_Grass;
				num = num5 + (float)(i * 40);
				num2 = num6 - 20f;
			}
			else if (i < 40)
			{
				FishType = DNTG_FISH_TYPE.Fish_Shrimp;
				num = num5 + (float)((i - 20) * 40);
				num2 = num6 - 50f;
			}
			else if (i < 70)
			{
				int num7 = 0;
				if (i % 2 == 0)
				{
					FishType = DNTG_FISH_TYPE.Fish_Shrimp;
					num7 = 0;
				}
				else
				{
					FishType = DNTG_FISH_TYPE.Fish_Grass;
					num7 = 20;
				}
				num = num5 + (float)num7;
				num2 = num6 - 50f - (float)((i - 40) * 10);
			}
			else if (i < 90)
			{
				FishType = DNTG_FISH_TYPE.Fish_Shrimp;
				num = num5 + (float)((i - 70) * 40);
				num2 = num6 - 50f - 300f;
			}
			else if (i < 110)
			{
				FishType = DNTG_FISH_TYPE.Fish_Grass;
				num = num5 + (float)((i - 90) * 40);
				num2 = num6 - 50f - 300f - 30f;
			}
			else if (i < 140)
			{
				int num8 = 0;
				if (i % 2 == 0)
				{
					FishType = DNTG_FISH_TYPE.Fish_Shrimp;
					num8 = 0;
				}
				else
				{
					FishType = DNTG_FISH_TYPE.Fish_Grass;
					num8 = 20;
				}
				num = num5 + 760f + (float)num8;
				num2 = num6 - 50f - (float)((i - 110) * 10);
			}
			else if (i < 142)
			{
				UnityEngine.Debug.LogError("====创建车轮=====" + i);
				SpecialType = SpecialFishType.HeavenFish;
				FishType = DNTG_FISH_TYPE.Fish_BigEars;
				num = num5 + (float)((i - 122 + i) * 40);
				num2 = num6 - 50f - 300f;
			}
			num /= 100f;
			num2 /= 100f;
			int i2 = i;
			FormationType = DNTG_FORMATION.Formation_MonkeyByCar;
			CreateAFish(FormationType, num, num2, num4, FishType, i2, 0f, SpecialType);
		}
	}

	public void ReleaseAFish(GameObject fish)
	{
		DNTG_FishPoolMngr.GetSingleton().DestroyFish(fish);
	}

	private void DynamicCreateFormationHexagonal()
	{
		if (!ShowFormationHex)
		{
			return;
		}
		if (mHexagonalIndex > 4)
		{
			mHexagonalIndex = 0;
			ShowFormationHex = false;
			return;
		}
		totalTime += Time.deltaTime;
		if (totalTime > 7.5f)
		{
			mHexagonalIndex++;
			totalTime = 0f;
			CreateFormation_Hexagonal();
		}
	}
}
