namespace PathSystem
{
	public class FK3_SingleType<T> : FK3_GeneratorBase<T>
	{
		public T type;

		public override FK3_AgentData<T> GetNext(object userData)
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
