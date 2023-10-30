using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class FK3_EffectMgr : MonoBehaviour
	{
		private static FK3_EffectMgr s_intance;

		[SerializeField]
		private FK3_LockChainController m_prefabLockChain;

		[SerializeField]
		private GameObject[] m_bossBoomPrefabs;

		[SerializeField]
		private GameObject m_blinkCircle;

		private FK3_LockChainController[] m_lockChains = new FK3_LockChainController[4];

		public static FK3_EffectMgr Get()
		{
			return s_intance;
		}

		private void Awake()
		{
			s_intance = this;
			Prepare_LockChain();
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void Prepare_LockChain()
		{
			if (!(m_prefabLockChain == null))
			{
				Transform parent = base.transform.Find("Bullet_LockChains");
				for (int i = 0; i < 4; i++)
				{
					FK3_LockChainController fK3_LockChainController = UnityEngine.Object.Instantiate(m_prefabLockChain);
					m_lockChains[i] = fK3_LockChainController;
					fK3_LockChainController.name = m_prefabLockChain.name + "_" + (i + 1);
					fK3_LockChainController.transform.SetParent(parent);
					fK3_LockChainController.transform.localScale = Vector3.one;
					fK3_LockChainController.gameObject.SetActive(value: false);
				}
			}
		}

		public FK3_LockChainController GetLockChain(int seatId)
		{
			return m_lockChains[seatId - 1];
		}

		public void ResetAllLockChains()
		{
			FK3_LockChainController[] lockChains = m_lockChains;
			foreach (FK3_LockChainController fK3_LockChainController in lockChains)
			{
				if (fK3_LockChainController != null)
				{
					fK3_LockChainController.Reset_EventHandler();
					fK3_LockChainController.Reset_Chain();
				}
			}
		}

		public void PlayBossBoom1(Vector3 pos, int type)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(m_bossBoomPrefabs[type]);
			gameObject.transform.position = pos;
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localScale = Vector3.one;
			UnityEngine.Object.Destroy(gameObject, 5f);
		}

		public void PlayBlinkCirle(Transform tr, int index)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(m_blinkCircle);
			gameObject.SetActive(value: true);
			gameObject.transform.parent = tr;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			UnityEngine.Object.Destroy(gameObject, 2f);
		}
	}
}
