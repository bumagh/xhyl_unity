public interface BCBM_ISubject
{
	void Attach(BCBM_IObserver observer);

	void Detach(BCBM_IObserver observer);

	void Notify(object obj = null);
}
