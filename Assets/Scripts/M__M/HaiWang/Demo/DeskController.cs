using DG.Tweening;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Demo
{
	public class DeskController : MonoBehaviour
	{
		private static DeskController instance;

		private int currentDeskIndex;

		private int deskCount;

		private List<DeskInfo> deskList;

		private DeskInfo curDesk;

		private int currentIndex;

		private Action readyInGameAct;

		private float _lastPosX;

		private float _currentPosX;

		private float _speed = 0.01f;

		private Vector3 pox0 = new Vector3(-1280f, 0f, 0f);

		private Vector3 pox1 = new Vector3(0f, 0f, 0f);

		private Vector3 pox2 = new Vector3(1280f, 0f, 0f);

		private Coroutine coShowSeatUserInfo;

		public static DeskController Get()
		{
			return instance;
		}

		private void OnEnable()
		{
			ResetState();
			ResetDesk();
		}

		private void ResetDesk()
		{
		}

		private void Awake()
		{
			instance = this;
		}

		public void UpdateUI_RoomInfoPush(int index, int roomId, Action act = null)
		{
			readyInGameAct = (Action)Delegate.Remove(readyInGameAct, act);
			readyInGameAct = (Action)Delegate.Combine(readyInGameAct, act);
			ResetDeskList();
			index = 0;
			if (deskList != null && curDesk != null)
			{
				for (int i = 0; i < deskList.Count; i++)
				{
					if (deskList[i].name.Equals(curDesk.name))
					{
						index = i;
						break;
					}
				}
			}
			currentDeskIndex = index;
			GeneratePointItem(deskList.Count);
			RefreshDeskUI();
		}

		private void ResetDeskList()
		{
			deskList = HW2_GVars.lobby.desks;
			deskCount = deskList.Count - 1;
		}

		private void Update()
		{
			SlipEffect();
		}

		private void SlipEffect()
		{
			if (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Vector2 position = UnityEngine.Input.GetTouch(0).position;
				_lastPosX = position.x;
			}
			if (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				Vector2 position2 = UnityEngine.Input.GetTouch(0).position;
				_currentPosX = position2.x;
				if (_currentPosX - _lastPosX > 100f)
				{
					OnBtnArrow_Click(-1);
				}
				else if (_currentPosX - _lastPosX < -100f)
				{
					OnBtnArrow_Click(1);
				}
			}
		}

		public void UpdateUI_Click(int index)
		{
			if (index != currentDeskIndex)
			{
				if (currentDeskIndex > index)
				{
					OnClickRightArrow();
				}
				else
				{
					OnClickLeftArrow();
				}
			}
			currentDeskIndex = index;
			RefreshDeskUI();
		}

		public void UpdateUI_SeatInfoPush(int changeDeskId)
		{
			ResetDeskList();
			UnityEngine.Debug.Log("changeDeskId: " + changeDeskId);
			UnityEngine.Debug.Log("currentDeskIndex: " + currentDeskIndex);
			if (currentDeskIndex == changeDeskId)
			{
				RefreshDeskUI();
			}
		}

		private void RefreshDeskUI()
		{
			RefreshDeskPersonShow(0, deskList[(currentDeskIndex - 1 < 0) ? deskCount : (currentDeskIndex - 1)]);
			RefreshDeskPersonShow(1, deskList[currentDeskIndex]);
			RefreshDeskPersonShow(2, deskList[(currentDeskIndex + 1 <= deskCount) ? (currentDeskIndex + 1) : 0]);
			RefreshBottomDeskUI(deskList[currentDeskIndex]);
			ChangePoint(currentDeskIndex);
		}

		private void RefreshBottomDeskUI(DeskInfo desk)
		{
			curDesk = desk;
		}

		private void RefreshDeskPersonShow(int index, DeskInfo desk)
		{
			currentIndex = index;
			for (int i = 0; i < 4; i++)
			{
				int num = i;
				if (desk.seats[i].isUsed)
				{
				}
			}
		}

		private void ResetState()
		{
		}

		private void SetGunUIFont(DeskInfo desk)
		{
			string text = desk.minGunValue.ToString();
			GetFontLength(text.Length);
		}

		private void GetFontLength(int length)
		{
			switch (length)
			{
			case 1:
				foreach (GunUIController gunUI in GunUIMgr.Get().GunUIList)
				{
					gunUI.txtPower.fontSize = 25;
				}
				break;
			case 2:
				foreach (GunUIController gunUI2 in GunUIMgr.Get().GunUIList)
				{
					gunUI2.txtPower.fontSize = 20;
				}
				break;
			case 3:
				foreach (GunUIController gunUI3 in GunUIMgr.Get().GunUIList)
				{
					gunUI3.txtPower.fontSize = 18;
				}
				break;
			case 4:
				foreach (GunUIController gunUI4 in GunUIMgr.Get().GunUIList)
				{
					gunUI4.txtPower.fontSize = 15;
				}
				break;
			}
		}

		public void ShowSeatUserInfo(int gameScore, SeatInfo seat, int level, int dailyHonor, int weekHonor, int totalHonor)
		{
			if (coShowSeatUserInfo != null)
			{
				StopCoroutine(coShowSeatUserInfo);
				coShowSeatUserInfo = null;
			}
		}

		public void OnBtnArrow_Click(int change)
		{
			if (change == -1)
			{
				OnClickRightArrow();
			}
			if (change == 1)
			{
				OnClickLeftArrow();
			}
			currentDeskIndex += change;
			if (currentDeskIndex < 0)
			{
				currentDeskIndex = deskCount;
			}
			if (currentDeskIndex > deskCount)
			{
				currentDeskIndex = 0;
			}
			RefreshDeskUI();
		}

		private void OnClickRightArrow()
		{
			Sequence t = DOTween.Sequence();
			t.OnComplete(delegate
			{
			});
			SwapDesk(2, 0);
			SwapDesk(1, 2);
		}

		private void OnClickLeftArrow()
		{
			Sequence t = DOTween.Sequence();
			t.OnComplete(delegate
			{
			});
			SwapDesk(0, 1);
			SwapDesk(1, 2);
		}

		private void SwapDesk(int a, int b)
		{
		}

		private void GeneratePointItem(int count)
		{
		}

		private void ChangePoint(int index)
		{
		}

		private IEnumerator DelayCall(float time, Action callback)
		{
			yield return new WaitForSeconds(time);
			callback();
		}
	}
}
