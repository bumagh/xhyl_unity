using M__M.HaiWang.UIDefine;
using UnityEngine;

namespace M__M.HaiWang.UI
{
	public class InGameUI2 : SimpleSingletonBehaviour<InGameUI2>
	{
		[SerializeField]
		private RectTransform m_inGameUIRoot;

		public GameObject[] UI_GmObj;

		private InGameUIContext m_context = new InGameUIContext();

		private void Awake()
		{
			SimpleSingletonBehaviour<InGameUI2>.s_instance = this;
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

		public InGameUIContext GetContext()
		{
			return m_context;
		}
	}
}
