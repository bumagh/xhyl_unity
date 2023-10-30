using DG.Tweening;
using FullInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Scenario
{
	public class FK3_StoryScenarioBehaviour : BaseBehavior<FullSerializerSerializer>
	{
		[SerializeField]
		private FK3_ScenarioBase m_scenario;

		public Image logoBoss;

		[SerializeField]
		private Vector3 m_baisc_scale = Vector3.one;

		public FK3_ScenarioType type => (m_scenario == null) ? FK3_ScenarioType.Boss_Crab_霸王蟹 : m_scenario.type;

		public void Start()
		{
			m_scenario.SetManger(this);
			m_scenario.Assert();
		}

		public void ScenarioProgress(int dur)
		{
			m_scenario.ScenarioProgress(dur);
		}

		[InspectorButton]
		public void Enter()
		{
			StopAllCoroutines();
			m_scenario.Enter();
		}

		[InspectorButton]
		public void BossEnter(int dur)
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("BOSS来袭阶段的背景音乐", loop: true);
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("BOSS来袭阶段的背景音乐", 0.7f);
			m_scenario.BossEnter();
			if (dur < 2)
			{
				StartCoroutine(IE_BossEnter());
			}
		}

		public void ScenarioRotate(int count)
		{
			m_scenario.ScenarioRotate(count);
		}

		public void ScenarioMove(int dur)
		{
			m_scenario.ScenarioMove(dur);
		}

		private IEnumerator IE_BossEnter()
		{
			logoBoss.gameObject.SetActive(value: true);
			Sequence s = DOTween.Sequence();
			logoBoss.transform.position = Vector3.right * 10f;
			logoBoss.transform.localScale = Vector3.one;
			s.Append(logoBoss.transform.DOMoveX(0f, 1f).SetEase(Ease.InOutBack));
			s.Append(logoBoss.transform.DOLocalRotate(new Vector3(0f, 0f, 10f), 0.5f).OnStart(delegate
			{
				logoBoss.transform.DOScale(1.2f, 0.5f).SetLoops(6, LoopType.Yoyo);
			}));
			s.Append(logoBoss.transform.DOLocalRotate(new Vector3(0f, 0f, -10f), 0.5f).SetLoops(4, LoopType.Yoyo));
			s.Append(logoBoss.transform.DOLocalRotate(new Vector3(0f, 0f, -0f), 0.5f));
			s.Append(logoBoss.transform.DOMoveX(-15f, 1f).SetEase(Ease.OutBack));
			s.AppendCallback(delegate
			{
				logoBoss.gameObject.SetActive(value: false);
			});
			yield break;
		}

		[InspectorButton]
		public void Exit()
		{
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("BOSS来袭阶段的背景音乐"));
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("剧情结束动画背景音乐");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("剧情结束动画背景音乐", 0.7f);
			m_scenario.Exit();
		}

		public Image[] GetRenderers4Fade()
		{
			return m_scenario.GetRenderers4Fade();
		}
	}
}
