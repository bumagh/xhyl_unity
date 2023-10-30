using DP_GameCommon;
using UnityEngine;

public class DP_SceneGame : MonoBehaviour
{
	public DP_Bet sptBet;

	public DP_Hud sptHud;

	public DP_ResultCtrl sptResult;

	public DP_RecordCtrl sptRecord;

	private DP_GameInfo gameInfo;

	private void Start()
	{
		gameInfo = DP_GameInfo.getInstance();
		gameInfo.GetAppState = AppState.App_On_Game;
		gameInfo.SceneUi = null;
		gameInfo.SceneGame = this;
		DP_TipManager.GetSingleton().Init();
		sptBet.Init();
		sptBet.gameObject.SetActive(value: false);
		if (!sptHud.bInit)
		{
			sptHud.Init();
		}
		sptHud.gameObject.SetActive(value: false);
		sptResult.Init();
		sptResult.gameObject.SetActive(value: false);
		sptRecord.Init();
		sptRecord.gameObject.SetActive(value: false);
		sptHud.btnResultRecord.onClick.AddListener(delegate
		{
			DP_SoundManager.GetSingleton().playButtonSound();
			sptRecord.gameObject.SetActive(value: true);
		});
		sptHud.btnBet.onClick.AddListener(ClickBtnBet);
		InitGame();
	}

	public void ClickBtnBet()
	{
		DP_SoundManager.GetSingleton().playButtonSound();
		sptBet.ShowBet(!sptBet.bShowBet);
		sptHud.txtBtnBet.text = ((!sptBet.bShowBet) ? "下注" : "收起");
	}

	private void InitGame()
	{
		gameInfo.UpdateTable();
		sptHud.ShowHud();
		if (gameInfo.betTime > 0)
		{
			DP_GameInfo.getInstance().SceneGame.sptHud.SetGameCD(gameInfo.betTime, bIsJoinGame: true);
			if (!sptBet.bShowBet)
			{
				ClickBtnBet();
			}
			DP_GameInfo.getInstance().SceneGame.sptBet.SetAnimalPower(gameInfo.beilv);
			DP_GameInfo.getInstance().SceneGame.sptBet.ClearAllBet();
			DP_MusicMngr.GetSingleton().Reset();
			DP_GameCtrl.GetSingleton().Reset();
		}
		DP_GameCtrl.GetSingleton().animalColorCtrl.SetPointToIndex(gameInfo.pointerLocation);
		DP_GameCtrl.GetSingleton().animalColorCtrl.SetColorIndexs(gameInfo.colors);
	}
}
