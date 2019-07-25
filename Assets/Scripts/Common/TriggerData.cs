/****************************************************
    文件：TriggerData.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/25 8:31:23
	功能：地图触发数据类
*****************************************************/

using UnityEngine;

public class TriggerData : MonoBehaviour
{
    public int triggleWave;
    public MapMgr mapMgr;

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (mapMgr != null) {
                mapMgr.TriggleMonsterBorn(this, triggleWave);
            }
        }
    }
}