/****************************************************
	文件：StateHit.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/22 23:46   	
	功能：受击状态
*****************************************************/

using UnityEngine;

public class StateHit : IState
{
    public void Enter(EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Hit;
        entity.SetDir(Vector2.zero);
        entity.SetSkillMoveState(false);

        foreach (var tid in entity.skMoveCBLst) {
            TimerSvc.Instance.DelTask(tid);
        }

        foreach (var tid in entity.skActCBLst) {
            TimerSvc.Instance.DelTask(tid);
        }

        entity.SKClear();

        //攻击被中断,要删除定时回调
        if (entity.skEndCB != -1) {
            TimerSvc.Instance.DelTask(entity.skEndCB);
            entity.skEndCB = -1;
        }

        //清空连招
        if (entity.nextSkillID != 0 || entity.comboQue.Count > 0) {
            entity.nextSkillID = 0;
            entity.comboQue.Clear();

            entity.battleMgr.lastAtkTime = 0;
            entity.battleMgr.comboIndex = 0;
        }
    }

    public void Exit(EntityBase entity, params object[] args) {
    }

    public void Process(EntityBase entity, params object[] args) {
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.ActionHit);

        //受击音效
        if (entity.entityType == EntityType.Player) {
            AudioSource charAudio = entity.GetAudio();
            AudioSvc.Instance.PlayCharAudio(Constants.Assassin_Hit, charAudio);
        }

        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constants.ActionDefault);
            entity.Idle();
        }, (int)(GetHitAniLen(entity) * 1000));
    }

    private float GetHitAniLen(EntityBase entity) {
        AnimationClip[] clips = entity.GetAniClips();
        for (int i = 0; i < clips.Length; i++) {
            string clipName = clips[i].name;
            if (clipName.Contains("hit") ||
                clipName.Contains("Hit") ||
                clipName.Contains("HIT")) {
                return clips[i].length;
            }
        }
        //保护值
        return 1;
    }
}

