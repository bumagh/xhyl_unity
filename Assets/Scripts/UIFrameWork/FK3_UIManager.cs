using M__M.HaiWang.Demo;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
	public class FK3_UIManager : MonoBehaviour
	{
		private Dictionary<string, string> dicUIFormsPaths;

		private Dictionary<string, FK3_BaseUIForm> dicAllUIForms;

		private Stack<FK3_BaseUIForm> stackPopupUIForms;

		[SerializeField]
		private Transform traCanvasTransform;

		private Transform traNormal;

		private Transform traPopup;

		private Transform traFront;

		private Transform traMaskPanel;

		private Transform traGoTopPanel;

		private Transform traScriptsMgr;

		private FK3_JsonManager jsonManager = new FK3_JsonManager();

		public virtual void Awake()
		{
			dicUIFormsPaths = new Dictionary<string, string>();
			dicAllUIForms = new Dictionary<string, FK3_BaseUIForm>();
			stackPopupUIForms = new Stack<FK3_BaseUIForm>();
			traNormal = traCanvasTransform.Find("Normal");
			traPopup = traCanvasTransform.Find("Popup");
			traFront = traCanvasTransform.Find("Front");
			traMaskPanel = traPopup.Find("MaskPanel");
			traMaskPanel.gameObject.SetActive(value: false);
			traGoTopPanel = traCanvasTransform.Find("Popup/GoTopPanel");
			traScriptsMgr = traCanvasTransform.Find("ScriptsMgr");
			dicUIFormsPaths = jsonManager.LoadJsonData("Config\\UIFormPathes");
		}

		public virtual void OpenUI(string uiName, bool show = true)
		{
			string value = string.Empty;
			FK3_BaseUIForm value2 = null;
			if (!dicAllUIForms.TryGetValue(uiName, out value2) && dicUIFormsPaths.TryGetValue(uiName, out value))
			{
				value2 = FK3_LoadingLogic.Get().FindGame(uiName).GetComponent<FK3_BaseUIForm>();
				if (value2 == null)
				{
					UnityEngine.Debug.LogError(value + " 是空的");
					return;
				}
				value2.name = uiName;
				value2.gameObject.SetActive(value: false);
				FK3_UIType uiType = value2.uiType;
				if (uiType.uiFormType == FK3_UIFormTypes.Normal)
				{
					value2.transform.SetParent(traNormal);
				}
				else if (uiType.uiFormType == FK3_UIFormTypes.Popup)
				{
					value2.transform.SetParent(traPopup);
				}
				else if (uiType.uiFormType == FK3_UIFormTypes.Front)
				{
					value2.transform.SetParent(traFront);
				}
				if (uiName != "OpScore")
				{
					value2.transform.localScale = Vector3.one;
				}
				value2.transform.localRotation = Quaternion.identity;
			}
			if (show)
			{
				FK3_UIType uiType2 = value2.uiType;
				if (uiType2.uiFormType == FK3_UIFormTypes.Popup)
				{
					if (stackPopupUIForms.Count > 0)
					{
						FK3_BaseUIForm fK3_BaseUIForm = stackPopupUIForms.Peek();
						fK3_BaseUIForm.transform.parent = traGoTopPanel;
					}
					FK3_MaskMgr.GetInstance().ShowMaskPanel(value2);
					stackPopupUIForms.Push(value2);
				}
				value2.Display();
			}
			if (!dicAllUIForms.ContainsKey(uiName))
			{
				dicAllUIForms.Add(uiName, value2);
			}
		}

		public virtual void CloseUI(string uiName)
		{
			FK3_BaseUIForm value = null;
			if (!dicAllUIForms.TryGetValue(uiName, out value) || !value.gameObject.activeInHierarchy)
			{
				return;
			}
			value.hide();
			FK3_UIType uiType = value.uiType;
			if (uiType.uiFormType == FK3_UIFormTypes.Popup)
			{
				FK3_BaseUIForm fK3_BaseUIForm = stackPopupUIForms.Pop();
				if (stackPopupUIForms.Count > 0)
				{
					fK3_BaseUIForm = stackPopupUIForms.Peek();
					fK3_BaseUIForm.transform.parent = traPopup;
					FK3_MaskMgr.GetInstance().ShowMaskPanel(fK3_BaseUIForm);
				}
				else
				{
					FK3_MaskMgr.GetInstance().HideMaskPanel();
				}
			}
		}
	}
}
