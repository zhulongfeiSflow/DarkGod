/****************************************************
	文件：BaseData.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/14 17:42   	
	功能：配置数据类
*****************************************************/
using UnityEngine;

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID;//触发任务目标NPC索引号
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
}


public class BaseData<T>
{
    public int ID;
}