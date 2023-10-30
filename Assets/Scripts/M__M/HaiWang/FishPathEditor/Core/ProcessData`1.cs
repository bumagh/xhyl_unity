using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class ProcessData<T>
	{
		public readonly T value;

		public readonly int index;

		public readonly string info = string.Empty;

		public readonly SpawnerBase<T> spawner;

		public readonly Dictionary<string, object> fields;

		public readonly SpawnerContext context;

		public GameObject obj;

		public MonoBehaviour objBehaviour;

		public Action<ProcessData<T>> onDespawn;

		public bool ignore = true;

		public ProcessData()
		{
		}

		public ProcessData(T value, int index, SpawnerBase<T> spawner, string info = "", bool ignore = false)
		{
			this.value = value;
			this.index = index;
			this.spawner = spawner;
			this.info = info;
			fields = spawner.fields;
			context = spawner.context;
			this.ignore = ignore;
		}

		public static ProcessData<T> Create(T value, int index, SpawnerBase<T> spawner, string info = "")
		{
			return new ProcessData<T>(value, index, spawner, info);
		}

		public static ProcessData<T> CreateIgnore(T value, int index, SpawnerBase<T> spawner, string info = "")
		{
			return new ProcessData<T>(value, index, spawner, info, ignore: true);
		}
	}
}
