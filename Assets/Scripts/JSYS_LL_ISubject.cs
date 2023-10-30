public interface JSYS_LL_ISubject
{
	void Attach(JSYS_LL_IObserver observer);

	void Detach(JSYS_LL_IObserver observer);

	void Notify(object obj = null);
}
