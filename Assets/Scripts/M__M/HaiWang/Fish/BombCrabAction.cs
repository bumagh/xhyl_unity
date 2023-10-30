using DG.Tweening;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Effect;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Misc;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class BombCrabAction : MonoBehaviour
	{
		protected int _killerSeatID;

		protected Vector3 _killerGunPos;

		protected HashSet<FishType> _validFishTypes;

		protected FishBehaviour _bombCrab;

		protected bool _hitFishMsgReturned;

		protected bool _canContinue;

		protected int _bombCount;

		protected bool _faceDown;

		protected NumberSprite _numSprite;

		protected Queue<Vector2> _remoteNextPointQueue;

		protected Task _actionTask;

		private Action<FishBehaviour, FishDeadInfo> _fishDeadAction;

		private Effect_Logo _effect_LogobombCrab;

		private static BombCrabAction instance;

		private float _waitBigFishShowTime;

		private int _totalScore;

		private Stopwatch stopwatch = new Stopwatch();

		private Action _cleanAction;

		private ParticleSystem _effectBomb;

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

		protected bool _isOwner => _killerSeatID == HW2_GVars.game.curSeatId;

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

		public static BombCrabAction Get()
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
				NumberSprite original = Resources.Load<NumberSprite>("FishEffect/LightningFish/Number");
				_numSprite = UnityEngine.Object.Instantiate(original);
				UnityEngine.Object.DontDestroyOnLoad(_numSprite.gameObject);
				HW2_GVars.dontDestroyOnLoadList.Add(_numSprite.gameObject);
				_numSprite.name = "炸弹蟹数字";
				_numSprite.SetText("1");
				_numSprite.SetActive(active: false);
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

		public void Play(FishBehaviour bombCrab, Action<FishBehaviour, FishDeadInfo> fishDeadAction, int seatID, int bulletPower)
		{
			if (!(_bombCrab != null))
			{
				PlaySound();
				HW2_Singleton<SoundMgr>.Get().PlayClip("电磁炮聚能音效", loop: true);
				preIndex = -1;
				_bombCrab = bombCrab;
				_killerSeatID = seatID;
				_fishDeadAction = fishDeadAction;
				_canContinue = true;
				totalScore = 0;
				waitBigFishShowTime = 0f;
				GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(seatID);
				_killerGunPos = gunById.transform.position;
				_effect_LogobombCrab = Effect_LogoMgr.Get().SpawnBombCrabLogo(bombCrab.GetPosition(), _killerSeatID, bulletPower);
				_hitFishMsgReturned = false;
				_remoteNextPointQueue.Clear();
				bombCrab.Dying();
				_actionTask = new Task(IE_DoAction());
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

		public void OnActionMsgReturn(bool canContinue, List<DeadFishData> deadFishs)
		{
			UnityEngine.Debug.Log($"BombCrabAction.OnActionMsgReturn> continue:{canContinue}, deadFishs:{deadFishs.Count}");
			_canContinue = canContinue;
			_hitFishMsgReturned = true;
			stopwatch.Reset();
			stopwatch.Start();
			foreach (DeadFishData deadFish in deadFishs)
			{
				FishDeadInfo fishDeadInfo = new FishDeadInfo();
				fishDeadInfo.fishId = deadFish.fish.id;
				fishDeadInfo.bulletPower = deadFish.bulletPower;
				fishDeadInfo.fishType = (int)deadFish.fish.type;
				fishDeadInfo.killerSeatId = _killerSeatID;
				fishDeadInfo.addScore = deadFish.score;
				fishDeadInfo.deadWay = 5;
				fishDeadInfo.fishRate = deadFish.fishRate;
				_fishDeadAction(deadFish.fish, fishDeadInfo);
			}
		}

		public void OnMsgBombCrabNextPointPush(float x, float y)
		{
			Vector2 item = new Vector2(x, y);
			_remoteNextPointQueue.Enqueue(item);
		}

		private IEnumerator IE_DoAction()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Lime($"炸弹蟹 begin"));
			DOTween.Kill(_bombCrab.transform);
			DOTween.Kill(_numSprite.transform);
			_bombCrab.Reset_Scale();
			float waitDuration = 1.2f;
			_bombCount = 1;
			try
			{
				_bombCrab.PlayAni("Ready");
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("============动画播放错误==========" + arg);
			}
			_numSprite.transform.SetParent(_bombCrab.transform);
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
					List<FishBehaviour> screenFishList = FishMgr.Get().GetScreenFishList(delegate(FishBehaviour _fish)
					{
						if (!_fish.IsLive())
						{
							return false;
						}
						if (_fish.id == _bombCrab.id)
						{
							return false;
						}
						bool flag = Vector2.Distance(_bombCrab.transform.position, _fish.transform.position) < 2f;
						return (!flag) ? flag : (_fish.type <= FishType.Mobula_蝠魟 || _fish.type == FishType.Big_Clown_巨型小丑鱼 || _fish.type == FishType.Big_Rasbora_巨型鲽鱼 || _fish.type == FishType.Big_Puffer_巨型河豚);
					});
					List<FishData4Hit> list = (from _fish in screenFishList
						select new FishData4Hit(_fish)).ToList();
					if (list.Count == 0)
					{
						UnityEngine.Debug.LogWarning($"BombCrab>findFish failed. _bombCount:{_bombCount}");
					}
					string text = (from _fish in list
						select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("-");
					object[] args = new object[2]
					{
						10,
						text
					};
					HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/gunHitFishInAction", args);
				}
				yield return new WaitUntil(() => _hitFishMsgReturned || !_canContinue);
				_hitFishMsgReturned = false;
				ParticleSystem effectInst = GetEffectBomb();
				effectInst.transform.position = _bombCrab.transform.position;
				effectInst.name = "炸弹蟹爆炸特效";
				fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().ShakeScreen(1);
				HW2_Singleton<SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
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
				try
				{
					_bombCrab.PlayAni("Boom");
				}
				catch (Exception arg2)
				{
					UnityEngine.Debug.LogError("============动画播放错误==========" + arg2);
				}
				_bombCrab.SetLayerOrder(14501);
				_bombCount++;
				_numSprite.SetText(_bombCount.ToString());
				UnityEngine.Debug.Log(HW2_LogHelper.Lime($"炸弹蟹 bombCount:{_bombCount}"));
				yield return new WaitForSeconds(flyDuration);
				_bombCrab.SetLayerOrder(sortingOrder);
				try
				{
					_bombCrab.PlayAni("Ready");
				}
				catch (Exception arg3)
				{
					UnityEngine.Debug.LogError("============动画播放错误==========" + arg3);
				}
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
				ParticleSystem effectBomb = GetEffectBomb();
				effectBomb.transform.position = _bombCrab.transform.position;
				effectBomb.name = "炸弹蟹爆炸特效";
				HW2_Singleton<SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
				UnityEngine.Object.Destroy(effectBomb.gameObject, 5f);
			}
			_bombCrab.SetLayerOrder(sortingOrder);
			try
			{
				_bombCrab.PlayAni("Ready");
			}
			catch (Exception arg4)
			{
				UnityEngine.Debug.LogError("============动画播放错误==========" + arg4);
			}
			UnityEngine.Debug.Log(HW2_LogHelper.Lime($"炸弹蟹 end"));
			_bombCrab.Die();
			UnityEngine.Object.Destroy(effectRange.gameObject);
			_numSprite.transform.SetParent(null);
			_numSprite.SetActive(active: false);
			_bombCrab = null;
			_cleanAction = null;
			HW2_Singleton<SoundMgr>.Get().StopClip(HW2_Singleton<SoundMgr>.Get().GetClip("电磁炮聚能音效"));
			yield return new WaitForSeconds(2f);
			GunBehaviour gun = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			int beiLv = totalScore / gun.GetPower();
			PalyResultClip(beiLv);
			if (waitBigFishShowTime > 0f)
			{
				yield return new WaitForSeconds(waitBigFishShowTime);
			}
			Vector3 position = _effect_LogobombCrab.transform.position;
			ShortcutExtensions.DOMoveY(endValue: position.y + ((_killerSeatID == 1 || _killerSeatID == 2) ? 0.2f : (-0.2f)), target: _effect_LogobombCrab.transform, duration: 0.5f);
			Effect_Logo effect_logo_BombCrabScore = Effect_LogoMgr.Get().SpawnLogoScore(_killerGunPos, _killerSeatID, totalScore, Effect_LogoMgr.LogoScoreTypes.ScoreBombCrab);
			yield return new WaitForSeconds(3f);
			effect_logo_BombCrabScore.DoFade();
			_effect_LogobombCrab.DoFade();
			yield return new WaitForSeconds(1f);
			effect_logo_BombCrabScore.Over();
			_effect_LogobombCrab.Over();
			stopwatch.Stop();
			UnityEngine.Debug.Log(HW2_LogHelper.Orange($"连环炸弹最后一次收到死鱼通知后动画效果耗时：{stopwatch.Elapsed.TotalSeconds}"));
		}

		public void PalyResultClip(int temp)
		{
			if (temp < 20)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("结果1");
			}
			else if (temp >= 20 && temp < 90)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("结果3");
			}
			else if (temp >= 90 && temp < 120)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("结果2");
			}
			else if (temp >= 120)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("结果4");
			}
		}

		private ParticleSystem GetEffectBomb()
		{
			if (_effectBomb == null)
			{
				_effectBomb = Resources.Load<ParticleSystem>("VFX/VFX_Boom_00");
			}
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(_effectBomb);
			particleSystem.SetActive();
			return particleSystem;
		}

		private Animator GetEffectRange()
		{
			string path = "FishEffect/CrabBoom_Range" + _killerSeatID;
			_effectRange = Resources.Load<Animator>(path);
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
			yield return new WaitForSeconds(delay);
			object[] args = new object[2]
			{
				point.x,
				point.y
			};
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/askForFishFormation", args);
		}

		private void SendNextPoint(Vector2 point)
		{
			object[] args = new object[2]
			{
				point.x,
				point.y
			};
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/bombCrabNextPoint", args);
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
				HW2_Singleton<SoundMgr>.Get().PlayClip("捕获连环炸弹蟹获得连环炸弹时的语音1");
				break;
			case 2:
				HW2_Singleton<SoundMgr>.Get().PlayClip("捕获连环炸弹蟹获得连环炸弹时的语音2");
				break;
			case 3:
				HW2_Singleton<SoundMgr>.Get().PlayClip("捕获连环炸弹蟹获得连环炸弹时的语音3");
				break;
			default:
				UnityEngine.Debug.Log("seed:" + num);
				break;
			}
		}
	}
}
