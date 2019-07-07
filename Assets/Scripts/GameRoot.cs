/****************************************************
    文件：GameRoot.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/6 10:32:37
	功能：游戏启动入口
*****************************************************/

using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance = null;

    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("Game Start...");

        ClearUIRoot();

        Init(); 
    }

    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");

        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        dynamicWnd.SetWndState(true);
    }

    private void Init()
    {
        //服务器模块初始化
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();

        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();

        //进入登录场景并加载相应的UI
        login.EnterLogin();
    }

    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTip(tips);
    }
}