/****************************************************
	文件：SkillMgr.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 1:58   	
	功能：技能管理器
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    private ResSvc resSvc;
    private TimerSvc timerSvc;

    public void Init() {
        resSvc = ResSvc.Instance;
        timerSvc = TimerSvc.Instance;
        PECommon.Log("Init SkillMgr Done.");
    }

    /// <summary>
    /// 技能效果表现
    /// </summary>
    public void AttackEffect(EntityBase entity, int skillID) {
        SkillCfg skillData = resSvc.GetSkillCfg(skillID);

        entity.SetAction(skillData.aniAction);
        entity.SetFX(skillData.fx, skillData.skillTime);

        CalcSkillMove(entity, skillData);

        entity.canControl = false;
        entity.SetDir(Vector2.zero);

        timerSvc.AddTimeTask((int tid) =>
        {
            entity.Idle();

        }, skillData.skillTime);
    }

    private void CalcSkillMove(EntityBase entity, SkillCfg skillData) {
        List<string> skillMoveLst = skillData.skillMoveLst;
        int sum = 0;
        //多段位移
        for (int i = 0; i < skillMoveLst.Count; i++) {
            SkillMoveCfg skillMoveCfg = resSvc.GetSkillMoveCfg(int.Parse(skillMoveLst[i]));

            float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);
            sum += skillMoveCfg.delayTime;
            //if (sum > 0) {
            //    timerSvc.AddTimeTask((int tid) =>
            //    {
            //        entity.SetSkillMoveState(true, speed);
            //    }, sum);
            //}
            //else {
            //    entity.SetSkillMoveState(true, speed);
            //}
            timerSvc.AddTimeTask((int tid) =>
            {
                entity.SetSkillMoveState(true, speed);
            }, sum);

            sum += skillMoveCfg.moveTime;
            timerSvc.AddTimeTask((int tid) =>
            {
                entity.SetSkillMoveState(false);

            }, sum);
        }
    }
}
