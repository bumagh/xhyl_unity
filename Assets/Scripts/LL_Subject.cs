using System.Collections;

public class LL_Subject : LL_ISubject
{
	private ArrayList mObserverList;

	private int _type;

	public LL_Subject(int type)
	{
		mObserverList = new ArrayList();
		_type = type;
	}

	public void Attach(LL_IObserver observer)
	{
		mObserverList.Add(observer);
	}

	public void Detach(LL_IObserver observer)
	{
		mObserverList.Remove(observer);
	}

	public void Notify(object obj = null)
	{
		for (int i = 0; i < mObserverList.Count; i++)
		{
			LL_IObserver lL_IObserver = (LL_IObserver)mObserverList[i];
			lL_IObserver.OnRcvMsg(_type, obj);
		}
	}
}
