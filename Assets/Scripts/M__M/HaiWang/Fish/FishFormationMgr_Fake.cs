using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FishFormationMgr_Fake : MonoBehaviour
	{
		[SerializeField]
		private Dictionary<int, FishFormation> _formationMap;

		private static FishFormationMgr_Fake s_instance;

		public static FishFormationMgr_Fake Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
			_formationMap = new Dictionary<int, FishFormation>();
			int childCount = base.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				FishFormation component = child.GetComponent<FishFormation>();
				if (component == null)
				{
					UnityEngine.Debug.LogWarning("Please add a FishFormation script to GameObject " + child.name);
				}
				else if (_formationMap.ContainsKey(component.id))
				{
					UnityEngine.Debug.LogWarning("Formation ID " + component.id + " duplicated!");
				}
				else
				{
					_formationMap.Add(component.id, component);
				}
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public FishFormation GetFormationById(int id)
		{
			FishFormation value = null;
			if (_formationMap.TryGetValue(id, out value))
			{
				return value;
			}
			UnityEngine.Debug.Log($"GetFormationById. [id:{id}] not found");
			return null;
		}
	}
}
