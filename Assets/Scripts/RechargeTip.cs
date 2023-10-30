using UnityEngine;
using UnityEngine.UI;

public class RechargeTip : MonoBehaviour
{
	private string strAlipayRechage = "充值比例：1:1\n温馨提示：\n1、需要安装支付宝客户端\n2、充值会打开外部浏览器网页，请放心支付\n3、请不要修改金额，修改金额会造成充值不到账";

	private string strAlipayCode = "充值比例：1:1\n温馨提示：\n1、需要安装支付宝客户端\n2、充值会打开外部浏览器网页，请放心支付\n3、请不要重复扫码，重复扫码会造成充值不到账\n4、请不要修改金额，修改金额会造成充值不到账";

	private string strWeChatRechage = "充值比例：1:1\n温馨提示：\n1、需要安装微信客户端\n2、充值会打开外部浏览器网页，请放心支付\n3、请不要修改金额，修改金额会造成充值不到账";

	private string strWeChatCode = "充值比例：1:1\n温馨提示：\n1、需要安装微信客户端\n2、充值会打开外部浏览器网页，请放心支付\n3、请不要重复扫码，重复扫码会造成充值不到账\n4、请不要修改金额，修改金额会造成充值不到账";

	private string strUnionPay = "充值比例：1:1\n温馨提示：\n1、银联支付不支持信用卡\n2、充值会打开外部浏览器网页，请放心支付\n3、请不要修改金额，修改金额会造成充值不到账";

	public Image img;

	public Text txt;

	public string[] strs;

	private void Awake()
	{
		img = base.transform.Find("Image").GetComponent<Image>();
		txt = base.transform.Find("RechargeTip").GetComponent<Text>();
		strs = new string[5];
		strs[0] = strAlipayRechage;
		strs[1] = strAlipayCode;
		strs[2] = strWeChatRechage;
		strs[3] = strWeChatCode;
		strs[4] = strUnionPay;
	}
}
