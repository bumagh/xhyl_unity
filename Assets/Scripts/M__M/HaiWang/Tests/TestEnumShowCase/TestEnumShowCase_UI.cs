using UnityEngine;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class TestEnumShowCase_UI : MonoBehaviour
	{
		public View_Test view_test;

		public View_Test_Selector view_test_selector;

		public ViewMgr<UI_View_Name> viewMgr;

		public ViewMgr<UI_View_Name2> viewMgr2;

		private void Awake()
		{
			view_test = new View_Test();
			view_test_selector = new View_Test_Selector();
			viewMgr = new ViewMgr<UI_View_Name>();
			viewMgr2 = new ViewMgr<UI_View_Name2>();
			Test_Veiw_Test_Selector();
		}

		private void Start()
		{
			UnityEngine.Debug.Log("TestEnumShowCase.Start");
		}

		private void Test_View_Test()
		{
			viewMgr.Register(view_test, UI_View_Name.View_Test);
			viewMgr.ChangeView(UI_View_Name.View_Test);
		}

		private void Test_Veiw_Test_Selector()
		{
			viewMgr2.Register(view_test_selector, UI_View_Name2.View_Test_Selector);
			viewMgr2.ChangeView(UI_View_Name2.View_Test_Selector);
		}

		private void Update()
		{
			viewMgr.Update();
			viewMgr2.Update();
		}

		private void OnGUI()
		{
			viewMgr.Display();
			viewMgr2.Display();
		}

		private void Get_View_Test()
		{
		}

		private void OnGUI_View_Test()
		{
		}
	}
}
