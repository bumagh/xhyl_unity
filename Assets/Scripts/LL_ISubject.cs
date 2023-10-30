public interface LL_ISubject
{
	void Attach(LL_IObserver observer);

	void Detach(LL_IObserver observer);

	void Notify(object obj = null);
}
