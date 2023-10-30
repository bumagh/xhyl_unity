namespace FullInspector.Samples.ItemDatabase
{
	public class WeaponItem : IItem
	{
		public enum WeaponType
		{
			Sword,
			Axe,
			Hammer
		}

		public WeaponType Type;

		public float AttackStrength;
	}
}
