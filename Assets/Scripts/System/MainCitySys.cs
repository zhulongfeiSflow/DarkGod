/****************************************************
    文件：MainCitySys.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/13 14:57:17
	功能：主城业务系统
*****************************************************/

using UnityEngine;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;

    public MainCityWnd mainCityWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init MainCitySys...");
    }

    public void EnterMainCity()
    {
        resSvc.AsyncLoadScene(Constants.SceneMainCity,()=> {
            PECommon.Log("Enter MainCity...");

            //TODO 加载游戏主角

            //打开主城场景UI
            mainCityWnd.SetWndState(true);

            //播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);

            //TODO 设置人物展示
        });
    }

}