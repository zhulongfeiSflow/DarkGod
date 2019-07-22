/****************************************************
	文件：Controller.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 12:47   	
	功能：表现实体控制器抽象基类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Animator ani;
    public CharacterController ctrl;

    protected bool isMove = false;
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir {
        get {
            return dir;
        }
        set {
            if (value == Vector2.zero) {
                isMove = false;
            }
            else {
                isMove = true;
            }
            dir = value;
        }
    }

    protected bool skillMove = false;
    protected float skillMoveSpeed = 0f;

    protected TimerSvc TimerSvc;
    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();

    public virtual void Init() {
        TimerSvc = TimerSvc.Instance;
    }

    public virtual void SetBlend(float blend) {
        ani.SetFloat("Blend", blend);
    }

    public virtual void SetAction(int act) {
        ani.SetInteger("Action", act);
    }

    public virtual void SetFX(string name, float destroy) {

    }

    public void SetSkillMoveState(bool move, float skillSpeed = 0f
        ) {
        skillMove = move;
        skillMoveSpeed = skillSpeed;
    }
}