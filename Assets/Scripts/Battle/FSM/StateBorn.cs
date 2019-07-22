/****************************************************
	文件：StateBorn.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/22 19:38   	
	功能：出生状态
*****************************************************/

public class StateBorn : IState
{
    public void Enter(EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Born;
    }

    public void Exit(EntityBase entity, params object[] args) {
    }

    public void Process(EntityBase entity, params object[] args) {
        //播放出生动画
        entity.SetAction(Constants.ActionBorn);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constants.ActionDefault);
        }, 500);
    }
}