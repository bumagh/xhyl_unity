using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Game
{
	public class AppConfigLoader : MonoBehaviour
	{
		public AppConfig appConfig;

		private void Awake()
		{
			HW2_GVars.SetAppConfig(appConfig);
			Object.DontDestroyOnLoad(base.gameObject);
			HW2_GVars.dontDestroyOnLoadList.Add(base.gameObject);
		}

		private void ShowAppInfo()
		{
			Transform transform = base.transform.Find("/Canvas/AppInfo");
			Text component = transform.GetComponent<Text>();
			component.text = appConfig.info;
		}
	}
}
