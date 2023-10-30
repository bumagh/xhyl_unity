using GameCommon;
using System;
using UnityEngine;

public class STTF_Formation : MonoBehaviour
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

	public static STTF_Formation G_Formation;

	private STTF_FISH_TYPE FishType;

	private STTF_FORMATION FormationType;

	private Vector3 rotVector = default(Vector3);

	public static STTF_Formation GetSingleton()
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

	private void CreateAFish(STTF_FORMATION formationType, float x, float y, float rot, STTF_FISH_TYPE FishType, int i, float index = 0f)
	{
		Transform transform = STTF_FishPoolMngr.GetSingleton().CreateFishForBig(FishType, i);
		rotVector = transform.eulerAngles;
		rotVector.x = rot / (float)Math.PI * 180f;
		transform.eulerAngles = rotVector;
		transform.GetComponent<STTF_NormalFish>().InitNormalFish(FormationType, x, y, rot, index, FishType, i);
		transform.GetComponent<STTF_NormalFish>().StartMove(move: true);
	}

	public void ShowFormation(STTF_FORMATION type)
	{
		ShowFormationHex = false;
		switch (type)
		{
		case STTF_FORMATION.Formation_BigFishes:
			CreateFormationBigFishes();
			break;
		case STTF_FORMATION.Formation_Hexagonal:
			CreateFormation_Hexagonal();
			ShowFormationHex = true;
			totalTime = 0f;
			break;
		case STTF_FORMATION.Formation_NineCircle:
			CreateFormation_NineCircle();
			break;
		case STTF_FORMATION.Formation_Fluctuate:
			CreateFormation_Fluctuate();
			break;
		case STTF_FORMATION.Formation_RedCarpet:
			CreateFormationRedCarpet();
			break;
		case STTF_FORMATION.Formation_SixCircles:
			CreateFormationSixCircles();
			break;
		case STTF_FORMATION.Formation_Wedding:
			CrateFormation_Wedding();
			break;
		case STTF_FORMATION.Formation_TwoCircles:
			CreateFormation_TwoCircles();
			break;
		case STTF_FORMATION.Formation_Cars:
			CrateFormation_Cars();
			break;
		case STTF_FORMATION.Formation_Symmetry:
			CreateFormationSymmetry();
			break;
		case STTF_FORMATION.Formation_TwoArmies:
			CreateFormation_TwoArmies();
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
				FishType = STTF_FISH_TYPE.Fish_Toad;
				num4 = num3 - num - (float)(i * 600 * 1366 / 1567);
				num5 = 750f - num2 + (float)(i * 600 * 768 / 1567);
				rot = Mathf.Acos(0.871729434f);
			}
			else if (i < 16)
			{
				FishType = STTF_FISH_TYPE.Fish_Beauty;
				num4 = num3 - num + 150f - (float)((i - 8) * 600 * 1366 / 1567);
				num5 = 900f - num2 + (float)((i - 8) * 600 * 768 / 1567);
				rot = Mathf.Acos(0.871729434f);
			}
			else if (i < 24)
			{
				FishType = STTF_FISH_TYPE.Fish_Dragon;
				num4 = num3 - num - (float)((i - 16) * 600 * 1366 / 1567);
				num5 = -412.44f - (float)((i - 16) * 600 * 768 / 1567);
				rot = 0f - Mathf.Acos(0.871729434f);
			}
			else if (i < 32)
			{
				FishType = STTF_FISH_TYPE.Fish_Butterfly;
				num4 = num3 - num + 160f - (float)((i - 24) * 600 * 1366 / 1567);
				num5 = -650 - (i - 24) * 600 * 768 / 1567;
				rot = 0f - Mathf.Acos(0.871729434f);
			}
			else if (i < 40)
			{
				FishType = STTF_FISH_TYPE.Fish_Arrow;
				num4 = 1466f - num + (float)((i - 32) * 600 * 1366 / 1567);
				num5 = -550 - (i - 32) * 600 * 768 / 1567;
				rot = -(float)Math.PI + Mathf.Acos(0.871729434f);
			}
			else if (i < 48)
			{
				FishType = STTF_FISH_TYPE.Fish_GoldenShark;
				num4 = 1566f - num + (float)((i - 40) * 600 * 1366 / 1567);
				num5 = -400 - (i - 40) * 600 * 768 / 1567;
				rot = -(float)Math.PI + Mathf.Acos(0.871729434f);
			}
			else if (i < 56)
			{
				FishType = STTF_FISH_TYPE.Fish_SilverShark;
				num4 = 1616f - num + (float)((i - 48) * 600 * 1366 / 1567);
				num5 = 800f - num2 + (float)((i - 48) * 600 * 768 / 1567);
				rot = (float)Math.PI - Mathf.Acos(0.871729434f);
			}
			else
			{
				FishType = STTF_FISH_TYPE.Fish_Bat;
				num4 = 1466f - num + (float)((i - 56) * 600 * 1366 / 1567);
				num5 = 950f - num2 + (float)((i - 56) * 600 * 768 / 1567);
				rot = (float)Math.PI - Mathf.Acos(0.871729434f);
			}
			num4 /= 110f;
			num5 /= 110f;
			FormationType = STTF_FORMATION.Formation_BigFishes;
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
				FishType = STTF_FISH_TYPE.Fish_Shrimp;
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
				FishType = STTF_FISH_TYPE.Fish_Zebra;
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
				FishType = STTF_FISH_TYPE.Fish_YellowSpot;
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
				FishType = STTF_FISH_TYPE.Fish_Lamp;
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
				FishType = STTF_FISH_TYPE.Fish_Trailer;
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
			FormationType = STTF_FORMATION.Formation_Fluctuate;
			CreateAFish(FormationType, num, num2, rot, FishType, i, num);
		}
		for (int j = 250; j < 500; j++)
		{
			if (j < 320)
			{
				FishType = STTF_FISH_TYPE.Fish_Grass;
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
				FishType = STTF_FISH_TYPE.Fish_BigEars;
				if (j % 2 == 0)
				{
					num = -620f + (float)j * 1500f / 70f - 6850f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 650f;
				}
				else
				{
					num = -620f + (float)j * 1500f / 70f - 6850f;
					num2 = 400f + (float)((j - 80) * 10) * arr[j - 250] % 6f * 200f / 2f + 650f;
				}
			}
			else if (j < 430)
			{
				FishType = STTF_FISH_TYPE.Fish_Ugly;
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
				FishType = STTF_FISH_TYPE.Fish_Hedgehog;
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
				FishType = STTF_FISH_TYPE.Fish_Turtle;
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
			FormationType = STTF_FORMATION.Formation_Fluctuate;
			CreateAFish(FormationType, num, num2, rot2, FishType, j, num);
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
				FishType = STTF_FISH_TYPE.Fish_Shrimp;
				num4 = 100f;
				num = (float)Math.PI / 30f;
				num5 = i;
			}
			else if (i < 90)
			{
				FishType = STTF_FISH_TYPE.Fish_YellowSpot;
				num4 = 100f;
				num5 = i - 60;
				num = (float)Math.PI / 15f;
			}
			else if (i < 110)
			{
				FishType = STTF_FISH_TYPE.Fish_Ugly;
				num4 = 80f;
				num5 = i - 90;
				num = (float)Math.PI / 10f;
			}
			else if (i < 125)
			{
				FishType = STTF_FISH_TYPE.Fish_Hedgehog;
				num4 = 80f;
				num5 = i - 110;
				num = (float)Math.PI * 2f / 15f;
			}
			else if (i < 137)
			{
				FishType = STTF_FISH_TYPE.Fish_Lamp;
				num4 = 80f;
				num5 = i - 125;
				num = (float)Math.PI / 6f;
			}
			else if (i < 147)
			{
				FishType = STTF_FISH_TYPE.Fish_Turtle;
				num4 = 60f;
				num5 = i - 137;
				num = (float)Math.PI / 5f;
			}
			else if (i < 153)
			{
				FishType = STTF_FISH_TYPE.Fish_Trailer;
				num4 = 60f;
				num5 = i - 147;
				num = (float)Math.PI / 3f;
			}
			else if (i < 157)
			{
				FishType = STTF_FISH_TYPE.Fish_Arrow;
				num4 = 60f;
				num5 = i - 153;
				num = (float)Math.PI / 2f;
			}
			else if (i < 160)
			{
				FishType = STTF_FISH_TYPE.Fish_SilverShark;
				num4 = 60f;
				num5 = i - 157;
				num = (float)Math.PI * 2f / 3f;
			}
			float num6 = (float)num5 * num;
			float num7 = num2 + num4 * Mathf.Sin(num6);
			float num8 = num3 + num4 * Mathf.Cos(num6);
			num7 /= 110f;
			num8 /= 110f;
			FormationType = STTF_FORMATION.Formation_NineCircle;
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
			FishType = STTF_FISH_TYPE.Fish_BigEars;
			num4 = 3;
			num5 = 0;
		}
		else if (mHexagonalIndex == 1)
		{
			num4 = 120;
			num5 = 3;
			num3 = 20f;
			FishType = STTF_FISH_TYPE.Fish_Shrimp;
		}
		else if (mHexagonalIndex == 2)
		{
			num4 = 120;
			num5 = 123;
			num3 = 20f;
			FishType = STTF_FISH_TYPE.Fish_Zebra;
		}
		else if (mHexagonalIndex == 3)
		{
			num4 = 84;
			num5 = 243;
			num3 = 30f;
			FishType = STTF_FISH_TYPE.Fish_Ugly;
		}
		else if (mHexagonalIndex == 4)
		{
			num4 = 60;
			num5 = 327;
			num3 = 40f;
			FishType = STTF_FISH_TYPE.Fish_Hedgehog;
		}
		else if (mHexagonalIndex == 5)
		{
			num4 = 60;
			num5 = 387;
			num3 = 40f;
			FishType = STTF_FISH_TYPE.Fish_Lamp;
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
			FormationType = STTF_FORMATION.Formation_Hexagonal;
			CreateAFish(FormationType, num8, num9, num10, FishType, num5 + i, num3);
		}
	}

	private void CreateFormationRedCarpet()
	{
		for (int i = 0; i < 160; i++)
		{
			FishType = STTF_FISH_TYPE.Fish_Shrimp;
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
			FormationType = STTF_FORMATION.Formation_RedCarpet;
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
					FishType = STTF_FISH_TYPE.Fish_Bat;
					num5 = num4 - (float)(j * 300);
				}
				else if (j < 6)
				{
					FishType = STTF_FISH_TYPE.Fish_SilverShark;
					num5 = num4 - 900f - (float)((j - 3) * 400);
				}
				else if (j < 9)
				{
					FishType = STTF_FISH_TYPE.Fish_GoldenShark;
					num5 = num4 - 900f - 1200f - (float)((j - 6) * 400);
				}
				else if (j < 10)
				{
					num5 = num4 - 900f - 1200f - 1200f - 100f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_Toad;
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
					FishType = STTF_FISH_TYPE.Fish_Bat;
					num5 = num7 + (float)((j - 11) * 300);
				}
				else if (j < 17)
				{
					FishType = STTF_FISH_TYPE.Fish_SilverShark;
					num5 = num7 + 900f + (float)((j - 14) * 400);
				}
				else if (j < 20)
				{
					FishType = STTF_FISH_TYPE.Fish_GoldenShark;
					num5 = num7 + 900f + 1200f + (float)((j - 17) * 400);
				}
				else if (j < 21)
				{
					num5 = num7 + 900f + 1200f + 1200f + 100f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_Toad;
					num5 = num7 + 900f + 1200f + 1200f + 100f + 500f;
				}
				num6 = 120f;
				rot2 = (float)Math.PI;
			}
			num5 /= 110f;
			num6 /= 110f;
			int i3 = 160 + j;
			FormationType = STTF_FORMATION.Formation_RedCarpet;
			CreateAFish(FormationType, num5, num6, rot2, FishType, i3, num5);
		}
	}

	public void ReleaseAFish(GameObject fish)
	{
		STTF_FishPoolMngr.GetSingleton().DestroyFish(fish);
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

	private void CreateFormation_TwoCircles()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		for (int i = 0; i < 216; i++)
		{
			if (i < 108)
			{
				int num6;
				if (i < 1)
				{
					FishType = STTF_FISH_TYPE.Fish_BigShark;
					num3 = 0f;
					num6 = 0;
				}
				else if (i < 14)
				{
					FishType = STTF_FISH_TYPE.Fish_Hedgehog;
					num3 = 140f;
					num6 = i - 1;
					num = 0.483321965f;
				}
				else if (i < 38)
				{
					FishType = STTF_FISH_TYPE.Fish_Ugly;
					num3 = 230f;
					num6 = i - 14;
					num = (float)Math.PI / 12f;
				}
				else if (i < 68)
				{
					FishType = STTF_FISH_TYPE.Fish_YellowSpot;
					num3 = 280f;
					num6 = i - 38;
					num = (float)Math.PI / 15f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_Grass;
					num3 = 300f;
					num6 = i - 68;
					num = (float)Math.PI / 20f;
				}
				num2 = (0f - (float)num6) * num + (float)Math.PI;
				num4 = -300f + num3 * Mathf.Sin(num2);
				num5 = num3 * Mathf.Cos(num2);
			}
			else if (i > 107 && i < 216)
			{
				int num7;
				if (i < 109)
				{
					FishType = STTF_FISH_TYPE.Fish_Toad;
					num3 = 0f;
					num7 = 0;
				}
				else if (i < 122)
				{
					FishType = STTF_FISH_TYPE.Fish_Ugly;
					num3 = 160f;
					num7 = i - 1 - 108;
					num = 0.483321965f;
				}
				else if (i < 146)
				{
					FishType = STTF_FISH_TYPE.Fish_BigEars;
					num3 = 230f;
					num7 = i - 14 - 108;
					num = (float)Math.PI / 12f;
				}
				else if (i < 176)
				{
					FishType = STTF_FISH_TYPE.Fish_Zebra;
					num3 = 280f;
					num7 = i - 38 - 108;
					num = (float)Math.PI / 15f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_Shrimp;
					num3 = 300f;
					num7 = i - 68 - 108;
					num = (float)Math.PI / 20f;
				}
				num2 = (0f - (float)num7) * num + (float)Math.PI;
				num4 = 300f + num3 * Mathf.Sin(num2);
				num5 = num3 * Mathf.Cos(num2);
			}
			num4 /= 100f;
			num5 /= 100f;
			int i2 = i;
			FormationType = STTF_FORMATION.Formation_TwoCircles;
			CreateAFish(FormationType, num4, num5, num2, FishType, i2, num3);
		}
	}

	private void CrateFormation_Cars()
	{
		float num = 50f;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < 94; i++)
		{
			if (i < 2)
			{
				FishType = STTF_FISH_TYPE.Fish_Trailer;
				num2 = num - 50f;
				num3 = 340f + (float)(i * 140);
			}
			else if (i < 4)
			{
				FishType = STTF_FISH_TYPE.Fish_Turtle;
				num2 = num - 300f - (float)(650 * (i - 2));
				num3 = 400f;
			}
			else if (i < 7)
			{
				FishType = STTF_FISH_TYPE.Fish_Ugly;
				num2 = num - 480f - (float)(150 * (i - 4));
				num3 = 260f;
			}
			else if (i < 8)
			{
				FishType = STTF_FISH_TYPE.Fish_GoldenShark;
				num2 = num - 650f;
				num3 = 380f;
			}
			else if (i < 57)
			{
				FishType = STTF_FISH_TYPE.Fish_Grass;
				if (i < 27)
				{
					num2 = num - 200f - (float)(50 * (i - 8));
					num3 = 500f;
				}
				else if (i < 37)
				{
					num2 = num - 400f - (float)(50 * (i - 27));
					num3 = 200f;
				}
				else if (i < 41)
				{
					num2 = num - 150f - (float)(50 * (i - 37));
					num3 = 300f;
				}
				else if (i < 45)
				{
					num2 = num - 900f - (float)(50 * (i - 41));
					num3 = 300f;
				}
				else if (i < 49)
				{
					num2 = num - 150f;
					num3 = 500f - (float)(50 * (i - 45));
				}
				else if (i < 53)
				{
					num2 = num - 1000f - 100f;
					num3 = 500f - (float)(50 * (i - 48));
				}
				else
				{
					switch (i)
					{
					case 53:
						num2 = num - 400f + 40f;
						num3 = 240f;
						break;
					case 54:
						num2 = num - 860f;
						num3 = 240f;
						break;
					case 55:
						num2 = num - 400f + 80f;
						num3 = 280f;
						break;
					case 56:
						num2 = num - 860f - 20f;
						num3 = 280f;
						break;
					}
				}
			}
			else if (i < 62)
			{
				FishType = STTF_FISH_TYPE.Fish_Zebra;
				num2 = num - 1000f;
				num3 = 260f - (float)(50 * (i - 57));
			}
			else if (i < 64)
			{
				FishType = STTF_FISH_TYPE.Fish_CoralReefs;
				num2 = num - 1200f - (float)(150 * (i - 62));
				num3 = 450f;
			}
			else if (i < 79)
			{
				FishType = STTF_FISH_TYPE.Fish_Shrimp;
				num2 = num - 440f + 100f * arr[i];
				num3 = 560f + 100f * arr[i];
			}
			else if (i < 94)
			{
				FishType = STTF_FISH_TYPE.Fish_Shrimp;
				num2 = num - 800f - 40f + 100f * arr[i];
				num3 = 560f + 100f * arr[i];
			}
			float rot = 0f;
			num2 = (num2 - 800f) / 100f;
			num3 = (num3 - 300f) / 100f;
			int i2 = i;
			FormationType = STTF_FORMATION.Formation_Cars;
			CreateAFish(FormationType, num2, num3, rot, FishType, i2);
		}
	}

	private void CrateFormation_Wedding()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 520f;
		float num4 = 0f;
		float num5 = 0f;
		for (int i = 0; i < 282; i++)
		{
			float f;
			if (i < 1)
			{
				FishType = STTF_FISH_TYPE.Fish_Turtle;
				num4 = num - 520f;
				num5 = 334f;
			}
			else if (i < 2)
			{
				FishType = STTF_FISH_TYPE.Fish_Trailer;
				num4 = num - 520f;
				num5 = 434f;
			}
			else if (i < 122)
			{
				FishType = STTF_FISH_TYPE.Fish_Shrimp;
				if (i < 32)
				{
					if (i < 12)
					{
						float num6 = (float)Math.PI * 2f / 85f;
						int num7 = i - 2;
						f = (float)num7 * num6;
						num4 = num - 640f + num3 * Mathf.Cos(f);
					}
					else if (i < 22)
					{
						float num8 = (float)Math.PI * 2f / 85f;
						int num9 = i - 12;
						f = (float)num9 * num8;
						num4 = num - 580f + num3 * Mathf.Cos(f);
					}
					else
					{
						float num10 = (float)Math.PI * 2f / 85f;
						int num11 = i - 22;
						f = (float)num11 * num10;
						num4 = num - 520f + num3 * Mathf.Cos(f);
					}
					num5 = 384f - num3 * Mathf.Sin(f);
				}
				else if (i < 92)
				{
					if (i < 52)
					{
						float num12 = (float)Math.PI * 2f / 85f;
						int num13 = i - 32;
						f = (float)Math.PI - 9f * num12 + (float)num13 * num12;
						num4 = num - 460f + num3 * Mathf.Cos(f);
					}
					else if (i < 72)
					{
						float num14 = (float)Math.PI * 2f / 85f;
						int num15 = i - 52;
						f = (float)Math.PI - 9f * num14 + (float)num15 * num14;
						num4 = num - 400f + num3 * Mathf.Cos(f);
					}
					else
					{
						float num16 = (float)Math.PI * 2f / 85f;
						int num17 = i - 72;
						f = (float)Math.PI - 9f * num16 + (float)num17 * num16;
						num4 = num - 520f + num3 * Mathf.Cos(f);
					}
					num5 = 384f - num3 * Mathf.Sin(f);
				}
				else
				{
					if (i < 102)
					{
						float num18 = (float)Math.PI * 2f / 85f;
						int num19 = i - 92;
						f = (0f - (float)num19) * num18 - num18;
						num4 = num - 640f + num3 * Mathf.Cos(f);
					}
					else if (i < 112)
					{
						float num20 = (float)Math.PI * 2f / 85f;
						int num21 = i - 102;
						f = (0f - (float)num21) * num20 - num20;
						num4 = num - 580f + num3 * Mathf.Cos(f);
					}
					else
					{
						float num22 = (float)Math.PI * 2f / 85f;
						int num23 = i - 112;
						f = 0f - num22 - (float)num23 * num22;
						num4 = num - 520f + num3 * Mathf.Cos(f);
					}
					num5 = 384f - num3 * Mathf.Sin(f);
				}
			}
			else if (i < 190)
			{
				FishType = STTF_FISH_TYPE.Fish_Zebra;
				if (i < 128)
				{
					num5 = num2 + 384f;
					num4 = ((i >= 125) ? (num - 520f - 180f - (float)((i - 125) * 60)) : (num - 520f + 180f + (float)((i - 122) * 60)));
				}
				else if (i < 134)
				{
					num5 = num2 + 384f - 60f;
					num4 = ((i >= 131) ? (num - 520f - 120f - (float)((i - 131) * 60)) : (num - 520f + 120f + (float)((i - 128) * 60)));
				}
				else if (i < 142)
				{
					num5 = num2 + 384f - 120f;
					num4 = ((i >= 138) ? (num - 520f - 60f - (float)((i - 138) * 60)) : (num - 520f + 60f + (float)((i - 134) * 60)));
				}
				else if (i < 151)
				{
					num5 = num2 + 384f - 180f;
					num4 = ((i >= 147) ? (num - 520f - 60f - (float)((i - 147) * 60)) : (num - 520f + (float)((i - 142) * 60)));
				}
				else if (i < 158)
				{
					num5 = num2 + 384f - 240f;
					num4 = ((i >= 155) ? (num - 520f - 60f - (float)((i - 155) * 60)) : (num - 520f + (float)((i - 151) * 60)));
				}
				else if (i < 159)
				{
					num5 = num2 + 384f - 300f;
					num4 = num - 520f;
				}
				else if (i < 165)
				{
					num5 = num2 + 384f + 60f;
					num4 = ((i >= 162) ? (num - 520f - 120f - (float)((i - 162) * 60)) : (num - 520f + 120f + (float)((i - 159) * 60)));
				}
				else if (i < 173)
				{
					num5 = num2 + 384f + 120f;
					num4 = ((i >= 169) ? (num - 520f - 60f - (float)((i - 169) * 60)) : (num - 520f + 60f + (float)((i - 165) * 60)));
				}
				else if (i < 182)
				{
					num5 = num2 + 384f + 180f;
					num4 = ((i >= 178) ? (num - 520f - 60f - (float)((i - 178) * 60)) : (num - 520f + (float)((i - 173) * 60)));
				}
				else if (i < 189)
				{
					num5 = num2 + 384f + 240f;
					num4 = ((i >= 186) ? (num - 520f - 60f - (float)((i - 186) * 60)) : (num - 520f + (float)((i - 182) * 60)));
				}
				else
				{
					num5 = num2 + 384f + 300f;
					num4 = num - 520f;
				}
			}
			else if (i < 258)
			{
				FishType = STTF_FISH_TYPE.Fish_Grass;
				if (i < 196)
				{
					num5 = num2 + 384f - 15f;
					num4 = ((i >= 193) ? (num - 520f - 180f + 25f - (float)((i - 193) * 60)) : (num - 520f + 180f + 25f + (float)((i - 190) * 60)));
				}
				else if (i < 202)
				{
					num5 = num2 + 384f - 60f - 15f;
					num4 = ((i >= 199) ? (num - 520f - 120f + 25f - (float)((i - 199) * 60)) : (num - 520f + 120f + 25f + (float)((i - 196) * 60)));
				}
				else if (i < 210)
				{
					num5 = num2 + 384f - 120f - 15f;
					num4 = ((i >= 206) ? (num - 520f - 60f + 25f - (float)((i - 206) * 60)) : (num - 520f + 60f + 25f + (float)((i - 202) * 60)));
				}
				else if (i < 219)
				{
					num5 = num2 + 384f - 180f - 15f;
					num4 = ((i >= 215) ? (num - 520f - 60f + 25f - (float)((i - 215) * 60)) : (num - 520f + 25f + (float)((i - 210) * 60)));
				}
				else if (i < 226)
				{
					num5 = num2 + 384f - 240f - 15f;
					num4 = ((i >= 223) ? (num - 520f - 60f + 25f - (float)((i - 223) * 60)) : (num - 520f + 25f + (float)((i - 219) * 60)));
				}
				else if (i < 227)
				{
					num5 = num2 + 384f - 300f - 15f;
					num4 = num - 520f + 25f;
				}
				else if (i < 233)
				{
					num5 = num2 + 384f + 60f - 15f;
					num4 = ((i >= 230) ? (num - 520f - 120f + 25f - (float)((i - 230) * 60)) : (num - 520f + 120f + 25f + (float)((i - 227) * 60)));
				}
				else if (i < 241)
				{
					num5 = num2 + 384f + 120f - 15f;
					num4 = ((i >= 237) ? (num - 520f - 60f + 25f - (float)((i - 237) * 60)) : (num - 520f + 60f + 25f + (float)((i - 233) * 60)));
				}
				else if (i < 250)
				{
					num5 = num2 + 384f + 180f - 15f;
					num4 = ((i >= 246) ? (num - 520f - 60f + 25f - (float)((i - 246) * 60)) : (num - 520f + 25f + (float)((i - 241) * 60)));
				}
				else if (i < 257)
				{
					num5 = num2 + 384f + 240f - 15f;
					num4 = ((i >= 254) ? (num - 520f - 60f + 25f - (float)((i - 254) * 60)) : (num - 520f + 25f + (float)((i - 250) * 60)));
				}
				else
				{
					num5 = num2 + 384f + 300f - 15f;
					num4 = num - 520f + 25f;
				}
			}
			f = 0f;
			num4 = (num4 - 800f) / 100f;
			num5 = (num5 - 400f) / 100f;
			int i2 = i;
			FormationType = STTF_FORMATION.Formation_Wedding;
			CreateAFish(FormationType, num4, num5, f, FishType, i2, num);
		}
	}

	private void CreateFormation_TwoArmies()
	{
		float num = 0f;
		float num2 = -300f;
		float num3 = 384f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float rot = 0f;
		for (int i = 0; i < 218; i++)
		{
			if (i < 109)
			{
				int num7;
				if (i < 29)
				{
					FishType = STTF_FISH_TYPE.Fish_Zebra;
					num4 = 240f;
					num7 = i;
					num = 0.216661572f;
				}
				else if (i < 88)
				{
					FishType = STTF_FISH_TYPE.Fish_Shrimp;
					num4 = 310f;
					num7 = i - 29;
					num = 0.106494673f;
				}
				else if (i < 108)
				{
					FishType = STTF_FISH_TYPE.Fish_BigEars;
					num4 = 160f;
					num7 = i - 88;
					num = (float)Math.PI / 10f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_SilverShark;
					num4 = 0f;
					num7 = 0;
				}
				rot = (float)num7 * num;
				num5 = num2 + num4 * Mathf.Cos(rot);
				num6 = num3 - num4 * Mathf.Sin(rot);
				rot = 0f;
			}
			else if (i < 218)
			{
				int num8;
				if (i < 138)
				{
					FishType = STTF_FISH_TYPE.Fish_Grass;
					num4 = 250f;
					num8 = i - 109;
					num = 0.216661572f;
				}
				else if (i < 197)
				{
					FishType = STTF_FISH_TYPE.Fish_Shrimp;
					num4 = 310f;
					num8 = i - 138;
					num = 0.106494673f;
				}
				else if (i < 217)
				{
					FishType = STTF_FISH_TYPE.Fish_YellowSpot;
					num4 = 170f;
					num8 = i - 197;
					num = (float)Math.PI / 10f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_GoldenShark;
					num5 = num2 + 600f + 1366f;
					num4 = 0f;
					num8 = 1;
				}
				rot = (float)num8 * num;
				num5 = num2 + 650f + 1366f + num4 * Mathf.Cos(rot);
				num6 = num3 - num4 * Mathf.Sin(rot);
				rot = (float)Math.PI;
			}
			num5 = (num5 - 700f) / 100f;
			num6 = (num6 - 400f) / 100f;
			int i2 = i;
			FormationType = STTF_FORMATION.Formation_TwoArmies;
			CreateAFish(FormationType, num5, num6, rot, FishType, i2, num4);
		}
	}

	private void CreateFormationSymmetry()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		for (int i = 0; i < 264; i++)
		{
			float f;
			if (i < 4)
			{
				FishType = STTF_FISH_TYPE.Fish_Bat;
				if (i < 2)
				{
					num4 = 100 + i % 2 * 300;
					num5 = 100 + i % 2 * -200;
				}
				else if (i < 4)
				{
					num4 = 100 + i % 2 * 300;
					num5 = 100 + (i - 1) % 2 * -200;
				}
			}
			else if (i < 84)
			{
				FishType = STTF_FISH_TYPE.Fish_Shrimp;
				float num6 = (float)Math.PI / 40f;
				num3 = 320f;
				int num7 = i - 4;
				float num8 = num;
				float num9 = num2;
				f = (0f - (float)num7) * num6;
				num4 = num8 + num3 * Mathf.Cos(f);
				num5 = num9 + num3 * Mathf.Sin(f);
			}
			else if (i < 164)
			{
				FishType = STTF_FISH_TYPE.Fish_Shrimp;
				float num10 = (float)Math.PI / 40f;
				num3 = 320f;
				int num11 = i - 80 - 4;
				float num12 = 600f;
				float num13 = num2;
				f = (0f - (float)num11) * num10;
				num4 = num12 + num3 * Mathf.Cos(f);
				num5 = num13 + num3 * Mathf.Sin(f);
			}
			else if (i < 184)
			{
				FishType = STTF_FISH_TYPE.Fish_BlueAlgae;
				float num14 = (float)Math.PI / 20f;
				num3 = 280f;
				int num15 = i - 160 - 4;
				float num16 = 60f;
				float num17 = num2;
				f = (0f - (float)num15) * num14;
				f += 4.712389f;
				num4 = num16 + num3 * Mathf.Cos(f);
				num5 = num17 + num3 * Mathf.Sin(f);
			}
			else if (i < 204)
			{
				FishType = STTF_FISH_TYPE.Fish_BlueAlgae;
				float num18 = (float)Math.PI / 20f;
				num3 = 280f;
				int num19 = i - 180 - 4;
				float num20 = 500f;
				float num21 = num2;
				f = (0f - (float)num19) * num18;
				f += (float)Math.PI / 2f;
				num4 = num20 + num3 * Mathf.Cos(f);
				num5 = num21 + num3 * Mathf.Sin(f);
			}
			else if (i < 364)
			{
				FishType = STTF_FISH_TYPE.Fish_YellowSpot;
				float num22 = (float)Math.PI / 30f;
				num3 = 320f;
				int num23 = i - 200 - 4;
				float num24 = 290f;
				float num25 = num2;
				f = (0f - (float)num23) * num22;
				num4 = num24 + num3 * Mathf.Cos(f);
				num5 = num25 + num3 * Mathf.Sin(f);
			}
			num4 = (num4 - 1600f) / 100f;
			num5 /= 100f;
			f = 0f;
			FormationType = STTF_FORMATION.Formation_Symmetry;
			CreateAFish(FormationType, num4, num5, f, FishType, i, num3);
		}
	}

	private void CreateFormationSixCircles()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = -10f;
		for (int i = 0; i < 185; i++)
		{
			float num9;
			float num10;
			float f;
			if (i < 100)
			{
				FishType = STTF_FISH_TYPE.Fish_Shrimp;
				num5 = 320f;
				float num7 = (float)Math.PI / 50f;
				int num8 = i;
				f = (0f - (float)num8) * num7 + (float)Math.PI / 2f;
				num9 = num + num3 + num5 * Mathf.Cos(f);
				num10 = num2 + num4 + num5 * Mathf.Sin(f);
				num9 = num9 / 100f + num6;
				num10 /= 100f;
			}
			else if (i < 117)
			{
				if (i < 101)
				{
					FishType = STTF_FISH_TYPE.Fish_CoralReefs;
					UnityEngine.Debug.Log(FishType);
					num9 = 1.271549f + num6;
					num10 = -1.226139f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_Grass;
					float num11 = (float)Math.PI / 8f;
					num5 = 90f;
					int num12 = i - 101;
					num3 = 120f;
					num4 = -125f;
					f = (0f - (float)num12) * num11;
					num9 = num + num3 + num5 * Mathf.Cos(f);
					num10 = num2 + num4 + num5 * Mathf.Sin(f);
					num9 = num9 / 100f + num6;
					num10 /= 100f;
				}
			}
			else if (i < 134)
			{
				if (i < 118)
				{
					FishType = STTF_FISH_TYPE.Fish_CoralReefs;
					num3 = -100f;
					num4 = 120f;
					num9 = -1.15272f + num6;
					num10 = 1.2f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_YellowSpot;
					float num13 = (float)Math.PI / 8f;
					num5 = 90f;
					int num14 = i - 118;
					num3 = -120f;
					num4 = 120f;
					f = (0f - (float)num14) * num13;
					num9 = num + num3 + num5 * Mathf.Cos(f);
					num10 = num2 + num4 + num5 * Mathf.Sin(f);
					num9 = num9 / 100f + num6;
					num10 /= 100f;
				}
			}
			else if (i < 151)
			{
				if (i < 135)
				{
					FishType = STTF_FISH_TYPE.Fish_CoralReefs;
					num3 = 100f;
					num4 = -120f;
					num9 = 1.2f + num6;
					num10 = 1.2f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_Zebra;
					float num15 = (float)Math.PI / 8f;
					num5 = 90f;
					int num16 = i - 135;
					num3 = 120f;
					num4 = 120f;
					f = (0f - (float)num16) * num15;
					num9 = num + num3 + num5 * Mathf.Cos(f);
					num10 = num2 + num4 + num5 * Mathf.Sin(f);
					num9 = num9 / 100f + num6;
					num10 /= 100f;
				}
			}
			else if (i < 168)
			{
				if (i < 152)
				{
					FishType = STTF_FISH_TYPE.Fish_CoralReefs;
					UnityEngine.Debug.Log(FishType);
					num3 = 100f;
					num4 = -120f;
					num9 = -1.028679f + num6;
					num10 = -1.343764f;
				}
				else
				{
					FishType = STTF_FISH_TYPE.Fish_BigEars;
					float num17 = (float)Math.PI / 8f;
					num5 = 90f;
					int num18 = i - 152;
					num3 = -120f;
					num4 = -125f;
					f = (0f - (float)num18) * num17;
					num9 = num + num3 + num5 * Mathf.Cos(f);
					num10 = num2 + num4 + num5 * Mathf.Sin(f);
					num9 = num9 / 100f + num6;
					num10 /= 100f;
				}
			}
			else if (i < 169)
			{
				FishType = STTF_FISH_TYPE.Fish_CoralReefs;
				num3 = 100f;
				num4 = -120f;
				num9 = num6;
				num10 = 0f;
			}
			else
			{
				FishType = STTF_FISH_TYPE.Fish_Ugly;
				float num19 = (float)Math.PI / 8f;
				num5 = 90f;
				int num20 = i - 169;
				num3 = 0f;
				num4 = 0f;
				f = (0f - (float)num20) * num19;
				num9 = num + num3 + num5 * Mathf.Cos(f);
				num10 = num2 + num4 + num5 * Mathf.Sin(f);
				num9 = num9 / 100f + num6;
				num10 /= 100f;
			}
			f = 0f;
			FormationType = STTF_FORMATION.Formation_SixCircles;
			CreateAFish(FormationType, num9, num10, f, FishType, i, num5);
		}
	}
}
