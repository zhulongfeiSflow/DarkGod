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
    public InfoWnd infoWnd;

    private PlayerController playerCtrl;
    private Transform charCamTrans;
    private AutoGuideCfg curtTaskData;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init MainCitySys...");
    }

    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);

        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            PECommon.Log("Enter MainCity...");

            //TODO 加载游戏主角
            LoadPlayer(mapData);

            //打开主城场景UI
            mainCityWnd.SetWndState(true);

            //播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);

            //设置人物展示
            if (charCamTrans == null)
            {
                charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
            }
            charCamTrans.gameObject.SetActive(false);
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinCityPlayerPrefab, true);

        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();

    }

    public void SetMoveDir(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendWalk);
        }

        playerCtrl.Dir = dir;
    }

    public void OpenInfoWnd()
    {
        //设置人物展示相机的相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        charCamTrans.localEulerAngles = new Vector3(0, 180f + playerCtrl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);

        infoWnd.SetWndState(true);
    }

    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
        }
        infoWnd.SetWndState(false);
    }

    private float startRoate = 0;
    public void SetStartRoate()
    {
        startRoate = playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRoate(float roate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRoate + roate, 0);
    }

    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            curtTaskData = agc;
        }

        //解析任务数据
        if (curtTaskData.npcID != -1)
        {

        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void OpenGuideWnd()
    {
        //TODO
    }

}