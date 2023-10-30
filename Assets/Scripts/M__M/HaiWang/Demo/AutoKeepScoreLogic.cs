using FullInspector;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.UIDefine;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Demo
{
	[fiInspectorOnly]
	public class AutoKeepScoreLogic : MonoBehaviour
	{
		[ShowInInspector]
		private InGameUIContext m_uiContext;

		[ShowInInspector]
		private GameContext m_context;

		public int minScore = 10000;

		[NotSerialized]
		public int count = 3;

		private Coroutine co_auto;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		public void Init(InGameUIContext uiContext, GameContext context)
		{
			m_uiContext = uiContext;
			m_context = context;
		}

		public bool CheckValid()
		{
			return m_uiContext != null && m_context != null;
		}

		private void OnEnable()
		{
			StartCoroutine(IE_Monitor());
		}

		private void OnDisable()
		{
			co_auto = null;
		}

		private IEnumerator IE_Monitor()
		{
			yield return null;
			if (!CheckValid())
			{
				UnityEngine.Debug.LogError("check result is invalid");
				yield break;
			}
			while (true)
			{
				WaitForSeconds delay = new WaitForSeconds(0.1f);
				if (m_uiContext.curScore < m_context.curDesk.onceExchangeValue * 5 && co_auto == null)
				{
					co_auto = StartCoroutine(IE_Auto(count));
				}
				yield return delay;
			}
		}

		private IEnumerator IE_Auto(int count)
		{
			UnityEngine.Debug.Log($"IE_Auto begin.count:[{count}]");
			while (!m_context.isPlaying)
			{
				yield return null;
			}
			SimpleSingletonBehaviour<OptionController>.Get().Show_InOutPanel();
			yield return new WaitForSeconds(0.3f);
			if (HW2_MB_Singleton<HW2_NetManager>.Get().isReady)
			{
				int num = 400000 / m_context.curDesk.exchange;
				object[] args = new object[1]
				{
					num
				};
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/userCoinIn", args);
			}
			yield return new WaitUntil(() => m_uiContext.curScore > m_context.curDesk.onceExchangeValue * 10);
			UnityEngine.Debug.Log("IE_Auto finish");
			Clean_Auto();
			GunBehaviour nativeGun = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			if (nativeGun != null)
			{
				nativeGun.DoAuto();
			}
		}

		private void Clean_Auto()
		{
			if (SimpleSingletonBehaviour<SaveAndTakeScores>.Get().isActiveAndEnabled)
			{
				SimpleSingletonBehaviour<SaveAndTakeScores>.Get().HideInOutPanel();
			}
			co_auto = null;
		}
	}
}
