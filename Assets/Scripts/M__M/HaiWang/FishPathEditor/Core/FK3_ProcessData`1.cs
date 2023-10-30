using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_ProcessData<T>
	{
		public readonly T value;

		public readonly int index;

		public readonly string info = string.Empty;

		public readonly FK3_SpawnerBase<T> spawner;

		public readonly Dictionary<string, object> fields;

		public readonly FK3_SpawnerContext context;

		public GameObject obj;

		public MonoBehaviour objBehaviour;

		public Action<FK3_ProcessData<T>> onDespawn;

		public bool ignore = true;

		public FK3_ProcessData()
		{
		}

		public FK3_ProcessData(T value, int index, FK3_SpawnerBase<T> spawner, string info = "", bool ignore = false)
		{
			this.value = value;
			this.index = index;
			this.spawner = spawner;
			this.info = info;
			fields = spawner.fields;
			context = spawner.context;
			this.ignore = ignore;
		}

		public static FK3_ProcessData<T> Create(T value, int index, FK3_SpawnerBase<T> spawner, string info = "")
		{
			return new FK3_ProcessData<T>(value, index, spawner, info);
		}

		public static FK3_ProcessData<T> CreateIgnore(T value, int index, FK3_SpawnerBase<T> spawner, string info = "")
		{
			return new FK3_ProcessData<T>(value, index, spawner, info, ignore: true);
		}
	}
}
