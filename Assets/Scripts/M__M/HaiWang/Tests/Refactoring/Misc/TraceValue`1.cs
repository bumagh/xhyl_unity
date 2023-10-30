using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.Tests.Refactoring.Misc
{
	public class TraceValue<T>
	{
		[InspectorDisabled]
		[ShowInInspector]
		private T _lastValue;

		[SerializeField]
		private T _curValue;

		public bool isChanged => object.Equals(curValue, _lastValue);

		public T curValue
		{
			get
			{
				return _curValue;
			}
			private set
			{
				_lastValue = curValue;
				_curValue = value;
			}
		}

		public T lastValue => _lastValue;

		public void Clear()
		{
			_lastValue = _curValue;
		}
	}
}
