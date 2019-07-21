/****************************************************
    文件：GameRoot.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/6 10:32:37
	功能：游戏启动入口
*****************************************************/

using PEProtocol;
using UnityEngine;
using System;

public class GameRoot : MonoBehaviour {
    public static GameRoot Instance = null;

    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start() {
        Instance = this;
        DontDestroyOnLoad(this);
        PECommon.Log("Game Start...");

        ClearUIRoot();

        Init();
    }

    private void ClearUIRoot() {
        Transform canvas = transform.Find("Canvas");

        for (int i = 0; i < canvas.childCount; i++) {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        dynamicWnd.SetWndState(true);
    }

    private void Init() {
        //服务器模块初始化
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        TimerSvc timerSvc = GetComponent<TimerSvc>();
        timerSvc.InitSvc();

        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();
        FubenSys fubenSys = GetComponent<FubenSys>();
        fubenSys.InitSys();
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();

        //进入登录场景并加载相应的UI
        login.EnterLogin();

        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            PECommon.Log("TEST Timer");
        }, 1000);
    }

    public static void AddTips(string tips) {
        Instance.dynamicWnd.AddTip(tips);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData {
        get {
            return playerData;
        }

    }

    public void SetPlayerData(RspLogin data) {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name) {
        PlayerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data) {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.guideid = data.guideid;

    }

    public void SetPlayerDataByRspStrong(RspStrong data) {
        PlayerData.coin = data.coin;
        PlayerData.crystal = data.crystal;
        PlayerData.hp = data.hp;
        PlayerData.ad = data.ad;
        PlayerData.ap = data.ap;
        PlayerData.addef = data.addef;
        PlayerData.apdef = data.apdef;

        PlayerData.strongArr = data.strongArr;

    }

    public void SetPlayerDataByBuy(RspBuy data) {
        PlayerData.diamond = data.diamond;
        PlayerData.coin = data.coin;
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByPower(PshPower data) {
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByTask(RspTakeTaskReward data) {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByTaskPsh(PshTaskPrgs data) {
        PlayerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByFBStart(RspFBFight data) {
        PlayerData.power = data.power;
    }
}