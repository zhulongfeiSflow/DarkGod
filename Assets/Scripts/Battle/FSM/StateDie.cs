/****************************************************
	文件：StateDie.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/22 21:25   	
	功能：死亡状态
*****************************************************/

public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Die;
        PECommon.Log("Enter Die");
    }

    public void Exit(EntityBase entity, params object[] args) {
        PECommon.Log("Exit Die");
    }

    public void Process(EntityBase entity, params object[] args) {
        //播放死亡动画
        entity.SetAction(Constants.ActionDie);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            //死亡后取消激活
            entity.controller.gameObject.SetActive(false);
        }, Constants.DieAniLenght);
        PECommon.Log("Process Die");
    }
}
