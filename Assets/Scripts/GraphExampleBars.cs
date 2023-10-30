using Rapid.Tools;
using System.Diagnostics;
using UnityEngine;

public class GraphExampleBars : MonoBehaviour
{
	public Material RenderMaterial;

	public Color MainColor;

	public Color SubColor;

	public Color OutlineColor;

	public Color OverflowColor = Color.red;

	public Color RealValueIndicatorColor = Color.white;

	private GraphBar _deltaBar;

	private GraphBar _smoothDeltaBar;

	private GraphBar _bar;

	private Stopwatch stopwatch = new Stopwatch();

	public bool ScreenSpace
	{
		get
		{
			return _deltaBar.ScreenSpace;
		}
		set
		{
			GraphBar deltaBar = _deltaBar;
			GraphBar smoothDeltaBar = _smoothDeltaBar;
			_bar.ScreenSpace = value;
			smoothDeltaBar.ScreenSpace = value;
			deltaBar.ScreenSpace = value;
		}
	}

	private void Awake()
	{
		Camera main = Camera.main;
		MonoBehaviour.print("被我注销了");
		_deltaBar = new GraphBar(main, RenderMaterial)
		{
			Position = new Vector2(20f, 30f),
			MainColor = MainColor,
			SubColor = SubColor,
			OutlineColor = OutlineColor,
			OverflowColor = OverflowColor,
			RealValueColor = RealValueIndicatorColor
		};
		_smoothDeltaBar = new GraphBar(main, RenderMaterial)
		{
			Position = new Vector2(20f, 70f),
			MainColor = MainColor,
			SubColor = SubColor,
			OutlineColor = OutlineColor,
			OverflowColor = OverflowColor,
			RealValueColor = RealValueIndicatorColor
		};
		_bar = new GraphBar(main, RenderMaterial)
		{
			Position = new Vector2(20f, 110f),
			MainColor = MainColor,
			SubColor = SubColor,
			OutlineColor = OutlineColor,
			OverflowColor = OverflowColor,
			RealValueColor = RealValueIndicatorColor
		};
	}

	private void Update()
	{
		_deltaBar.Update(Time.deltaTime * 10f);
		_smoothDeltaBar.Update(Time.smoothDeltaTime * 10f);
		stopwatch.Reset();
		stopwatch.Start();
		PerformHeavyCalculations(UnityEngine.Random.Range(50000, 70000));
		stopwatch.Stop();
		_bar.Update((float)stopwatch.ElapsedMilliseconds / 10f);
	}

	private void PerformHeavyCalculations(int amount)
	{
		float num = 0f;
		for (int i = 0; i < amount; i++)
		{
			num += Mathf.Sin(i);
		}
	}

	private void OnPostRender()
	{
		_deltaBar.Draw();
		_smoothDeltaBar.Draw();
		_bar.Draw();
	}

	private void OnGUI()
	{
		ScreenSpace = GUILayout.Toggle(ScreenSpace, "Screen space");
		if (ScreenSpace)
		{
			Vector2 endPosition = _deltaBar.EndPosition;
			Rect position = new Rect(endPosition.x, 0f, 200f, 20f);
			float num = _deltaBar.Thickness * 0.5f;
			position.y = (float)Screen.height - _deltaBar.Position.y - num;
			GUI.Label(position, "<- Time.deltaTime * 10");
			position.y = (float)Screen.height - _smoothDeltaBar.Position.y - num;
			GUI.Label(position, "<- Time.smoothDeltaTime * 10");
			position.y = (float)Screen.height - _bar.Position.y - num;
			position.width = 400f;
			GUI.Label(position, "<- System.Diagnostics.Stopwatch measured time");
		}
	}
}
