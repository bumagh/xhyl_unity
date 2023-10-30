using GameCommon;
using System;
using UnityEngine;

public class STTF_FishForFormation : MonoBehaviour
{
	public enum FishState
	{
		FishState_Sleep,
		FishState_Alive
	}

	private Vector3 newVector;

	private float r;

	private STTF_FISH_TYPE FishType;

	private float totalTime;

	private float Speed;

	private bool Move;

	private int Index;

	private int II;

	private float Rot;

	private float IniX;

	private float IniY;

	private float s;

	private float ds;

	private GameObject objFish;

	public STTF_FORMATION FormationType;

	public FishState FishStates;

	public static STTF_FishForFormation G_FishFormation;

	public static STTF_FishForFormation GetSingleton()
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

	public void InitNormalFish(STTF_FORMATION formationType, float x, float y, float rot, float index, STTF_FISH_TYPE fishtype, int i)
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
		II = i;
		r = index;
		newVector = base.transform.position;
		newVector.x = x;
		newVector.y = y;
		base.transform.position = newVector;
		Speed = 0.55f;
		totalTime = 0f;
		if (FormationType == STTF_FORMATION.Formation_RedCarpet)
		{
			if (FishType == STTF_FISH_TYPE.Fish_Shrimp)
			{
				Speed = 0.8f;
			}
			else
			{
				Speed = 1.35f;
			}
		}
		else if (FormationType == STTF_FORMATION.Formation_BigFishes)
		{
			Speed = 1.255f;
		}
		else if (FormationType == STTF_FORMATION.Formation_Fluctuate)
		{
			Speed = 1f;
		}
		else if (FormationType == STTF_FORMATION.Formation_NineCircle)
		{
			Speed = 1.6f;
		}
		else if (FormationType == STTF_FORMATION.Formation_Hexagonal)
		{
			newVector = base.transform.eulerAngles;
			newVector.x = rot / (float)Math.PI * 180f;
			newVector.z = 0f;
			newVector.y = 270f;
			base.transform.eulerAngles = newVector;
			Speed = 0.9f;
		}
		else if (FormationType == STTF_FORMATION.Formation_TwoCircles)
		{
			Speed = 0.6f;
		}
		else if (FormationType == STTF_FORMATION.Formation_Cars)
		{
			Speed = 0.6f;
		}
		else if (FormationType == STTF_FORMATION.Formation_Wedding)
		{
			Speed = 0.6f;
		}
		else if (FormationType == STTF_FORMATION.Formation_SixCircles)
		{
			objFish.SetActive(value: true);
		}
		else if (FormationType == STTF_FORMATION.Formation_Symmetry)
		{
			Speed = 1f;
		}
		else if (FormationType == STTF_FORMATION.Formation_TwoArmies)
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
		switch (FormationType)
		{
		case STTF_FORMATION.Formation_BigFishes:
		{
			newVector = base.transform.position;
			float num2 = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			if (FishType == STTF_FISH_TYPE.Fish_Toad || FishType == STTF_FISH_TYPE.Fish_Beauty)
			{
				newVector.x += num2 * 1366f / 1567f;
				newVector.y -= num2 * 768f / 1567f;
			}
			else if (FishType == STTF_FISH_TYPE.Fish_Dragon || FishType == STTF_FISH_TYPE.Fish_Butterfly)
			{
				newVector.x += num2 * 1366f / 1567f;
				newVector.y += num2 * 768f / 1567f;
			}
			else if (FishType == STTF_FISH_TYPE.Fish_GoldenShark || FishType == STTF_FISH_TYPE.Fish_Arrow)
			{
				newVector.x -= num2 * 1366f / 1567f;
				newVector.y += num2 * 768f / 1567f;
			}
			else if (FishType == STTF_FISH_TYPE.Fish_Bat || FishType == STTF_FISH_TYPE.Fish_SilverShark)
			{
				newVector.x -= num2 * 1366f / 1567f;
				newVector.y -= num2 * 768f / 1567f;
			}
			base.transform.position = newVector;
			break;
		}
		case STTF_FORMATION.Formation_Hexagonal:
			HexagonalUpdate();
			break;
		case STTF_FORMATION.Formation_NineCircle:
		{
			newVector = base.transform.position;
			float num9 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			if (FishType == STTF_FISH_TYPE.Fish_Shrimp || (totalTime > 6f && FishType == STTF_FISH_TYPE.Fish_YellowSpot) || (totalTime > 12f && FishType == STTF_FISH_TYPE.Fish_Ugly) || (totalTime > 18f && FishType == STTF_FISH_TYPE.Fish_Hedgehog) || (totalTime > 24f && FishType == STTF_FISH_TYPE.Fish_Lamp) || (totalTime > 36f && FishType == STTF_FISH_TYPE.Fish_Trailer) || (totalTime > 30f && FishType == STTF_FISH_TYPE.Fish_Turtle) || (totalTime > 42f && FishType == STTF_FISH_TYPE.Fish_Arrow) || (totalTime > 45f && FishType == STTF_FISH_TYPE.Fish_SilverShark))
			{
				objFish.SetActive(value: true);
				newVector.x += num9 * Mathf.Cos(Rot) / 2f;
				newVector.y -= num9 * Mathf.Sin(Rot) / 2f;
			}
			base.transform.position = newVector;
			break;
		}
		case STTF_FORMATION.Formation_Fluctuate:
		{
			newVector = base.transform.position;
			float num10 = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			totalTime += Time.deltaTime;
			if (Rot == -(float)Math.PI / 2f)
			{
				if ((FishType == STTF_FISH_TYPE.Fish_Trailer && totalTime > 30f) || (FishType == STTF_FISH_TYPE.Fish_Lamp && totalTime > 24f) || (FishType == STTF_FISH_TYPE.Fish_YellowSpot && totalTime > 16f) || (FishType == STTF_FISH_TYPE.Fish_Zebra && totalTime > 8f) || (FishType == STTF_FISH_TYPE.Fish_Shrimp && totalTime > 1f))
				{
					newVector.y += num10;
				}
			}
			else if (Rot == (float)Math.PI / 2f && ((FishType == STTF_FISH_TYPE.Fish_Turtle && totalTime > 30f) || (FishType == STTF_FISH_TYPE.Fish_Hedgehog && totalTime > 24f) || (FishType == STTF_FISH_TYPE.Fish_Ugly && totalTime > 16f) || (FishType == STTF_FISH_TYPE.Fish_BigEars && totalTime > 8f) || (FishType == STTF_FISH_TYPE.Fish_Grass && totalTime > 1f)))
			{
				newVector.y -= num10;
			}
			base.transform.position = newVector;
			break;
		}
		case STTF_FORMATION.Formation_RedCarpet:
		{
			objFish.SetActive(value: true);
			totalTime += Time.deltaTime;
			float num7 = Speed * Time.deltaTime;
			newVector = base.transform.position;
			if (FishType == STTF_FISH_TYPE.Fish_Shrimp)
			{
				if (Rot == -(float)Math.PI / 2f)
				{
					if (newVector.y <= -2.6f + s || (totalTime > 38f && totalTime < 40f))
					{
						newVector.y += num7;
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
						newVector.y -= num7;
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
					newVector.x += num7;
				}
				else if (Rot == (float)Math.PI)
				{
					newVector.x -= num7;
				}
			}
			base.transform.position = newVector;
			break;
		}
		case STTF_FORMATION.Formation_SixCircles:
		{
			newVector = base.gameObject.transform.localPosition;
			float num3 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			newVector.x += 0.68f * num3 * Mathf.Cos(Rot);
			base.transform.localPosition = newVector;
			break;
		}
		case STTF_FORMATION.Formation_Wedding:
		{
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			float num4 = Speed * Time.deltaTime;
			newVector.x += 0.85f * num4;
			base.transform.localPosition = newVector;
			break;
		}
		case STTF_FORMATION.Formation_TwoCircles:
		{
			newVector = base.gameObject.transform.localPosition;
			float num8 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			objFish.SetActive(value: true);
			if ((FishType == STTF_FISH_TYPE.Fish_BigShark && totalTime > 46f) || (FishType == STTF_FISH_TYPE.Fish_Hedgehog && totalTime > 40f) || (FishType == STTF_FISH_TYPE.Fish_YellowSpot && totalTime > 32f) || (FishType == STTF_FISH_TYPE.Fish_Grass && totalTime > 30f) || (FishType == STTF_FISH_TYPE.Fish_Ugly && II > 10 && II < 40 && totalTime > 38f) || (FishType == STTF_FISH_TYPE.Fish_Ugly && II > 100 && II < 125 && totalTime > 42f) || (FishType == STTF_FISH_TYPE.Fish_Shrimp && totalTime > 28f) || (FishType == STTF_FISH_TYPE.Fish_Zebra && totalTime > 34f) || (FishType == STTF_FISH_TYPE.Fish_BigEars && totalTime > 36f) || (FishType == STTF_FISH_TYPE.Fish_Toad && totalTime > 45f))
			{
				newVector.x += 10f * num8 * Mathf.Cos(Rot);
				newVector.y -= 10f * num8 * Mathf.Sin(Rot);
			}
			else if (FishType == STTF_FISH_TYPE.Fish_Toad || FishType == STTF_FISH_TYPE.Fish_Hedgehog || FishType == STTF_FISH_TYPE.Fish_YellowSpot || FishType == STTF_FISH_TYPE.Fish_Grass || (FishType == STTF_FISH_TYPE.Fish_Ugly && II > 10 && II < 40))
			{
				Rot += Speed * Time.deltaTime;
				newVector.x = -3.25f - r * Mathf.Sin(Rot + (float)Math.PI) / 100f;
				newVector.y = (0f - r) * Mathf.Cos(Rot + (float)Math.PI) / 100f;
				base.gameObject.transform.eulerAngles = new Vector3(Rot / (float)Math.PI * 180f, 90f, 0f);
			}
			else if (FishType == STTF_FISH_TYPE.Fish_Shrimp || FishType == STTF_FISH_TYPE.Fish_Zebra || FishType == STTF_FISH_TYPE.Fish_BigEars || FishType == STTF_FISH_TYPE.Fish_BigShark || (FishType == STTF_FISH_TYPE.Fish_Ugly && II > 100 && II < 125))
			{
				Rot += Speed * Time.deltaTime;
				newVector.x = 3.25f - r * Mathf.Sin(Rot + (float)Math.PI) / 100f;
				newVector.y = (0f - r) * Mathf.Cos(Rot + (float)Math.PI) / 100f;
				base.gameObject.transform.eulerAngles = new Vector3(Rot / (float)Math.PI * 180f, 90f, 0f);
			}
			base.transform.localPosition = newVector;
			break;
		}
		case STTF_FORMATION.Formation_Cars:
		{
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			float num6 = Speed * Time.deltaTime;
			newVector.x += num6;
			base.transform.localPosition = newVector;
			break;
		}
		case STTF_FORMATION.Formation_Symmetry:
		{
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			float num5 = Speed * Time.deltaTime;
			newVector.x += 0.8f * num5;
			base.transform.localPosition = newVector;
			break;
		}
		case STTF_FORMATION.Formation_TwoArmies:
		{
			objFish.SetActive(value: true);
			newVector = base.gameObject.transform.localPosition;
			float num = Speed * Time.deltaTime;
			if (FishType == STTF_FISH_TYPE.Fish_Zebra || FishType == STTF_FISH_TYPE.Fish_BigEars || (FishType == STTF_FISH_TYPE.Fish_SilverShark && Rot == 0f))
			{
				newVector.x += 0.65f * num;
			}
			else if (FishType == STTF_FISH_TYPE.Fish_Grass || FishType == STTF_FISH_TYPE.Fish_YellowSpot || (FishType == STTF_FISH_TYPE.Fish_GoldenShark && Rot == (float)Math.PI))
			{
				newVector.x -= 0.65f * num;
			}
			else if (FishType == STTF_FISH_TYPE.Fish_Shrimp)
			{
				if (II > 25 && II < 90)
				{
					newVector.x += 0.65f * num;
				}
				else if (II > 135 && II < 200)
				{
					newVector.x -= 0.65f * num;
				}
			}
			base.transform.localPosition = newVector;
			break;
		}
		}
		if (InsideWindow() && FormationType != 0 && FishStates == FishState.FishState_Sleep)
		{
			FishStates = FishState.FishState_Alive;
		}
		if (FishStates == FishState.FishState_Alive && FormationType != 0 && !InsideWindow())
		{
			STTF_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
		}
	}

	private void HexagonalUpdate()
	{
		newVector = base.transform.position;
		float num = Speed * Time.deltaTime;
		totalTime += Time.deltaTime;
		if (FishType == STTF_FISH_TYPE.Fish_BigEars)
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
		if (FishType == STTF_FISH_TYPE.Fish_Shrimp || FishType == STTF_FISH_TYPE.Fish_Zebra || FishType == STTF_FISH_TYPE.Fish_Ugly || FishType == STTF_FISH_TYPE.Fish_Hedgehog || FishType == STTF_FISH_TYPE.Fish_Lamp)
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
