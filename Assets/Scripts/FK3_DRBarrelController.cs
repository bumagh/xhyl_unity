using DG.Tweening;
using HW3L;
using LitJson;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Bullet;
using M__M.HaiWang.Fish;
using M__M.HaiWang.Scenario;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FK3_DRBarrelController : MonoBehaviour
{
	private float dieTime = 4f;

	private HTExplosion _effectBomb;

	private void OnEnable()
	{
		GetComponent<FK3_BulletController>().isCanMove = true;
		dieTime = 4f;
		StartCoroutine(WaitDie());
	}

	private IEnumerator WaitDie()
	{
		yield return new WaitForSeconds(dieTime);
		GetComponent<FK3_BulletController>().isCanMove = false;
		base.transform.localScale = Vector3.one * 1.5f;
		base.transform.GetComponent<Image>().DOFade(0f, 0f);
		Boom();
		for (int i = 0; i < 5; i++)
		{
			SetBomDieFish();
			yield return new WaitForSeconds(0.1f);
		}
		if ((bool)base.gameObject)
		{
			Object.DestroyObject(base.gameObject);
		}
	}

	private void Boom()
	{
		HTExplosion effectBomb = GetEffectBomb();
		effectBomb.transform.SetParent(FK3_DrillCrabAction.Get().transform);
		Transform transform = effectBomb.transform;
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		transform.position = new Vector3(x, position2.y, 0f);
		effectBomb.name = "爆炸特效";
		FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(3);
		FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
	}

	private void SetBomDieFish()
	{
		List<FK3_FishBehaviour> inLaserFishList = FK3_FishMgr.Get().GetInLaserFishList(GetComponent<Collider>(), FK3_BulletMgr.Get().bulletBorder.rect, (FK3_FishBehaviour _fish) => _fish.IsLive() && _fish.Hitable && _fish.type <= FK3_FishType.Boss_Lantern_暗夜炬兽);
		List<FK3_FishData4Hit> source = (from _fish in inLaserFishList
			select new FK3_FishData4Hit(_fish)).ToList();
		string text = (from _fish in source
			select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
		object[] array = new object[2]
		{
			13,
			text
		};
		UnityEngine.Debug.LogError("发送爆炸伤害: " + JsonMapper.ToJson(array));
		FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", array);
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
		hTExplosion.gameObject.SetActive(value: true);
		return hTExplosion;
	}
}
