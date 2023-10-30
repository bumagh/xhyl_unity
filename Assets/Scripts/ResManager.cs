using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
	private class loadAction
	{
		public int type;

		public string name = string.Empty;

		public Action<UnityEngine.Object> cbAction;
	}

	private static ResManager _instance;

	private Stack actionList = new Stack();

	private Dictionary<string, UnityEngine.Object> resSaver = new Dictionary<string, UnityEngine.Object>();

	private bool isFree = true;

	public static ResManager GetInstance => _instance;

	private void Awake()
	{
		_instance = this;
	}

	public void loadSprite(string name, Action<UnityEngine.Object> cb)
	{
		pushAction(1, name, cb);
	}

	public void loadConfig(string name, Action<UnityEngine.Object> cb)
	{
		pushAction(2, name, cb);
	}

	public void loadPrefab(string name, Action<UnityEngine.Object> cb)
	{
		pushAction(3, name, cb);
	}

	public void loadClip(string name, Action<UnityEngine.Object> cb)
	{
		pushAction(4, name, cb);
	}

	public void loadBone(string name, Action<UnityEngine.Object> cb)
	{
		pushAction(5, name, cb);
	}

	private void pushAction(int type, string name, Action<UnityEngine.Object> cb)
	{
		UnityEngine.Object value = null;
		if (resSaver.TryGetValue(name, out value))
		{
			cb(value);
			return;
		}
		loadAction loadAction = new loadAction();
		loadAction.type = type;
		loadAction.name = name;
		loadAction.cbAction = cb;
		actionList.Push(loadAction);
		if (isFree)
		{
			dealAction();
		}
	}

	private void dealAction()
	{
		object obj = actionList.Pop();
		if (obj != null)
		{
			isFree = false;
			loadAction loadAction = (loadAction)obj;
			if (loadAction.name == string.Empty || loadAction.name == null)
			{
				UnityEngine.Debug.Log("action.name is empty");
				dealAction();
				return;
			}
			UnityEngine.Object @object = null;
			switch (loadAction.type)
			{
			case 1:
			{
				@object = Resources.Load<UnityEngine.Object>("Texture/" + loadAction.name);
				Sprite sprite = Sprite.Create(@object as Texture2D, new Rect(0f, 0f, (@object as Texture2D).width, (@object as Texture2D).height), new Vector2(0.5f, 0.5f));
				@object = sprite;
				break;
			}
			case 2:
				@object = Resources.Load<UnityEngine.Object>("Config/" + loadAction.name);
				break;
			case 3:
				@object = Resources.Load<UnityEngine.Object>("Prefab/" + loadAction.name);
				break;
			case 4:
				@object = Resources.Load<UnityEngine.Object>("Audio/" + loadAction.name);
				break;
			case 5:
				@object = Resources.Load<UnityEngine.Object>("Bone/" + loadAction.name);
				break;
			}
			if (@object != null)
			{
				if (!resSaver.ContainsKey(loadAction.name))
				{
					resSaver.Add(loadAction.name, @object);
				}
				loadAction.cbAction(@object);
			}
			else
			{
				loadAction.cbAction(null);
				UnityEngine.Debug.LogWarning(loadAction.name);
			}
			isFree = true;
		}
		else
		{
			isFree = true;
		}
	}

	private void Update()
	{
		if (actionList.Count > 0 && isFree)
		{
			dealAction();
		}
	}
}
