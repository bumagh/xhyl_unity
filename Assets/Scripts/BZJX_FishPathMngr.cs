using System.Collections;
using UnityEngine;

public class BZJX_FishPathMngr : MonoBehaviour
{
	public static BZJX_FishPathMngr G_FishPathMngr;

	private BZJX_PathManager[] allFishPath;

	private BZJX_PathManager[] smallFishPath;

	private int[] _PathNonRepeat;

	private ArrayList _UsableFishPathList = new ArrayList();

	private ArrayList _UnUsableFishPathList = new ArrayList();

	private int _AllPathLength;

	private string[] strAllPath = new string[88]
	{
		"-9.053246,-5.375661,1 -1.94571,0.6451483,1 3.581036,-2.859877,1 7.192537,1.372771,1 8.846195,5.059741,1",
		"-8.417452,-6.319533,1 0.1612186,-0.8215675,1 9.282089,4.365077,1",
		"-7.51765,-6.408392,1 2.075293,-0.5649152,1 9.76981,3.605391,1",
		"-9.168772,-4.629575,1 -0.438848,0.961241,1 8.381762,6.035608,1",
		"-9.203857,-3.500474,1 -0.7389474,1.568567,1 10.78351,5.418996,1",
		"-5.281365,-6.572134,1 1.24999,0.9916725,1 10.44555,4.549338,1",
		"-9.087809,-4.0427,1 -0.5894527,-1.159135,1 10.33281,3.868065,1",
		"-4.722266,-6.635246,1 1.628587,0.278676,1 6.324905,0.5928154,1 10.48571,2.944265,1",
		"-9.175189,-0.8831686,1 -1.128472,-2.15621,1 4.844234,-0.8431319,1 8.189156,5.298454,1 10.15465,6.242857,1",
		"-9.305482,-1.934118,1 -3.425798,-3.253732,1 -1.000333,0.1099725,1 3.951473,1.530609,1 10.3323,1.811576,1",
		"-9.184858,-1.671867,1 -4.02659,-1.686127,1 -0.4396563,0.8561642,1 4.117493,1.08095,1 8.999068,5.059877,1 10.05081,5.490676,1",
		"-5.670007,-6.700662,1 -5.347782,-1.244894,1 -3.243857,1.603301,1 2.392094,2.694188,1 10.85962,4.833958,1",
		"-9.2865,-3.00381,1 -1.950543,-2.811057,1 1.231843,-0.6207955,1 4.602608,-0.03693628,1 10.77849,3.827408,1",
		"-9.108993,-5.95398,1 -4.646976,-2.527048,1 -4.241901,0.1381288,1 -0.8979235,1.456864,1 2.553401,0.54356,1 5.088662,2.954418,1 9.45771,6.0648,1",
		"-7.786317,-6.201888,1 -0.8720481,-3.3057,1 1.015844,-1.817241,1 1.205452,1.93745,1 10.91375,5.758997,1",
		"-4.790679,-6.674663,1 -0.06286645,-2.302335,1 0.638819,0.3070889,1 4.037184,3.666646,1 8.190424,6.369721,1",
		"-6.24044,-6.560896,1 -2.297866,-1.257499,1 3.150308,1.900231,1 10.80923,2.407549,1",
		"-9.050541,-1.934931,1 -3.969206,1.608059,1 -1.002547,2.600645,1 5.28064,1.372874,1 10.59195,-4.367733,1",
		"-7.120005,-6.678612,1 -4.967554,-0.7819595,1 -2.270458,0.6745729,1 1.343405,1.582395,1 7.088586,6.289902,1",
		"-8.789673,-5.848773,1 -3.296025,-1.35937,1 1.531142,-1.135128,1 5.010434,-0.2991624,1 6.244188,6.339003,1",
		"-9.156652,-0.1315292,1 -2.661464,2.264122,1 4.515964,0.8265367,1 10.15422,-5.335656,1",
		"-8.490602,-0.3352807,1 -2.688921,0.1960242,1 1.844424,0.3007139,1 10.37134,0.21452,1",
		"9.186223,6.051419,1 -0.4853106,0.6301694,1 -7.895724,-2.831876,1 -10.70652,-3.910094,1",
		"11.01676,5.512065,1 -1.195365,-0.789129,1 -10.67839,-5.045696,1",
		"10.33758,4.583398,1 0.1673117,-0.8461988,1 -9.74167,-5.642054,1",
		"10.6168,3.64047,1 -9.542789,-4.960751,1",
		"7.100655,7.884793,1 -0.5141563,-0.361722,1 -4.772697,-0.9570398,1 -10.53591,-1.694062,1",
		"10.68932,0.9461635,1 4.114129,2.219072,1 -4.346508,-0.4478436,1 -10.22401,-4.392807,1",
		"4.37024,6.709569,1 4.425939,0.3741603,1 -0.3725505,-2.18625,1 -10.30863,-1.979702,1",
		"10.3189,0.2027626,1 2.552353,0.629602,1 -0.8825679,1.481925,1 -10.96189,-3.313896,1",
		"3.433423,6.676543,1 3.34756,1.963744,1 1.955776,-1.130441,1 -8.634529,-5.840994,1",
		"2.723676,6.549915,1 2.723059,2.24776,1 -2.416161,-1.016006,1 -10.59222,-0.6453762,1",
		"7.619529,6.672798,1 5.277498,-0.4213305,1 -4.006506,1.78131,1 -10.081,0.2630062,1",
		"9.862785,-1.747257,1 2.950058,1.736682,1 -0.4849105,2.390305,1 -4.630082,0.9716489,1 -10.87629,-1.411831,1",
		"5.769049,6.357724,1 1.927999,1.566546,1 -3.60917,-3.911479,1 -6.732486,-6.040092,1",
		"8.352898,5.593095,1 4.135113,1.798083,1 -1.983588,0.9553471,1 -4.621512,-1.857235,1 -4.938994,-6.287831,1",
		"9.33737,5.274627,1 6.420657,1.340509,1 4.662045,-0.5228238,1 1.813185,-0.5014486,1 -4.024015,-1.844975,1 -6.344028,0.3935781,1 -9.261812,3.980845,1 -10.49226,5.246979,1",
		"10.14476,0.2861996,1 3.080438,2.8532,1 -2.26493,0.8147472,1 -5.253697,1.518603,1 -9.896448,-3.438574,1",
		"10.03283,4.511064,1 8.490685,2.79681,1 2.498789,2.351901,1 -1.355808,1.412636,1 -2.863455,-1.528393,1 -7.559895,-6.425759,1",
		"10.67542,2.829697,1 6.137658,2.718621,1 1.785671,1.603313,1 0.0369339,-1.185977,1 -5.207849,-2.896275,1 -10.26654,-4.048697,1",
		"8.221323,6.363315,1 5.245133,3.388196,1 2.343368,0.5618443,1 -1.079062,-1.966906,1 -8.146811,-5.722623,1",
		"5.096996,6.400856,1 5.133516,3.239435,1 3.831273,1.156768,1 0.7809324,-0.7025456,1 -5.803675,-6.28078,1",
		"10.22888,1.899917,1 4.463634,1.417048,1 -1.748007,0.6738777,1 -10.60079,-2.039024,1",
		"6.809719,6.328453,1 4.056272,1.86526,1 1.93544,-1.258965,1 -9.261387,-5.201087,1",
		"-8.962481,6.217966,1 -1.190038,0.5266671,1 10.48785,-5.053474,1",
		"-9.185862,5.213738,1 1.562409,0.786818,1 10.26445,-6.132089,1",
		"-7.660666,6.254934,1 -4.165015,3.279264,1 5.393021,-1.221957,1 10.52536,-3.528515,1",
		"-6.693607,6.254989,1 10.48835,-2.71011,1",
		"-9.483615,4.283779,1 9.074201,-6.280649,1",
		"-9.074409,4.581428,1 -5.429747,2.721316,1 2.38117,3.055602,1 6.248247,-2.375041,1 8.069954,-6.243506,1",
		"-9.483817,3.316731,1 1.079221,2.386206,1 10.52574,-1.780257,1",
		"-9.558428,2.275427,1 -2.975233,1.19617,1 4.79751,-3.081622,1 7.400423,-6.392233,1",
		"-8.255769,6.292302,1 -5.020936,1.159256,1 1.487987,0.637918,1 3.347032,-2.59799,1 6.693807,-6.020236,1",
		"-5.726528,6.366502,1 -5.243766,2.758497,1 -2.491916,0.192018,1 4.42613,-0.4037395,1 10.60027,-1.073702,1",
		"-4.982655,6.292054,1 -3.160836,2.94431,1 -3.830899,0.2665071,1 -3.199209,-1.204945,1 0.1109333,-0.9928246,1 10.34003,-0.5157685,1",
		"-9.706574,5.250845,1 -6.136662,1.679932,1 -2.826987,-1.33292,1 0.5947323,-2.15145,1 6.135828,-6.317621,1",
		"-8.441922,5.43685,1 -4.127781,3.465101,1 1.897388,1.865426,1 5.356449,-6.094352,1",
		"-9.297196,1.345095,1 -4.052289,2.646676,1 -0.7421036,1.41913,1 1.554868,-1.865222,1 9.969129,-4.309221,1",
		"-9.632107,0.6756115,1 -3.234344,1.568615,1 -1.326201,-1.857543,1 9.857704,-3.528139,1",
		"-9.81756,3.167634,1 -6.024008,1.084599,1 -3.830083,-0.8820791,1 -0.4082165,-1.294259,1 10.37874,-2.114781,1",
		"-7.473534,6.366239,1 -4.089286,3.576531,1 -2.490697,-0.3661208,1 1.749475,-1.184567,1 10.4156,-3.751328,1",
		"-9.854887,2.535331,1 -4.983154,-1.928173,1 2.493402,-1.147406,1 10.26777,0.7863708,1",
		"-9.780092,4.432234,1 -5.949229,2.907112,1 -4.648015,-0.142869,1 -0.258606,1.195943,1 5.990066,0.005469799,1 10.15686,3.947888,1",
		"-10.22706,1.531104,1 -4.164715,-1.333098,1 -0.7048216,1.828265,1 3.684177,1.232974,1 10.23078,1.753426,1",
		"-9.892159,2.163391,1 -5.614621,2.163215,1 -0.8536711,1.530716,1 3.348666,-2.30046,1 10.11875,-0.3294489,1",
		"-10.00418,0.1177168,1 -4.163743,3.278981,1 1.006334,2.497687,1 2.939675,-1.519366,1 4.352229,-6.280265,1",
		"10.23781,-6.002087,1 -0.6791105,0.1537819,1 -8.738279,6.162977,1",
		"9.028741,-6.038677,1 -9.507907,5.100466,1",
		"10.53106,-5.342588,1 -9.361657,3.671515,1",
		"10.60452,-4.49988,1 -2.254857,-1.238457,1 -9.691585,2.828819,1",
		"10.6047,-3.620532,1 0.640048,0.9964397,1 -9.288025,5.356932,1",
		"7.966144,-6.441673,1 3.570806,-0.4692569,1 -1.081566,2.82848,1 -9.545244,1.802911,1",
		"5.694658,-6.001904,1 5.145927,-2.008179,1 1.885821,1.326146,1 -4.489129,1.912627,1 -9.58214,0.5938082,1",
		"10.34842,-2.741173,1 1.225484,-2.667534,1 -4.452972,-0.3590221,1 -9.50901,-0.1023438,1",
		"10.56846,-1.751917,1 0.3830462,-1.531675,1 -3.279852,2.901849,1 -8.262133,5.430172,1",
		"6.500799,-5.525625,1 2.214782,-2.37446,1 2.032428,1.582617,1 -9.398814,1.216673,1",
		"10.31212,-1.129034,1 4.267067,0.1535889,1 1.519715,2.645182,1 -4.342792,0.8867192,1 -9.765729,-1.2748,1",
		"7.013547,-6.441632,1 5.109173,-2.55777,1 4.743381,0.2268457,1 -2.657577,0.1905003,1 -9.398145,4.404305,1",
		"9.871908,-3.730424,1 4.596703,-0.3593769,1 1.116538,1.912405,1 -3.573762,-0.8720121,1 -9.656084,-2.557188,1",
		"9.321881,-5.892131,1 6.098722,-1.092231,1 0.6032219,0.1170921,1 -5.149187,-0.7620311,1 -9.252864,-1.604575,1",
		"8.112814,-5.892083,1 5.219297,-1.568509,1 0.7137814,3.158165,1 -8.922184,2.792153,1",
		"6.244131,-6.478241,1 5.329058,-2.301305,1 4.70682,0.5932446,1 1.59288,2.095589,1 -2.071533,-0.6155925,1 -6.137888,1.766136,1 -9.142589,0.0808363,1",
		"4.632168,-5.891946,1 4.156686,-2.044779,1 4.487053,0.8863711,1 1.922678,2.352051,1 -2.73068,0.9965749,1 -9.655039,2.389145,1",
		"9.651916,-4.499843,1 3.460402,-2.777541,1 2.874895,0.556675,1 -1.631926,-0.8720827,1 -7.932248,6.089671,1",
		"3.423016,-6.294932,1 4.962583,-2.740962,1 4.633499,0.3734131,1 0.8234098,1.802499,1 -9.472458,-0.5053825,1",
		"9.689548,0.1900129,1 4.523239,-1.238724,1 -1.04534,0.8865889,1 -9.43642,-3.363264,1",
		"10.23901,-0.3595996,1 2.837603,-2.521039,1 -2.951349,-2.960485,1 -3.829908,0.6302202,1 -5.624087,5.833101,1",
		"4.852066,-5.562198,1 3.314669,1.106254,1 -0.7520132,1.912483,1 -4.855218,3.304946,1 -6.979621,6.236188,1"
	};

