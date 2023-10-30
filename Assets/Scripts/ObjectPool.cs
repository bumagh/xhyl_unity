using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
	private readonly Stack<T> m_Stack = new Stack<T>();

	private readonly Func<T> m_Creator;

	private readonly Action<T> m_Destroyer;

	private readonly Action<T> m_ActionOnGet;

	private readonly Action<T> m_ActionOnRelease;

	public int countAll
	{
		get;
		private set;
	}

	public int countActive => countAll - countInactive;

	public int countInactive => m_Stack.Count;

	public ObjectPool(Func<T> creator, Action<T> destroyer = null, Action<T> actionOnGet = null, Action<T> actionOnRelease = null)
	{
		m_Creator = creator;
		m_Destroyer = destroyer;
		m_ActionOnGet = actionOnGet;
		m_ActionOnRelease = actionOnRelease;
	}

	public T Get()
	{
		T val;
		if (m_Stack.Count == 0)
		{
			val = m_Creator();
			countAll++;
		}
		else
		{
			val = m_Stack.Pop();
		}
		if (m_ActionOnGet != null)
		{
			m_ActionOnGet(val);
		}
		return val;
	}

	public void Release(T element)
	{
		if (m_Stack.Count > 0 && object.ReferenceEquals(m_Stack.Peek(), element))
		{
			UnityEngine.Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
		}
		if (m_ActionOnRelease != null)
		{
			m_ActionOnRelease(element);
		}
		m_Stack.Push(element);
	}

	public void Prepare(int count)
	{
		for (int i = 0; i < count; i++)
		{
			T item = m_Creator();
			countAll++;
			m_Stack.Push(item);
		}
	}

	public void Clear()
	{
		int countInactive = this.countInactive;
		countAll -= countInactive;
		for (int i = 0; i < countInactive; i++)
		{
			T obj = Get();
			if (m_Destroyer != null)
			{
				m_Destroyer(obj);
			}
		}
	}
}
