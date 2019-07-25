/****************************************************
    文件：BattleEndWnd.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/25 11:51:22
	功能：战斗结算界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class BattleEndWnd : WindowRoot
{
    #region UI Define
    public Transform rewardTrans;

    public Button btnClose;
    public Button btnExit;
    public Button btnSure;

    public Text txtTime;
    public Text txtRestHP;
    public Text txtReward;

    public Animation ani;

    #endregion
    private FBEndType endType = FBEndType.None;

    protected override void InitWnd() {
        base.InitWnd();

        RefreshUI();
    }

    private void RefreshUI() {
        switch (endType) {
            case FBEndType.Pause:
                SetActive(rewardTrans, false);
                SetActive(btnExit, true);
                SetActive(btnClose, true);
                break;
            case FBEndType.Win:
                SetActive(rewardTrans, false);
                SetActive(btnExit, false);
                SetActive(btnClose, false);

                MapCfg cfg = resSvc.GetMapCfgData(fbid);
                int min = costtime / 60;
                int sec = costtime % 60;
                int coin = cfg.coin;
                int exp = cfg.exp;
                int crystal = cfg.crystal;
                SetText(txtTime, "通关时间:" + min + ":" + sec);
                SetText(txtRestHP, "剩余血量:" + resthp);
                SetText(txtReward, "关卡奖励:" + Constants.Color(coin + "金币", TxtColor.Green) + Constants.Color(exp + "经验", TxtColor.Yellow) + Constants.Color(crystal + "水晶", TxtColor.Blue));

                timerSvc.AddTimeTask((int tid) =>
                {
                    SetActive(rewardTrans, true);
                    ani.Play();

                    timerSvc.AddTimeTask((int tid1) =>
                    {
                        audioSvc.PlayUIAudio(Constants.FBItemEnter);

                        timerSvc.AddTimeTask((int tid2) =>
                        {
                            audioSvc.PlayUIAudio(Constants.FBItemEnter);

                            timerSvc.AddTimeTask((int tid3) =>
                            {
                                audioSvc.PlayUIAudio(Constants.FBItemEnter);

                                timerSvc.AddTimeTask((int tid4) =>
                                {
                                    audioSvc.PlayUIAudio(Constants.FBLogoEnter);
                                }, 300);
                            }, 270);
                        }, 270);
                    }, 325);
                }, 1000);
                break;
            case FBEndType.Lose:
                SetActive(rewardTrans, false);
                SetActive(btnExit, true);
                SetActive(btnClose, false);
                audioSvc.PlayUIAudio(Constants.FBLose);
                break;
        }
    }

    public void ClickClose() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.battleMgr.isPauseGame = false;
        SetWndState(false);
    }

    public void ClickExitBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        //进入主城,销毁当前战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
    }

    public void ClickSureBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        //进入主城,销毁当前战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
        //打开副本界面
        FubenSys.Instance.EnterFuben();
    }

    public void SetWndType(FBEndType endType) {
        this.endType = endType;
    }

    private int fbid;
    private int costtime;
    private int resthp;
    public void SetBattleEndData(int fbid, int costtime, int resthp) {
        this.fbid = fbid;
        this.costtime = costtime;
        this.resthp = resthp;
    }
}

public enum FBEndType
{
    None,
    Pause,
    Win,
    Lose,
}