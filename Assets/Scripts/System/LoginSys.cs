/****************************************************
    文件：LoginSys.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/6 10:36:15
	功能：登录注册业务系统
*****************************************************/

using PEProtocol;

public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init LoginSys...");
    }

    /// <summary>
    /// 进入登录场景
    /// </summary>
    public void EnterLogin()
    {
        //to do 
        //异步加载登录场景
        //显示加载的进度条
        resSvc.AsyncLoadScene(Constants.SceneLogin, () =>
        {
            //加载完成后打开注册登录界面
            loginWnd.SetWndState(true);
            audioSvc.PlayBGMusic(Constants.BGLogin);
        });

    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录成功");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);

        if (msg.rspLogin.playerData.name == "")
        {
            createWnd.SetWndState(true);
        }
        else
        {
            MainCitySys.Instance.EnterMainCity();
        }
        //关闭登录界面
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);

        //跳转场景进入主城
        MainCitySys.Instance.EnterMainCity();

        //关闭创建界面
        createWnd.SetWndState(false);
    }
}