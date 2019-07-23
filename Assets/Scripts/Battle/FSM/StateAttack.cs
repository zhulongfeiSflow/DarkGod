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
        entity.curtSkillCfg = ResSvc.Instance.GetSkillCfg((int)args[0]);
        //PECommon.Log("Enter Attack");
    }

    public void Exit(EntityBase entity, params object[] args) {
        entity.ExitCurtSkill();
        //PECommon.Log("Exit Attack");
    }

    public void Process(EntityBase entity, params object[] args) {
        entity.SkillAttack((int)args[0]);
        //PECommon.Log("Process Attack");
    }
}

