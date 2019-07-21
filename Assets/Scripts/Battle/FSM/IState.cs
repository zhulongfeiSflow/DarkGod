/****************************************************
	文件：IState.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 12:14   	
	功能：状态接口
*****************************************************/
public interface IState
{
    void Enter(EntityBase entity, params object[] args);
    void Process(EntityBase entity, params object[] args);
    void Exit(EntityBase entity, params object[] args);
}

public enum AniState
{
    None,
    Idle,
    Move,
    Attack,
}