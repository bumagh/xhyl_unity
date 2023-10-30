using DG.Tweening;
using Framework;
using HW3L;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_LightningDeadFish : FK3_ObjectBase
	{
		protected const float LINK_TIME = 0.4f;

		protected FK3_FishBehaviour _fish;

		protected int _bulletPower;

		protected int _fishRate;

		protected int _score;

		public int index;

		protected FK3_FishBehaviour _sourceFish;

		protected FK3_FishBehaviour _moveTarget;

		protected Transform _chainEffect;

		protected Transform _yellowBall;

		protected Transform _blueBall;

		protected Transform _headEffect;

		protected FK3_Task _playTask;

		protected FK3_Task _updateTask;

		private WaitForEndOfFrame _waitFrameEnd;

		private bool _linked;

		private float _linkElapsedTime;

		private FK3_NumberSprite _numSprite;

		private FK3_LightingFishAction _lightingFishAction;

		public FK3_FishBehaviour fish => _fish;

		public int bulletPower => _bulletPower;

		public int fishRate => _fishRate;

		public int score => _score;

		internal FK3_FishBehaviour moveTarget
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

		public FK3_LightningDeadFish(FK3_FishBehaviour fish, int bulletPower, int fishRate, int score, FK3_NumberSprite numberSprite, FK3_LightingFishAction fishDeadAction)
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
					_blueBall.name = "LightingYellowBall";
				}
				else
				{
					_blueBall = fish.transform.Find("LightingYellowBall");
				}
				UnityEngine.Debug.LogError("_blueBall: " + _blueBall.name);
				_numSprite = numberSprite;
				_waitFrameEnd = new WaitForEndOfFrame();
				_chainEffect.SetParent(fish.transform.parent);
				_yellowBall.SetParent(fish.transform.parent);
				_blueBall.SetParent(fish.transform.parent);
				_chainEffect.localScale = Vector3.one;
				_chainEffect.localPosition = Vector3.zero;
				_yellowBall.localScale = Vector3.one;
				_blueBall.localScale = Vector3.one;
				_headEffect.localScale = Vector3.one;
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		public Coroutine Play(FK3_FishBehaviour source = null)
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
			_playTask = new FK3_Task(_doPlay());
			_updateTask = new FK3_Task(_doUpdate());
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
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("闪电鱼连接音效");
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
			if (!_blueBall)
			{
				_blueBall = fish.transform.Find("LightingYellowBall");
			}
			Vector3 vec = _fish.GetPosition();
			if ((bool)_blueBall)
			{
				_blueBall.SetPosition(vec);
			}
			if ((bool)_blueBall)
			{
				_blueBall.SetActive();
			}
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("闪电鱼电网从大变小");
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
			if (_fish.State != FK3_FishState.Dead)
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
