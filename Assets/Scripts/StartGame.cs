using UnityEngine;

public class StartGame : MonoBehaviour
{
	private void Start()
	{
		UIRoomManager.GetInstance().OpenUI("UI_Background");
	}
}
