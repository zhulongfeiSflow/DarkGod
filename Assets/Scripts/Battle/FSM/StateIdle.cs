/****************************************************
	文件：StateIdle.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 12:17   	
	功能：待机状态
*****************************************************/

using UnityEngine;

public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
        PECommon.Log("Enter idle");
    }

    public void Exit(EntityBase entity, params object[] args) {
        PECommon.Log("Exit idle");
    }

    public void Process(EntityBase entity, params object[] args) {
        if (entity.GetDirInput() != Vector2.zero) {
            entity.Move();
            entity.SetDir(entity.GetDirInput());
        }
        else {
            entity.SetBlend(Constants.BlendIdle);
        }
        PECommon.Log("Process idle");
    }
}
