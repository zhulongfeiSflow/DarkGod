/****************************************************
	文件：MonsterController.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/22 14:20   	
	功能：怪物表现实体角色控制器类
*****************************************************/

using UnityEngine;

public class MonsterController : Controller
{
    public override void Init() {
        base.Init();
    }

    private void Update() {
        //AI逻辑
        if (isMove) {
            SetDir();
            SetMove();

        }
        else {

        }
    }

    private void SetDir() {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove() {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.MonsterMoveSpeed);
        //给一个向下的速度,便于在没有Apply root时怪物可以落地,修复老资源bug
        ctrl.Move(Vector3.down * Time.deltaTime * Constants.MonsterMoveSpeed);
    }



}
