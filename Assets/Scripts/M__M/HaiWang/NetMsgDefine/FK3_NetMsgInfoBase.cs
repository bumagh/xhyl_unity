using System.Collections.Generic;

namespace M__M.HaiWang.NetMsgDefine
{
	public class FK3_NetMsgInfoBase
	{
		public int code;

		public string message;

		public bool valid = true;

		protected object[] m_args;

		public Dictionary<string, object> basicDic;

		public FK3_NetMsgInfoBase(object[] args = null)
		{
			m_args = args;
		}

		public virtual void Parse()
		{
			try
			{
				object[] args = m_args;
				Dictionary<string, object> dictionary = basicDic = (args[0] as Dictionary<string, object>);
				code = (int)dictionary["code"];
				message = (dictionary["message"] as string);
			}
			catch
			{
				valid = false;
			}
		}

		public virtual void Parse2()
		{
			try
			{
				object[] args = m_args;
				Dictionary<string, object> dictionary = basicDic = (args[0] as Dictionary<string, object>);
				code = (int)dictionary["code"];
				message = (dictionary["message"] as string);
			}
			catch
			{
				valid = false;
			}
		}
	}
}
