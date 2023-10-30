using DG.Tweening;
using M__M.HaiWang.Fish;
using M__M.HaiWang.Player.Gun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_Logo_Score : FK3_Effect_Logo
	{
		[SerializeField]
		private List<Image> renders;

		[SerializeField]
		private Text m_score;

		[SerializeField]
		private int gameNumber;

		private Transform logo1;

		private Transform logo2;

		private void Awake()
		{
			logo1 = renders[0].transform;
			logo2 = renders[1].transform;
		}

		public override void Reset_Logo()
		{
			base.Reset_Logo();
			DOTween.Kill(base.transform);
			foreach (Image render in renders)
			{
				Vector4 v = render.color;
				v.w = 1f;
				render.color = v;
			}
			Vector4 v2 = m_score.material.color;
			v2.w = 1f;
			m_score.material.color = v2;
		}

		public void SetScoreDes(string scoreNumStr)
		{
			int length = scoreNumStr.Length;
			float num = 0.6f + 0.25f * (float)(length - 1);
			Vector3 position = logo1.position;
			position.x = 0f - num + 0.15f;
			logo1.position = position;
			position = logo2.position;
			position.x = num + 0.15f;
			logo2.position = position;
		}

		public override void Play(Vector3 pos, Vector3 posAdd, int seatId, int[] values)
		{
			Vector3 position = base.transform.position;
			float num = pos.z = position.z;
			m_score.GetComponent<Text>().text = string.Empty + values[0];
			SetScoreDes(values[0].ToString());
			base.gameObject.SetActive(value: true);
			base.transform.localEulerAngles = new Vector3(0f, 0f, (seatId <= 2) ? 0f : 180f);
			Vector3 vector = pos;
			pos.y = vector.y + 1f;
			base.transform.position = pos;
			showTask = new FK3_Task(Show(seatId));
		}

		private IEnumerator Show(int seatId)
		{
			Vector3 position = base.transform.position;
			float moveY = position.y + 0.2f;
			yield return null;
			base.transform.DOMoveY(moveY, 0.5f).SetLoops(-1, LoopType.Yoyo);
			yield return new WaitForSeconds(0.2f);
			if (gameNumber == 1)
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("闪电鱼分数跳出，庆祝音效");
				FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("闪电鱼分数跳出，庆祝音效", 0.5f);
			}
			if (gameNumber == 2)
			{
				int totalScore = FK3_BombCrabAction.Get().totalScore;
				FK3_GunBehaviour nativeGun = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
				int temp = totalScore / nativeGun.GetPower();
				FK3_BombCrabAction.Get().PalyResultClip(temp);
			}
			if (gameNumber == 3)
			{
				int totalScore2 = FK3_LaserCrabAction.Get().totalScore;
				FK3_GunBehaviour nativeGun2 = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
				int temp2 = totalScore2 / nativeGun2.GetPower();
				FK3_LaserCrabAction.Get().PalyResultClip(temp2);
			}
		}

		public override void DoFade()
		{
			foreach (Image render in renders)
			{
				render.DOFade(0f, 0.5f);
			}
			m_score.material.DOFade(0f, 0.5f);
		}

		public override void SetLayerOrder(int order)
		{
		}
	}
}
