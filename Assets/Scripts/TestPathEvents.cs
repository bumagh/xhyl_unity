using PathSystem;
using System.Collections.Generic;
using UnityEngine;

public class TestPathEvents : MonoBehaviour
{
	private NavPathAgent _agent;

	private void Start()
	{
		_agent = GetComponentInChildren<NavPathAgent>();
		if (_agent.path != null)
		{
			List<int> list = new List<int>();
			_agent.path.GetEventPoints(list);
			foreach (int item in list)
			{
				_agent.AddEventListener(item, delegate
				{
					Vector3 localScale = _agent.gameObject.transform.localScale;
					if (localScale.x < 0.7f)
					{
						_agent.gameObject.transform.localScale = Vector3.one;
					}
					else
					{
						_agent.gameObject.transform.localScale = Vector3.one / 2f;
					}
				});
			}
		}
	}

	public void EventHandler1(int index)
	{
		UnityEngine.Debug.Log("Point " + index + " reached!");
	}

	private void Update()
	{
	}
}
