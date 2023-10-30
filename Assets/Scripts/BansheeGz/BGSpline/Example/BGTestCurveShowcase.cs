using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestCurveShowcase : MonoBehaviour
	{
		private abstract class Phase
		{
			private readonly float periodMin;

			private readonly float periodMax;

			internal float period = -10000f;

			internal float startTime;

			protected Phase(float periodMin, float periodMax)
			{
				this.periodMin = periodMin;
				this.periodMax = periodMax;
			}

			protected internal virtual void PhaseStart()
			{
				startTime = Time.time;
				period = UnityEngine.Random.Range(periodMin, periodMax);
			}

			public abstract void Update();
		}

		private sealed class PhaseDelay : Phase
		{
			public PhaseDelay(float periodMin, float periodMax)
				: base(periodMin, periodMax)
			{
			}

			public override void Update()
			{
			}
		}

		private abstract class Effect : Phase
		{
			private readonly List<Phase> phases = new List<Phase>();

			private int currentPhaseIndex;

			protected Effect(float periodMin, float periodMax)
				: base(periodMin, periodMax)
			{
				phases.Add(this);
			}

			protected void AddPhase(Phase phase)
			{
				phases.Add(phase);
			}

			protected void AddPhase(Phase phase, int index)
			{
				phases.Insert(index, phase);
			}

			public override void Update()
			{
				Phase phase = phases[currentPhaseIndex];
				bool flag = false;
				if (Time.time - phase.startTime > phase.period)
				{
					flag = true;
					currentPhaseIndex++;
					if (currentPhaseIndex == phases.Count)
					{
						currentPhaseIndex = 0;
					}
					phase = phases[currentPhaseIndex];
					phase.PhaseStart();
				}
				if (phase is Effect)
				{
					Effect effect = (Effect)phase;
					if (flag)
					{
						effect.Start();
					}
					effect.Update((Time.time - phase.startTime) / phase.period);
				}
			}

			protected float CheckReverse(float ratio, bool reverse)
			{
				return (!reverse) ? ratio : (1f - ratio);
			}

			protected abstract void Update(float ratio);

			protected virtual void Start()
			{
			}

			protected static float Scale(float ratio, float count)
			{
				float num = 1f / count;
				return (ratio - (float)Mathf.FloorToInt(ratio / num) * num) / num;
			}
		}

		private sealed class EffectScale : Effect
		{
			private readonly GameObject target;

			private Vector3 min;

			private Vector3 max;

			private Vector3 oldScale;

			private Vector3 newScale;

			public EffectScale(GameObject target, float periodMin, float periodMax, Vector3 min, Vector3 max)
				: base(periodMin, periodMax)
			{
				this.target = target;
				newScale = target.transform.localScale;
				this.min = min;
				this.max = max;
			}

			protected override void Update(float ratio)
			{
				target.transform.localScale = Vector3.Lerp(oldScale, newScale, ratio);
			}

			protected void Start()
			{
				oldScale = newScale;
				newScale = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
			}
		}

		private sealed class EffectRotate : Effect
		{
			internal enum CycleType
			{
				FirstToLast,
				Swing,
				Random
			}

			private readonly GameObject target;

			private readonly Vector3 min;

			private readonly Vector3 max;

			private readonly CycleType cycleType;

			private bool reverse;

			public EffectRotate(CycleType cycleType, GameObject target, float period, Vector3 min, Vector3 max, float delayMin, float delayMax)
				: this(cycleType, target, period, min, max)
			{
				AddPhase(new PhaseDelay(delayMin, delayMax), 0);
			}

			public EffectRotate(CycleType cycleType, GameObject target, float period, Vector3 min, Vector3 max)
				: base(period, period)
			{
				this.target = target;
				this.cycleType = cycleType;
				this.min = min;
				this.max = max;
			}

			protected override void Update(float ratio)
			{
				target.transform.eulerAngles = Vector3.Lerp(min, max, CheckReverse(ratio, reverse));
			}

			protected void Start()
			{
				switch (cycleType)
				{
				case CycleType.FirstToLast:
					reverse = false;
					break;
				case CycleType.Swing:
					reverse = !reverse;
					break;
				default:
					reverse = (UnityEngine.Random.Range(0, 2) == 0);
					break;
				}
			}
		}

		private sealed class EffectChangeTiling : Effect
		{
			private readonly float tileXMin;

			private readonly float tileXMax;

			private readonly float tileYMin;

			private readonly float tileYMax;

			private readonly Material material;

			private bool reverse;

			public EffectChangeTiling(float period, Material material, float tileXMin, float tileXMax, float tileYMin, float tileYMax)
				: base(period, period)
			{
				this.material = material;
				this.tileXMin = tileXMin;
				this.tileXMax = tileXMax;
				this.tileYMin = tileYMin;
				this.tileYMax = tileYMax;
			}

			protected override void Update(float ratio)
			{
				ratio = CheckReverse(ratio, reverse);
				material.mainTextureScale = new Vector2(Mathf.Lerp(tileXMin, tileXMax, ratio), Mathf.Lerp(tileYMin, tileYMax, ratio));
			}

			protected void Start()
			{
				reverse = !reverse;
			}
		}

		private sealed class EffectMoveAndRotateAlongCurve : Effect
		{
			private readonly GameObject target;

			private readonly BGCcCursor cursor;

			private readonly float rotateCount;

			private readonly float rotationDistance;

			private readonly float initialRotationRadians;

			public EffectMoveAndRotateAlongCurve(BGCcCursor cursor, GameObject target, float period, int rotateCount, float rotationDistance, float initialRotationRadians = 0f)
				: base(period, period)
			{
				this.target = target;
				this.cursor = cursor;
				this.rotateCount = rotateCount;
				this.rotationDistance = rotationDistance;
				this.initialRotationRadians = initialRotationRadians;
			}

			protected override void Update(float ratio)
			{
				Vector3 a = cursor.CalculatePosition();
				Vector3 forward = cursor.CalculateTangent();
				float f = initialRotationRadians + Mathf.Lerp(0f, (float)Math.PI * 2f, Effect.Scale(ratio, rotateCount));
				Vector3 position = a + Quaternion.LookRotation(forward) * (new Vector3(Mathf.Sin(f), Mathf.Cos(f)) * rotationDistance);
				target.transform.position = position;
			}
		}

		private sealed class EffectDynamicCurve : Effect
		{
			private const int PointsCount = 3;

			private const float SpanX = 8f;

			private const float SpanZ = 4f;

			private readonly BGCcMath math;

			private readonly Light[] lights;

			private readonly float[] fromDistanceRatios;

			private readonly float[] toDistanceRatios;

			public EffectDynamicCurve(GameObject target, float period, params Light[] lights)
				: base(period, period)
			{
				target.AddComponent<BGCurve>();
				math = target.AddComponent<BGCcMath>();
				math.Curve.Closed = true;
				this.lights = lights;
				fromDistanceRatios = new float[lights.Length];
				toDistanceRatios = new float[lights.Length];
			}

			protected override void Update(float ratio)
			{
				for (int i = 0; i < lights.Length; i++)
				{
					Light light = lights[i];
					light.gameObject.transform.position = math.Math.CalcByDistanceRatio(BGCurveBaseMath.Field.Position, Mathf.Lerp(fromDistanceRatios[i], toDistanceRatios[i], ratio));
					if ((double)ratio < 0.1)
					{
						light.intensity = Mathf.Lerp(0f, 3f, ratio * 10f);
					}
					else if ((double)ratio > 0.9)
					{
						light.intensity = Mathf.Lerp(3f, 0f, (ratio - 0.9f) * 10f);
					}
				}
			}

			protected void Start()
			{
				BGCurve curve = math.Curve;
				curve.Clear();
				for (int i = 0; i < 3; i++)
				{
					AddPoint(curve);
				}
				for (int j = 0; j < fromDistanceRatios.Length; j++)
				{
					fromDistanceRatios[j] = UnityEngine.Random.Range(0f, 1f);
					toDistanceRatios[j] = UnityEngine.Random.Range(0f, 1f);
				}
			}

			private void AddPoint(BGCurve curve)
			{
				Vector3 vector = RandomVector();
				curve.AddPoint(new BGCurvePoint(curve, RandomVector(), BGCurvePoint.ControlTypeEnum.BezierSymmetrical, vector, -vector));
			}

			private Vector3 RandomVector()
			{
				return new Vector3(UnityEngine.Random.Range(-8f, 8f), 0f, UnityEngine.Random.Range(-4f, 4f));
			}
		}

		[Header("Light")]
		public Light Light;

		[Header("Logo parts")]
		public GameObject B;

		public GameObject G;

		public BGCcMath Curve;

		[Header("Projectiles")]
		public GameObject ProjectileFolder;

		public TrailRenderer Projectile1;

		public TrailRenderer Projectile2;

		public BGCcCursor ProjectileCurve1;

		[Header("Particles")]
		public ParticleSystem BParticles;

		public ParticleSystem GParticles;

		public ParticleSystem CurveParticles1;

		public ParticleSystem CurveParticles2;

		[Header("Dynamic")]
		public GameObject DynamicCurve;

		public Light Light1;

		public Light Light2;

		public Light Light3;

		private readonly List<Effect> effects = new List<Effect>();

		private const float ScaleMin = 0.85f;

		private const float ScaleMax = 1.15f;

		private static readonly Vector3 FromScale = new Vector3(0.85f, 0.85f, 0.85f);

		private static readonly Vector3 ToScale = new Vector3(1.15f, 1.15f, 1.15f);

		private const float ScalePeriodMin = 1f;

		private const float ScalePeriodMax = 2f;

		private void Start()
		{
			effects.Add(new EffectScale(base.gameObject, 1f, 2f, FromScale, ToScale));
			effects.Add(new EffectScale(B, 1f, 2f, FromScale, ToScale));
			effects.Add(new EffectScale(G, 1f, 2f, FromScale, ToScale));
			effects.Add(new EffectScale(Curve.gameObject, 1f, 2f, FromScale, ToScale));
			effects.Add(new EffectRotate(EffectRotate.CycleType.Random, B.gameObject, 1f, Vector3.zero, new Vector3(0f, 360f, 0f), 2f, 3f));
			effects.Add(new EffectRotate(EffectRotate.CycleType.Random, G.gameObject, 1.6f, Vector3.zero, new Vector3(0f, 360f, 0f), 4f, 6f));
			effects.Add(new EffectChangeTiling(2f, B.GetComponent<LineRenderer>().sharedMaterial, 0f, 0f, 0.2f, 1f));
			effects.Add(new EffectRotate(EffectRotate.CycleType.Swing, Light.gameObject, 3f, new Vector3(70f, -90f, 0f), new Vector3(110f, -90f, 0f)));
			effects.Add(new EffectRotate(EffectRotate.CycleType.FirstToLast, ProjectileFolder, 10f, Vector3.zero, new Vector3(360f, 0f, 0f)));
			effects.Add(new EffectMoveAndRotateAlongCurve(ProjectileCurve1, Projectile1.gameObject, 3f, 5, 0.1f));
			effects.Add(new EffectMoveAndRotateAlongCurve(ProjectileCurve1, Projectile2.gameObject, 3f, 5, 0.1f, (float)Math.PI));
			effects.Add(new EffectDynamicCurve(DynamicCurve, 4f, Light1, Light2, Light3));
		}

		private void Update()
		{
			foreach (Effect effect in effects)
			{
				effect.Update();
			}
		}
	}
}
