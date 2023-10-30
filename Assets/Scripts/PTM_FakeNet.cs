using JsonFx.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTM_FakeNet
{
	public delegate void ResponseHandler(object[] args);

	private PTM_NetManager _net;

	private Queue<PTM_NetMessage> _netMsgQueue;

	private Dictionary<string, string> _reqToRespMap = new Dictionary<string, string>();

	private Dictionary<string, ResponseHandler> _handerMap = new Dictionary<string, ResponseHandler>();

	public PTM_FakeNet(PTM_NetManager net, Queue<PTM_NetMessage> netMsgQueue)
	{
		_net = net;
		_netMsgQueue = netMsgQueue;
	}

	public void RegisterHandler(string reqMethod, string respMethod, ResponseHandler hander)
	{
		_reqToRespMap.Add(reqMethod, respMethod);
		if (!_handerMap.ContainsKey(respMethod))
		{
			_handerMap.Add(respMethod, hander);
		}
	}

	public void Init()
	{
		RegisterHandler("userService/userLogin", "userLogin", delegate
		{
			Dictionary<string, object> dictionary8 = new Dictionary<string, object>
			{
				{
					"isLogin",
					true
				},
				{
					"user",
					new Dictionary<string, object>
					{
						{
							"id",
							12345
						},
						{
							"username",
							"FakeNet"
						},
						{
							"nickname",
							"FakeNet nick"
						},
						{
							"sex",
							"shemale"
						},
						{
							"level",
							10
						},
						{
							"gameGold",
							19999
						},
						{
							"expeGold",
							999
						},
						{
							"photoId",
							3
						},
						{
							"overflow",
							0
						},
						{
							"type",
							0
						},
						{
							"promoterName",
							"FakeNet推广员"
						}
					}
				},
				{
					"special",
					false
				}
			};
			_netMsgQueue.Enqueue(new PTM_NetMessage("userLogin", new object[1]
			{
				dictionary8
			}));
		});
		RegisterHandler("userService/enterRoom", "roomInfo", delegate
		{
			List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
			for (int i = 1; i <= 11; i++)
			{
				Dictionary<string, object> dictionary7 = new Dictionary<string, object>
				{
					{
						"id",
						i
					},
					{
						"name",
						"net桌子" + i
					},
					{
						"roomId",
						2
					},
					{
						"minGold",
						10
					},
					{
						"maxSinglelineBet",
						80
					},
					{
						"minSinglelineBet",
						10
					},
					{
						"singlechangeScore",
						30
					},
					{
						"exchange",
						10
					},
					{
						"onceExchangeValue",
						100
					},
					{
						"diceSwitch",
						1
					},
					{
						"diceDirectSwitch",
						1
					},
					{
						"diceOverflow",
						50000
					}
				};
				bool flag2 = i <= 3;
				dictionary7.Add("full", flag2);
				if (flag2)
				{
					dictionary7.Add("userId", i + 20000);
					dictionary7.Add("userPhotoId", i);
					dictionary7.Add("nickname", "fake占位" + i);
				}
				else
				{
					dictionary7.Add("userId", -1);
					dictionary7.Add("userPhotoId", -1);
					dictionary7.Add("nickname", string.Empty);
				}
				list.Add(dictionary7);
			}
			_netMsgQueue.Enqueue(new PTM_NetMessage("roomInfo", new object[1]
			{
				list.ToArray()
			}));
		});
		RegisterHandler("userService/leaveRoom", "empty", delegate
		{
		});
		RegisterHandler("userService/leaveDesk", "empty", delegate
		{
		});
		RegisterHandler("userService/heart", "empty", delegate
		{
		});
		RegisterHandler("userService/enterDesk", "enterDesk", delegate
		{
			Dictionary<string, object> value8 = new Dictionary<string, object>
			{
				{
					"success",
					true
				},
				{
					"lastResult",
					new int[3][]
					{
						new int[5]
						{
							1,
							2,
							3,
							4,
							5
						},
						new int[5]
						{
							1,
							2,
							3,
							4,
							5
						},
						new int[5]
						{
							1,
							2,
							3,
							4,
							5
						}
					}
				},
				{
					"betRecord",
					new int[3]
					{
						1,
						0,
						-1
					}
				}
			};
			UnityEngine.Debug.Log(JsonWriter.Serialize(value8));
			value8 = (JsonReader.Deserialize(JsonWriter.Serialize(value8)) as Dictionary<string, object>);
			_netMsgQueue.Enqueue(new PTM_NetMessage("enterDesk", new object[1]
			{
				value8
			}));
		});
		RegisterHandler("userService/gameStart", "gameResult", delegate
		{
			UnityEngine.Debug.Log("gameResult+++=");
			Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
			int[,] randomMatrix3x = PTM_Utils.GetRandomMatrix3x5();
			dictionary5.Add("gameContent", randomMatrix3x);
			dictionary5.Add("totalWin", 100);
			dictionary5.Add("multipGame", true);
			bool flag = false;
			dictionary5.Add("m_IsFree", flag);
			dictionary5.Add("times", 3);
			dictionary5.Add("specialAward", 0);
			UnityEngine.Debug.Log(JsonWriter.Serialize(dictionary5));
			_netMsgQueue.Enqueue(new PTM_NetMessage("gameResult", new object[1]
			{
				dictionary5
			}));
			if (flag)
			{
				Dictionary<string, object> dictionary6 = new Dictionary<string, object>();
				int[] value5 = new int[7]
				{
					1,
					5,
					12,
					10,
					18,
					20,
					0
				};
				int[,] value6 = new int[7, 4]
				{
					{
						1,
						2,
						3,
						4
					},
					{
						5,
						6,
						7,
						8
					},
					{
						1,
						2,
						3,
						4
					},
					{
						5,
						6,
						7,
						8
					},
					{
						1,
						2,
						3,
						4
					},
					{
						5,
						6,
						7,
						8
					},
					{
						1,
						2,
						3,
						4
					}
				};
				int[] value7 = new int[3]
				{
					0,
					4000,
					5000
				};
				int num5 = 12000;
				int num6 = 10000;
				dictionary6.Add("photoNumber", value5);
				dictionary6.Add("photos", value6);
				dictionary6.Add("totalWin", value7);
				dictionary6.Add("credit", num5);
				dictionary6.Add("totalBet", num6);
				_netMsgQueue.Enqueue(new PTM_NetMessage("maryResult", new object[1]
				{
					dictionary6
				}));
			}
		});
		RegisterHandler("userService/multipleInfo", "multipResult", delegate
		{
			Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
			int[] value4 = new int[2]
			{
				1,
				6
			};
			dictionary4.Add("number", value4);
			dictionary4.Add("totalWin", 2000);
			dictionary4.Add("overflow", false);
			dictionary4 = (JsonReader.Deserialize(JsonWriter.Serialize(dictionary4)) as Dictionary<string, object>);
			_netMsgQueue.Enqueue(new PTM_NetMessage("multipResult", new object[1]
			{
				dictionary4
			}));
		});
		RegisterHandler("userService/maryStart", "maryResult", delegate
		{
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
			int[] value = new int[3]
			{
				1,
				5,
				12
			};
			int[][] value2 = new int[3][]
			{
				new int[4]
				{
					1,
					2,
					3,
					4
				},
				new int[4]
				{
					5,
					6,
					7,
					8
				},
				new int[4]
				{
					1,
					1,
					1,
					2
				}
			};
			int[] value3 = new int[3]
			{
				0,
				4000,
				5000
			};
			int num3 = 12000;
			int num4 = 10000;
			dictionary3.Add("photoNumber", value);
			dictionary3.Add("photos", value2);
			dictionary3.Add("totalWin", value3);
			dictionary3.Add("credit", num3);
			dictionary3.Add("totalBet", num4);
			_netMsgQueue.Enqueue(new PTM_NetMessage("maryResult", new object[1]
			{
				dictionary3
			}));
		});
		RegisterHandler("userService/userCoinIn", "updateGoldAndScore1", delegate(object[] args)
		{
			int num2 = (int)args[0];
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>
			{
				{
					"gold",
					2000
				},
				{
					"gameScore",
					20000
				}
			};
			_netMsgQueue.Enqueue(new PTM_NetMessage("updateGoldAndScore", new object[1]
			{
				dictionary2
			}));
		});
		RegisterHandler("userService/userCoinOut", "updateGoldAndScore2", delegate(object[] args)
		{
			int num = (int)args[0];
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"gold",
					2100
				},
				{
					"gameScore",
					21000
				}
			};
			_netMsgQueue.Enqueue(new PTM_NetMessage("updateGoldAndScore", new object[1]
			{
				dictionary
			}));
		});
	}

	public IEnumerator MakeResponse(string reqMethod, object[] args)
	{
		yield return new WaitForSeconds(0.1f);
		if (_reqToRespMap.ContainsKey(reqMethod))
		{
			string text = _reqToRespMap[reqMethod];
			if (text != "empty")
			{
				ResponseHandler responseHandler = _handerMap[text];
				responseHandler(args);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("unregister method: " + reqMethod);
		}
	}
}
