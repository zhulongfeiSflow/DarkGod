/****************************************************
	文件：BattleMgr.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 1:47   	
	功能：战场管理器
*****************************************************/

using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;

    public EntityPlayer entitySelfPlayer;

    public void Init(int mapid) {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;

        //初始化管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();

        //加载战斗地图
        MapCfg mapData = resSvc.GetMapCfgData(mapid);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            //初始化地图数据

            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.transform.GetComponent<MapMgr>();
            mapMgr.Init();

            Camera.main.transform.position = mapData.mainCamPos;
            Camera.main.transform.localEulerAngles = mapData.mainCamRote;

            LoadPlayer(mapData);

            audioSvc.PlayBGMusic(Constants.BGHuangYe);
        });
    }

    private void LoadPlayer(MapCfg mapData) {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinBattlePlayerPrefab, true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;

        PlayerController playerCtrl
            = player.GetComponent<PlayerController>();
        playerCtrl.Init();

        entitySelfPlayer = new EntityPlayer {
            battleMgr = this,
            stateMgr = stateMgr,
            controller = playerCtrl,
            skillMgr = skillMgr,
        };

        entitySelfPlayer.Idle();

    }

    public void SetSelfPlayerMoveDir(Vector2 dir) {
        //设置玩家移动
        //PECommon.Log(dir.ToString());
        //释放技能不能移动
        if (entitySelfPlayer.canControl == false) {
            return;
        }

        if (dir == Vector2.zero) {
            entitySelfPlayer.Idle();
        }
        else {
            entitySelfPlayer.Move();
            entitySelfPlayer.SetDir(dir);
        }
    }

    public void ReqReleaseSkill(int index) {
        switch (index) {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }

    private void ReleaseNormalAtk() {
        PECommon.Log("Click Normal Atk");
    }

    private void ReleaseSkill1() {
        //PECommon.Log("Click Skill1");
        entitySelfPlayer.Attack(101);
    }

    private void ReleaseSkill2() {
        PECommon.Log("Click Skill2");
    }

    private void ReleaseSkill3() {
        PECommon.Log("Click Skill3");
    }

    public Vector2 GetDirInput() {
        return BattleSys.Instance.GetDirInput();
    }

}
