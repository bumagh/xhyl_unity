using ProtoBuf;
using System;

namespace gprotocol
{
	[Serializable]
	[ProtoContract(Name = "CS_LOGINSERVER")]
	public class CS_LOGINSERVER : IExtensible
	{
		private string _account;

		private string _password;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "account", DataFormat = DataFormat.Default)]
		public string account
		{
			get
			{
				return _account;
			}
			set
			{
				_account = value;
			}
		}

		[ProtoMember(2, IsRequired = true, Name = "password", DataFormat = DataFormat.Default)]
		public string password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
