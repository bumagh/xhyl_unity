using M__M.HaiWang.Fish;
using System.Linq;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class TestEnumShowCase_Logic : MonoBehaviour
	{
		[SerializeField]
		private TestEnumShowCase_UI m_ui;

		private XSelector<EnumFoo> selector;

		[SerializeField]
		private Fish_Selector_Context context;

		private void Start()
		{
			UnityEngine.Debug.Log("TestEnumShowCase_Logic start");
			Init();
			m_ui.view_test_selector.leftBtn.SetAction(OnBtnLeft_Click);
			m_ui.view_test_selector.rightBtn.SetAction(OnBtnRight_Click);
			m_ui.view_test_selector.analyzeBtn.SetAction(OnBtnAnalyze_Click);
		}

		private void Init()
		{
			FishType at = context.selector.GetAt(0);
			SetFish(at);
		}

		private void OnBtnLeft_Click()
		{
			UnityEngine.Debug.Log("OnBtnLeft_Click");
			FishType prev = context.selector.GetPrev();
			SetFish(prev);
		}

		private void OnBtnRight_Click()
		{
			UnityEngine.Debug.Log("OnBtnRight_Click");
			FishType next = context.selector.GetNext();
			SetFish(next);
		}

		private void OnBtnAnalyze_Click()
		{
			FishBehaviour component = context.curFish.GetComponent<FishBehaviour>();
			Analyze_Fish_Animator(component);
		}

		private void SetFish(FishType fishType)
		{
			if (context.curFish != null)
			{
				FishBehaviour component = context.curFish.GetComponent<FishBehaviour>();
				component.Die();
			}
			FishBehaviour fishBehaviour = FishMgr.Get().SpawnSingleFish(fishType);
			context.curFish = fishBehaviour.transform;
			m_ui.view_test_selector.fishName = fishType.ToString();
			Analyze_Fish_Animator(fishBehaviour);
		}

		private void Update()
		{
		}

		private void Test_EnumFoo()
		{
			EnumFoo enumFoo = EnumFoo.AAA;
			UnityEngine.Debug.Log(enumFoo);
		}

		private void Analyze_Fish_Animator(FishBehaviour fish)
		{
			Animator animator = fish.animator;
			string[] clips = (from x in animator.runtimeAnimatorController.animationClips
				select x.name).ToArray();
			string[] parameters = (from x in animator.parameters
				select x.name).ToArray();
			UnityEngine.Debug.Log($"parameterCount:[{animator.parameterCount}]");
			UnityEngine.Debug.Log(string.Format("parameters:[{0}]", string.Join(",", parameters)));
			UnityEngine.Debug.Log(string.Format("clips:[{0}]", string.Join(",", clips)));
			m_ui.view_test_selector.clips = clips;
			m_ui.view_test_selector.parameters = parameters;
			m_ui.view_test_selector.onClipClick = delegate(int index)
			{
				UnityEngine.Debug.Log("onClipClick: " + index);
				animator.Play(clips[index]);
			};
			m_ui.view_test_selector.onParameterClick = delegate(int index)
			{
				UnityEngine.Debug.Log("onParameterClick: " + index);
				animator.SetTrigger(parameters[index]);
			};
		}
	}
}
