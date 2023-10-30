using FullInspector;
using M__M.GameHall.Common;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.GameHall.Net
{
	public class NetLogFilter : BaseBehavior<FullSerializerSerializer>
	{
		private static NetLogFilter s_instance;

		public List<FilterMessageItem> ignoreSendList = new List<FilterMessageItem>();

		[SerializeField]
		public List<FilterMessageItem> ignoreRecvList = new List<FilterMessageItem>();

		[SerializeField]
		public List<int> testList;

		public static NetLogFilter Get()
		{
			return s_instance;
		}

		protected override void Awake()
		{
			base.Awake();
			s_instance = this;
		}

		private void Start()
		{
			HW2_MB_Singleton<HW2_NetManager>.Get().filterRecvMsgLogFunc = FilterRecv;
			HW2_MB_Singleton<HW2_NetManager>.Get().filterSendMsgLogFunc = FilterSend;
		}

		private bool FilterSend(string method)
		{
			return Filter(method, ignoreSendList);
		}

		private bool FilterRecv(string method)
		{
			return Filter(method, ignoreRecvList);
		}

		private bool Filter(string method, List<FilterMessageItem> list)
		{
			if (list == null)
			{
				return true;
			}
			bool result = true;
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				FilterMessageItem filterMessageItem = list[i];
				if (filterMessageItem != null && !filterMessageItem.Filter(method))
				{
					result = false;
					break;
				}
			}
			return result;
		}
	}
}
