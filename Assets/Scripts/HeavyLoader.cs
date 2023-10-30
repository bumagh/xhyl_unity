using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeavyLoader : SimpleSingletonBehaviour<HeavyLoader>
{
	public List<LoadItem> items = new List<LoadItem>();

	public bool loadOnAwake = true;

	public bool isLoaded;

	private void Awake()
	{
		SimpleSingletonBehaviour<HeavyLoader>.s_instance = this;
		if (loadOnAwake)
		{
			LoadAll();
		}
	}

	public void LoadAll()
	{
		if (!isLoaded)
		{
			foreach (LoadItem item in items)
			{
				GameObject gameObject = Object.Instantiate(item.obj);
				if (item.root != null)
				{
					gameObject.transform.SetParent(base.transform);
				}
				gameObject.SetActive(value: true);
			}
			isLoaded = true;
		}
	}

	public List<GameObject> GetAllObjs()
	{
		return (from item in items
			select item.obj).ToList();
	}
}
