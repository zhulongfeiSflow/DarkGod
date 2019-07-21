/****************************************************
	文件：EntityPlayer.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/21 12:35   	
	功能：玩家逻辑实体类
*****************************************************/

using UnityEngine;

public class EntityPlayer : EntityBase
{
    public override Vector2 GetDirInput() {
        return battleMgr.GetDirInput();
    }
}