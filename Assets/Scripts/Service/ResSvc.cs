/****************************************************
    文件：ResSvc.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/6 10:40:53
	功能：资源加载服务
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;

    public void InitSvc()
    {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        PECommon.Log("Init ResSvc...");
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState(true);

        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);

        prgCB = () =>
        {
            float val = sceneAsync.progress;
            GameRoot.Instance.loadingWnd.SetProgress(val);
            if (val == 1)
            {
                if (loaded != null)
                {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
    }

    private void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDic.Add(path, au);
            }
        }
        return au;
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if (prefab != null)
        {
            go = Instantiate(prefab);
        }
        return go;
    }

    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    #region InitCfgs
    private bool TryGetRootNodeList(string path, out XmlNodeList nodList)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + " not exist!", LogType.Error);
        }
        else
        {
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
    private void InitRDNameCfg(string path)
    {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList))
        {
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
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

    public string GetRDNameData(bool man = true)
    {
        System.Random rd = new System.Random();
        string rdName = surnameLst[PETools.RDint(0, surnameLst.Count - 1)];
        if (man)
        {
            rdName += manLst[PETools.RDint(0, manLst.Count - 1)];
        }
        else
        {
            rdName += womanLst[PETools.RDint(0, womanLst.Count - 1)];
        }
        return rdName;
    }
    #endregion

    #region 地图
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg(string path)
    {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList))
        {
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                MapCfg mc = new MapCfg() { ID = ID };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "mainCamPos":
                            mc.mainCamPos = GetVec3ByString(e.InnerText);
                            break;
                        case "mainCamRote":
                            mc.mainCamRote = GetVec3ByString(e.InnerText);
                            break;
                        case "playerBornPos":
                            mc.playerBornPos = GetVec3ByString(e.InnerText);
                            break;
                        case "playerBornRote":
                            mc.playerBornRote = GetVec3ByString(e.InnerText);
                            break;
                    }
                }
                mapCfgDataDic.Add(ID, mc);
            }
        }
    }

    public MapCfg GetMapCfgData(int id)
    {
        MapCfg data;
        if (mapCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path)
    {
        XmlNodeList nodList = null;
        if (TryGetRootNodeList(path, out nodList))
        {
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                AutoGuideCfg agc = new AutoGuideCfg() { ID = ID };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
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

    public AutoGuideCfg GetAutoGuideData(int id)
    {
        AutoGuideCfg agc = null;
        if (guideTaskDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion

    #endregion


    #region 字符串转化 
    public static Vector3 GetVec3ByString(string p_sVec3)
    {
        if (p_sVec3.Length <= 0)
            return Vector3.zero;

        string[] tmp_sValues = p_sVec3.Trim(' ').Split(',');
        if (tmp_sValues != null && tmp_sValues.Length == 3)
        {
            float tmp_fX = float.Parse(tmp_sValues[0]);
            float tmp_fY = float.Parse(tmp_sValues[1]);
            float tmp_fZ = float.Parse(tmp_sValues[2]);

            return new Vector3(tmp_fX, tmp_fY, tmp_fZ);
        }
        return Vector3.zero;
    }
    #endregion
}