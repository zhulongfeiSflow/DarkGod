/****************************************************
    文件：GuideWnd.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/16 11:31:22
	功能：引导对话界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot
{
    public Text txtName;
    public Text txtTalk;
    public Image imgIco;

    private PlayerData pd;
    private AutoGuideCfg curtTaskData;
    private string[] dialogArr;
    private int index;

    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        curtTaskData = MainCitySys.Instance.GetCurtTaskData();
        dialogArr = curtTaskData.dilogArr.Split('#');
        index = 1;

        SetTalk();
    }

    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        if (talkArr[0] == "0")
        {
            //自己
            SetSprite(imgIco, PathDefine.SelfIcon);
            SetText(txtName, pd.name);
        }
        else
        {
            //对话NPC
            switch (curtTaskData.npcID)
            {
                case 0:
                    SetSprite(imgIco, PathDefine.WiseManIcon);
                    SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIco, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIco, PathDefine.ArtisanIcon);
                    SetText(txtName, "工匠");
                    break;
                case 3:
                    SetSprite(imgIco, PathDefine.TraderIcon);
                    SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIco, PathDefine.GuideIcon);
                    SetText(txtName, "小芸");
                    break;
            }
        }

        imgIco.SetNativeSize();
        SetText(txtTalk, talkArr[1].Replace("$name", pd.name));
    }

    public void ClickNextBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        index += 1;
        if (index==dialogArr.Length)
        {
            //TODO发送任务引导完成信息
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide
                {
                    guideid = curtTaskData.ID
                }
            };
            netSvc.SendMsg(msg);

            SetWndState(false);
        }
        else
        {
            SetTalk();
        }
    }

}