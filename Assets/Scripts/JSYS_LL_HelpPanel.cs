using UnityEngine;

public class JSYS_LL_HelpPanel : MonoBehaviour
{
	protected UIPanel mHelpPanel;

	protected Collider mBackgroundCol;

	protected Collider mEGameCenterTagCol;

	protected Collider mGameDescriptionTagCol;

	protected Collider mCloseCol;

	protected Collider mGamePanelCol;

	protected Collider mEGameCenterPanelCol;

	protected Collider mSharedButtonCol;

	protected GameObject mEGameCenter;

	protected GameObject mGameDescription;

	protected GameObject mEGameCenterTagPg;

	protected GameObject mGameDescriptionTagPg;

	protected JSYS_LL_UICheckbox mEGameCenterTag;

	protected GameObject mCode;

	private void Start()
	{
		TextAsset textAsset = Resources.Load("EGameCenterDescription", typeof(TextAsset)) as TextAsset;
		mHelpPanel = base.transform.GetComponent<UIPanel>();
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		mEGameCenterTagCol = base.transform.Find("Tag").Find("EGameCenterDescriptionTag").GetComponent<Collider>();
		mGameDescriptionTagCol = base.transform.Find("Tag").Find("GameDescriptionTag").GetComponent<Collider>();
		mCloseCol = base.transform.Find("CloseBtn").GetComponent<Collider>();
		mEGameCenterPanelCol = base.transform.Find("EGameCenterDescription").Find("EGameCenter").GetComponent<Collider>();
		mGamePanelCol = base.transform.Find("GameDescription").Find("Game").GetComponent<Collider>();
		mSharedButtonCol = base.transform.Find("sharedButton").Find("backGround").GetComponent<Collider>();
		mEGameCenterTag = base.transform.Find("Tag").Find("EGameCenterDescriptionTag").GetComponent<JSYS_LL_UICheckbox>();
		mCode = base.transform.Find("code").gameObject;
		mEGameCenter = base.transform.Find("EGameCenterDescription").gameObject;
		mGameDescription = base.transform.Find("GameDescription").gameObject;
		mGameDescriptionTagPg = base.transform.Find("Tag").Find("GameDescriptionTag").Find("Preground")
			.gameObject;
			mEGameCenterTagPg = base.transform.Find("Tag").Find("EGameCenterDescriptionTag").Find("Preground")
				.gameObject;
				if ((bool)textAsset)
				{
					base.transform.Find("EGameCenterDescription").Find("EGameCenter").GetComponent<UITextList>()
						.Add(textAsset.text);
				}
				string text = base.transform.Find("GameDescription").Find("Game").Find("Description")
					.GetComponent<UILabel>()
					.text;
					base.transform.Find("GameDescription").Find("Game").GetComponent<UITextList>()
						.Add(text);
					mCode.SetActive(value: false);
					HideHelp();
				}

				public void ShowHelp()
				{
					mHelpPanel.enabled = true;
					_setColliderActive(bIsActive: true);
					mEGameCenterTag.isChecked = true;
					_showEGameCenterDescription();
				}

				public void HideHelp()
				{
					mHelpPanel.enabled = false;
					mEGameCenter.SetActiveRecursively(state: false);
					mGameDescription.SetActiveRecursively(state: false);
					_setColliderActive(bIsActive: false);
				}

				protected void _setColliderActive(bool bIsActive)
				{
					mBackgroundCol.enabled = bIsActive;
					mEGameCenterTagCol.enabled = bIsActive;
					mGameDescriptionTagCol.enabled = bIsActive;
					mCloseCol.enabled = bIsActive;
					mEGameCenterPanelCol.enabled = bIsActive;
					mGamePanelCol.enabled = bIsActive;
					mSharedButtonCol.enabled = bIsActive;
				}

				protected void _showGameDescription()
				{
					mEGameCenter.SetActiveRecursively(state: false);
					mGameDescription.SetActiveRecursively(state: true);
					mGameDescriptionTagPg.SetActiveRecursively(state: false);
					mEGameCenterTagPg.SetActiveRecursively(state: true);
					mGamePanelCol.enabled = true;
					UICamera.Notify(mGamePanelCol.gameObject, "OnSelect", true);
				}

				protected void _showEGameCenterDescription()
				{
					mEGameCenter.SetActiveRecursively(state: true);
					mGameDescription.SetActiveRecursively(state: false);
					mGameDescriptionTagPg.SetActiveRecursively(state: true);
					mEGameCenterTagPg.SetActiveRecursively(state: false);
					mEGameCenterPanelCol.enabled = true;
					UICamera.Notify(mEGameCenterPanelCol.gameObject, "OnSelect", true);
				}

				protected void setTextListActive()
				{
					if (mEGameCenter.activeSelf)
					{
						UICamera.Notify(mEGameCenterPanelCol.gameObject, "OnSelect", true);
					}
					else
					{
						UICamera.Notify(mGamePanelCol.gameObject, "OnSelect", true);
					}
					if (mCode.activeSelf)
					{
						mCode.SetActive(value: false);
						_setColliderActive(bIsActive: true);
					}
				}

				protected void showSharedDialog()
				{
					mCode.SetActive(value: true);
					_setColliderActive(bIsActive: false);
					mBackgroundCol.enabled = true;
				}
			}
