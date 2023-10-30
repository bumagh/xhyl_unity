using System;
using System.Collections.Generic;

public static class NotifyAction
{
	private static Dictionary<Gamestate, List<Action>> notifyac = new Dictionary<Gamestate, List<Action>>();

	public static void Init()
	{
		notifyac.Clear();
	}

	public static void ResushState(Gamestate state)
	{
		if (notifyac.ContainsKey(state))
		{
			List<Action> list = notifyac[state];
			for (int i = 0; i < list.Count; i++)
			{
				list[i]();
			}
		}
	}

	public static void AddAction(Gamestate state, Action ac)
	{
		if (notifyac.ContainsKey(state))
		{
			notifyac[state].Add(ac);
			return;
		}
		List<Action> list = new List<Action>();
		list.Add(ac);
		notifyac.Add(state, list);
	}
}
