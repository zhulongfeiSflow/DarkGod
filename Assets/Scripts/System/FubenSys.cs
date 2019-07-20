/****************************************************
	文件：FubenSys.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/20 17:49   	
	功能：副本业务系统
*****************************************************/

public class FubenSys : SystemRoot {
    public static FubenSys Instance = null;

    public FubenWnd fubenWnd;

    public override void InitSys() {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init FubenSys...");
    }

    public void EnterFuben() {
        OpenFubenWnd();
    }

    #region Fuben Wnd
    public void OpenFubenWnd() {
        fubenWnd.SetWndState(true);
    }
    #endregion

}
