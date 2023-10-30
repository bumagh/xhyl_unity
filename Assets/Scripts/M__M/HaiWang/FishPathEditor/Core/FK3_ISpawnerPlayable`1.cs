namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface FK3_ISpawnerPlayable<T>
	{
		bool IsPlaying
		{
			get;
		}

		void Play();

		void Play(float playTime);

		void Stop();
	}
}
