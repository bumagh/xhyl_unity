using DG.Tweening;
using Framework;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class LightningDeadFish : ObjectBase
	{
		protected const float LINK_TIME = 0.4f;

		protected FishBehaviour _fish;

		protected int _bulletPower;

		protected int _fishRate;

		protected int _score;

		public int index;

		protected FishBehaviour _sourceFish;

		protected FishBehaviour _moveTarget;

		protected Transform _chainEffect;

		protected Transform _yellowBall;

		protected Transform _blueBall;

		protected Transform _headEffect;

		protected Task _playTask;

		protected Task _updateTask;

		private WaitForEndOfFrame _waitFrameEnd;

		private bool _linked;

		private float _linkElapsedTime;

		private NumberSprite _numSprite;

		private LightingFishAction _lightingFishAction;

		public FishBehaviour fish => _fish;

		public int bulletPower => _bulletPower;

		public int fishRate => _fishRate;

		public int score => _score;

		internal FishBehaviour moveTarget
		{
			get
			{
				return _moveTarget;
			}
			set
			{
				_moveTarget = value;
			}
		}

		public LightningDeadFish(FishBehaviour fish, int bulletPower, int fishRate, int score, NumberSprite numberSprite, LightingFishAction fishDeadAction)
		{
			try
			{
				_lightingFishAction = fishDeadAction;
				_fish = fish;
				_bulletPower = bulletPower;
				_fishRate = fishRate;
				_score = score;
				_chainEffect = this.Instantiate("FishEffect/LightningFish/LightningChain", active: false);
				_headEffect = _chainEffect.Find("LightningChain/HeadEffect");
				_yellowBall = this.Instantiate("FishEffect/LightningFish/YellowBall", active: false);
				if (!_fish.isLightning)
				{
					_blueBall = this.Instantiate("FishEffect/LightningFish/BlueBall", active: false);
				}
				_numSprite = numberSprite;
				_waitFrameEnd = new WaitForEndOfFrame();
				_chainEffect.SetParent(fish.transform.parent);
				_yellowBall.SetParent(fish.transform.parent);
				if ((bool)_blueBall)
				{
					_blueBall.SetParent(fish.transform.parent);
				}
				_chainEffect.localScale = Vector3.one;
				_chainEffect.localPosition = Vector3.zero;
				_yellowBall.localScale = Vector3.one;
				if ((bool)_blueBall)
				{
					_blueBall.localScale = Vector3.one;
				}
				_headEffect.localScale = Vector3.one;
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		public Coroutine Play(FishBehaviour source = null)
		{
			if (source != null)
			{
				UnityEngine.Debug.LogError("source: " + source.name);
			}
			else
			{
				UnityEngine.Debug.LogError("source为空");
			}
			_sourceFish = source;
			_linked = false;
			_moveTarget = _sourceFish;
			_playTask = new Task(_doPlay());
			_updateTask = new Task(_doUpdate());
			return _playTask.coroutine;
		}

		protected IEnumerator _doUpdate()
		{
			while (true)
			{
				if (null == _moveTarget)
				{
					yield return _waitFrameEnd;
					continue;
				}
				Vector3 moveTargetPos = _moveTarget.GetPosition();
				Vector3 vec = moveTargetPos - _fish.GetPosition();
				Vector3 dir = vec.normalized;
				float distance = vec.magnitude;
				if (_linked)
				{
					float d = 0.2f;
					if (distance > 1.5f)
					{
						d = Mathf.Min((distance - 1.5f) / 3f, 1.8f);
					}
					_fish.SetPosition(_fish.GetPosition() + d * dir * Time.deltaTime, adjustZDepth: false);
				}
				Quaternion rotation = Quaternion.AngleAxis(100f * Time.deltaTime, Vector3.forward);
				_fish.transform.rotation *= rotation;
				if ((bool)_blueBall)
				{
					_blueBall.SetPosition(_fish.GetPosition());
				}
				_yellowBall.SetPosition(_fish.GetPosition());
				_yellowBall.transform.rotation *= rotation;
				if (_sourceFish != null)
				{
					_chainEffect.SetPosition(moveTargetPos);
					Vector3 vector = _fish.GetPosition() - moveTargetPos;
					float num = Mathf.Atan2(vector.y, vector.x);
					_chainEffect.rotation = Quaternion.AngleAxis(57.29578f * num - 90f, Vector3.forward);
					float d2 = Mathf.Lerp(0f, distance, _linkElapsedTime / 0.4f);
					_linkElapsedTime += Time.deltaTime;
					_chainEffect.SetLocalScale(d2 * Vector3.one * 0.5f);
					Vector3 vector2 = _fish.GetPosition();
					if (distance > 1f)
					{
						vector2 += dir * 0.5f;
					}
					_numSprite.SetPosition(Vector3.Lerp(moveTargetPos, vector2, _linkElapsedTime / 0.4f));
				}
				yield return _waitFrameEnd;
			}
		}

		protected IEnumerator _doPlay()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("闪电鱼连接音效");
			_fish.Dying(playAni: false);
			_headEffect.SetActive();
			if (_sourceFish != null)
			{
				_chainEffect.SetActive();
				_chainEffect.position = _sourceFish.GetPosition();
				_chainEffect.SetLocalScale(0.1f * Vector3.one);
				_numSprite.SetPosition(_sourceFish.GetPosition());
				_numSprite.SetText(index.ToString());
			}
			else
			{
				UnityEngine.Debug.LogError("=======_sourceFish为空======");
			}
			_linkElapsedTime = 0f;
			yield return new WaitForSeconds(0.4f);
			_linked = true;
			_headEffect.SetActive(active: false);
			_blueBall.transform.localScale = Vector3.one;
			Vector3 vec = _fish.GetPosition();
			_blueBall.SetPosition(vec);
			_blueBall.SetActive();
			HW2_Singleton<SoundMgr>.Get().PlayClip("闪电鱼电网从大变小");
			Tweener tweener = ShortcutExtensions.DOScale(endValue: Mathf.Clamp(_fish.radius * 1.2f, 0.6f, 9f), target: _blueBall.transform, duration: 0.52f).SetEase(Ease.OutQuint).OnComplete(delegate
			{
				_blueBall.SetActive(active: false);
				vec = _fish.GetPosition();
				_yellowBall.SetPosition(vec);
				_yellowBall.SetLocalScale(_blueBall.GetLocalScale());
				_yellowBall.SetActive();
			});
			yield return tweener.WaitForCompletion();
		}

		public void Stop()
		{
			if (_fish.State != FishState.Dead)
			{
				_lightingFishAction.OnFishDead(_fish, bulletPower, fishRate, score);
			}
			this.Destroy(ref _chainEffect);
			this.Destroy(ref _yellowBall);
			this.Destroy(ref _blueBall);
			this.StopTask(ref _playTask);
			this.StopTask(ref _updateTask);
		}
	}
}
