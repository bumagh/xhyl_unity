using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Scenario
{
	[Serializable]
	public class CrabScenario : ScenarioBase
	{
		public Image bg_green;

		public Image floating_island;

		private CrabScenario()
			: base(ScenarioType.Boss_Crab_霸王蟹)
		{
		}

		public override void Assert()
		{
		}

		public override Image[] GetRenderers4Fade()
		{
			return new Image[2]
			{
				bg_green,
				floating_island
			};
		}

		public override void Enter()
		{
			Stop_Crab();
			Reset_Crab();
			UnityEngine.Debug.Log(HW2_LogHelper.Brown("CrabScenario Enter"));
			HW2_Singleton<SoundMgr>.Get().PlayClip("霸王蟹剧情背景音乐", loop: true);
			HW2_Singleton<SoundMgr>.Get().SetVolume("霸王蟹剧情背景音乐", 0.7f);
			floating_island.transform.DOScale(new Vector3(0.94f, 0.94f, 1f), 2.5f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
		}

		public override void BossEnter()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Magenta("CrabScenario BossEnter"));
			HW2_Singleton<SoundMgr>.Get().StopClip(HW2_Singleton<SoundMgr>.Get().GetClip("霸王蟹剧情背景音乐"));
			Stop_Crab();
			floating_island.transform.DOScale(new Vector3(0.64f, 0.64f, 1f), 6f).SetEase(Ease.OutQuad).OnComplete(delegate
			{
				floating_island.transform.DOScale(new Vector3(0.68f, 0.68f, 1f), 2.5f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
			});
		}

		public override void Exit()
		{
			Stop_Crab();
			Reset_Crab();
		}

		private void Reset_Crab()
		{
			bg_green.transform.localScale = Vector3.one;
			floating_island.transform.localScale = Vector3.one;
		}

		private void Stop_Crab()
		{
			DOTween.Kill(bg_green.transform);
			DOTween.Kill(floating_island.transform);
		}

		private IEnumerator IE_Enter_Crab()
		{
			UnityEngine.Debug.Log("IE_Enter_Crab");
			Stop_Crab();
			Reset_Crab();
			yield return null;
			bg_green.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 5f);
			floating_island.transform.DOScale(Vector3.one * 1.5f, 5f).SetLoops(-1, LoopType.Yoyo);
		}

		private IEnumerator IE_DelayCall(float time, Action act)
		{
			yield return new WaitForSeconds(time);
			act();
		}
	}
}
