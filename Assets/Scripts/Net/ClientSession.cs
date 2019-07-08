/****************************************************
    文件：ClientSession.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/8 11:21:0
	功能：客户端网络回话
*****************************************************/

using PENet;
using PEProtocol;
using UnityEngine;

public class ClientSession : PESession<GameMsg>
{
    protected override void OnConnected()
    {
        PECommon.Log("Server Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("Server Req:");
    }

    protected override void OnDisConnected()
    {
        PECommon.Log("Server DisConnect");
    }
}