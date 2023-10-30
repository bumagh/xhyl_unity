using FullInspector;
using M__M.GameHall.Common;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.GameHall.Net
{
	public class FK3_NetLogFilter : BaseBehavior<FullSerializerSerializer>
	{
		private static FK3_NetLogFilter s_instance;

		public List<FK3_FilterMessageItem> ignoreSendList = new List<FK3_FilterMessageItem>();

		[SerializeField]
		public List<FK3_FilterMessageItem> ignoreRecvList = new List<FK3_FilterMessageItem>();

		[SerializeField]
		public List<int> testList;

		public static FK3_NetLogFilter Get()
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
			FK3_MB_Singleton<FK3_NetManager>.Get().filterRecvMsgLogFunc = FilterRecv;
			FK3_MB_Singleton<FK3_NetManager>.Get().filterSendMsgLogFunc = FilterSend;
		}

		private bool FilterSend(string method)
		{
			return Filter(method, ignoreSendList);
		}

		private bool FilterRecv(string method)
		{
			return Filter(method, ignoreRecvList);
		}

		private bool Filter(string method, List<FK3_FilterMessageItem> list)
		{
			if (list == null)
			{
				return true;
			}
			bool result = true;
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				FK3_FilterMessageItem fK3_FilterMessageItem = list[i];
				if (fK3_FilterMessageItem != null && !fK3_FilterMessageItem.Filter(method))
				{
					result = false;
					break;
				}
			}
			return result;
		}
	}
}
