using FullInspector;
using M__M.HaiWang.Fish;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestFish
{
	[fiInspectorOnly]
	public class TestFishLogic : MonoBehaviour
	{
		[SerializeField]
		private FishBehaviour _targetFish;

		[SerializeField]
		private DragonBehaviour _dragon;

		private void Awake()
		{
		}

		private void Start()
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			PlayDragon();
		}

		private void Update()
		{
		}

		[InspectorButton]
		private void PlayDie()
		{
			try
			{
				_targetFish.PlayAni("Die");
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("============动画播放错误==========" + arg);
			}
		}

		[InspectorButton]
		private void PlayLife()
		{
			try
			{
				_targetFish.PlayAni("Life");
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("============动画播放错误==========" + arg);
			}
		}

		[InspectorButton]
		private void PlayDragon()
		{
			_dragon.Event_FishDieFinish_Handler = delegate(FishBehaviour _)
			{
				_.gameObject.SetActive(value: false);
				StartCoroutine(IE_PlayDragon());
			};
			_dragon.gameObject.SetActive(value: true);
			_dragon.Prepare();
		}

		private IEnumerator IE_PlayDragon()
		{
			yield return new WaitForSeconds(1f);
			PlayDragon();
		}
	}
}
