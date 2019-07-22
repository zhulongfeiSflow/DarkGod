/****************************************************
	文件：MapMgr.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 1:59   	
	功能：地图管理器
*****************************************************/

using UnityEngine;

public class MapMgr : MonoBehaviour
{
    private int waveIndex = 1;//默认第一批怪物
    private BattleMgr battleMgr;

    public void Init(BattleMgr bm) {
        battleMgr = bm;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        //实例化第一批怪物
        battleMgr.LoadMonsterByWaveID(waveIndex);

        PECommon.Log("Init MapMgr Done.");
    }
}
