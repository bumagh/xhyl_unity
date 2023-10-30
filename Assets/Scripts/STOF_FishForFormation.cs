using GameCommon;
using System;
using UnityEngine;

public class STOF_FishForFormation : MonoBehaviour
{
	public enum FishState
	{
		FishState_Sleep,
		FishState_Alive
	}

	private Vector3 newVector;

	private float r;

	private STOF_FISH_TYPE FishType;

	private float totalTime;

	private float Speed;

	private bool Move;

	private int Index;

	private float Rot;

	private float IniX;

	private float IniY;

	private float s;

	private float ds;

	private GameObject objFish;

	public STOF_FORMATION FormationType;

	public FishState FishStates;

	public static STOF_FishForFormation G_FishFormation;

	public static STOF_FishForFormation GetSingleton()
	{
		return G_FishFormation;
	}

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

	public void InitNormalFish(STOF_FORMATION formationType, float x, float y, float rot, float index, STOF_FISH_TYPE fishtype, int i)
	{
		FishType = fishtype;
		FormationType = formationType;
		FishStates = FishState.FishState_Sleep;
		if (objFish == null)
		{
			objFish = base.transform.Find("Fish").gameObject;
		}
		objFish.SetActive(value: false);
		IniX = x;
		IniY = y;
		Rot = rot;
		r = index;
		newVector = base.transform.position;
		newVector.x = x;
		newVector.y = y;
		base.transform.position = newVector;
		Speed = 0.55f;
		totalTime = 0f;
		if (FormationType == STOF_FORMATION.Formation_RedCarpet)
		{
			if (FishType == STOF_FISH_TYPE.Fish_Shrimp)
			{
				Speed = 0.8f;
			}
			else
			{
				Speed = 1.35f;
			}
		}
		else if (FormationType == STOF_FORMATION.Formation_BigFishes)
		{
			Speed = 1.255f;
		}
		else if (FormationType == STOF_FORMATION.Formation_Fluctuate)
		{
			Speed = 1f;
		}
		else if (FormationType == STOF_FORMATION.Formation_ConcentricCircles)
		{
			Speed = 0.6f;
			ds = 0f;
			if (FishType == STOF_FISH_TYPE.Fish_GoldenSharkB)
			{
				Rot -= (float)Math.PI / 2f;
				objFish.SetActive(value: true);
			}
			else if (FishType == STOF_FISH_TYPE.Fish_Ugly)
			{
				s = i - 1;
				Rot = rot;
			}
			else if (FishType == STOF_FISH_TYPE.Fish_BigEars)
			{
				s = i - 14;
				Rot = rot;
			}
			else if (FishType == STOF_FISH_TYPE.Fish_Zebra)
			{
				s = i - 38;
				Rot = rot;
			}
			else if (FishType == STOF_FISH_TYPE.Fish_Shrimp)
			{
				s = i - 78;
				Rot = rot;
			}
		}
		else if (FormationType == STOF_FORMATION.Formation_NineCircle)
		{
			Speed = 1.6f;
		}
		else if (FormationType == STOF_FORMATION.Formation_Hexagonal)
		{
			newVector = base.transform.eulerAngles;
			newVector.x = rot / (float)Math.PI * 180f;
			newVector.z = 0f;
			newVector.y = 270f;
			base.transform.eulerAngles = newVector;
			Speed = 0.9f;
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
		switch (FormationType)
		{
		case STOF_FORMATION.Formation_BigFishes:
		{
			newVector = base.transform.position;
			float num3 = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			if (FishType == STOF_FISH_TYPE.Fish_GoldenDragon || FishType == STOF_FISH_TYPE.Fish_Beauty)
			{
				newVector.x += num3 * 1366f / 1567f;
				newVector.y -= num3 * 768f / 1567f;
			}
			else if (FishType == STOF_FISH_TYPE.Fish_GoldenSharkB || FishType == STOF_FISH_TYPE.Fish_Butterfly)
			{
				newVector.x += num3 * 1366f / 1567f;
				newVector.y += num3 * 768f / 1567f;
			}
			else if (FishType == STOF_FISH_TYPE.Fish_GoldenShark || FishType == STOF_FISH_TYPE.Fish_Arrow)
			{
				newVector.x -= num3 * 1366f / 1567f;
				newVector.y += num3 * 768f / 1567f;
			}
			else if (FishType == STOF_FISH_TYPE.Fish_Bat || FishType == STOF_FISH_TYPE.Fish_SilverShark)
			{
				newVector.x -= num3 * 1366f / 1567f;
				newVector.y -= num3 * 768f / 1567f;
			}
			base.transform.position = newVector;
			break;
		}
		case STOF_FORMATION.Formation_Hexagonal:
			HexagonalUpdate();
			break;
		case STOF_FORMATION.Formation_NineCircle:
		{
			newVector = base.transform.position;
			float num5 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			if (FishType == STOF_FISH_TYPE.Fish_Shrimp || (totalTime > 6f && FishType == STOF_FISH_TYPE.Fish_YellowSpot) || (totalTime > 12f && FishType == STOF_FISH_TYPE.Fish_Ugly) || (totalTime > 18f && FishType == STOF_FISH_TYPE.Fish_Hedgehog) || (totalTime > 24f && FishType == STOF_FISH_TYPE.Fish_Lamp) || (totalTime > 36f && FishType == STOF_FISH_TYPE.Fish_Trailer) || (totalTime > 30f && FishType == STOF_FISH_TYPE.Fish_Turtle) || (totalTime > 42f && FishType == STOF_FISH_TYPE.Fish_Arrow) || (totalTime > 45f && FishType == STOF_FISH_TYPE.Fish_SilverShark))
			{
				objFish.SetActive(value: true);
				newVector.x += num5 * Mathf.Cos(Rot) / 2f;
				newVector.y -= num5 * Mathf.Sin(Rot) / 2f;
			}
			base.transform.position = newVector;
			break;
		}
		case STOF_FORMATION.Formation_ConcentricCircles:
		{
			int num = 0;
			newVector = base.transform.position;
			ds += Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			if (FishType == STOF_FISH_TYPE.Fish_Ugly)
			{
				num = 13;
			}
			else if (FishType == STOF_FISH_TYPE.Fish_BigEars)
			{
				num = 24;
			}
			else if (FishType == STOF_FISH_TYPE.Fish_Zebra || FishType == STOF_FISH_TYPE.Fish_Shrimp)
			{
				num = 40;
			}
			if (!objFish.activeSelf && ds >= (float)Math.PI * 2f / (float)num * s)
			{
				objFish.SetActive(value: true);
				base.transform.eulerAngles = Vector3.right * Rot / (float)Math.PI * 180f + Vector3.up * 90f;
			}
			if ((totalTime > 45f && FishType == STOF_FISH_TYPE.Fish_GoldenSharkB) || (totalTime > 40f && FishType == STOF_FISH_TYPE.Fish_Ugly) || (totalTime > 35f && FishType == STOF_FISH_TYPE.Fish_BigEars) || (totalTime > 30f && FishType == STOF_FISH_TYPE.Fish_Zebra) || (totalTime > 25f && FishType == STOF_FISH_TYPE.Fish_Shrimp))
			{
				float num2 = Speed * Time.deltaTime;
				newVector.x += num2 * Mathf.Cos(Rot);
				newVector.y -= num2 * Mathf.Sin(Rot);
			}
			else
			{
				Rot += Speed * Time.deltaTime;
				newVector.x = 0f - r * Mathf.Sin(Rot + (float)Math.PI) / 100f;
				newVector.y = (0f - r) * Mathf.Cos(Rot + (float)Math.PI) / 100f;
				base.transform.eulerAngles = Vector3.right * Rot / (float)Math.PI * 180f + Vector3.up * 90f;
			}
			base.transform.position = newVector;
			break;
		}
		case STOF_FORMATION.Formation_Fluctuate:
		{
			newVector = base.transform.position;
			float num4 = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			totalTime += Time.deltaTime;
			if (Rot == -(float)Math.PI / 2f)
			{
				if ((FishType == STOF_FISH_TYPE.Fish_Trailer && totalTime > 30f) || (FishType == STOF_FISH_TYPE.Fish_Lamp && totalTime > 24f) || (FishType == STOF_FISH_TYPE.Fish_YellowSpot && totalTime > 16f) || (FishType == STOF_FISH_TYPE.Fish_Zebra && totalTime > 8f) || (FishType == STOF_FISH_TYPE.Fish_Shrimp && totalTime > 1f))
				{
					newVector.y += num4;
				}
			}
			else if (Rot == (float)Math.PI / 2f && ((FishType == STOF_FISH_TYPE.Fish_Turtle && totalTime > 30f) || (FishType == STOF_FISH_TYPE.Fish_Hedgehog && totalTime > 24f) || (FishType == STOF_FISH_TYPE.Fish_Ugly && totalTime > 16f) || (FishType == STOF_FISH_TYPE.Fish_BigEars && totalTime > 8f) || (FishType == STOF_FISH_TYPE.Fish_Grass && totalTime > 1f)))
			{
				newVector.y -= num4;
			}
			base.transform.position = newVector;
			break;
		}
		case STOF_FORMATION.Formation_RedCarpet:
		{
			objFish.SetActive(value: true);
			totalTime += Time.deltaTime;
			float num6 = Speed * Time.deltaTime;
			newVector = base.transform.position;
			if (FishType == STOF_FISH_TYPE.Fish_Shrimp)
			{
				if (Rot == -(float)Math.PI / 2f)
				{
					if (newVector.y <= -2.6f + s || (totalTime > 38f && totalTime < 40f))
					{
						newVector.y += num6;
					}
					else if (totalTime >= 40f)
					{
						newVector.y += 3f * (Mathf.Sin(0.0400000028f * (float)(Index % 5 + 1)) * Time.deltaTime + Speed * Time.deltaTime);
					}
				}
				else if (Rot == (float)Math.PI / 2f)
				{
					if (newVector.y >= 2.6f + s || (totalTime > 38f && totalTime < 40f))
					{
						newVector.y -= num6;
					}
					else if (totalTime >= 40f)
					{
						newVector.y -= 3f * Mathf.Sin(0.0400000028f * (float)(Index % 5 + 1) * Time.deltaTime + Speed * Time.deltaTime);
					}
				}
			}
			else if ((double)totalTime > 0.9)
			{
				if (Rot == 0f)
				{
					newVector.x += num6;
				}
				else if (Rot == (float)Math.PI)
				{
					newVector.x -= num6;
				}
			}
			base.transform.position = newVector;
			break;
		}
		case STOF_FORMATION.Formation_YaoQianShuL:
		case STOF_FORMATION.Formation_YaoQianShuR:
			newVector = base.gameObject.transform.position;
			if (Rot == 0f)
			{
				newVector.x += Speed * Time.deltaTime;
			}
			else if (Rot == (float)Math.PI)
			{
				newVector.x -= Speed * Time.deltaTime;
			}
			base.transform.position = newVector;
			break;
		}
		if (InsideWindow() && FormationType != 0 && FishStates == FishState.FishState_Sleep)
		{
			FishStates = FishState.FishState_Alive;
		}
		if (FishStates == FishState.FishState_Alive && FormationType != 0 && !InsideWindow())
		{
			STOF_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
		}
	}

	private void HexagonalUpdate()
	{
		newVector = base.transform.position;
		float num = Speed * Time.deltaTime;
		totalTime += Time.deltaTime;
		if (FishType == STOF_FISH_TYPE.Fish_BigEars)
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
		if (FishType == STOF_FISH_TYPE.Fish_Shrimp || FishType == STOF_FISH_TYPE.Fish_Zebra || FishType == STOF_FISH_TYPE.Fish_Ugly || FishType == STOF_FISH_TYPE.Fish_Hedgehog || FishType == STOF_FISH_TYPE.Fish_Lamp)
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
