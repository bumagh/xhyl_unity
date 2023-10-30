using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class STMF_FlyObj : MonoBehaviour
{
	private Text txt;

	private Color col;

	private Material material;

	private void OnEnable()
	{
		SetMaterial();
	}

	public void SetMaterial()
	{
		if (txt == null)
		{
			txt = GetComponent<Text>();
		}
		if (material == null)
		{
			material = Resources.Load<Material>("FlowLightShader_ImgLine");
		}
		if (material != null && txt != null)
		{
			txt.material = material;
		}
	}

	public void AnimatedFlyTo(Vector3 destPos)
	{
		SetMaterial();
		Vector3 vector = base.transform.position - destPos;
		Vector3 vector2 = (!(vector.y > 0f)) ? Vector3.down : Vector3.up;
		Quaternion quaternion = default(Quaternion);
		quaternion.SetFromToRotation(Vector3.up, vector2);
		Transform transform = base.transform;
		Vector3 forward = Vector3.forward;
		Vector3 eulerAngles = quaternion.eulerAngles;
		transform.eulerAngles = forward * eulerAngles.z;
		Vector3 endValue = vector2 * 1f + base.transform.position;
		Vector3 position = base.transform.position;
		base.transform.DOMove(endValue, 0.5f).SetEase(Ease.InOutSine);
		base.transform.DOMove(position, 0.3f).SetEase(Ease.InOutSine).SetDelay(0.5f);
		base.transform.DOMove(destPos, 0.5f).SetEase(Ease.InSine).SetDelay(1.5f)
			.OnComplete(_onFlyEnd);
	}

	public void FlyTo(Vector3 destPos)
	{
		float sqrMagnitude = (destPos - base.transform.position).sqrMagnitude;
		float duration = 2f;
		base.transform.DOMove(destPos, duration).SetEase(Ease.InOutSine).OnComplete(_onFlyEnd);
	}

	public void AnimatedShow(Vector3 destPos)
	{
		SetMaterial();
		Vector3 vector = base.transform.position - destPos;
		Vector3 vector2 = (!(vector.y > 0f)) ? Vector3.down : Vector3.up;
		Quaternion quaternion = default(Quaternion);
		quaternion.SetFromToRotation(Vector3.up, vector2);
		Transform transform = base.transform;
		Vector3 forward = Vector3.forward;
		Vector3 eulerAngles = quaternion.eulerAngles;
		transform.eulerAngles = forward * eulerAngles.z;
		Vector3 endValue = vector2 * 0.5f + base.transform.position;
		Vector3 position = base.transform.position;
		base.transform.DOMove(endValue, 0.5f).SetEase(Ease.InOutSine);
		base.transform.DOMove(position, 0.3f).SetEase(Ease.InOutSine).SetDelay(0.5f);
		if (txt == null)
		{
			txt = GetComponent<Text>();
		}
		txt.DOFade(0f, 0.5f).SetDelay(1.5f).OnComplete(_onFadeEnd);
	}

	private void _onFadeEnd()
	{
		txt.material = null;
		col = txt.color;
		col.a = 1f;
		txt.color = col;
		STMF_EffectMngr.GetSingleton().DestroyUIEffectObj(base.gameObject);
	}

	private void _onFlyEnd()
	{
		STMF_EffectMngr.GetSingleton().DestroyEffectObj(base.gameObject);
	}

	public void OnDespawned()
	{
		base.transform.DOKill();
	}
}
