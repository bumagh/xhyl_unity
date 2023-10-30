using FullInspector;
using M__M.HaiWang.GameDefine;
using PathSystem;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishFormation : FK3_Formation<FK3_FishType>
	{
		public int id;

		public List<FK3_RandomData> randomList;

		protected Dictionary<int, FK3_FishType> _randomMap;

		protected int _startID;

		protected override void Start()
		{
			if (_factory == null)
			{
				_factory = FK3_FishFormationFactory.Get();
			}
			base.OnComplete += delegate
			{
				base.gameObject.SetActive(value: false);
			};
			if (PlayOnStart)
			{
				PlayFormation(0);
			}
		}

		protected override FK3_NavPathAgent _createObject(FK3_FishType type, object userData)
		{
			return _factory.Create(type, (int)userData);
		}

		public void UpdateFormation()
		{
			_subFormations.RemoveAll((FK3_SubFormation<FK3_FishType> _sub) => _sub == null);
			_subFormations.ForEach(delegate(FK3_SubFormation<FK3_FishType> _sub)
			{
				_sub.path.UpdatePath();
			});
		}

		public void PlayFormation(int startID, List<FK3_RandomDataInput> randomInputDatas = null)
		{
			base.gameObject.SetActive(value: true);
			_startID = startID;
			int num = _startID;
			for (int i = 0; i < _subFormations.Count; i++)
			{
				FK3_FishSubFormation fK3_FishSubFormation = (FK3_FishSubFormation)_subFormations[i];
				fK3_FishSubFormation.SetStartID(num);
				num += fK3_FishSubFormation.count;
			}
			if (randomList != null)
			{
				_randomMap = new Dictionary<int, FK3_FishType>();
				foreach (FK3_RandomData randomData in randomList)
				{
					if (randomInputDatas == null)
					{
						FK3_FishType value = randomData.types[Random.Range(0, randomData.types.Count)];
						_randomMap.Add(randomData.randID, value);
					}
					else
					{
						FK3_RandomDataInput fK3_RandomDataInput = randomInputDatas.Find((FK3_RandomDataInput item) => item.randID == randomData.randID);
						_randomMap.Add(randomData.randID, fK3_RandomDataInput.type);
					}
				}
			}
			Play();
		}

		internal FK3_FishType GetRandomType(int randID)
		{
			return _randomMap[randID];
		}

		[InspectorName("Stop Formation")]
		[InspectorButton]
		private void _stopFormation()
		{
			if (!Application.isPlaying)
			{
				UnityEngine.Debug.Log("Game not in playing state!");
			}
			else if (base.state == FK3_FormationState.Playing)
			{
				Stop();
			}
			else
			{
				UnityEngine.Debug.Log("Formation is not in playing, please play it first!");
			}
		}

		public void ResetFormation()
		{
			if (base.state == FK3_FormationState.Playing)
			{
				Stop();
			}
		}

		[InspectorName("Play Formation")]
		[InspectorButton]
		private void _playFormation()
		{
			if (!Application.isPlaying)
			{
				UnityEngine.Debug.Log("Game not in playing state!");
			}
			else if (base.state != FK3_FormationState.Playing)
			{
				PlayFormation(0);
			}
			else
			{
				UnityEngine.Debug.Log("Formation is in playing, please try it later!");
			}
		}
	}
}
