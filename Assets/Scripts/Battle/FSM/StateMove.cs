/****************************************************
	文件：StateMove.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 12:19   	
	功能：移动状态
*****************************************************/

public class StateMove : IState
{
    public void Enter(EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Move;
        //PECommon.Log("Enter move");
    }

    public void Exit(EntityBase entity, params object[] args) {
        //PECommon.Log("Exit move");
    }

    public void Process(EntityBase entity, params object[] args) {
        //PECommon.Log("Process move");
        entity.SetBlend(Constants.BlendMove);
    }
}