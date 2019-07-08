/****************************************************
    文件：SystemRoot.cs
	作者：朱龙飞
    邮箱: 398670608@qq.com
    日期：2019/7/7 4:18:51
	功能：业务系统基类
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour 
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetSvc netSvc = null;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
    }
}