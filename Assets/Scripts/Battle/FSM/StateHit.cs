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
    }

    public void Exit(EntityBase entity, params object[] args) {
    }

    public void Process(EntityBase entity, params object[] args) {
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.ActionHit);

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
            if (clipName.Contains("hit") || clipName.Contains("Hit") || clipName.Contains("HIT")) {
                return clips[i].length;
            }
        }
        //保护值
        return 1;
    }
}

