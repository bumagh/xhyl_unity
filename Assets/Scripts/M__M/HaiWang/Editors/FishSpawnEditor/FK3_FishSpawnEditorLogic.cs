using FullInspector;
using M__M.HaiWang.Fish;
using M__M.HaiWang.StoryDefine;
using PathSystem;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Editors.FishSpawnEditor
{
	public class FK3_FishSpawnEditorLogic : BaseBehavior<FullSerializerSerializer>
	{
		[InspectorOrder(10.0)]
		[SerializeField]
		private FK3_GameStoryTimelineData m_storyData;

		public bool forbiddenPlayOnStart;

		[InspectorOrder(4.0)]
		public int storyId;

		private int fishStartId;

		protected override void Awake()
		{
			base.Awake();
			FK3_Formation<FK3_FishType>.forbiddenPlayOnStart = forbiddenPlayOnStart;
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
			FK3_FishFormationFactory.Get().createFun = delegate(FK3_FishType _fishType, int id)
			{
				int num = FK3_FishMgr.Get().IncreaseFishIndex();
				FK3_FishCreationInfo info = new FK3_FishCreationInfo(_fishType);
				FK3_FishBehaviour fish = FK3_FishMgr.Get().GenNewFish(info);
				fish.SetPosition(new Vector3(-3f, 0f));
				FK3_NavPathAgent fK3_NavPathAgent = fish.gameObject.GetComponent<FK3_NavPathAgent>();
				if (fK3_NavPathAgent == null)
				{
					fK3_NavPathAgent = fish.gameObject.AddComponent<FK3_NavPathAgent>();
				}
				else
				{
					fK3_NavPathAgent.enabled = true;
				}
				FK3_FishBehaviour fK3_FishBehaviour = fish;
				fK3_FishBehaviour.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
				{
					FK3_AgentData<FK3_FishType> fK3_AgentData = fish.GetComponent<FK3_NavPathAgent>().userData as FK3_AgentData<FK3_FishType>;
					fK3_AgentData?.formation.RemoveObject(fK3_AgentData.agent);
				});
				return fK3_NavPathAgent;
			};
			FK3_FishFormationFactory.Get().destoryAction = delegate(FK3_NavPathAgent _navPathAgent, FK3_FishType _fishType)
			{
				FK3_FishBehaviour component = _navPathAgent.GetComponent<FK3_FishBehaviour>();
				if (component.State == FK3_FishState.Live)
				{
					component.Die();
					_navPathAgent.enabled = false;
				}
			};
		}

		[InspectorOrder(3.0)]
		[InspectorButton]
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
			foreach (FK3_StoryItem story in m_storyData.storys)
			{
				StartCoroutine(IE_PlayStory(story));
				yield return new WaitForSeconds(story.duration);
			}
		}

		private IEnumerator IE_PlayStory(int storyId)
		{
			yield return null;
			bool found = false;
			foreach (FK3_StoryItem story in m_storyData.storys)
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

		private IEnumerator IE_PlayStory(FK3_StoryItem story)
		{
			float beginTime = Time.realtimeSinceStartup;
			UnityEngine.Debug.Log(FK3_LogHelper.Magenta($"story[{story.id}] begin @{Time.realtimeSinceStartup}, will takes {story.duration}s"));
			foreach (FK3_EventItem @event in story.events)
			{
				StartCoroutine(IE_PlayEvent(story, @event));
			}
			yield return new WaitForSeconds(story.duration);
			UnityEngine.Debug.Log(FK3_LogHelper.Magenta($"story[{story.id}] end @{Time.realtimeSinceStartup}, takes {Time.realtimeSinceStartup - beginTime}s"));
		}

		private IEnumerator IE_PlayEvent(FK3_StoryItem story, FK3_EventItem eventItem)
		{
			yield return new WaitForSeconds(eventItem.delay);
			float chance2 = 0f;
			float rnd = UnityEngine.Random.Range(0f, 1f);
			bool hit = false;
			bool debugHitDetialInfo = true;
			bool hitFish = false;
			foreach (FK3_FishItem fish in eventItem.fishList)
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
			foreach (FK3_GroupItem group in eventItem.groupList)
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
			foreach (FK3_FormationItem formation in eventItem.formationList)
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
			FK3_FishFormation formationById = FK3_FishFormationMgr.Get().GetFormationById(formationId);
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
			FK3_FishMgr.Get().GetAllLiveFishList().ForEach(delegate(FK3_FishBehaviour _fish)
			{
				_fish.Die();
			});
		}
	}
}
