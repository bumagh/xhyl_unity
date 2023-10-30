using GameCommon;
using System;
using UnityEngine;

public class STQM_FishForFormation : MonoBehaviour
{
	public enum FishState
	{
		FishState_Sleep,
		FishState_Alive
	}

	private Vector3 newVector;

	private float r;

	private STQM_FISH_TYPE FishType;

	private float totalTime;

	private float Speed;

	private bool Move;

	private int Index;

	private float Rot;

	private float IniX;

	private float IniY;

	private float s;

	private int II;

	private float ds;

	private GameObject objFish;

	private int[] a = new int[5]
	{
		0,
		1,
		2,
		3,
		4
	};

	public STQM_FORMATION formationType;

	private float[] arr = new float[480]
	{
		0.06807313f,
		0.1055535f,
		0.09931546f,
		0.1736708f,
		0.1729527f,
		0.09069102f,
		0.04040235f,
		0.111112f,
		0.007278894f,
		0.1749917f,
		0.1211426f,
		0.1871434f,
		0.07478809f,
		0.05786431f,
		0.04801437f,
		0.07160478f,
		0.1478723f,
		0.1649855f,
		0.003295587f,
		0.01405652f,
		0.1689751f,
		0.03349778f,
		0.1180732f,
		0.1464165f,
		0.1978718f,
		0.1789083f,
		0.191886f,
		0.1088348f,
		0.04359574f,
		0.1917632f,
		0.08090749f,
		0.004320438f,
		0.1060002f,
		0.1591235f,
		0.1129723f,
		0.1317076f,
		0.1204621f,
		0.2018194f,
		0.03096512f,
		0.04164356f,
		0.1505082f,
		0.06484502f,
		0.1337958f,
		0.1448762f,
		0.1036611f,
		0.06920794f,
		0.1463583f,
		0.05083386f,
		0.04379693f,
		0.1709212f,
		0.1306596f,
		0.08795077f,
		0.1467296f,
		0.06038149f,
		0.1432993f,
		0.08351699f,
		0.03517532f,
		0.1043258f,
		0.08709024f,
		0.1679051f,
		0.03322165f,
		0.1146416f,
		0.06606488f,
		0.05129436f,
		0.1316838f,
		0.07924936f,
		0.0611762f,
		0.1180996f,
		0.1126476f,
		0.1542889f,
		0.0644886f,
		0.02792462f,
		0.03248781f,
		0.1737493f,
		0.1889433f,
		0.1294035f,
		0.08419205f,
		0.1368916f,
		0.145835f,
		0.09861657f,
		0.02188721f,
		0.1176692f,
		0.07634439f,
		0.05512752f,
		509f / (995f * (float)Math.PI),
		0.1078601f,
		0.05145864f,
		0.1588733f,
		0.07151825f,
		0.1625767f,
		0.009954821f,
		0.03411497f,
		0.006412684f,
		0.2057645f,
		0.1753631f,
		0.1337081f,
		0.1099646f,
		0.02469837f,
		0.1213588f,
		0.1493031f,
		0.04097589f,
		0.1388665f,
		0.06110689f,
		0.03891236f,
		0.001128939f,
		0.1780353f,
		0.07174635f,
		0.1531681f,
		0.1651049f,
		0.03718533f,
		0.2063225f,
		0.1496962f,
		0.1787945f,
		0.1528819f,
		0.1158853f,
		0.1818402f,
		0.04593072f,
		0.04438141f,
		0.0303656f,
		0.02614116f,
		0.06807313f,
		0.1055535f,
		0.09931546f,
		0.1736708f,
		0.1729527f,
		0.09069102f,
		0.04040235f,
		0.111112f,
		0.007278894f,
		0.1749917f,
		0.1211426f,
		0.1871434f,
		0.07478809f,
		0.05786431f,
		0.04801437f,
		0.07160478f,
		0.1478723f,
		0.1649855f,
		0.003295587f,
		0.01405652f,
		0.1689751f,
		0.03349778f,
		0.1180732f,
		0.1464165f,
		0.1978718f,
		0.1789083f,
		0.191886f,
		0.1088348f,
		0.04359574f,
		0.1917632f,
		0.08090749f,
		0.004320438f,
		0.1060002f,
		0.1591235f,
		0.1129723f,
		0.1317076f,
		0.1204621f,
		0.2018194f,
		0.03096512f,
		0.04164356f,
		0.1505082f,
		0.06484502f,
		0.1337958f,
		0.1448762f,
		0.1036611f,
		0.06920794f,
		0.1463583f,
		0.05083386f,
		0.04379693f,
		0.1709212f,
		0.1306596f,
		0.08795077f,
		0.1467296f,
		0.06038149f,
		0.1432993f,
		0.08351699f,
		0.03517532f,
		0.1043258f,
		0.08709024f,
		0.1679051f,
		0.03322165f,
		0.1146416f,
		0.06606488f,
		0.05129436f,
		0.1316838f,
		0.07924936f,
		0.0611762f,
		0.1180996f,
		0.1126476f,
		0.1542889f,
		0.0644886f,
		0.02792462f,
		0.03248781f,
		0.1737493f,
		0.1889433f,
		0.1294035f,
		0.08419205f,
		0.1368916f,
		0.145835f,
		0.09861657f,
		0.02188721f,
		0.1176692f,
		0.07634439f,
		0.05512752f,
		509f / (995f * (float)Math.PI),
		0.1078601f,
		0.05145864f,
		0.1588733f,
		0.07151825f,
		0.1625767f,
		0.009954821f,
		0.03411497f,
		0.006412684f,
		0.2057645f,
		0.1753631f,
		0.1337081f,
		0.1099646f,
		0.02469837f,
		0.1213588f,
		0.1493031f,
		0.04097589f,
		0.1388665f,
		0.06110689f,
		0.03891236f,
		0.001128939f,
		0.1780353f,
		0.07174635f,
		0.1531681f,
		0.1651049f,
		0.03718533f,
		0.2063225f,
		-0.1496962f,
		0.1787945f,
		0.1528819f,
		0.1158853f,
		0.1818402f,
		0.04593072f,
		0.04438141f,
		0.0303656f,
		0.02614116f,
		0.06807313f,
		0.1055535f,
		0.09931546f,
		0.1736708f,
		0.1729527f,
		0.09069102f,
		0.04040235f,
		0.111112f,
		0.007278894f,
		0.1749917f,
		0.1211426f,
		0.1871434f,
		0.07478809f,
		0.05786431f,
		0.04801437f,
		0.07160478f,
		0.1478723f,
		0.1649855f,
		0.003295587f,
		0.01405652f,
		0.1689751f,
		0.03349778f,
		0.1180732f,
		0.1464165f,
		0.1978718f,
		0.1789083f,
		0.191886f,
		0.1088348f,
		0.04359574f,
		0.1917632f,
		0.08090749f,
		0.004320438f,
		0.1060002f,
		0.1591235f,
		0.1129723f,
		0.1317076f,
		0.1204621f,
		0.2018194f,
		0.03096512f,
		0.04164356f,
		0.1505082f,
		0.06484502f,
		0.1337958f,
		0.1448762f,
		0.1036611f,
		0.06920794f,
		0.1463583f,
		0.05083386f,
		0.04379693f,
		0.1709212f,
		0.1306596f,
		0.08795077f,
		0.1467296f,
		0.06038149f,
		0.1432993f,
		0.08351699f,
		0.03517532f,
		0.1043258f,
		0.08709024f,
		0.1679051f,
		0.03322165f,
		0.1146416f,
		0.06606488f,
		0.05129436f,
		0.1316838f,
		0.07924936f,
		0.0611762f,
		0.1180996f,
		0.1126476f,
		0.1542889f,
		0.0644886f,
		0.02792462f,
		0.03248781f,
		0.1737493f,
		0.1889433f,
		0.1294035f,
		0.08419205f,
		0.1368916f,
		0.145835f,
		0.09861657f,
		0.02188721f,
		0.1176692f,
		0.07634439f,
		0.05512752f,
		509f / (995f * (float)Math.PI),
		0.1078601f,
		0.05145864f,
		0.1588733f,
		0.07151825f,
		0.1625767f,
		0.009954821f,
		0.03411497f,
		0.006412684f,
		0.2057645f,
		0.1753631f,
		0.1337081f,
		0.1099646f,
		0.02469837f,
		0.1213588f,
		0.1493031f,
		0.04097589f,
		0.1388665f,
		0.06110689f,
		0.03891236f,
		0.001128939f,
		0.1780353f,
		0.07174635f,
		0.1531681f,
		0.1651049f,
		0.03718533f,
		0.2063225f,
		0.1496962f,
		0.1787945f,
		0.1528819f,
		0.1158853f,
		0.1818402f,
		0.04593072f,
		0.04438141f,
		0.0303656f,
		0.02614116f,
		0.06807313f,
		0.1055535f,
		0.09931546f,
		0.1736708f,
		0.1729527f,
		0.09069102f,
		0.04040235f,
		0.111112f,
		0.007278894f,
		0.1749917f,
		0.1211426f,
		0.1871434f,
		0.07478809f,
		0.05786431f,
		0.04801437f,
		0.07160478f,
		0.1478723f,
		0.1649855f,
		0.003295587f,
		0.01405652f,
		0.1689751f,
		0.03349778f,
		0.1180732f,
		0.1464165f,
		0.1978718f,
		0.1789083f,
		0.191886f,
		0.1088348f,
		0.04359574f,
		0.1917632f,
		0.08090749f,
		0.004320438f,
		0.1060002f,
		0.1591235f,
		0.1129723f,
		0.1317076f,
		0.1204621f,
		0.2018194f,
		0.03096512f,
		0.04164356f,
		0.1505082f,
		0.06484502f,
		-0.1337958f,
		0.1448762f,
		0.1036611f,
		0.06920794f,
		0.1463583f,
		0.05083386f,
		0.04379693f,
		0.1709212f,
		0.1306596f,
		0.08795077f,
		0.1467296f,
		0.06038149f,
		0.1432993f,
		0.08351699f,
		0.03517532f,
		0.1043258f,
		0.08709024f,
		0.1679051f,
		0.03322165f,
		0.1146416f,
		0.06606488f,
		0.05129436f,
		0.1316838f,
		0.07924936f,
		0.0611762f,
		0.1180996f,
		0.1126476f,
		0.1542889f,
		0.0644886f,
		0.02792462f,
		0.03248781f,
		0.1737493f,
		0.1889433f,
		0.1294035f,
		0.08419205f,
		0.1368916f,
		0.145835f,
		0.09861657f,
		0.02188721f,
		0.1176692f,
		0.07634439f,
		0.05512752f,
		509f / (995f * (float)Math.PI),
		0.1078601f,
		0.05145864f,
		0.1588733f,
		0.07151825f,
		0.1625767f,
		0.009954821f,
		0.03411497f,
		0.006412684f,
		0.2057645f,
		0.1753631f,
		0.1337081f,
		0.1099646f,
		0.02469837f,
		0.1213588f,
		0.1493031f,
		0.04097589f,
		0.1388665f,
		0.06110689f,
		0.03891236f,
		0.001128939f,
		0.1780353f,
		0.07174635f,
		0.1531681f,
		0.1651049f,
		0.03718533f,
		0.2063225f,
		0.1496962f,
		0.1787945f,
		0.1528819f,
		0.1158853f,
		0.1818402f,
		0.04593072f,
		0.04438141f,
		0.0303656f,
		0.02614116f
	};

