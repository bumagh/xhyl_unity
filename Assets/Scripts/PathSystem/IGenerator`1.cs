namespace PathSystem
{
	public interface IGenerator<T>
	{
		void Init(SubFormation<T> formation);

		AgentData<T> GetNext(object userData);

		T[] GetTypes();

		void Reset();
	}
}
