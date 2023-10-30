using FullInspector;
using M__M.HaiWang.GameDefine;
using PathSystem;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FishFormation : Formation<FishType>
	{
		public int id;

		public List<RandomData> randomList;

		protected Dictionary<int, FishType> _randomMap;

		protected int _startID;

		protected override void Start()
		{
			if (_factory == null)
			{
				_factory = FishFormationFactory.Get();
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

		protected override NavPathAgent _createObject(FishType type, object userData)
		{
			return _factory.Create(type, (int)userData);
		}

		public void UpdateFormation()
		{
			_subFormations.RemoveAll((SubFormation<FishType> _sub) => _sub == null);
			_subFormations.ForEach(delegate(SubFormation<FishType> _sub)
			{
				_sub.path.UpdatePath();
			});
		}

		public void PlayFormation(int startID, List<RandomDataInput> randomInputDatas = null)
		{
			base.gameObject.SetActive(value: true);
			_startID = startID;
			int num = _startID;
			for (int i = 0; i < _subFormations.Count; i++)
			{
				FishSubFormation fishSubFormation = (FishSubFormation)_subFormations[i];
				fishSubFormation.SetStartID(num);
				num += fishSubFormation.count;
			}
			if (randomList != null)
			{
				_randomMap = new Dictionary<int, FishType>();
				foreach (RandomData randomData in randomList)
				{
					if (randomInputDatas == null)
					{
						FishType value = randomData.types[Random.Range(0, randomData.types.Count)];
						_randomMap.Add(randomData.randID, value);
					}
					else
					{
						RandomDataInput randomDataInput = randomInputDatas.Find((RandomDataInput item) => item.randID == randomData.randID);
						_randomMap.Add(randomData.randID, randomDataInput.type);
					}
				}
			}
			Play();
		}

		internal FishType GetRandomType(int randID)
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
			else if (base.state == FormationState.Playing)
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
			if (base.state == FormationState.Playing)
			{
				Stop();
			}
		}

		[InspectorButton]
		[InspectorName("Play Formation")]
		private void _playFormation()
		{
			if (!Application.isPlaying)
			{
				UnityEngine.Debug.Log("Game not in playing state!");
			}
			else if (base.state != FormationState.Playing)
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
