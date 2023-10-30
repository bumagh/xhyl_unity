using M__M.HaiWang.Scenario;
using System.Collections;
using UnityEngine;

public class Scene_TestScenario_Main : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(IE_StoryControl());
	}

	private IEnumerator IE_StoryControl()
	{
		yield return new WaitForSeconds(0.5f);
		fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().SetScenario(ScenarioType.Boss_Crab_霸王蟹);
		yield return new WaitForSeconds(0.5f);
		while (true)
		{
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().EnterNewScenario(ScenarioType.Boss_Kraken_深海八爪鱼);
			yield return new WaitForSeconds(10f);
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().EnterNewStage(3, 0);
			yield return new WaitForSeconds(10f);
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().EnterNewScenario(ScenarioType.Boss_Lantern_暗夜炬兽);
			yield return new WaitForSeconds(10f);
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().EnterNewStage(3, 0);
			yield return new WaitForSeconds(10f);
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().EnterNewScenario(ScenarioType.Boss_Crab_霸王蟹);
			yield return new WaitForSeconds(10f);
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().EnterNewStage(3, 0);
			yield return new WaitForSeconds(10f);
		}
	}
}
