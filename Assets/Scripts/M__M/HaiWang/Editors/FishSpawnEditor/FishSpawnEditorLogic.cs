using FullInspector;
using M__M.HaiWang.Fish;
using M__M.HaiWang.StoryDefine;
using PathSystem;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Editors.FishSpawnEditor
{
	public class FishSpawnEditorLogic : BaseBehavior<FullSerializerSerializer>
	{
		[SerializeField]
		[InspectorOrder(10.0)]
		private GameStoryTimelineData m_storyData;

		public bool forbiddenPlayOnStart;

		[InspectorOrder(4.0)]
		public int storyId;

		private int fishStartId;

		protected override void Awake()
		{
			base.Awake();
			Formation<FishType>.forbiddenPlayOnStart = forbiddenPlayOnStart;
		}

		private void Start()
		{
			PreInit_Formation();
		}

		private void Update()
		{
		}

		private void PreInit_Formation()
		{
			FishFormationFactory.Get().createFun = delegate(FishType _fishType, int id)
			{
				int num = FishMgr.Get().IncreaseFishIndex();
				FishCreationInfo info = new FishCreationInfo(_fishType);
				FishBehaviour fish = FishMgr.Get().GenNewFish(info);
				fish.SetPosition(new Vector3(-3f, 0f));
				NavPathAgent navPathAgent = fish.gameObject.GetComponent<NavPathAgent>();
				if (navPathAgent == null)
				{
					navPathAgent = fish.gameObject.AddComponent<NavPathAgent>();
				}
				else
				{
					navPathAgent.enabled = true;
				}
				FishBehaviour fishBehaviour = fish;
				fishBehaviour.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
				{
					AgentData<FishType> agentData = fish.GetComponent<NavPathAgent>().userData as AgentData<FishType>;
					agentData?.formation.RemoveObject(agentData.agent);
				});
				return navPathAgent;
			};
			FishFormationFactory.Get().destoryAction = delegate(NavPathAgent _navPathAgent, FishType _fishType)
			{
				FishBehaviour component = _navPathAgent.GetComponent<FishBehaviour>();
				if (component.State == FishState.Live)
				{
					component.Die();
					_navPathAgent.enabled = false;
				}
			};
		}

		[InspectorButton]
		[InspectorOrder(3.0)]
		private void PlayAllStories()
		{
			StopAllCoroutines();
			ClearAllFishes();
			StartCoroutine(IE_PlayAllStories());
		}

		[InspectorOrder(5.0)]
		[InspectorButton]
		private void PlayStory()
		{
			StopAllCoroutines();
			ClearAllFishes();
			StartCoroutine(IE_PlayStory(storyId));
		}

		private IEnumerator IE_PlayAllStories()
		{
			yield return null;
			foreach (StoryItem story in m_storyData.storys)
			{
				StartCoroutine(IE_PlayStory(story));
				yield return new WaitForSeconds(story.duration);
			}
		}

		private IEnumerator IE_PlayStory(int storyId)
		{
			yield return null;
			bool found = false;
			foreach (StoryItem story in m_storyData.storys)
			{
				if (story.id == storyId)
				{
					StartCoroutine(IE_PlayStory(story));
					found = true;
					break;
				}
			}
			if (!found)
			{
				UnityEngine.Debug.LogError($"Story[id:{storyId}] cannot be found");
			}
		}

		private IEnumerator IE_PlayStory(StoryItem story)
		{
			float beginTime = Time.realtimeSinceStartup;
			UnityEngine.Debug.Log(HW2_LogHelper.Magenta($"story[{story.id}] begin @{Time.realtimeSinceStartup}, will takes {story.duration}s"));
			foreach (EventItem @event in story.events)
			{
				StartCoroutine(IE_PlayEvent(story, @event));
			}
			yield return new WaitForSeconds(story.duration);
			UnityEngine.Debug.Log(HW2_LogHelper.Magenta($"story[{story.id}] end @{Time.realtimeSinceStartup}, takes {Time.realtimeSinceStartup - beginTime}s"));
		}

		private IEnumerator IE_PlayEvent(StoryItem story, EventItem eventItem)
		{
			yield return new WaitForSeconds(eventItem.delay);
			float chance2 = 0f;
			float rnd = UnityEngine.Random.Range(0f, 1f);
			bool hit = false;
			bool debugHitDetialInfo = true;
			bool hitFish = false;
			foreach (FishItem fish in eventItem.fishList)
			{
				chance2 += fish.chance;
				if (rnd < chance2)
				{
					UnityEngine.Debug.Log($"Story[{story.id}].Event[{eventItem.id}]:> Execute fishAction[fishType: {fish.fishType}, fishId: {fish.pathId}]");
					hit = true;
					hitFish = true;
					break;
				}
			}
			if (eventItem.fishList.Count > 0 && !hitFish && debugHitDetialInfo)
			{
				UnityEngine.Debug.Log($"Story[{story.id}].Event[{eventItem.id}]:> Execute fishAction failed");
			}
			bool hitGroup = false;
			chance2 = 0f;
			foreach (GroupItem group in eventItem.groupList)
			{
				chance2 += group.chance;
				if (rnd < chance2)
				{
					UnityEngine.Debug.Log($"Story[{story.id}].Event[{eventItem.id}]:> Execute groupAction[id: {group.id}]");
					hit = true;
					hitGroup = true;
				}
			}
			if (eventItem.groupList.Count > 0 && !hitGroup && debugHitDetialInfo)
			{
				UnityEngine.Debug.Log($"Story[{story.id}].Event[{eventItem.id}]:> Execute groupAction failed");
			}
			bool hitFormation = false;
			chance2 = 0f;
			foreach (FormationItem formation in eventItem.formationList)
			{
				chance2 += formation.chance;
				if (rnd < chance2)
				{
					if (debugHitDetialInfo)
					{
						UnityEngine.Debug.Log($"Story[{story.id}].Event[{eventItem.id}]:> Execute formationAction[id: {formation.id}]");
					}
					hit = true;
					hitFormation = true;
					DoFishFormation(formation.id);
				}
			}
			if (eventItem.formationList.Count > 0 && !hitFormation && debugHitDetialInfo)
			{
				UnityEngine.Debug.Log($"Story[{story.id}].Event[{eventItem.id}]:> Execute formationAction failed");
			}
			if (!hit)
			{
				UnityEngine.Debug.LogError($"Story[{story.id}].Event[{eventItem.id}]:> Execute failed!");
			}
		}

		private void DoFishFormation(int formationId)
		{
			UnityEngine.Debug.Log($"DoFishFormation id:[{formationId}]");
			FishFormation formationById = FishFormationMgr.Get().GetFormationById(formationId);
			if (formationById == null)
			{
				UnityEngine.Debug.LogError($"Formation[id:{formationId}] cannot be found");
				return;
			}
			fishStartId += 1000;
			fishStartId %= 100000;
			formationById.PlayFormation(fishStartId);
		}

		private void ClearAllFishes()
		{
			FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FishBehaviour _fish)
			{
				_fish.Die();
			});
		}
	}
}
