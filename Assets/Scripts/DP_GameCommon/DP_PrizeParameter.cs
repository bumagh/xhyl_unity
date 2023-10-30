namespace DP_GameCommon
{
	public class DP_PrizeParameter
	{
		public AnimalType animalType;

		public int pointIndex;

		public float fAnimalSpinTime = 15f;

		public float fLightPointerSpinTime = 10f;

		public int luckyNum;

		public int bonusNum;

		public DP_PrizeParameter()
		{
			animalType = AnimalType.Lion;
			pointIndex = 0;
			fAnimalSpinTime = 15f;
			fLightPointerSpinTime = 10f;
			luckyNum = 0;
			bonusNum = 0;
		}
	}
}
