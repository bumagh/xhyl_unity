using System;
using System.Collections.Generic;

public static class FK3_AppSceneMgr
{
	public delegate void SceneAction(object arg, Action<object> next);

	public static string scene_loading_name = string.Empty;

	public static string scene_lobby_name = string.Empty;

	public static string scene_mainGame_name = string.Empty;

	public static string cur_scene_name = string.Empty;

	public static string first_scene_name = string.Empty;

	public static List<string> scene_list = new List<string>();

	public static Dictionary<string, SceneAction> actionDic = new Dictionary<string, SceneAction>();

	public static void RegisterScene(string sceneName)
	{
		if (scene_list.Count == 0)
		{
			first_scene_name = sceneName;
		}
		scene_list.Add(sceneName);
	}

	public static void RegisterAction(string actionName, SceneAction action = null)
	{
		if (!actionDic.ContainsKey(actionName))
		{
			actionDic.Add(actionName, action);
		}
		else
		{
			actionDic[actionName] = action;
		}
	}

	public static void RunAction(string actionName, object arg, Action<object> next)
	{
		SceneAction value = null;
		actionDic.TryGetValue(actionName, out value);
		value?.Invoke(arg, next);
	}

	public static bool isFirstScene(string sceneName)
	{
		return first_scene_name.Equals(sceneName);
	}
}
