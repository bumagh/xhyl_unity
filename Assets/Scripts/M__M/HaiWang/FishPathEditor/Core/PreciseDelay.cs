using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class PreciseDelay
	{
		public enum AdjustMode
		{
			Disable,
			Positive,
			Nagative,
			Always
		}

		public delegate void PreDelayAction(float expectedDelay, float adjustedDelay);

		public delegate void AfterDelayAction(float expectedDelay, float adjustedDelay, float actualDelay);

		private float _diffTime;

		private float _timeBeforeDelay;

		private float _expectedDelay;

		private float _adjustedDelay;

		private float _actualDelay;

		private AdjustMode _mode;

		public PreDelayAction onPreDelay;

		public AfterDelayAction onDelayDone;

		public float ExpectedDelay => _expectedDelay;

		public float AdjustedDelay => _adjustedDelay;

		public float ActualDelay => _actualDelay;

		public float Difference => _diffTime;

		public void SetMode(AdjustMode mode)
		{
			_mode = mode;
		}

		public void Clear()
		{
			_diffTime = 0f;
			_timeBeforeDelay = 0f;
			_expectedDelay = 0f;
			_actualDelay = 0f;
		}

		public WaitForSeconds WaitDelay(float delay, bool adjust = true)
		{
			_adjustedDelay = CalAdjustedDelay(delay);
			if (onPreDelay != null)
			{
				onPreDelay(delay, _adjustedDelay);
			}
			_timeBeforeDelay = Time.time;
			_expectedDelay = delay;
			return new WaitForSeconds(_adjustedDelay);
		}

		private float CalAdjustedDelay(float delay, bool adjust = true)
		{
			float result = delay;
			if (!adjust)
			{
				return result;
			}
			switch (_mode)
			{
			case AdjustMode.Always:
				result = delay - _diffTime;
				break;
			case AdjustMode.Nagative:
				result = ((!(_diffTime >= 0f)) ? (delay - _diffTime) : delay);
				break;
			case AdjustMode.Positive:
				result = ((!(_diffTime <= 0f)) ? (delay - _diffTime) : delay);
				break;
			}
			return result;
		}

		public void AddCPUDelay()
		{
		}

		public void Done()
		{
			_actualDelay = Time.time - _timeBeforeDelay;
			_diffTime = _actualDelay - _expectedDelay;
			if (onDelayDone != null)
			{
				onDelayDone(_expectedDelay, _actualDelay, _actualDelay);
			}
		}
	}
}
