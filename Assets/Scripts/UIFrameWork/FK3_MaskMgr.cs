using UnityEngine;

namespace UIFrameWork
{
	public class FK3_MaskMgr : MonoBehaviour
	{
		private static FK3_MaskMgr instance;

		private Transform traCanvasTransform;

		private Transform traMaskPanel;

		private Transform traGoTopPanel;

		private Transform traScriptsMgr;

		public static FK3_MaskMgr GetInstance()
		{
			if (instance == null)
			{
				instance = new GameObject("MaskMgr").AddComponent<FK3_MaskMgr>();
			}
			return instance;
		}

		private void Awake()
		{
			traCanvasTransform = GameObject.Find("Canvas").transform;
			traMaskPanel = traCanvasTransform.Find("Popup/MaskPanel");
			traMaskPanel.gameObject.SetActive(value: false);
			traGoTopPanel = traCanvasTransform.Find("Popup/GoTopPanel");
			traScriptsMgr = traCanvasTransform.Find("ScriptsMgr");
			base.gameObject.transform.parent = traScriptsMgr;
		}

		public void ShowMaskPanel(FK3_BaseUIForm baseUIForm)
		{
			traMaskPanel.gameObject.SetActive(value: true);
			traGoTopPanel.SetAsFirstSibling();
			traMaskPanel.SetAsLastSibling();
			baseUIForm.transform.SetAsLastSibling();
		}

		public void HideMaskPanel()
		{
			traMaskPanel.gameObject.SetActive(value: false);
		}
	}
}
