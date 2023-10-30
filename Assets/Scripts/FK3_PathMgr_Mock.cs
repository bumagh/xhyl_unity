using SWS;
using System.Collections.Generic;
using UnityEngine;

public class FK3_PathMgr_Mock : MonoBehaviour
{
	private static FK3_PathMgr_Mock _instance;

	public List<PathManager> pathList = new List<PathManager>();

	public Dictionary<string, PathManager> pathDic;

	public static FK3_PathMgr_Mock Get()
	{
		return _instance;
	}

	private void Awake()
	{
		_instance = this;
		pathDic = new Dictionary<string, PathManager>();
		Init();
	}

	private void Init()
	{
		foreach (PathManager path in pathList)
		{
			pathDic.Add(path.name, path);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
