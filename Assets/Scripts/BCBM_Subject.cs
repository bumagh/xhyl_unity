using System.Collections;

public class BCBM_Subject : BCBM_ISubject
{
	private ArrayList mObserverList;

	private int _type;

	public BCBM_Subject(int type)
	{
		mObserverList = new ArrayList();
		_type = type;
	}

	public void Attach(BCBM_IObserver observer)
	{
		mObserverList.Add(observer);
	}

	public void Detach(BCBM_IObserver observer)
	{
		mObserverList.Remove(observer);
	}

	public void Notify(object obj = null)
	{
		for (int i = 0; i < mObserverList.Count; i++)
		{
			BCBM_IObserver bCBM_IObserver = (BCBM_IObserver)mObserverList[i];
			bCBM_IObserver.OnRcvMsg(_type, obj);
		}
	}
}
