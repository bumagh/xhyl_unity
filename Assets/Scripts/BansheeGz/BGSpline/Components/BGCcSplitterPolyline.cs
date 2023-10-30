using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcSplitterPolyline")]
	[CcDescriptor(Description = "Calculates points positions for polyline along the curve. It does not change or modify anything. Use Positions field to access points.", Name = "Splitter Polyline", Icon = "BGCcSplitterPolyline123")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcSplitterPolyline")]
	public class BGCcSplitterPolyline : BGCcWithMath
	{
		public enum SplitModeEnum
		{
			UseMathData,
			PartsTotal,
			PartsPerSection
		}

		public struct PolylinePoint
		{
			public Vector3 Position;

			public float Distance;

			public Vector3 Tangent;

			public PolylinePoint(Vector3 position, float distance)
			{
				this = default(PolylinePoint);
				Position = position;
				Distance = distance;
			}

			public PolylinePoint(Vector3 position, float distance, Vector3 tangent)
			{
				this = new PolylinePoint(position, distance);
				Tangent = tangent;
			}

			public override string ToString()
			{
				return "Pos=" + Position + "; Distance=" + Distance;
			}
		}

		[SerializeField]
		[Tooltip("How to split the curve. TotalSections -total sections for whole curve;\r\n PartSections - each part (between 2 points) will use the same amount of splits;\r\nUseMathData -use data, precalculated by Math component. Note, you can tweak some params at Math as well.")]
		private SplitModeEnum splitMode;

		[Range(1f, 1000f)]
		[SerializeField]
		[Tooltip("Total number of parts to split a curve to. The actual number of parts can be less than partsTotal due to optimization, but never more.")]
		private int partsTotal = 30;

		[Range(1f, 150f)]
		[Tooltip("Every section of the curve will be split on even parts. The actual number of parts can be less than partsPerSection due to optimization, but never more.")]
		[SerializeField]
		private int partsPerSection = 30;

		[SerializeField]
		[Tooltip("Split straight lines. Straight lines are optimized by default and are not split.")]
		private bool doNotOptimizeStraightLines;

		[Tooltip("By default positions in world coordinates. Set this parameter to true to use local coordinates. Local coordinates are calculated slower.")]
		[SerializeField]
		protected bool useLocal;

		protected readonly List<Vector3> positions = new List<Vector3>();

		protected readonly List<PolylinePoint> points = new List<PolylinePoint>();

		protected bool dataValid;

		private BGPolylineSplitter splitter;

		private BGPolylineSplitter.Config config;

		public SplitModeEnum SplitMode
		{
			get
			{
				return splitMode;
			}
			set
			{
				ParamChanged(ref splitMode, value);
			}
		}

		public int PartsTotal
		{
			get
			{
				return Mathf.Clamp(partsTotal, 1, 1000);
			}
			set
			{
				ParamChanged(ref partsTotal, Mathf.Clamp(value, 1, 1000));
			}
		}

		public int PartsPerSection
		{
			get
			{
				return Mathf.Clamp(partsPerSection, 1, 150);
			}
			set
			{
				ParamChanged(ref partsPerSection, Mathf.Clamp(value, 1, 150));
			}
		}

		public bool DoNotOptimizeStraightLines
		{
			get
			{
				return doNotOptimizeStraightLines;
			}
			set
			{
				ParamChanged(ref doNotOptimizeStraightLines, value);
			}
		}

		public virtual bool UseLocal
		{
			get
			{
				return useLocal;
			}
			set
			{
				ParamChanged(ref useLocal, value);
			}
		}

		public override string Warning
		{
			get
			{
				BGCcMath math = base.Math;
				string result = string.Empty;
				if (math == null)
				{
					return result;
				}
				switch (SplitMode)
				{
				case SplitModeEnum.PartsTotal:
				{
					int num = math.Math.SectionsCount - ((!DoNotOptimizeStraightLines) ? BGPolylineSplitter.CountStraightLines(base.Math.Math, null) : 0);
					int num2 = (num != 0) ? (PartsTotal / num) : 0;
					if (num2 > math.SectionParts)
					{
						result = "Math use less parts per section (" + math.SectionParts + "). You now use " + num2 + " parts for curved section. You need to increase Math's 'SectionParts' field accordingly to increase polyline precision.";
					}
					break;
				}
				case SplitModeEnum.PartsPerSection:
					if (PartsPerSection > math.SectionParts)
					{
						result = "Math use less parts per section (" + math.SectionParts + "). You need to increase Math's 'SectionParts' field accordingly to increase polyline precision.";
					}
					break;
				}
				return result;
			}
		}

		public override string Info => "Polyline has " + PointsCount + " points";

		public override bool SupportHandles => true;

		public override bool SupportHandlesSettings => true;

		public int PointsCount
		{
			get
			{
				if (!dataValid)
				{
					UpdateData();
				}
				return (positions != null) ? positions.Count : 0;
			}
		}

		public List<Vector3> Positions
		{
			get
			{
				if (!dataValid)
				{
					UpdateData();
				}
				return positions;
			}
		}

		public List<PolylinePoint> Points
		{
			get
			{
				if (!dataValid)
				{
					UpdateData();
				}
				return points;
			}
		}

		public event EventHandler ChangedPositions;

		public override void Start()
		{
			AddListeners();
		}

		public override void OnDestroy()
		{
			RemoveListeners();
		}

		public void AddListeners()
		{
			base.Math.ChangedMath -= UpdateRequested;
			base.Math.ChangedMath += UpdateRequested;
			base.ChangedParams -= UpdateRequested;
			base.ChangedParams += UpdateRequested;
		}

		public void InvalidateData()
		{
			dataValid = false;
			if (this.ChangedPositions != null)
			{
				this.ChangedPositions(this, null);
			}
		}

		public void RemoveListeners()
		{
			try
			{
				base.Math.ChangedMath -= UpdateRequested;
				base.ChangedParams -= UpdateRequested;
			}
			catch (MissingReferenceException)
			{
			}
		}

		private void UpdateData()
		{
			dataValid = true;
			Transform transform;
			try
			{
				transform = base.transform;
			}
			catch (MissingReferenceException)
			{
				RemoveListeners();
				return;
			}
			bool flag = true;
			try
			{
				flag = (base.Math == null || base.Math.Math == null || base.Math.Math.SectionsCount == 0);
			}
			catch (MissingReferenceException)
			{
			}
			positions.Clear();
			points.Clear();
			if (!flag)
			{
				if (splitter == null)
				{
					splitter = new BGPolylineSplitter();
				}
				if (config == null)
				{
					config = new BGPolylineSplitter.Config();
				}
				config.DoNotOptimizeStraightLines = doNotOptimizeStraightLines;
				config.SplitMode = splitMode;
				config.PartsTotal = partsTotal;
				config.PartsPerSection = partsPerSection;
				config.UseLocal = UseLocal;
				config.Transform = transform;
				splitter.Bind(positions, base.Math, config, points);
			}
		}

		protected virtual void UpdateRequested(object sender, EventArgs e)
		{
			InvalidateData();
		}
	}
}
