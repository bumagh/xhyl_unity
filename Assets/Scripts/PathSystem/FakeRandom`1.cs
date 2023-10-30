using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public class FakeRandom<T> : GeneratorBase<T>
	{
		public List<T> types;

		public int seed;

		protected int _curSeed;

		public override void Init(SubFormation<T> subFormation)
		{
			base.Init(subFormation);
			_curSeed = seed;
		}

		public override AgentData<T> GetNext(object userData)
		{
			Random.InitState(_curSeed++);
			int index = Random.Range(0, types.Count);
			return _getNext(types[index], userData);
		}

		public override void Reset()
		{
			_curSeed = seed;
		}
	}
}
