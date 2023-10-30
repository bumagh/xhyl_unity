using GameCommon;
using System;
using UnityEngine;

public class DNTG_FishForFormation : MonoBehaviour
{
	public enum FishState
	{
		FishState_Sleep,
		FishState_Alive
	}

	private Vector3 newVector;

	private float r;

	private DNTG_FISH_TYPE FishType;

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

	[HideInInspector]
	public DNTG_FORMATION FormationType;

	[HideInInspector]
	public FishState FishStates;

	public static DNTG_FishForFormation G_FishFormation;

	public static DNTG_FishForFormation GetSingleton()
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
			Transform transform = base.transform.Find("Fish");
			if (transform != null)
			{
				objFish = transform.gameObject;
			}
		}
	}

	public void InitNormalFish(DNTG_FORMATION formationType, float x, float y, float rot, float index, DNTG_FISH_TYPE fishtype, int i)
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
		if (FormationType == DNTG_FORMATION.Formation_RedCarpet)
		{
			if (FishType == DNTG_FISH_TYPE.Fish_Shrimp)
			{
				Speed = 0.8f;
			}
			else
			{
				Speed = 1.35f;
			}
		}
		else if (FormationType == DNTG_FORMATION.Formation_BigFishes)
		{
			Speed = 1.255f;
		}
		else if (FormationType == DNTG_FORMATION.Formation_Fluctuate)
		{
			Speed = 1f;
		}
		else if (FormationType == DNTG_FORMATION.Formation_ConcentricCircles)
		{
			Speed = 0.6f;
			ds = 0f;
			if (FishType == DNTG_FISH_TYPE.Fish_Boss)
			{
				Rot -= (float)Math.PI / 2f;
				objFish.SetActive(value: true);
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_Ugly)
			{
				s = i - 1;
				Rot = rot;
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_BigEars)
			{
				s = i - 14;
				Rot = rot;
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_Zebra)
			{
				s = i - 38;
				Rot = rot;
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_Shrimp)
			{
				s = i - 78;
				Rot = rot;
			}
		}
		else if (FormationType == DNTG_FORMATION.Formation_NineCircle)
		{
			Speed = 1.6f;
		}
		else if (FormationType == DNTG_FORMATION.Formation_Hexagonal)
		{
			newVector = base.transform.eulerAngles;
			newVector.x = rot / (float)Math.PI * 180f;
			newVector.z = 0f;
			newVector.y = 270f;
			base.transform.eulerAngles = newVector;
			Speed = 0.9f;
		}
		else if (FormationType == DNTG_FORMATION.Formation_FishArray)
		{
			if (objFish != null)
			{
				objFish.SetActive(value: true);
			}
			else
			{
				UnityEngine.Debug.LogError("====objFish为空====");
			}
			Speed = 2f;
		}
		else if (FormationType == DNTG_FORMATION.Formation_MonkeyByCar)
		{
			if (objFish != null)
			{
				objFish.SetActive(value: true);
			}
			else
			{
				UnityEngine.Debug.LogError("====objFish为空====");
			}
			Speed = 1.2f;
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
		case DNTG_FORMATION.Formation_BigFishes:
		{
			newVector = base.transform.position;
			float num5 = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			if (FishType == DNTG_FISH_TYPE.Fish_GoldenDragon || FishType == DNTG_FISH_TYPE.Fish_Beauty)
			{
				newVector.x += num5 * 1366f / 1567f;
				newVector.y -= num5 * 768f / 1567f;
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_BlueDolphin || FishType == DNTG_FISH_TYPE.Fish_Butterfly)
			{
				newVector.x += num5 * 1366f / 1567f;
				newVector.y += num5 * 768f / 1567f;
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_GoldenShark || FishType == DNTG_FISH_TYPE.Fish_Arrow)
			{
				newVector.x -= num5 * 1366f / 1567f;
				newVector.y += num5 * 768f / 1567f;
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_Bat || FishType == DNTG_FISH_TYPE.Fish_SilverShark)
			{
				newVector.x -= num5 * 1366f / 1567f;
				newVector.y -= num5 * 768f / 1567f;
			}
			base.transform.position = newVector;
			break;
		}
		case DNTG_FORMATION.Formation_Hexagonal:
			HexagonalUpdate();
			break;
		case DNTG_FORMATION.Formation_NineCircle:
		{
			newVector = base.transform.position;
			float num6 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			if (FishType == DNTG_FISH_TYPE.Fish_Shrimp || (totalTime > 6f && FishType == DNTG_FISH_TYPE.Fish_YellowSpot) || (totalTime > 12f && FishType == DNTG_FISH_TYPE.Fish_Ugly) || (totalTime > 18f && FishType == DNTG_FISH_TYPE.Fish_Hedgehog) || (totalTime > 24f && FishType == DNTG_FISH_TYPE.Fish_Lamp) || (totalTime > 36f && FishType == DNTG_FISH_TYPE.Fish_Trailer) || (totalTime > 30f && FishType == DNTG_FISH_TYPE.Fish_Turtle) || (totalTime > 42f && FishType == DNTG_FISH_TYPE.Fish_Arrow) || (totalTime > 45f && FishType == DNTG_FISH_TYPE.Fish_SilverShark))
			{
				objFish.SetActive(value: true);
				newVector.x += num6 * Mathf.Cos(Rot) / 2f;
				newVector.y -= num6 * Mathf.Sin(Rot) / 2f;
			}
			base.transform.position = newVector;
			break;
		}
		case DNTG_FORMATION.Formation_FishArray:
		{
			newVector = base.transform.position;
			float num3 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			objFish.SetActive(value: true);
			newVector.x += num3 * Mathf.Cos(Rot) / 2f;
			newVector.y -= num3 * Mathf.Sin(Rot) / 2f;
			base.transform.position = newVector;
			break;
		}
		case DNTG_FORMATION.Formation_MonkeyByCar:
		{
			newVector = base.transform.position;
			float num2 = Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			objFish.SetActive(value: true);
			newVector.x += num2 * Mathf.Cos(Rot) / 2f;
			newVector.y -= num2 * Mathf.Sin(Rot) / 2f;
			base.transform.position = newVector;
			break;
		}
		case DNTG_FORMATION.Formation_ConcentricCircles:
		{
			int num7 = 0;
			newVector = base.transform.position;
			ds += Speed * Time.deltaTime;
			totalTime += Time.deltaTime;
			if (FishType == DNTG_FISH_TYPE.Fish_Ugly)
			{
				num7 = 13;
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_BigEars)
			{
				num7 = 24;
			}
			else if (FishType == DNTG_FISH_TYPE.Fish_Zebra || FishType == DNTG_FISH_TYPE.Fish_Shrimp)
			{
				num7 = 40;
			}
			if (!objFish.activeSelf && ds >= (float)Math.PI * 2f / (float)num7 * s)
			{
				objFish.SetActive(value: true);
				base.transform.eulerAngles = Vector3.right * Rot / (float)Math.PI * 180f + Vector3.up * 90f;
			}
			if ((totalTime > 45f && FishType == DNTG_FISH_TYPE.Fish_Boss) || (totalTime > 40f && FishType == DNTG_FISH_TYPE.Fish_Ugly) || (totalTime > 35f && FishType == DNTG_FISH_TYPE.Fish_BigEars) || (totalTime > 30f && FishType == DNTG_FISH_TYPE.Fish_Zebra) || (totalTime > 25f && FishType == DNTG_FISH_TYPE.Fish_Shrimp))
			{
				float num8 = Speed * Time.deltaTime;
				newVector.x += num8 * Mathf.Cos(Rot);
				newVector.y -= num8 * Mathf.Sin(Rot);
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
		case DNTG_FORMATION.Formation_Fluctuate:
		{
			newVector = base.transform.position;
			float num = Speed * Time.deltaTime;
			objFish.SetActive(value: true);
			totalTime += Time.deltaTime;
			if (Rot == -(float)Math.PI / 2f)
			{
				if ((FishType == DNTG_FISH_TYPE.Fish_Trailer && totalTime > 30f) || (FishType == DNTG_FISH_TYPE.Fish_Lamp && totalTime > 24f) || (FishType == DNTG_FISH_TYPE.Fish_YellowSpot && totalTime > 16f) || (FishType == DNTG_FISH_TYPE.Fish_Zebra && totalTime > 8f) || (FishType == DNTG_FISH_TYPE.Fish_Shrimp && totalTime > 1f))
				{
					newVector.y += num;
				}
			}
			else if (Rot == (float)Math.PI / 2f && ((FishType == DNTG_FISH_TYPE.Fish_Turtle && totalTime > 30f) || (FishType == DNTG_FISH_TYPE.Fish_Hedgehog && totalTime > 24f) || (FishType == DNTG_FISH_TYPE.Fish_Ugly && totalTime > 16f) || (FishType == DNTG_FISH_TYPE.Fish_BigEars && totalTime > 8f) || (FishType == DNTG_FISH_TYPE.Fish_Grass && totalTime > 1f)))
			{
				newVector.y -= num;
			}
			base.transform.position = newVector;
			break;
		}
		case DNTG_FORMATION.Formation_RedCarpet:
		{
			objFish.SetActive(value: true);
			totalTime += Time.deltaTime;
			float num4 = Speed * Time.deltaTime;
			newVector = base.transform.position;
			if (FishType == DNTG_FISH_TYPE.Fish_Shrimp)
			{
				if (Rot == -(float)Math.PI / 2f)
				{
					if (newVector.y <= -2.6f + s || (totalTime > 38f && totalTime < 40f))
					{
						newVector.y += num4;
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
						newVector.y -= num4;
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
					newVector.x += num4;
				}
				else if (Rot == (float)Math.PI)
				{
					newVector.x -= num4;
				}
			}
			base.transform.position = newVector;
			break;
		}
		case DNTG_FORMATION.Formation_YaoQianShuL:
		case DNTG_FORMATION.Formation_YaoQianShuR:
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
			DNTG_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
		}
	}

	private void HexagonalUpdate()
	{
		newVector = base.transform.position;
		float num = Speed * Time.deltaTime;
		totalTime += Time.deltaTime;
		if (FishType == DNTG_FISH_TYPE.Fish_BigEars)
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
		if (FishType == DNTG_FISH_TYPE.Fish_Shrimp || FishType == DNTG_FISH_TYPE.Fish_Zebra || FishType == DNTG_FISH_TYPE.Fish_Ugly || FishType == DNTG_FISH_TYPE.Fish_Hedgehog || FishType == DNTG_FISH_TYPE.Fish_Lamp)
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
