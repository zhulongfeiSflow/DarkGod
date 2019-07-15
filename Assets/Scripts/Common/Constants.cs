/****************************************************
    文件：Constants.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/6 13:42:43
	功能：常量配置
*****************************************************/

public class Constants
{
    //AutoGuideNpc
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;

    //场景名称
    public const string SceneLogin = "SceneLogin";
    public const int MainCityMapID = 10000;
    //public const string SceneMainCity = "SceneMainCity";

    //音效名字
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";

    //登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";

    //常规UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";

    //屏幕标准宽高
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeght = 750;
    //摇杆点标准焦距
    public const int ScreenOPDis = 90;

    //屏幕参数
    public const int BlendIdle = 0;
    public const int BlendWalk = 1;

    //角色移动速度
    public const int PlayerMoveSpeed = 8;
    public const int MonsterMoveSpeed = 4;

    //运动平滑加速度
    public const float AccelerSpeed = 5;
}