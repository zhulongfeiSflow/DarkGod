/****************************************************
	文件：EntityPlayer.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 12:35   	
	功能：玩家逻辑实体类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityBase
{
    public override Vector2 GetDirInput() {
        return battleMgr.GetDirInput();
    }

    public override Vector2 CalcTargetDir() {
        EntityMonster monster = FindClosedTarget();
        if (monster != null) {
            var target = monster.GetPos();
            var self = GetPos();
            var dir = new Vector2(target.x - self.x, target.z - self.z);
            return dir.normalized;
        }
        else {
            return Vector2.zero;
        }
    }

    private EntityMonster FindClosedTarget() {
        List<EntityMonster> Lst = battleMgr.GetEntityMonster();
        if (Lst == null || Lst.Count == 0) {
            return null;
        }
        Vector3 self = GetPos();
        EntityMonster targetMonster = null;
        float dis = float.MaxValue;
        for (int i = 0; i < Lst.Count; i++) {
            var target = Lst[i].GetPos();
            var disTemp = Vector3.Distance(self, target);
            if (dis > disTemp) {
                dis = disTemp;
                targetMonster = Lst[i];
            }
        }

        return targetMonster;
    }
}