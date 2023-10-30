using UnityEngine;

public class RegisterInvoke : MonoBehaviour
{
	private string username = string.Empty;

	private void Start()
	{
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Register_Invoke, this, Repeat);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("dingFen", HandleNetMsg_DingFen);
	}

	private void HandleNetMsg_DingFen(object[] objs)
	{
		ZH2_GVars.ScoreOverflow = true;
	}

	public void CancelRepeat()
	{
		Debug.Log("CancelRepeat");
		CancelInvoke();
	}

	private void Repeat(object obj)
	{
		Debug.Log("==开启询问==");
		username = (string)obj;
		InvokeRepeating("SendRegister", 10f, 10f);
	}

	private void SendRegister()
	{
		Debug.Log("发送 询问 " + username + " 注册情况 ");
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/isRegistOK", new object[1]
		{
			username
		});
	}
}
