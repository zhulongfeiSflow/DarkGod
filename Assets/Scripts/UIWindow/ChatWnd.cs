/****************************************************
    文件：ChatWnd.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/17 22:39:6
	功能：聊天界面
*****************************************************/


using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWnd : WindowRoot
{
    public InputField iptChat;
    public Text txtChat;
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;

    private int chatType;
    private List<string> chatList = new List<string>();

    protected override void InitWnd()
    {
        base.InitWnd();

        chatType = 0;

        RefreshUI();
    }

    public void AddChatMsg(string name, string chat)
    {
        chatList.Add(Constants.Color(name + ":", TxtColor.Blue) + chat);
        if (chatList.Count > 12)
        {
            chatList.RemoveAt(0);
        }
        if (GetWndSatate())
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (chatType == 0)
        {
            string chatMsg = "";
            for (int i = 0; i < chatList.Count; i++)
            {
                chatMsg += chatList[i] + "\n";
            }
            SetText(txtChat, chatMsg);

            SetSprite(imgWorld, PathDefine.BtnType1);
            SetSprite(imgGuild, PathDefine.BtnType2);
            SetSprite(imgFriend, PathDefine.BtnType2);
        }
        else if (chatType == 1)
        {
            SetText(txtChat, "尚未加入公会");
            SetSprite(imgWorld, PathDefine.BtnType2);
            SetSprite(imgGuild, PathDefine.BtnType1);
            SetSprite(imgFriend, PathDefine.BtnType2);
        }
        else if (chatType == 2)
        {
            SetText(txtChat, "暂无好友信息");
            SetSprite(imgWorld, PathDefine.BtnType2);
            SetSprite(imgGuild, PathDefine.BtnType2);
            SetSprite(imgFriend, PathDefine.BtnType1);
        }
    }

    private bool canSend = true;
    public void ClickSendBtn()
    {
        if (!canSend)
        {
            GameRoot.AddTips("聊天消息每5秒钟才能发送一条");
            return;
        }

        if (iptChat.text != null && iptChat.text.Trim() != "")
        {
            if (iptChat.text.Length > 12)
            {
                GameRoot.AddTips("输入信息不能超过12个字");
            }
            else
            {
                //发送网络消息到服务器
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text,
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);
                canSend = false;

                timerSvc.AddTimeTask((int tid) =>
                {
                    canSend = true;
                }, 5, PETimeUnit.Second);
            }
        }
        else
        {
            GameRoot.AddTips("尚未输入聊天信息");
        }
    }

    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }

    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }

    public void ClickFirendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        SetWndState(false);
    }

}