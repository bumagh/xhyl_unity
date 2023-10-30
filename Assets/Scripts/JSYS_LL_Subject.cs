using System.Collections;

public class JSYS_LL_Subject : JSYS_LL_ISubject
{
	private ArrayList mObserverList;

	private int _type;

	public JSYS_LL_Subject(int type)
	{
		mObserverList = new ArrayList();
		_type = type;
	}

	public void Attach(JSYS_LL_IObserver observer)
	{
		mObserverList.Add(observer);
	}

	public void Detach(JSYS_LL_IObserver observer)
	{
		mObserverList.Remove(observer);
	}

	public void Notify(object obj = null)
	{
		for (int i = 0; i < mObserverList.Count; i++)
		{
			JSYS_LL_IObserver jSYS_LL_IObserver = (JSYS_LL_IObserver)mObserverList[i];
			jSYS_LL_IObserver.OnRcvMsg(_type, obj);
		}
	}
}
