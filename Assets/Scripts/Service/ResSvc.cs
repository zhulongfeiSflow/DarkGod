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
        InitRDNameCfg();

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

    #region InitCfgs
    private List<string> surnameLst = new List<string>();
    private List<string> manLst = new List<string>();
    private List<string> womanLst = new List<string>();
    private void InitRDNameCfg()
    {
        TextAsset xml = Resources.Load<TextAsset>(PathDefine.RDNameCfg);
        if (!xml)
        {
            PECommon.Log("xml file:" + PathDefine.RDNameCfg + " not exist!", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;

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
}