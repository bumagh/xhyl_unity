using HW3L;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_UserInfoInGame : FK3_SimpleSingletonBehaviour<FK3_UserInfoInGame>
	{
		[SerializeField]
		private Image _imgIcon;

		[SerializeField]
		private Text _textNickname;

		[SerializeField]
		private Text _textLevel;

		[SerializeField]
		private Text _textScore;

		[SerializeField]
		private Text _textDay;

		[SerializeField]
		private Text _textWeek;

		[SerializeField]
		private Text _textTotal;

		[SerializeField]
		private Sprite[] _icon;

		public Action<int> Event_PrivateChatClick_act;

		private FK3_SeatInfo2 seatInfo;

		private int selectSeatId = -1;

		private void Awake()
		{
			FK3_SimpleSingletonBehaviour<FK3_UserInfoInGame>.s_instance = this;
			base.gameObject.SetActive(value: false);
		}

		public void ShowUserInfo(int index, int level, int dailyHonor, int weekHonor, int totalHonor)
		{
			base.gameObject.SetActive(value: true);
			selectSeatId = index;
			seatInfo = FK3_GVars.lobby.inGameSeats[index - 1];
			_imgIcon.sprite = _icon[seatInfo.user.photoId - 1];
			_textNickname.text = seatInfo.user.nickname;
			_textLevel.text = "Lv." + level + " " + FK3_Utils.GetLevelName(level, FK3_GVars.language);
			_textScore.text = seatInfo.user.gameScore.ToString();
			_textDay.text = ((dailyHonor == -1) ? "未上榜" : ("No." + dailyHonor));
			_textWeek.text = ((weekHonor == -1) ? "未上榜" : ("No." + weekHonor));
			_textTotal.text = ((totalHonor == -1) ? "未上榜" : ("No." + totalHonor));
			StartCoroutine(DelayCall(3f, delegate
			{
				base.gameObject.SetActive(value: false);
			}));
		}

		private void Update()
		{
			if (base.gameObject.activeSelf)
			{
				_textScore.text = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(selectSeatId).GetScore()
					.ToString();
			}
		}

		public void OnBtnPrivateChat_Click()
		{
			if (Event_PrivateChatClick_act != null)
			{
				Event_PrivateChatClick_act(seatInfo.id);
			}
			base.gameObject.SetActive(value: false);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		private IEnumerator DelayCall(float delay, Action call)
		{
			yield return new WaitForSeconds(delay);
			call();
		}
	}
}
