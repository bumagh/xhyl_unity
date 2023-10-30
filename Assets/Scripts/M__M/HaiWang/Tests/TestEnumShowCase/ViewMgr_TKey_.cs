using System.Collections.Generic;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class ViewMgr<TKey>
	{
		private struct Pair
		{
			public TKey key;

			public ViewBase value;
		}

		private Dictionary<TKey, ViewBase> dic = new Dictionary<TKey, ViewBase>();

		private Pair cur_pair = default(Pair);

		public void Register(ViewBase view, TKey name)
		{
			dic[name] = view;
		}

		public ViewBase GetViewBase(TKey name)
		{
			return dic[name];
		}

		public T GetView<T>(TKey name) where T : ViewBase
		{
			return (T)GetViewBase(name);
		}

		public void ChangeView(TKey name)
		{
			cur_pair.key = name;
			cur_pair.value = GetViewBase(name);
		}

		public void Display()
		{
			if (cur_pair.value != null)
			{
				cur_pair.value.Display();
			}
		}

		public void Update()
		{
			if (cur_pair.value != null)
			{
				cur_pair.value.Update();
			}
		}
	}
}
