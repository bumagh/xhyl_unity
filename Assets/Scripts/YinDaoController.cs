using UnityEngine;
using UnityEngine.UI;

public class YinDaoController : MonoBehaviour
{
	[SerializeField]
	private Image _imgCheck;

	private void Start()
	{
	}

	public void ClickCheck()
	{
		_imgCheck.SetActive(!_imgCheck.gameObject.activeSelf);
	}
}
