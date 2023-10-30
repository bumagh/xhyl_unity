namespace PathSystem
{
	public interface FK3_IAgentFactory<T>
	{
		FK3_NavPathAgent Create(T type, object userData);

		void Destroy(FK3_NavPathAgent obj, T type);
	}
}
