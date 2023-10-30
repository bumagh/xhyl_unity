namespace PathologicalGames
{
	internal interface FK3_ITargetTracker
	{
		FK3_TargetList targets
		{
			get;
			set;
		}

		bool dirty
		{
			get;
			set;
		}

		FK3_Area area
		{
			get;
		}

		void AddOnPostSortDelegate(FK3_OnPostSortDelegate del);

		void SetOnPostSortDelegate(FK3_OnPostSortDelegate del);

		void RemoveOnPostSortDelegate(FK3_OnPostSortDelegate del);

		void AddOnNewDetectedDelegate(FK3_OnNewDetectedDelegate del);

		void SetOnNewDetectedDelegate(FK3_OnNewDetectedDelegate del);

		void RemoveOnNewDetectedDelegate(FK3_OnNewDetectedDelegate del);
	}
}
