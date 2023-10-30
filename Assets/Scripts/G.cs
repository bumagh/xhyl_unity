using Framework;
using M__M.HaiWang.Demo;
using UnityEngine;

public static class G
{
	public static void Destroy<T>(this ObjectBase target, ref T component) where T : Component
	{
		if ((Object)component != (Object)null)
		{
			UnityEngine.Object.Destroy(component.gameObject);
			component = (T)null;
		}
	}

	public static void StopTask(this ObjectBase target, ref Task task)
	{
		if (task != null)
		{
			task.Stop();
			task = null;
		}
	}

	public static T Instantiate<T>(this ObjectBase target, string resPath, bool active = true) where T : Component
	{
		Object original = Resources.Load(resPath);
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.SetActive(active);
		return gameObject.GetComponent<T>();
	}

	public static Transform Instantiate(this ObjectBase target, string resPath, bool active = true)
	{
		UnityEngine.Debug.LogError("我企图在这里加载: " + resPath);
		string text = resPath.Replace("FishEffect/LightningFish/", string.Empty);
		text = text.Trim();
		GameObject gameObject = LoadingLogic.Get().FindGame(text);
		UnityEngine.Debug.LogError("加载完成: " + gameObject.name);
		gameObject.SetActive(active);
		return gameObject.transform;
	}

	public static void SetActive(this Component target, bool active = true)
	{
		target.gameObject.SetActive(active);
	}

	public static Vector3 GetPosition(this Component target)
	{
		return target.transform.position;
	}

	public static void SetPosition(this Component target, Vector3 position)
	{
		target.transform.position = position;
	}

	public static Vector3 GetLocalScale(this Component target)
	{
		return target.transform.localScale;
	}

	public static void SetLocalScale(this Component target, Vector3 scale)
	{
		target.transform.localScale = scale;
	}
}
