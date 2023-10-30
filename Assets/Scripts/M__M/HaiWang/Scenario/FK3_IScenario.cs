namespace M__M.HaiWang.Scenario
{
	public interface FK3_IScenario
	{
		void Assert();

		void ScenarioProgress(int dur);

		void Enter();

		void BossEnter();

		void ScenarioRotate(int dur);

		void ScenarioMove(int dur);

		void Exit();
	}
}
