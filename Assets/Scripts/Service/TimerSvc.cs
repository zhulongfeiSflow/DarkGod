/****************************************************
	文件：TimerSvc.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/19 10:25   	
	功能：计时服务
*****************************************************/

using System;

public class TimerSvc : SystemRoot
{
    public static TimerSvc Instance = null;

    private PETimer pt = null;
    public void InitSvc()
    {
        Instance = this;
        pt = new PETimer();
        //设置日志输出
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });

        PECommon.Log("Init TimerSvc...");
    }

    private void Update()
    {
        pt.Update();
    }

    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }

    public double GetNowTime() {
        return pt.GetMillisecondsTime();
    }

    public void DelTask(int tid) {
        pt.DeleteTimeTask(tid);
    }

}
