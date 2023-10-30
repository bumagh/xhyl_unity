using DG.Tweening;
using M__M.HaiWang.Fish;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.FishEffect
{
	public class FishEffect_CrabBoomEffect : MonoBehaviour
	{
		[SerializeField]
		private Transform _crabBoomRange;

		[SerializeField]
		private Transform _crabBoomNumber;

		[SerializeField]
		private GameObject _boomEffect;

		private FishBehaviour _crabBoom;

		private float _angles = 6f;

		private float _stayTime = 0.6f;

		private int _boomCount = 1;

		private Vector3 _carbBigScale = new Vector3(4f, 4f, 1f);

		private bool useFake = true;

		private Vector3[] fakePosition = new Vector3[4]
		{
			new Vector3(1f, 1f, 1f),
			new Vector3(4f, 4f, 1f),
			new Vector3(-2f, -2f, 1f),
			new Vector3(3f, 3f, 1f)
		};

		private int index;

		private bool hasNextBoom = true;

		private float moveTime = 0.8f;

		private void Start()
		{
		}

		public void Init(FishBehaviour crabBoom)
		{
			_crabBoomRange.gameObject.SetActive(value: true);
			_crabBoomNumber.gameObject.SetActive(value: true);
			_crabBoom = crabBoom;
			ChangeNumber(_boomCount);
			GetNextMessage();
		}

		private void GetNextMessage()
		{
			if (useFake)
			{
				StartCoroutine(CrabBoomEffetStart(hasNextBoom, fakePosition[index % fakePosition.Length]));
				index++;
				if (index > fakePosition.Length - 1)
				{
					hasNextBoom = false;
				}
			}
		}

		private IEnumerator CrabBoomEffetStart(bool hasNext, Vector3 nextPos)
		{
			if (_boomCount == 1)
			{
				RangeAniPlay(1);
				yield return new WaitForSeconds(3f);
			}
			if (!hasNext)
			{
				yield return new WaitForSeconds(0.1f);
				CrabBoomOver();
			}
			else
			{
				BoomEffect();
				StartCoroutine(CrabBoomMoveNext(nextPos));
			}
		}

		private void CrabBoomOver()
		{
			UnityEngine.Debug.Log("CrabBoomOver");
			BoomEffect();
			_crabBoom.Die();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void BoomEffect()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Blue("爆炸效果"));
			StartCoroutine(CreateBoomEffect());
		}

		private IEnumerator CreateBoomEffect()
		{
			GameObject boom = UnityEngine.Object.Instantiate(_boomEffect);
			boom.transform.SetParent(_crabBoom.transform.parent);
			boom.transform.localPosition = _crabBoom.transform.localPosition;
			boom.GetComponent<ParticleSystem>().Play();
			yield return new WaitForSeconds(1f);
			UnityEngine.Object.Destroy(boom);
		}

		private IEnumerator CrabBoomMoveNext(Vector3 nextPos)
		{
			_crabBoom.gameObject.GetComponent<Animator>().SetTrigger("Boom");
			_boomCount++;
			ChangeNumber(_boomCount);
			_crabBoomRange.gameObject.SetActive(value: false);
			Sequence seq = DOTween.Sequence();
			seq.Append(_crabBoom.transform.DOScale(_carbBigScale, moveTime / 2f));
			seq.Append(_crabBoom.transform.DOScale(new Vector3(2f, 2f, 1f), moveTime / 2f));
			base.transform.DOMove(nextPos, moveTime).SetEase(Ease.OutQuad);
			_crabBoom.transform.DOMove(nextPos, moveTime).SetEase(Ease.OutQuad);
			yield return new WaitForSeconds(moveTime);
			_crabBoomRange.gameObject.SetActive(value: true);
			_crabBoom.gameObject.GetComponent<Animator>().SetTrigger("Ready");
			RangeScalePlay();
			yield return new WaitForSeconds(_stayTime);
			if (useFake)
			{
				GetNextMessage();
			}
		}

		private void Update()
		{
			if ((bool)_crabBoom)
			{
				_crabBoom.transform.localEulerAngles += new Vector3(0f, 0f, _angles);
			}
		}

		private void ChangeNumber(int count)
		{
			_crabBoomNumber.GetComponentInChildren<TextMesh>().text = count.ToString();
		}

		private void RangeAniPlay(int index)
		{
			_crabBoomRange.localScale = new Vector3(10f, 10f, 1f);
			string trigger = "Range" + index;
			_crabBoomRange.GetComponent<Animator>().SetTrigger(trigger);
		}

		private void RangeScalePlay()
		{
			_crabBoomRange.localScale = Vector3.one;
			_crabBoomRange.DOScale(new Vector3(10f, 10f, 1f), _stayTime);
		}

		private void Reset()
		{
			_crabBoom = null;
			_crabBoomRange.localScale = new Vector3(10f, 10f, 1f);
			_boomCount = 1;
			hasNextBoom = true;
			ChangeNumber(_boomCount);
		}
	}
}
