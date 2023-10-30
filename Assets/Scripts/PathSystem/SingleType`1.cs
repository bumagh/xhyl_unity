namespace PathSystem
{
	public class SingleType<T> : GeneratorBase<T>
	{
		public T type;

		public override AgentData<T> GetNext(object userData)
		{
			return _getNext(type, userData);
		}

		public override T[] GetTypes()
		{
			return new T[1]
			{
				type
			};
		}
	}
}
