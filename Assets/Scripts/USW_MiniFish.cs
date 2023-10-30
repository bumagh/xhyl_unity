using DG.Tweening;
using UnityEngine;

public class USW_MiniFish : MonoBehaviour
{
	public enum FishState
	{
		Forward,
		Waiting,
		TurnAround
	}

	private Tweener mSwimTw;

	private float speed = 4f;

	private float posX;

	private float TagY;

	private Vector2 oldPos;

	public FishState mState;

	private void Awake()
	{
		oldPos = base.transform.localPosition;
		Vector3 localScale = base.transform.localScale;
		posX = localScale.x;
	}

	private void OnEnable()
	{
		GoToForward();
		base.transform.localPosition = oldPos;
		Transform transform = base.transform;
		float x = posX;
		Vector3 localScale = base.transform.localScale;
		transform.localScale = new Vector2(x, localScale.y);
		TagY = UnityEngine.Random.Range(-7f, 7f);
		speed = UnityEngine.Random.Range(3f, 6f);
	}

	private void Update()
	{
		switch (mState)
		{
		case FishState.Forward:
			ForwardListener();
			break;
		case FishState.Waiting:
			WaitingListener();
			break;
		case FishState.TurnAround:
			TurnAroundListener();
			break;
		}
	}

	private void OnDisable()
	{
		GoToWaiting();
	}

	private void ForwardListener()
	{
		base.transform.Translate(((!(posX > 0f)) ? Vector3.right : Vector3.left) * speed * Time.deltaTime);
	}

	private void WaitingListener()
	{
	}

	private void TurnAroundListener()
	{
	}

	private void ChangeState(FishState state)
	{
		mState = state;
		switch (state)
		{
		case FishState.Forward:
			ForwardFunction();
			break;
		case FishState.Waiting:
			WaitingFunction();
			break;
		case FishState.TurnAround:
			TurnAroundFunction();
			break;
		}
	}

	private void ForwardFunction()
	{
		Transform transform = base.transform;
		Vector3 position = base.transform.position;
		mSwimTw = transform.DOMoveY(position.y + TagY, UnityEngine.Random.Range(1f, 2.5f)).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
	}

	private void WaitingFunction()
	{
		if (mSwimTw != null)
		{
			mSwimTw.Kill();
		}
	}

	private void TurnAroundFunction()
	{
	}

	public void GoToForward()
	{
		ChangeState(FishState.Forward);
	}

	public void GoToWaiting()
	{
		ChangeState(FishState.Waiting);
	}

	public void GoToTurnAround()
	{
		ChangeState(FishState.TurnAround);
	}
}
