namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface ISpawnerPlayable<T>
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
