namespace PathSystem
{
	public interface IAgentFactory<T>
	{
		NavPathAgent Create(T type, object userData);

		void Destroy(NavPathAgent obj, T type);
	}
}
