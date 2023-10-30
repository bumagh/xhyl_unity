namespace PathSystem
{
	public interface FK3_IGenerator<T>
	{
		void Init(FK3_SubFormation<T> formation);

		FK3_AgentData<T> GetNext(object userData);

		T[] GetTypes();

		void Reset();
	}
}
