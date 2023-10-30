using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class USW_SymbolC : MonoBehaviour
{
	private Image iconimg;

	private Image frame;

	private float posy;

	private float startposy;

	private int Rollidx;

	private int SymbolId;

	private bool isroll;

	private int symidx = -1;

	private Color LightColor = new Color(1f, 1f, 1f);

	private Color DarkColor = new Color(0.4f, 0.4f, 0.4f);

	private Sequence Seq_sym;

	private float s;

	private float masx;

	public Transform Linne0_icoY;

	public Transform Linne0_ico5Y;

	private float topy = 325f;

	private float bottomy = -375f;

	private float symh = 148f;

	private float normalspeed = 2000f;

	private float speed;

	private int symlen = 13;

	private int Lineidx = -1;

	private USW_Line lineroll;

	private bool show;

	private int Inlineidx;

	private int rollidx;

	private float stopy;

	private List<int> rollnuelist = new List<int>();

	private Sequence Seq_ani;

	private Sequence Seq_Bolimgani;

	private float frames = 15f;

	private string aniname = "ELEWin{0:0}_";

	private string aniname2 = "ELEWin{0:0}_";

	private string bolimgAniName = "ELE{0:0}_";

	private int aniindex = 1;

	private bool loopdragon;

	private USW_RollSymbol Rs;

	private List<long> randomscorelist = new List<long>();

	private int scoreidx;

	private long score;

	private int diomadid = 11;

	private void Awake()
	{
		Vector3 localPosition = Linne0_icoY.localPosition;
		topy = localPosition.y;
		Vector3 localPosition2 = Linne0_ico5Y.localPosition;
		bottomy = localPosition2.y;
		Vector3 localPosition3 = base.transform.localPosition;
		posy = localPosition3.y;
		symlen = Cinstance<USW_Gcore>.Instance.Symcont;
		iconimg = GetComponent<Image>();
		speed = normalspeed;
		Seq_sym = DOTween.Sequence();
		Seq_sym.Pause();
		Seq_sym.SetAutoKill(autoKillOnCompletion: false);
		Seq_sym.Append(iconimg.DOFade(0.2f, 1f));
		Seq_sym.Append(iconimg.DOFade(1f, 1f));
		Seq_sym.SetLoops(-1);
	}

	private void Start()
	{
		Inlineidx = (int)((topy - posy) / symh);
	}

	public void Init(int idx, int symid, USW_Line line = null)
	{
		symidx = idx;
		lineroll = line;
		SetSymbolimg(symid, 0L, isInit: true);
		if (lineroll == null)
		{
			frame = base.transform.Find<Image>("frame");
			frame.gameObject.SetActive(value: false);
		}
	}

	public void SetSymbolimg(int idx, long score = 0L, bool isInit = false)
	{
		iconimg.sprite = Cinstance<USW_Gcore>.Instance.SymSprlist[idx];
		if (isInit)
		{
			PlayBolimgAni(idx);
		}
	}

	public void PlayBolimgAni(int idx)
	{
		try
		{
			Seq_Bolimgani = iconimg.USW_PlayAni(string.Format(bolimgAniName, (idx + 1).ToString("00")) + "{0:0}", string.Format(bolimgAniName, (idx + 1).ToString("00")) + "{0:0}", 1, 10f);
			Seq_Bolimgani.Restart();
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("动画播放错误: " + arg);
			throw;
		}
	}

	private void Onesymstop()
	{
		iconimg.enabled = true;
		SetSymbolimg(SymbolId, 0L);
		Rs.Showimg(isshow: false);
	}

	public void ShowSymbol(bool islight)
	{
		iconimg.color = ((!islight) ? DarkColor : LightColor);
	}

	public void Playchange(int type)
	{
		SymbolId = type;
	}

	public void ChangeSym()
	{
		SetSymbolimg(SymbolId, 0L);
	}

	public void StopRoll()
	{
		SetPosy(stopy);
		posy = stopy;
		if (isroll)
		{
			isroll = false;
			if (Inlineidx > 2)
			{
				int index = (Inlineidx - 3) * 5 + Lineidx;
				int idx = Cinstance<USW_Gcore>.Instance.Result[index];
				SetSymbolimg(idx, 0L);
			}
		}
		USW_SoundManager.Instance.PlayStopAndio();
		speed = normalspeed;
	}

	public void Setsymid(int id)
	{
		SymbolId = id;
	}

	private void InitSym()
	{
		iconimg.DOFade(1f, 0.1f);
		SetSymbolimg(SymbolId, score);
	}

	public void StopAni(bool allanistop = false)
	{
		if (allanistop || SymbolId != 13)
		{
			Seq_sym.Pause();
			frame.gameObject.SetActive(value: false);
			Seq_ani.Kill();
			Seq_Bolimgani.Kill();
			Seq_Bolimgani = null;
			InitSym();
			ShowSymbol(islight: true);
		}
	}

	public void PlayRollani(int symid)
	{
		SymbolId = symid;
		SetSymbolimg(symid, 0L);
	}

	public void Startrollsym(int idx, int result)
	{
		SymbolId = result;
		iconimg.enabled = false;
		Rs.RollSym(idx, result);
	}

	public void Roll(int lineidx, List<int> randomlist, bool isspeed = false)
	{
		Seq_Bolimgani.Kill();
		Seq_Bolimgani = null;
		rollnuelist.Clear();
		s = 0f;
		isroll = true;
		startposy = posy;
		Lineidx = lineidx;
		int num = (!isspeed) ? (Lineidx * 6 + 18) : ((Lineidx * 6 + 18) * (Lineidx - 1));
		masx = (float)num * symh;
		stopy = topy - (topy - startposy + masx % (symh * 6f)) % (topy - bottomy + symh);
		Inlineidx = (int)((topy - stopy) / symh);
		show = (Inlineidx >= 3);
		rollidx = ((Inlineidx == 2) ? (-1) : 0);
		Setrollnum(randomlist, num);
	}

	private void Setrollnum(List<int> randomlist, int rolllen)
	{
		randomscorelist.Clear();
		scoreidx = 0;
		int count = randomlist.Count;
		for (int i = 0; i < rolllen / 6; i++)
		{
			int index = UnityEngine.Random.Range(0, count);
			int item = randomlist[index];
			rollnuelist.Add(item);
		}
		if (Inlineidx > 2)
		{
			int count2 = rollnuelist.Count;
			int index2 = (Inlineidx - 3) * 5 + Lineidx;
			int value = Cinstance<USW_Gcore>.Instance.Result[index2];
			rollnuelist[count2 - 1] = value;
		}
	}

	public void Playani(int winline = -1)
	{
		if (winline >= 0)
		{
			frame.gameObject.SetActive(value: true);
		}
		ShowSymbol(islight: true);
		if (SymbolId == 0 || SymbolId == 1 || SymbolId == 8)
		{
			try
			{
				Seq_ani = iconimg.USW_PlayAni(string.Format(aniname, (SymbolId + 1).ToString("00")) + "{0:0}", string.Format(aniname, (SymbolId + 1).ToString("00")) + "{0:0}", 1, frames);
				Seq_ani.Restart();
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("动画播放错误: " + arg);
				Seq_sym.Restart();
				throw;
			}
		}
		else
		{
			Seq_sym.Restart();
		}
	}

	public void SetPosy(float y, bool init = false)
	{
		Vector3 localPosition = base.transform.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = new Vector3(x, y, 0f);
		base.transform.localPosition = localPosition2;
		if (init)
		{
			posy = localPosition2.y;
		}
	}

	private void Symtoshow()
	{
		if (rollidx >= 0)
		{
			int idx = rollnuelist[rollidx++];
			SetSymbolimg(idx, 0L);
		}
		else
		{
			rollidx += 2;
		}
	}

	private void FixedUpdate()
	{
		MoveIcon();
	}

	private void MoveIcon()
	{
		if (!isroll)
		{
			return;
		}
		float num = speed * Time.deltaTime;
		s += num;
		if (s >= masx)
		{
			isroll = false;
			s = 0f;
			StopRoll();
			lineroll.Playani();
			return;
		}
		posy = topy - (topy - posy + num) % (topy - bottomy + symh);
		if (posy <= topy && posy > topy - symh)
		{
			show = false;
		}
		SetPosy(posy);
		if (!show && posy < topy - 2f * symh && posy > topy - 3f * symh)
		{
			Symtoshow();
			show = true;
		}
	}
}
