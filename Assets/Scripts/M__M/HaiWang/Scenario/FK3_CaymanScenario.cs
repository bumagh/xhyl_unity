using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Scenario
{
	public class FK3_CaymanScenario : FK3_ScenarioBase
	{
		public Image mapDown;

		public Image mapUp;

		public Transform mapDownOldPos;

		public Transform mapUpOldPos;

		public Transform mapDownTagPos;

		public Transform mapUpTagPos;

		private Tween tweenMoveBg;

		private FK3_CaymanScenario()
			: base(FK3_ScenarioType.Boss_Cayman_鳄鱼)
		{
		}

		public override void Assert()
		{
		}

		private void Reset()
		{
			mapDown.gameObject.SetActive(value: true);
			mapUp.gameObject.SetActive(value: true);
			mapDown.transform.localPosition = mapDownOldPos.localPosition;
			mapUp.transform.localPosition = mapUpOldPos.localPosition;
		}

		public override Image[] GetRenderers4Fade()
		{
			return new Image[2]
			{
				mapDown,
				mapUp
			};
		}

		public override void Enter()
		{
			Reset();
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("史前巨鳄背景音乐", loop: true);
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("史前巨鳄背景音乐", 0.7f);
		}

		public override void BossEnter()
		{
			UnityEngine.Debug.LogError("史前巨鳄来袭");
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("史前巨鳄背景音乐"));
			BGMove();
		}

		private void BGMove()
		{
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(25);
			mapDown.transform.DOLocalMove(mapDownTagPos.localPosition, 2.5f);
			tweenMoveBg = mapUp.transform.DOLocalMove(mapUpTagPos.localPosition, 2.5f);
			tweenMoveBg.OnComplete(delegate
			{
				mapDown.gameObject.SetActive(value: false);
				mapUp.gameObject.SetActive(value: false);
			});
		}

		public override void Exit()
		{
			Reset();
		}
	}
}
