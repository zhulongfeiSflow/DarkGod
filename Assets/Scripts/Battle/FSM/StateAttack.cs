/****************************************************
	文件：StateAttack.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 16:43   	
	功能：攻击状态
*****************************************************/


public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Attack;
        PECommon.Log("Enter Attack");
    }

    public void Exit(EntityBase entity, params object[] args) {
        entity.canControl = true;
        entity.SetAction(Constants.ActionDefault);
        PECommon.Log("Exit Attack");
    }

    public void Process(EntityBase entity, params object[] args) {
        entity.AttackEffect((int)args[0]);
        PECommon.Log("Process Attack");
    }
}

