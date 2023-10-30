using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_FormationPlayParam<T>
	{
		public int formationId;

		public int startId;

		public bool hasOffset;

		public Vector3 offset;

		public int count;

		public float startTime;

		public Func<int, T, T> generatorFunc;
	}
}
