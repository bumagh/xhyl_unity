using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Samples.DatabaseEditor
{
	public class Ability
	{
		public string Name;

		public int UnlockGoldCost;

		public Texture Image;

		[InspectorTextArea]
		public string Description;

		public List<ISkillActivator> ActivationRequirements;

		public List<BaseSkillEffect> Effects;
	}
}
