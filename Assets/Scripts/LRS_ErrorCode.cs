public class LRS_ErrorCode
{
	public static string GetErrorMsg(int code, bool isChinese)
	{
		string empty = string.Empty;
		switch (code)
		{
		case 0:
			return isChinese ? "网络异常，请重新进入" : "Network error, please re-enter";
		case 1:
			return isChinese ? "成功" : "Success";
		case 2:
		case 3:
			return isChinese ? "账户名或密码错误" : "Wrong user id or password";
		case 6:
			return isChinese ? "您的账号已被冻结，请联系客服" : "Your account has been frozen, please contact customer service";
		case 7:
			return isChinese ? "服务器维护，无法进入游戏" : "Server maintenance, can't enter the game";
		default:
			empty = (isChinese ? "未知错误: " : "Unknown error: ");
			return empty + code;
		}
	}
}