	public FishState FishStates;

	private void Start()
	{
		newVector.x = 0f;
		newVector.y = 0f;
		newVector.z = 0f;
		if (objFish == null)
		{
			objFish = base.transform.Find("Fish").gameObject;
		}
	}

	public void InitNormalFish(STQM_FORMATION formationType, float x, float y, float rot, float index, STQM_FISH_TYPE fishtype, int i)
	{
		FishType = fishtype;
		this.formationType = formationType;
		FishStates = FishState.FishState_Sleep;
		if (objFish == null)
		{
			objFish = base.transform.Find("Fish").gameObject;
		}
		objFish.SetActive(value: false);
		IniX = x;
		IniY = y;
		Rot = rot;
		II = i;
		r = index;
		newVector = base.gameObject.transform.localPosition;
		newVector.x = x;
		newVector.y = y;
		base.gameObject.transform.localPosition = newVector;
		Speed = 0.55f;
		totalTime = 0f;
		if (this.formationType == STQM_FORMATION.Formation_RedCarpet)
		{
			if (FishType == STQM_FISH_TYPE.Fish_Shrimp)
			{
				Speed = 0.8f;
			}
			else
			{
				Speed = 1.35f;
			}
		}
		else if (this.formationType == STQM_FORMATION.Formation_BigFishes)
		{
			Speed = 1.255f;
		}
		else if (this.formationType == STQM_FORMATION.Formation_Fluctuate)
		{
			Speed = 1f;
		}
		else if (this.formationType == STQM_FORMATION.Formation_Hexagonal)
		{
			newVector = base.gameObject.transform.eulerAngles;
			newVector.x = rot / (float)Math.PI * 180f;
			newVector.z = 0f;
			newVector.y = 270f;
			base.gameObject.transform.eulerAngles = newVector;
			Speed = 0.9f;
		}
		else if (this.formationType == STQM_FORMATION.Formation_TwoCircles)
		{
			Speed = 0.6f;
		}
		else if (this.formationType == STQM_FORMATION.Formation_Cars)
		{
			Speed = 0.6f;
		}
		else if (this.formationType == STQM_FORMATION.Formation_Wedding)
		{
			Speed = 0.6f;
		}
		else if (this.formationType == STQM_FORMATION.Formation_SixCircles)
		{
			objFish.SetActive(value: true);
		}
		else if (this.formationType == STQM_FORMATION.Formation_Symmetry)
		{
			Speed = 1f;
		}
		else if (this.formationType == STQM_FORMATION.Formation_TwoArmies)
		{
			Speed = 0.6f;
		}
		Move = true;
	}

