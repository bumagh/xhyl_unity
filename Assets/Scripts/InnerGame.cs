using Spine.Unity;

using System;
using UnityEngine;

[Serializable]
public class InnerGame
{

    public string name_cn;

	public string name_en;

	public string name_vn;


    [Header("spine动画")]
    public SkeletonDataAsset spine;

    public int id;

	public int rankType;

	public Sprite spriteIcon;

	[Header("游戏名图片中文")]
	public Sprite spiName;

    [Header("游戏名图片英文")]
    public Sprite spiNameEN;

    [Header("游戏名图片越南文")]
    public Sprite spiNameVN;


	public string plat_type;

	public string game_type;

	public string game_code = "dating";


	[Header("是否为热更新游戏")]
	public bool isOurGame;

	public bool isLuaGame;

	[Header("是否为网页游戏")]
	public bool isHttpGame = true;

	public string RunStatusKey;

	public int RunStatus;

	public string action;

	public string package;

	public string schema;

	[NonSerialized]
	public InnerGameState state;

	[NonSerialized]
	public int totalSize;

	[NonSerialized]
	public int savedSize;

	[NonSerialized]
	public float progress;

	[NonSerialized]
	public string url;

	[NonSerialized]
	public string version;

	[NonSerialized]
	public string latestVersion;

	[NonSerialized]
	public float speed;

	[NonSerialized]
	public float downloadDuration;

	[NonSerialized]
	public float downSize;
}
