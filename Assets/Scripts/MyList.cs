using System;
using System.Collections;
using System.Collections.Generic;

public class MyList : IListType, IList<int>, ICollection<int>, IEnumerable<int>, IEnumerable
{
	public List<int> _backing;

	public int Count => _backing.Count;

	public bool IsReadOnly
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public int this[int index]
	{
		get
		{
			return _backing[index];
		}
		set
		{
			_backing[index] = value;
		}
	}

	public MyList()
	{
		_backing = new List<int>();
	}

	public MyList(List<int> list)
	{
		_backing = new List<int>(list);
	}

	public IEnumerator<int> GetEnumerator()
	{
		return _backing.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(int item)
	{
		_backing.Add(item);
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	public bool Contains(int item)
	{
		throw new NotImplementedException();
	}

	public void CopyTo(int[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	public bool Remove(int item)
	{
		throw new NotImplementedException();
	}

	public int IndexOf(int item)
	{
		throw new NotImplementedException();
	}

	public void Insert(int index, int item)
	{
		throw new NotImplementedException();
	}

	public void RemoveAt(int index)
	{
		throw new NotImplementedException();
	}
}
