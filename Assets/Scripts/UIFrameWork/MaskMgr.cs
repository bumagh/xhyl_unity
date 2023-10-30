using UnityEngine;

namespace UIFrameWork
{
	public class MaskMgr : MonoBehaviour
	{
		private static MaskMgr instance;

		private Transform traCanvasTransform;

		private Transform traMaskPanel;

		private Transform traGoTopPanel;

		private Transform traScriptsMgr;

		public static MaskMgr GetInstance()
		{
			if (instance == null)
			{
				instance = new GameObject("MaskMgr").AddComponent<MaskMgr>();
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

		public void ShowMaskPanel(BaseUIForm baseUIForm)
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
