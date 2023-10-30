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
public class StoryScenarioMgr : fiSimpleSingletonBehaviour<StoryScenarioMgr>
{
	[SerializeField]
	private ScenarioBehaviourDictionary m_scenarios = new ScenarioBehaviourDictionary();

	[SerializeField]
	private Image m_seaWave;

	[SerializeField]
	private GameObject m_pointLight;

	[SerializeField]
	private Image m_gameLogo;

	[SerializeField]
	private Image m_monsterLogo;

	private StoryScenarioBehaviour m_lastScenario;

	private StoryScenarioBehaviour m_curScenario;

	public Transform krakenBornPosList;

	private ScenarioType curScenarioType;

	private int curStage;

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

	public ScenarioType CurScenarioType
	{
		get
		{
			return curScenarioType;
		}
	}

	public int CurStage
	{
		get
		{
			return curStage;
		}
	}

	protected override void Awake()
	{
		UnityEngine.Debug.Log("StoryScenarioMgr.Awake");
		fiSimpleSingletonBehaviour<StoryScenarioMgr>.s_instance = this;
	}

	public void Init(bool faceDown)
	{
		UnityEngine.Debug.Log("m_scenarios为空? " + (m_scenarios == null));
		UnityEngine.Debug.Log("StoryScenario/LogoGame: " + base.transform.Find("StoryScenario/LogoGame"));
		UnityEngine.Debug.Log("StoryScenario/LogoBossArr: " + base.transform.Find("StoryScenario/LogoBossArr"));
		foreach (KeyValuePair<ScenarioType, StoryScenarioBehaviour> item in m_scenarios.Value)
		{
			UnityEngine.Debug.Log(string.Format("key:{0}, value:{1}", item.Key, item.Value));
			if (item.Value == null)
			{
				UnityEngine.Debug.Log("sceneario empty");
				continue;
			}
			Image[] renderers4Fade = item.Value.GetRenderers4Fade();
			if (renderers4Fade == null)
			{
				UnityEngine.Debug.Log("renderers empty");
				continue;
			}
			for (int i = 0; i < renderers4Fade.Length; i++)
			{
				Image image = renderers4Fade[i];
				if (image == null)
				{
					UnityEngine.Debug.Log("empty:" + i);
					continue;
				}
				UnityEngine.Debug.Log("renderer: " + image.name);
				SetAlpha(image, 1f);
			}
			G.SetActive(item.Value, false);
		}
		base.transform.Find("StoryScenario/LogoGame").localRotation = Quaternion.Euler(0f, 0f, 0f);
		base.transform.Find("StoryScenario/LogoBossArr").localRotation = Quaternion.Euler(0f, 0f, 0f);
	}

	public void DieFish()
	{
		StartCoroutine(WaitDieFish());
	}

