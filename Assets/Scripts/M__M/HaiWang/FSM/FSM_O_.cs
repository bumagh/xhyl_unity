namespace M__M.HaiWang.FSM
{
	public class FSM<O>
	{
		public O Owner;

		private State<O> _currentState;

		private State<O> _previousState;

		private State<O> _globalState;

		public State<O> CurrentState
		{
			get
			{
				return _currentState;
			}
			set
			{
				_currentState = value;
			}
		}

		public State<O> PreviousState
		{
			get
			{
				return _previousState;
			}
			set
			{
				_previousState = value;
			}
		}

		public State<O> GlobalState
		{
			set
			{
				_globalState = value;
			}
		}

		public FSM(O o)
		{
			Owner = o;
			_currentState = null;
			_previousState = null;
			_globalState = null;
		}

		public T ChangeState<T>() where T : State<O>, new()
		{
			return ChangeState<T>(null, force: false);
		}

		public T ChangeState<T>(ArgList args, bool force) where T : State<O>, new()
		{
			if (!force && _currentState is T)
			{
				return _currentState as T;
			}
			ExitState();
			return EnterState<T>(args);
		}

		public T EnterState<T>() where T : State<O>, new()
		{
			return EnterState<T>(null);
		}

		public T EnterState<T>(ArgList args) where T : State<O>, new()
		{
			T val = new T();
			val.Owner = Owner;
			val.Fsm = this;
			T result = (T)(_currentState = val);
			result.OnEnter(args);
			return result;
		}

		public State<O> ExitState()
		{
			if (_currentState != null)
			{
				_previousState = _currentState;
				_currentState = null;
				_previousState.OnExit();
				return _previousState;
			}
			return null;
		}

		public void RevertToPreviousState()
		{
			RevertToPreviousState(null);
		}

		public void RevertToPreviousState(ArgList args)
		{
			if (_previousState != null)
			{
				ExitState();
				_currentState = _previousState;
				_previousState.OnEnter(args);
			}
		}

		public void Update()
		{
			if (_currentState != null)
			{
				_currentState.OnUpdate();
			}
			if (_globalState != null)
			{
				_globalState.OnUpdate();
			}
		}

		public void OnEvent(Event evt)
		{
			if (_currentState != null)
			{
				_currentState.OnEvent(evt);
			}
			if (_globalState != null)
			{
				_globalState.OnEvent(evt);
			}
		}

		public bool IsInState<T>()
		{
			return _currentState is T;
		}
	}
}
