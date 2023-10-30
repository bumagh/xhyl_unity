using System.Collections.Generic;

namespace FullInspector.Internal
{
	public class fiArrayDictionary<TKey, TValue>
	{
		private List<KeyValuePair<TKey, TValue>> _elements = new List<KeyValuePair<TKey, TValue>>();

		public TValue this[TKey key]
		{
			set
			{
				for (int i = 0; i < _elements.Count; i++)
				{
					KeyValuePair<TKey, TValue> keyValuePair = _elements[i];
					if (EqualityComparer<TKey>.Default.Equals(key, keyValuePair.Key))
					{
						_elements[i] = new KeyValuePair<TKey, TValue>(key, value);
						return;
					}
				}
				_elements.Add(new KeyValuePair<TKey, TValue>(key, value));
			}
		}

		public bool Remove(TKey key)
		{
			for (int i = 0; i < _elements.Count; i++)
			{
				KeyValuePair<TKey, TValue> keyValuePair = _elements[i];
				if (EqualityComparer<TKey>.Default.Equals(key, keyValuePair.Key))
				{
					_elements.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public bool ContainsKey(TKey key)
		{
			TValue value;
			return TryGetValue(key, out value);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			for (int i = 0; i < _elements.Count; i++)
			{
				KeyValuePair<TKey, TValue> keyValuePair = _elements[i];
				if (EqualityComparer<TKey>.Default.Equals(key, keyValuePair.Key))
				{
					value = keyValuePair.Value;
					return true;
				}
			}
			value = default(TValue);
			return false;
		}
	}
}
