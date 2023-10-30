using UnityEngine;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class FK3_TestEnumShowCase_UI : MonoBehaviour
	{
		public FK3_View_Test view_test;

		public FK3_View_Test_Selector view_test_selector;

		public FK3_ViewMgr<FK3_UI_View_Name> viewMgr;

		public FK3_ViewMgr<FK3_UI_View_Name2> viewMgr2;

		private void Awake()
		{
			view_test = new FK3_View_Test();
			view_test_selector = new FK3_View_Test_Selector();
			viewMgr = new FK3_ViewMgr<FK3_UI_View_Name>();
			viewMgr2 = new FK3_ViewMgr<FK3_UI_View_Name2>();
			Test_Veiw_Test_Selector();
		}

		private void Start()
		{
			UnityEngine.Debug.Log("TestEnumShowCase.Start");
		}

		private void Test_View_Test()
		{
			viewMgr.Register(view_test, FK3_UI_View_Name.FK3_View_Test);
			viewMgr.ChangeView(FK3_UI_View_Name.FK3_View_Test);
		}

		private void Test_Veiw_Test_Selector()
		{
			viewMgr2.Register(view_test_selector, FK3_UI_View_Name2.FK3_View_Test_Selector);
			viewMgr2.ChangeView(FK3_UI_View_Name2.FK3_View_Test_Selector);
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
