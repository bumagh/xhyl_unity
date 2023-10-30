using FullInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Scenario
{
	[Serializable]
	public class ScenarioBase : IScenario
	{
		[InspectorDisabled]
		public readonly ScenarioType type;

		protected MonoBehaviour manager;

		protected ScenarioBase(ScenarioType _type)
		{
			type = _type;
		}

		public void SetManger(MonoBehaviour manager)
		{
			this.manager = manager;
		}

		public virtual void Assert()
		{
		}

		public virtual void ScenarioProgress(int dur)
		{
		}

		public virtual void Enter()
		{
		}

		public virtual void BossEnter()
		{
		}

		public virtual void ScenarioRotate(int dur)
		{
		}

		public virtual void ScenarioMove(int dur)
		{
		}

		public virtual void Exit()
		{
		}

		public virtual Image[] GetRenderers4Fade()
		{
			return null;
		}
	}
}
