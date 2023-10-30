using M__M.HaiWang.Scenario;
using System.Collections;
using UnityEngine;

public class FK3_Scene_TestScenario_Main : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(IE_StoryControl());
	}

	private IEnumerator IE_StoryControl()
	{
		yield return new WaitForSeconds(0.5f);
		FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().SetScenario(FK3_ScenarioType.Boss_Crab_霸王蟹);
		yield return new WaitForSeconds(0.5f);
		while (true)
		{
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().EnterNewScenario(FK3_ScenarioType.Boss_Kraken_深海八爪鱼);
			yield return new WaitForSeconds(10f);
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().EnterNewStage(3, 0);
			yield return new WaitForSeconds(10f);
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().EnterNewScenario(FK3_ScenarioType.Boss_Lantern_暗夜炬兽);
			yield return new WaitForSeconds(10f);
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().EnterNewStage(3, 0);
			yield return new WaitForSeconds(10f);
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().EnterNewScenario(FK3_ScenarioType.Boss_Crab_霸王蟹);
			yield return new WaitForSeconds(10f);
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().EnterNewStage(3, 0);
			yield return new WaitForSeconds(10f);
		}
	}
}
