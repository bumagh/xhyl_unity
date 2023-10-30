using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class IPv4GUI : BaseGUI
	{
		private string value;

		private IntGUI x;

		private IntGUI y;

		private IntGUI z;

		private IntGUI w;

		public string Value
		{
			get
			{
				return value;
			}
			set
			{
				int[] array = ParseIPAddressText(value);
				x.Value = array[0];
				y.Value = array[1];
				z.Value = array[2];
				w.Value = array[3];
				this.value = array[0] + "." + array[1] + "." + array[2] + "." + array[3];
			}
		}

		public IPv4GUI(string title = null, bool boldTitle = false, string value = "0.0.0.0")
			: base(title, boldTitle)
		{
			int[] array = ParseIPAddressText(value);
			x = new IntGUI(null, boldTitle: false, array[0], 0);
			y = new IntGUI(null, boldTitle: false, array[1], 0);
			z = new IntGUI(null, boldTitle: false, array[2], 0);
			w = new IntGUI(null, boldTitle: false, array[3], 0);
			this.value = x.Value + "." + y.Value + "." + z.Value + "." + w.Value;
		}

		protected virtual int[] ParseIPAddressText(string ipAddressText)
		{
			string[] array = ipAddressText.Split('.');
			int[] array2 = new int[4];
			for (int i = 0; i < 4; i++)
			{
				array2[i] = 0;
			}
			for (int j = 0; j < array.Length && j < 4; j++)
			{
				int result = 0;
				int.TryParse(array[j], out result);
				array2[j] = result;
			}
			return array2;
		}

		public string Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			base.ShowTilte();
			UnityEngine.GUILayout.BeginHorizontal();
			int num = x.Show();
			UnityEngine.GUILayout.Label(".", GUILayout.LowerCenterAlignedLabelStyle);
			int num2 = y.Show();
			UnityEngine.GUILayout.Label(".", GUILayout.LowerCenterAlignedLabelStyle);
			int num3 = z.Show();
			UnityEngine.GUILayout.Label(".", GUILayout.LowerCenterAlignedLabelStyle);
			int num4 = w.Show();
			value = num + "." + num2 + "." + num3 + "." + num4;
			UnityEngine.GUILayout.EndHorizontal();
			UnityEngine.GUILayout.EndVertical();
			return value;
		}
	}
}
