using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FK3_HeavyLoader : FK3_SimpleSingletonBehaviour<FK3_HeavyLoader>
{
	public List<FK3_LoadItem> items = new List<FK3_LoadItem>();

	public bool loadOnAwake = true;

	public bool isLoaded;

	private void Awake()
	{
		FK3_SimpleSingletonBehaviour<FK3_HeavyLoader>.s_instance = this;
		if (loadOnAwake)
		{
			LoadAll();
		}
	}

	public void LoadAll()
	{
		if (!isLoaded)
		{
			foreach (FK3_LoadItem item in items)
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
