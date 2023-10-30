using DG.Tweening;
using DG.Tweening.Core;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Scenario
{
	public class FK3_StoryScenarioMgr : FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>
	{
		[SerializeField]
		private FK3_ScenarioBehaviourDictionary m_scenarios = new FK3_ScenarioBehaviourDictionary();

		[SerializeField]
		private Image m_seaWave;

		[SerializeField]
		private GameObject m_pointLight;

		[SerializeField]
		private Image m_gameLogo;

		[SerializeField]
		private Image m_monsterLogo;

		private FK3_StoryScenarioBehaviour m_lastScenario;

		private FK3_StoryScenarioBehaviour m_curScenario;

		public Transform krakenBornPosList;

		public Transform caymanDieEffect;

		private FK3_ScenarioType curScenarioType;

		private int curStage;

		private FK3_ShakeEffect fK3_ShakeEffect;

		private Vector3[] pointLightPoint = new Vector3[6]
		{
			new Vector3(0f, -3f, 1f),
			new Vector3(-4f, 0f, 1f),
			new Vector3(0f, 3f, 1f),
			new Vector3(4f, 0f, 1f),
			new Vector3(0f, -3f, 1f),
			new Vector3(-4f, 0f, 1f)
		};

		[CompilerGenerated]
		private static DOGetter<Color> f_mgcache;

		[CompilerGenerated]
		private static DOGetter<Color> f_mgcache1;

		private Coroutine wiatShak;

		[CompilerGenerated]
		private static DOGetter<Color> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static DOGetter<Color> _003C_003Ef__mg_0024cache1;

		public FK3_ScenarioType CurScenarioType => curScenarioType;

		public int CurStage => curStage;

		protected override void Awake()
		{
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.s_instance = this;
			fK3_ShakeEffect = new FK3_ShakeEffect(base.transform);
		}

		private void Update()
		{
			fK3_ShakeEffect.OnUpdate();
		}

		public void Init(bool faceDown)
		{
			foreach (KeyValuePair<FK3_ScenarioType, FK3_StoryScenarioBehaviour> item in m_scenarios.Value)
			{
				if (item.Value == null)
				{
					UnityEngine.Debug.Log("sceneario empty");
				}
				else
				{
					Image[] renderers4Fade = item.Value.GetRenderers4Fade();
					if (renderers4Fade == null)
					{
						UnityEngine.Debug.Log("renderers empty");
					}
					else
					{
						for (int i = 0; i < renderers4Fade.Length; i++)
						{
							Image image = renderers4Fade[i];
							if (image == null)
							{
								UnityEngine.Debug.Log("empty:" + i);
							}
							else
							{
								SetAlpha(image, 1f);
							}
						}
						item.Value.SetActive(active: false);
					}
				}
			}
		}

		public void ShakeScreen(int num = 1, float time = 0.1f)
		{
			if (wiatShak != null)
			{
				StopCoroutine(wiatShak);
			}
			wiatShak = StartCoroutine(WiatShak(num, time));
		}

		private IEnumerator WiatShak(int num, float time = 0.1f)
		{
			for (int i = 0; i < num; i++)
			{
				fK3_ShakeEffect.SetShakeInfo(-1f, time);
				yield return new WaitForSeconds(time);
			}
		}

		//TODO 没有被调用到且有错，所以注释掉
		// private void PointLightControl()
		// {
		// 	m_pointLight.transform.localPosition = pointLightPoint[0];
		// 	m_pointLight.SetActive(value: true);
		// 	DOTween.To((DOGetter<Color>)RenderSettings.get_ambientLight, (DOSetter<Color>)delegate(Color value)
		// 	{
		// 		RenderSettings.ambientLight = value;
		// 	}, new Color(0.1f, 0.1f, 0.1f, 0.1f), 1f);
		// 	m_pointLight.transform.DOLocalPath(pointLightPoint, 5f, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(delegate
		// 	{
		// 		UnityEngine.Debug.Log(FK3_LogHelper.Brown("pointLight.transform.DOLocalPath OnComplete"));
		// 		DOTween.Kill(m_pointLight.transform);
		// 		m_pointLight.SetActive(value: false);
		// 		m_pointLight.transform.localPosition = new Vector3(0f, 7f, 0f);
		// 		DOTween.To((DOGetter<Color>)RenderSettings.get_ambientLight, (DOSetter<Color>)delegate(Color value)
		// 		{
		// 			RenderSettings.ambientLight = value;
		// 		}, Color.white, 1f);
		// 	});
		// }

		public void ResetScenario()
		{
			m_curScenario.Exit();
		}

		public void SetStage(FK3_ScenarioType scenarioType, int stage, int stageType, int sceneDuration, int stageDuration)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Yellow($"sceneDuration:{sceneDuration},stageDuration:{stageDuration},"));
			if (stage == 3)
			{
				int num = sceneDuration - stageDuration;
				UnityEngine.Debug.Log(FK3_LogHelper.Brown($"stageDuration:{stageDuration},dur:{num},"));
				if (scenarioType == FK3_ScenarioType.Boss_Kraken_深海八爪鱼)
				{
					m_curScenario.ScenarioProgress(num);
					m_curScenario.ScenarioRotate(stageDuration);
				}
				if (scenarioType == FK3_ScenarioType.Boss_Lantern_暗夜炬兽)
				{
					m_curScenario.ScenarioMove(stageDuration);
				}
				m_curScenario.BossEnter(stageDuration);
			}
			else
			{
				m_curScenario.ScenarioProgress(sceneDuration);
				m_curScenario.Enter();
			}
		}

		public void EnterNewStage(int stage, int stageType)
		{
			UnityEngine.Debug.Log($"Boss Coming! [sceneario:{m_curScenario.type},stage:{stage},stageType:{stageType}]");
			if (stage == 3)
			{
				m_curScenario.BossEnter(0);
			}
			else
			{
				m_curScenario.Enter();
			}
		}

		public void SetScenario(FK3_ScenarioType scenarioType)
		{
			try
			{
				FK3_StoryScenarioBehaviour fK3_StoryScenarioBehaviour = m_curScenario = m_scenarios.Value[scenarioType];
				m_curScenario.SetActive();
				m_curScenario.Start();
				m_gameLogo.SetActive(active: false);
				m_seaWave.SetActive(active: false);
			}
			catch (Exception arg)
			{
				MonoBehaviour.print("错误: " + arg);
			}
		}

		public void RefreshScenario(FK3_ScenarioType newScenarioType, int stage, int stageType)
		{
			if (stage == 1)
			{
				RefreshNewScenario(newScenarioType);
			}
			if (stage == 3)
			{
				m_curScenario.BossEnter(0);
			}
		}

		public void ScenarioLogic(FK3_ScenarioType newScenarioType, int stage, int stageType)
		{
			curScenarioType = newScenarioType;
			curStage = stage;
			if (stage != 2)
			{
				FK3_MessageCenter.SendMessage("StopHandleNewFish", null);
				Invoke("ClearFish", 1f);
			}
			if (stage == 1)
			{
				EnterNewScenario(newScenarioType, stageType);
			}
			if (stage == 3)
			{
				m_curScenario.BossEnter(0);
			}
		}

		private void ClearFish()
		{
			FK3_FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FK3_FishBehaviour _fish)
			{
				if (_fish.IsLive())
				{
					_fish.LeaveScene();
				}
			});
			FK3_MessageCenter.SendMessage("SetCursorUsageLinearSpeed", null);
		}

		public void EnterNewScenario(FK3_ScenarioType newScenarioType, int stageType = 2)
		{
			StartCoroutine(IE_EnterNewScenario(newScenarioType, stageType));
		}

		private IEnumerator IE_EnterNewScenario(FK3_ScenarioType newScenarioType, int stageType = 2)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			UnityEngine.Debug.Log($"from scenario[{m_curScenario.type}] to scenario[{newScenarioType}]");
			if (m_curScenario.type == newScenarioType)
			{
				yield break;
			}
			m_lastScenario = m_curScenario;
			m_curScenario = m_scenarios.Value[newScenarioType];
			if (m_lastScenario.type == FK3_ScenarioType.Boss_Lantern_暗夜炬兽)
			{
				for (int i = 0; i < m_lastScenario.GetRenderers4Fade().Length; i++)
				{
					m_lastScenario.GetRenderers4Fade()[i].transform.GetChild(0).gameObject.SetActive(value: false);
				}
			}
			yield return new WaitForSeconds(1f);
			float fadeDuration = 2f;
			Image[] renderers3 = m_lastScenario.GetRenderers4Fade();
			Image[] array = renderers3;
			foreach (Image target in array)
			{
				target.DOFade(0f, fadeDuration);
			}
			m_seaWave.SetActive();
			SetAlpha(m_seaWave, 0f);
			m_seaWave.DOFade(1f, fadeDuration);
			switch (newScenarioType)
			{
			case FK3_ScenarioType.Boss_Crab_霸王蟹:
				FK3_Effect_LogoMgr.Get().SpawnBossKrakenShadow(new Vector3(-14f, 0f, 0f));
				break;
			case FK3_ScenarioType.Boss_Lantern_暗夜炬兽:
				FK3_Effect_LogoMgr.Get().SpawnBossCrabShadow(new Vector3(-24f, 0f, 0f));
				break;
			}
			m_gameLogo.SetActive();
			SetAlpha(m_gameLogo, 0f);
			m_gameLogo.DOFade(1f, fadeDuration);
			m_lastScenario.Exit();
			if (m_lastScenario.type == FK3_ScenarioType.Boss_Lantern_暗夜炬兽)
			{
				UnityEngine.Debug.LogError("注销了暗夜巨兽灯光");
			}
			yield return new WaitForSeconds(2f);
			m_monsterLogo.gameObject.SetActive(value: false);
			if (stageType == 1)
			{
				m_monsterLogo.gameObject.SetActive(value: true);
				SetAlpha(m_monsterLogo, 1f);
				m_monsterLogo.transform.localScale = Vector3.zero;
				m_monsterLogo.transform.DOScale(1.5f, 1f).SetEase(Ease.InQuad).OnComplete(delegate
				{
					m_monsterLogo.transform.DOScale(1.2f, 0.4f).SetEase(Ease.OutQuad);
				});
			}
			yield return new WaitForSeconds(1f);
			m_lastScenario.SetActive(active: false);
			m_seaWave.SetActive(active: false);
			m_curScenario.SetActive();
			Image[] renderers2 = m_curScenario.GetRenderers4Fade();
			Image[] array2 = renderers2;
			foreach (Image image in array2)
			{
				if (m_lastScenario.type != FK3_ScenarioType.Boss_Lantern_暗夜炬兽)
				{
					UnityEngine.Debug.Log($"renderer[{image.name}]");
					SetAlpha(image, 0f);
					image.DOFade(1f, fadeDuration);
				}
				else
				{
					SetAlpha(image, 1f);
				}
			}
			m_curScenario.transform.DOScale(1.2f, 2f).SetLoops(2, LoopType.Yoyo);
			SetAlpha(m_seaWave, 1f);
			m_seaWave.DOFade(0f, fadeDuration);
			SetAlpha(m_gameLogo, 1f);
			m_gameLogo.DOFade(0f, fadeDuration);
			yield return new WaitForSeconds(fadeDuration + 0.01f);
			if (m_monsterLogo.gameObject.activeInHierarchy)
			{
				SetAlpha(m_monsterLogo, 1f);
				m_monsterLogo.DOFade(0f, 0.5f);
			}
			m_curScenario.Enter();
			m_gameLogo.SetActive(active: false);
			m_seaWave.SetActive(active: false);
			stopwatch.Stop();
			UnityEngine.Debug.Log(FK3_LogHelper.Orange($"剧情切换耗时：{stopwatch.Elapsed.TotalSeconds}"));
		}

		private void RefreshNewScenario(FK3_ScenarioType newScenarioType)
		{
			UnityEngine.Debug.Log($"from scenario[{m_curScenario.type}] to scenario[{newScenarioType}]");
			if (m_curScenario.type != newScenarioType)
			{
				m_lastScenario = m_curScenario;
				Image[] renderers4Fade = m_lastScenario.GetRenderers4Fade();
				Image[] array = renderers4Fade;
				foreach (Image renderer in array)
				{
					SetAlpha(renderer, 0f);
				}
				m_lastScenario.Exit();
				m_lastScenario.SetActive(active: false);
				m_curScenario = m_scenarios.Value[newScenarioType];
				m_curScenario.SetActive();
				Image[] renderers4Fade2 = m_curScenario.GetRenderers4Fade();
				Image[] array2 = renderers4Fade2;
				foreach (Image image in array2)
				{
					UnityEngine.Debug.Log($"renderer[{image.name}]");
					SetAlpha(image, 1f);
				}
				m_curScenario.Enter();
			}
		}

		public static void SetAlpha(SpriteRenderer renderer, float value)
		{
			Color color = renderer.color;
			color.a = value;
			renderer.color = color;
		}

		public static void SetAlpha(Image renderer, float value)
		{
			Color color = renderer.color;
			color.a = value;
			renderer.color = color;
		}
	}
}
