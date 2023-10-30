using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Scenario
{
	[Serializable]
	public class LanternScenario : ScenarioBase
	{
		public Transform bg_root;

		public Image bg_cyan;

		public Image bg_cyan1;

		public Image bg_cyan2;

		public Image bg_cyan3;

		public Image bg_cyan4;

		public GameObject pointLight;

		private int lastTime = 180;

		private int direction = 1;

		private LanternScenario()
			: base(ScenarioType.Boss_Lantern_暗夜炬兽)
		{
		}

		public override Image[] GetRenderers4Fade()
		{
			return new Image[5]
			{
				bg_cyan,
				bg_cyan1,
				bg_cyan2,
				bg_cyan3,
				bg_cyan4
			};
		}

		private void ParticleControl(bool play)
		{
			for (int i = 0; i < GetRenderers4Fade().Length; i++)
			{
				GetRenderers4Fade()[i].transform.GetChild(0).gameObject.SetActive(play);
			}
		}

		public override void Enter()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Brown("LanternScenario Enter"));
			HW2_Singleton<SoundMgr>.Get().PlayClip("暗夜炬兽背景音乐", loop: true);
			HW2_Singleton<SoundMgr>.Get().SetVolume("暗夜炬兽背景音乐", 0.7f);
			bg_root.transform.localPosition = new Vector3(0f, 0f, 0f);
			ParticleControl(play: true);
		}

		public override void BossEnter()
		{
			pointLight.SetActive(value: true);
			UnityEngine.Debug.Log(HW2_LogHelper.Magenta("LanternScenario BossEnter"));
			HW2_Singleton<SoundMgr>.Get().StopClip(HW2_Singleton<SoundMgr>.Get().GetClip("暗夜炬兽背景音乐"));
			ChangeEnvironment();
			bg_root.transform.DOLocalMoveX(54f * (float)direction, lastTime).SetEase(Ease.Linear);
		}

		private void ChangeEnvironment()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Magenta("LanternScenario ChangeEnvironment"));
		}

		public override void ScenarioMove(int dur)
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Brown("LanternScenario ScenarioMove"));
			lastTime -= dur;
			bg_root.transform.localPosition = new Vector3((float)dur * 0.3f * (float)direction, 0f, 0f);
		}

		public override void Exit()
		{
			pointLight.SetActive(value: false);
			DOTween.Kill(bg_root.transform);
			bg_root.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
	}
}
