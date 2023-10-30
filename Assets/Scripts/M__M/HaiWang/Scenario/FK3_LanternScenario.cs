using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Scenario
{
	[Serializable]
	public class FK3_LanternScenario : FK3_ScenarioBase
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

		private FK3_LanternScenario()
			: base(FK3_ScenarioType.Boss_Lantern_暗夜炬兽)
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
			UnityEngine.Debug.Log(FK3_LogHelper.Brown("暗夜炬兽 Enter"));
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("暗夜炬兽背景音乐", loop: true);
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("暗夜炬兽背景音乐", 0.7f);
			bg_root.transform.localPosition = new Vector3(0f, 0f, 0f);
			ParticleControl(play: true);
		}

		public override void BossEnter()
		{
			pointLight.SetActive(value: true);
			UnityEngine.Debug.Log(FK3_LogHelper.Magenta("FK3_LanternScenario BossEnter"));
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("暗夜炬兽背景音乐"));
			ChangeEnvironment();
			bg_root.transform.DOLocalMoveX(54f * (float)direction, lastTime).SetEase(Ease.Linear);
		}

		private void ChangeEnvironment()
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Magenta("FK3_LanternScenario ChangeEnvironment"));
		}

		public override void ScenarioMove(int dur)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Brown("FK3_LanternScenario ScenarioMove"));
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
