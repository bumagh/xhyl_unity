using BansheeGz.BGSpline.Curve;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[CcDescriptor(Description = "Triangulate 2D spline. Currently only simple polygons are supported.", Name = "Triangulate 2D", Icon = "BGCcTriangulate2D123")]
	[DisallowMultipleComponent]
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcTriangulate2D")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcTriangulate2D")]
	public class BGCcTriangulate2D : BGCcSplitterPolyline
	{
		[SerializeField]
		[Tooltip("UV scale")]
		private Vector2 scaleUV = new Vector2(1f, 1f);

		[SerializeField]
		[Tooltip("UV offset")]
		private Vector2 offsetUV = new Vector2(0f, 0f);

		[Tooltip("Flip triangles")]
		[SerializeField]
		private bool flip;

		[Tooltip("Double sided")]
		[SerializeField]
		private bool doubleSided;

		[Tooltip("UV scale for back side")]
		[SerializeField]
		private Vector2 scaleBackUV = new Vector2(1f, 1f);

		[SerializeField]
		[Tooltip("UV offset for back side")]
		private Vector2 offsetBackUV = new Vector2(0f, 0f);

		[Tooltip("Update mesh every frame, even if curve's not changed. This can be useful, if UVs are animated.")]
		[SerializeField]
		private bool updateEveryFrame;

		private int updateAtFrame;

		private bool everyFrameUpdateIsRunning;

		[NonSerialized]
		private MeshFilter meshFilter;

		[NonSerialized]
		private BGTriangulator2D triangulator;

		public Vector2 ScaleUv
		{
			get
			{
				return scaleUV;
			}
			set
			{
				if (!(Mathf.Abs(scaleUV.x - value.x) < 1E-05f) || !(Mathf.Abs(scaleUV.y - value.y) < 1E-05f))
				{
					ParamChanged(ref scaleUV, value);
				}
			}
		}

		public bool Flip
		{
			get
			{
				return flip;
			}
			set
			{
				if (flip != value)
				{
					ParamChanged(ref flip, value);
				}
			}
		}

		public bool DoubleSided
		{
			get
			{
				return doubleSided;
			}
			set
			{
				ParamChanged(ref doubleSided, value);
			}
		}

		public bool UpdateEveryFrame
		{
			get
			{
				return updateEveryFrame;
			}
			set
			{
				if (updateEveryFrame != value)
				{
					updateEveryFrame = value;
					ParamChanged(ref updateEveryFrame, value);
					if (updateEveryFrame && !everyFrameUpdateIsRunning && base.gameObject.activeSelf && Application.isPlaying)
					{
						StartCoroutine(UiUpdater());
					}
				}
			}
		}

		public override bool UseLocal => true;

		public override string Error => base.Curve.Mode2DOn ? null : "Curve should be in 2D mode";

		public override string Info
		{
			get
			{
				MeshFilter meshFilter = MeshFilter;
				if (meshFilter == null)
				{
					return "No data.";
				}
				Mesh sharedMesh = meshFilter.sharedMesh;
				if (sharedMesh == null)
				{
					return "No data.";
				}
				return "Mesh uses " + sharedMesh.vertexCount + " vertices and " + sharedMesh.triangles.Length / 3 + " triangles.";
			}
		}

		public MeshFilter MeshFilter
		{
			get
			{
				if (meshFilter == null)
				{
					meshFilter = GetComponent<MeshFilter>();
				}
				return meshFilter;
			}
		}

		public override void Start()
		{
			base.Start();
			if (MeshFilter.sharedMesh == null)
			{
				UpdateUI();
			}
			if (updateEveryFrame && base.gameObject.activeSelf && Application.isPlaying)
			{
				StartCoroutine(UiUpdater());
			}
		}

		private void OnEnable()
		{
			if (updateEveryFrame && !everyFrameUpdateIsRunning && Application.isPlaying)
			{
				StartCoroutine(UiUpdater());
			}
		}

		private void OnDisable()
		{
			if (updateEveryFrame && everyFrameUpdateIsRunning && Application.isPlaying)
			{
				everyFrameUpdateIsRunning = false;
			}
		}

		public void UpdateUI()
		{
			updateAtFrame = Time.frameCount;
			if (base.Curve.Mode2DOn)
			{
				List<Vector3> positions = base.Positions;
				MeshFilter meshFilter;
				try
				{
					meshFilter = MeshFilter;
				}
				catch (MissingReferenceException)
				{
					RemoveListeners();
					return;
				}
				Mesh mesh = meshFilter.mesh;
				if (mesh == null)
				{
					mesh = (meshFilter.mesh = new Mesh());
				}
				if (triangulator == null)
				{
					triangulator = new BGTriangulator2D();
				}
				triangulator.Bind(mesh, positions, new BGTriangulator2D.Config
				{
					Closed = base.Curve.Closed,
					Mode2D = base.Curve.Mode2D,
					Flip = flip,
					ScaleUV = scaleUV,
					OffsetUV = offsetUV,
					DoubleSided = doubleSided,
					ScaleBackUV = scaleBackUV,
					OffsetBackUV = offsetBackUV
				});
			}
		}

		protected override void UpdateRequested(object sender, EventArgs e)
		{
			base.UpdateRequested(sender, e);
			UpdateUI();
		}

		private IEnumerator UiUpdater()
		{
			everyFrameUpdateIsRunning = true;
			while (updateEveryFrame)
			{
				if (updateAtFrame != Time.frameCount)
				{
					UpdateUI();
				}
				yield return null;
			}
			everyFrameUpdateIsRunning = false;
		}
	}
}
