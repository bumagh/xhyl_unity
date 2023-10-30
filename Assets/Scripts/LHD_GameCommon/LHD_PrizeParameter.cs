namespace LHD_GameCommon
{
	public class LHD_PrizeParameter
	{
		public AnimalType mAnimal;

		public int mnLightIndex;

		public float mfAnimalSpinTime = 15f;

		public float mfLightPointerSpinTime = 10f;

		public int mMoreInfoValue;

		public int mnLuckyNum;

		public int mnBonus;

		public LHD_PrizeParameter()
		{
			mAnimal = AnimalType.Lion;
			mnLightIndex = 0;
			mfAnimalSpinTime = 15f;
			mfLightPointerSpinTime = 10f;
			mMoreInfoValue = 0;
			mnLuckyNum = 0;
			mnBonus = 0;
		}
	}
}