	public static BZJX_FishPathMngr GetSingleton()
	{
		return G_FishPathMngr;
	}

	public int GetPathCount()
	{
		return _AllPathLength;
	}

	private void Awake()
	{
		if (G_FishPathMngr == null)
		{
			G_FishPathMngr = this;
		}
		int num = 0;
		_AllPathLength = 88;
		allFishPath = new BZJX_PathManager[_AllPathLength];
		_PathNonRepeat = new int[_AllPathLength];
		for (int i = 0; i < _AllPathLength; i++)
		{
			_UsableFishPathList.Add(num);
			_PathNonRepeat[num] = 0;
			string[] array = strAllPath[i].Split(' ');
			int num2 = array.Length;
			allFishPath[i] = new BZJX_PathManager();
			allFishPath[i].vecs = new Vector3[num2];
			for (int j = 0; j < num2; j++)
			{
				allFishPath[i].vecs[j] = Vector3.zero;
				string[] array2 = array[j].Split(',');
				allFishPath[i].vecs[j][0] = float.Parse(array2[0]);
				allFishPath[i].vecs[j][1] = float.Parse(array2[1]);
				allFishPath[i].vecs[j][2] = float.Parse(array2[2]);
			}
		}
		smallFishPath = new BZJX_PathManager[_AllPathLength * 4];
		for (int k = 0; k < _AllPathLength; k++)
		{
			if (k % 3 == 0)
			{
				smallFishPath[k * 4] = ChangePath(allFishPath[k], Vector3.up * 0.4f);
				smallFishPath[k * 4 + 1] = ChangePath(allFishPath[k], Vector3.down * 0.6f);
				smallFishPath[k * 4 + 2] = ChangePath(allFishPath[k], Vector3.up * 0.2f + Vector3.left * 0.2f);
				smallFishPath[k * 4 + 3] = ChangePath(allFishPath[k], Vector3.down * 0.6f + Vector3.right * 0.6f);
			}
			else if (k % 4 == 0)
			{
				smallFishPath[k * 4] = ChangePath(allFishPath[k], Vector3.up * 0.6f);
				smallFishPath[k * 4 + 1] = ChangePath(allFishPath[k], Vector3.down * 0.6f);
				smallFishPath[k * 4 + 2] = ChangePath(allFishPath[k], Vector3.left * 0.3f);
				smallFishPath[k * 4 + 3] = ChangePath(allFishPath[k], Vector3.down * 0.2f + Vector3.right * 0.2f);
			}
			else if (k % 5 == 0)
			{
				smallFishPath[k * 4] = ChangePath(allFishPath[k], Vector3.up * 0.3f);
				smallFishPath[k * 4 + 1] = ChangePath(allFishPath[k], Vector3.down * 0.9f);
				smallFishPath[k * 4 + 2] = ChangePath(allFishPath[k], Vector3.left * 0.4f);
				smallFishPath[k * 4 + 3] = ChangePath(allFishPath[k], Vector3.down * 0.3f + Vector3.right * 0.2f);
			}
			else if (k % 7 == 0)
			{
				smallFishPath[k * 4] = ChangePath(allFishPath[k], Vector3.up * 0.8f);
				smallFishPath[k * 4 + 1] = ChangePath(allFishPath[k], Vector3.down * 0.6f);
				smallFishPath[k * 4 + 2] = ChangePath(allFishPath[k], Vector3.left * 0.2f);
				smallFishPath[k * 4 + 3] = ChangePath(allFishPath[k], Vector3.right * 0.6f);
			}
			else
			{
				smallFishPath[k * 4] = ChangePath(allFishPath[k], Vector3.up * 0.6f);
				smallFishPath[k * 4 + 1] = ChangePath(allFishPath[k], Vector3.down * 0.7f);
				smallFishPath[k * 4 + 2] = ChangePath(allFishPath[k], Vector3.up * 0.2f + Vector3.left * 0.4f);
				smallFishPath[k * 4 + 3] = ChangePath(allFishPath[k], Vector3.down * 0.2f + Vector3.right * 0.2f);
			}
		}
	}

