using GameCommon;
using System;
using UnityEngine;

public class STMF_FishForFormation : MonoBehaviour
{
	private enum FishState
	{
		FishState_Sleep,
		FishState_Alive
	}

	private Vector3 newVector;

	private float fWidth;

	private float fHeight;

	private STMF_FISH_TYPE FishType;

	private float totalTime;

	private float Speed;

	private bool Move;

	private int Index;

	private float Rot;

	private float IniX;

	private float IniY;

	private float s;

	public STMF_FORMATION FormationType;

	private FishState FishStates;

	private void Start()
	{
		newVector = Vector3.forward;
		fWidth = 0.871729434f;
		fHeight = 0.4901085f;
	}

	public void InitNormalFish(STMF_FORMATION formationType, float x, float y, float rot, int index, STMF_FISH_TYPE fishtype, float ss = 0f)
	{
		FishType = fishtype;
		FormationType = formationType;
		FishStates = FishState.FishState_Sleep;
		IniX = x;
		IniY = y;
		Rot = rot;
		s = ss;
		Index = index;
		newVector = base.gameObject.transform.localPosition;
		newVector.x = x;
		newVector.y = y;
		base.gameObject.transform.localPosition = newVector;
		Speed = 0.55f;
		totalTime = 0f;
		if (FormationType == STMF_FORMATION.Formation_RedCarpet)
		{
			if (FishType == STMF_FISH_TYPE.Fish_Shrimp)
			{
				Speed = 0.8f;
			}
			else
			{
				Speed = 1.35f;
			}
		}
		else if (FormationType == STMF_FORMATION.Formation_BigFishes)
		{
			Speed = 1.255f;
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
		case STMF_FORMATION.Formation_Normal:
			if (!Move)
			{
			}
			break;
		case STMF_FORMATION.Formation_BigFishes:
		{
			newVector = base.gameObject.transform.localPosition;
			float num2 = Speed * Time.deltaTime;
			if (FishType == STMF_FISH_TYPE.Fish_SilverDragon || FishType == STMF_FISH_TYPE.Fish_Beauty)
			{
				newVector.x += num2 * fWidth;
				newVector.y -= num2 * fHeight;
			}
			else if (FishType == STMF_FISH_TYPE.Fish_Knife_Butterfly_Group || FishType == STMF_FISH_TYPE.Fish_Butterfly)
			{
				newVector.x += num2 * fWidth;
				newVector.y += num2 * fHeight;
			}
			else if (FishType == STMF_FISH_TYPE.Fish_GoldenShark || FishType == STMF_FISH_TYPE.Fish_Arrow)
			{
				newVector.x -= num2 * fWidth;
				newVector.y += num2 * fHeight;
			}
			else if (FishType == STMF_FISH_TYPE.Fish_Bat || FishType == STMF_FISH_TYPE.Fish_SilverShark)
			{
				newVector.x -= num2 * fWidth;
				newVector.y -= num2 * fHeight;
			}
			base.transform.localPosition = newVector;
			break;
		}
		case STMF_FORMATION.Formation_RedCarpet:
		{
			totalTime += Time.deltaTime;
			float num = Speed * Time.deltaTime;
			newVector = base.gameObject.transform.localPosition;
			if (FishType == STMF_FISH_TYPE.Fish_Shrimp)
			{
				if (Rot == -(float)Math.PI / 2f)
				{
					if (newVector.y <= -2.6f + s || (totalTime > 38f && totalTime < 40f))
					{
						newVector.y += num;
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
						newVector.y -= num;
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
					newVector.x += num;
				}
				else if (Rot == (float)Math.PI)
				{
					newVector.x -= num;
				}
			}
			base.transform.localPosition = newVector;
			break;
		}
		case STMF_FORMATION.Formation_YaoQianShuL:
		case STMF_FORMATION.Formation_YaoQianShuR:
			newVector = base.gameObject.transform.localPosition;
			if (Rot == 0f)
			{
				newVector.x += Speed * Time.deltaTime;
			}
			else if (Rot == (float)Math.PI)
			{
				newVector.x -= Speed * Time.deltaTime;
			}
			base.transform.localPosition = newVector;
			break;
		}
		if (InsideWindow() && FormationType != 0 && FishStates == FishState.FishState_Sleep)
		{
			FishStates = FishState.FishState_Alive;
		}
		if (FishStates == FishState.FishState_Alive && FormationType != 0 && !InsideWindow())
		{
			STMF_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
		}
	}
}
