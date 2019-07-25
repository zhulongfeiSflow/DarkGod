/****************************************************
    文件：ResSvc.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/6 10:40:53
	功能：资源加载服务
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;

    public void InitSvc() {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);

        InitMonsterCfg(PathDefine.MonsterCfg);
        InitMapCfg(PathDefine.MapCfg);

        InitGuideCfg(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        InitTaskRewardCfg(PathDefine.TaskCfg);

        InitSkillCfg(PathDefine.SkillCfg);
        InitSkillMoveCfg(PathDefine.SkillMoveCfg);
        InitSkillActionCfg(PathDefine.SkillActionCfg);
        PECommon.Log("Init ResSvc...");
    }

    public void ResetSkillCfgs() {//动态测试技能效果
        //return;
        InitSkillCfg(PathDefine.SkillCfg);
        InitSkillMoveCfg(PathDefine.SkillMoveCfg);
        InitSkillActionCfg(PathDefine.SkillActionCfg);
        PECommon.Log("Reset SkillCfgs...");
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded) {
        GameRoot.Instance.loadingWnd.SetWndState(true);

        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);

        prgCB = () =>
        {
            float val = sceneAsync.progress;
            GameRoot.Instance.loadingWnd.SetProgress(val);
            if (val == 1) {
                if (loaded != null) {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
    }

    private void Update() {
        if (prgCB != null) {
            prgCB();
        }
    }

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false) {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au)) {
            au = Resources.Load<AudioClip>(path);
            if (cache) {
                adDic.Add(path, au);
            }
        }
        return au;
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false) {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab)) {
            prefab = Resources.Load<GameObject>(path);
            if (cache) {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if (prefab != null) {
            go = Instantiate(prefab);
        }
        return go;
    }

    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false) {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp)) {
            sp = Resources.Load<Sprite>(path);
            if (cache) {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    #region InitCfgs

    private bool TryGetRootNodeList(string path, out XmlNodeList nodList) {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml) {
            PECommon.Log("xml file:" + path + " not exist!", LogType.Error);
        }
        else {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            nodList = doc.SelectSingleNode("root").ChildNodes;
            return true;
        }
        nodList = null;
        return false;
    }

    #region 随机名字
    private List<string> surnameLst = new List<string>();
    private List<string> manLst = new List<string>();
    private List<string> womanLst = new List<string>();
    private void InitRDNameCfg(string path) {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                //int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                foreach (XmlElement e in nodList[i].ChildNodes) {
                    switch (e.Name) {
                        case "surname":
                            surnameLst.Add(e.InnerText);
                            break;
                        case "man":
                            manLst.Add(e.InnerText);
                            break;
                        case "woman":
                            womanLst.Add(e.InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public string GetRDNameData(bool man = true) {
        string rdName = surnameLst[PETools.RDint(0, surnameLst.Count - 1)];
        if (man) {
            rdName += manLst[PETools.RDint(0, manLst.Count - 1)];
        }
        else {
            rdName += womanLst[PETools.RDint(0, womanLst.Count - 1)];
        }
        return rdName;
    }
    #endregion

    #region 地图
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg(string path) {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                MapCfg mc = new MapCfg() { ID = ID, monsterLst = new List<MonsterData>(), };

                foreach (XmlElement e in nodList[i].ChildNodes) {
                    switch (e.Name) {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "power":
                            mc.power = int.Parse(e.InnerText);
                            break;
                        case "mainCamPos":
                            mc.mainCamPos = StrToVec3(e.InnerText);
                            break;
                        case "mainCamRote":
                            mc.mainCamRote = StrToVec3(e.InnerText);
                            break;
                        case "playerBornPos":
                            mc.playerBornPos = StrToVec3(e.InnerText);
                            break;
                        case "playerBornRote":
                            mc.playerBornRote = StrToVec3(e.InnerText);
                            break;
                        case "monsterLst": {
                                string[] valArr = e.InnerText.Trim().Split('#');
                                for (int waveindex = 0; waveindex < valArr.Length; waveindex++) {
                                    if (waveindex == 0) {
                                        continue;
                                    }
                                    string[] tempArr = valArr[waveindex].Split('|');
                                    for (int j = 0; j < tempArr.Length; j++) {
                                        if (j == 0) {
                                            continue;
                                        }
                                        string[] arr = tempArr[j].Split(',');
                                        int id = int.Parse(arr[0]);
                                        MonsterData md = new MonsterData {
                                            ID = id,
                                            mWave = waveindex,
                                            mIndex = j,
                                            mCfg = GetMonsterCfgData(id),
                                            mBornPos = new Vector3(float.Parse(arr[1]), float.Parse(arr[2]), float.Parse(arr[3])),
                                            mBornRote = new Vector3(0, float.Parse(arr[4]), 0),
                                            mLevel = int.Parse(arr[5]),
                                        };
                                        mc.monsterLst.Add(md);
                                    }
                                }
                            }
                            break;
                        case "exp":
                            mc.exp = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            mc.coin = int.Parse(e.InnerText);
                            break;
                        case "crystal":
                            mc.crystal = int.Parse(e.InnerText);
                            break;
                    }
                }
                mapCfgDataDic.Add(ID, mc);
            }
        }
    }

    public MapCfg GetMapCfgData(int id) {
        MapCfg data;
        if (mapCfgDataDic.TryGetValue(id, out data)) {
            return data;
        }
        return null;
    }
    #endregion

    #region 怪物
    private Dictionary<int, MonsterCfg> monsterDic = new Dictionary<int, MonsterCfg>();
    private void InitMonsterCfg(string path) {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                MonsterCfg mc = new MonsterCfg() {
                    ID = ID,
                    bps = new BattleProps(),
                };

                foreach (XmlElement e in nodList[i].ChildNodes) {
                    switch (e.Name) {
                        case "mName":
                            mc.mName = e.InnerText;
                            break;
                        case "mType":
                            mc.mType = (MonsterType)int.Parse(e.InnerText);
                            break;
                        case "isStop":
                            mc.isStop = e.InnerText.Equals("1");
                            break;
                        case "resPath":
                            mc.resPath = e.InnerText;
                            break;
                        case "skillID":
                            mc.skillID = int.Parse(e.InnerText);
                            break;
                        case "atkDis":
                            mc.atkDis = float.Parse(e.InnerText);
                            break;
                        case "hp":
                            mc.bps.hp = int.Parse(e.InnerText);
                            break;
                        case "ad":
                            mc.bps.ad = int.Parse(e.InnerText);
                            break;
                        case "ap":
                            mc.bps.ap = int.Parse(e.InnerText);
                            break;
                        case "addef":
                            mc.bps.addef = int.Parse(e.InnerText);
                            break;
                        case "apdef":
                            mc.bps.apdef = int.Parse(e.InnerText);
                            break;
                        case "dodge":
                            mc.bps.dodge = int.Parse(e.InnerText);
                            break;
                        case "pierce":
                            mc.bps.pierce = int.Parse(e.InnerText);
                            break;
                        case "critical":
                            mc.bps.critical = int.Parse(e.InnerText);
                            break;
                    }
                }
                monsterDic.Add(ID, mc);
            }
        }
    }

    public MonsterCfg GetMonsterCfgData(int id) {
        MonsterCfg data;
        if (monsterDic.TryGetValue(id, out data)) {
            return data;
        }
        return null;
    }
    #endregion

    #region 强化升级配置
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg(string path) {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                StrongCfg sd = new StrongCfg() { ID = ID };

                foreach (XmlElement e in nodList[i].ChildNodes) {
                    int val = int.Parse(e.InnerText);
                    switch (e.Name) {
                        case "pos":
                            sd.pos = val;
                            break;
                        case "starlv":
                            sd.starlv = val;
                            break;
                        case "addhp":
                            sd.addhp = val;
                            break;
                        case "addhurt":
                            sd.addhurt = val;
                            break;
                        case "adddef":
                            sd.adddef = val;
                            break;
                        case "minlv":
                            sd.minlv = val;
                            break;
                        case "coin":
                            sd.coin = val;
                            break;
                        case "crystal":
                            sd.crystal = val;
                            break;
                    }
                }

                Dictionary<int, StrongCfg> dic = null;
                if (strongDic.TryGetValue(sd.pos, out dic)) {
                    dic.Add(sd.starlv, sd);
                }
                else {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(sd.starlv, sd);
                    strongDic.Add(sd.pos, dic);
                }
            }
        }
    }

    public StrongCfg GetStrongCfg(int pos, int startlv) {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;

        if (strongDic.TryGetValue(pos, out dic)) {
            if (dic.ContainsKey(startlv)) {
                sd = dic[startlv];
            }
        }
        return sd;
    }

    public int GetPropAddValPreLv(int pos, int starlv, int type) {

        Dictionary<int, StrongCfg> posDic = null;
        int val = 0;
        if (strongDic.TryGetValue(pos, out posDic)) {
            for (int i = 0; i < starlv; i++) {
                StrongCfg sd;
                if (posDic.TryGetValue(i, out sd)) {
                    switch (type) {
                        case 1:
                            val += sd.addhp;
                            break;
                        case 2:
                            val += sd.addhurt;
                            break;
                        case 3:
                            val += sd.adddef;
                            break;
                    }
                }
            }
        }
        return val;
    }
    #endregion

    #region 任务奖励配置
    private Dictionary<int, TaskRewardCfg> taskRewardCfgDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardCfg(string path) {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                TaskRewardCfg rec = new TaskRewardCfg() { ID = ID };

                foreach (XmlElement e in nodList[i].ChildNodes) {
                    switch (e.Name) {
                        case "taskName":
                            rec.taskName = e.InnerText;
                            break;
                        case "count":
                            rec.count = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            rec.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            rec.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                taskRewardCfgDic.Add(ID, rec);
            }
        }
    }

    public TaskRewardCfg GetTaskRewardCfg(int id) {
        TaskRewardCfg trc = null;
        if (taskRewardCfgDic.TryGetValue(id, out trc)) {
            return trc;
        }
        return null;
    }
    #endregion

    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path) {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                AutoGuideCfg agc = new AutoGuideCfg() { ID = ID };

                foreach (XmlElement e in nodList[i].ChildNodes) {
                    switch (e.Name) {
                        case "npcID":
                            agc.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            agc.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            agc.actID = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            agc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            agc.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                guideTaskDic.Add(ID, agc);
            }
        }
    }

    public AutoGuideCfg GetAutoGuideCfg(int id) {
        AutoGuideCfg agc = null;
        if (guideTaskDic.TryGetValue(id, out agc)) {
            return agc;
        }
        return null;
    }
    #endregion

    #region 技能信息配置
    private Dictionary<int, SkillCfg> skillDic = new Dictionary<int, SkillCfg>();
    private void InitSkillCfg(string path) {
        skillDic.Clear();
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                SkillCfg sc = new SkillCfg() {
                    ID = ID,
                    skillMoveLst = new List<int>(),
                    skillActionLst = new List<int>(),
                    skillDamageLst = new List<int>(),
                };

                foreach (XmlElement e in nodList[i].ChildNodes) {
                    switch (e.Name) {
                        case "skillName":
                            sc.skillName = e.InnerText;
                            break;
                        case "cdTime":
                            sc.cdTime = int.Parse(e.InnerText);
                            break;
                        case "skillTime":
                            sc.skillTime = int.Parse(e.InnerText);
                            break;
                        case "aniAction":
                            sc.aniAction = int.Parse(e.InnerText);
                            break;
                        case "fx":
                            sc.fx = e.InnerText;
                            break;
                        case "isCombo":
                            sc.isCombo = e.InnerText.Equals("1");
                            break;
                        case "isCollide":
                            sc.isCollide = e.InnerText.Equals("1");
                            break;
                        case "isBreak":
                            sc.isBreak = e.InnerText.Equals("1");
                            break;
                        case "dmgType":
                            sc.dmgType = (DamageType)int.Parse(e.InnerText);
                            break;
                        case "skillMoveLst":
                            sc.skillMoveLst = GetSplitList<int>(e.InnerText, '|');
                            break;
                        case "skillActionLst":
                            sc.skillActionLst = GetSplitList<int>(e.InnerText, '|');
                            break;
                        case "skillDamageLst":
                            sc.skillDamageLst = GetSplitList<int>(e.InnerText, '|');
                            break;
                    }
                }
                skillDic.Add(ID, sc);
            }
        }
    }

    public SkillCfg GetSkillCfg(int id) {
        SkillCfg sc = null;
        if (skillDic.TryGetValue(id, out sc)) {
            return sc;
        }
        return null;
    }
    #endregion

    #region 技能移动配置
    private Dictionary<int, SkillMoveCfg> skillMoveDic = new Dictionary<int, SkillMoveCfg>();
    private void InitSkillMoveCfg(string path) {
        skillMoveDic.Clear();
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                SkillMoveCfg smc = new SkillMoveCfg() { ID = ID };

                foreach (XmlElement e in nodList[i].ChildNodes) {
                    switch (e.Name) {
                        case "delayTime":
                            smc.delayTime = int.Parse(e.InnerText);
                            break;
                        case "moveTime":
                            smc.moveTime = int.Parse(e.InnerText);
                            break;
                        case "moveDis":
                            smc.moveDis = float.Parse(e.InnerText);
                            break;
                    }
                }
                skillMoveDic.Add(ID, smc);
            }
        }
    }

    public SkillMoveCfg GetSkillMoveCfg(int id) {
        SkillMoveCfg smc = null;
        if (skillMoveDic.TryGetValue(id, out smc)) {
            return smc;
        }
        return null;
    }
    #endregion

    #region 技能伤害范围配置
    private Dictionary<int, SkillActionCfg> skillActionDic = new Dictionary<int, SkillActionCfg>();
    private void InitSkillActionCfg(string path) {
        skillActionDic.Clear();
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList)) {
            for (int i = 0; i < nodList.Count; i++) {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                SkillActionCfg sac = new SkillActionCfg() { ID = ID };

                foreach (XmlElement e in nodList[i].ChildNodes) {
                    switch (e.Name) {
                        case "delayTime":
                            sac.delayTime = int.Parse(e.InnerText);
                            break;
                        case "radius":
                            sac.radius = float.Parse(e.InnerText);
                            break;
                        case "angle":
                            sac.angle = int.Parse(e.InnerText);
                            break;
                    }
                }
                skillActionDic.Add(ID, sac);
            }
        }
    }

    public SkillActionCfg GetSkillActionCfg(int id) {
        SkillActionCfg sac = null;
        if (skillActionDic.TryGetValue(id, out sac)) {
            return sac;
        }
        return null;
    }
    #endregion

    #endregion

    #region 字符串转化 
    public static Vector3 StrToVec3(string source) {
        if (source.Length <= 0)
            return Vector3.zero;

        string[] tmp_sValues = source.Trim(' ').Split(',');
        if (tmp_sValues != null && tmp_sValues.Length == 3) {
            float tmp_fX = float.Parse(tmp_sValues[0]);
            float tmp_fY = float.Parse(tmp_sValues[1]);
            float tmp_fZ = float.Parse(tmp_sValues[2]);

            return new Vector3(tmp_fX, tmp_fY, tmp_fZ);
        }
        return Vector3.zero;
    }

    public static List<T> GetSplitList<T>(string source, char cha) {
        var lst = source.Trim().Split(cha);
        List<T> retlst = new List<T>();
        foreach (var item in lst) {

            T localVal = default(T);
            localVal = (T)Convert.ChangeType(item, typeof(T));

            retlst.Add(localVal);
        }

        return retlst;
    }
    #endregion
}