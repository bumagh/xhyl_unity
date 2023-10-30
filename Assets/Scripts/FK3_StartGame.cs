using UnityEngine;

public class FK3_StartGame : MonoBehaviour
{
	private void Start()
	{
		FK3_UIRoomManager.GetInstance().OpenUI("UI_Background");
	}
}
