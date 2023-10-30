using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class EffectMgr : MonoBehaviour
	{
		private static EffectMgr s_intance;

		[SerializeField]
		private LockChainController m_prefabLockChain;

		[SerializeField]
		private GameObject[] m_bossBoomPrefabs;

		[SerializeField]
		private GameObject m_blinkCircle;

		private LockChainController[] m_lockChains = new LockChainController[4];

		public static EffectMgr Get()
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
					LockChainController lockChainController = UnityEngine.Object.Instantiate(m_prefabLockChain);
					m_lockChains[i] = lockChainController;
					lockChainController.name = m_prefabLockChain.name + "_" + (i + 1);
					lockChainController.transform.SetParent(parent);
					lockChainController.transform.localScale = Vector3.one;
					lockChainController.SetActive(active: false);
				}
			}
		}

		public LockChainController GetLockChain(int seatId)
		{
			return m_lockChains[seatId - 1];
		}

		public void ResetAllLockChains()
		{
			LockChainController[] lockChains = m_lockChains;
			foreach (LockChainController lockChainController in lockChains)
			{
				if (lockChainController != null)
				{
					lockChainController.Reset_EventHandler();
					lockChainController.Reset_Chain();
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
