using M__M.HaiWang.Fish;
using System.Linq;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class FK3_TestEnumShowCase_Logic : MonoBehaviour
	{
		[SerializeField]
		private FK3_TestEnumShowCase_UI m_ui;

		private FK3_XSelector<FK3_EnumFoo> selector;

		[SerializeField]
		private FK3_Fish_Selector_Context context;

		private void Start()
		{
			UnityEngine.Debug.Log("FK3_TestEnumShowCase_Logic start");
			Init();
			m_ui.view_test_selector.leftBtn.SetAction(OnBtnLeft_Click);
			m_ui.view_test_selector.rightBtn.SetAction(OnBtnRight_Click);
			m_ui.view_test_selector.analyzeBtn.SetAction(OnBtnAnalyze_Click);
		}

		private void Init()
		{
			FK3_FishType at = context.selector.GetAt(0);
			SetFish(at);
		}

		private void OnBtnLeft_Click()
		{
			UnityEngine.Debug.Log("OnBtnLeft_Click");
			FK3_FishType prev = context.selector.GetPrev();
			SetFish(prev);
		}

		private void OnBtnRight_Click()
		{
			UnityEngine.Debug.Log("OnBtnRight_Click");
			FK3_FishType next = context.selector.GetNext();
			SetFish(next);
		}

		private void OnBtnAnalyze_Click()
		{
			FK3_FishBehaviour component = context.curFish.GetComponent<FK3_FishBehaviour>();
			Analyze_Fish_Animator(component);
		}

		private void SetFish(FK3_FishType fishType)
		{
			if (context.curFish != null)
			{
				FK3_FishBehaviour component = context.curFish.GetComponent<FK3_FishBehaviour>();
				component.Die();
			}
			FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().SpawnSingleFish(fishType);
			context.curFish = fK3_FishBehaviour.transform;
			m_ui.view_test_selector.fishName = fishType.ToString();
			Analyze_Fish_Animator(fK3_FishBehaviour);
		}

		private void Update()
		{
		}

		private void Test_EnumFoo()
		{
			FK3_EnumFoo fK3_EnumFoo = FK3_EnumFoo.AAA;
			UnityEngine.Debug.Log(fK3_EnumFoo);
		}

		private void Analyze_Fish_Animator(FK3_FishBehaviour fish)
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
