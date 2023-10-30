using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcVisualizationLineRenderer")]
	[RequireComponent(typeof(LineRenderer))]
	[DisallowMultipleComponent]
	[CcDescriptor(Description = "Visualize curve with standard LineRenderer Unity component.", Name = "Cc Line Renderer", Icon = "BGCcVisualizationLineRenderer123")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcLineRenderer")]
	public class BGCcVisualizationLineRenderer : BGCcSplitterPolyline
	{
		[SerializeField]
		[Tooltip("Update LineRenderer at Start method.")]
		private bool updateAtStart;

		private LineRenderer lineRenderer;

		public bool UpdateAtStart
		{
			get
			{
				return updateAtStart;
			}
			set
			{
				updateAtStart = value;
			}
		}

		public override string Error => ChoseMessage(base.Error, () => (!(LineRenderer == null)) ? null : "LineRenderer is null");

		public override string Warning
		{
			get
			{
				string text = base.Warning;
				LineRenderer lineRenderer = LineRenderer;
				if (lineRenderer == null)
				{
					return text;
				}
				if (!lineRenderer.useWorldSpace)
				{
					text += "\r\nLineRenderer uses local space (LineRenderer.useWorldSpace=false)! This is not optimal, especially if you plan to update a curve at runtime. Try to set LineRenderer.useWorldSpace to true";
				}
				return (text.Length != 0) ? text : null;
			}
		}

		public override string Info => (!(lineRenderer != null)) ? "LineRenderer is null" : ("LineRenderer uses " + base.PointsCount + " points");

		public override bool SupportHandles => false;

		public LineRenderer LineRenderer
		{
			get
			{
				if (lineRenderer == null)
				{
					lineRenderer = GetComponent<LineRenderer>();
				}
				return lineRenderer;
			}
		}

		public event EventHandler ChangedVisualization;

		public override void Start()
		{
			base.Start();
			if (updateAtStart)
			{
				UpdateUI();
			}
			else
			{
				base.Math.EnsureMathIsCreated();
			}
		}

		public override void AddedInEditor()
		{
			UpdateUI();
		}

		public void UpdateUI()
		{
			try
			{
				if (base.Math == null)
				{
					return;
				}
			}
			catch (MissingReferenceException)
			{
				return;
			}
			BGCurveBaseMath math = base.Math.Math;
			if (math == null)
			{
				return;
			}
			LineRenderer lineRenderer;
			try
			{
				lineRenderer = LineRenderer;
			}
			catch (MissingReferenceException)
			{
				return;
			}
			if (lineRenderer == null)
			{
				return;
			}
			BGCurve curve = base.Curve;
			if (curve == null)
			{
				return;
			}
			if (math.SectionsCount == 0)
			{
				lineRenderer.positionCount = 0;
				if (base.positions != null && base.positions.Count > 0 && this.ChangedVisualization != null)
				{
					this.ChangedVisualization(this, null);
				}
				base.positions.Clear();
				return;
			}
			useLocal = !lineRenderer.useWorldSpace;
			List<Vector3> positions = base.Positions;
			int count = positions.Count;
			if (count > 0)
			{
				lineRenderer.positionCount = count;
				for (int i = 0; i < count; i++)
				{
					lineRenderer.SetPosition(i, positions[i]);
				}
			}
			else
			{
				lineRenderer.positionCount = 0;
			}
			if (this.ChangedVisualization != null)
			{
				this.ChangedVisualization(this, null);
			}
		}

		protected override void UpdateRequested(object sender, EventArgs e)
		{
			base.UpdateRequested(sender, e);
			UpdateUI();
		}
	}
}