	private void updateUnusablePath()
	{
		for (int i = 0; i < _UnUsableFishPathList.Count; i++)
		{
			int num = (int)_UnUsableFishPathList[i];
			_PathNonRepeat[num]--;
			if (_PathNonRepeat[num] <= 0)
			{
				_PathNonRepeat[num] = 0;
				_UsableFishPathList.Add(num);
				_UnUsableFishPathList.RemoveAt(i);
			}
		}
	}

	public int GetRandomPath()
	{
		updateUnusablePath();
		if (_UsableFishPathList.Count >= 1)
		{
			int index = UnityEngine.Random.Range(0, _UsableFishPathList.Count);
			int num = (int)_UsableFishPathList[index];
			_PathNonRepeat[num] = 60;
			_UsableFishPathList.RemoveAt(index);
			_UnUsableFishPathList.Add(num);
			return num;
		}
		return UnityEngine.Random.Range(0, _AllPathLength);
	}

	public BZJX_PathManager GetFishPath(int type)
	{
		return allFishPath[type];
	}

	public BZJX_PathManager GetSmallFishPath(int type)
	{
		return smallFishPath[type];
	}

	private BZJX_PathManager ChangePath(BZJX_PathManager path, Vector3 vec)
	{
		int num = path.vecs.Length;
		BZJX_PathManager bZJX_PathManager = new BZJX_PathManager();
		bZJX_PathManager.vecs = new Vector3[num];
		for (int i = 0; i < num; i++)
		{
			bZJX_PathManager.vecs[i] = path.vecs[i] + vec;
		}
		return bZJX_PathManager;
	}

	public void GetAllPath(int type)
	{
		BZJX_PathManager bZJX_PathManager = smallFishPath[type];
		int num = bZJX_PathManager.vecs.Length;
		string text = string.Empty;
		for (int i = 0; i < num; i++)
		{
			string text2 = text;
			text = text2 + bZJX_PathManager.vecs[i][0].ToString() + "," + bZJX_PathManager.vecs[i][1].ToString() + "," + bZJX_PathManager.vecs[i][2].ToString() + " ";
		}
		MonoBehaviour.print(text);
	}
}
