/****************************************************
	文件：BattleSys.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 1:34   	
	功能：战斗业务系统
*****************************************************/

using PEProtocol;
using UnityEngine;

public class BattleSys : SystemRoot
{
    public static BattleSys Instance = null;
    public PlayerCtrlWnd playerCtrlWnd;
    public BattleEndWnd battleEndWnd;
    public BattleMgr battleMgr;

    private int fbid;
    private double startTime;

    public override void InitSys() {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init BattleSys...");
    }

    public void StartBattle(int mapid) {
        fbid = mapid;
        GameObject go = new GameObject {
            name = "BattleRoot"
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapid, () =>
        {
            startTime = timerSvc.GetNowTime();
        });
        SetPlayerCtrlWndState();
    }

    public void EndBattle(bool isWin, int restHP) {
        playerCtrlWnd.SetWndState(false);
        GameRoot.Instance.dynamicWnd.RemoveAllHpItemInfo();

        if (isWin) {
            double endTime = timerSvc.GetNowTime();
            //发送结算战斗请求
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqFBFightEnd,
                reqFBFightEnd = new ReqFBFightEnd {
                    win = isWin,
                    fbid = fbid,
                    resthp = restHP,
                    costtime = (int)((endTime - startTime) / 1000),
                }
            };

            netSvc.SendMsg(msg);
        }
        else {
            SetBattleEndWndState(FBEndType.Lose);
        }
    }

    public void DestroyBattle() {
        SetPlayerCtrlWndState(false);
        SetBattleEndWndState(FBEndType.None, false);
        GameRoot.Instance.dynamicWnd.RemoveAllHpItemInfo();
        Destroy(battleMgr.gameObject);
    }

    public void SetPlayerCtrlWndState(bool isActive = true) {
        playerCtrlWnd.SetWndState(isActive);
    }

    public void SetBattleEndWndState(FBEndType endType, bool isActive = true) {
        battleMgr.isPauseGame = true;
        battleEndWnd.SetWndType(endType);
        battleEndWnd.SetWndState(isActive);
    }

    public void RspFBFightEnd(GameMsg msg) {
        RspFBFightEnd data = msg.rspFBFightEnd;
        GameRoot.Instance.SetPlayerDataByFBEnd(data);

        battleEndWnd.SetBattleEndData(data.fbid, data.costtime, data.resthp);
        SetBattleEndWndState(FBEndType.Win, true);

        if (msg.pshTaskPrgs != null) {
            MainCitySys.Instance.PshTaskPrgs(msg);
        }
    }

    public void SetMoveDir(Vector2 dir) {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index) {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput() {
        return playerCtrlWnd.currentDir;
    }

    public AniState GetPlayerState() {
        return battleMgr.entitySelfPlayer.currentAniState;
    }
}