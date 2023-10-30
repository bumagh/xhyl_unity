using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeNet
{
	public delegate void ResponseHandler(object[] args);

	private NetManager _net;

	private Queue<NetMessage> _netMsgQueue;

	private Dictionary<string, string> _reqToRespMap = new Dictionary<string, string>();

	private Dictionary<string, ResponseHandler> _handerMap = new Dictionary<string, ResponseHandler>();

	public FakeNet(NetManager net, Queue<NetMessage> netMsgQueue)
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
		RegisterHandler("gcuserService/login", "login", delegate
		{
			Dictionary<string, object> dictionary5 = new Dictionary<string, object>
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
							"username",
							"FakeNet"
						},
						{
							"nickname",
							"FakeNet nick"
						},
						{
							"phone",
							"ddddddd"
						},
						{
							"sex",
							"dd"
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
							"lottery",
							5623
						},
						{
							"expeGold",
							999
						},
						{
							"security",
							1
						},
						{
							"safeBox",
							1
						},
						{
							"photoId",
							3
						},
						{
							"overflow",
							4
						},
						{
							"type",
							0
						}
					}
				}
			};
			_netMsgQueue.Enqueue(new NetMessage("login", new object[1]
			{
				dictionary5
			}));
		});
		RegisterHandler("userService/heart", "empty", delegate
		{
		});
		RegisterHandler("gcuserService/gameranktype", "gameranktype", delegate(object[] args)
		{
			int num3 = (int)args[0];
			Dictionary<string, object> dictionary4 = new Dictionary<string, object>
			{
				{
					"ranktype",
					num3
				}
			};
			if (num3 < 2)
			{
				List<TopRank> list3 = new List<TopRank>();
				for (int m = 0; m < 4; m++)
				{
					TopRank item = new TopRank();
					list3.Add(item);
				}
				dictionary4.Add("TrList", list3);
			}
			else
			{
				List<TopRank> list4 = new List<TopRank>();
				for (int n = 0; n < 6; n++)
				{
					list4.Add(new TopRank
					{
						nickname = "zhanghe" + n,
						gold = n * 10,
						awardName = "quanpanjiang",
						datetime = "2016.05.0" + n
					});
				}
				dictionary4.Add("TrList", list4);
			}
			_netMsgQueue.Enqueue(new NetMessage("gameranktype", new object[1]
			{
				dictionary4
			}));
		});
		List<Campaign> trCamp = new List<Campaign>();
		for (int i = 0; i < 8; i++)
		{
			Campaign campaign = new Campaign();
			campaign.Id = i;
			campaign.content = "neirong" + i + "type";
			campaign.status = 0;
			campaign.userSchedule = 50;
			campaign.targetSchedule = 100;
			campaign.awardGold = 50;
			trCamp.Add(campaign);
		}
		RegisterHandler("gcuserService/campagin", "campagin", delegate(object[] args)
		{
			int num2 = (int)args[0];
			UnityEngine.Debug.Log("type: " + num2);
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
			for (int l = 0; l < 8; l++)
			{
				trCamp[l].content = "neirong" + l + "type" + num2;
			}
			trCamp[2].status = 1;
			trCamp[3].status = 1;
			dictionary3.Add("campagin", trCamp);
			_netMsgQueue.Enqueue(new NetMessage("campagin", new object[1]
			{
				dictionary3
			}));
		});
		RegisterHandler("gcuserService/campaigncomplete", "campaigncomplete", delegate(object[] args)
		{
			Campaign campaign2 = args[0] as Campaign;
			UnityEngine.Debug.Log("cc.awardGold: " + campaign2.awardGold);
			UnityEngine.Debug.Log("campaigncomplete");
			trCamp[campaign2.Id].status = 2;
			trCamp[campaign2.Id].awardGold = 1;
		});
		RegisterHandler("gcuserService/newmail", "newmail", delegate
		{
			UnityEngine.Debug.Log("gcuserService/newmail");
		});
		RegisterHandler("gcuserService/dwmtype", "dwmtype", delegate(object[] args)
		{
			int num = (int)args[0];
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			List<TopRank> list2 = new List<TopRank>();
			for (int k = 0; k < 6; k++)
			{
				list2.Add(new TopRank
				{
					nickname = "dwty" + k + "dwm" + num,
					gold = k * 10,
					awardName = "quanpanjiang1111",
					datetime = "2016.06.0" + k
				});
			}
			dictionary2.Add("TrList", list2);
			_netMsgQueue.Enqueue(new NetMessage("dwmtype", new object[1]
			{
				dictionary2
			}));
		});
		RegisterHandler("gcuserService/mail", "mail", delegate
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			List<Mail> list = new List<Mail>();
			for (int j = 0; j < 6; j++)
			{
				list.Add(new Mail
				{
					mailId = j,
					mailName = "jiangwen" + j,
					mailSendPeople = "admin" + j,
					mailTime = "2016.05.0" + j
				});
			}
			dictionary.Add("mail", list);
			_netMsgQueue.Enqueue(new NetMessage("mail", new object[1]
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
