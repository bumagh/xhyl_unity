using M__M.HaiWang.UIDefine;
using UnityEngine;

namespace M__M.HaiWang.UI
{
	public class FK3_InGameUI2 : FK3_SimpleSingletonBehaviour<FK3_InGameUI2>
	{
		[SerializeField]
		private RectTransform m_inGameUIRoot;

		public GameObject[] UI_GmObj;

		private FK3_InGameUIContext m_context = new FK3_InGameUIContext();

		private void Awake()
		{
			FK3_SimpleSingletonBehaviour<FK3_InGameUI2>.s_instance = this;
			GameObject[] uI_GmObj = UI_GmObj;
			foreach (GameObject gameObject in uI_GmObj)
			{
				gameObject.SetActive(value: true);
				gameObject.SetActive(value: false);
			}
		}

		private void Start()
		{
		}

		public FK3_InGameUIContext GetContext()
		{
			return m_context;
		}
	}
}
