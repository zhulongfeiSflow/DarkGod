/****************************************************
	文件：StateMgr.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 1:52   	
	功能：状态管理器
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class StateMgr : MonoBehaviour
{
    private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();

    public void Init() {
        fsm.Add(AniState.Born, new StateBorn());
        fsm.Add(AniState.Idle, new StateIdle());
        fsm.Add(AniState.Move, new StateMove());
        fsm.Add(AniState.Attack, new StateAttack());
        fsm.Add(AniState.Hit, new StateHit());
        fsm.Add(AniState.Die, new StateDie());

        PECommon.Log("Init StateMgr Done.");
    }

    public void ChangeStatus(EntityBase entity, AniState targetState, params object[] args) {
        if (entity.currentAniState == targetState) {
            return;
        }

        if (fsm.ContainsKey(targetState)) {
            if (entity.currentAniState != AniState.None) {
                fsm[entity.currentAniState].Exit(entity, args);
            }
            fsm[targetState].Enter(entity, args);
            fsm[targetState].Process(entity, args);
        }
    }
}