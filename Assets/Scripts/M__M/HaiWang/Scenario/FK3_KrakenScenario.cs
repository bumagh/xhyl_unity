using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Scenario
{
	[Serializable]
	public class FK3_KrakenScenario : FK3_ScenarioBase
	{
		public Image bg_red;

		public Image Background_Kraken_BlackHolel;

		public Image Kraken;

		public Image Kraken_Eye;

		public Image Background_KrakenRock_1;

		public Image Background_KrakenRock_2;

		public Image Background_KrakenRock_3;

		public Image Background_KrakenRock_4;

		public Image Background_KrakenRock_5;

		public Image Background_KrakenRock_6;

		private Vector3[] krakenMovePoint = new Vector3[4]
		{
			new Vector3(0.2f, 0.2f, 1f),
			new Vector3(0.2f, -0.2f, 1f),
			new Vector3(-0.2f, -0.2f, 1f),
			new Vector3(-0.2f, 0.2f, 1f)
		};

		private float rotateSpeed = 1f;

		private float fixedAngle = 90f;

		private int startTime;

		private int bossStayTime = 90;

		private FK3_KrakenScenario()
			: base(FK3_ScenarioType.Boss_Kraken_深海八爪鱼)
		{
		}

		public override Image[] GetRenderers4Fade()
		{
			return new Image[9]
			{
				bg_red,
				Background_Kraken_BlackHolel,
				Kraken,
				Background_KrakenRock_1,
				Background_KrakenRock_2,
				Background_KrakenRock_3,
				Background_KrakenRock_4,
				Background_KrakenRock_5,
				Background_KrakenRock_6
			};
		}

		public override void ScenarioProgress(int dur)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Brown("FK3_KrakenScenario ScenarioProgress"));
			bg_red.transform.localEulerAngles = new Vector3(0f, 0f, (0f - rotateSpeed) * (float)dur);
		}

		public override void Enter()
		{
			Reset();
			UnityEngine.Debug.Log(FK3_LogHelper.Brown("FK3_KrakenScenario Enter"));
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("深海八爪鱼剧情背景音乐", loop: true);
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("深海八爪鱼剧情背景音乐", 0.7f);
			bg_red.transform.DOLocalRotate(new Vector3(0f, 0f, 300f), 300f).SetEase(Ease.Linear);
			KrakenMove();
		}

		private void KrakenMove()
		{
			Kraken_Eye.DOFade(0f, 1.8f).SetLoops(-1, LoopType.Yoyo);
		}

		private void Reset()
		{
			DOTween.Kill(bg_red.transform);
			DOTween.Kill(Kraken_Eye.GetComponent<Image>());
			Kraken_Eye.color = new Color(1f, 1f, 1f, 1f);
		}

		private void RotateFixedAngle()
		{
		}

		public override void BossEnter()
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Magenta("FK3_KrakenScenario BossEnter"));
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("深海八爪鱼剧情背景音乐"));
			DOTween.Kill(bg_red.transform);
			manager.StartCoroutine(IE_KrakenSceneRotate(startTime));
			UnityEngine.Debug.Log("IsTweening: " + DOTween.IsTweening(Kraken_Eye));
			if (!DOTween.IsTweening(Kraken_Eye))
			{
				KrakenMove();
			}
		}

		public override void ScenarioRotate(int dur)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Brown("FK3_KrakenScenario ScenarioRotate"));
			int num = dur / 10;
			startTime = dur;
			bg_red.transform.localEulerAngles += new Vector3(0f, 0f, (0f - (float)num) * fixedAngle);
		}

		private IEnumerator IE_KrakenSceneRotate(int wait)
		{
			bossStayTime -= wait;
			int interval = 10;
			float duration = 1.2f;
			while (bossStayTime > 10)
			{
				yield return new WaitForSeconds(interval);
				bossStayTime -= interval;
				ShortcutExtensions.DOLocalRotate(endValue: bg_red.transform.localEulerAngles + new Vector3(0f, 0f, -90f), target: bg_red.transform, duration: duration);
			}
		}

		public override void Exit()
		{
			Reset();
			startTime = 0;
			bossStayTime = 90;
			bg_red.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			Kraken_Eye.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		}
	}
}
