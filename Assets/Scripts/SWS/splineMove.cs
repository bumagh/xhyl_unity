using DG.Tweening;
using M__M.HaiWang.Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SWS
{
	[AddComponentMenu("Simple Waypoint System/splineMove")]
	public class splineMove : MonoBehaviour
	{
		public enum TimeValue
		{
			time,
			speed
		}

		public enum LoopType
		{
			none,
			loop,
			pingPong,
			random,
			yoyo
		}

		public PathManager pathContainer;

		public Vector3 offset = Vector3.zero;

		public bool onStart;

		public bool moveToPath;

		public bool reverse;

		public int startPoint;

		[HideInInspector]
		public int currentPoint;

		public bool closeLoop;

		public bool local;

		public float lookAhead;

		public float sizeToAdd;

		public TimeValue timeValue = TimeValue.speed;

		public float speed = 5f;

		private float originSpeed;

		public AnimationCurve animEaseType;

		public LoopType loopType;

		[HideInInspector]
		public Vector3[] waypoints;

		private Vector3[] wpPos;

		[HideInInspector]
		public List<UnityEvent> events = new List<UnityEvent>();

		public PathType pathType = PathType.CatmullRom;

		public PathMode pathMode = PathMode.Full3D;

		public Ease easeType = Ease.Linear;

		public AxisConstraint lockPosition;

		public AxisConstraint lockRotation;

		[HideInInspector]
		public Tweener tween;

		private System.Random rand = new System.Random();

		private int[] rndArray;

		private void Start()
		{
			if (onStart)
			{
				StartMove();
			}
		}

		public void StartMove()
		{
			if (pathContainer == null)
			{
				UnityEngine.Debug.LogError(base.gameObject.name + " 没有分配路径控制器!");
				return;
			}
			waypoints = pathContainer.GetPathPoints(local);
			originSpeed = speed;
			startPoint = Mathf.Clamp(startPoint, 0, waypoints.Length - 1);
			int num = startPoint;
			if (reverse)
			{
				Array.Reverse((Array)waypoints);
				num = waypoints.Length - 1 - num;
			}
			Initialize(num);
			Stop();
			CreateTween();
		}

		private void Initialize(int startAt = 0)
		{
			if (!moveToPath)
			{
				startAt = 0;
			}
			wpPos = new Vector3[waypoints.Length - startAt];
			for (int i = 0; i < wpPos.Length; i++)
			{
				wpPos[i] = waypoints[i + startAt] + new Vector3(0f, sizeToAdd, 0f);
			}
			for (int j = events.Count; j <= pathContainer.GetEventsCount() - 1; j++)
			{
				events.Add(new UnityEvent());
			}
		}

		public void InitializeEvents()
		{
			events.Clear();
			for (int i = events.Count; i <= pathContainer.GetEventsCount() - 1; i++)
			{
				events.Add(new UnityEvent());
			}
		}

		private void CreateTween()
		{
			TweenParams tweenParams = new TweenParams();
			if (timeValue == TimeValue.speed)
			{
				tweenParams.SetSpeedBased();
			}
			if (loopType == LoopType.yoyo)
			{
				tweenParams.SetLoops(-1, DG.Tweening.LoopType.Yoyo);
			}
			if (easeType == Ease.Unset)
			{
				tweenParams.SetEase(animEaseType);
			}
			else
			{
				tweenParams.SetEase(easeType);
			}
			if (moveToPath)
			{
				tweenParams.OnWaypointChange(OnWaypointReached);
			}
			else
			{
				if (loopType == LoopType.random)
				{
					RandomizeWaypoints();
				}
				else if (loopType == LoopType.yoyo)
				{
					tweenParams.OnStepComplete(ReachedEnd);
				}
				Vector3 position = wpPos[0];
				if (local)
				{
					position = pathContainer.transform.TransformPoint(position);
				}
				base.transform.position = position;
				tweenParams.OnWaypointChange(OnWaypointChange);
				tweenParams.OnComplete(ReachedEnd);
			}
			base.transform.position += offset;
			for (int i = 0; i < wpPos.Length; i++)
			{
				wpPos[i] += offset;
			}
			if (local)
			{
				tween = base.transform.DOLocalPath(wpPos, originSpeed, pathType, pathMode).SetAs(tweenParams).SetOptions(closeLoop, lockPosition, lockRotation)
					.SetLookAt(lookAhead);
			}
			else
			{
				tween = base.transform.DOPath(wpPos, originSpeed, pathType, pathMode).SetAs(tweenParams).SetOptions(closeLoop, lockPosition, lockRotation)
					.SetLookAt(lookAhead);
			}
			if (!moveToPath && startPoint > 0)
			{
				GoToWaypoint(startPoint);
				startPoint = 0;
			}
			if (originSpeed != speed)
			{
				ChangeSpeed(speed);
			}
		}

		private void OnWaypointReached(int index)
		{
			if (index > 0)
			{
				Stop();
				moveToPath = false;
				Initialize();
				CreateTween();
			}
		}

		private void OnWaypointChange(int index)
		{
			index = pathContainer.GetWaypointIndex(index);
			if (index != -1)
			{
				if (loopType != LoopType.yoyo && reverse)
				{
					index = waypoints.Length - 1 - index;
				}
				if (loopType == LoopType.random)
				{
					index = rndArray[index];
				}
				currentPoint = index;
				FishBehaviour component = GetComponent<FishBehaviour>();
				if (index == events.Count - 1)
				{
				}
				if (events != null && events.Count - 1 >= index && events[index] != null && (loopType != LoopType.random || index != rndArray[rndArray.Length - 1]))
				{
					events[index].Invoke();
				}
			}
		}

		private void ReachedEnd()
		{
			switch (loopType)
			{
			case LoopType.none:
				break;
			case LoopType.loop:
				currentPoint = 0;
				CreateTween();
				break;
			case LoopType.pingPong:
				reverse = !reverse;
				Array.Reverse((Array)waypoints);
				Initialize();
				CreateTween();
				break;
			case LoopType.random:
				RandomizeWaypoints();
				CreateTween();
				break;
			case LoopType.yoyo:
				reverse = !reverse;
				break;
			}
		}

		private void RandomizeWaypoints()
		{
			Initialize();
			rndArray = new int[wpPos.Length];
			for (int i = 0; i < rndArray.Length; i++)
			{
				rndArray[i] = i;
			}
			int num = wpPos.Length;
			while (num > 1)
			{
				int num3 = rand.Next(num--);
				Vector3 vector = wpPos[num];
				wpPos[num] = wpPos[num3];
				wpPos[num3] = vector;
				int num4 = rndArray[num];
				rndArray[num] = rndArray[num3];
				rndArray[num3] = num4;
			}
			Vector3 vector2 = wpPos[0];
			int num5 = rndArray[0];
			for (int j = 0; j < wpPos.Length; j++)
			{
				if (rndArray[j] == currentPoint)
				{
					rndArray[j] = num5;
					wpPos[0] = wpPos[j];
					wpPos[j] = vector2;
				}
			}
			rndArray[0] = currentPoint;
		}

		public void GoToWaypoint(int index)
		{
			if (tween != null)
			{
				if (reverse)
				{
					index = waypoints.Length - 1 - index;
				}
				tween.ForceInit();
				tween.GotoWaypoint(index, andPlay: true);
			}
		}

		public void Pause(float seconds = 0f)
		{
			StopCoroutine(Wait());
			if (tween != null)
			{
				tween.Pause();
			}
			if (seconds > 0f)
			{
				StartCoroutine(Wait(seconds));
			}
		}

		private IEnumerator Wait(float secs = 0f)
		{
			yield return new WaitForSeconds(secs);
			Resume();
		}

		public void Resume()
		{
			StopCoroutine(Wait());
			if (tween != null)
			{
				tween.Play();
			}
		}

		public void Reverse()
		{
			reverse = !reverse;
			float fullPosition = 0f;
			if (tween != null)
			{
				fullPosition = tween.Duration(includeLoops: false) - tween.Elapsed(includeLoops: false);
			}
			startPoint = waypoints.Length - 1 - currentPoint;
			StartMove();
			tween.ForceInit();
			tween.fullPosition = fullPosition;
		}

		public void SetPath(PathManager newPath)
		{
			Stop();
			pathContainer = newPath;
			StartMove();
		}

		public void Stop()
		{
			StopAllCoroutines();
			if (tween != null)
			{
				tween.Kill();
			}
			tween = null;
		}

		public void ResetToStart()
		{
			Stop();
			currentPoint = 0;
			if ((bool)pathContainer)
			{
				base.transform.position = pathContainer.waypoints[currentPoint].position + new Vector3(0f, sizeToAdd, 0f);
			}
		}

		public void ChangeSpeed(float value)
		{
			float timeScale = (timeValue != TimeValue.speed) ? (originSpeed / value) : (value / originSpeed);
			speed = value;
			if (tween != null)
			{
				tween.timeScale = timeScale;
			}
		}
	}
}
