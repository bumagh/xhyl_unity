using DG.Tweening;
using M__M.HaiWang.Fish;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_Score : MonoBehaviour
	{
		private Transform m_scoreBackTrans;

		private Text m_textMesh;

		private int m_score;

		private float m_scaleFactor;

		private bool bigfishScoreShouldFollow;

		public static int bibiCount;

		public event Action<FK3_Effect_Score> Event_Over_Handler;

		private void Awake()
		{
			m_textMesh = GetComponent<Text>();
			Vector3 localScale = base.transform.localScale;
			m_scaleFactor = localScale.x;
		}

		private void Start()
		{
		}

		private void Init()
		{
		}

		public void SetScoreBackTrans(Transform scoreBackTrans)
		{
			m_scoreBackTrans = scoreBackTrans;
		}

		public void Reset_EventHandler()
		{
			this.Event_Over_Handler = null;
		}

		public void Reset_Score()
		{
			DOTween.Kill(base.transform);
		}

		private void Update()
		{
		}

		public void SetScore(int score)
		{
			m_textMesh.text = score.ToString();
			m_score = score;
		}

		public void Play(int num, FK3_FishType fishType, int bulletPower, int seatId, FK3_FollowData followData, bool isBoss, bool bigScore = false, bool scoreFollow = false, Transform scoreBack = null, Action next = null)
		{
			Event_Over_Handler += delegate
			{
				if (next != null)
				{
					next();
				}
			};
			base.transform.position = followData.position;
			base.gameObject.SetActive(value: true);
			if (bigScore)
			{
				StartCoroutine(IE_NumIncrease(fishType, bulletPower, seatId, scoreFollow, isBoss, followData, scoreBack, 0, num));
				return;
			}
			SetScore(num);
			StartCoroutine(IE_PlayControl(followData));
		}

		private IEnumerator IE_PlayControl(FK3_FollowData followData)
		{
			base.transform.localScale = Vector3.one * m_scaleFactor * 0.1f;
			Sequence sequence = DOTween.Sequence();
			Vector3 position = base.transform.position;
			int num = UnityEngine.Random.Range(-1, 2);
			int num2 = UnityEngine.Random.Range(-1, 2);
			int num3 = UnityEngine.Random.Range(-1, 2);
			int num4 = UnityEngine.Random.Range(-1, 2);
			position.x += (float)(num * UnityEngine.Random.Range(0, 3)) * 0.1f + (float)num2 * 0.2f;
			position.y += (float)(num3 * UnityEngine.Random.Range(-3, 4)) * 0.1f + (float)num4 * 0.3f;
			sequence.Append(followData.transform.DOMove(position, 0.4f).SetEase(Ease.Linear).OnStart(delegate
			{
				transform.DOScale(m_scaleFactor * 1.5f, 0.4f);
			}));
			Vector3 position2 = base.transform.position;
			position2.x += (0f - (float)num) * (float)UnityEngine.Random.Range(0, 2) * 0.1f + (0f - (float)num2) * 0.2f;
			position2.y += (0f - (float)num3) * (float)UnityEngine.Random.Range(0, 3) * 0.1f + (0f - (float)num4) * (float)UnityEngine.Random.Range(-1, 2) * 0.3f;
			sequence.Append(followData.transform.DOMove(position, 0.9f).SetEase(Ease.Linear).OnStart(delegate
			{
				transform.DOScale(m_scaleFactor * 1f, 0.3f);
			}));
			sequence.OnUpdate(delegate
			{
				if (followData.transform != null)
				{
					Vector3 position3 = followData.transform.position;
					position3.y += ((FK3_GVars.lobby.curSeatId == 1 || FK3_GVars.lobby.curSeatId == 2) ? 0.15f : (-0.15f));
					Vector3 position4 = transform.position;
					position3.z = position4.z;
					transform.position = position3;
				}
				else
				{
					UnityEngine.Debug.LogError("followData.transform为空");
				}
			});
			sequence.OnComplete(delegate
			{
				Over();
			});
			yield break;
		}

		private IEnumerator IE_PlayControl_Large(int num, FK3_FollowData followData)
		{
			bool finish = false;
			int count = 0;
			SetScore(0);
			while (!finish)
			{
				base.transform.localScale = Vector3.one * m_scaleFactor * 0.1f;
				Sequence mySequence = DOTween.Sequence();
				mySequence.Append(base.transform.DOScale(m_scaleFactor * 1.5f, 0.3f));
				mySequence.Append(base.transform.DOScale(m_scaleFactor * 1f, 0.2f));
				mySequence.AppendInterval(0.3f);
				DOTween.Sequence();
				count++;
				if (count == 5)
				{
					finish = true;
				}
				yield return new WaitForSeconds(1f);
			}
			Over();
		}

		private IEnumerator SetBigFishScoreFollow(int seatId, FK3_FollowData followData, Transform scoreBack, bool isBoss)
		{
			while (bigfishScoreShouldFollow)
			{
				if (followData.transform != null)
				{
					Vector3 position = followData.transform.position;
					Vector3 position2 = base.transform.position;
					position.z = position2.z;
					if (isBoss)
					{
						if (seatId == 1 || seatId == 2)
						{
							if (FK3_GVars.lobby.curSeatId == 1 || FK3_GVars.lobby.curSeatId == 2)
							{
								position.y += -0.5f;
								position.x += -0.1f;
							}
							else
							{
								position.y -= 0.6f;
								position.x += 0.1f;
							}
						}
						else if (FK3_GVars.lobby.curSeatId == 3 || FK3_GVars.lobby.curSeatId == 4)
						{
							position.y += 0.5f;
							position.x += 0.1f;
						}
						else
						{
							position.y += 0.6f;
							position.x += -0.1f;
						}
					}
					base.transform.position = position;
					if (FK3_GVars.lobby.curSeatId == 1 || FK3_GVars.lobby.curSeatId == 2)
					{
						position.y -= 0.075f;
					}
					else
					{
						position.y += 0.115f;
					}
					if (scoreBack != null)
					{
						scoreBack.transform.position = position;
					}
				}
				else
				{
					UnityEngine.Debug.LogError("followData.transform为空");
				}
				yield return null;
			}
		}

		private IEnumerator IE_NumIncrease(FK3_FishType fishType, int bulletPower, int seatId, bool scoreFollow, bool isBoss, FK3_FollowData followData, Transform scoreBack, int from, int to)
		{
			switch (fishType)
			{
			case FK3_FishType.Boss_Dorgan_狂暴火龙:
			case FK3_FishType.Boss_Dorgan_冰封暴龙:
			case FK3_FishType.Boss_Crab_霸王蟹:
			case FK3_FishType.Boss_Kraken_深海八爪鱼:
			case FK3_FishType.Boss_Crocodil_史前巨鳄:
			case FK3_FishType.Boss_Lantern_暗夜炬兽:
				bibiCount++;
				if (bibiCount == 1)
				{
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("BBB5", loop: true);
					FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("BBB5"), 0.3f);
				}
				else
				{
					FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("BBB5"));
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("BBB5", loop: true);
					FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("BBB5"), 0.3f);
				}
				break;
			}
			float floatNum = from;
			int distance = to - from;
			if (scoreFollow)
			{
				bigfishScoreShouldFollow = true;
				StartCoroutine(SetBigFishScoreFollow(seatId, followData, scoreBack, isBoss));
			}
			int count2 = 0;
			int index4 = 1;
			FK3_FishMgr fishMgr3 = FK3_FishMgr.Get();
			int num3;
			int index3 = num3 = count2;
			count2 = num3 + 1;
			int curJumpRate = fishMgr3.GetFishRate(fishType, index3);
			int curJumpScore = bulletPower * curJumpRate;
			if (FK3_FishMgr.bigFishSet.Contains(fishType))
			{
				SetScore(to);
				yield return new WaitForSeconds(2f);
			}
			else if (fishType == FK3_FishType.GoldShark_霸王鲸)
			{
				SetScore(to);
				yield return new WaitForSeconds(5f);
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("BOSS获取金币音效");
			}
			else
			{
				ScoreBackColor(seatId);
				float targetScale;
				while (true)
				{
					floatNum += (float)distance / 100f;
					int num2 = Mathf.FloorToInt(floatNum);
					if (num2 < to)
					{
						if (num2 < curJumpScore)
						{
							SetScore(num2);
							yield return new WaitForSeconds(0.03f);
							continue;
						}
						num2 = curJumpScore;
						SetScore(num2);
						int preJumpRate = curJumpRate;
						FK3_FishMgr fishMgr2 = FK3_FishMgr.Get();
						index3 = (num3 = count2);
						count2 = num3 + 1;
						curJumpRate = fishMgr2.GetFishRate(fishType, index3);
						if (curJumpRate == preJumpRate)
						{
							break;
						}
						curJumpScore = bulletPower * curJumpRate;
						FK3_Singleton<FK3_SoundMgr>.Get().PlayClip(GetSoundName(index4));
						FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(GetSoundName(index4), 1f);
						index4++;
						targetScale = 0.1f + (float)count2 * 0.01f;
						Sequence sequence = DOTween.Sequence();
						sequence.Append(base.transform.DOScale(new Vector3(targetScale + 0.05f, targetScale + 0.05f, 0.1f), 0.1f));
						sequence.Append(base.transform.DOScale(new Vector3(targetScale, targetScale, 0.1f), 0.3f));
						yield return new WaitForSeconds(0.5f);
						continue;
					}
					SetScore(to);
					break;
				}
				Sequence sequence2 = DOTween.Sequence();
				targetScale = 0.1f + (float)count2 * 0.01f;
				sequence2.Append(base.transform.DOScale(new Vector3(targetScale + 0.05f, targetScale + 0.05f, 0.1f), 0.1f));
				sequence2.Append(base.transform.DOScale(new Vector3(targetScale, targetScale, 0.1f), 0.3f));
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip(GetSoundName(index4));
				FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(GetSoundName(index4), 1f);
				bibiCount--;
				if (bibiCount == 0)
				{
					FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("BBB5"));
				}
				yield return new WaitForSeconds(0.5f);
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("分数跳动结束音效");
				switch (fishType)
				{
				case FK3_FishType.Boss_Dorgan_狂暴火龙:
				case FK3_FishType.Boss_Dorgan_冰封暴龙:
				case FK3_FishType.Boss_Crab_霸王蟹:
				case FK3_FishType.Boss_Kraken_深海八爪鱼:
					yield return new WaitForSeconds(1f);
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("BOSS获取金币音效");
					break;
				default:
					yield return new WaitForSeconds(2f);
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("大鱼（除霸王鲸）获取金币音效");
					break;
				case FK3_FishType.GoldShark_霸王鲸:
					break;
				}
			}
			if (scoreFollow)
			{
				bigfishScoreShouldFollow = false;
			}
			Over();
		}

		private void ScoreBackColor(int seatId)
		{
			if (!(m_scoreBackTrans == null))
			{
				switch (seatId)
				{
				case 1:
					m_scoreBackTrans.GetComponent<Image>().color = new Color(0.788f, 0.035f, 0.098f);
					break;
				case 2:
					m_scoreBackTrans.GetComponent<Image>().color = new Color(0.99f, 0.83f, 0.4f);
					break;
				case 3:
					m_scoreBackTrans.GetComponent<Image>().color = new Color(0.188f, 0.447f, 0.788f);
					break;
				case 4:
					m_scoreBackTrans.GetComponent<Image>().color = new Color(0.99f, 0.352f, 0.701f);
					break;
				}
			}
		}

		private string GetSoundName(int count)
		{
			string result = "分数上涨音效1";
			switch (count)
			{
			case 1:
				result = "分数上涨音效1";
				break;
			case 2:
				result = "分数上涨音效2";
				break;
			case 3:
				result = "分数上涨音效3";
				break;
			case 4:
				result = "分数上涨音效4";
				break;
			case 5:
				result = "分数上涨音效5";
				break;
			}
			return result;
		}

		private void Over()
		{
			if (this.Event_Over_Handler != null)
			{
				this.Event_Over_Handler(this);
			}
		}
	}
}
