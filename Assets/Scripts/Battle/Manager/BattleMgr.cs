/****************************************************
	文件：BattleMgr.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 1:47   	
	功能：战场管理器
*****************************************************/

using PEProtocol;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;

    public EntityPlayer entitySelfPlayer;
    private MapCfg mapCfg;

    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();

    public void Init(int mapid) {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;

        //初始化管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();

        //加载战斗地图
        mapCfg = resSvc.GetMapCfgData(mapid);
        resSvc.AsyncLoadScene(mapCfg.sceneName, () =>
        {
            //初始化地图数据
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.transform.GetComponent<MapMgr>();
            mapMgr.Init(this);

            Camera.main.transform.position = mapCfg.mainCamPos;
            Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

            LoadPlayer(mapCfg);
            entitySelfPlayer.Idle();

            //延迟激活第一批怪物
            ActiveCurrentBatchMonster();

            audioSvc.PlayBGMusic(Constants.BGHuangYe);
        });
    }

    public void Update() {
        foreach (var item in monsterDic.Values) {
            item.TickAILogic();
        }
    }

    private void LoadPlayer(MapCfg mapData) {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinBattlePlayerPrefab, true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;

        PlayerData pd = GameRoot.Instance.PlayerData;
        BattleProps props = new BattleProps {
            hp = pd.hp,
            ad = pd.ad,
            ap = pd.ap,
            addef = pd.addef,
            apdef = pd.apdef,
            dodge = pd.dodge,
            pierce = pd.pierce,
            critical = pd.critical,
        };

        entitySelfPlayer = new EntityPlayer {
            battleMgr = this,
            stateMgr = stateMgr,
            skillMgr = skillMgr,
            Name = "AssassinBattle",
        };
        entitySelfPlayer.SetBattleProps(props);

        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        entitySelfPlayer.SetCtrl(playerCtrl);
    }

    public void LoadMonsterByWaveID(int wave) {
        for (int i = 0; i < mapCfg.monsterLst.Count; i++) {
            MonsterData md = mapCfg.monsterLst[i];
            if (md.mWave == wave) {
                GameObject monstePref = resSvc.LoadPrefab(md.mCfg.resPath, true);
                monstePref.transform.localPosition = md.mBornPos;
                monstePref.transform.localEulerAngles = md.mBornRote;
                monstePref.transform.localScale = Vector3.one;

                monstePref.name = md.mCfg.mName + md.mWave + "_" + md.mIndex;

                EntityMonster monsterEntity = new EntityMonster {
                    battleMgr = this,
                    stateMgr = stateMgr,
                    skillMgr = skillMgr,
                };
                //设置初始属性
                monsterEntity.md = md;
                monsterEntity.SetBattleProps(md.mCfg.bps);
                monsterEntity.Name = monstePref.name;

                MonsterController mc = monstePref.GetComponent<MonsterController>();
                mc.Init();
                monsterEntity.SetCtrl(mc);

                monstePref.SetActive(false);
                monsterDic.Add(monstePref.name, monsterEntity);
                GameRoot.Instance.dynamicWnd.AddHpItemInfo(monstePref.name, mc.hpRoot, monsterEntity.HP);
            }
        }
    }

    public void ActiveCurrentBatchMonster() {
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            foreach (var item in monsterDic.Values) {
                item.SetActive(true);
                item.Born();
                TimerSvc.Instance.AddTimeTask((int id) =>
                {
                    //出生一秒后进入Idle
                    item.Idle();
                }, 1000);
            }
        }, 500);
    }

    public List<EntityMonster> GetEntityMonster() {
        return monsterDic.Values.ToList();
    }

    public void RemoveMonster(string key) {
        if (monsterDic.ContainsKey(key)) {
            monsterDic.Remove(key);
            GameRoot.Instance.dynamicWnd.RemoveHpItemInfo(key);
        }
    }

    #region 技能释放于角色控制
    public void SetSelfPlayerMoveDir(Vector2 dir) {
        //设置玩家移动
        //PECommon.Log(dir.ToString());
        //释放技能不能移动
        if (entitySelfPlayer.canControl == false) {
            return;
        }
        if (entitySelfPlayer.currentAniState == AniState.Idle ||
            entitySelfPlayer.currentAniState == AniState.Move) {
            if (dir == Vector2.zero) {
                entitySelfPlayer.Idle();
            }
            else {
                entitySelfPlayer.Move();
                entitySelfPlayer.SetDir(dir);
            }
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

    private int[] comboArr = new int[] { 111, 112, 113, 114, 115 };
    public double lastAtkTime = 0;
    public int comboIndex = 0;
    private void ReleaseNormalAtk() {
        //PECommon.Log("Click Normal Atk");
        if (entitySelfPlayer.currentAniState == AniState.Attack) {
            //在500ms以内进行第二次点击,存数据
            var nowAtkTime = TimerSvc.Instance.GetNowTime();
            if (nowAtkTime - lastAtkTime < Constants.ComboSpace && lastAtkTime != 0) {
                if (comboIndex != comboArr.Length - 1) {
                    comboIndex += 1;
                    entitySelfPlayer.comboQue.Enqueue(comboArr[comboIndex]);
                    lastAtkTime = nowAtkTime;
                }
                else {
                    comboIndex = 0;
                    lastAtkTime = 0;
                }
            }
        }
        else if (entitySelfPlayer.currentAniState == AniState.Idle ||
            entitySelfPlayer.currentAniState == AniState.Move) {
            comboIndex = 0;
            lastAtkTime = TimerSvc.Instance.GetNowTime();
            entitySelfPlayer.Attack(comboArr[comboIndex]);
        }
    }

    private void ReleaseSkill1() {
        //PECommon.Log("Click Skill1");
        entitySelfPlayer.Attack(101);
    }

    private void ReleaseSkill2() {
        //PECommon.Log("Click Skill2");
        entitySelfPlayer.Attack(102);
    }

    private void ReleaseSkill3() {
        //PECommon.Log("Click Skill3");
        entitySelfPlayer.Attack(103);
    }

    public Vector2 GetDirInput() {
        return BattleSys.Instance.GetDirInput();
    }
    #endregion



}
