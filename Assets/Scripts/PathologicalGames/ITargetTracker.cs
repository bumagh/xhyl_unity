namespace PathologicalGames
{
	internal interface ITargetTracker
	{
		TargetList targets
		{
			get;
			set;
		}

		bool dirty
		{
			get;
			set;
		}

		Area area
		{
			get;
		}

		void AddOnPostSortDelegate(OnPostSortDelegate del);

		void SetOnPostSortDelegate(OnPostSortDelegate del);

		void RemoveOnPostSortDelegate(OnPostSortDelegate del);

		void AddOnNewDetectedDelegate(OnNewDetectedDelegate del);

		void SetOnNewDetectedDelegate(OnNewDetectedDelegate del);

		void RemoveOnNewDetectedDelegate(OnNewDetectedDelegate del);
	}
}
