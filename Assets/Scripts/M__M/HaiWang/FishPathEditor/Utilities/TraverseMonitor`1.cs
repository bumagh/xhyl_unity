namespace M__M.HaiWang.FishPathEditor.Utilities
{
	public class TraverseMonitor<T>
	{
		public delegate void NodeCallback(T node);

		private NodeCallback onReachDepthLimit;

		private NodeCallback onReachCountLimit;

		private NodeCallback onReachNode;

		public int count
		{
			get;
			private set;
		}

		public int depth
		{
			get;
			private set;
		}

		public int topDepth
		{
			get;
			private set;
		}

		public int countLimit
		{
			get;
			private set;
		}

		public int depthLimit
		{
			get;
			private set;
		}

		public TraverseMonitor(int depthLimit = int.MaxValue, NodeCallback onReachDepthLimit = null, int countLimit = int.MaxValue, NodeCallback onReachCountLimit = null, NodeCallback onReachNode = null)
		{
			this.depthLimit = depthLimit;
			this.countLimit = countLimit;
			this.onReachDepthLimit = onReachDepthLimit;
			this.onReachCountLimit = onReachCountLimit;
			this.onReachNode = onReachNode;
			Reset();
		}

		public void Reset()
		{
			count = 0;
			depth = 0;
			topDepth = 0;
		}

		public void StepIn(T node)
		{
			count++;
			depth++;
			if (depth > depthLimit && onReachDepthLimit != null)
			{
				onReachDepthLimit(node);
			}
			if (count > countLimit && onReachCountLimit != null)
			{
				onReachCountLimit(node);
			}
			if (onReachCountLimit != null)
			{
				onReachNode(node);
			}
		}

		public void StepOut()
		{
			depth--;
		}
	}
}
