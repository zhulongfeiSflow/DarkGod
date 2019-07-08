/****************************************************
    文件：NetSvc.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/8 11:11:6
	功能：网络服务
*****************************************************/

using PENet;
using PEProtocol;
using UnityEngine;

public class NetSvc : MonoBehaviour
{
    public static NetSvc Instance = null;

    PESocket<ClientSession, GameMsg> client = null;

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
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);


        PECommon.Log("Init NetSvc...");
    }

    public void SendMsg(GameMsg msg)
    {
        if (client.session!=null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("服务器未连接");
            InitSvc();
        }
    }

    private void Update()
    {

    }

}