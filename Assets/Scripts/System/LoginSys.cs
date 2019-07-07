/****************************************************
    文件：LoginSys.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/6 10:36:15
	功能：登录注册业务系统
*****************************************************/

using UnityEngine;

public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init LoginSys...");
    }

    /// <summary>
    /// 进入登录场景
    /// </summary>
    public void EnterLogin()
    {
        //to do 
        //异步加载登录场景
        //显示加载的进度条
        resSvc.AsyncLoadScene(Constants.SceneLogin, ()=>
        {
            //加载完成后打开注册登录界面
            loginWnd.SetWndState(true);
            audioSvc.PlayBGMusic(Constants.BGLogin);
        });

    }

    public void RsLogin()
    {
        GameRoot.AddTips("登录成功");

        //打开角色创建界面
        createWnd.SetWndState(true);
        //关闭登录界面
        loginWnd.SetWndState(false);
    }
}