	private IEnumerator WaitDieFish()
	{
		yield return new WaitForSeconds(0.5f);
		try
		{
			for (int i = 0; i < FishMgr.Get().GetAllLiveFishList().Count; i++)
			{
				if (FishMgr.Get().GetAllLiveFishList()[i].State != FishState.Dead)
				{
					FishMgr.Get().GetAllLiveFishList()[i].State = FishState.Dead;
				}
			}
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		FishMgr.Get().KillFish();
	}

	public void ShakeScreen(int type)
	{
		if (type == 0)
		{
			TweenSettingsExtensions.OnComplete(ShortcutExtensions.DOShakePosition(base.transform, 2.8f, 0.15f), delegate
			{
				ShortcutExtensions.DOLocalMove(base.transform, Vector3.zero, 1.2f);
			});
		}
		if (type == 1)
		{
			TweenSettingsExtensions.OnComplete(ShortcutExtensions.DOShakePosition(base.transform, 1.2f, 0.1f), delegate
			{
				ShortcutExtensions.DOLocalMove(base.transform, Vector3.zero, 0.6f);
			});
		}
	}

	//TODO 没有被调用到且有错，所以注释掉
	// private unsafe void PointLightControl()
	// {
	// 	m_pointLight.transform.localPosition = pointLightPoint[0];
	// 	m_pointLight.SetActive(true);
	// 	DOTween.To(new DOGetter<Color>(null, (IntPtr)(void*)(ulong)(UIntPtr/*delegate*<Color>*/)(&RenderSettings.get_ambientLight)), delegate(Color value)
	// 	{
	// 		RenderSettings.ambientLight = value;
	// 	}, new Color(0.1f, 0.1f, 0.1f, 0.1f), 1f);
	// 	TweenSettingsExtensions.OnComplete(TweenSettingsExtensions.SetEase(ShortcutExtensions.DOLocalPath(m_pointLight.transform, pointLightPoint, 5f, PathType.CatmullRom), Ease.Linear), delegate
	// 	{
	// 		UnityEngine.Debug.Log(HW2_LogHelper.Brown("pointLight.transform.DOLocalPath OnComplete"));
	// 		DOTween.Kill(m_pointLight.transform);
	// 		m_pointLight.SetActive(false);
	// 		m_pointLight.transform.localPosition = new Vector3(0f, 7f, 0f);
	// 		DOTween.To(new DOGetter<Color>(null, (IntPtr)(void*)(ulong)(UIntPtr/*delegate*<Color>*/)(&RenderSettings.get_ambientLight)), delegate(Color value)
	// 		{
	// 			RenderSettings.ambientLight = value;
	// 		}, Color.white, 1f);
	// 	});
	// }

	public void ResetScenario()
	{
		m_curScenario.Exit();
	}

	public void SetStage(ScenarioType scenarioType, int stage, int stageType, int sceneDuration, int stageDuration)
	{
		UnityEngine.Debug.Log(HW2_LogHelper.Yellow(string.Format("sceneDuration:{0},stageDuration:{1},", sceneDuration, stageDuration)));
		if (stage == 3)
		{
			int num = sceneDuration - stageDuration;
			UnityEngine.Debug.Log(HW2_LogHelper.Brown(string.Format("stageDuration:{0},dur:{1},", stageDuration, num)));
			if (scenarioType == ScenarioType.Boss_Kraken_深海八爪鱼)
			{
				m_curScenario.ScenarioProgress(num);
				m_curScenario.ScenarioRotate(stageDuration);
			}
			if (scenarioType == ScenarioType.Boss_Lantern_暗夜炬兽)
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
		UnityEngine.Debug.Log(string.Format("Boss Coming! [sceneario:{0},stage:{1},stageType:{2}]", m_curScenario.type, stage, stageType));
		if (stage == 3)
		{
			m_curScenario.BossEnter(0);
		}
		else
		{
			m_curScenario.Enter();
		}
	}

	public void SetScenario(ScenarioType scenarioType)
	{
		try
		{
			StoryScenarioBehaviour storyScenarioBehaviour = (m_curScenario = m_scenarios.Value[scenarioType]);
			G.SetActive(m_curScenario);
			m_curScenario.Start();
			G.SetActive(m_gameLogo, false);
			G.SetActive(m_seaWave, false);
		}
		catch (Exception ex)
		{
			MonoBehaviour.print("错误: " + ex);
		}
	}

	public void RefreshScenario(ScenarioType newScenarioType, int stage, int stageType)
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

	public void ScenarioLogic(ScenarioType newScenarioType, int stage, int stageType)
	{
		curScenarioType = newScenarioType;
		curStage = stage;
		if (stage != 2)
		{
			HW2_MessageCenter.SendMessage("StopHandleNewFish", null);
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
		FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FishBehaviour _fish)
		{
			if (_fish.IsLive())
			{
				_fish.LeaveScene();
			}
		});
		HW2_MessageCenter.SendMessage("SetCursorUsageLinearSpeed", null);
	}

	public void EnterNewScenario(ScenarioType newScenarioType, int stageType = 2)
	{
		StartCoroutine(IE_EnterNewScenario(newScenarioType, stageType));
	}

	private IEnumerator IE_EnterNewScenario(ScenarioType newScenarioType, int stageType = 2)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		UnityEngine.Debug.Log(string.Format("from scenario[{0}] to scenario[{1}]", m_curScenario.type, newScenarioType));
		if (m_curScenario.type == newScenarioType)
		{
			yield break;
		}
		m_lastScenario = m_curScenario;
		m_curScenario = m_scenarios.Value[newScenarioType];
		if (m_lastScenario.type == ScenarioType.Boss_Lantern_暗夜炬兽)
		{
			for (int i = 0; i < m_lastScenario.GetRenderers4Fade().Length; i++)
			{
				m_lastScenario.GetRenderers4Fade()[i].transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		yield return new WaitForSeconds(1f);
		float fadeDuration = 2f;
		Image[] renderers = m_lastScenario.GetRenderers4Fade();
		Image[] array = renderers;
		foreach (Image target in array)
		{
			ShortcutExtensions46.DOFade(target, 0f, fadeDuration);
		}
		G.SetActive(m_seaWave);
		SetAlpha(m_seaWave, 0f);
		ShortcutExtensions46.DOFade(m_seaWave, 1f, fadeDuration);
		switch (newScenarioType)
		{
		case ScenarioType.Boss_Crab_霸王蟹:
			Effect_LogoMgr.Get().SpawnBossKrakenShadow(new Vector3(-14f, 0f, 0f));
			break;
		case ScenarioType.Boss_Lantern_暗夜炬兽:
			Effect_LogoMgr.Get().SpawnBossCrabShadow(new Vector3(-24f, 0f, 0f));
			break;
		}
		G.SetActive(m_gameLogo);
		SetAlpha(m_gameLogo, 0f);
		ShortcutExtensions46.DOFade(m_gameLogo, 1f, fadeDuration);
		m_lastScenario.Exit();
		if (m_lastScenario.type == ScenarioType.Boss_Lantern_暗夜炬兽)
		{
			UnityEngine.Debug.LogError("注销了暗夜巨兽灯光");
		}
		yield return new WaitForSeconds(2f);
		m_monsterLogo.gameObject.SetActive(false);
		if (stageType == 1)
		{
			m_monsterLogo.gameObject.SetActive(true);
			SetAlpha(m_monsterLogo, 1f);
			m_monsterLogo.transform.localScale = Vector3.zero;
			TweenSettingsExtensions.OnComplete(TweenSettingsExtensions.SetEase(ShortcutExtensions.DOScale(m_monsterLogo.transform, 1.5f, 1f), Ease.InQuad), delegate
			{
				TweenSettingsExtensions.SetEase(ShortcutExtensions.DOScale(m_monsterLogo.transform, 1.2f, 0.4f), Ease.OutQuad);
			});
		}
		yield return new WaitForSeconds(1f);
		G.SetActive(m_lastScenario, false);
		G.SetActive(m_seaWave, false);
		G.SetActive(m_curScenario);
		Image[] renderers2 = m_curScenario.GetRenderers4Fade();
		Image[] array2 = renderers2;
		foreach (Image image in array2)
		{
			if (m_lastScenario.type != ScenarioType.Boss_Lantern_暗夜炬兽)
			{
				UnityEngine.Debug.Log(string.Format("renderer[{0}]", image.name));
				SetAlpha(image, 0f);
				ShortcutExtensions46.DOFade(image, 1f, fadeDuration);
			}
			else
			{
				SetAlpha(image, 1f);
			}
		}
		TweenSettingsExtensions.SetLoops(ShortcutExtensions.DOScale(m_curScenario.transform, 1.2f, 2f), 2, LoopType.Yoyo);
		SetAlpha(m_seaWave, 1f);
		ShortcutExtensions46.DOFade(m_seaWave, 0f, fadeDuration);
		SetAlpha(m_gameLogo, 1f);
		ShortcutExtensions46.DOFade(m_gameLogo, 0f, fadeDuration);
		yield return new WaitForSeconds(fadeDuration + 0.01f);
		if (m_monsterLogo.gameObject.activeInHierarchy)
		{
			SetAlpha(m_monsterLogo, 1f);
			ShortcutExtensions46.DOFade(m_monsterLogo, 0f, 0.5f);
		}
		m_curScenario.Enter();
		G.SetActive(m_gameLogo, false);
		G.SetActive(m_seaWave, false);
		stopwatch.Stop();
		UnityEngine.Debug.Log(HW2_LogHelper.Orange(string.Format("剧情切换耗时：{0}", stopwatch.Elapsed.TotalSeconds)));
	}

	private void RefreshNewScenario(ScenarioType newScenarioType)
	{
		UnityEngine.Debug.Log(string.Format("from scenario[{0}] to scenario[{1}]", m_curScenario.type, newScenarioType));
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
			G.SetActive(m_lastScenario, false);
			m_curScenario = m_scenarios.Value[newScenarioType];
			G.SetActive(m_curScenario);
			Image[] renderers4Fade2 = m_curScenario.GetRenderers4Fade();
			Image[] array2 = renderers4Fade2;
			foreach (Image image in array2)
			{
				UnityEngine.Debug.Log(string.Format("renderer[{0}]", image.name));
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
