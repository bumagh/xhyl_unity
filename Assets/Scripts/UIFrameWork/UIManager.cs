using M__M.HaiWang.Demo;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
	public class UIManager : MonoBehaviour
	{
		private Dictionary<string, string> dicUIFormsPaths;

		private Dictionary<string, BaseUIForm> dicAllUIForms;

		private Stack<BaseUIForm> stackPopupUIForms;

		[SerializeField]
		private Transform traCanvasTransform;

		private Transform traNormal;

		private Transform traPopup;

		private Transform traFront;

		private Transform traMaskPanel;

		private Transform traGoTopPanel;

		private Transform traScriptsMgr;

		private JsonManager jsonManager = new JsonManager();

		public virtual void Awake()
		{
			dicUIFormsPaths = new Dictionary<string, string>();
			dicAllUIForms = new Dictionary<string, BaseUIForm>();
			stackPopupUIForms = new Stack<BaseUIForm>();
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
			BaseUIForm value2 = null;
			if (!dicAllUIForms.TryGetValue(uiName, out value2) && dicUIFormsPaths.TryGetValue(uiName, out value))
			{
				value2 = LoadingLogic.Get().FindGame(uiName).GetComponent<BaseUIForm>();
				if (value2 == null)
				{
					UnityEngine.Debug.LogError(value + " 是空的");
					return;
				}
				UnityEngine.Debug.LogError(value + " 加载完成");
				value2.name = uiName;
				value2.gameObject.SetActive(value: false);
				UIType uiType = value2.uiType;
				if (uiType.uiFormType == UIFormTypes.Normal)
				{
					value2.transform.SetParent(traNormal);
				}
				else if (uiType.uiFormType == UIFormTypes.Popup)
				{
					value2.transform.SetParent(traPopup);
				}
				else if (uiType.uiFormType == UIFormTypes.Front)
				{
					value2.transform.SetParent(traFront);
				}
				value2.transform.localScale = Vector3.one;
				value2.transform.localRotation = Quaternion.identity;
			}
			if (show)
			{
				UIType uiType2 = value2.uiType;
				if (uiType2.uiFormType == UIFormTypes.Popup)
				{
					if (stackPopupUIForms.Count > 0)
					{
						BaseUIForm baseUIForm = stackPopupUIForms.Peek();
						baseUIForm.transform.parent = traGoTopPanel;
					}
					MaskMgr.GetInstance().ShowMaskPanel(value2);
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
			BaseUIForm value = null;
			if (!dicAllUIForms.TryGetValue(uiName, out value) || !value.gameObject.activeInHierarchy)
			{
				return;
			}
			value.hide();
			UIType uiType = value.uiType;
			if (uiType.uiFormType == UIFormTypes.Popup)
			{
				BaseUIForm baseUIForm = stackPopupUIForms.Pop();
				if (stackPopupUIForms.Count > 0)
				{
					baseUIForm = stackPopupUIForms.Peek();
					baseUIForm.transform.parent = traPopup;
					MaskMgr.GetInstance().ShowMaskPanel(baseUIForm);
				}
				else
				{
					MaskMgr.GetInstance().HideMaskPanel();
				}
			}
		}
	}
}
