public class AppRoot : MB_Singleton<AppRoot>
{
	private void Awake()
	{
		MB_Singleton<AppRoot>.SetInstance(this);
	}
}
