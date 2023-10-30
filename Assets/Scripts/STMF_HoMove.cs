using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Simple Waypoint System/hoMove")]
public class STMF_HoMove : MonoBehaviour
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
		random
	}

	public PathType pathtype = PathType.Curved;

	public bool onStart = true;

	public bool moveToPath = true;

	public bool closePath;

	public bool orientToPath = true;

	public bool local;

	public float lookAhead;

	public float sizeToAdd;

	[HideInInspector]
	public float[] StopAtPoint;

	[HideInInspector]
	public List<STMF_MessageOptions> _messages = new List<STMF_MessageOptions>();

	public TimeValue timeValue = TimeValue.speed;

	private float speed = 0.9f;

	public EaseType easetype;

	public LoopType looptype = LoopType.loop;

	public Vector3[] waypoints;

	[HideInInspector]
	public int currentPoint;

	private bool repeat;

	public Axis lockAxis;

	public Axis lockPosition;

	public Tweener tween;

	private Vector3[] wpPos;

	private TweenParms tParms;

	private PlugVector3Path plugPath;

	private System.Random rand = new System.Random();

	private int[] rndArray;

	private int rndIndex;

	private bool waiting;

	private float originSpeed;

	internal void Start()
	{
		if (onStart)
		{
			StartMove();
		}
	}

	internal void InitWaypoints()
	{
		wpPos = new Vector3[waypoints.Length];
		for (int i = 0; i < wpPos.Length; i++)
		{
			wpPos[i] = waypoints[i] + Vector3.up * sizeToAdd;
		}
	}

	public void StartMove()
	{
		if (waypoints.Length != 0)
		{
			originSpeed = speed;
			if (StopAtPoint == null)
			{
				StopAtPoint = new float[waypoints.Length];
			}
			else if (StopAtPoint.Length < waypoints.Length)
			{
				float[] array = new float[StopAtPoint.Length];
				Array.Copy(StopAtPoint, array, StopAtPoint.Length);
				StopAtPoint = new float[waypoints.Length];
				Array.Copy(array, StopAtPoint, array.Length);
			}
			if (_messages.Count > 0)
			{
				InitializeMessageOptions();
			}
			StartCoroutine(Move());
		}
	}

	internal IEnumerator Move()
	{
		if (moveToPath)
		{
			yield return StartCoroutine(MoveToPath());
		}
		else
		{
			InitWaypoints();
			base.transform.position = waypoints[currentPoint] + Vector3.up * sizeToAdd;
			if (orientToPath && currentPoint < wpPos.Length - 1)
			{
				base.transform.LookAt(wpPos[currentPoint + 1]);
			}
		}
		if (looptype == LoopType.random)
		{
			StartCoroutine(ReachedEnd());
			yield break;
		}
		CreateTween();
		StartCoroutine(NextWaypoint());
	}

	internal IEnumerator MoveToPath()
	{
		int max = (waypoints.Length > 4) ? 4 : waypoints.Length;
		wpPos = new Vector3[max];
		for (int i = 1; i < max; i++)
		{
			wpPos[i] = waypoints[i - 1] + Vector3.up * sizeToAdd;
		}
		wpPos[0] = base.transform.position;
		CreateTween();
		if (tween.isPaused)
		{
			tween.Play();
		}
		yield return StartCoroutine(tween.UsePartialPath(-1, 1).WaitForCompletion());
		moveToPath = false;
		tween.Kill();
		tween = null;
		InitWaypoints();
	}

	internal void CreateTween()
	{
		plugPath = new PlugVector3Path(wpPos, p_isRelative: true, pathtype);
		if (orientToPath || lockAxis != 0)
		{
			plugPath.OrientToPath(lookAhead, lockAxis);
		}
		if (lockPosition != 0)
		{
			plugPath.LockPosition(lockPosition);
		}
		if (closePath)
		{
			plugPath.ClosePath(p_close: true);
		}
		tParms = new TweenParms();
		if (local)
		{
			tParms.Prop("localPosition", plugPath);
		}
		else
		{
			tParms.Prop("position", plugPath);
		}
		tParms.AutoKill(p_active: false);
		tParms.Pause(p_pause: true);
		tParms.Loops(1);
		if (timeValue == TimeValue.speed)
		{
			tParms.SpeedBased();
			tParms.Ease(EaseType.Linear);
		}
		else
		{
			tParms.Ease(easetype);
		}
		tween = HOTween.To(base.transform, originSpeed, tParms);
		if (originSpeed != speed)
		{
			ChangeSpeed(speed);
		}
	}

	internal IEnumerator NextWaypoint()
	{
		for (int point = 0; point < wpPos.Length - 1; point++)
		{
			StartCoroutine(SendMessages());
			if (StopAtPoint[currentPoint] > 0f)
			{
				yield return StartCoroutine(WaitDelay());
			}
			while (waiting)
			{
				yield return null;
			}
			tween.Play();
			yield return StartCoroutine(tween.UsePartialPath(point, point + 1).WaitForCompletion());
			if (repeat)
			{
				currentPoint--;
			}
			else if (looptype == LoopType.random)
			{
				rndIndex++;
				currentPoint = rndArray[rndIndex];
			}
			else
			{
				currentPoint++;
			}
		}
		if (looptype != LoopType.pingPong && looptype != LoopType.random)
		{
			StartCoroutine(SendMessages());
			if (StopAtPoint[currentPoint] > 0f)
			{
				yield return StartCoroutine(WaitDelay());
			}
		}
		StartCoroutine(ReachedEnd());
	}

	internal IEnumerator WaitDelay()
	{
		tween.Pause();
		float timer = Time.time + StopAtPoint[currentPoint];
		while (!waiting && Time.time < timer)
		{
			yield return null;
		}
	}

	internal IEnumerator SendMessages()
	{
		if (_messages.Count != waypoints.Length)
		{
			yield break;
		}
		for (int i = 0; i < _messages[currentPoint].message.Count; i++)
		{
			if (!(_messages[currentPoint].message[i] == string.Empty))
			{
				STMF_MessageOptions sTMF_MessageOptions = _messages[currentPoint];
				switch (sTMF_MessageOptions.type[i])
				{
				case STMF_MessageOptions.ValueType.None:
					SendMessage(sTMF_MessageOptions.message[i], SendMessageOptions.DontRequireReceiver);
					break;
				case STMF_MessageOptions.ValueType.Object:
					SendMessage(sTMF_MessageOptions.message[i], sTMF_MessageOptions.obj[i], SendMessageOptions.DontRequireReceiver);
					break;
				case STMF_MessageOptions.ValueType.Text:
					SendMessage(sTMF_MessageOptions.message[i], sTMF_MessageOptions.text[i], SendMessageOptions.DontRequireReceiver);
					break;
				case STMF_MessageOptions.ValueType.Numeric:
					SendMessage(sTMF_MessageOptions.message[i], sTMF_MessageOptions.num[i], SendMessageOptions.DontRequireReceiver);
					break;
				case STMF_MessageOptions.ValueType.Vector2:
					SendMessage(sTMF_MessageOptions.message[i], sTMF_MessageOptions.vect2[i], SendMessageOptions.DontRequireReceiver);
					break;
				case STMF_MessageOptions.ValueType.Vector3:
					SendMessage(sTMF_MessageOptions.message[i], sTMF_MessageOptions.vect3[i], SendMessageOptions.DontRequireReceiver);
					break;
				}
			}
		}
	}

	internal void InitializeMessageOptions()
	{
		if (_messages.Count < waypoints.Length)
		{
			for (int i = _messages.Count; i < waypoints.Length; i++)
			{
				STMF_MessageOptions item = AddMessageToOption(new STMF_MessageOptions());
				_messages.Add(item);
			}
		}
		else if (_messages.Count > waypoints.Length)
		{
			for (int num = _messages.Count - 1; num >= waypoints.Length; num--)
			{
				_messages.RemoveAt(num);
			}
		}
	}

	internal STMF_MessageOptions AddMessageToOption(STMF_MessageOptions opt)
	{
		opt.message.Add(string.Empty);
		opt.type.Add(STMF_MessageOptions.ValueType.None);
		opt.obj.Add(null);
		opt.text.Add(null);
		opt.num.Add(0f);
		opt.vect2.Add(Vector2.zero);
		opt.vect3.Add(Vector3.zero);
		return opt;
	}

	internal IEnumerator ReachedEnd()
	{
		switch (looptype)
		{
		case LoopType.none:
			tween.Kill();
			tween = null;
			yield break;
		case LoopType.loop:
			if (closePath)
			{
				tween.Play();
				yield return StartCoroutine(tween.UsePartialPath(currentPoint, -1).WaitForCompletion());
			}
			currentPoint = 0;
			break;
		case LoopType.pingPong:
			tween.Kill();
			tween = null;
			if (!repeat)
			{
				repeat = true;
				for (int k = 0; k < wpPos.Length; k++)
				{
					wpPos[k] = waypoints[waypoints.Length - 1 - k] + Vector3.up * sizeToAdd;
				}
			}
			else
			{
				InitWaypoints();
				repeat = false;
			}
			CreateTween();
			break;
		case LoopType.random:
		{
			rndIndex = 0;
			InitWaypoints();
			if (tween != null)
			{
				tween.Kill();
				tween = null;
			}
			rndArray = new int[wpPos.Length];
			for (int i = 0; i < rndArray.Length; i++)
			{
				rndArray[i] = i;
			}
			int num = wpPos.Length;
			while (num > 1)
			{
				System.Random random = rand;
				int maxValue;
				num = (maxValue = num) - 1;
				int num2 = random.Next(maxValue);
				Vector3 vector = wpPos[num];
				wpPos[num] = wpPos[num2];
				wpPos[num2] = vector;
				int num3 = rndArray[num];
				rndArray[num] = rndArray[num2];
				rndArray[num2] = num3;
			}
			Vector3 vector2 = wpPos[0];
			int num4 = rndArray[0];
			for (int j = 0; j < wpPos.Length; j++)
			{
				if (rndArray[j] == currentPoint)
				{
					rndArray[j] = num4;
					wpPos[0] = wpPos[j];
					wpPos[j] = vector2;
				}
			}
			rndArray[0] = currentPoint;
			CreateTween();
			break;
		}
		}
		StartCoroutine(NextWaypoint());
		base.gameObject.SendMessage("OnPathEnd");
	}

	public void Stop()
	{
		StopAllCoroutines();
		HOTween.Kill(base.transform);
		plugPath = null;
		tween = null;
	}

	public void Reset()
	{
		Stop();
		currentPoint = 0;
		if (waypoints.Length != 0)
		{
			base.transform.position = waypoints[currentPoint] + Vector3.up * sizeToAdd;
		}
	}

	public void Pause()
	{
		waiting = true;
		HOTween.Pause(base.transform);
	}

	public void Resume()
	{
		waiting = false;
		HOTween.Play(base.transform);
	}

	public void ChangeSpeed(float value)
	{
		float timeScale = (timeValue != TimeValue.speed) ? (originSpeed / value) : (value / originSpeed);
		speed = value;
		tween.timeScale = timeScale;
	}

	public STMF_MessageOptions GetMessageOption(int waypointID, int messageID)
	{
		InitializeMessageOptions();
		STMF_MessageOptions sTMF_MessageOptions = _messages[waypointID];
		for (int i = sTMF_MessageOptions.message.Count; i <= messageID; i++)
		{
			AddMessageToOption(sTMF_MessageOptions);
		}
		return sTMF_MessageOptions;
	}
}
