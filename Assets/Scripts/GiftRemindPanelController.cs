using UnityEngine;
using UnityEngine.UI;

public class GiftRemindPanelController : MonoBehaviour
{
	[SerializeField]
	private Text gift;

	[SerializeField]
	private Text giftTime;

	[SerializeField]
	private Text giftRemark;

	public void Init(string uname, int gold, string time, string note)
	{
		gift.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"Congratulations on receiving {gold} coins from your friends {uname}" : ((ZH2_GVars.language_enum != 0) ? $"ย\u0e34นด\u0e35ด\u0e49วยนะ ท\u0e35\u0e48ได\u0e49ย\u0e34นอย\u0e48างน\u0e31\u0e49น{uname}สำหร\u0e31บ{gold}ช\u0e37\u0e48อเกม" : $"恭喜您收到好友{uname}赠送的{gold}游戏币"));
		giftTime.text = time;
		giftRemark.text = note;
	}

	public void OnBtnClick_Sure()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
