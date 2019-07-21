/****************************************************
	文件：EntityBase.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 12:15   	
	功能：逻辑实体的基类
*****************************************************/

using UnityEngine;

public abstract class EntityBase
{
    public AniState currentAniState = AniState.None;

    public BattleMgr battleMgr = null;
    public StateMgr stateMgr = null;
    public SkillMgr skillMgr = null;
    public Controller controller = null;

    public bool canControl = true;

    public void Move() {
        stateMgr.ChangeStatus(this, AniState.Move, null);
    }

    public void Idle() {
        stateMgr.ChangeStatus(this, AniState.Idle, null);
    }

    public void Attack(int skillID) {
        stateMgr.ChangeStatus(this, AniState.Attack, skillID);
    }

    public virtual void SetBlend(float blend) {
        if (controller != null) {
            controller.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir) {
        if (controller != null) {
            controller.Dir = dir;
        }
    }

    public virtual void SetAction(int act) {
        if (controller != null) {
            controller.SetAction(act);
        }
    }

    public virtual void SetFX(string name, float destroy) {
        if (controller != null) {
            controller.SetFX(name, destroy);
        }
    }

    public virtual void SetSkillMoveState(bool move, float speed = 0f) {
        if (controller != null) {
            controller.SetSkillMoveState(move, speed);
        }
    }

    public virtual void AttackEffect(int skillID) {
        skillMgr.AttackEffect(this, skillID);
    }

    public virtual Vector2 GetDirInput() {
        return Vector2.zero;
    }

}