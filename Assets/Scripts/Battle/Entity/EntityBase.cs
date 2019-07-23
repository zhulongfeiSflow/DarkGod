/****************************************************
	文件：EntityBase.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 12:15   	
	功能：逻辑实体的基类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase
{
    public AniState currentAniState = AniState.None;

    public BattleMgr battleMgr = null;
    public StateMgr stateMgr = null;
    public SkillMgr skillMgr = null;
    protected Controller controller = null;
    private string name;
    public string Name {
        get {
            return name;
        }

        set {
            name = value;
        }
    }

    public bool canControl = true;

    private BattleProps props;
    public BattleProps Props {
        get {
            return props;
        }

        protected set {
            props = value;
        }
    }

    private int hp;
    public int HP {
        get {
            return hp;
        }

        set {
            //通知UI血量变化
            PECommon.Log("hp change:" + hp + "to" + value);
            SetHPVal(hp, value);
            hp = value;
        }
    }

    public Queue<int> comboQue = new Queue<int>();
    public int nextSkillID = 0;

    public SkillCfg curtSkillCfg;

    public void Born() {
        stateMgr.ChangeStatus(this, AniState.Born, null);
    }

    public void Move() {
        stateMgr.ChangeStatus(this, AniState.Move, null);
    }

    public void Idle() {
        stateMgr.ChangeStatus(this, AniState.Idle, null);
    }

    public void Attack(int skillID) {
        stateMgr.ChangeStatus(this, AniState.Attack, skillID);
    }

    public void Hit() {
        stateMgr.ChangeStatus(this, AniState.Hit, null);
    }

    public void Die() {
        stateMgr.ChangeStatus(this, AniState.Die, null);
    }

    public void SetCtrl(Controller ctrl) {
        controller = ctrl;
    }

    public void SetActive(bool active = true) {
        if (controller != null) {
            controller.gameObject.SetActive(active);
        }
    }

    public virtual void SetBattleProps(BattleProps props) {
        HP = props.hp;
        Props = props;
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

    public virtual void SetAtkRotation(Vector2 dir, bool offset = false) {
        if (controller != null) {
            if (offset) {
                controller.SetAtkRotationCam(dir);
            }
            else {
                controller.SetAtkRotationLocal(dir);
            }
        }
    }

    public virtual void SetDodge() {
        if (controller != null) {
            GameRoot.Instance.dynamicWnd.SetDodge(name);
        }
    }

    public virtual void SetCritical(int critical) {
        if (controller != null) {
            GameRoot.Instance.dynamicWnd.SetCritical(name, critical);
        }
    }

    public virtual void SetHurt(int hurt) {
        if (controller != null) {
            GameRoot.Instance.dynamicWnd.SetHurt(name, hurt);
        }
    }

    public virtual void SetHPVal(int oldVal, int newVal) {
        if (controller != null) {
            GameRoot.Instance.dynamicWnd.SetHPVal(name, oldVal, newVal);
        }
    }

    public virtual void SkillAttack(int skillID) {
        skillMgr.SkillAttack(this, skillID);
    }

    public virtual Vector2 GetDirInput() {
        return Vector2.zero;
    }

    public virtual Vector3 GetPos() {
        return controller.transform.position;
    }

    public virtual Transform GetTrans() {
        return controller.transform;
    }

    public AnimationClip[] GetAniClips() {
        if (controller != null) {
            return controller.ani.runtimeAnimatorController.animationClips;
        }
        return null;
    }

    public virtual Vector2 CalcTargetDir() {
        return Vector2.zero;
    }

    public void ExitCurtSkill() {
        canControl = true;
        //连招判定
        if (curtSkillCfg.isCombo) {
            if (comboQue.Count > 0) {
                nextSkillID = comboQue.Dequeue();
            }
            else {
                nextSkillID = 0;
            }
        }

        SetAction(Constants.ActionDefault);
    }
}