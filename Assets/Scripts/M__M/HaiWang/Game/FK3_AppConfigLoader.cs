using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Game
{
	public class FK3_AppConfigLoader : MonoBehaviour
	{
		public FK3_AppConfig appConfig;

		private void Awake()
		{
			FK3_GVars.SetAppConfig(appConfig);
			Object.DontDestroyOnLoad(base.gameObject);
			FK3_GVars.dontDestroyOnLoadList.Add(base.gameObject);
		}

		private void ShowAppInfo()
		{
			Transform transform = base.transform.Find("/Canvas/AppInfo");
			Text component = transform.GetComponent<Text>();
			component.text = appConfig.info;
		}
	}
}
