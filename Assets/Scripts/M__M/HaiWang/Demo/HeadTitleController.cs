using System;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class HeadTitleController : SimpleSingletonBehaviour<HeadTitleController>
	{
		[SerializeField]
		private Text _deskName;

		[SerializeField]
		private Text _txtNowTime;

		[SerializeField]
		private Image _wifi;

		[SerializeField]
		private Sprite[] _imgWifi;

		private void Awake()
		{
			SimpleSingletonBehaviour<HeadTitleController>.s_instance = this;
		}

		private void Start()
		{
			registerSignalListen();
		}

		public void Init()
		{
			_deskName.text = HW2_GVars.lobby.GetCurDesk().name;
		}

		private void Update()
		{
			_txtNowTime.text = DateTime.Now.ToString("HH:mm");
		}

		public void CheckNetwork()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				UnityEngine.Debug.Log("当前网络：不可用");
			}
			else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
			{
				UnityEngine.Debug.Log("当前网络：3G/4G");
			}
			else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
			{
				UnityEngine.Debug.Log("当前网络 : WIFI");
			}
		}

		public void updateSignal(string sLevel)
		{
			UnityEngine.Debug.Log("_wifi sLevel: " + sLevel);
			if (sLevel == "disabled")
			{
				_wifi.sprite = _imgWifi[5];
				return;
			}
			int num = int.Parse(sLevel);
			_wifi.sprite = _imgWifi[num];
		}

		public void registerSignalListen()
		{
			UnityEngine.Debug.LogError("registerSignalListen 等待处理  TODO");
		}

		public void unRegisterSinalListen()
		{
			UnityEngine.Debug.LogError("unRegisterSinalListen 等待处理  TODO");
		}
	}
}