	private bool InsideWindow()
	{
		return newVector.x >= -12f && newVector.x <= 12f && newVector.y >= -7f && newVector.y <= 7f;
	}

	public void StartMove(bool move)
	{
		Move = move;
	}

	private void Update()
	{
		if (!Move)
		{
			return;
		}
		switch (formationType)
		{
		case STQM_FORMATION.Formation_Hexagonal:
			HexagonalUpdate();
			break;
		case STQM_FORMATION.Formation_BigFishes:
		{
			newVector = base.gameObject.transform.localPosition;
			float num4 = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			if (FishType == STQM_FISH_TYPE.Fish_BowlFish || FishType == STQM_FISH_TYPE.Fish_Butterfly)
			{
				newVector.x += num4 * 1366f / 1567f;
				newVector.y -= num4 * 768f / 1567f;
			}
			else if (FishType == STQM_FISH_TYPE.Fish_Beauty || FishType == STQM_FISH_TYPE.Fish_SuperPenguin)
			{
				newVector.x += num4 * 1366f / 1567f;
				newVector.y += num4 * 768f / 1567f;
			}
			else if (FishType == STQM_FISH_TYPE.Fish_GoldenShark || FishType == STQM_FISH_TYPE.Fish_Arrow)
			{
				newVector.x -= num4 * 1366f / 1567f;
				newVector.y += num4 * 768f / 1567f;
			}
			else if (FishType == STQM_FISH_TYPE.Fish_Bat || FishType == STQM_FISH_TYPE.Fish_SilverShark)
			{
				newVector.x -= num4 * 1366f / 1567f;
				newVector.y -= num4 * 768f / 1567f;
			}
			base.transform.localPosition = newVector;
			break;
		}
		case STQM_FORMATION.Formation_RedCarpet:
		{
			totalTime += Time.deltaTime;
			float num3 = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			if (FishType == STQM_FISH_TYPE.Fish_Shrimp)
			{
				if (Rot == -(float)Math.PI / 2f)
				{
					if (newVector.y <= -2.6f + s)
					{
						newVector.y += num3;
					}
					else if (totalTime >= 45f)
					{
						if (II % 5 == 0)
						{
							newVector.y += 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 1.1f * Speed * Time.deltaTime);
						}
						else if (II % 5 == 1)
						{
							newVector.y += 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 0.95f * Speed * Time.deltaTime);
						}
						else if (II % 5 == 2)
						{
							newVector.y += 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 0.9f * Speed * Time.deltaTime);
						}
						else if (II % 5 == 3)
						{
							newVector.y += 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 1.05f * Speed * Time.deltaTime);
						}
						else
						{
							newVector.y += 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 1f * Speed * Time.deltaTime);
						}
					}
				}
				else if (Rot == (float)Math.PI / 2f)
				{
					if (newVector.y >= 2.6f + s)
					{
						newVector.y -= num3;
					}
					else if (totalTime >= 45f)
					{
						if (II % 5 == 0)
						{
							newVector.y -= 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 0.95f * Speed * Time.deltaTime);
						}
						else if (II % 5 == 1)
						{
							newVector.y -= 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 1.05f * Speed * Time.deltaTime);
						}
						else if (II % 5 == 2)
						{
							newVector.y -= 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 0.9f * Speed * Time.deltaTime);
						}
						else if (II % 5 == 3)
						{
							newVector.y -= 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 1.1f * Speed * Time.deltaTime);
						}
						else
						{
							newVector.y -= 3f * Mathf.Sin(0.002f * (float)(Index % 5 + 1) * Time.deltaTime + 1f * Speed * Time.deltaTime);
						}
					}
				}
			}
			else if ((double)totalTime > 0.9)
			{
				if (Rot == 0f)
				{
					if (FishType == STQM_FISH_TYPE.Fish_BowlFish)
					{
						newVector.x -= 0.8f * num3;
					}
					else
					{
						newVector.x += 0.8f * num3;
					}
				}
				else if (Rot == (float)Math.PI)
				{
					if (FishType == STQM_FISH_TYPE.Fish_BowlFish)
					{
						newVector.x += 0.8f * num3;
					}
					else
					{
						newVector.x -= 0.8f * num3;
					}
				}
			}
			base.transform.localPosition = newVector;
			break;
		}
		case STQM_FORMATION.Formation_SixCircles:
		{
			newVector = base.gameObject.transform.localPosition;
			float num6 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			newVector.x += 0.68f * num6 * Mathf.Cos(Rot);
			base.transform.localPosition = newVector;
			break;
		}
		case STQM_FORMATION.Formation_Wedding:
		{
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			float num5 = Speed * Time.deltaTime;
			newVector.x += 0.85f * num5;
			base.transform.localPosition = newVector;
			break;
		}
		case STQM_FORMATION.Formation_TwoCircles:
		{
			newVector = base.gameObject.transform.localPosition;
			float num9 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			objFish.SetActive(value: true);
			if ((FishType == STQM_FISH_TYPE.Fish_BowlFish && totalTime > 46f) || (FishType == STQM_FISH_TYPE.Fish_Hedgehog && totalTime > 40f) || (FishType == STQM_FISH_TYPE.Fish_YellowSpot && totalTime > 32f) || (FishType == STQM_FISH_TYPE.Fish_Grass && totalTime > 30f) || (FishType == STQM_FISH_TYPE.Fish_Ugly && II > 10 && II < 40 && totalTime > 38f) || (FishType == STQM_FISH_TYPE.Fish_Ugly && II > 100 && II < 125 && totalTime > 42f) || (FishType == STQM_FISH_TYPE.Fish_Shrimp && totalTime > 28f) || (FishType == STQM_FISH_TYPE.Fish_Zebra && totalTime > 34f) || (FishType == STQM_FISH_TYPE.Fish_BigEars && totalTime > 36f) || (FishType == STQM_FISH_TYPE.Fish_SuperPenguin && totalTime > 45f))
			{
				newVector.x += 10f * num9 * Mathf.Cos(Rot);
				newVector.y -= 10f * num9 * Mathf.Sin(Rot);
			}
			else if (FishType == STQM_FISH_TYPE.Fish_BowlFish || FishType == STQM_FISH_TYPE.Fish_Hedgehog || FishType == STQM_FISH_TYPE.Fish_YellowSpot || FishType == STQM_FISH_TYPE.Fish_Grass || (FishType == STQM_FISH_TYPE.Fish_Ugly && II > 10 && II < 40))
			{
				Rot += Speed * Time.deltaTime;
				newVector.x = -3.25f - r * Mathf.Sin(Rot + (float)Math.PI) / 100f;
				newVector.y = (0f - r) * Mathf.Cos(Rot + (float)Math.PI) / 100f;
				base.gameObject.transform.eulerAngles = new Vector3(Rot / (float)Math.PI * 180f, 90f, 0f);
			}
			else if (FishType == STQM_FISH_TYPE.Fish_Shrimp || FishType == STQM_FISH_TYPE.Fish_Zebra || FishType == STQM_FISH_TYPE.Fish_BigEars || FishType == STQM_FISH_TYPE.Fish_SuperPenguin || (FishType == STQM_FISH_TYPE.Fish_Ugly && II > 100 && II < 125))
			{
				Rot += Speed * Time.deltaTime;
				newVector.x = 3.25f - r * Mathf.Sin(Rot + (float)Math.PI) / 100f;
				newVector.y = (0f - r) * Mathf.Cos(Rot + (float)Math.PI) / 100f;
				base.gameObject.transform.eulerAngles = new Vector3(Rot / (float)Math.PI * 180f, 90f, 0f);
			}
			base.transform.localPosition = newVector;
			break;
		}
		case STQM_FORMATION.Formation_Cars:
		{
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			float num7 = Speed * Time.deltaTime;
			newVector.x += num7;
			base.transform.localPosition = newVector;
			break;
		}
		case STQM_FORMATION.Formation_Symmetry:
		{
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			float num8 = Speed * Time.deltaTime;
			newVector.x += 0.8f * num8;
			base.transform.localPosition = newVector;
			break;
		}
		case STQM_FORMATION.Formation_TwoArmies:
		{
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			float num2 = Speed * Time.deltaTime;
			if (FishType == STQM_FISH_TYPE.Fish_Zebra || FishType == STQM_FISH_TYPE.Fish_BigEars || (FishType == STQM_FISH_TYPE.Fish_SilverShark && Rot == 0f))
			{
				newVector.x += 0.65f * num2;
			}
			else if (FishType == STQM_FISH_TYPE.Fish_Grass || FishType == STQM_FISH_TYPE.Fish_YellowSpot || (FishType == STQM_FISH_TYPE.Fish_GoldenShark && Rot == (float)Math.PI))
			{
				newVector.x -= 0.65f * num2;
			}
			else if (FishType == STQM_FISH_TYPE.Fish_Shrimp)
			{
				if (II > 25 && II < 90)
				{
					newVector.x += 0.65f * num2;
				}
				else if (II > 135 && II < 200)
				{
					newVector.x -= 0.65f * num2;
				}
			}
			base.transform.localPosition = newVector;
			break;
		}
		case STQM_FORMATION.Formation_Fluctuate:
		{
			newVector = base.gameObject.transform.localPosition;
			float num = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			totalTime += Time.deltaTime;
			if (FishType == STQM_FISH_TYPE.Fish_Shrimp)
			{
				if (Rot == -(float)Math.PI / 2f)
				{
					newVector.y += 0.45f * num;
				}
				else if (Rot == (float)Math.PI / 2f)
				{
					newVector.y -= 0.45f * num;
				}
			}
			else if (newVector.x >= -7f)
			{
				if (FishType == STQM_FISH_TYPE.Fish_BowlFish)
				{
					newVector.x += num;
				}
				else
				{
					newVector.x += num * Mathf.Cos(Rot);
					newVector.y -= num * Mathf.Sin(Rot);
				}
			}
			else
			{
				newVector.x += num;
			}
			base.transform.localPosition = newVector;
			break;
		}
		}
		if (InsideWindow() && formationType != 0 && FishStates == FishState.FishState_Sleep)
		{
			FishStates = FishState.FishState_Alive;
		}
		if (FishStates == FishState.FishState_Alive && formationType != 0 && !InsideWindow())
		{
			STQM_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
		}
	}

	private void HexagonalUpdate()
	{
		newVector = base.transform.position;
		float num = Speed * Time.deltaTime;
		totalTime += Time.deltaTime;
		if (FishType == STQM_FISH_TYPE.Fish_BigEars)
		{
			if (-(float)Math.PI / 15f <= Rot && Rot <= (float)Math.PI / 15f && (double)totalTime > 3.4)
			{
				objFish.SetActive(value: true);
				newVector.x -= num * Mathf.Cos(Rot);
				newVector.y += num * Mathf.Sin(Rot);
			}
			else if (3.97935081f <= Rot && Rot <= 4.3982296f && (double)totalTime > 3.7)
			{
				objFish.SetActive(value: true);
				newVector.x += num * Mathf.Cos(Rot + (float)Math.PI);
				newVector.y += num * Mathf.Sin(Rot + (float)Math.PI);
			}
			else if ((float)Math.PI * 3f / 5f <= Rot && Rot <= (float)Math.PI * 11f / 15f && totalTime > 4f)
			{
				objFish.SetActive(value: true);
				newVector.x += num * Mathf.Cos(Rot + (float)Math.PI);
				newVector.y -= num * Mathf.Cos(Rot + (float)Math.PI);
			}
		}
		if (FishType == STQM_FISH_TYPE.Fish_Shrimp || FishType == STQM_FISH_TYPE.Fish_Zebra || FishType == STQM_FISH_TYPE.Fish_Ugly || FishType == STQM_FISH_TYPE.Fish_Hedgehog || FishType == STQM_FISH_TYPE.Fish_Lamp)
		{
			if (5.550147f <= Rot && Rot <= 5.96902657f)
			{
				if (newVector.x >= 0f)
				{
					newVector.x -= 0.866f * num;
					newVector.y += 0.5f * num;
				}
				else
				{
					objFish.SetActive(value: true);
					newVector.x -= num * Mathf.Cos(Rot);
					newVector.y -= num * Mathf.Sin(Rot);
				}
			}
			else if (4.50294971f <= Rot && Rot <= 4.92182827f)
			{
				if (newVector.y <= 0f)
				{
					newVector.y += num;
				}
				else
				{
					objFish.SetActive(value: true);
					if (Rot == 4.712389f)
					{
						newVector.y += num;
					}
					else
					{
						newVector.x += num * Mathf.Cos(Rot);
						newVector.y -= num * Mathf.Sin(Rot);
					}
				}
			}
			else if (3.45575213f <= Rot && Rot <= 3.87463117f)
			{
				if (newVector.x <= 0f)
				{
					newVector.x += 0.866f * num;
					newVector.y += 0.5f * num;
				}
				else
				{
					objFish.SetActive(value: true);
					newVector.x -= num * Mathf.Cos(Rot);
					newVector.y -= num * Mathf.Sin(Rot);
				}
			}
			else if (2.40855455f <= Rot && Rot <= 2.82743359f)
			{
				if (newVector.x <= 0f)
				{
					newVector.x += 0.866f * num;
					newVector.y -= 0.5f * num;
				}
				else
				{
					objFish.SetActive(value: true);
					newVector.x -= num * Mathf.Cos(Rot);
					newVector.y -= num * Mathf.Sin(Rot);
				}
			}
			else if ((float)Math.PI * 13f / 30f <= Rot && Rot <= (float)Math.PI * 17f / 30f)
			{
				if (newVector.y >= 0f)
				{
					newVector.y -= num;
				}
				else
				{
					objFish.SetActive(value: true);
					if (Rot == (float)Math.PI / 2f)
					{
						newVector.y -= num;
					}
					else
					{
						newVector.x += num * Mathf.Cos(Rot);
						newVector.y -= num * Mathf.Sin(Rot);
					}
				}
			}
			else if ((float)Math.PI / 10f <= Rot && Rot <= (float)Math.PI * 7f / 30f)
			{
				if (newVector.x >= 0f)
				{
					newVector.x -= 0.866f * num;
					newVector.y -= 0.5f * num;
				}
				else
				{
					objFish.SetActive(value: true);
					newVector.x -= num * Mathf.Cos(Rot);
					newVector.y -= num * Mathf.Sin(Rot);
				}
			}
		}
		base.transform.position = newVector;
	}
}
