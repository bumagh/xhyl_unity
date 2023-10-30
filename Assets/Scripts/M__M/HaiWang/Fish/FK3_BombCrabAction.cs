using DG.Tweening;
using DragonBones;
using HW3L;
using LitJson;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Effect;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Fish
{
	public class FK3_BombCrabAction : MonoBehaviour
	{
		protected int _killerSeatID;

		protected Vector3 _killerGunPos;

		protected HashSet<FK3_FishType> _validFishTypes;

		protected FK3_FishBehaviour _bombCrab;

		protected bool _hitFishMsgReturned;

		protected bool _canContinue;

		protected int _bombCount;

		protected bool _faceDown;

		protected FK3_NumberSprite _numSprite;

		protected Queue<Vector2> _remoteNextPointQueue;

		protected FK3_Task _actionTask;

		private Action<FK3_FishBehaviour, FK3_FishDeadInfo> _fishDeadAction;

		private UnityArmatureComponent unityArmature;

		private FK3_Effect_Logo _effect_LogobombCrab;

		private static FK3_BombCrabAction instance;

		private float _waitBigFishShowTime;

		private int _totalScore;

		private Stopwatch stopwatch = new Stopwatch();

		private Action _cleanAction;

		private HTExplosion _effectBomb;

		private Animator _effectRange;

		private int preIndex = -1;

		private List<Vector2> bombPoints0 = new List<Vector2>
		{
			new Vector2(-4f, 2f),
			new Vector2(-3f, 1.5f),
			new Vector2(-2f, 2f)
		};

		private List<Vector2> bombPoints1 = new List<Vector2>
		{
			new Vector2(4f, 2f),
			new Vector2(3f, 1.5f),
			new Vector2(2f, 2f)
		};

		private List<Vector2> bombPoints2 = new List<Vector2>
		{
			new Vector2(4f, -2f),
			new Vector2(3f, -1.5f),
			new Vector2(2f, -2f)
		};

		private List<Vector2> bombPoints3 = new List<Vector2>
		{
			new Vector2(-4f, -2f),
			new Vector2(-3f, -1.5f),
			new Vector2(-2f, -2f)
		};

		private Dictionary<int, List<Vector2>> bombPointsDic = new Dictionary<int, List<Vector2>>();

		private bool IsPlayOne;

		protected bool _isOwner => _killerSeatID == FK3_GVars.game.curSeatId;

		public float waitBigFishShowTime
		{
			get
			{
				return _waitBigFishShowTime;
			}
			set
			{
				_waitBigFishShowTime = value;
			}
		}

		public int totalScore
		{
			get
			{
				return _totalScore;
			}
			set
			{
				_totalScore = value;
			}
		}

		public static FK3_BombCrabAction Get()
		{
			return instance;
		}

		private void Awake()
		{
			instance = this;
			Init();
			InitBombPointsDic();
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (_bombCrab != null)
			{
				_bombCrab.transform.localEulerAngles -= new Vector3(0f, 0f, 5f);
				if (IsPlayOne)
				{
					IsPlayOne = false;
					unityArmature.animation.Play("fish26_bigger");
				}
			}
			if (waitBigFishShowTime > 0f)
			{
				waitBigFishShowTime -= Time.deltaTime;
			}
		}

		public void Init()
		{
			if (_numSprite == null)
			{
				FK3_NumberSprite original = Resources.Load<FK3_NumberSprite>("FishEffect/LightningFish/Number");
				_numSprite = UnityEngine.Object.Instantiate(original);
				UnityEngine.Object.DontDestroyOnLoad(_numSprite.gameObject);
				FK3_GVars.dontDestroyOnLoadList.Add(_numSprite.gameObject);
				_numSprite.gameObject.transform.localScale = Vector3.one;
				_numSprite.name = "炸弹蟹数字";
				_numSprite.SetText("1");
				_numSprite.gameObject.SetActive(value: false);
				_numSprite.fixFace = true;
			}
			_remoteNextPointQueue = new Queue<Vector2>();
			DoReset();
		}

		public void DoReset()
		{
			_canContinue = false;
			_bombCrab = null;
			if (_cleanAction != null)
			{
				_cleanAction();
			}
		}

		public void Play(FK3_FishBehaviour bombCrab, Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction, int seatID, int bulletPower)
		{
			if (!(_bombCrab != null))
			{
				PlaySound();
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("电磁炮聚能音效", loop: true);
				preIndex = -1;
				_bombCrab = bombCrab;
				unityArmature = null;
				IsPlayOne = true;
				unityArmature = _bombCrab.transform.Find("crab").GetComponent<UnityArmatureComponent>();
				unityArmature.animation.Play("fish26_swim");
				_killerSeatID = seatID;
				_fishDeadAction = fishDeadAction;
				_canContinue = true;
				totalScore = 0;
				waitBigFishShowTime = 0f;
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatID);
				_killerGunPos = gunById.transform.position;
				_effect_LogobombCrab = FK3_Effect_LogoMgr.Get().SpawnBombCrabLogo(bombCrab.GetPosition(), _killerSeatID, bulletPower);
				_hitFishMsgReturned = false;
				_remoteNextPointQueue.Clear();
				bombCrab.Dying();
				_actionTask = new FK3_Task(IE_DoAction());
			}
		}

		public void Stop()
		{
			if (_actionTask != null)
			{
				_actionTask.Stop();
				_actionTask = null;
			}
			if (_effect_LogobombCrab != null)
			{
				_effect_LogobombCrab.Over();
				_effect_LogobombCrab = null;
			}
			if (_bombCrab != null)
			{
				_bombCrab.Die();
			}
			_fishDeadAction = null;
			_numSprite.transform.SetParent(null);
			_numSprite.SetActive(active: false);
			DoReset();
		}

		public void OnActionMsgReturn(bool canContinue, List<FK3_DeadFishData> deadFishs)
		{
			UnityEngine.Debug.Log($"BombCrabAction.OnActionMsgReturn> continue:{canContinue}, deadFishs:{deadFishs.Count}");
			_canContinue = canContinue;
			_hitFishMsgReturned = true;
			stopwatch.Reset();
			stopwatch.Start();
			foreach (FK3_DeadFishData deadFish in deadFishs)
			{
				FK3_FishDeadInfo fK3_FishDeadInfo = new FK3_FishDeadInfo();
				fK3_FishDeadInfo.fishId = deadFish.fish.id;
				fK3_FishDeadInfo.bulletPower = deadFish.bulletPower;
				fK3_FishDeadInfo.fishType = (int)deadFish.fish.type;
				fK3_FishDeadInfo.killerSeatId = _killerSeatID;
				fK3_FishDeadInfo.addScore = deadFish.score;
				fK3_FishDeadInfo.deadWay = 5;
				fK3_FishDeadInfo.fishRate = deadFish.fishRate;
				_fishDeadAction(deadFish.fish, fK3_FishDeadInfo);
			}
		}

		public void OnMsgBombCrabNextPointPush(float x, float y)
		{
			Vector2 item = new Vector2(x, y);
			_remoteNextPointQueue.Enqueue(item);
		}

		private IEnumerator IE_DoAction()
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Lime($"炸弹蟹 begin"));
			DOTween.Kill(_bombCrab.transform);
			DOTween.Kill(_numSprite.transform);
			_bombCrab.Reset_Scale();
			float waitDuration = 1.2f;
			_bombCount = 1;
			unityArmature.animation.Play("fish26_fuse");
			_numSprite.transform.SetParent(_bombCrab.transform);
			_numSprite.transform.localScale = Vector3.one;
			_numSprite.transform.localPosition = Vector3.zero;
			_numSprite.SetActive();
			_numSprite.SetText(_bombCount.ToString());
			Animator effectRange = GetEffectRange();
			effectRange.transform.position = _bombCrab.transform.position;
			effectRange.transform.localScale = Vector3.zero;
			_cleanAction = (Action)Delegate.Combine(_cleanAction, (Action)delegate
			{
				if (effectRange != null && effectRange.gameObject != null)
				{
					UnityEngine.Object.Destroy(effectRange.gameObject);
				}
				_cleanAction = null;
			});
			Sequence seq3 = DOTween.Sequence();
			seq3.Append(effectRange.transform.DOScale(Vector3.one * 10f, 0.8f));
			seq3.Append(effectRange.transform.DOScale(Vector3.one * 10f, 1.1f).OnComplete(delegate
			{
				effectRange.transform.localScale = Vector3.zero;
			}));
			int sortingOrder = _bombCrab.displayOrder;
			yield return new WaitForSeconds(waitDuration + 0.5f);
			while (_canContinue)
			{
				if (_isOwner)
				{
					List<FK3_FishBehaviour> screenFishList = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish)
					{
						if (!_fish.IsLive())
						{
							return false;
						}
						if (_fish.id == _bombCrab.id)
						{
							return false;
						}
						bool flag = Vector2.Distance(_bombCrab.transform.position, _fish.transform.position) <= 2.5f;
						return (!flag) ? flag : (_fish.type <= FK3_FishType.Mobula_蝠魟 || _fish.type == FK3_FishType.Big_Clown_巨型小丑鱼 || _fish.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || _fish.type == FK3_FishType.Big_Puffer_巨型河豚);
					});
					List<FK3_FishData4Hit> list = (from _fish in screenFishList
						select new FK3_FishData4Hit(_fish)).ToList();
					if (list.Count == 0)
					{
						UnityEngine.Debug.LogError($"炸弹蟹 查找失败. 数量:{_bombCount}");
					}
					string text = (from _fish in list
						select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
					object[] array = new object[2]
					{
						10,
						text
					};
					UnityEngine.Debug.LogError("发送锁炸弹蟹: " + JsonMapper.ToJson(array));
					FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", array);
				}
				unityArmature.animation.Play("fish26_ready_explode");
				yield return new WaitUntil(() => _hitFishMsgReturned || !_canContinue);
				_hitFishMsgReturned = false;
				UnityEngine.Debug.LogError("炸弹蟹爆炸特效1");
				HTExplosion effectInst = GetEffectBomb();
				effectInst.transform.SetParent(base.transform);
				UnityEngine.Transform transform = effectInst.transform;
				Vector3 position = _bombCrab.transform.position;
				float x = position.x;
				Vector3 position2 = _bombCrab.transform.position;
				transform.position = new Vector3(x, position2.y, 0f);
				effectInst.name = "炸弹蟹爆炸特效";
				try
				{
					effectInst.transform.GetComponent<MeshRenderer>().sortingOrder = 20;
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("炸弹蟹爆炸特效1" + arg);
				}
				FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(3);
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
				UnityEngine.Object.Destroy(effectInst.gameObject, 5f);
				if (!_canContinue)
				{
					break;
				}
				Vector2 zero = Vector2.zero;
				float flyDuration = 0.8f;
				Vector2 nextPoint;
				if (_isOwner)
				{
					nextPoint = GetNextBombPoint();
					SendNextPoint(nextPoint);
					StartCoroutine(IE_SendAskForFishFormation(nextPoint, flyDuration));
				}
				else
				{
					yield return new WaitUntil(() => _remoteNextPointQueue.Count != 0 || !_canContinue);
					if (!_canContinue)
					{
						break;
					}
					nextPoint = _remoteNextPointQueue.Dequeue();
				}
				_bombCrab.transform.DOMove(new Vector3(nextPoint.x, nextPoint.y, 0f), flyDuration);
				_bombCrab.transform.DOScale(4f, flyDuration / 2f).SetLoops(2, LoopType.Yoyo);
				_bombCrab.SetLayerOrder(14501);
				_bombCount++;
				_numSprite.SetText(_bombCount.ToString());
				UnityEngine.Debug.Log(FK3_LogHelper.Lime($"炸弹蟹 bombCount:{_bombCount}"));
				yield return new WaitForSeconds(flyDuration);
				_bombCrab.SetLayerOrder(sortingOrder);
				unityArmature.animation.Play("fish26_fuse");
				effectRange.transform.position = _bombCrab.transform.position;
				effectRange.transform.localScale = Vector3.zero;
				Sequence seq2 = DOTween.Sequence();
				seq2.Append(effectRange.transform.DOScale(Vector3.one * 10f, 0.8f));
				seq2.Append(effectRange.transform.DOScale(Vector3.one * 10f, 0.6f).OnComplete(delegate
				{
					effectRange.transform.localScale = Vector3.zero;
				}));
				yield return new WaitForSeconds(waitDuration);
				yield return null;
			}
			if (!_isOwner)
			{
				HTExplosion effectBomb = GetEffectBomb();
				effectBomb.transform.SetParent(base.transform);
				UnityEngine.Debug.LogError("炸弹蟹爆炸特效2");
				UnityEngine.Transform transform2 = effectBomb.transform;
				Vector3 position3 = _bombCrab.transform.position;
				float x2 = position3.x;
				Vector3 position4 = _bombCrab.transform.position;
				transform2.position = new Vector3(x2, position4.y, 0f);
				effectBomb.name = "炸弹蟹爆炸特效";
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
				UnityEngine.Object.Destroy(effectBomb.gameObject, 5f);
			}
			_bombCrab.SetLayerOrder(sortingOrder);
			unityArmature.animation.Play("fish26_fuse");
			UnityEngine.Debug.Log(FK3_LogHelper.Lime($"炸弹蟹 end"));
			_bombCrab.Die();
			UnityEngine.Object.Destroy(effectRange.gameObject);
			_numSprite.transform.SetParent(null);
			_numSprite.SetActive(active: false);
			_bombCrab = null;
			_cleanAction = null;
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("电磁炮聚能音效"));
			yield return new WaitForSeconds(2f);
			FK3_GunBehaviour gun = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			int beiLv = totalScore / gun.GetPower();
			PalyResultClip(beiLv);
			if (waitBigFishShowTime > 0f)
			{
				yield return new WaitForSeconds(waitBigFishShowTime);
			}
			Vector3 position5 = _effect_LogobombCrab.transform.position;
			ShortcutExtensions.DOMoveY(endValue: position5.y + 1f, target: _effect_LogobombCrab.transform, duration: 0.5f);
			FK3_Effect_Logo effect_logo_BombCrabScore = FK3_Effect_LogoMgr.Get().SpawnLogoScore(_killerGunPos, _killerSeatID, totalScore, FK3_Effect_LogoMgr.LogoScoreTypes.ScoreBombCrab);
			effect_logo_BombCrabScore.GetComponent<Image>().DOFade(1f, 0.1f);
			_effect_LogobombCrab.GetComponent<Image>().DOFade(1f, 0.1f);
			yield return new WaitForSeconds(3f);
			effect_logo_BombCrabScore.GetComponent<Image>().DOFade(0f, 1f);
			_effect_LogobombCrab.GetComponent<Image>().DOFade(0f, 1f);
			yield return new WaitForSeconds(1f);
			effect_logo_BombCrabScore.Over();
			_effect_LogobombCrab.Over();
			stopwatch.Stop();
			UnityEngine.Debug.Log(FK3_LogHelper.Orange($"连环炸弹最后一次收到死鱼通知后动画效果耗时：{stopwatch.Elapsed.TotalSeconds}"));
		}

		public void PalyResultClip(int temp)
		{
			if (temp < 20)
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("结果1");
			}
			else if (temp >= 20 && temp < 90)
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("结果3");
			}
			else if (temp >= 90 && temp < 120)
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("结果2");
			}
			else if (temp >= 120)
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("结果4");
			}
		}

		private HTExplosion GetEffectBomb()
		{
			if (_effectBomb == null)
			{
				_effectBomb = Resources.Load<HTExplosion>("VFX/VFX_Boom_00");
			}
			if (_effectBomb == null)
			{
				UnityEngine.Debug.LogError("_effectBomb为空!");
				return null;
			}
			HTExplosion hTExplosion = UnityEngine.Object.Instantiate(_effectBomb);
			hTExplosion.SetActive();
			return hTExplosion;
		}

		private Animator GetEffectRange()
		{
			string text = "FishEffect/CrabBoom_Range" + _killerSeatID;
			UnityEngine.Debug.LogError("path: " + text);
			_effectRange = Resources.Load<Animator>(text);
			return UnityEngine.Object.Instantiate(_effectRange);
		}

		private IEnumerator IE_GetNextBombPoint()
		{
			while (true)
			{
				yield return new Vector2(-4f, 2f);
				yield return new Vector2(4f, 2f);
				yield return new Vector2(4f, -2f);
				yield return new Vector2(-4f, -2f);
			}
		}

		private void InitBombPointsDic()
		{
			bombPointsDic.Add(0, bombPoints0);
			bombPointsDic.Add(1, bombPoints1);
			bombPointsDic.Add(2, bombPoints2);
			bombPointsDic.Add(3, bombPoints3);
		}

		private Vector2 GetNextBombPoint()
		{
			int num = UnityEngine.Random.Range(0, 4);
			while (preIndex == num)
			{
				num = UnityEngine.Random.Range(0, 4);
			}
			preIndex = num;
			List<Vector2> list = bombPointsDic[num];
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		private IEnumerator IE_SendAskForFishFormation(Vector2 point, float delay)
		{
			point *= 1.1f;
			yield return new WaitForSeconds(delay);
			object[] args = new object[2]
			{
				(double)point.x,
				(double)point.y
			};
			try
			{
				UnityEngine.Debug.LogError("==发送炸弹蟹位置1: " + JsonMapper.ToJson(args));
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/askForFishFormation", args);
		}

		private void SendNextPoint(Vector2 point)
		{
			point *= 1.1f;
			object[] array = new object[2]
			{
				(double)point.x,
				(double)point.y
			};
			try
			{
				UnityEngine.Debug.LogError("==发送炸弹蟹位置1: " + JsonMapper.ToJson(array));
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/bombCrabNextPoint", array);
		}

		public void SetFaceDown(bool faceDown)
		{
			_faceDown = faceDown;
			_numSprite.SetFaceDown(faceDown);
		}

		private void PlaySound()
		{
			int num = UnityEngine.Random.Range(1, 4);
			switch (num)
			{
			case 1:
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("捕获连环炸弹蟹获得连环炸弹时的语音1");
				break;
			case 2:
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("捕获连环炸弹蟹获得连环炸弹时的语音2");
				break;
			case 3:
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("捕获连环炸弹蟹获得连环炸弹时的语音3");
				break;
			default:
				UnityEngine.Debug.Log("seed:" + num);
				break;
			}
		}
	}
}
