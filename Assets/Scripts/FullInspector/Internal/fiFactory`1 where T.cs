using System;
using System.Collections.Generic;

namespace FullInspector.Internal
{
	public class fiFactory<T> where T : new()
	{
		private Stack<T> _reusable = new Stack<T>();

		private Action<T> _reset;

		private object[] _constructArgs;

		public fiFactory(Action<T> reset, params object[] constructArgs)
		{
			_reset = reset;
			_constructArgs = constructArgs;
		}

		public T GetInstance()
		{
			if (_reusable.Count == 0)
			{
				return (T)Activator.CreateInstance(typeof(T), _constructArgs);
			}
			return _reusable.Pop();
		}

		public void ReuseInstance(T instance)
		{
			_reset(instance);
			_reusable.Push(instance);
		}
	}
}
