/****************************************************
    文件：NetSvc.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/8 11:11:6
	功能：网络服务
*****************************************************/

//#define 使用外网

using PENet;
using PEProtocol;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetSvc : MonoBehaviour
{
    public static NetSvc Instance = null;

    public bool isInner = true;
    public static readonly string obj = "lock";
    private PESocket<ClientSession, GameMsg> client = null;
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();

    public void InitSvc()
    {
        Instance = this;

        client = new PESocket<ClientSession, GameMsg>();
        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
                default:
                    break;
            }
        });
        //client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
        if (isInner)
        {
            client.StartAsClient(SrvCfg.localIP, SrvCfg.innerPort); //内网测试
        }
        else
        {
            IPHostEntry host = Dns.GetHostEntry(SrvCfg.strDomain);
            IPAddress ip = host.AddressList[0];
            client.StartAsClient(ip.ToString(), SrvCfg.externalPort); //外网测试
        }

        PECommon.Log("Init NetSvc...");
    }

    public void SendMsg(GameMsg msg)
    {
        if (client.session != null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("服务器未连接");
            InitSvc();
        }
    }

    public void AddNetPkg(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    private void Update()
    {
        if (msgQue.Count > 0)
        {
            lock (obj)
            {
                GameMsg msg = msgQue.Dequeue();
                ProcessMsg(msg);
            }
        }
    }

    private void ProcessMsg(GameMsg msg)
    {
        if (msg.err != (int)ErrorCode.None)
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.ServerDataError:
                    PECommon.Log("服务器数据异常", LogType.Error);
                    GameRoot.AddTips("客户端数据异常");
                    break;
                case ErrorCode.UpdateDBError:
                    PECommon.Log("数据库更新异常", LogType.Error);
                    GameRoot.AddTips("网络不稳定");
                    break;
                case ErrorCode.ClientDataError:
                    PECommon.Log("客户端数据异常", LogType.Error);
                    break;
                case ErrorCode.AcctIsOnLine:
                    GameRoot.AddTips("当前账号已经上线");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("密码错误");
                    break;
                case ErrorCode.NameIsExist:
                    GameRoot.AddTips("该角色名已存在请重新选择！");
                    break;
                case ErrorCode.LackLevel:
                    GameRoot.AddTips("角色等级不够");
                    break;
                case ErrorCode.LackCoin:
                    GameRoot.AddTips("金币数量不够");
                    break;
                case ErrorCode.LackCrystal:
                    GameRoot.AddTips("水晶数量不够");
                    break;
                case ErrorCode.LackDiamond:
                    GameRoot.AddTips("钻石数量不够");
                    break;
                default:
                    GameRoot.AddTips("未知错误！"+ ((ErrorCode)msg.err).ToString());
                    break;
            }
            return;
        }

        switch ((CMD)msg.cmd)
        {
            case CMD.None:
                break;
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspGuide(msg);
                break;
            case CMD.RspStrong:
                MainCitySys.Instance.RspStrong(msg);
                break;
            case CMD.PshChat:
                MainCitySys.Instance.PshCaht(msg);
                break;
            case CMD.RspBuy:
                MainCitySys.Instance.RspBuy(msg);
                break;
            case CMD.PshPower:
                MainCitySys.Instance.PshPower(msg);
                break;
            case CMD.RspTakeTaskReward:
                MainCitySys.Instance.RspTakeTaskReward(msg);
                break;
            case CMD.PshTaskPrgs:
                MainCitySys.Instance.PshTaskPrgs(msg);
                break;
            default:
                GameRoot.AddTips("该请求客户端不支持！"+((CMD)msg.cmd).ToString());
                break;
        }
    }

}